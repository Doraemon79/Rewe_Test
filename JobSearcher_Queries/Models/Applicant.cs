namespace JobSearcher_Queries.Models
{
    public class Applicant
    {
        public string titleCode { get; set; } = "";
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public Gender gender { get; set; } = Gender.Female;
        public string nationality { get; set; } = "Austrian";
        public string telephoneNumber { get; set; } = "";
        public string email { get; set; } = "";
        public DateTime birthDate { get; set; } = DateTime.Now;
        public string countryCode { get; set; } = "";
        public string zip { get; set; } = "";
        public string city { get; set; } = "";
        public string street { get; set; } = "";
        public bool workPermit { get; set; } = false;
        public string driversLincenseClasses { get; set; } = "A";
        public bool employedBefore { get; set; } = false;
        public bool militaryServiceFinished { get; set; } = true;
    }
}
