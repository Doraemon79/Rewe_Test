using JobSearcher_Queries.Models;

namespace Rewe_JobSearcher.Interfaces
{
    public interface ICredentialsLogic
    {
        Task<string> GetToken();
        Filter GetFilter();
    }
}
