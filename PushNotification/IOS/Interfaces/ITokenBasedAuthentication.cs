using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Interfaces
{
    public interface IP8EncryptJwt
    {
        string GetJwt();
    }
    public interface ITokenBasedAuthentication
    {
        string P8key { get; set; }
        string P8keyID { get; set; }
    }
}
