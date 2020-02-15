using PushNotification.Common.Interfaces;
using PushNotification.IOS.Enums;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.IOS.Main
{

    public class IOSPushNotificationService: IPushNotificationService
    {
        private ITokenBasedAuthentication jwtProvider;
        private IAPNs aPNsSender;

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
            if (string.IsNullOrEmpty(appBundleID))
                throw new Exception("appBundleID can't be null");
            
            jwtProvider = new TokenBasedAuthentication(p8key, p8keyID, teamID);
            aPNsSender = new APNsSender(appBundleID);
        }

        public async Task<APNsResponse> PushAsync(string deviceToken, object notification)
        {
            if (aPNsSender == null) return null;

            string jwt = jwtProvider.GetJwt();
            return await aPNsSender.SendAsync(jwt, notification, deviceToken, _envType);
        }


        public IOSPushNotificationService Build()
        {
            jwtProvider = new TokenBasedAuthentication(_p8key, _p8keyID, _teamID);
            aPNsSender = new APNsSender(_appBundleID);
            return this;
        }
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

        public static IOSPushNotificationService Builder()
        {
            return new IOSPushNotificationService();
        }
      
    }
}
