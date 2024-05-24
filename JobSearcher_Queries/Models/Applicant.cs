using System.Text.Json.Serialization;

namespace JobSearcher_Queries.Models
{
    public class Applicant
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string titleCode { get; set; } = null;
        public string firstName { get; set; } = "Iggy";
        public string lastName { get; set; } = null;
        public Gender gender { get; set; } = Gender.Female;
        public string nationality { get; set; } = "AUT";
        public string telephoneNumber { get; set; } = "123456";
        public string email { get; set; } = "fkeemail";
        public DateTime birthDate { get; set; } = DateTime.Now.AddYears(-18);
        public string countryCode { get; set; } = "+43";
        public string zip { get; set; } = "1060";
        public string city { get; set; } = "Wien";
        public string street { get; set; } = "mystreet";
        public bool workPermit { get; set; } = true;
        public string driversLincenseClasses { get; set; } = "A";
        public bool employedBefore { get; set; } = true;
        public bool militaryServiceFinished { get; set; } = true;
    }
}
