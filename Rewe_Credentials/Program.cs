using JobSearcher_Queries;
using JobSearcher_Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Rewe_JobSearcher
{
    internal class Program
    {
        private static IServiceProvider ServiceProvider;
        static async Task Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/reportgenerator.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service...");
                ServiceProvider = ConfigureServices(args);
                await FindJobsAndSubmit();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed.");
            }
            finally
            {
                Log.CloseAndFlush();
            }


            // Your client credentials
            string clientId = "2f7680b4-35c2-45d9-8560-3e7af1be61fa";
            string clientSecret = "1855ed7e-88d5-4d13-8ab2-b40da734befa";

            // Combine the credentials into a single string
            string credentials = $"{clientId}:{clientSecret}";

            // Encode the credentials in Base64
            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            // The URL of the token endpoint
            string url = "https://dev.auth.rewe-group.at/v1/api/auth/token";

            // Create an HTTP client
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
                string accessToken = ExtractAccessToken(responseContent);

                await UseGet(accessToken);
            }
        }



        private static IServiceProvider ConfigureServices(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IQueriesRepository, QueriesRepository>();

            return services.BuildServiceProvider();
        }

        private static async Task<string> GetToken()
        {
            // Your client credentials
            string clientId = "2f7680b4-35c2-45d9-8560-3e7af1be61fa";
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

        private static async Task FindJobsAndSubmit()
        {
            string token = await GetToken();
            bool correct = false;
            do
            {
                Console.WriteLine("Would you like to read jobs advertisements [R] or submit [S]  for one");
                ConsoleKeyInfo cki = Console.ReadKey();

                if (cki.Key.ToString() == "R")
                {
                    correct = true;
                    await UseGet(token);
                }
                if (cki.Key.ToString() == "S")
                {
                    correct = true;
                    //await UsePost(token);
                }
                else
                {
                    Console.WriteLine("Invalid input please try again");
                }
            } while (!correct);


        }

        private static async Task UseGet(string token)
        {
            // The URL of the API endpoint
            string url = "https://dev.apply.rewe-group.at/v1/api/jobs/export";
            //string token = "E29D4B44F5752B86EC7ACC09BC4361B2";
            string _response = "";

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

                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();



                    _response = responseContent;
                    // Output the response content to the console
                    Console.WriteLine(responseContent);
                }
                catch (HttpRequestException e)
                {
                    // Handle request exceptions
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
        }

        private static string ExtractAccessToken(string jsonResponse)
        {
            // Parse the JSON response and extract the access_token
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;
                string accessToken = root.GetProperty("access_token").GetString();
                return accessToken;
            }
        }
    }
}
