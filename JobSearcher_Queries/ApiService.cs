using JobSearcher_Queries.Interfaces;
using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JobSearcher_Queries
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ActionResult<Credential>> PostCredentials(string token, Credential credential)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/credentials");

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
                credential.Id = jObj["id"].ToObject<int>();
                credential.AuthCode = jObj["authCode"].ToString();

                Console.WriteLine($"id is :  {credential.id}");
                Console.WriteLine($"authCode is :  {credential.AuthCode}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return new OkObjectResult(response);
        }

        public async Task<ActionResult<SearchResponse>> PostSearchAsync(string token, Filter filter)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/jobs/search");
            request.Headers.Add("application-auth-code", "recaptcha");

            // Add headers if needed
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // Serialize the model to JSON and set the request content
            var jsonRequestBody = JsonSerializer.Serialize(filter);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

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
            return new OkObjectResult(response.Content);
        }

        public async Task<ActionResult<JobApplicationResult>> Submit(string token, string applicationId, SubmitDetails submitDetails, string authCode)
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
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject jObj = JObject.Parse(responseContent);
                JobApplicationResult jobApplicationResult = new JobApplicationResult();
                jobApplicationResult.JobId = jObj["jobId"].ToString();
                jobApplicationResult.DesiredSalary = jObj["desiredSalary"].ToObject<int>();
                Console.WriteLine($"JobId  is:  {jobApplicationResult.JobId}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new BadRequestResult();
            }
            return new OkObjectResult(response.Content);
        }


        public async Task<ActionResult<DocumentResponse>> Documents(string token, int applicationId, Document document, string authCode)
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
    }
}


