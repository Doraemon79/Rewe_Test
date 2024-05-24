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
            //Console.WriteLine("Please write your client-Id");
            //var clientId = Console.ReadLine();
            string clientId = "2f7680b4-35c2-45d9-8560-3e7af1be61fa";
            //Console.WriteLine("Please write your client-Secret");
            //string clientSecret = Console.ReadLine();
            string clientSecret = "1855ed7e-88d5-4d13-8ab2-b40da734befa";
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
            //this filler has been made in a simplified form for testing purposes
            Filter filter = new Filter();
            //filter.accountingCompanyId = "";
            //filter.AccountingCompanyIds = new List<string> { ""};
            //filter.JobGroupIds = new List<int> { 0 };
            //filter.SubJobGroupIds = new List<int> { 0 };
            //filter.JobTypeIds = new List<int> { 0 };
            //filter.ProvinceIdList = new List<string> { "" };
            //filter.DistrictIdList = new List<string> { "" };
            //filter.employmentLevelId = "V";
            //filter.searchTerm = "";
            //filter.JobLevels = new List<string> { "" };
            //filter.jobDescriptionId = "";
            //filter.CityList = new List<string> { "" };
            filter.zip = "1200";
            //filter.minWorkingHours = 0;
            //filter.maxWorkingHours = 0;
            //filter.offset = 0;
            //filter.limit = 0;
            //filter.sortField = "Relevancy";
            //filter.sortDirection = "Ascending";
            //filter.includeInternal = true;

            return filter;
        }

        public string ConvertToBase64(string path)
        {
            Byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        public ApplicantDocument DocumentFiller()
        {
            ApplicantDocument document = new ApplicantDocument();
            Console.WriteLine();
            Console.WriteLine("Please insert the type of the document (Cv, MotivationalLetter, Foto, Misc, GradeSheet)");
            document.documentType = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please insert the path of the document");
            var documentPath = Console.ReadLine();
            if (File.Exists(documentPath))
            {
                if ((document.documentType == "Foto" && (!documentPath.EndsWith(".jpg") && !documentPath.EndsWith(".bmp") && !documentPath.EndsWith(".png") && !documentPath.EndsWith(".pdf"))) ||
                    (document.documentType == "Cv" && (!documentPath.EndsWith(".pdf") && !documentPath.EndsWith(".doc") && !documentPath.EndsWith(".docx"))) ||
                    (document.documentType == "MotivationalLetter" && (!documentPath.EndsWith(".pdf") && !documentPath.EndsWith(".doc") && !documentPath.EndsWith(".docx"))))
                {
                    Console.WriteLine("Invalid document name");
                }
                else
                {
                    document.documentName = Path.GetFileName(documentPath);
                }
                document.documentBlob = ConvertToBase64(documentPath);
            }
            else
            {
                Console.WriteLine("Invalid path");
            }
            return document;
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
