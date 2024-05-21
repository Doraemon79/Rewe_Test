namespace JobSearcher_Queries.Models
{
    public class Filter
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

        public string sortField { get; set; }
        public string sortDirection { get; set; }
        public bool includeInternal { get; set; }
        public bool excludeInternal { get; set; }
        public bool excludeExternal { get; set; }
        /*
         * "accountingCompanyId": "string",
  "accountingCompanyIds": [
    "string"
  ],
  "jobGroupIds": [
    0
  ],
  "subJobGroupIds": [
    0
  ],
  "jobTypeIds": [
    0
  ],
  "provinceIdList": [
    "string"
  ],
  "districtIdList": [
    "string"
  ],
  "employmentLevelId": "G",
  "searchTerm": "string",
  "jobLevels": [
    "string"
  ],
  "jobDescriptionId": "string",
  "cityList": [
    "string"
  ],
  "zip": "string",
  "minWorkingHours": 0,
  "maxWorkingHours": 0,
  "offset": 0,
  "limit": 0,
  "sortField": "Relevancy",
  "sortDirection": "Ascending",
  "includeInternal": true
         */
    }
}
