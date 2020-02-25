using PushNotification.Common.Interfaces;
using PushNotification.IOS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.Common.Model
{
    public class IOSPushData : IData
    {

        public string DeviceToken { get; set; }
        public AppleNotification Notification { get; set; }

        public DataType GetDataType()
        {
            return DataType.IOS;
        }
    }
}
