using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher_Queries.Interfaces
{
    public interface IApiService
    {
        Task<ActionResult<Credential>> PostCredentials(string token, Credential credential);
        Task<ActionResult<SearchResponse>> PostSearchAsync(string token, Filter filter);
        Task<ActionResult<JobApplicationResult>> Submit(string token, string applicationId, SubmitDetails submitDetails, string authCode);
        Task<ActionResult<DocumentResponse>> Documents(string token, int applicationId, Document document, string authCode);
    }
}
