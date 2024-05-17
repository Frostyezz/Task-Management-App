namespace BusinessLayer.Contracts
{
    public interface IApiService
    {
        Task<object> GetAsync(string uri);
        Task<object> PostAsync(string uri, object data);
        Task<object> PutAsync(string uri, object data);
        Task<bool> DeleteAsync(string uri);
    }
}
