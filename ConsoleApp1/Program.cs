using Newtonsoft.Json;
using PushNotification.IOS.Enums;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Main;
using PushNotification.IOS.Model;
using PushNotification.IOS.P8;
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
        static string devicetoken1 = "73fc49879af114304e8203f4b3fb2cd3d93d832c68676fffaae809d5628f25d5"; //justin
        static string devicetoken2 = "e2cb81a9417812db39fd1c435c0f1734db50809acf487cf6aed7af827c1eedd4"; //Ruru
        static string devicetoken3 = "25dd28084fbbe6763756a101e4fa283ee07f296b20152586e844de87ee62c08e"; //羿伻
        //
        static string appBundleID = "com.hamastar.intelligence.cityservice";
        //推播的類
        static IOSPushNotificationService iOSPushService;

        async static Task Main(string[] args)
        {

            IP8FileHelper p8FileHelper = new P8FileHelper(GetP8FilePath());
            Debug.WriteLine(p8FileHelper.ToString());

            iOSPushService = IOSPushNotificationService.Builder()
                                .SetP8key(p8FileHelper.GetP8Key()) //require
                                .SetP8keyID(p8FileHelper.GeP8KeyID()) //require
                                .SetP8TeamID(p8FileHelper.GetTeamID()) //require
                                .SetAppBundleID(appBundleID) ////require
                                .SetAPNsServer(APNsServer.Development) ////require
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
                Debug.WriteLine("APNsResponse: " + aPNsResponse);
            }
        }

        static AppleNotification GetTestPayload()
        {
            var notification = new AppleNotification();
            var alertBody = new AlertBody();
            alertBody.Title = "標頭2";
            alertBody.SubTitle = "副標題";
            alertBody.Content = "內文2";

            var payload = new AppleNotification.ApsPayload();
            payload.Category = "NORMAL"; 
            payload.Badge = 22;
            payload.Sound = "default";
            payload.AlertBody = alertBody;

            var detailPlayload = new DetailPlayload();
            detailPlayload.SN = 12;


            notification.Aps = payload;
            notification.Detail = detailPlayload;

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
