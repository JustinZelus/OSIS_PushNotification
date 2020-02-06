using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PushNotification.IOS.IO
{
    public abstract class BaseFileHelper : IFileHelper
    {
        public string RootPath { get => AppDomain.CurrentDomain.BaseDirectory; }
        IFileInfo fileInfo;

        public BaseFileHelper()
        {
        }

        public bool CheckExists(string fileName, string path = "")
        {
            var provider = new PhysicalFileProvider(RootPath + path);
            fileInfo = provider.GetFileInfo(fileName);
            return fileInfo.Exists;
        }

        public void Create(string path, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName))
                    throw new IOException("path,fileName can't be null");
                File.Create(Path.Combine(path, fileName)).Close();


            }
            catch (IOException ex)
            {
                Debug.WriteLine("BaseFileHelper: " + ex.Message);
            }
        }

        public void WriteLines(string path, string[] lines)
        {
            try
            {
                //分行寫入
                File.WriteAllLines(path, lines);

                //using (StreamWriter writer = new StreamWriter(path))
                //{
                //    var target = lines;
                //    for (int i = 0; i < lines.Length; i++)
                //    {
                //        writer.WriteLine(target[i]);
                //    }
                //}
            }
            catch (IOException ex)
            {
                Debug.WriteLine("BaseFileHelper: " + ex.Message);
            }
        }

        private bool GetFileInfo(string path, string fileName)
        {
            var provider = new PhysicalFileProvider(path);
            fileInfo = null;
            fileInfo = provider.GetFileInfo(fileName);

            return fileInfo.Exists;
        }

        public string[] Read(string path, string fileName)
        {
            if (!GetFileInfo(path, fileName))
                return null;

            List<string> content = new List<string>();

            if (fileInfo != null)
            {
                Stream sr = fileInfo.CreateReadStream();
                try
                {
                    using (var reader = new StreamReader(sr))
                    {
                        string line;

                        //修正
                        while ((line = reader.ReadLine()) != null)
                        {
                            content.Add(line);
                        }
                        reader.Close();
                        sr.Close();
                    }

                }
                catch (IOException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return content.ToArray();
        }


    }
}
