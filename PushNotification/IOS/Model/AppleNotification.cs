using Newtonsoft.Json;
using PushNotification.IOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Model
{
    public class AppleNotification
    {
        public class ApsPayload: IApsPayload
        {
            [JsonProperty("alert")]
            public IAlertBody AlertBody { get; set; }

            [JsonProperty("badge")]
            public int Badge { get; set; }

            [JsonProperty("sound")]
            public string Sound { get; set; } = "default";

            [JsonProperty("category")]
            public string Category { get; set; } = "0"; //ios對應enum

            [JsonProperty("mutable-content")]
            public int MutableContent { get; set; } //手機收到推播前預先處裡機制，暫時不用
        }

        [JsonProperty("aps")]
        public IApsPayload Aps { get; set; }

        public int NotificationSN { get; set; }
        public string CreateDate { get; set; }
        public string EncryptSN { get; set; }
        public string Link { get; set; }

    }

    public class AlertBody: IAlertBody
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty("body")]
        public string Content { get; set; }
    }


}
