using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.Kernel;

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

        protected override TBuilder WithProperty<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            return AddCustomization(expression, ()=> value); 
        } 
        
        public TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, Func<TProperty> func)
        {
            return AddCustomization(expression, func); 
        } 

        public TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, IBuilder<TProperty> builder)
        {
            return AddCustomization(expression, () => builder.Object); 
        }

        private TBuilder AddCustomization<TProperty>(Expression<Func<T, TProperty>> expression, Func<TProperty> func)
        {
            var newCustomization = new OverridePropertyBuilder<T, TProperty>(expression, func);

            var existingCustomizationForThisProperty = _fixture.Customizations.FirstOrDefault(x =>
                x is OverridePropertyBuilder<T, TProperty> existingCustomization && 
                existingCustomization.PropertyInfo.Name == newCustomization.PropertyInfo.Name);

            if (existingCustomizationForThisProperty != null)
            {
                _fixture.Customizations.Remove(existingCustomizationForThisProperty);
            }

            _fixture.Customizations.Add(newCustomization);
            
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
        public  readonly PropertyInfo PropertyInfo;
        private readonly Func<TProp> _value;

        public OverridePropertyBuilder(Expression<Func<T, TProp>> expr, TProp value)
        {
            PropertyInfo = (expr.Body as MemberExpression)?.Member as PropertyInfo ??
                            throw new InvalidOperationException("invalid property expression");
            _value = ()=> value;
        }
        public OverridePropertyBuilder(Expression<Func<T, TProp>> expr, Func<TProp> value)
        {
            PropertyInfo = (expr.Body as MemberExpression)?.Member as PropertyInfo ??
                           throw new InvalidOperationException("invalid property expression");
            _value = value;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;
            if (pi == null)
                return new NoSpecimen();

            var camelCase = Regex.Replace(PropertyInfo.Name, @"(\w)(.*)",
                m => m.Groups[1].Value.ToLower() + m.Groups[2]);

            if (pi.ParameterType != typeof(TProp) || pi.Name != camelCase)
                return new NoSpecimen();

            return _value.Invoke();
        }
    }
}