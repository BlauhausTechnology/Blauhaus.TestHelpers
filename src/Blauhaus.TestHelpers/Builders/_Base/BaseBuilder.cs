using System;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.Dsl;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Blauhaus.TestHelpers.Builders._Base
{
    public abstract class BaseBuilder<TBuilder, T> : IBuilder<TBuilder, T>
        where TBuilder :  BaseBuilder<TBuilder, T>
    {

        private readonly ICustomizationComposer<T> _fixture;
        private T _object;

        protected Random Random;
        protected readonly IFixture MyFixture;

        protected BaseBuilder()
        {
            Random = new Random();
            MyFixture = new Fixture();
            _fixture = MyFixture.Build<T>();
        }

        public TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            _fixture.With(expression);
            return this as TBuilder;
        }

        public virtual T Object
        {
            get
            {
                if (_object == null) _object = _fixture.Create();
                return _object;
            }
        }

        public static T Default => Activator.CreateInstance<TBuilder>().Object;
        
    }
}