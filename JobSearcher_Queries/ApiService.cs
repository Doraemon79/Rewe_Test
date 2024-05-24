using JobSearcher_Queries.Interfaces;
using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobSearcher_Queries
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //generic request method
        //
        public async Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string url, TRequest data, string authCode = null, string accessToken = null)
        {

            var request = new HttpRequestMessage(method, url);
            if (authCode != null)
            {
                request.Headers.Add("application-auth-code", authCode);
            }

            // Serialize the request data to JSON
            var json = JsonSerializer.Serialize(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Set authorization header if access token is provided
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            }

            // Make the HTTP POST request
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseJson))
            {
                Console.WriteLine($"Status code is:{response.StatusCode}");
                return default(TResponse);
            }
            var result = JsonSerializer.Deserialize<TResponse>(responseJson);
            return result;

        }

        // This method is used to post the credentials to the API
        public async Task<IActionResult> PostCredentials(string token, Credential credential)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/credentials");

            //dummy values for the recaptcha test, very annoying they do not allow me to use a generic call
            request.Headers.Add("ChallengeType", "recaptcha");
            request.Headers.Add("ChallengeKey", "123");

            // Example body content (if needed)
            var requestBody = new { Key = "Value" };
            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            // Set Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject jObj = JObject.Parse(responseContent);
                credential.Id = (int)jObj["id"];
                credential.AuthCode = jObj["authCode"].ToString();

                Console.WriteLine($"id is :  {credential.Id}");
                Console.WriteLine($"authCode is :  {credential.AuthCode}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return new OkObjectResult(response.Content);
        }

        public async Task<IActionResult> PostSearchAsync(string token, Filter filter)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/jobs/search");
            request.Headers.Add("application-auth-code", "recaptcha");

            // Add headers if needed
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // When the value is set to null by default the property is ignored this is used just for testing purposes
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            // Serialize the model to JSON and set the request content
            var jsonRequestBody = JsonSerializer.Serialize(filter);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            //get the response
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject jObj = JObject.Parse(responseContent);
                SearchResponse searchResponse = new SearchResponse();
                searchResponse.totalCount = jObj["totalCount"].ToObject<int>();
                searchResponse.jobs = jObj["jobs"].ToObject<List<Job>>();
                searchResponse.numberOfHits = jObj["numberOfHits"].ToObject<int>();
                Console.WriteLine($"Count of Jobs is:  {searchResponse.totalCount}");
                Console.WriteLine($"number of hits is:  {searchResponse.numberOfHits}");
                foreach (var el in searchResponse.jobs)
                {
                    Console.WriteLine($"Job is: {el.jobId}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new BadRequestResult();
            }
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> PostSubmit(string token, int applicationId, SubmitDetails submitDetails, string authCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + applicationId + "/submit");

            request.Headers.Add("application-auth-code", authCode);

            // Add headers if needed
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // Serialize the model to JSON and set the request content
            var jsonRequestBody = JsonSerializer.Serialize(submitDetails);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Success");
                var responseContent = await response.Content.ReadAsStringAsync();

                JObject jObj = JObject.Parse(responseContent);
                JToken valueToken = jObj["applicationDetails"]?["jobId"];

                JobApplicationResult jobApplicationResult = new JobApplicationResult();
                jobApplicationResult.JobId = valueToken?.ToString();
                Console.WriteLine($"Success your applicationfor JobId   {jobApplicationResult.JobId} has been submitted");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new BadRequestResult();
            }
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> PostDocuments(string token, int applicationId, ApplicantDocument document, string authCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + applicationId + "/documents");

            request.Headers.Add("application-auth-code", authCode);

            // Add headers if needed
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // Serialize the model to JSON and set the request content
            var jsonRequestBody = JsonSerializer.Serialize(document);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject jObj = JObject.Parse(responseContent);
                DocumentResponse documentResponse = new DocumentResponse();
                documentResponse.DocumentId = jObj["documentId"].ToObject<int>();
                documentResponse.DocumentType = jObj["documentType"].ToString();
                Console.WriteLine($"DocumentId  is saved:  {documentResponse.DocumentId}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new BadRequestResult();
            }
            return new OkObjectResult(response.Content);
        }

        public async Task<IActionResult> PutApplicantProfile(string token, int applicationId, string authCode)
        {

            Applicant applicant = ApplicantFiller();
            var request = new HttpRequestMessage(HttpMethod.Put, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + applicationId + "/applicant");

            request.Headers.Add("application-auth-code", authCode);

            // Add headers if needed
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // When the value is set to null by default the property is ignored
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            // Serialize the model to JSON and set the request content
            var jsonRequestBody = JsonSerializer.Serialize(applicant);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Your details have been saved");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new BadRequestResult();
            }

            return new OkObjectResult(response.StatusCode);
        }

        private Applicant ApplicantFiller()
        {
            var cultureInfo = new CultureInfo("de-DE");
            Applicant applicant = new Applicant();
            Console.WriteLine();
            Console.WriteLine("Please write your family name ");
            applicant.lastName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please write your Nationality code (3 letters Ex: ITA, AUT. Default is AUT) ");
            applicant.nationality = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please write your country code  code (3 letters Ex: +39, +43Default is +43) ");
            applicant.nationality = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please write your bith date ");
            string date = Console.ReadLine();
            applicant.birthDate = DateTime.Parse(date, cultureInfo);
            return applicant;
        }
    }
}


