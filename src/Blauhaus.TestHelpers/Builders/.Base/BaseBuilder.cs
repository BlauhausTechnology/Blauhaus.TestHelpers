using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Blauhaus.TestHelpers.Builders.Base
{
    public abstract class BaseBuilder<TBuilder, T> : IBuilder<TBuilder, T>
        where TBuilder :  BaseBuilder<TBuilder, T>
        where T : class
    {

        private Random? _random;
        private PropertyInfo[]? _properties;
        private T? _object;


        protected Random Random => 
            _random ??= new Random(); 

        protected IEnumerable<PropertyInfo> Properties => 
            _properties ??= typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
         
        public virtual TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            var propertyName = (expression.Body as MemberExpression)?.Member.Name;
            var propertyToSet = Properties.FirstOrDefault(property => property.Name == propertyName);

            if (propertyToSet != null)
            {
                propertyToSet.SetValue(Object, value);
            }
            return (TBuilder) this;
        }


        public T Object => _object ??= Construct();

        protected virtual T Construct()
        {
            return (T)Activator.CreateInstance(typeof(T))!;
        } 

        public static T Default => Activator.CreateInstance<TBuilder>().Object;
        
    }
}