using PushNotification.IOS.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PushNotification.IOS.Base
{
    //照官方文件設計出來的抽象類
    abstract class BaseTokenBasedAuthentication : BasedAuthentication, ITokenBasedAuthentication, IP8EncryptJwt
    {
        public string P8key { get; set; }
        public string P8keyID { get; set; }

        protected abstract void CreateJwt();
        protected abstract bool ValidateJwtTime();
        protected abstract void RefreshJwt();

        public BaseTokenBasedAuthentication(string p8key, string p8keyID, string teamID) : base(teamID)
        {
            P8key = p8key;
            P8keyID = p8keyID;
            if (string.IsNullOrEmpty(P8key) || string.IsNullOrEmpty(P8keyID)) 
                throw new Exception("p8key,p8keyID can't be null");

        }
        public abstract string GetJwt();
        
    }
}
