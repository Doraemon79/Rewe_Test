namespace JobSearcher_Queries.Models
{
    public class ApplicantDetails
    {
        public string titleCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Gender gender { get; set; }
        public string nationality { get; set; }
        public string telephoneNumber { get; set; }
        public string email { get; set; }
        public DateTime birthDate { get; set; }
        public string countryCode { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public bool workPermit { get; set; }
        public string driversLincenseClasses { get; set; }
        public bool employedBefore { get; set; }
        public bool militaryServiceFinished { get; set; }

    }
}
