using JobSearcher_Queries;
using JobSearcher_Queries.Models;
using Moq;
using Moq.Protected;
using System.Net;

namespace JobSearcher_QueriesTest
{
    public class ApiServiceTest
    {

        [Fact]
        public async Task PostCredentials_ShouldReturnResponse()
        {
            // Arrange
            var credential = new Credential
            {
                Id = 123,
                AuthCode = "test_AuthCode",
                ExpirationTimestamp = "test_ExpirationTimestamp"
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\":7129,\"authCode\":\"240ITlvQ6tE617\",\"expirationTimestamp\":\"23.05.2024 23:41:40\"}")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);

            // Act
            var result = await apiService.PostCredentials("123", credential);

            // Assert
            Assert.Equal("Microsoft.AspNetCore.Mvc.OkObjectResult", result.ToString());
        }

        //[Fact]
        //public async Task PostSubmit_ShouldReturnResponse()
        //{
        //    // Arrange
        //    SubmitDetails submitDetails = new SubmitDetails();
        //    submitDetails.jobId = "123";
        //    submitDetails.desiredSalary = 1000;

        //    var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent("{\"id\":7129,\"authCode\":\"240ITlvQ6tE617\",\"expirationTimestamp\":\"23.05.2024 23:41:40\"}")
        //    };

        //    var handlerMock = new Mock<HttpMessageHandler>();
        //    handlerMock
        //        .Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //            "SendAsync",
        //            ItExpr.IsAny<HttpRequestMessage>(),
        //            ItExpr.IsAny<CancellationToken>()
        //        )
        //        .ReturnsAsync(responseMessage);

        //    var httpClient = new HttpClient(handlerMock.Object);
        //    var apiService = new ApiService(httpClient);

        //    // Act
        //    var result = await apiService.PostSubmit("123", "123", submitDetails, "Test_authCode");

        //    // Assert
        //    Assert.Equal("Microsoft.AspNetCore.Mvc.OkObjectResult", result.Result.ToString());
        //}
    }
}