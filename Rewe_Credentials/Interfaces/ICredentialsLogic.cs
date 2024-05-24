using JobSearcher_Queries.Models;

namespace Rewe_JobSearcher.Interfaces
{
    public interface ICredentialsLogic
    {
        Task<string> GetToken();
        Filter GetFilter();
        string ConvertToBase64(string path);
        public ApplicantDocument DocumentFiller();
    }
}
