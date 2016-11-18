using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using Codeplex.Data;       

namespace ThetaShelter
{   
    class Theta //Singleton
    {
        private static Theta theta;

        private Theta() //Private Constructor
        {
            //do nothing
        }

        public static Theta Instance
        {
            get
            {
                theta = theta ?? new Theta();
                return theta;
            }
        }

        public bool Connect()
        {
             try
            {
                Task.Run(async () =>
                {
                    var sid = await StartSession();
                    await ChangeApiLevelTo21(sid); 
                });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string TakePicture()
        {
            string state = string.Empty;
            try
            {
                Task task = Task.Run(async () =>
                {
                    state = await GetState(); 
                });
                task.Wait();                       
            }
            catch
            {
                state = "Exception!!";
            }
                               
            return state;     
        }

        private async Task<string> StartSession()
        {
            string uri = "http://192.168.1.1/osc/commands/execute";
            string request = @"
            {
                ""name"": ""camera.startSession"",
                ""parameters"": { }
            }";

            var json = await PostAsync(uri, request);
            dynamic result = DynamicJson.Parse(json);
            return result.parameters.sessionId;
        }

        private async Task ChangeApiLevelTo21(string sid)
        {
            if (sid.Equals(string.Empty)) throw new ArgumentException();
            string uri = "http://192.168.1.1/osc/commands/execute";
            string request = @"
            {
                ""name"": ""camera.setOptions"",
                ""parameters"": {
                    ""sessionId"": ""SID_xxxx"",
                    ""options"": {
                        ""clientVersion"": 2
                    }
                }
            }".Replace("SID_xxxx", sid);

            await PostAsync(uri, request);
            return;
        }

        private async Task<string> GetState()
        {
            string uri = "http://192.168.1.1/osc/state";
            string request = @"";

            var json = await PostAsync(uri, request);
            dynamic result = DynamicJson.Parse(json);
            return result.fingerprint;              
            
        }


        private async Task<string> PostAsync(string uri, string request)
        {
            var client = new HttpClient();
            var response = new HttpResponseMessage();

            try
            {
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch
            {
                return "";
            }
        }
    }
}