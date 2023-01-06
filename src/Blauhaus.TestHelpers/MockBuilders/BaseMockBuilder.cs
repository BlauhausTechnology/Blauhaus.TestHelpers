using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Blauhaus.TestHelpers.Builders.Base;
using Moq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace Blauhaus.TestHelpers.MockBuilders
{

    public abstract class BaseMockBuilder<TMockBuilder, TMock> : IMockBuilder<TMockBuilder, TMock>
        where TMockBuilder : BaseMockBuilder<TMockBuilder, TMock>
        where TMock : class
    {

        protected IFixture MyFixture => _fixture ??= new Fixture();
        private IFixture? _fixture;

        protected TProperty Get<TProperty>() => MyFixture.Create<TProperty>();

        public Mock<TMock> Mock { get; } = new();
        public TMock Object => Mock.Object;
        public List<TMock> ToList => new() {Mock.Object};
        public static TMock Default => Activator.CreateInstance<TMockBuilder>().Object;

        public TMockBuilder With<TProperty>(Expression<Func<TMock, TProperty>> expression, TProperty value)
        {
            Mock.Setup(expression).Returns(value);
            return (TMockBuilder) this;
        }
        
        public TMockBuilder Setup<TProperty>(Expression<Func<TMock, TProperty>> expression, TProperty value)
        {
            Mock.Setup(expression).Returns(value);
            return (TMockBuilder) this;
        }
        public TMockBuilder SetupAsync<TProperty>(Expression<Func<TMock, Task<TProperty>>> expression, TProperty value)
        {
            Mock.Setup(expression).ReturnsAsync(value);
            return (TMockBuilder) this;
        }
         
        public TMockBuilder Verify(Expression<Action<TMock>> expression)
        {
            Mock.Verify(expression);
            return (TMockBuilder) this;
        }
        public TMockBuilder Verify(Expression<Action<TMock>> expression, Times times)
        {
            Mock.Verify(expression, times);
            return (TMockBuilder) this;
        }

        protected T Is<T>(Expression<Func<T, bool>> func) => It.Is(func);
        protected string IsString(Expression<Func<string, bool>> func) => It.Is(func);

        protected T Any<T>() => It.IsAny<T>();
        protected  CancellationToken AnyToken => It.IsAny<CancellationToken>();
        protected string AnyString => It.IsAny<string>();
    }
}