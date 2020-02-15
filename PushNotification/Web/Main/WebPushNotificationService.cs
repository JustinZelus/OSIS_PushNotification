using PushNotification.Common.Interfaces;
using PushNotification.Web.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.Web.Main
{
    public class WebPushNotificationService: IPushNotificationService
    {
        public async Task<WebResponse> PushAsync()
        {
            return null;
        }
    }
}
