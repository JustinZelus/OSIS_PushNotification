using PushNotification.Android.Model;
using PushNotification.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.Android.Main
{
    public class AndroidPushNotificationService: IPushNotificationService
    {
        public AndroidPushNotificationService()
        {

        }
        public async Task<GCMResponse> PushAsync()
        {
            return null;
        }
    }
}
