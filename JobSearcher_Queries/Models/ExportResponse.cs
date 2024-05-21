namespace JobSearcher_Queries.Models
{
    public class ExportResponse
    {
        public string id { get; set; }
        public string title { get; set; }
        public Company company { get; set; }
        public Location location { get; set; }
        public List<List<object>> content { get; set; }
        public string url { get; set; }
    }
}
