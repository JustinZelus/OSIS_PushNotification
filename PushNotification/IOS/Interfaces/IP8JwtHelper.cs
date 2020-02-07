using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.Interfaces
{
    public interface IP8JwtHelper: IP8EncryptJwt
    {
        bool ValidateJwtTime();
        void RefreshJwt();
        void CreateJwt();
    }
}
