
using PushNotification.Android.Main;
using PushNotification.Android.Model;
using PushNotification.Common.Interfaces;
using PushNotification.Common.Model;
using PushNotification.IOS.Main;
using PushNotification.IOS.Model;
using PushNotification.Web.Main;
using PushNotification.Web.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace PushNotification.Common
{
    public enum DataType
    {
        Android, IOS, Web
    }

    public interface IData
    {
        DataType GetDataType();
    }

    public class PushNotificationServiceProvider<TDada> where TDada : IData
    {

        private IPushNotificationService _service = null;
        public IPushNotificationService Service
        {
            set { _service = value; }
        }


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
        private ConcurrentQueue<IData> _dataSet = new ConcurrentQueue<IData>();
        

        public PushNotificationServiceProvider()
        {
            var gTypes =   this.GetType().GetGenericArguments();
            var gInstance = (TDada)Activator.CreateInstance(gTypes[0]);
            _execTimer.Interval = 100;
            switch (gInstance.GetDataType())
            {
                case DataType.Android:
                    _execTimer.Elapsed += new ElapsedEventHandler(SendProcess2);
                    
                    break;
                case DataType.IOS:
                    _execTimer.Elapsed += new ElapsedEventHandler(SendProcess1);
                   
                    break;
                case DataType.Web:
                    _execTimer.Elapsed += new ElapsedEventHandler(SendProcess3);

                    break;
                default:
                    throw new Exception();
            }
            _execTimer.Enabled = true;
            _execTimer.Start();
        }

        public void EnqueueData(IData data)
        {
            _dataSet.Enqueue(data);
        }


        private async void SendProcess1(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            try
            {

                if (_dataSet.IsEmpty)
                    return;
                else
                    timer.Enabled = false;
                if (_dataSet.TryDequeue(out IData result))
                {
                    //ios推播
                    var iosData = result as IOSPushData;

                    if (result == null)
                        return;

                    if (_service == null)
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(
                                _ => { throw new Exception("Exception on timer."); });
                    }
                        

                    IOSPushNotificationService service = _service as IOSPushNotificationService;
                    APNsResponse aPNsResponse = await service.PushAsync(iosData.DeviceToken, iosData.Notification);
                    Debug.WriteLine("APNsResponse: ", aPNsResponse);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendProcess1 Exception:", ex);
            }
            finally
            {
                timer.Enabled = true;

            }
        }



        private async void SendProcess2(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            try
            {

                if (_androidDataSet.IsEmpty)
                    return;
                else
                    timer.Enabled = false;
                if (_androidDataSet.TryDequeue(out AndroidPushData result))
                {
                    //Android推播
                    if (result != null && result.GetType().Equals("IOS"))
                    {
                        GCMResponse gCMResponse = await _androidPushService.PushAsync();
                        Debug.WriteLine("GCMResponse: ", gCMResponse);
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendProcess2 Exception:", ex);
            }
            finally
            {
                timer.Enabled = true;

            }
        }

        private async void SendProcess3(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            try
            {

                if (_webDataSet.IsEmpty)
                    return;
                else
                    timer.Enabled = false;
                if (_webDataSet.TryDequeue(out WebPushData result))
                {
                    //web推播
                    if (result != null && result.GetType().Equals("IOS"))
                    {
                        WebResponse webResponse = await _webPushService.PushAsync();
                        Debug.WriteLine("WebResponse: ", webResponse);
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendProcess3 Exception:", ex);
            }
            finally
            {
                timer.Enabled = true;

            }
        }

    }



    public class PushNotificationService2
    {
        private static PushNotificationService2 _instance { get; set; } = null;
        public PushNotificationService2 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PushNotificationService2();
                }
                return _instance;
            }

        }

        //private Timer _execTimer1 = new Timer();
        //public Timer ExecTimer1
        //{
        //    get
        //    {
        //        if (_execTimer1 == null)
        //            _execTimer1 = new Timer();
        //        return _execTimer1;
        //    }

        //}

        //private Timer _execTimer2 = new Timer();
        //public Timer ExecTimer2
        //{
        //    get
        //    {
        //        if (_execTimer2 == null)
        //            _execTimer2 = new Timer();
        //        return _execTimer2;
        //    }

        //}

        //private Timer _execTimer3 = new Timer();
        //public Timer ExecTimer3
        //{
        //    get
        //    {
        //        if (_execTimer3 == null)
        //            _execTimer3 = new Timer();
        //        return _execTimer3;
        //    }

        //}


        private PushNotificationServiceProvider<IOSPushData> iosServiceProvier { get; set; }
        private PushNotificationServiceProvider<AndroidPushData> androidServiceProvier { get; set; }
        private PushNotificationServiceProvider<WebPushData> webServiceProvier { get; set; }
        public PushNotificationService2()
        {
            iosServiceProvier = new PushNotificationServiceProvider<IOSPushData>();
            //androidService = new PushNotificationServiceProvider<AndroidPushData>();

        //    _execTimer1.Interval = 100;
        //    _execTimer1.Elapsed += new ElapsedEventHandler(SendProcess1);
        //    _execTimer1.Enabled = true;
        //    _execTimer1.Start();

            //    _execTimer2.Interval = 100;
            //    _execTimer2.Elapsed += new ElapsedEventHandler(SendProcess2);
            //    _execTimer2.Enabled = true;
            //    _execTimer2.Start();

            //    _execTimer3.Interval = 100;
            //    _execTimer3.Elapsed += new ElapsedEventHandler(SendProcess3);
            //    _execTimer3.Enabled = true;
            //    _execTimer3.Start();


        }
        public void EnqueueData(IData data)
        {
            if (data is IOSPushData)
                iosServiceProvier.EnqueueData(data);
            if (data is  AndroidPushData)
                androidServiceProvier.EnqueueData(data);
            if (data is WebPushData)
                webServiceProvier.EnqueueData(data);
        }

        private void SetService(IPushNotificationService service)
        {
            if (service is IOSPushNotificationService)
                iosServiceProvier.Service = service;

            if (service is AndroidPushNotificationService)
                androidServiceProvier.Service = service;

            if (service is WebPushNotificationService)
                webServiceProvier.Service = service;
        }


        //private static ConcurrentQueue<Dictionary<string, IPushData>> _vip_PushDataSet = new ConcurrentQueue<Dictionary<string, AppleNotification>>();
        //private static ConcurrentQueue<IOSPushData> _iosDataSet = new ConcurrentQueue<IOSPushData>();
        //private static ConcurrentQueue<AndroidPushData> _androidDataSet = new ConcurrentQueue<AndroidPushData>();
        //private static ConcurrentQueue<WebPushData> _webDataSet = new ConcurrentQueue<WebPushData>();


        private PushData _notificationData = new PushData();
        public PushData NotificationData 
        {
            get => _notificationData;
        }


        public IOSPushNotificationService IOSPushService 
        {
            set => SetService(value);
        }

        public AndroidPushNotificationService AndroidPushService
        {
            set => SetService(value);
        }

        private WebPushNotificationService WebPushService
        {
            set => SetService(value);
        }


        public void Run()
        {
            foreach (var iosData in NotificationData.IOS)
            {
                EnqueueData(iosData);
            }

            foreach (var androidData in NotificationData.Android)
            {
                EnqueueData(androidData);
            }

            foreach (var webData in NotificationData.Web)
            {
                EnqueueData(webData);
            }
        }


        //private async void SendProcess1(object sender, ElapsedEventArgs e)
        //{
        //    Timer timer = (Timer)sender;
        //    try
        //    {

        //        if (_iosDataSet.IsEmpty)
        //            return;
        //        else
        //            timer.Enabled = false;
        //        if (_iosDataSet.TryDequeue(out IOSPushData result))
        //        {
        //            //ios推播
        //            if (result != null && result.GetType().Equals("IOS"))
        //            {
        //                APNsResponse aPNsResponse = await _iOSPushService.PushAsync(result.DeviceToken, result.Notification);
        //                Debug.WriteLine("APNsResponse: " , aPNsResponse);
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("SendProcess1 Exception:", ex);
        //    }
        //    finally
        //    {
        //        timer.Enabled = true;

        //    }
        //}

        //private async void SendProcess2(object sender, ElapsedEventArgs e)
        //{
        //    Timer timer = (Timer)sender;
        //    try
        //    {

        //        if (_androidDataSet.IsEmpty)
        //            return;
        //        else
        //            timer.Enabled = false;
        //        if (_androidDataSet.TryDequeue(out AndroidPushData result))
        //        {
        //            //Android推播
        //            if (result != null && result.GetType().Equals("IOS"))
        //            {
        //                GCMResponse gCMResponse = await _androidPushService.PushAsync();
        //                Debug.WriteLine("GCMResponse: ", gCMResponse);
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("SendProcess2 Exception:", ex);
        //    }
        //    finally
        //    {
        //        timer.Enabled = true;

        //    }
        //}

        //private async void SendProcess3(object sender, ElapsedEventArgs e)
        //{
        //    Timer timer = (Timer)sender;
        //    try
        //    {

        //        if (_webDataSet.IsEmpty)
        //            return;
        //        else
        //            timer.Enabled = false;
        //        if (_webDataSet.TryDequeue(out WebPushData result))
        //        {
        //            //web推播
        //            if (result != null && result.GetType().Equals("IOS"))
        //            {
        //                WebResponse webResponse = await _webPushService.PushAsync();
        //                Debug.WriteLine("WebResponse: ", webResponse);
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("SendProcess3 Exception:", ex);
        //    }
        //    finally
        //    {
        //        timer.Enabled = true;

        //    }
        //}

        //private void EnqueueData1(IOSPushData data)
        //{
        //    if (_iosDataSet == null)
        //        return;

        //    if(!_iosDataSet.Contains(data))
        //        _iosDataSet.Enqueue(data);
        //}

        //private void EnqueueData2(AndroidPushData data)
        //{
        //    if (_androidDataSet == null)
        //        return;

        //    if (!_androidDataSet.Contains(data))
        //        _androidDataSet.Enqueue(data);
        //}

        //private void EnqueueData3(WebPushData data)
        //{
        //    if (_webDataSet == null)
        //        return;

        //    if (!_webDataSet.Contains(data))
        //        _webDataSet.Enqueue(data);
        //}
    }
}
