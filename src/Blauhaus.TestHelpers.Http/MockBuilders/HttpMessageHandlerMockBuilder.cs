﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Blauhaus.TestHelpers.Http.MockBuilders
{
    public class HttpMessageHandlerMockBuilder : Mock<HttpMessageHandler>
    {
         private HttpStatusCode _code = HttpStatusCode.Accepted;
        private string _content = string.Empty;
        private string _reasonPhrase;
        private Exception? _exception;

        public HttpMessageHandlerMockBuilder()
        {
            this.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonSerializer.Serialize(""))
                })
                .Verifiable();
        }
        
        public HttpMessageHandlerMockBuilder Build()
        {
            var response = new HttpResponseMessage
            {
                ReasonPhrase = _reasonPhrase,
                StatusCode = _code,
                Content = new StringContent(_content),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (_exception != null)
            {
                this.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ThrowsAsync(_exception)
                    .Verifiable();
            }
            else
            {
                this.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .Callback<HttpRequestMessage, CancellationToken>(async (m, c) =>
                    {
                        Debug.WriteLine(await m.Content.ReadAsStringAsync());
                    })
                    .ReturnsAsync(response)
                    .Verifiable();
            }

            return this;
        }

        public HttpMessageHandlerMockBuilder Where_SendAsync_returns_StatusCode(HttpStatusCode code)
        {
            _code = code;
            return this;
        }

        public HttpMessageHandlerMockBuilder Where_SendAsync_throws(Exception e)
        {
            _exception = e;
            return this;
        }

        public HttpMessageHandlerMockBuilder Where_SendAsync_returns_Content(string content)
        {
            _content = content;
            return this;
        }

        public HttpMessageHandlerMockBuilder Where_SendAsync_returns_ReasonPhrase(string value)
        {
            _reasonPhrase = value;
            return this;
        }

        public void VerifyMethod(HttpMethod method)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(y => y.Method == method),
                    ItExpr.IsAny<CancellationToken>());
        }
        public void VerifyUri(string uri)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(y => y.RequestUri == new Uri(uri)),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyContent(string content)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(y => string.Equals(y.Content.ReadAsStringAsync().Result, content, StringComparison.InvariantCultureIgnoreCase)),
                    ItExpr.IsAny<CancellationToken>());
        }
         
        public void VerifyHeader(string key, string value, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => y.Headers.Any(x => x.Key == key && x.Value.First() == value)),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyAuthHeader(string scheme, string value, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => 
                        y.Headers.Authorization.Scheme == scheme && 
                        y.Headers.Authorization.Parameter == value),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyNoAuthHeader()
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(y => 
                        y.Headers.Authorization == null),
                    ItExpr.IsAny<CancellationToken>());
        }
    }
}