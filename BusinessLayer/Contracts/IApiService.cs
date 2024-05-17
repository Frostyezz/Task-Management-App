namespace BusinessLayer.Contracts
{
    public interface IApiService
    {
        Task<IApiResponse<object>> GetAsync(string uri);
        Task<IApiResponse<object>> PostAsync(string uri, object data);
        Task<IApiResponse<object>> PutAsync(string uri, object data);
        Task<IApiResponse<object>> DeleteAsync(string uri);
    }
}
