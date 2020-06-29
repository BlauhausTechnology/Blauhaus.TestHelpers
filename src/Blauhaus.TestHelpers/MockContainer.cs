using System;
using System.Collections.Generic;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.TestHelpers
{
    public class MockContainer
    {
        private readonly Dictionary<string, object> _mocks = new Dictionary<string, object>();

        /// <summary>
        /// Adds specific mockbuilder to the mock container. Provide a unique name if MockBuilder may have the same Type name as another - eg when using generics
        /// </summary>
        public Func<TMockBuilder> AddMock<TMockBuilder, T>(string mockUniqueName = "") 
            where TMockBuilder : BaseMockBuilder<TMockBuilder, T>, new() 
            where T : class
        {
            if (string.IsNullOrEmpty(mockUniqueName))
            {
                mockUniqueName = typeof(T).Name;
            }

            return () =>
            {
                if (!_mocks.ContainsKey(mockUniqueName))
                {
                    _mocks[mockUniqueName] = new TMockBuilder();
                }

                return (TMockBuilder) _mocks[mockUniqueName];
            };

        }
        /// <summary>
        /// Adds specific mockbuilder to the mock container. Provide a unique name if MockBuilder may have the same Type name as another - eg when using generics
        /// </summary>
        public Func<MockBuilder<T>> AddMock<T>(string mockUniqueName = "") 
            where T : class
        {
            if (string.IsNullOrEmpty(mockUniqueName))
            {
                mockUniqueName = typeof(T).Name;
            }

            return () =>
            {
                if (!_mocks.ContainsKey(mockUniqueName))
                {
                    _mocks[mockUniqueName] = new MockBuilder<T>();
                }

                return (MockBuilder<T>) _mocks[mockUniqueName];
            };

        }

        public void Clear()
        {
            _mocks.Clear();
        }
    }
}