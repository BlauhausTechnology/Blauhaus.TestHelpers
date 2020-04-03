using System;
using System.Collections.Generic;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.TestHelpers
{
    public class MockContainer
    {
        private readonly Dictionary<string, object> _mocks = new Dictionary<string, object>();

        public Func<TMockBuilder> AddMock<TMockBuilder, T>() 
            where TMockBuilder : BaseMockBuilder<TMockBuilder, T>, new() 
            where T : class
        {
            var mockName = typeof(T).Name;

            return () =>
            {
                if (!_mocks.ContainsKey(mockName))
                {
                    _mocks[mockName] = new TMockBuilder();
                }

                return (TMockBuilder) _mocks[mockName];
            };

        }
        public Func<MockBuilder<T>> AddMock<T>() 
            where T : class
        {
            var mockName = typeof(T).Name;

            return () =>
            {
                if (!_mocks.ContainsKey(mockName))
                {
                    _mocks[mockName] = new MockBuilder<T>();
                }

                return (MockBuilder<T>) _mocks[mockName];
            };

        }

        public void Clear()
        {
            _mocks.Clear();
        }
    }
}