using JobSearcher_Queries.Models;

namespace Rewe_JobSearcher.Interfaces
{
    public interface ICredentialsLogic
    {
        public ApplicantDocument DocumentFiller();
        public Applicant ApplicantFiller();
        public void ShowJobs(SearchResponse response);
        void ShowSubmitResponse(SubmitResponse response);
        void ShowDocumentsResponse(DocumentResponse documentsResponse);
        void ShowJobDescriptionResponse(JobDescriptionResponse jobDescriptionResponse);
        void ShowJobWithDetailedDescriptionResponse(JobWithDetailedDescriptionResponse jobWithDetailedDescriptionResponse);
    }
}
