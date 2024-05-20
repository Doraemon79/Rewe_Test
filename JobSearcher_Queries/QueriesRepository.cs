using JobSearcher_Queries.Interfaces;
using System.Net.Http.Headers;

namespace JobSearcher_Queries
{
    public class QueriesRepository : IQueriesRepository
    {

        public async Task UseGet(string token)
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
    }
}
