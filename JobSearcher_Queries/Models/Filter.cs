namespace JobSearcher_Queries.Models
{
    public class Filter
    {
        public string AccountingCompanyId { get; set; } = "";
        public List<string> AccountingCompanyIds { get; set; } = new List<string>();
        public List<int> JobGroupIds { get; set; } = new List<int>();
        public List<int> SubJobGroupIds { get; set; } = new List<int>();
        public List<int> JobTypeIds { get; set; } = new List<int>();
        public List<string> ProvinceIdList { get; set; } = new List<string>();
        public List<string> DistrictIdList { get; set; } = new List<string>();
        public string EmploymentLevelId { get; set; } = "";
        public string SearchTerm { get; set; } = "";
        public List<string> JobLevels { get; set; } = new List<string>();
        public string JobDescriptionId { get; set; } = "";
        public List<string> CityList { get; set; } = new List<string>();
        public string Zip { get; set; } = "";
        public int MinWorkingHours { get; set; } = 0;
        public int MaxWorkingHours { get; set; } = 48;
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public string SortField { get; set; } = "";
        public string SortDirection { get; set; } = "";
        public bool IncludeInternal { get; set; } = true;
    }
}
