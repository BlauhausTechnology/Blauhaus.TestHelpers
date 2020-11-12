using AutoFixture;
using Blauhaus.TestHelpers.Builders._Base;

namespace Blauhaus.TestHelpers.Builders
{
    public abstract class BaseFixtureBuilder<TBuilder, T> : BaseBuilder<TBuilder, T> 
        where TBuilder : BaseFixtureBuilder<TBuilder, T>
        where T : new()
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