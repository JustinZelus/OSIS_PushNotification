using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using PushNotification.IOS;
using PushNotification.IOS.Enums;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Main;
using PushNotification.IOS.Model;
using PushNotification.IOS.P8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PushNotification
{
    class Program
    {
        static string devicetoken1 = "63d591a04b573af2545c2d587fb1d94071d3b7d24fb5e0dd9fc16ea70fc0af9b"; //justin
        static string devicetoken2 = "e2cb81a9417812db39fd1c435c0f1734db50809acf487cf6aed7af827c1eedd4"; //Ruru
        static string devicetoken3 = "25dd28084fbbe6763756a101e4fa283ee07f296b20152586e844de87ee62c08e"; //羿伻

        static string appBundleID = "com.hamastar.intelligence.cityservice";

        static IOSPushNotificationService iOSPushService;

         async static Task Main(string[] args)
        {



            IP8FileHelper p8FileHelper = new P8FileHelper(GetP8FilePath());
            
            Debug.WriteLine(p8FileHelper.ToString());

            //iOSPushService = new IOSPushNotificat11ionService(p8FileHelper.GetP8Key(),
            //                                                p8FileHelper.GeP8KeyID(),
            //                                                p8FileHelper.GetTeamID(),
            //                                                appBundleID);
            iOSPushService = IOSPushNotificationService.Builder()
                                .SetP8key(p8FileHelper.GetP8Key())
                                .SetP8keyID(p8FileHelper.GeP8KeyID())
                                .SetP8TeamID(p8FileHelper.GetTeamID())
                                .SetAppBundleID(appBundleID)
                                .SetAPNsServer(APNsServer.Development)
                                .Build();
            await DoSomething();



           
        }


        async static Task DoSomething()
        {
            var notification = GetTestPayload();


            var jsonObj = JsonConvert.SerializeObject(notification);
            Debug.WriteLine("JSON Dictionary: " + jsonObj);

            List<string> deviceTokens = new List<string>();
            deviceTokens.Add(devicetoken1);


            //---回覆的資料---
            foreach (var devicetoken in deviceTokens)
            {
                APNsResponse aPNsResponse = await iOSPushService.PushAsync(notification, devicetoken);
                // Debug.WriteLine("yoyoyoyoy");
                Debug.WriteLine("APNsResponse: " + aPNsResponse);
            }
        }

        static AppleNotification GetTestPayload()
        {
            AppleNotification notification = new AppleNotification();
            var alertBody = new AlertBody();
            alertBody.Title = "標頭2";
            alertBody.Content = "內文2";
            var payload = new AppleNotification.ApsPayload();
            payload.Badge = 20;
            payload.Sound = "default";


            payload.AlertBody = alertBody;
            notification.Aps = payload;

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
