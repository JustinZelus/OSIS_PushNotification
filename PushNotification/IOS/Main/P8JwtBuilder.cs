using Newtonsoft.Json;
using PushNotification.IOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace PushNotification.IOS.Main
{
    public class P8JwtBuilder
    {
        private string _p8key;
        private string _p8keyID;
        private string _teamID;
        private int _validTime;
        private string _jwt;

        public string JWT { get => _jwt; }

        public P8JwtBuilder Build(int time = 0)
        {
            if (!Check())
            {
                Debug.WriteLine("IP8PushToken Check fail");
                return null;
            }

            if (time > 0)
                _validTime = time;

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
                 _jwt = $"{unsignedJwtData}.{signature}";
                Debug.WriteLine("P8Jwt generate succeed.");
                return this;
            }
        }

        public P8JwtBuilder SetP8Key(string key)
        {
            _p8key = key;
            return this;
        }

        public P8JwtBuilder SetP8KeyID(string keyID)
        {
            _p8keyID = keyID;
            return this;
        }

        public P8JwtBuilder SetP8TeamID(string teamID)
        {
            _teamID = teamID;
            return this;
        }

        public P8JwtBuilder SetValidTime(int time)
        {
            _validTime = time;
            return this;
        }

        private bool Check()
        {
            return !(string.IsNullOrEmpty(_p8key) || string.IsNullOrEmpty(_p8keyID) || string.IsNullOrEmpty(_teamID));
        }
    }
}
