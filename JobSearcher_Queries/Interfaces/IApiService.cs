using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher_Queries.Interfaces
{
    public interface IApiService
    {
        Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string url, TRequest data, string authCode = null, string accessToken = null);
        Task<IActionResult> PostCredentials(string token, Credential credential);
        Task<IActionResult> SendDeleteAsync<TRequest>(string url, TRequest data, string authCode = null, string accessToken = null);
    }
}
