using PushNotification.IOS.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PushNotification.IOS.P8
{
    public class P8JwtHelper : BaseP8JwtHelper
    {
        IP8PushToken p8PushToken;

        public P8JwtHelper(string p8key, string p8keyID, string teamID) : base(p8key, p8keyID, teamID)
        {
            base.SearchJwtProcedure(Constants.iosFolder, Constants.APNS_FILE);
        }

        public override void CreateJwt()
        {
            if (P8Key == null || P8KeyID == null || TeamID == null) return;

            p8PushToken = new P8PushTokenBuilder()
                                    .SetP8Key(P8Key)
                                    .SetP8KeyID(P8KeyID)
                                    .SetP8TeamID(TeamID)
                                    .SetValidTime(Tools.CreateEpochTime())
                                    .Build();
            if (p8PushToken != null)
            {
                Jwt = p8PushToken.Jwt;
                ValidTime = p8PushToken.ValidTime.ToString();
            }
        }

        public override void RefreshJwt()
        {
            CreateJwt();
            
            //檢查預設路徑bin資料夾底下(AppDomain)
            if(CheckFileExists(Constants.APNS_FILE,Constants.iosFolder))
                WriteLines(GetRootPath(), Constants.APNS_FILE,GetLines());
        }

        public override bool ValidateJwtTime()
        {
            var isValid = false;

            if (ValidTime == null)
                return isValid;

            var now = Tools.CreateEpochTime();
             
            if (int.TryParse(ValidTime, out int result))
                if (now - result <= 1800) //3500可自訂
                {
                    isValid = true;
                }
            return isValid;
        }

        protected override string[] GetLines()
        {
            string[] lines = {Constants.KEY_WORD + ":" + Jwt ,
                                  Constants.VALID_TIME_WORD + ":" + ValidTime};
            return lines;
        }
    }
}
