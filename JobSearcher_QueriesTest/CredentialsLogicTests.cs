using JobSearcher_Queries.Models;
using Rewe_JobSearcher.BusinessLogic;

namespace JobSearcher_QueriesTest
{
    public class CredentialsLogicTests
    {

        [Fact]
        public void ShowJobs_ShouldWriteJobDetailsToConsole()
        {
            // Arrange
            var logic = new CredentialsLogic();
            var response = new SearchResponse
            {
                numberOfHits = 5,
                totalCount = 10,
                jobs = new List<Job>
                {
                    new Job
                    {
                        jobId = "1",
                        jobDescriptionId = "JD1",
                        title = "Software Engineer",
                        city = "Berlin",
                        zip = "12345",
                        employmentLevel = "Senior"
                    },
                    new Job
                    {
                        jobId = "2",
                        jobDescriptionId = "JD2",
                        title = "Data Analyst",
                        city = "Munich",
                        zip = "54321",
                        employmentLevel = "Junior"
                    }
                }
            };

            //Redirect the Output
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            logic.ShowJobs(response);
            var consoleOutput = stringWriter.ToString();

            // Assert
            Assert.Contains("Software Engineer", consoleOutput);
            Assert.Contains("Berlin", consoleOutput);
            Assert.Contains("Data Analyst", consoleOutput);
        }

        [Fact]
        public void ShowSubmitResponse_ShouldWriteDocumentDetailsToConsole()
        {
            // Arrange
            var logic = new CredentialsLogic();
            var submitResponse = new SubmitResponse()
            {
                applicationDetails = new JobApplicationResult()
                {
                    JobId = "001",
                    StoreIdList = new List<int> { 2, 3 },
                    DesiredSalary = 1000,
                    RecommendedBy = "John Doe",
                    AvailableFrom = DateTime.Now,
                    AgreedToDataProcessing = true,
                    AgreedToDataRelaying = true,
                    ExternalSource = "Indeed",
                    Id = 007,
                    Applicant = new Applicant() { firstName = "Chuck", lastName = "Norris" },
                    Documents = new List<ApplicantDocument>
                    {
                        new ApplicantDocument
                        {
                            documentBlob = "blobblob",
                            documentType = "CV",
                            documentName = "CV.pdf"
                        }
                        ,
                        new ApplicantDocument
                        {
                             documentBlob = "Bigblob",
                            documentType = "MotivationalLetter",
                            documentName = "MotivationalLetter.pdf"
                        }
                    }
                },
                id = 1,
                applicant = new Applicant()
                {
                    titleCode = "Mr",
                    firstName = "Chuck",
                    lastName = "Norris",
                },
                documents = new List<Documents>
                {
                    new Documents
                    {
                        documentId = 1,
                        documentType = "CV",
                        documentName = "CV.pdf"
                    }
                    ,
                    new Documents
                    {
                        documentId = 2,
                        documentType = "MotivationalLetter",
                        documentName = "MotivationalLetter.pdf"
                    }
                }

            };

            //Redirect the Output
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            logic.ShowSubmitResponse(submitResponse);
            var consoleOutput = stringWriter.ToString();

            // Assert
            Assert.Contains("You submitted succesfuly for the job with", consoleOutput);
            Assert.Contains("Document of type MotivationalLetter with name MotivationalLetter.pdf", consoleOutput);
        }

        [Fact]
        public void ShowDocumentsResponse_ShouldWriteDocumentsResponseDetailsToConsole()
        {
            // Arrange
            var logic = new CredentialsLogic();
            var documentsResponse = new DocumentResponse() { documentId = 1, documentType = "CV", documentName = "CV.pdf" };

            //Redirect the Output
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            logic.ShowDocumentsResponse(documentsResponse);
            var consoleOutput = stringWriter.ToString();

            // Assert
            Assert.Contains("Document of type CV with name CV.pdf and Id: 1 has been  uploaded", consoleOutput);
        }

        [Fact]
        public void ShowJobDescriptionResponse_ShouldWriteDocumentsResponseDetailsToConsole()
        {
            // Arrange
            var logic = new CredentialsLogic();
            var jobDescriptionResponse = new JobDescriptionResponse() { jobDescriptionId = "WTR", description = "Texas Ranger" };

            //Redirect the Output
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            logic.ShowJobDescriptionResponse(jobDescriptionResponse);
            var consoleOutput = stringWriter.ToString();

            // Assert
            Assert.Contains("Job description with id WTR has been retrieved\r\nThe job description is: Texas Ranger\r\n", consoleOutput);
        }

        [Fact]
        public void showJobWithDetailedDescriptionResponse_ShouldWriteDocumentsResponseDetailsToConsole()
        {
            // Arrange
            var logic = new CredentialsLogic();

            var jobWithDetailedDescriptionResponse = new JobWithDetailedDescriptionResponse()
            {
                job = new Job()
                {
                    jobId = "007",
                    jobDescriptionId = "WTR",
                    accountingCompany = "Accounting Company"
                },
                description = new JobDescriptionResponse() { jobDescriptionId = "WTR", description = "Texas Ranger" }
            };

            //Redirect the Output
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            logic.ShowJobWithDetailedDescriptionResponse(jobWithDetailedDescriptionResponse);
            var consoleOutput = stringWriter.ToString();

            // Assert
            Assert.Contains("Job description with id WTR has been retrieved\r\nThe job description is: Texas Ranger\r\nJob accountingCompany is: Accounting Company", consoleOutput);

        }
    }
}
