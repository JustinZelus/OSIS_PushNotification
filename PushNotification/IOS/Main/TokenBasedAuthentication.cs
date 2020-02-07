using PushNotification.IOS.Base;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Others;
using PushNotification.IOS.P8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PushNotification.IOS.Main
{
    /// <summary>
    /// 功能:
    /// 檢查Token有無過期
    /// 產出Token
    /// </summary>
    public class TokenBasedAuthentication : BaseTokenBasedAuthentication
    {
        private IP8JwtHelper p8JwtHelper;

        public TokenBasedAuthentication(string p8key, string p8keyID, string teamID) : base(p8key, p8keyID,teamID)
        {
            if (p8JwtHelper == null)
            {
                //P8JwtHelper應該由外部傳進來
                p8JwtHelper = new P8JwtHelper(P8key, P8keyID, TeamID);
            }
            System.Collections.ICollection d;
        }

        protected override void CreateJwt()
        {
            if (p8JwtHelper != null)
            {
                p8JwtHelper.CreateJwt();
            }
        }

        protected override void RefreshJwt()
        {
            if (p8JwtHelper == null) return ;
            p8JwtHelper.RefreshJwt();
        }

        protected override bool ValidateJwtTime()
        {
            if (p8JwtHelper == null) return false;
            return p8JwtHelper.ValidateJwtTime();
        }

        public override string GetJwt()
        {
            if (!ValidateJwtTime())
                RefreshJwt();

            if (p8JwtHelper.GetJwt() == null)
                CreateJwt();

            return p8JwtHelper.GetJwt();
        }

        //TODO:要驗證是不是UtcNow EPOCH的time
        private bool IsValidTimeFormat(string input)
        {
            return TimeSpan.TryParse(input, out var dummyOutput);
        }
    }
}
