using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotification.IOS.IO
{
    interface IFileHelper
    {
        string RootPath { get; }
        void Create(string path, string fileName);
        bool CheckExists(string fileName, string path = "");
        void WriteLines(string path, string[] lines);
        string[] Read(string path, string fileName);
    }
}
