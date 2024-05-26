namespace JobSearcher_Queries.Models
{
    public class JobWithDetailedDescriptionResponse
    {
        public Job job { get; set; }
        public JobDescriptionResponse description { get; set; }
    }
}
