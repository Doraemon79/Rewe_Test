namespace JobSearcher_Queries.Models
{
    public class SearchFilter
    {
        public string accountingCompanyId { get; set; } = null;
        public List<string> accountingCompanyIds { get; set; } = null;
        public List<int> jobGroupIds { get; set; } = null;
        public List<int> subJobGroupIds { get; set; } = null;
        public List<int> jobTypeIds { get; set; } = null;
        public List<string> provinceIdList { get; set; } = null;
        public List<string> districtIdList { get; set; } = null;
        public string employmentLevelId { get; set; } = null;
        public string searchTerm { get; set; } = null;
        public List<string> jobLevels { get; set; } = null;
        public string jobDescriptionId { get; set; } = null;
        public List<string> cityList { get; set; } = null;
        public string zip { get; set; } = null;
        //public int minWorkingHours { get; set; }=null;
        //public int maxWorkingHours { get; set; }=null;
        //public int offset { get; set; }
        //public int limit { get; set; }
        //public Relevancy sortField { get; set; } ;
        //public SortDirection sortDirection { get; set; }
        //public bool includeInternal { get; set; } 

    }
}
