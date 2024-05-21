namespace JobSearcher_Queries.Models
{
    public class Job
    {
        public string jobId { get; set; }
        public string jobDescriptionId { get; set; }

        public string accountingCompanyId { get; set; }
        public string accountingCompany { get; set; }
        public string displayAccountingCompanyId { get; set; }
        public string displayAccountingCompany { get; set; }
        public string displayCompanyId { get; set; }
        public string displayCompany { get; set; }

        public string jobType { get; set; }

        public string jobTypeDescription { get; set; }
        public string title { get; set; }
        public string shortDescription { get; set; }
        public string employmentLevelId { get; set; }
        public string employmentLevel { get; set; }
        public int jobNumber { get; set; }
        public int paymentClass { get; set; }
        public List<JobGroups> jobGroups { get; set; }
        public string countryCode { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string provinceId { get; set; }
        public int provinceNumber { get; set; }
        public string provinceName { get; set; }
        public string districtId { get; set; }
        public int districtNumber { get; set; }
        public string districtName { get; set; }
        public string startDate { get; set; }
        public string creationDate { get; set; }
        public int hours { get; set; }
        public int minHours { get; set; }
        public int maxHours { get; set; }
        public int amountOfJobs { get; set; }
        public List<Stores> stores { get; set; }
        public List<Links> links { get; set; }
        public List<string> synonyms { get; set; }
        public List<string> jobLevels { get; set; }
    }
}
