namespace BusinessLayer.Contracts
{
    public interface IApiResponse<T>
    {
        T Data { get; set; }
        bool IsSuccess { get; set; }
        string ErrorMessage { get; set; }
    }

}
