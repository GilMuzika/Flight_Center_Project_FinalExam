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

namespace Web_Api_interface
{
    class JsonToDictionaryConverter
    {


        public JsonToDictionaryConverter()
        {
        }

        /// <summary>
        /// Converts Json object to a "Dictionary<string, object>"
        /// where the value "object" may be a JArray, JObject or String.
        /// </summary>
        /// <param name="jObjectData">input data as JSON object (JToken)</param>
        /// <returns></returns>
        public Dictionary<string, object> ProvideAPIDataFromJSON(JToken jObjectData)
        {         
            if(jObjectData is JObject)
            return JsonToDictioanary(jObjectData);

            Dictionary<string, object> superDict = new Dictionary<string, object>();
            if (jObjectData is JArray)
            {                
                superDict.Add($"Class1_Property1Array__0", JsonToDictioanary(jObjectData));                
            }
            return superDict;
        }


        private delegate void LikeActionButWithParameter(JToken jobject);
        private int _iterationsCount = 0;
        private int _upperLevelClassesCount = 1;
        private Dictionary<string, object> JsonToDictioanary(JToken jtoken)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            LikeActionButWithParameter processJObject = (JToken jobject) => 
            {              
                foreach (var s in JObject.Parse(jobject.ToString()))
                {
                    string name = s.Key;
                    JToken value = s.Value;

                    try
                    {
                        if (value.HasValues)
                        {
  

                            if (value.Type is Newtonsoft.Json.Linq.JTokenType.Array)
                            {
                                if (!value.Children().FirstOrDefault().HasValues)
                                {
                                    if (ReplaceJsonTypeByCsharpType(value.Children().FirstOrDefault().Type.ToString(), out string CsharpType))
                                         dictionary.Add($"{name}_{CsharpType}Array__{_iterationsCount}", name);                                    
                                    else dictionary.Add($"{name}_{value.Children().FirstOrDefault().Type}Array__{_iterationsCount}", name);
                                }
                                else
                                {
                                    if (ReplaceJsonTypeByCsharpType(value.Children().FirstOrDefault().Type.ToString(), out string CsharpType))
                                         dictionary.Add($"{name}_{CsharpType}Array__{_iterationsCount}", JsonToDictioanary(value));
                                    else dictionary.Add($"{name}Array__{_iterationsCount}", JsonToDictioanary(value));
                                }

                            }
                            else dictionary.Add($"{name}__{_iterationsCount}", JsonToDictioanary(value));
                        }

                        else
                        {
                            if (ReplaceJsonTypeByCsharpType(value.Type.ToString(), out string CsharpType))
                                dictionary.Add($"{name}_{CsharpType}__{_iterationsCount}", value.ToString());
                            else dictionary.Add($"{name}_{value.Type}__{_iterationsCount}", value.ToString());
                        }
                        _iterationsCount++;


                        _upperLevelClassesCount++;
                    }
                    catch { }
                }
            };


            if (jtoken is JObject) processJObject(jtoken);
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
            jsonAndCsharpDataTypeCorrelation.Add("Integer", "int?");            
            jsonAndCsharpDataTypeCorrelation.Add("Float", "float?");            




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
