namespace JobSearcher_Queries.Models
{
    public class SubmitDetails
    {
        public string jobId { get; set; } = "0";
        public List<int> storeIdList { get; set; } = new List<int>();
        public int desiredSalary { get; set; } = 0;
        public string recommendedBy { get; set; } = "0";
        public DateTime availableFrom { get; set; } = DateTime.Now;
        public bool agreedToDataProcessing { get; set; } = false;
        public bool agreedToDataRelaying { get; set; } = false;
        public string externalSource { get; set; } = "0";
        public string externalDestination { get; set; } = "0";
    }
}
