namespace JobSearcher_Queries.Models
{
    public class JobWithDetailedDescriptionResponse
    {
        public Job job { get; set; }
        public JobDescriptionResponse description { get; set; }


        /**
         * "job": {
    "jobId": "string",
    "jobDescriptionId": "string",
    "accountingCompanyId": "string",
    "accountingCompany": "string",
    "displayAccountingCompanyId": "string",
    "displayAccountingCompany": "string",
    "jobType": "string",
    "jobTypeDescription": "string",
    "title": "string",
    "shortDescription": "string",
    "employmentLevelId": "G",
    "employmentLevel": "string",
    "jobNumber": 0,
    "paymentClass": 0,
    "jobGroups": [
      {
        "jobGroupId": 0,
        "name": "string",
        "subGroups": [
          {}
        ]
      }
    ],
    "countryCode": "string",
    "zip": "string",
    "city": "string",
    "provinceId": "string",
    "provinceNumber": 0,
    "provinceName": "string",
    "districtId": "string",
    "districtNumber": 0,
    "districtName": "string",
    "startDate": "2024-05-24T17:01:50.485Z",
    "creationDate": "2024-05-24T17:01:50.485Z",
    "hours": 0,
    "minHours": 0,
    "maxHours": 0,
    "amountOfJobs": 0,
    "stores": [
      {
        "id": 0,
        "street": "string",
        "amountOfJobs": 0
      }
    ],
    "links": [
      {
        "name": "string",
        "method": "string",
        "href": "string"
      }
    ],
    "synonyms": [
      "string"
    ],
    "jobLevels": [
      "string"
    ]
  },
  "description": {
    "jobDescriptionId": "string",
    "accountingCompany": "string",
    "jobType": 0,
    "jobNumber": 0,
    "paymentClass": 0,
    "name": "string",
    "shortDescription": "string",
    "description": "string",
    "functions": [
      "string"
    ],
    "skills": [
      "string"
    ],
    "offers": [
      "string"
    ],
    "functionsTitle": "string",
    "skillsTitle": "string",
    "offersTitle": "string",
    "paymentInfo": "string",
    "paymentInfoAddition": "string"
  } 
         */
    }
}
