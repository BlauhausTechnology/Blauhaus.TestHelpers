using System;
using System.Linq.Expressions;
using AutoFixture;

namespace Blauhaus.TestHelpers.Builders.Base
{
    public abstract class BaseReadonlyFixtureBuilder<TBuilder, T>: BaseBuilder<TBuilder, T>
        where TBuilder : BaseReadonlyFixtureBuilder<TBuilder, T> 
        where T : class
    {
        private readonly Fixture _fixture;

        protected BaseReadonlyFixtureBuilder()
        {
            _fixture = new Fixture();
        }

        public new TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            _fixture.Customize<T>(x => x.With(expression, value));
            return (TBuilder)this;
        } 
        
        protected override T Construct()
        {
            return _fixture.Create<T>();
        }
    }
}