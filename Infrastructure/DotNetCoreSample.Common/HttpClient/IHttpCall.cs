using  AuthService.Common.GenericResponses;

namespace  AuthService.Common.HttpClient
{
    public interface IHttpCall
    {
        Task<GenericApiResponse<T>> Get<T>(string url, string ClientType);
        Task<GenericApiResponse<T>> Post<T, P>(string url, string ClientType, string bearerToken, P jsonPayload);
        //Task<GenericApiResponse<T>> PostFormDataAsync<T, P>(string url, string ClientType, string token, P data);
        Task<GenericApiResponse<T>> PostForm<T, P>(string url, string ClientType, string bearerToken, P jsonPayload);
    }
}
