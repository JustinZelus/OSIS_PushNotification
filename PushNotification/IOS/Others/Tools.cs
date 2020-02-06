using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Others
{
    public static class Tools
    {
        public static int CreateEpochTime()
        {
            return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
