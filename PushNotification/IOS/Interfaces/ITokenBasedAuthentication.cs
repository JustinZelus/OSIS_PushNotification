using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Interfaces
{
    interface IP8EncryptJwt
    {
        string GetJwt();
    }
    interface ITokenBasedAuthentication
    {
        string P8key { get; set; }
        string P8keyID { get; set; }
    }
}
