﻿using PushNotification.Android.Model;
using PushNotification.Common;
using PushNotification.Common.Model;
using PushNotification.IOS.Model;
using PushNotification.Web.Model;
using System.Collections.Generic;

namespace PushNotification.Common.Model
{
    public interface IPushData
    {
        string GetType();

    }

    public class PushData
    {
        
        private List<IOSPushData> _iOSData = new List<IOSPushData>();
        private List<AndroidPushData> _androidData = new List<AndroidPushData>();
        private List<WebPushData> _webData = new List<WebPushData>();

        public List<IOSPushData> IOS { get => _iOSData; }
        public List<AndroidPushData> Android { get => _androidData; }
        public List<WebPushData> Web { get => _webData; }

        public int GetIOSCount()
        {
            if (_iOSData == null)
                return 0;
            return _iOSData.Count;
        }

        public int GetAndroidCount()
        {
            if (_androidData == null)
                return 0;
            return _androidData.Count;
        }

        public void AddIOS(IOSPushData data)
        {
            if (_iOSData != null)
                _iOSData.Add(data);
        }

        public void AddAndroid(AndroidPushData data)
        {
            if (_androidData != null)
                _androidData.Add(data);
        }

        public void AddWeb(WebPushData data)
        {
            if (_webData != null)
                _webData.Add(data);
        }

        //public void Add(IPushData data)
        //{
        //    var type = data.GetType();
        //    if (type.Equals("IOS"))
        //    {
        //        if (_iOSData != null)
        //            _iOSData.Add(data);
        //    }
        //    else if (type.Equals("Android"))
        //    {
        //        if (_androidData != null)
        //            _androidData.Add(data);
        //    }
        //}
    }
}

public class IOSPushData :IData
{ 

    public string DeviceToken { get; set; }
    public AppleNotification Notification { get; set; }

    public DataType GetDataType()
    {
        return DataType.IOS;
    }
}

public class AndroidPushData : IData
{ 
    public string DeviceToken { get; set; }
    public AndroidNotification Notification { get; set; }
    //string IPushData.GetType()
    //{
    //    return "Android";
    //}
    public DataType GetDataType()
    {
        return DataType.Android;
    }

}

public class WebPushData : IData
{
    public string DeviceToken { get; set; }
    public WebNotification Notification { get; set; }
    public DataType GetDataType()
    {
        return DataType.Web;
    }
}
