using PushNotification.IOS.Enums;
using PushNotification.IOS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.IOS.Interfaces
{
    interface IAPNs
    {
        Task<APNsResponse> SendAsync(object notification,
                                   string deviceToken,
                                   APNsServer envType,
                                   string apnsId = null,
                                   int apnsExpiration = 0,
                                   int apnsPriority = 10,
                                   bool isBackground = false);


        //TODO:測試用
        
    }
}
