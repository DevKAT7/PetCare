namespace PetCare.MobileApp.Services
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string endpoint);

        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);

        //Post bez zwracania wyniku
        Task PostAsync<TRequest>(string endpoint, TRequest data);
    }
}
