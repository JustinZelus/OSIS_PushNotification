using Newtonsoft.Json;
using PushNotification.IOS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Interfaces
{
    public interface IApsPayload
    {
        [JsonProperty("alert")]
        public IAlertBody AlertBody { get; set; }

        [JsonProperty("badge")]
        public int Badge { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
