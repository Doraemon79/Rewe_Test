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
                string ApplicationId = "";
                bool correct = false;
                do
                {
                    Console.WriteLine("Would you like to read jobs advertisements [R] or submit [S]  for one");
                    ConsoleKeyInfo cki = Console.ReadKey();
                    Credential credential = new Credential();
                    await apiService.PostCredentials(token, credential);
                    if (cki.Key.ToString() == "R")
                    {
                        correct = true;

                        Filter filter = CredentialsLogicService.GetFilter();
                        await apiService.PostSearchAsync(token, filter);
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    if (cki.Key.ToString() == "S")
                    {
                        Console.WriteLine("Please insert the ApplicationId");
                        ApplicationId = Console.ReadLine();
                        await apiService.Submit(token, ApplicationId, new SubmitDetails(), credential.AuthCode);
                        correct = true;
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else
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
