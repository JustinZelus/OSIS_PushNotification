using PushNotification.IOS.Interfaces;
using PushNotification.IOS.IO;
using PushNotification.IOS.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PushNotification.IOS.P8
{
    public abstract class BaseP8JwtHelper : IP8JwtHelper
    {
        IFileHelper fileHelper;
        private Dictionary<string, string> tokens = new Dictionary<string, string>()
        {
            { "p8key",null},
            { "p8keyID",null},
            { "teamID",null},
            { "Jwt",null},
            { "ValidTime",null}
        };
        protected string P8Key
        {
            get
            {
                if (tokens.ContainsKey("p8key"))
                    return tokens["p8key"];
                return null;
            }
            set
            {
                if (tokens.ContainsKey("p8key"))
                    tokens["p8key"] = value;
            }
        }
        protected string P8KeyID
        {
            get
            {
                if (tokens.ContainsKey("p8keyID"))
                    return tokens["p8keyID"];
                return null;
            }
            set
            {
                if (tokens.ContainsKey("p8keyID"))
                    tokens["p8keyID"] = value;
            }
        }
        protected string TeamID
        {
            get
            {
                if (tokens.ContainsKey("teamID"))
                    return tokens["teamID"];
                return null;
            }
            set
            {
                if (tokens.ContainsKey("teamID"))
                    tokens["teamID"] = value;
            }
        }
        protected string Jwt
        {
            get
            {
                if (tokens.ContainsKey("Jwt"))
                    return tokens["Jwt"];
                return null;
            }
            set
            {
                if (tokens.ContainsKey("Jwt"))
                    tokens["Jwt"] = value;
            }
        }
        protected string ValidTime
        {
            get
            {
                if (tokens.ContainsKey("ValidTime"))
                    return tokens["ValidTime"];
                return null;
            }
            set
            {
                if (tokens.ContainsKey("ValidTime"))
                    tokens["ValidTime"] = value;
            }
        }
        /// <summary>
        /// 初始化p8key p8keyID teamID
        /// </summary>
        /// <param name="p8key"></param>
        /// <param name="p8keyID"></param>
        /// <param name="teamID"></param>
        public BaseP8JwtHelper(string p8key, string p8keyID, string teamID)
        {
            if(fileHelper == null)
                fileHelper = new NormalFileHelper();

            if (tokens.ContainsKey("p8key"))
                tokens["p8key"] = p8key;
            if (tokens.ContainsKey("p8key"))
                tokens["p8keyID"] = p8keyID;
            if (tokens.ContainsKey("p8key"))
                tokens["teamID"] = teamID;
        }

        public abstract void CreateJwt();
        public abstract void RefreshJwt();
        public abstract bool ValidateJwtTime();
        public string GetJwt()
        {
            if (!tokens.ContainsKey("Jwt"))
                return null;
            return tokens["Jwt"];
        }
        protected abstract string[] GetLines();
        protected string GetRootPath()
        {
            if (fileHelper == null) return null;
            return fileHelper.RootPath;
        }
        protected bool CheckFileExists(string fileName, string folder)
        {
            if (fileHelper == null) return false;
            return fileHelper.CheckExists(fileName, folder);
        }
        /// <summary>
        /// 初始化Jwt、ValidTime
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        protected virtual void SearchJwtProcedure(string folder, string fileName)
        {
            if (fileHelper == null) return;

            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(fileName))
                return;

            var path = fileHelper.RootPath + folder;

            //if (!fileHelper.CheckExists(fileName, folder))
            if (!CheckFileExists(fileName, folder))
            {
                fileHelper.Create(path, fileName);

                CreateJwt();
                WriteLines(path, fileName, GetLines());
            }
            else
            {
                string[] lines = fileHelper.Read(path, Constants.APNS_FILE);

                if (lines == null)
                {
                    CreateJwt();
                    WriteLines(path, fileName, GetLines());
                    lines = fileHelper.Read(path, Constants.APNS_FILE);
                }

                foreach (string line in lines)
                {
                    string[] contents = line.Split(":");
                    foreach (string content in contents)
                    {
                        if (contents.Length == 2)
                        {
                            if (content.Contains(Constants.KEY_WORD))
                            {
                                Jwt = contents[1];
                                break;
                            }

                            if (content.Contains(Constants.VALID_TIME_WORD))
                            {
                                ValidTime = contents[1];
                                break;
                            }
                        }
                    }
                }
                if (!ValidateJwtTime())
                {
                    RefreshJwt();
                    WriteLines(path,fileName,GetLines());
                }
            }
        }

        protected void WriteLines(string path ,string fileName,string[] lines)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName) || lines.Length == 0)
                return;
            if (fileHelper == null) return;

            fileHelper.WriteLines(Path.Combine(path, fileName), GetLines());
        }

        protected void WriteLines(string path,string[] lines)
        {
            if (string.IsNullOrEmpty(path)  || lines.Length == 0)
                return;
            if (fileHelper == null) return;

            fileHelper.WriteLines(path, GetLines());
        }
    }
}
