using JobSearcher_Queries;
using JobSearcher_Queries.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

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
        public async Task GetToken_ShouldReturnToken()
        {
            var clientId = "testClientId";
            var clientSecret = "testClientSecret";
            var expectedResponseContent = "{\"access_token\":\"token_value\"}";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();


            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.Headers.Authorization.Scheme == "Basic" &&
                        req.Headers.Authorization.Parameter == Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")) &&
                        req.Content.ReadAsStringAsync().Result == "grant_type=client_credentials"),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponseContent, Encoding.UTF8, "application/json")
                });


            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var apiService = new ApiService(httpClient);
            var url = "http://example.com/auth/token";


            // Act
            string accessToken = await apiService.GetToken(url, clientId, clientSecret);

            // Assert
            Assert.NotNull(accessToken);
            Assert.Equal("token_value", accessToken);
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
                Content = new StringContent("{\"id\":7129,\"authCode\":\"test_AuthCode\",\"expirationTimestamp\":\"test_ExpirationTimestamp\"}")
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
        public async Task DeleteAsync_ShouldReturn_Response()
        {
            var url = "https://api.example.com/endpoint";
            var requestData = new { Id = 1, Name = "Test" };
            var documentId = "123";
            var accessToken = "token";
            var credential = new Credential
            {
                Id = 123,
                AuthCode = "test_AuthCode",
                ExpirationTimestamp = "test_ExpirationTimestamp"
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\":7129,\"authCode\":\"test_AuthCode\",\"expirationTimestamp\":\"test_ExpirationTimestamp\"}")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.NoContent
                  });

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);

            // Act
            var result = apiService.SendDeleteAsync<object>(url, documentId, credential.AuthCode, accessToken);

            // Assert
            Assert.Equal("RanToCompletion", result.Status.ToString());
        }

        [Fact]
        public async Task SendAsync_ShouldReturn_Response()
        {
            // Arrange
            var url = "https://api.example.com/endpoint";
            var requestData = new { Id = 1, Name = "Test" };
            var authCode = "123";
            var accessToken = "token";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\":7129,\"authCode\":\"240ITlvQ6tE617\",\"expirationTimestamp\":\"23.05.2024 23:41:40\"}")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);
            // Act & Assert

            var result = apiService.SendAsync<object, object>(HttpMethod.Post, url, requestData, authCode, accessToken);
            var jObj = JObject.Parse(result.Result.ToString());
            Assert.Equal(7129, (int)jObj["id"]);
            Assert.Equal("240ITlvQ6tE617", jObj["authCode"].ToString());
        }





        [Fact]
        public async Task SendAsync_ShouldThrowException_OnHttpRequestConflictException()
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
                .ThrowsAsync(new HttpRequestException("Conflict"));

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);
            // Act & Assert

            await Assert.ThrowsAsync<HttpRequestException>(() =>
                apiService.SendAsync<object, object>(HttpMethod.Post, url, requestData, authCode, accessToken));

        }

        [Fact]
        public async Task SendAsync_ShouldThrowException_OnHttpRequestBadRequestException()
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
                .ThrowsAsync(new HttpRequestException("Bad Request"));

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);
            // Act & Assert

            await Assert.ThrowsAsync<HttpRequestException>(() =>
                apiService.SendAsync<object, object>(HttpMethod.Post, url, requestData, authCode, accessToken));
        }

        [Fact]
        public async Task SendAsync_ShouldThrowException_OnHttpRequestUnauthorizedException()
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
                .ThrowsAsync(new HttpRequestException("Bad Request"));

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);
            // Act & Assert

            await Assert.ThrowsAsync<HttpRequestException>(() =>
                apiService.SendAsync<object, object>(HttpMethod.Post, url, requestData, authCode, accessToken));
        }

        [Theory]
        [InlineData("Conflict")]
        [InlineData("Bad Request")]
        [InlineData("Unauthorized")]
        [InlineData("Not Found")]
        [InlineData("Internal Server Error")]
        public async Task SendAsync_ShouldThrowException_OnHttpRequestException(string error)
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
                .ThrowsAsync(new HttpRequestException(error));

            var httpClient = new HttpClient(handlerMock.Object);
            var apiService = new ApiService(httpClient);
            // Act & Assert

            await Assert.ThrowsAsync<HttpRequestException>(() =>
                apiService.SendAsync<object, object>(HttpMethod.Post, url, requestData, authCode, accessToken));
        }
    }
}