using JobSearcher_Queries.Interfaces;
using JobSearcher_Queries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace JobSearcher_Queries
{

    public class QueriesRepository : IQueriesRepository
    {
        private readonly HttpClient _httpClient;

        public QueriesRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<ActionResult> Applicant(string token, int applicationId, ApplicantDetails applicantDetails)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<DocumentResponse>> Documents(string token, int applicationId, Document document)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<ExportResponse>> Export(string token)
        {
            // The URL of the API endpoint
            string url = "https://dev.apply.rewe-group.at/v1/api/jobs/export";
            ExportResponse exportResponse = null;

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    // Set the Content-Type header
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Set the Authorization header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new StatusCodeResult((int)response.StatusCode);
                    }
                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    try
                    {
                        exportResponse = JsonSerializer.Deserialize<ExportResponse>(jsonResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    catch (JsonException)
                    {
                        return new BadRequestResult();
                    }
                    if (exportResponse == null)
                    {
                        return new NotFoundResult();
                    }
                    // Output the response content to the console
                    //return new OkObjectResult(exportResponse);
                }
                catch (HttpRequestException e)
                {
                    // Handle request exceptions
                    Console.WriteLine($"Request error: {e.Message}");
                }

            }
            return new OkObjectResult(exportResponse);
        }

        public async Task<ActionResult<JobDescription>> GetJobDescriptionId(string token, string jobDescriptionId)
        {
            string url = "https://dev.apply.rewe-group.at/v1/api/jobs/" + jobDescriptionId;
            List<JobDescription> jobDescriptions = new List<JobDescription>();
            jobDescriptions = new List<JobDescription>();

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    // Set the Content-Type header
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Set the Authorization header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new StatusCodeResult((int)response.StatusCode);
                    }
                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    try
                    {
                        jobDescriptions = JsonSerializer.Deserialize<List<JobDescription>>(jsonResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    catch (JsonException)
                    {
                        return new BadRequestResult();
                    }
                    if (jobDescriptions == null)
                    {
                        return new NotFoundResult();
                    }
                    // Output the response content to the console
                    //return new OkObjectResult(exportResponse);
                }
                catch (HttpRequestException e)
                {
                    // Handle request exceptions
                    Console.WriteLine($"Request error: {e.Message}");
                }

            }
            return new OkObjectResult(jobDescriptions);
        }

        public async Task<ActionResult<SearchResponse>> Search(string token, SearchFilter filter)
        {
            string jsonRequest = JsonSerializer.Serialize(filter);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the Content-Type header
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Set the Authorization header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                catch (HttpRequestException e)
                {
                    // Handle request exceptions
                    Console.WriteLine($"Request error: {e.Message}");
                }

                throw new NotImplementedException();
            }
        }

        public async Task<ActionResult> Submit(string token, int applicationId, SubmitDetails submitDetails)
        {
            string jsonRequest = JsonSerializer.Serialize(submitDetails);
            throw new NotImplementedException();
        }
    }
}
