using PushNotification.Android.Main;
using PushNotification.Common.Interfaces;
using PushNotification.Common.Model;
using PushNotification.IOS.Main;
using PushNotification.IOS.Model;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace PushNotification.Common
{
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
            var gTypes = this.GetType().GetGenericArguments();
            var gInstance = (TDada)Activator.CreateInstance(gTypes[0]);

            _execTimer.Interval = 100;

            switch (gInstance.GetDataType())
            {
                case DataType.Android:
                    _execTimer.Elapsed += new ElapsedEventHandler(SendAndroidProcess);

                    break;
                case DataType.IOS:
                    _execTimer.Elapsed += new ElapsedEventHandler(SendIOSProcess);

                    break;
                case DataType.Web:
                    _execTimer.Elapsed += new ElapsedEventHandler(SendWebProcess);

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


        private async void SendIOSProcess(object sender, ElapsedEventArgs e)
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


        private async void SendAndroidProcess(object sender, ElapsedEventArgs e)
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
                    var androidData = result as AndroidPushData;
                    //Android推播
                    if (result == null)
                        return;

                    if (_service == null)
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(
                                _ => { throw new Exception("Exception on timer."); });
                    }

                    AndroidPushNotificationService service = _service as AndroidPushNotificationService;
                    //  GCMResponse gCMResponse = await service.PushAsync(iosData.DeviceToken, iosData.Notification);
                    //  Debug.WriteLine("GCMResponse: ", gCMResponse);
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

        private async void SendWebProcess(object sender, ElapsedEventArgs e)
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
                    var webData = result as WebPushData;
                    if (result == null)
                        return;

                    if (_service == null)
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(
                                _ => { throw new Exception("Exception on timer."); });
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
}
