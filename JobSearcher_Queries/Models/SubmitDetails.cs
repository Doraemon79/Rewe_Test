namespace JobSearcher_Queries.Models
{
    public class SubmitDetails
    {
        public string jobId { get; set; }
        public List<int> storeIdList { get; set; }
        public int desiredSalary { get; set; }

        public string recommendedBy { get; set; }
        public DateTime availableFrom { get; set; }
        public bool agreedToDataProcessing { get; set; }
        public bool agreedToDataRelaying { get; set; }
        public string externalSource { get; set; }
        public string externalDestination { get; set; }
    }
}
