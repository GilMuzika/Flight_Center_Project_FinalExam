using Flight_Center_Project_FinalExam_DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Web_Api_interface.Views.FlightSystemMain
{
    public class WebClientSideJobDoer<T> where T : class, IPoco, new()
    {
        private string _apiControllerMethodUrl;
        public string ApiControllerMethodUrl
        {
            get => _apiControllerMethodUrl;
            set
            {
                _apiControllerMethodUrl = value;
                _client.BaseAddress = new Uri(_apiControllerMethodUrl);
            }
        }

        private T _value;
        public T Value
        {
            get => _value;
            set => _value = value;
        }

        private string _authorisationToken;
        public string AuthorisationToken
        {
            get => _authorisationToken;
            set
            {
                _authorisationToken = value;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", _authorisationToken);
            }
        }


        HttpClient _client = new HttpClient();

        public WebClientSideJobDoer(string apiControllerMethodUrl, T value, string authorisationToken)
        {            
            _apiControllerMethodUrl = apiControllerMethodUrl;            
            _value = value;
            _authorisationToken = authorisationToken;

            Initialize();
        }

        private void Initialize()
        {
            //_client.BaseAddress = new Uri(_apiControllerMethodUrl);
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", _authorisationToken);
        }

        /*public async Task<string> PostAsJsonAsync()
        {
            return await Task.Run(async() => 
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(_apiControllerMethodUrl, _value);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                else return String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);


            });
        }*/
        public async Task<string> PostAsJsonAsync()
        {


            var data = new StringContent(JsonConvert.SerializeObject(_value, new JsonSerializerSettings(){ DefaultValueHandling = DefaultValueHandling.Ignore }).ToString(), Encoding.UTF8, "application/json");

            Initialize();


            HttpResponseMessage response = null;
            using (var request = new HttpRequestMessage(HttpMethod.Post, _apiControllerMethodUrl))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", System.Web.HttpContext.Current.Session["WebApiAccessToken"].ToString()); // -> How to put the auth token to "Session"! Important!
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorisationToken);
                request.Content = data;

              
                response = await _client.SendAsync(request);
            }

             if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
             else return String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        }


    }
}
