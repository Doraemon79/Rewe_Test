using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher_Queries.Interfaces
{
    public interface IApiService
    {
        Task<IActionResult> PostCredentials(string token, Credential credential);
        Task<IActionResult> PostSearchAsync(string token, Filter filter);
        Task<IActionResult> PostSubmit(string token, int applicationId, SubmitDetails submitDetails, string authCode);
        Task<IActionResult> PostDocuments(string token, int applicationId, ApplicantDocument document, string authCode);
        Task<IActionResult> PutApplicantProfile(string token, int applicationId, string authCode);
    }
}
