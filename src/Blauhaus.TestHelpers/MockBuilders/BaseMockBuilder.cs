﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using AutoFixture;
using Blauhaus.TestHelpers.Builders._Base;
using Moq;

namespace Blauhaus.TestHelpers.MockBuilders
{

    public abstract class BaseMockBuilder<TMockBuilder, TMock> : IBuilder<TMockBuilder, TMock>
        where TMockBuilder : BaseMockBuilder<TMockBuilder, TMock>
        where TMock : class
    {

        protected IFixture MyFixture => _fixture ??= new Fixture();
        private IFixture _fixture;

        protected TProperty Get<TProperty>() => MyFixture.Create<TProperty>();

        public readonly Mock<TMock> Mock = new Mock<TMock>();
        public TMock Object => Mock.Object;
        public List<TMock> ToList => new List<TMock>{Mock.Object};
        public static TMock Default => Activator.CreateInstance<TMockBuilder>().Object;

        public TMockBuilder With<TProperty>(Expression<Func<TMock, TProperty>> expression, TProperty value)
        {
            Mock.Setup(expression).Returns(value);
            return this as TMockBuilder;
        }

        protected T Is<T>(Expression<Func<T, bool>> func) => It.Is(func);
        protected string IsString(Expression<Func<string, bool>> func) => It.Is(func);

        protected T Any<T>() => It.IsAny<T>();
        protected  CancellationToken AnyToken => It.IsAny<CancellationToken>();
        protected string AnyString => It.IsAny<string>();
    }
}