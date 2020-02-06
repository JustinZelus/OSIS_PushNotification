using PushNotification.IOS.Enums;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.IOS.Main
{
    class IOSPushNotificationService
    {
        IP8EncryptJwt tokenProvider;
        IAPNs aPNsSender;
        private string _p8key;
        private string _p8keyID;
        private string _teamID;
        private string _appBundleID;
        private APNsServer _envType = APNsServer.Development; //預設開發環境

        public IOSPushNotificationService()
        {

        }

        public IOSPushNotificationService(string p8key, string p8keyID, string teamID, string appBundleID)
        {
            tokenProvider = new TokenBasedAuthentication(p8key, p8keyID, teamID);
            var jwt = tokenProvider.GetJwt();

            if (!string.IsNullOrEmpty(jwt))
                aPNsSender = new APNsSender(jwt, appBundleID);
        }

        public async Task<APNsResponse> PushAsync(object notification, string deviceToken)
        {
            if (aPNsSender == null) return null;
            return await aPNsSender.SendAsync(notification, deviceToken, _envType);
        }

        //public async Task<APNsResponse> Push2(object notification, string deviceToken)
        //{
        //    if (aPNsSender == null) return null;
        //    return aPNsSender.SendAsync(notification, deviceToken, _envType).Result;
        //}

        public IOSPushNotificationService SetP8key(string key)
        {
            _p8key = key;
            return this;
        }
        public IOSPushNotificationService SetP8keyID(string keyID)
        {
            _p8keyID = keyID;
            return this;
        }
        public IOSPushNotificationService SetP8TeamID(string teamID)
        {
            _teamID = teamID;
            return this;
        }

        public IOSPushNotificationService SetAppBundleID(string appBundleID)
        {
            _appBundleID = appBundleID;
            return this;
        }

        public IOSPushNotificationService SetAPNsServer(APNsServer envType)
        {
            _envType = envType;
            return this;
        }

        public static IOSPushNotificationService Builder() {
                return new IOSPushNotificationService();
        }

        public IOSPushNotificationService Build()
        {
            //產生JWT
            tokenProvider = new TokenBasedAuthentication(_p8key, _p8keyID, _teamID);
            var jwt = tokenProvider.GetJwt();
            //初始化推播類
            if (!string.IsNullOrEmpty(jwt))
                aPNsSender = new APNsSender(jwt, _appBundleID);
            return this;
        }

    }
}
