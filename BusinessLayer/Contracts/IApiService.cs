using BusinessLayer.Types;

namespace BusinessLayer.Contracts
{
    public interface IApiService
    {
        Task<ApiResponse<T>> GetAsync<T>(string uri);
        Task<ApiResponse<T>> PostAsync<T>(string uri, object data);
        Task<ApiResponse<T>> PutAsync<T>(string uri, object data);
        Task<ApiResponse<T>> DeleteAsync<T>(string uri);
    }
}
