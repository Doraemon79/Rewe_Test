using System.ComponentModel.DataAnnotations;

namespace JobSearcher_Queries.Models
{
    public class ApplicantDocument
    {
        [Required]
        [AllowedValues("Cv", "MotivationalLetter", "Foto", "Misc", "GradeSheet")]
        public string documentType { get; set; } = "Cv";
        public string documentName { get; set; }
        public string documentBlob { get; set; }

    }
}
