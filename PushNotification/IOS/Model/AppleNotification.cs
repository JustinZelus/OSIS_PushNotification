using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Model
{
    class AppleNotification
    {
        public class ApsPayload
        {
            [JsonProperty("alert")]
            public AlertBody AlertBody { get; set; }

            [JsonProperty("badge")]
            public int Badge { get; set; }

            [JsonProperty("sound")]
            public string Sound { get; set; }
        }



        [JsonProperty("aps")]
        public ApsPayload Aps { get; set; }
    }

    class AlertBody
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Content { get; set; }
    }
}
