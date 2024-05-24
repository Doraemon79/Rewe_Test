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
                bool correct = false;
                Credential credential = new Credential();
                await apiService.PostCredentials(token, credential);
                do
                {

                    Console.WriteLine("Would you like to read jobs advertisements [R], submit [S], fill your profile [F], load document in your profile [D] or Exit [K]");
                    ConsoleKeyInfo cki = Console.ReadKey();
                    Console.WriteLine();

                    if (cki.Key.ToString() == "R")
                    {
                        Filter filter = CredentialsLogicService.GetFilter();
                        await apiService.PostSearchAsync(token, filter);

                    }
                    if (cki.Key.ToString() == "S")
                    {
                        Console.WriteLine("Please insert the JobId");
                        string jobId = Console.ReadLine();
                        var submitDetails = new SubmitDetails();
                        submitDetails.jobId = jobId;
                        apiService.PostSubmit(token, credential.Id, submitDetails, credential.AuthCode);
                    }
                    if (cki.Key.ToString() == "D")
                    {
                        ApplicantDocument document = new ApplicantDocument();
                        document = CredentialsLogicService.DocumentFiller();
                        await apiService.PostDocuments(token, credential.Id, document, credential.AuthCode);
                    }
                    if (cki.Key.ToString() == "K")
                    {
                        Environment.Exit(0);
                    }
                    if (cki.Key.ToString() == "F")
                    {
                        await apiService.PutApplicantProfile(token, credential.Id, credential.AuthCode);
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
