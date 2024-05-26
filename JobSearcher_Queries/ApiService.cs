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

        //Generic request method

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

            try
            {
                // Make the HTTP  request
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Check if the response has content
                    if (response.Content != null)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            // Deserialize response content
                            return JsonSerializer.Deserialize<TResponse>(responseContent);
                        }
                    }

                    // If response has no content or is null, return default(TResponse)
                    return default;
                }
                else
                {
                    // Handle various HTTP status codes
                    return HandleErrorResponse<TResponse>(response);
                }
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization error
                throw new JsonException($"JSON deserialization error: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request error
                throw new HttpRequestException($"HTTP request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        //Generic Delete method request for API
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

        public async Task<string> GetToken(string url, string clientId, string clientSecret)
        {
            string accessToken = null;
            // Combine the credentials into a single string
            string credentials = $"{clientId}:{clientSecret}";

            // Encode the credentials in Base64
            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

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

                //Console.ReadLine();
                accessToken = ExtractAccessToken(responseContent);
            }
            return accessToken;
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

        //Generic error handler to avoid code stops with common errors
        private TResponse HandleErrorResponse<TResponse>(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            switch (statusCode)
            {
                case System.Net.HttpStatusCode.BadRequest: // 400
                    Console.WriteLine($"Bad Request: {responseContent}");
                    break;
                case System.Net.HttpStatusCode.Unauthorized: // 401
                    Console.WriteLine($"Unauthorized: {responseContent}");
                    break;
                case System.Net.HttpStatusCode.Forbidden: // 403
                    Console.WriteLine($"Forbidden: {responseContent}");
                    break;
                case System.Net.HttpStatusCode.NotFound: // 404
                    Console.WriteLine($"Not Found: {responseContent}");
                    break;
                case System.Net.HttpStatusCode.Conflict: // 409
                    Console.WriteLine($"Conflict: {responseContent}");
                    break;
                case System.Net.HttpStatusCode.InternalServerError: // 500
                    Console.WriteLine($"Internal Server Error: {responseContent}");
                    break;
                default:
                    Console.WriteLine($"HTTP error {statusCode}: {responseContent}");
                    break;
            }

            // Optionally, you can return a specific error response or default value
            return default;
        }

    }
}


