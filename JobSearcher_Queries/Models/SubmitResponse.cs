namespace JobSearcher_Queries.Models
{
    public class SubmitResponse
    {
        public JobApplicationResult applicationDetails { get; set; }
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public List<Documents> documents { get; set; }
    }
}
