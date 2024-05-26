namespace JobSearcher_Queries.Models
{
    public class Filter
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
        //public double minWorkingHours { get; set; } = 0.0;
        //public double maxWorkingHours { get; set; } = 0.0;
        //public int offset { get; set; } = 0;
        //public int limit { get; set; } = 0;
        public string sortField { get; set; } = null;
        public string sortDirection { get; set; } = null;
        //public bool includeInternal { get; set; } = true;
    }
}
