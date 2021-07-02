using System;
using System.Linq.Expressions;
using System.Threading;
using AutoFixture;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using Moq;
// ReSharper disable MemberCanBePrivate.Global

namespace Blauhaus.TestHelpers.BaseTests
{
    public abstract class BaseUnitTest<TSut> 
    {
        protected IFixture MyFixture => _fixture ??= new Fixture();
        private IFixture? _fixture;

        protected CancellationToken CancelToken => CancellationTokenSource.Token;
        private CancellationTokenSource? _cancellationTokenSource = new();
        private CancellationTokenSource CancellationTokenSource => _cancellationTokenSource ??= new CancellationTokenSource();

        protected MockContainer Mocks => _mocks ??= new MockContainer();
        private MockContainer? _mocks;

        public Func<TBuilder> AddMock<TBuilder, TMock>()
            where TMock : class
            where TBuilder : BaseMockBuilder<TBuilder, TMock>, new() 
                => Mocks.AddMock<TBuilder, TMock>();

        public Func<MockBuilder<TMock>> AddMock<TMock>() where TMock : class
            => AddMock<MockBuilder<TMock>, TMock>();

        private TSut? _sut;

        protected TSut Sut
        {
            get
            {
                if(_sut == null || _sut.Equals(default(TSut)))
                {
                    _sut = ConstructSut();
                }
                return _sut;
            }
        }
        
        protected abstract TSut ConstructSut();
        
         
        protected virtual void Cleanup()
        {
            _sut = default;
            _mocks?.Clear();
            _fixture = null;
            _cancellationTokenSource = null;
        }


    }
}