using AutoFixture;

namespace Blauhaus.TestHelpers.BaseTests
{
    public abstract class BaseUnitTest<TSut> where TSut : class
    {
        protected IFixture MyFixture => _fixture ??= new Fixture();
        private IFixture _fixture;

        protected MockContainer Mocks => _mocks ??= new MockContainer();
        private MockContainer _mocks;

        private TSut _sut;
        protected TSut Sut => _sut ??= ConstructSut();
        protected abstract TSut ConstructSut();


        protected virtual void Cleanup()
        {
            _sut = null;
            _fixture = null;
            _mocks?.Clear();
        }
    }
}