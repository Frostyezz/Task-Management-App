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

        public async Task<IApiResponse<object>> GetAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            return await HandleResponse(response);
        }

        public async Task<IApiResponse<object>> PostAsync(string uri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            return await HandleResponse(response);
        }

        public async Task<IApiResponse<object>> PutAsync(string uri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);
            return await HandleResponse(response);
        }

        public async Task<IApiResponse<object>> DeleteAsync(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            return await HandleResponse(response);
        }

        private async Task<IApiResponse<object>> HandleResponse(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse<object>
            {
                IsSuccess = response.IsSuccessStatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    apiResponse.Data = JsonSerializer.Deserialize<object>(responseContent, _jsonSerializerOptions);
                }
                catch (JsonException)
                {
                    apiResponse.Data = responseContent;
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
