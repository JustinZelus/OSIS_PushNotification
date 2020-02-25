
using PushNotification.Android.Model;
using PushNotification.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.Common.Model
{
    public class AndroidPushData : IData
    {
        public string DeviceToken { get; set; }
        public AndroidNotification Notification { get; set; }

        public DataType GetDataType()
        {
            return DataType.Android;
        }

    }
}
