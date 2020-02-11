using PushNotification.IOS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using static PushNotification.IOS.Model.AppleNotification;

namespace PushNotification.IOS.Others
{
    public class IOSNotificationBuilder
    {
        private string _title;
        private string _subTitle;
        private string _content;
        private string _category;
        private int _badge;
        private int _notificationSN;
        private string _createDate;
        private string _link;
        private string _encryptSN;


        public AppleNotification Build()
        {
            if (!Check()) return null;

            var alertBody = new AlertBody();
            alertBody.Title = _title;
            if (!string.IsNullOrEmpty(_subTitle))
                alertBody.SubTitle = _subTitle;
            alertBody.Content = _content;


            var payload = new ApsPayload();
            payload.Category = _category;
            payload.Badge = _badge;
            payload.AlertBody = alertBody;

            var notification = new AppleNotification();
            notification.Aps = payload;

            notification.NotificationSN = _notificationSN;
            notification.CreateDate = _createDate;
            if (!string.IsNullOrEmpty(_link))
                notification.Link = _link;
            notification.EncryptSN = _encryptSN;

            return notification;
        }

        private bool Check()
        {

            if (string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_content) || string.IsNullOrEmpty(_category)
                || string.IsNullOrEmpty(_createDate) || string.IsNullOrEmpty(_encryptSN))
                return false;
            if (_badge == 0 || _notificationSN == 0)
                return false;
            return true;
        }

        public IOSNotificationBuilder SetTitle(string title)
        {
            _title = title;
            return this;
        }

        public IOSNotificationBuilder SetSubTitle(string subTitle)
        {
            _subTitle = subTitle;
            return this;
        }

        public IOSNotificationBuilder SetContent(string content)
        {
            _content = content;
            return this;
        }

        public IOSNotificationBuilder SetCategory(string category)
        {
            _category = category;
            return this;
        }

        public IOSNotificationBuilder SetBadge(int count)
        {
            _badge = count;
            return this;
        }

        public IOSNotificationBuilder SetNotificationSN(int notificationSN)
        {
            _notificationSN = notificationSN;
            return this;
        }

        public IOSNotificationBuilder SetCreateDate(string date)
        {
            _createDate = date;
            return this;
        }

        public IOSNotificationBuilder SetLink(string url)
        {
            _link = url;
            return this;
        }

        public IOSNotificationBuilder SetEncryptSN(string sn)
        {
            _encryptSN = sn;
            return this;
        }
    }
}
