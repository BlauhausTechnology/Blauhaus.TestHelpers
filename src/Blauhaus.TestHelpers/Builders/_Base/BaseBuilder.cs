using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Blauhaus.TestHelpers.Builders._Base
{
    public abstract class BaseBuilder<TBuilder, T> : IBuilder<TBuilder, T>
        where TBuilder :  BaseBuilder<TBuilder, T>
    {

        private Random _random;
        private PropertyInfo[] _properties;
        private T _object;


        protected Random Random => 
            _random ??= new Random(); 

        protected IEnumerable<PropertyInfo> Properties => 
            _properties ??= typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
         

        protected BaseBuilder()
        {
        }

        public TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            var propertyName = (expression.Body as MemberExpression)?.Member.Name;
            var propertyToSet = Properties.FirstOrDefault(property => property.Name == propertyName);

            if (propertyToSet != null)
            {
                propertyToSet.SetValue(Object, value);
            }
            return this as TBuilder;
        }


        public T Object
        {
            get
            {
                if (_object == null)
                {
                    _object = Construct();
                }
                return _object;
            } 
        }

        protected virtual T Construct()
        {
            return (T)Activator.CreateInstance(typeof(T));
        } 

        public static T Default => Activator.CreateInstance<TBuilder>().Object;
        
    }
}