using Microsoft.Extensions.FileProviders;
using PushNotification.IOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PushNotification.IOS.P8
{
    /// <summary>
    /// Only Read the two files 'xxxx.p8' and 'P8.txt'
    /// Implement Interface: IP8FileHelper
    /// 
    /// xxxxx.p8: Must be download from apple developer's backend.
    /// 
    /// P8.txt rule of content:
    /// 
    ///     KeyID:Your-P8-KeyID(You got this keyID when you first download p8 file from apple developer's backend.)
    ///     TeamID:Your-Apple-Developer-TeamID
    /// 
    /// </summary>

    public class P8FileHelper : IP8FileHelper
    {
        
        private readonly Dictionary<string, string> p8 = new Dictionary<string, string>()
        {
            {"Key" ,null},
            {"KeyID" ,null},
            {"TeamID" ,null}
        };


        public string GetP8Key()
        {
            string result = null;
            if (p8.ContainsKey("Key"))
                result = p8["Key"];
            return result;
        }

        public string GeP8KeyID()
        {
            string result = null;
            if (p8.ContainsKey("KeyID"))
                result = p8["KeyID"];
            return result;
        }

        public string GetTeamID()
        {
            string result = null;
            if (p8.ContainsKey("TeamID"))
                result = p8["TeamID"];
            return result;
        }


        public P8FileHelper(string path)
        {
            InitKeyIDTeamIDFromFile(path);
            InitP8KeyFromFile(path);
        }

        private void InitP8KeyFromFile(string path)
        {
            var provider = new PhysicalFileProvider(path);
            string[] files = Directory.GetFiles(provider.Root, "*.p8");
            string p8FilePath;
            if (files.Length > 0)
                p8FilePath = files[0].ToString();
            else
                throw new Exception("讀取P8Key路徑失敗");

            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(p8FilePath))
            {
                var privateKeyContent = File.ReadAllText(p8FilePath);
                var privateKeyList = privateKeyContent.Split('\n'); //hard code
                int upperIndex = privateKeyList.Length;

                for (int i = 1; i < upperIndex - 1; i++)
                {
                    builder.Append(privateKeyList[i]);
                }

                if (p8.ContainsKey("Key"))
                    p8["Key"] = builder.ToString();
            }
        }

        private void InitKeyIDTeamIDFromFile(string path)
        {
            var provider = new PhysicalFileProvider(path);

            IFileInfo fileInfo = provider.GetFileInfo("p8.txt");

            if (!fileInfo.Exists)
                throw new FileNotFoundException("p8.txt not found");

            Stream sr = fileInfo.CreateReadStream();
            if (sr == null)
                return;

            try
            {
                using (var reader = new StreamReader(sr))
                {

                    string[] content;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        content = line.Split(":");
                        if (content.Length == 2 && p8[content[0]] == null)
                        {
                            p8[content[0]] = content[1];
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
     
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in p8)
            {
                sb.AppendFormat("key={0}, value={1}\r\n", kvp.Key, kvp.Value);
            }

            return sb.ToString();
        }
       
    }
}
