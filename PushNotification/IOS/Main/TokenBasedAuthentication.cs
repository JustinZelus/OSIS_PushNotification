using PushNotification.IOS.Base;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Others;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PushNotification.IOS.Main
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class TokenBasedAuthentication : BaseTokenBasedAuthentication
    {
        //  private IP8JwtHelper p8JwtHelper;
        IJwtProvider jwtProvider;

        public TokenBasedAuthentication(string p8key, string p8keyID, string teamID) : base(p8key, p8keyID,teamID)
        {
            if (jwtProvider == null)
                jwtProvider = new JwtProvider(P8key, P8keyID, TeamID);
        }

        protected override void CreateJwt()
        {
            if (jwtProvider != null)
            {
                _jwt = jwtProvider.CreateJwt();
                _jwtTime = jwtProvider.ValidTime;
            }
            
            
        }

        protected override void RefreshJwt()
        {
            if (jwtProvider != null)
            {
                _jwt = jwtProvider.RefreshJwt();
                _jwtTime = jwtProvider.ValidTime;
            }
        }

        //protected override bool ValidateJwtTime()
        //{
        //    if (jwtProvider == null) return false;
        //    return jwtProvider.ValidateJwtTime();
        //}

        //public override string GetJwt()
        //{
        //    if (!ValidateJwtTime())
        //        RefreshJwt();

        //    if (jwtProvider.GetJwt() == null)
        //        CreateJwt();

        //    return jwtProvider.GetJwt();
        //}

        //TODO:要驗證是不是UtcNow EPOCH的time
        private bool IsValidTimeFormat(string input)
        {
            return TimeSpan.TryParse(input, out var dummyOutput);
        }
    }

    public interface IJwtProvider
    {
        public int ValidTime { get; }
        string CreateJwt();
        string RefreshJwt();
    }

    public class JwtProvider : IJwtProvider
    {
        private int _validTime;
        public int ValidTime { get => _validTime; }

        private P8JwtBuilder p8JwtBuilder;

        public JwtProvider(string p8key, string p8keyID, string teamID)
        {
            _validTime = CreateEpochTime();
            if (p8JwtBuilder == null)
            {
                p8JwtBuilder = new P8JwtBuilder()
                                       .SetP8Key(p8key)
                                       .SetP8KeyID(p8keyID)
                                       .SetP8TeamID(teamID)
                                       .SetValidTime(_validTime)
                                       .Build();
            }
        }

        public string CreateJwt()
        {
            if (string.IsNullOrEmpty(p8JwtBuilder.JWT))
            {
                if (p8JwtBuilder != null)
                    p8JwtBuilder.Build();
            }
            return p8JwtBuilder.JWT;
        }

        public string RefreshJwt()
        {
            if (p8JwtBuilder != null)
            {
                _validTime = CreateEpochTime();
                p8JwtBuilder.Build(_validTime);
            }

            return p8JwtBuilder.JWT;
        }
        private static int CreateEpochTime()
        {
            return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
