using BusinessLayer.Contracts;
using System.Text;
using System.Text.Json;
using BusinessLayer.Types;

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

        public async Task<ApiResponse<T>> GetAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            return await HandleResponse<T>(response);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string uri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            return await HandleResponse<T>(response);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string uri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);
            return await HandleResponse<T>(response);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            return await HandleResponse<T>(response);
        }

        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse<T>
            {
                IsSuccess = response.IsSuccessStatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string))
                {
                    apiResponse.Data = (T)(object)responseContent;
                }
                else
                {
                    try
                    {
                        apiResponse.Data = JsonSerializer.Deserialize<T>(responseContent, _jsonSerializerOptions);
                    }
                    catch (JsonException)
                    {
                        apiResponse.Data = default;
                    }
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(errorContent))
                {
                    apiResponse.ErrorMessage = errorContent;
                }
            }

            return apiResponse;
        }
    }
}
