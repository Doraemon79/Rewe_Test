namespace JobSearcher_Queries.Models
{
    public class JobDescriptionResponse
    {
        public string jobDescriptionId { get; set; }
        public string accountingCompany { get; set; }
        public int jobType { get; set; }
        public int jobNumber { get; set; }
        public int paymentClass { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public List<string> functions { get; set; }
        public List<string> skills { get; set; }
        public List<string> offers { get; set; }
        public string functionsTitle { get; set; }
        public string skillsTitle { get; set; }
        public string offersTitle { get; set; }

        public string paymentInfo { get; set; }
        public string paymentInfoAddition { get; set; }

    }
}
