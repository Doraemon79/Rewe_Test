namespace JobSearcher_Queries.Models
{
    public class JobApplicationResult
    {
        public string JobId { get; set; } = "";
        public List<int> StoreIdList { get; set; } = new List<int>();
        public int DesiredSalary { get; set; } = 0;
        public string RecommendedBy { get; set; } = "";
        public DateTime AvailableFrom { get; set; } = DateTime.Now;
        public bool AgreedToDataProcessing { get; set; } = false;
        public bool AgreedToDataRelaying { get; set; } = false;
        public string ExternalSource { get; set; } = "";
        public int Id { get; set; } = 0;
        public Applicant Applicant { get; set; } = new Applicant();
        public List<ApplicantDocument> Documents { get; set; } = new List<ApplicantDocument>();

    }
}
