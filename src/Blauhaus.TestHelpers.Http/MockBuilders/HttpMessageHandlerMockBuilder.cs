﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Blauhaus.TestHelpers.Http.MockBuilders
{
public class HttpRequestHandlerMockBuilder : Mock<HttpMessageHandler>
    {
        private HttpStatusCode _code = HttpStatusCode.Accepted;
        //private string _content = string.Empty;
        private string _reasonPhrase = string.Empty;
        private Exception? _exception;
        private Dictionary<string, string> _headers = new();
        private List<HttpResponseMessage>? _responses;
        private string _content = null!;

        public List<HttpRequestMessage> SentRequests { get; } = new();

        public HttpRequestHandlerMockBuilder()
        {
            this.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback((HttpRequestMessage request, CancellationToken _) => { SentRequests.Add(request); })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonSerializer.Serialize(""))
                })
                .Verifiable();
        }
        
        public HttpRequestHandlerMockBuilder Build()
        {

            if (_responses != null)
            {
                var queue = new Queue<HttpResponseMessage>(_responses);

                this.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                     .Callback((HttpRequestMessage request, CancellationToken _) => { SentRequests.Add(request); })
                    .ReturnsAsync(queue.Dequeue)
                    .Verifiable();
            }
            else
            {
                var response = new HttpResponseMessage
                {
                    ReasonPhrase = _reasonPhrase,
                    StatusCode = _code,
                    Content = new StringContent(_content)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                foreach (var header in _headers)
                {
                    response.Headers.Add(header.Key, new List<string> {header.Value});
                }

                if (_exception != null)
                {
                    this.Protected()
                        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                        .Callback((HttpRequestMessage request, CancellationToken _) => { SentRequests.Add(request); })
                        .ThrowsAsync(_exception)
                        .Verifiable();
                }
                else
                {
                    this.Protected()
                        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                        .Callback((HttpRequestMessage request, CancellationToken _) => { SentRequests.Add(request); })
                        .ReturnsAsync(response)
                        .Verifiable();
                }
            }

            return this;
        }

        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_StatusCode(HttpStatusCode code)
        {
            _code = code;
            return this;
        }

        public HttpRequestHandlerMockBuilder Where_SendAsync_throws(Exception e)
        {
            _exception = e;
            return this;
        }
        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_Sequence(List<HttpResponseMessage> responses)
        {
            _responses = responses;
            return this;
        }
 
        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_Content(string content)
        {
            _content = content;
            return this;
        }
        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_Content(object content)
        {
            _content = JsonSerializer.Serialize(content);
            return this;
        }

        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_ReasonPhrase(string value)
        {
            _reasonPhrase = value;
            return this;
        }
        
        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_Headers(Dictionary<string, string> headers)
        {
            _headers = headers;
            return this;
        }
        public HttpRequestHandlerMockBuilder Where_SendAsync_returns_Header(string key, string value)
        {
            _headers[key] = value;
            return this;
        }

        public void VerifyMethod(HttpMethod method, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => y.Method == method),
                    ItExpr.IsAny<CancellationToken>());
        }
        public void VerifyUri(string uri, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => y.RequestUri == new Uri(uri)),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyUri(Func<string, bool> predicate, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => predicate.Invoke(y.RequestUri!.AbsoluteUri)),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyContent(string content, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => string.Equals(y.Content!.ReadAsStringAsync().Result, content, StringComparison.InvariantCultureIgnoreCase)),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyContent(Func<string, bool> predicate)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(y => predicate.Invoke(y.Content!.ReadAsStringAsync().Result)),
                    ItExpr.IsAny<CancellationToken>());
        }
         
        public void VerifyHeader(string key, string value, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => y.Headers.Any(x => x.Key == key && x.Value.First() == value)),
                    ItExpr.IsAny<CancellationToken>());
        }
        public void VerifyHeader(string key, Func<string, bool> predicate, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => y.Headers.Any(x => x.Key == key && predicate.Invoke(x.Value.First()))),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyAuthHeader(string scheme, string value, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => 
                        y.Headers.Authorization!.Scheme == scheme && 
                        y.Headers.Authorization.Parameter == value),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void VerifyAuthHeaderExists(string scheme, int times = 1)
        {
            this.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(y => 
                        y.Headers.Authorization!.Scheme == scheme && 
                        y.Headers.Authorization.Parameter!.Length > 0),
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