using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Others;
using System;


namespace PushNotification.IOS.Base
{
    //照官方文件設計出來的抽象類
    //public abstract class BaseTokenBasedAuthentication : BasedAuthentication, ITokenBasedAuthentication, IP8EncryptJwt
    public abstract class BaseTokenBasedAuthentication : BasedAuthentication, ITokenBasedAuthentication
    {
        protected int timeInterval = 2400; //sec.

        public string P8key { get; set; }
        public string P8keyID { get; set; }

        protected string _jwt;
        protected int _jwtTime;

        protected abstract void CreateJwt();
        protected virtual bool ValidateJwtTime()
        {
            if (_jwtTime == 0) return false;
            var now = Tools.CreateEpochTime();
            bool result = (now - _jwtTime) <= timeInterval;
            return result;
        }
        protected abstract void RefreshJwt();

        public BaseTokenBasedAuthentication(string p8key, string p8keyID, string teamID) : base(teamID)
        {
            P8key = p8key;
            P8keyID = p8keyID;
            if (string.IsNullOrEmpty(P8key) || string.IsNullOrEmpty(P8keyID)) 
                throw new Exception("p8key,p8keyID can't be null");

        }
        public virtual string GetJwt()
        {
            if (string.IsNullOrEmpty(_jwt))
                CreateJwt();
            if (!ValidateJwtTime())
                RefreshJwt();
            if (!string.IsNullOrEmpty(_jwt))
                return _jwt;
            return null;
        }
        
    }
}
