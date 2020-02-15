using PushNotification.IOS.Main;
using PushNotification.IOS.Model;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace PushNotification.Common
{ 
    public class PushNotificationService
    {
        private static PushNotificationService _instance { get; set; } = null;
        public PushNotificationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PushNotificationService();
                }
                return _instance;
            }

        }

        public PushNotificationService()
        {
            _execTimer.Interval = 100;
            _execTimer.Elapsed += new ElapsedEventHandler(SendProcess);
            _execTimer.Enabled = true;
            _execTimer.Start();
        }
        
        private static ConcurrentQueue<Dictionary<string, AppleNotification>> vip_iosPushDataSet = new ConcurrentQueue<Dictionary<string, AppleNotification>>();
        private static ConcurrentQueue<Dictionary<string, AppleNotification>> iosPushDataSet = new ConcurrentQueue<Dictionary<string, AppleNotification>>();




        private Timer _execTimer = new Timer();
        public Timer ExecTimer
        {
            get
            {
                if (_execTimer == null)
                    _execTimer = new Timer();
                return _execTimer;
            }

        }
        private IOSPushNotificationService _iOSPushService;


        public void SetIOSPushNotificationService(IOSPushNotificationService iOSPushService)
        {
            if (iOSPushService != null)
                _iOSPushService = iOSPushService;
        }


        public void PushIOS(Dictionary<string, AppleNotification> data)
        {
            EnqueueData(data);
        }

        private void SendProcess(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            try
            {

                if (iosPushDataSet.IsEmpty && vip_iosPushDataSet.IsEmpty)
                    return;
                else
                    timer.Enabled = false;

                Dictionary<string, AppleNotification> result;

                if (vip_iosPushDataSet.TryDequeue(out result) || iosPushDataSet.TryDequeue(out result))
                {
                    //ios推播
                    if (result != null && _iOSPushService != null)
                    //if (result != null && result.key == "iOS")
                    {
                        //timer.Enabled = false;
                        //do something
                        var iosData = result.First();
                        if (iosData.Key != null && iosData.Value != null)
                        {
                            var task = _iOSPushService.PushAsync(iosData.Value, iosData.Key);
                            task.Wait();

                            APNsResponse aPNsResponse = task.Result;
                            Debug.WriteLine("APNsResponse: " + aPNsResponse);
                        }

                    }
                    //Android推播
                    else
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendProcess Exception:", ex);
            }
            finally
            {
                timer.Enabled = true;

            }
        }

        private void EnqueueData(Dictionary<string, AppleNotification> data)
        {
            if (iosPushDataSet != null && data != null && data.Count > 0)
            {
                foreach (KeyValuePair<string, AppleNotification> item in data)
                {
                    var dictionary = new Dictionary<string, AppleNotification>
                    {
                       {
                          item.Key, item.Value
                       }

                    };

                    iosPushDataSet.Enqueue(dictionary);
                }
            }

        }


    }
}
