using System;
using RestSharp;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Scorpion_MDB
{
    public class Scorpion_JSON
    {
        public string JSON_get(string URL)
        {
            Console.WriteLine("JSON: " + URL);
            Task<string> ts = JSON_getAsync(URL);
            ts.Wait();
            Console.WriteLine("Done JSON: " + URL + "GOT " + ts.Result);
            return ts.Result;
        }

        public string JSON_get_auth(string URL, string AUTH)
        {
            Console.WriteLine("auth JSON: " + URL);
            Task<string> ts = JSON_getAsync_auth(URL, AUTH);
            ts.Wait();
            Console.WriteLine("Done auth JSON: " + URL);
            return ts.Result;
        }

        public string JSON_post_auth(string URL, string AUTH, string TOKEN, string DATE)
        {
            Console.WriteLine("auth JSON: " + URL);
            Task<string> ts = JSON_postAsync_auth(URL, AUTH, TOKEN, DATE);
            ts.Wait();
            Console.WriteLine("Done auth JSON: " + URL);
            return ts.Result;
        }

        private async Task<string> JSON_getAsync(string URL)
        {
            var response = "";
            try
            {
                var client = new RestClient(URL);
                //client.Authenticator = new HttpBasicAuthenticator("username", "password");

                var request = new RestRequest("", DataFormat.None);
                response = await client.GetAsync<string>(request, CancellationToken.None);
            }
            catch { }
            return response;
        }

        private async Task<string> JSON_getAsync_auth(string URL, string AUTH)
        {
            var client = new RestClient(URL);
            client.AddDefaultHeader("Authorization", "Bearer " + AUTH);
            //client.Authenticator = new HttpBasicAuthenticator("username", "password");

            var request = new RestRequest("", DataFormat.None);
            var response = await client.GetAsync<string>(request, CancellationToken.None);

            return response;
        }

        private async Task<string> JSON_postAsync_auth(string URL, string USER, string TOKEN, string DATE)
        {
            //Always put ? , make sure php errors are suppressed
            var client = new RestClient(check_POSTURL(URL));
            var request = new RestRequest("", Method.POST);

            request.AddParameter("Authorization", TOKEN);
            request.AddParameter("User", USER);
            request.AddParameter("Date", DATE);
            request.AddParameter("Accept", "application/json ");
            request.AddParameter("Content-Type", "application/x-www-form-urlencoded");
            request.RequestFormat = DataFormat.Xml;

            string response = await client.PostAsync<string>(request, CancellationToken.None);
            Console.WriteLine("Response: {0}", response);
            return response;
        }
        //Convert
        public JArray jsontoarray(ref string JSON)
        {
            return JArray.Parse(JSON);
        }

        public string check_POSTURL(string URL)
        {
            if (!URL.EndsWith("?", StringComparison.CurrentCulture))
                return URL.Insert(URL.Length, "?");
            return URL;
        }
    }
}
