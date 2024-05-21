namespace JobSearcher_Queries.Models
{
    public class SearchFilter
    {
        public string accountingCompanyId { get; set; }
        public List<string> accountingCompanyIds { get; set; }
        public List<int> jobGroupIds { get; set; }
        public List<int> subJobGroupIds { get; set; }
        public List<int> jobTypeIds { get; set; }
        public List<string> provinceIdList { get; set; }
        public List<string> districtIdList { get; set; }
        public string employmentLevelId { get; set; }
        public string searchTerm { get; set; }
        public List<string> jobLevels { get; set; }
        public string jobDescriptionId { get; set; }
        public List<string> cityList { get; set; }
        public string zip { get; set; }
        public int minWorkingHours { get; set; }
        public int maxWorkingHours { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public Relevancy sortField { get; set; }
        public SortDirection sortDirection { get; set; }
        public bool includeInternal { get; set; }

    }
}
