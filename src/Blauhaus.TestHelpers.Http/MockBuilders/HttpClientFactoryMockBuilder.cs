using System.Net.Http;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.Common.TestHelpers.Http.MockBuilders
{
    public class HttpClientFactoryMockBuilder : BaseMockBuilder<HttpClientFactoryMockBuilder, IHttpClientFactory>
    {

        public HttpClientFactoryMockBuilder Where_CreateClient_returns(HttpClient client, string clientName = "")
        {
            if (string.IsNullOrEmpty(clientName))
            {
                Mock.Setup(x => x.CreateClient(It.IsAny<string>()))
                    .Returns(client);
            }
            else
            {
                Mock.Setup(x => x.CreateClient(clientName))
                    .Returns(client);
            }

            return this;
        }

        public HttpClientFactoryMockBuilder Where_CreateClient_returns_client_with_handler(HttpMessageHandler messageHandler, string clientName = "")
        {
            if (string.IsNullOrEmpty(clientName))
            {
                Mock.Setup(x => x.CreateClient(It.IsAny<string>()))
                    .Returns(new HttpClient(messageHandler));
            }
            else
            {
                Mock.Setup(x => x.CreateClient(clientName))
                    .Returns(new HttpClient(messageHandler));
            }

            return this;
        }
    }
}