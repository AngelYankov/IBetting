//using IBetting.Services.DataConsumeService;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using System.Net;
//using System.Xml;

//namespace IBetting.Tests.DataConsumeServiceTests
//{
//    public class DataConsumeService_Should
//    {
//        [Fact]
//        public async Task LoadFile_ValidXml_Success()
//        {
//            // Arrange
//            string validXml = "<root><element>Test</element></root>";
//            var response = new HttpResponseMessage(HttpStatusCode.OK)
//            {
//                Content = new StringContent(validXml),
//            };

//            var httpClient = CreateHttpClient(response);
//            var dataConsumeService = new DataConsumeService(httpClient);

//            // Act
//            XmlDocument result = await dataConsumeService.LoadFile();

//            // Assert
//            Assert.NotNull(result);
//        }

//        [Fact]
//        public async Task LoadFile_XmlDownloadFailure_ThrowsException()
//        {
//            // Arrange
//            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

//            var httpClient = CreateHttpClient(response);
//            var dataConsumeService = new DataConsumeService(httpClient);

//            // Act and Assert
//            await Assert.ThrowsAsync<Exception>(() => dataConsumeService.LoadFile());
//        }

//        [Fact]
//        public async Task LoadFile_XmlParsingFailure_ThrowsException()
//        {
//            // Arrange
//            string invalidXml = "<root><element>Test</root>";
//            var response = new HttpResponseMessage(HttpStatusCode.OK)
//            {
//                Content = new StringContent(invalidXml),
//            };

//            var httpClient = CreateHttpClient(response);
//            var dataConsumeService = new DataConsumeService(httpClient);

//            // Act and Assert
//            await Assert.ThrowsAsync<Exception>(() => dataConsumeService.LoadFile());
//        }

//        private HttpClient CreateHttpClient(HttpResponseMessage response)
//        {
//            var handler = new TestHttpMessageHandler(response);
//            return new HttpClient(handler);
//        }

//        private class TestHttpMessageHandler : DelegatingHandler
//        {
//            private readonly HttpResponseMessage _response;

//            public TestHttpMessageHandler(HttpResponseMessage response)
//            {
//                _response = response;
//            }

//            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//            {
//                return Task.FromResult(_response);
//            }
//        }
//    }
//}
