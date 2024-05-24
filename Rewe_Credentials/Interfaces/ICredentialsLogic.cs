using JobSearcher_Queries.Models;

namespace Rewe_JobSearcher.Interfaces
{
    public interface ICredentialsLogic
    {
        Task<string> GetToken();
        Filter GetFilter();
        string ConvertToBase64(string path);
        public ApplicantDocument DocumentFiller();
        public Applicant ApplicantFiller();
        public void ShowJobs(SearchResponse response);
        void ShowSubmitResponse(SubmitResponse response);
        void ShowDocumentsResponse(DocumentResponse documentsResponse);
    }
}
