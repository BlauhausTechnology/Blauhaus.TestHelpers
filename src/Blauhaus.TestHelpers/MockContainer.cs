using System;
using System.Collections.Generic;
using Blauhaus.TestHelpers.Builders.Base;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.TestHelpers
{
    public class MockContainer
    {
        private readonly Dictionary<Type, object> _mocks = new Dictionary<Type, object>();

        public Func<TMockBuilder> AddMock<TMockBuilder, T>() 
            where TMockBuilder : IBuilder<TMockBuilder, T>, new() 
            where T : class
        {
            return () =>
            {
                if (!_mocks.ContainsKey(typeof(T)))
                {
                    _mocks[typeof(T)] = new TMockBuilder();
                }

                return (TMockBuilder) _mocks[typeof(T)];
            };

        }
        public Func<MockBuilder<T>> AddMock<T>() 
            where T : class
        {
            return () =>
            {
                if (!_mocks.ContainsKey(typeof(T)))
                {
                    _mocks[typeof(T)] = new MockBuilder<T>();
                }

                return (MockBuilder<T>) _mocks[typeof(T)];
            };

        }

        public void Clear()
        {
            _mocks.Clear();
        }
    }
}