using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher_Queries.Interfaces
{
    public interface IQueriesRepository
    {
        Task<ActionResult<ExportResponse>> Export(string token);
        Task<ActionResult<SearchResponse>> Search(string token, SearchFilter filter);
        Task<ActionResult<JobDescription>> GetJobDescriptionId(string token, string jobDescriptionId);
        Task<ActionResult> Submit(string token, int applicationId, SubmitDetails submitDetails);
        Task<ActionResult> Applicant(string token, int applicationId, ApplicantDetails applicantDetails);
        Task<ActionResult<DocumentResponse>> Documents(string token, int applicationId, Document document);

    }
}
