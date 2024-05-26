using JobSearcher_Queries.Models;
using Rewe_JobSearcher.Interfaces;
using System.Globalization;
using System.Text.Json;

namespace Rewe_JobSearcher.BusinessLogic
{
    public class CredentialsLogic : ICredentialsLogic
    {
        public ApplicantDocument DocumentFiller()
        {
            ApplicantDocument document = new ApplicantDocument();
            Console.WriteLine();
            Console.WriteLine("Please insert the type of the document (Cv, MotivationalLetter, Foto, Misc, GradeSheet)");
            document.documentType = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please insert the path of the document");
            var documentPath = Console.ReadLine();
            if (File.Exists(documentPath))
            {
                if ((document.documentType == "Foto" && (!documentPath.EndsWith(".jpg") && !documentPath.EndsWith(".bmp") && !documentPath.EndsWith(".png") && !documentPath.EndsWith(".pdf"))) ||
                    (document.documentType == "Cv" && (!documentPath.EndsWith(".pdf") && !documentPath.EndsWith(".doc") && !documentPath.EndsWith(".docx"))) ||
                    (document.documentType == "MotivationalLetter" && (!documentPath.EndsWith(".pdf") && !documentPath.EndsWith(".doc") && !documentPath.EndsWith(".docx"))))
                {
                    Console.WriteLine("Invalid document name");
                }
                else
                {
                    document.documentName = Path.GetFileName(documentPath);
                }
                document.documentBlob = ConvertToBase64(documentPath);
            }
            else
            {
                Console.WriteLine("Invalid path");
            }
            return document;
        }

        public Applicant ApplicantFiller()
        {
            var cultureInfo = new CultureInfo("de-DE");
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
            Console.WriteLine("Please write your bith date ");
            string date = Console.ReadLine();
            applicant.birthDate = DateTime.Parse(date, cultureInfo);
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

        private string ExtractAccessToken(string jsonResponse)
        {
            // Parse the JSON response and extract the access_token
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;
                return root.GetProperty("access_token").GetString();
            }
        }
        private string ConvertToBase64(string path)
        {
            Byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }
    }
}
