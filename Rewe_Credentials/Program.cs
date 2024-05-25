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
                bool exit = false;
                do
                {
                    Console.WriteLine("Please choose one of the following request to send the API:");
                    Console.WriteLine("- [R] To read jobs advertisements ");
                    Console.WriteLine("- [S] To submit an application");
                    Console.WriteLine("- [F] To fill your profile ");
                    Console.WriteLine("- [D] To upload a document in your profile ");
                    Console.WriteLine("- [T] To retrieve a job description ");
                    Console.WriteLine("- [Det] To retrieve a detailed job description ");
                    Console.WriteLine("- [C] To cancel one of your documents in your profile ");
                    Console.WriteLine("- [E] To exit");

                    string input = Console.ReadLine();
                    Console.WriteLine();
                    switch (input)
                    {
                        case "R":
                            Console.WriteLine("You selected to read the job ads");
                            Filter filter = CredentialsLogicService.GetFilter();
                            var searchResponse = await apiService.SendAsync<Filter, SearchResponse>(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/jobs/search", filter, null, token);
                            CredentialsLogicService.ShowJobs(searchResponse);
                            break;
                        case "S":
                            Console.WriteLine("You selected to submit your application");
                            var submitRequest = new SubmitRequest();
                            Console.WriteLine("Please insert the JobId (this can be found when you read [R] the job ads )");
                            submitRequest.jobId = Console.ReadLine();
                            submitRequest.Id = credential.Id;

                            var submitResponse = await apiService.SendAsync<SubmitRequest, SubmitResponse>(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/submit", submitRequest, credential.AuthCode, token);
                            CredentialsLogicService.ShowSubmitResponse(submitResponse);
                            break;
                        case "F":
                            Console.WriteLine("You selected to fill your profile");
                            Applicant applicant = CredentialsLogicService.ApplicantFiller();
                            var applicantResponse = await apiService.SendAsync<Applicant, Applicant>(HttpMethod.Put, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/applicant", applicant, credential.AuthCode, token);
                            break;
                        case "D":
                            Console.WriteLine("You selected to upload the documents");
                            ApplicantDocument document = new ApplicantDocument();
                            document = CredentialsLogicService.DocumentFiller();

                            var documentResponse = await apiService.SendAsync<ApplicantDocument, DocumentResponse>(HttpMethod.Post, "https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/documents", document, credential.AuthCode, token);
                            CredentialsLogicService.ShowDocumentsResponse(documentResponse);
                            break;
                        case "C":
                            Console.WriteLine("You selected to cancel the documents");
                            Console.WriteLine("Please write the DocumentId of the document you want to delete (it is the number given when you uploaded it) :");
                            var jobDocumentId = Console.ReadLine();
                            var deleteResponse = await apiService.SendDeleteAsync<EmptyGenericRequest>("https://dev.apply.rewe-group.at:443/V1/api/job-applications/" + credential.Id + "/documents/" + jobDocumentId, null, credential.AuthCode, token);

                            break;
                        case "T":
                            Console.WriteLine("You selected to retrieve a job description the documents");
                            Console.WriteLine("Please write the JobDescriptionId of the document (the JobDescriptionId can be found when you read [R] the job ads ):");
                            var jobDescriptionId = Console.ReadLine();
                            var jobDescriptionResponse = await apiService.SendAsync<EmptyGenericRequest, JobDescriptionResponse>(HttpMethod.Get, "https://dev.apply.rewe-group.at:443/V1/api/jobs/" + jobDescriptionId, null, credential.AuthCode, token);
                            CredentialsLogicService.ShowDocumentsResponse(jobDescriptionResponse);
                            break;
                        case "Det":
                            Console.WriteLine("You selected to retrieve a job detailed description the documents");
                            Console.WriteLine("Please write the JobDescriptionId of the document (the JobDescriptionId can be found when you read [R] the job ads ):");
                            jobDescriptionId = Console.ReadLine();
                            Console.WriteLine("Please write the JobId of the job Offer (the JobId can be found when you read [R] the job ads ) :");
                            var JobId = Console.ReadLine();
                            var jobWithDetailedDescriptionResponse = await apiService.SendAsync<EmptyGenericRequest, JobWithDetailedDescriptionResponse>(HttpMethod.Get, "https://dev.apply.rewe-group.at:443/V1/api/jobs/" + jobDescriptionId + "/offers/" + JobId, null, credential.AuthCode, token);
                            CredentialsLogicService.ShowJobWithDetailedDescriptionResponseResponse(jobWithDetailedDescriptionResponse);
                            break;
                        case "K":
                            Console.WriteLine("Exiting...");
                            exit = true;
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid selection, please try again.");
                            break;
                    }



                } while (!exit);
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
