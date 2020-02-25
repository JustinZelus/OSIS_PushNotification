using Newtonsoft.Json;
using PushNotification.Android.Main;
using PushNotification.Android.Model;
using PushNotification.Common;
using PushNotification.Common.Interfaces;
using PushNotification.Common.Model;
using PushNotification.IOS.Enums;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Main;
using PushNotification.IOS.Model;
using PushNotification.IOS.Others;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static PushNotification.IOS.Model.AppleNotification;

namespace ConsoleApp1
{
    
    /// <summary>
    /// IOS推播使用範例
    /// 
    /// apple推播有兩種:Token-Based 或 Certificate
    /// 這邊走token-based，所需如下:
    /// 
    /// App BundleID : 手機app專案的BundleID 
    ///    xxxxxx.p8 : P8檔案，Apple後台申請後下載
    ///      P8KeyID : Apple後台下載p8檔時會給
    ///       TeamID : Apple後台查
    ///   APNsServer : 欲推播的環境    
    ///  Devicetoken : 手機的token 
    ///          JWT : 由P8Key,P8KeyID,TeamID,EpochTime ，經過Sha256加密後生成
    /// 
    /// 前置步驟:
    /// 1. 在專案底下的bin\Debug\netcoreapp3.1\ 路徑下建Apple資料夾
    /// 2. 裡面放兩個檔案: 一個從apple後台下載的xxxx.p8，一個自已建的p8.txt
    /// 3. p8.txt裡面放兩行data如下:
    ///      KeyID:你自己的KeyID 
    ///      TeamID:你自己的TeamID 
    /// 4. 以上步驟完成後，將Apple名稱資料夾的路徑給P8FileHelper類處理即可
    ///      
    /// </summary>
    class Program
    {
        //欲推播的手機token
        static string devicetoken1 = "109729e72fbc91d29c17e397a0be40b745b7b205269a82347d4076da0c7638dc"; //justin
        static string devicetoken2 = "8c45caf058b3660914bb751cfd98e5bae53fb7e8602e3bd589acb455762454f4"; //Ruru
        static string devicetoken3 = "aa69d0ac8d50ceca782747e107347ef72140918688a72a1b62b70ae1869da88f"; //羿伻
        //
        //static string appBundleID = "com.hamastar.intelligence.cityservice";
        //推播的類
       // static IOSPushNotificationService iOSPushService;

        static string deviceType = "IOS";

        async static Task Main(string[] args)
        {
            //ios service
            var iOSPushService = IOSPushNotificationService.Builder()
                                .SetP8key("MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgNdbDhCgPQOjJ9BQF21Q+IWXvyU/54swqOC5m0jkEvEKgCgYIKoZIzj0DAQehRANCAAR5PatQ2QHIVloZFOe7mwife1MW6fHg3BwRg0az3L6hUrfUY1YXwXGK86gTTUqlSGxnFuCoh/lJ8S/qLaXEC4aE")
                                .SetP8keyID("RRY854U4N4")
                                .SetP8TeamID("BK76F98DY7")
                                //.SetP8key(p8FileHelper.GetP8Key()) 
                                //.SetP8keyID(p8FileHelper.GeP8KeyID())
                                //.SetP8TeamID(p8FileHelper.GetTeamID()) 
                                .SetAppBundleID("com.hamastar.intelligence.cityservice") 
                                .SetAPNsServer(APNsServer.Development) 
                                .Build();

            //ios notification
            var notification = new IOSNotificationBuilder()
                                    .SetTitle("標頭2")
                                    .SetContent("一般訊息2")
                                    .SetCategory("R")
                                    .SetBadge(1)
                                    .SetNotificationSN(86)
                                    .SetCreateDate("2020-02-15 16:58:27")
                                    .SetLink("https://osis.hamastar.com.tw/ICS_Application/CheckMailandPhone/0/5")
                                    .SetEncryptSN("asdasd222")
                                    .Build();

            var jsonObj = JsonConvert.SerializeObject(notification);
            Debug.WriteLine("ios notification: ",jsonObj);

           
            ///////////////
          

            //android service
            var androidPushService = new AndroidPushNotificationService();
            //android notification
            //...
            //.... todo ...


            PushNotificationService pushNotificationService = new PushNotificationService().Instance;
            
           // pushNotificationService.AndroidPushService = androidPushService;
           if(iOSPushService != null)
                pushNotificationService.IOSPushService = iOSPushService;

            List<string> deviceTokens = new List<string>();
         //   deviceTokens.Add(devicetoken1);
            deviceTokens.Add(devicetoken1);

            foreach (var token in deviceTokens)
            {
                //TODO: 用工廠模式創建Data。
                //給property，若工廠判斷符合，則產出對應的data

                IData data = null;
                if (deviceType.Equals("IOS"))
                {
                    data = new IOSPushData
                    {
                        DeviceToken = token,
                        Notification = notification
                    };
                }
                else if (deviceType.Equals("Android"))
                {
                    data = new AndroidPushData
                    {
                        DeviceToken = token,
                        Notification = new AndroidNotification()
                    };
                }
                pushNotificationService.EnqueueData(data);
            }

            //for (int i = 0; i < deviceTokens.Count; i++)
            //{
            //    IData data = null;
            //    if (deviceType.Equals("IOS"))
            //    {
            //        data = new IOSPushData
            //        {
            //            DeviceToken = devicetoken1,
            //            Notification = notification
            //        };
            //    }
            //    else if (deviceType.Equals("Android"))
            //    {
            //        data = new AndroidPushData
            //        {
            //            DeviceToken = devicetoken1,
            //            Notification = new AndroidNotification()
            //        };
            //    }
            //    pushNotificationService.EnqueueData(data);
            //} 


            Console.ReadKey();
        }

        //async static Task SendNotification(AppleNotification notification, List<string> deviceTokens)
        //{
        //    if (notification == null || deviceTokens == null) return;
        //    if (deviceTokens.Count == 0) return;

        //    foreach (var devicetoken in deviceTokens)
        //    {
        //        APNsResponse aPNsResponse = await iOSPushService.PushAsync(notification, devicetoken);
        //        Debug.WriteLine("APNsResponse: " + aPNsResponse);
        //    }
        //}

        static AppleNotification GetTestPayload()
        {
            var alertBody = new AlertBody();
            alertBody.Title = "標頭2";
            alertBody.SubTitle = "副標題";
            alertBody.Content = "內文2";

            var payload = new ApsPayload();
            payload.Category = "0"; 
            payload.Badge = 2;
            payload.AlertBody = alertBody;
            //payload.MutableContent = 1; //手機收到推播前預先處裡機制

            var notification = new AppleNotification();
            notification.Aps = payload;
            notification.NotificationSN = 82;
            notification.CreateDate = "2020-02-11 10:58:27";
            notification.Link = "https://osis.hamastar.com.tw/ICS_Application/CheckMailandPhone/0/5";
            notification.EncryptSN = "@#$F@#$$@FF";
           
            return notification;
        }

        //必須給 (相對路徑)
        public static string GetP8FilePath()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string iosPath = @"Apple";
            string p8FilePath = rootPath + iosPath;
            return p8FilePath;
        }
    }
}
