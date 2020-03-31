using System;
using System.Collections.Generic;
using Blauhaus.Common.TestHelpers.MockBuilders;

namespace Blauhaus.Common.TestHelpers
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

        public void Clear()
        {
            _mocks.Clear();
        }
    }
}