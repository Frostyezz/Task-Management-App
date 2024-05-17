using BusinessLayer.Contracts;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<object> GetAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            return await HandleResponse(response);
        }

        public async Task<object> PostAsync(string uri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            return await HandleResponse(response);
        }

        public async Task<object> PutAsync(string uri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);
            return await HandleResponse(response);
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }

        private async Task<object> HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonSerializer.Deserialize<object>(responseContent, _jsonSerializerOptions);
                }
                catch (JsonException)
                {
                    return responseContent;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
