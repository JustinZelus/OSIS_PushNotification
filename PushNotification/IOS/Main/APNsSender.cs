using Newtonsoft.Json;
using PushNotification.IOS.Enums;
using PushNotification.IOS.Interfaces;
using PushNotification.IOS.Model;
using PushNotification.IOS.Others;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.IOS.Main
{
    public class APNsSender : IAPNs
    {
        private static readonly Dictionary<APNsServer, string> servers = new Dictionary<APNsServer, string>
        {
            {APNsServer.Development,"https://api.development.push.apple.com:443" },
            {APNsServer.Production,"https://api.push.apple.com:443" }
        };

        private const string APN_ID = "apns-id";
        private const string APN_ID_TOPIC = "apns-topic";
        private const string APN_EXPIRATION = "apns-expiration";
        private const string APN_PRIORITY = "apns-priority";
        private const string APN_PUSHTYPE = "apns-push-type";

   

        private readonly string appBundleId;
        private readonly string _jwt;
        private readonly Lazy<HttpClient> http;
        private readonly Lazy<Http2CustomHandler> handler;

        public APNsSender(string jwt,string appBundleId)
        {
            _jwt = jwt;


            this.appBundleId = appBundleId;
            this.handler = new Lazy<Http2CustomHandler>(() => new Http2CustomHandler());
            this.http = new Lazy<HttpClient>(() => new HttpClient(handler.Value));
        }

        public async Task<APNsResponse> SendAsync(object notification,
                                            string deviceToken,
                                            APNsServer envType,
                                            string apnsId = null,
                                            int apnsExpiration = 0,
                                            int apnsPriority = 10, //apple官方建議
                                            bool isBackground = false)
        {
            var path = $"/3/device/{deviceToken}";
            var json = JsonConvert.SerializeObject(notification);

            //@@test
            //Uri to request
           // var uri = new Uri(string.Format("https://{0}:{1}/3/device/{2}", "api.development.push.apple.com", "443", deviceToken));
            //@@---


            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(servers[envType] + path))
            {
                Version = new Version(2, 0),
                Content = new StringContent(json),
                Method = HttpMethod.Post,
                //    RequestUri = uri
            };

            var testOneHourToken = "eyJhbGciOiJFUzI1NiIsImtpZCI6IlJSWTg1NFU0TjQifQ==.eyJpc3MiOiJCSzc2Rjk4RFk3IiwiaWF0IjoxNTgwMDE5NjQ1fQ==.EK2ju4LCWjpk8CmQDBMCRWWt77kaefqx5BVAB9zHgheDfbCjySAd7+5P/EE6MZD5LjstRrbg7AkbhQaYGknBBQ==";

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _jwt); //jwtToken.Value
            request.Headers.TryAddWithoutValidation(":path", path);
            request.Headers.Add(APN_ID_TOPIC, appBundleId);
            request.Headers.Add(APN_EXPIRATION, apnsExpiration.ToString());
            request.Headers.Add(APN_PRIORITY, apnsPriority.ToString());
            request.Headers.Add(APN_PUSHTYPE, isBackground ? "background" : "alert");  // for iOS 13 required

            if (!string.IsNullOrWhiteSpace(apnsId))
            {
                request.Headers.Add(APN_ID, apnsId);
            }

            var response = await http.Value.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                var code = response.StatusCode;
                var body = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<APNsError>(body);

                var result = new APNsResponse
                {
                    statusCode = code,
                    IsSuccess = false,
                    Error = error
                };
                Debug.WriteLine("response失敗回應: ");
                Debug.WriteLine("  statusCode:  " + result.statusCode);
                Debug.WriteLine("  IsSuccess:  " + result.IsSuccess);
                Debug.WriteLine("  Error:  " + result.Error.Reason);
                return result;
            }
            else
            {
                var code = response.StatusCode;
                var succeed = response.IsSuccessStatusCode;
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<APNsError>(content);

                var result = new APNsResponse
                {
                    statusCode = code,
                    IsSuccess = succeed,
                    Error = error
                };
                Debug.WriteLine("response成功回應: ");
                Debug.WriteLine("  statusCode:  " + result.statusCode);
                Debug.WriteLine("  IsSuccess:  " + result.IsSuccess);
                Debug.WriteLine("  Error:  " + result.Error);
                return result;
            }

        }

    }
}
