using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndPoint { get; set; }

        public ApiClient(Uri baseEndpoint)
        {
            if(baseEndpoint==null) throw new ArgumentNullException("baseEndpoint");
            BaseEndPoint = baseEndpoint;
            _httpClient=new HttpClient();
        }

        private async Task<T> GetAsync<T>(Uri requestUrl)
        {
            addHeaders();
            
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        private async Task<Message<T>> PostAsync<T>(Uri requestUrl, T content)
        {
            addHeaders();

            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T>>(data);
        }

        private async Task<Message<T1>> PostAsync<T1, T2>(Uri requestUrl,T2 content)
        {
             addHeaders();

            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T1>>(data);
        }


        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var uriBuilder = new UriBuilder(new Uri(BaseEndPoint, relativePath));
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }
        

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        private void addHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("userIP");
            _httpClient.DefaultRequestHeaders.Add("userIP","192.168.1.1");
        }
    }
}