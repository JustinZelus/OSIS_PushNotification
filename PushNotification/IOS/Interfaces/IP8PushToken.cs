using PushNotification.IOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS
{
    interface IP8PushToken
    {
        string Jwt { get; set; }
        int ValidTime { get; set; }
    }
}
