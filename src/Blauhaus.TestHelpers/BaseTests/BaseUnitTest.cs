using System;
using System.Threading;
using AutoFixture;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.TestHelpers.BaseTests
{
    public abstract class BaseUnitTest<TSut> where TSut : class
    {
        protected IFixture MyFixture => _fixture ??= new Fixture();
        private IFixture _fixture;

        protected CancellationToken CancelToken => _cancellationTokenSource.Token;
        private CancellationTokenSource _cancellationTokenSource;

        protected MockContainer Mocks => _mocks ??= new MockContainer();
        private MockContainer _mocks;

        public Func<TBuilder> AddMock<TBuilder, TMock>()
            where TMock : class
            where TBuilder : BaseMockBuilder<TBuilder, TMock>, new() 
                => Mocks.AddMock<TBuilder, TMock>();

        public Func<MockBuilder<TMock>> AddMock<TMock>() where TMock : class
            => AddMock<MockBuilder<TMock>, TMock>();

        private TSut _sut;
        protected TSut Sut => _sut ??= ConstructSut();
        protected abstract TSut ConstructSut();


        protected virtual void Cleanup()
        {
            _sut = null;
            _fixture = null;
            _mocks?.Clear();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}