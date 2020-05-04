using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Generator_WPF
{
    class RestAPIConsumer
    {
        private string _aPIUrl;
        private WebClient _webClient = new WebClient();
        //private List<string> _allDataTypes;

        public RestAPIConsumer(string aPIUrl)
        {
            _aPIUrl = aPIUrl;

            
            //_allDataTypes = typeof(JToken).Assembly.GetTypes().Select(x => x.Name).ToList();

        }

        public Dictionary<string, object> ProvideAPIDataFromJSON()
        {
            _webClient.Headers["Content-Type"] = "application/json";
            string consumedData = _webClient.DownloadString(_aPIUrl);
            JObject jObjectData = JObject.Parse(consumedData);

            return JsonToDictioanary(jObjectData);
        }


        private delegate void LikeActionButWithParameter(JObject jobject);
        private Dictionary<string, object> JsonToDictioanary(JToken jtoken)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            LikeActionButWithParameter processJObject = (JObject jobject) => 
            {
                foreach (var s in JObject.Parse(jobject.ToString()))
                {
                    string name = s.Key;
                    JToken value = s.Value;

                    var r = value.Type;

                    if (value.HasValues)
                    {
                        if (value.Type is Newtonsoft.Json.Linq.JTokenType.Array)
                             dictionary.Add($"{name}Array", JsonToDictioanary(value));
                        else dictionary.Add(name, JsonToDictioanary(value));
                    }

                    else
                    {
                        if(ReplaceJsonTypeByCsharpType(value.Type.ToString(), out string CsharpType))
                             dictionary.Add($"{name}_{CsharpType}", value.ToString());
                        else dictionary.Add($"{name}_{value.Type}", value.ToString());
                    }
                }
            };

            if (jtoken is JObject) processJObject((JObject)jtoken);
            if (jtoken is JArray)
            {
                foreach (JObject child in jtoken.Children<JObject>())
                {
                    processJObject(child);
                }
            }
            return dictionary;           
        }


        private bool ReplaceJsonTypeByCsharpType(string jsonType, out string CsharpType)
        {
            bool toReturn = false;
            string CsharpTypeToOut = "no_such_a_type";
            
            Dictionary<string, string> jsonAndCsharpDataTypeCorrelation = new Dictionary<string, string>();
            jsonAndCsharpDataTypeCorrelation.Add("Date", "DateTime");
            jsonAndCsharpDataTypeCorrelation.Add("Integer", "int");
            jsonAndCsharpDataTypeCorrelation.Add("Null", "string");



            foreach (var s in jsonAndCsharpDataTypeCorrelation)
            {
                if (s.Key.Equals(jsonType))
                {
                    toReturn = true;
                    CsharpTypeToOut = jsonAndCsharpDataTypeCorrelation[jsonType];
                }
            }
            CsharpType = CsharpTypeToOut;
            return toReturn;
        }












    }
}
