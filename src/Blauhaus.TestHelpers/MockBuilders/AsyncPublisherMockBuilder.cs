using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Moq;

namespace Blauhaus.TestHelpers.MockBuilders
{

    public abstract class BaseAsyncPublisherMockBuilder<TBuilder, TMock, T> : BaseMockBuilder<TBuilder, TMock>
        where TBuilder : BaseAsyncPublisherMockBuilder<TBuilder, TMock, T>
        where TMock : class, IAsyncPublisher<T>
    {
        private readonly List<Func<T, Task>> _handlers = new List<Func<T, Task>>();

        public Mock<IDisposable> Where_SubscribeAsync_returns_token()
        {
            var mockToken = new Mock<IDisposable>();
            
            Mock.Setup(x => x.SubscribeAsync(It.IsAny<Func<T, Task>>()))
                .ReturnsAsync(mockToken.Object);
            
            return mockToken;
        }

        public Mock<IDisposable> Where_SubscribeAsync_publishes_immediately(T update)
        {
            var mockToken = new Mock<IDisposable>();

            Mock.Setup(x => x.SubscribeAsync(It.IsAny<Func<T, Task>>()))
                .Callback(async (Func<T, Task> handler) =>
                {
                    _handlers.Add(handler);
                    await handler.Invoke(update);
                }).ReturnsAsync(mockToken.Object);

            return mockToken;
        }
        
        public Mock<IDisposable> Where_SubscribeAsync_publishes_immediately(IEnumerable<T> updates)
        {
            var mockToken = new Mock<IDisposable>();

            Mock.Setup(x => x.SubscribeAsync(It.IsAny<Func<T, Task>>()))
                .Callback(async (Func<T, Task> handler) =>
                {
                    _handlers.Add(handler);
                    foreach (var update in updates)
                    {
                        await handler.Invoke(update);
                    }
                }).ReturnsAsync(mockToken.Object);

            return mockToken;
        }

        public Mock<IDisposable> Where_SubscribeAsync_publishes_sequence(IEnumerable<T> updates)
        {
            var mockToken = new Mock<IDisposable>();
            var queue = new Queue<T>(updates);

            Mock.Setup(x => x.SubscribeAsync(It.IsAny<Func<T, Task>>()))
                .Callback((Func<T, Task> handler) =>
                {
                    _handlers.Add(handler);
                    handler.Invoke(queue.Dequeue());
                }).ReturnsAsync(mockToken.Object);

            return mockToken;
        }
         

        public async Task PublishMockSubscriptionAsync(T model)
        {
            foreach (var handler in _handlers)
            {
                await handler.Invoke(model);
            }
        }
    }

    public class AsyncPublisherMockBuilder<TMock, T> : BaseAsyncPublisherMockBuilder<AsyncPublisherMockBuilder<TMock, T>, TMock, T>
        where TMock : class, IAsyncPublisher<T>
    {
        
      
    }
}