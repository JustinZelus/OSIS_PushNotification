using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Model
{
    public class AppleNotification
    {
        public class ApsPayload
        {
            [JsonProperty("alert")]
            public AlertBody AlertBody { get; set; }

            [JsonProperty("badge")]
            public int Badge { get; set; }

            [JsonProperty("sound")]
            public string Sound { get; set; }

            [JsonProperty("category")]
            public string Category { get; set; }
        }

        [JsonProperty("aps")]
        public ApsPayload Aps { get; set; }


        public class DetailPlayload
        {
            public int SN { get; set; }
        }

        public DetailPlayload Detail { get; set; }

    }

    public class AlertBody
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty("body")]
        public string Content { get; set; }
    }
}
