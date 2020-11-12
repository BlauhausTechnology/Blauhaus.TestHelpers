using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.Kernel;

namespace Blauhaus.TestHelpers.Builders._Base
{
    public abstract class BaseReadonlyFixtureBuilder<TBuilder, T>: BaseBuilder<TBuilder, T> where TBuilder : BaseReadonlyFixtureBuilder<TBuilder, T>
    {
        private readonly Fixture _fixture;

        protected BaseReadonlyFixtureBuilder()
        {

            _fixture = new Fixture();
        }


        public new TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            _fixture.Customizations.Add(new OverridePropertyBuilder<T, TProperty>(expression, value));
            return (TBuilder) this;
        } 


        protected override T Construct()
        {
            return _fixture.Create<T>();
        }
    }

    //courtesy of https://stackoverflow.com/questions/47391406/autofixture-and-read-only-properties

    internal class OverridePropertyBuilder<T, TProp> : ISpecimenBuilder
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly TProp _value;

        public OverridePropertyBuilder(Expression<Func<T, TProp>> expr, TProp value)
        {
            _propertyInfo = (expr.Body as MemberExpression)?.Member as PropertyInfo ??
                            throw new InvalidOperationException("invalid property expression");
            _value = value;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;
            if (pi == null)
                return new NoSpecimen();

            var camelCase = Regex.Replace(_propertyInfo.Name, @"(\w)(.*)",
                m => m.Groups[1].Value.ToLower() + m.Groups[2]);

            if (pi.ParameterType != typeof(TProp) || pi.Name != camelCase)
                return new NoSpecimen();

            return _value;
        }
    }
}