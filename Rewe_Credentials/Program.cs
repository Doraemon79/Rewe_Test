using JobSearcher_Queries;
using JobSearcher_Queries.Interfaces;
using JobSearcher_Queries.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rewe_JobSearcher.BusinessLogic;
using Rewe_JobSearcher.Interfaces;
using Serilog;

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
                var host = CreateHostBuilder(args).Build();

                // Resolve the service and use it
                var apiService = host.Services.GetRequiredService<IApiService>();
                var CredentialsLogicService = host.Services.GetRequiredService<ICredentialsLogic>();

                string token = CredentialsLogicService.GetToken().Result;
                Credential credential = new Credential();
                await apiService.PostCredentials(token, credential);
                bool correct = false;
                do
                {

                    Console.WriteLine("Would you like to read jobs advertisements [R], submit [S], fill your profile [F], load document in your profile [D] or Exit [K]");
                    ConsoleKeyInfo cki = Console.ReadKey();
                    Console.WriteLine();

                    if (cki.Key.ToString() == "R")
                    {
                        Filter filter = CredentialsLogicService.GetFilter();
                        var response = await apiService.SendAsync<Filter, SearchResponse>(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/jobs/search", filter, null, token);
                        CredentialsLogicService.ShowJobs(response);
                    }
                    if (cki.Key.ToString() == "S")
                    {
                        var submitRequest = new SubmitRequest();
                        Console.WriteLine("Please insert the JobId");
                        submitRequest.jobId = Console.ReadLine();
                        submitRequest.Id = credential.Id;

                        var response = await apiService.SendAsync<SubmitRequest, SubmitResponse>(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/submit", submitRequest, credential.AuthCode, token);
                        CredentialsLogicService.ShowSubmitResponse(response);
                    }
                    if (cki.Key.ToString() == "D")
                    {
                        ApplicantDocument document = new ApplicantDocument();
                        document = CredentialsLogicService.DocumentFiller();

                        var response = await apiService.SendAsync<ApplicantDocument, DocumentResponse>(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/documents", document, credential.AuthCode, token);
                        CredentialsLogicService.ShowDocumentsResponse(response);
                    }
                    if (cki.Key.ToString() == "F")
                    {
                        Applicant applicant = CredentialsLogicService.ApplicantFiller();
                        var response = await apiService.SendAsync<Applicant, Applicant>(HttpMethod.Put, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/applicant", applicant, credential.AuthCode, token);
                    }
                    if (cki.Key.ToString() == "K")
                    {
                        correct = true;
                        Environment.Exit(0);
                    }
                    if (cki.Key.ToString() != "R" && cki.Key.ToString() != "S" && cki.Key.ToString() != "D" && cki.Key.ToString() != "K" && cki.Key.ToString() != "F")
                    {
                        Console.WriteLine("Invalid input please try again");

                    }
                } while (!correct);


                await host.RunAsync();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHttpClient<IApiService, ApiService>();
            services.AddTransient<ICredentialsLogic, CredentialsLogic>();
        });
    }
}
