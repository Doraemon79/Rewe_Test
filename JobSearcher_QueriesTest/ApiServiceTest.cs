using JobSearcher_Queries;
using JobSearcher_Queries.Models;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JobSearcher_QueriesTest
{
    public class ApiServiceTests
    {
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly ApiService _apiService;

        public ApiServiceTests()
        {
            _httpClientMock = new Mock<HttpClient>();
            _apiService = new ApiService(_httpClientMock.Object);
        }

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

        [Fact]
        public async Task SendAsync_ShouldThrowException_OnHttpRequestException()
        {
            // Arrange
            var url = "https://api.example.com/endpoint";
            var requestData = new { Id = 1, Name = "Test" };
            var authCode = "123";
            var accessToken = "token";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);
            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                apiService.SendAsync<object, object>(HttpMethod.Post, url, requestData, authCode, accessToken));
        }

        [Fact]
        public async Task SendAsync_ShouldReturnResponse()
        {
            // Arrange
            var method = HttpMethod.Post;
            var url = "https://dev.apply.rewe-group.at:443/V1/api/job-applications/7129/submit";
            var requestData = new { Key = "Value" };
            var authCode = "123";
            var accessToken = "token";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var requestMessage = new HttpRequestMessage(method, url);
            requestMessage.Headers.Add("application-auth-code", authCode);
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.Content = new StringContent(JsonSerializer.Serialize(expectedResponse), Encoding.UTF8, "application/json");

            _httpClientMock
                .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responseMessage);

            // Act
            var response = await _apiService.SendAsync<object, HttpResponseMessage>(method, url, requestData, authCode, accessToken);

            // Assert
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }
    }
}