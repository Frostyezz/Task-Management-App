using BusinessLayer.Contracts;

namespace BusinessLayer.Types
{
    public class ApiResponse<T> : IApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
