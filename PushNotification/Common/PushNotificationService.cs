
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

        private PushNotificationServiceProvider<IOSPushData> iosServiceProvier { get; set; }
        private PushNotificationServiceProvider<AndroidPushData> androidServiceProvier { get; set; }
        private PushNotificationServiceProvider<WebPushData> webServiceProvier { get; set; }
        public PushNotificationService()
        {
            iosServiceProvier = new PushNotificationServiceProvider<IOSPushData>();
            //androidServiceProvier = new PushNotificationServiceProvider<AndroidPushData>();
            //webServiceProvier = new PushNotificationServiceProvider<WebPushData>();
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
    }
}
