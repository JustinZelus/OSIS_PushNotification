using Newtonsoft.Json;
using PushNotification.IOS.Base;
using PushNotification.IOS.Model;
using PushNotification.IOS.Others;
using System;

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace PushNotification.IOS.P8
{
    class P8PushTokenBuilder
    {
      
        private string _p8key;
        private string _p8keyID;
        private string _teamID;
        private int _validTime;

        public P8PushTokenBuilder SetP8Key(string key)
        {
            _p8key = key;
            return this;
        }

        public P8PushTokenBuilder SetP8KeyID(string keyID)
        {
            _p8keyID = keyID;
            return this;
        }

        public P8PushTokenBuilder SetP8TeamID(string teamID)
        {
            _teamID = teamID;
            return this;
        }

        public P8PushTokenBuilder SetValidTime(int time)
        {
            _validTime = time;
            return this;
        }

        public IP8PushToken Build()
        {
            if (!Check())
            {
                Debug.WriteLine("IP8PushToken Check fail");
                return null;
            } 

            var jwt = "";
            //var validTime = _validTime == 0 ? Tools.CreateEpochTime() : _validTime;

            var header = JsonConvert.SerializeObject(new { alg = "ES256", kid = _p8keyID });
            var payload = JsonConvert.SerializeObject(new { iss = _teamID, iat = _validTime });

            var privateKeyFile = Convert.FromBase64String(_p8key);
            var privateKey = CngKey.Import(privateKeyFile, CngKeyBlobFormat.Pkcs8PrivateBlob);
            using (var dsa = new ECDsaCng(privateKey))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
                var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
                var unsignedJwtData = $"{headerBase64}.{payloadBase64}";
                var signature = Convert.ToBase64String(dsa.SignData(Encoding.UTF8.GetBytes(unsignedJwtData)));                
                jwt = $"{unsignedJwtData}.{signature}";
                Debug.WriteLine("P8Jwt generate succeed.");
            }

            return new P8PushToken(jwt, _validTime);
        }


        private bool Check()
        {
            return !(string.IsNullOrEmpty(_p8key) || string.IsNullOrEmpty(_p8keyID) || string.IsNullOrEmpty(_teamID)
                || _validTime == 0);
        }
    }
}
