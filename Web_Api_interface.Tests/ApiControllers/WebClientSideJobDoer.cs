using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Web_Api_interface.Tests.ApiControllers
{
    class WebClientSideJobDoer<T> where T : class, IPoco, new()
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
            _client.BaseAddress = new Uri(_apiControllerMethodUrl);
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", _authorisationToken);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync()
        {
            return await _client.PostAsJsonAsync(_apiControllerMethodUrl, _value);
        }


    }
}
