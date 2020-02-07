using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Model
{
    public class P8PushToken : IP8PushToken
    {
        public string Jwt { get; set; }
        public int ValidTime { get; set; }
        public P8PushToken(string jwt,int time)
        {
            Jwt = jwt;
            ValidTime = time;
        }

       
    }
}
