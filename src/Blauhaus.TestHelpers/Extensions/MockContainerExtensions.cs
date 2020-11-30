using Blauhaus.Common.Utils.Contracts;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.TestHelpers.Extensions
{
    public static class MockContainerExtensions
    {
        public static AsyncPublisherMockBuilder<TMock, T> AddMockAsyncPublisher<TMock, T>(this MockContainer mocks) where TMock : class, IAsyncPublisher<T>
        {
            var mock = new AsyncPublisherMockBuilder<TMock, T>();
            mocks.AddMock<AsyncPublisherMockBuilder<TMock, T>, TMock>();
            return mock;
        }
    }
}