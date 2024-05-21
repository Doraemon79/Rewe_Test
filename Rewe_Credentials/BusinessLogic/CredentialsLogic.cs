using JobSearcher_Queries.Models;
using Rewe_JobSearcher.Interfaces;
using System.Text;
using System.Text.Json;

namespace Rewe_JobSearcher.BusinessLogic
{
    public class CredentialsLogic : ICredentialsLogic
    {
        public async Task<string> GetToken()
        {
            // Your client credentials
            Console.WriteLine("Please write your client-Id");
            var clientId = Console.ReadLine();
            //string clientId = "2f7680b4-35c2-45d9-8560-3e7af1be61fa";
            Console.WriteLine("Please write your client-Secret");
            string clientSecret = Console.ReadLine();
            string accessToken = null;

            // Combine the credentials into a single string
            string credentials = $"{clientId}:{clientSecret}";

            // Encode the credentials in Base64
            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            // The URL of the token endpoint
            string url = "https://dev.auth.rewe-group.at/v1/api/auth/token";

            // Create an HTTP client
            using (HttpClient client = new HttpClient())
            {
                // Set the Authorization header with the encoded credentials
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedCredentials);

                // The data to send in the request body
                var requestData = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                // Make the POST request
                HttpResponseMessage response = await client.PostAsync(url, requestData);

                // Read the response
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                Console.ReadLine();
                accessToken = ExtractAccessToken(responseContent);

            }
            return accessToken;
        }


        public Filter GetFilter()
        {
            Filter filter = new Filter();
            filter.AccountingCompanyId = "string";
            filter.AccountingCompanyIds = new List<string> { "string" };
            filter.JobGroupIds = new List<int> { 0 };
            filter.SubJobGroupIds = new List<int> { 0 };
            filter.JobTypeIds = new List<int> { 0 };
            filter.ProvinceIdList = new List<string> { "string" };
            filter.DistrictIdList = new List<string> { "string" };
            filter.EmploymentLevelId = "G";
            filter.SearchTerm = "";
            filter.JobLevels = new List<string> { "string" };
            filter.JobDescriptionId = "string";
            filter.CityList = new List<string> { "string" };
            filter.Zip = "string";
            filter.MinWorkingHours = 0;
            filter.MaxWorkingHours = 0;
            filter.Offset = 0;
            filter.Limit = 0;
            filter.SortField = "Relevancy";
            filter.SortDirection = "Ascending";
            filter.IncludeInternal = true;

            return filter;
        }


        private string ExtractAccessToken(string jsonResponse)
        {
            // Parse the JSON response and extract the access_token
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;
                return root.GetProperty("access_token").GetString();
            }
        }
    }
}
