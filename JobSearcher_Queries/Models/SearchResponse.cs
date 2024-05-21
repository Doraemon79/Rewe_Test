namespace JobSearcher_Queries.Models
{
    public class SearchResponse
    {
        public int numberOfHits { get; set; }
        public int totalCount { get; set; }
        public List<Job> jobs { get; set; }

    }
}
