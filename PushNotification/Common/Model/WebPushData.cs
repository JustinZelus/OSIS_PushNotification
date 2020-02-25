using PushNotification.Common.Interfaces;
using PushNotification.Web.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.Common.Model
{
    public class WebPushData : IData
    {
        public string DeviceToken { get; set; }
        public WebNotification Notification { get; set; }
        public DataType GetDataType()
        {
            return DataType.Web;
        }
    }
}
