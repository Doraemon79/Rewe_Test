namespace JobSearcher_Queries.Models
{
    public class Credential
    {
        internal int id;

        public int Id { get; set; } = 0;
        public string AuthCode { get; set; } = "";
        public string ExpirationTimestamp { get; set; } = "";

    }
}
