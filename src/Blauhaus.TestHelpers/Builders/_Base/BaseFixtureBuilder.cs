using AutoFixture;

namespace Blauhaus.TestHelpers.Builders._Base
{
    public abstract class BaseFixtureBuilder<TBuilder, T> : BaseBuilder<TBuilder, T> 
        where TBuilder : BaseFixtureBuilder<TBuilder, T>
        where T : class, new()
    {
        private readonly T _t;

        protected BaseFixtureBuilder()
        {
            _t = new Fixture().Create<T>();
        }

        protected override T Construct()
        {
            return _t;
        }
    }
}