using PushNotification.IOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PushNotification.IOS.Base
{
    public abstract class BasedAuthentication : IEstablishConnection
    {
        public string TeamID { get; set; }
        public BasedAuthentication(string teamID)
        {
            this.TeamID = teamID;
            if (string.IsNullOrEmpty(teamID)) Debug.WriteLine("TeamID can't be null or empty");
        }
    }
}
