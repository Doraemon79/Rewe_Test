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

            // Make the HTTP  request
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine($"Status code is:{response.StatusCode}");
            var responseJson = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TResponse>(responseJson);
            return result;
        }

        //Delete Request for API
        //This can be made specific but inorder to prepare for future requests, it is made generic
        public async Task<IActionResult> SendDeleteAsync<TRequest>(string url, TRequest data, string authCode = null, string accessToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
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

            // Make the HTTP  request
            var response = await _httpClient.SendAsync(request);


            return response.StatusCode == System.Net.HttpStatusCode.NoContent
                ? new OkObjectResult(response.Content)
                : new BadRequestObjectResult(response.Content);
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
    }
}


