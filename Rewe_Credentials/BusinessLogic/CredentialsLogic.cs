using AgeCalculator;
using JobSearcher_Queries.Models;
using Rewe_JobSearcher.Interfaces;
using System.Globalization;

namespace Rewe_JobSearcher.BusinessLogic
{
    public class CredentialsLogic : ICredentialsLogic
    {
        public ApplicantDocument DocumentFiller()
        {
            bool valid = false;
            ApplicantDocument document = new ApplicantDocument();
            do
            {
                Console.WriteLine();
                Console.WriteLine("Please insert the type of the document (Cv, MotivationalLetter, Foto, Misc, GradeSheet)");
                var type = Console.ReadLine().ToLower();
                if (type != null && (type.Equals("cv") || type.Equals("motivationalletter") || type.Equals("foto") || type.Equals("misc") || type.Equals("gradesheet")))
                {
                    document.documentType = type;
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Type is invalid please retry");
                    valid = false;
                }
            } while (!valid);
            do
            {
                valid = false;
                Console.WriteLine();
                Console.WriteLine("Please insert the path of the document");
                var documentPath = Console.ReadLine();
                if (File.Exists(documentPath))
                {
                    if ((document.documentType == "foto" && (!documentPath.EndsWith(".jpg") && !documentPath.EndsWith(".png") && !documentPath.EndsWith(".pdf"))) ||
                        (document.documentType == "cv" && (!documentPath.EndsWith(".pdf") && !documentPath.EndsWith(".doc") && !documentPath.EndsWith(".docx"))) ||
                        (document.documentType == "Motivationalletter" && (!documentPath.EndsWith(".pdf") && !documentPath.EndsWith(".doc") && !documentPath.EndsWith(".docx"))))
                    {
                        Console.WriteLine("Invalid document format");
                        valid = false;
                    }
                    // Create a FileInfo object
                    FileInfo fileInfo = new FileInfo(documentPath);

                    // Get the file size in bytes
                    long fileSizeInBytes = fileInfo.Length / 1000;
                    if (fileSizeInBytes > 3300)
                    {
                        Console.WriteLine("The file is too big, please upload a file smaller than 3MB");
                    }
                    else
                    {
                        document.documentName = Path.GetFileName(documentPath);
                        valid = true;
                    }

                    document.documentBlob = ConvertToBase64(documentPath);
                }
                else
                {
                    Console.WriteLine("Invalid path");
                }
            } while (!valid);
            return document;
        }

        public Filter GetFilter()
        {
            Filter filter = new Filter();
            var cities = GetCityList();
            filter.cityList = cities;
            Console.WriteLine("Please write your favourite zip:");
            filter.zip = Console.ReadLine();
            return filter;
        }

        private List<string> GetCityList()
        {
            bool end = false;
            List<string> cityList = new List<string>();
            do
            {
                Console.WriteLine("Please write your desired cities:");
                var city = Console.ReadLine();
                if (city != null && city != string.Empty)
                {
                    cityList.Add(city);
                    Console.WriteLine("Would you like to add another city [Y] or [N]");
                    var input = Console.ReadLine().ToLower();
                    end = !input.Equals("n");
                }
            } while (end);

            return cityList;
        }

        public Applicant ApplicantFiller()
        {

            Applicant applicant = new Applicant();
            Console.WriteLine();
            Console.WriteLine("Please write your family name ");
            applicant.lastName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please write your Nationality code (3 letters Ex: ITA, AUT. Default is AUT) ");
            applicant.nationality = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please write your country code  code (3 letters Ex: +39, +43 ... Default is +43) ");
            applicant.nationality = Console.ReadLine();
            Console.WriteLine();
            applicant.birthDate = DateChecker();
            return applicant;
        }

        public void ShowJobs(SearchResponse response)
        {
            Console.WriteLine(response.numberOfHits);
            Console.WriteLine(response.totalCount);
            foreach (var job in response.jobs)
            {
                Console.WriteLine("Job Id is: " + job.jobId);
                Console.WriteLine("Job DescriptionIdId is: " + job.jobDescriptionId);
                Console.WriteLine("Job Title is: " + job.title);
                Console.WriteLine("Location is: " + job.city);
                Console.WriteLine("zip is: " + job.zip);
                Console.WriteLine("Job Level is: " + job.employmentLevel);
            }

        }
        public void ShowSubmitResponse(SubmitResponse response)
        {
            if (response != null)
            {
                Console.WriteLine("You submitted succesfuly for the job with Id:" + response.id);
                Console.WriteLine("for this job we received the following documents:");
                foreach (var document in response.documents)
                {
                    Console.WriteLine($"Document of type {document.documentType} with name {document.documentName}");
                }
            }
        }

        public void ShowDocumentsResponse(DocumentResponse documentsResponse)
        {
            if (!string.IsNullOrEmpty(documentsResponse.documentName))
            {
                Console.WriteLine($"Document of type {documentsResponse.documentType} with name {documentsResponse.documentName} and Id: {documentsResponse.documentId} has been  uploaded");
            }
        }

        //helper to show a reduced  description of the job
        public void ShowJobDescriptionResponse(JobDescriptionResponse jobDescriptionResponse)
        {
            if (!string.IsNullOrEmpty(jobDescriptionResponse.jobDescriptionId))
            {
                Console.WriteLine($"Job description with id {jobDescriptionResponse.jobDescriptionId} has been retrieved");
                Console.WriteLine($"The job description is: {jobDescriptionResponse.description}");
            }
        }

        //Helper to show a reduced detailed description of the job
        public void ShowJobWithDetailedDescriptionResponse(JobWithDetailedDescriptionResponse jobWithDetailedDescriptionResponse)
        {
            if (!string.IsNullOrEmpty(jobWithDetailedDescriptionResponse.job.jobDescriptionId))
            {
                ShowJobDescriptionResponse(jobWithDetailedDescriptionResponse.description);
                Console.WriteLine($"Job accountingCompany is: {jobWithDetailedDescriptionResponse.job.accountingCompany}");
            }
        }

        private DateTime DateChecker()
        {
            bool adult = false;
            DateTime birthDate;
            do
            {
                Console.WriteLine("Please write your bith date ");
                string date = Console.ReadLine();
                var cultureInfo = new CultureInfo("de-DE");
                birthDate = DateTime.Parse(date, cultureInfo);
                var age = new Age(birthDate, DateTime.Today);
                if (age.Years < 18)
                {
                    Console.WriteLine("You are old enough to apply for a job");
                    //throw new FormatException("You are not old enough to apply for a job");
                }
                else
                {
                    adult = true;
                }
            } while (!adult);
            return birthDate;
        }
        private string ConvertToBase64(string path)
        {
            Byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }
    }
}
