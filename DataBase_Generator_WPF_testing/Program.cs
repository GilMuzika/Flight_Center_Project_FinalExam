using System;
using System.Net;
using System.Web.Script.Serialization;

namespace DataBase_Generator_WPF_testing
{
    class Program
    {
        static WebClient _webClient = new WebClient();
        static void Main(string[] args)
        {
            consumeRestAPI("https://randomuser.me/api");


            Console.Read();
        }

        static void consumeRestAPI(string url)
        {

            _webClient.Headers["Content-Type"] = "application/json";
            string consumedData = _webClient.DownloadString(url);
            var serializer = new JavaScriptSerializer();
            dynamic jsonObject = serializer.Deserialize<dynamic>(consumedData);

        }
    }
}
