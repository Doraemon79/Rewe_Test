namespace JobSearcher_Queries.Models
{
    public class JobGroups
    {
        public int jobGroupId { get; set; }
        public string name { get; set; }
        public List<JobGroups> subGroups { get; set; }

    }
}
