using System;
using System.Linq.Expressions;

namespace Blauhaus.TestHelpers.Builders._Base
{

    public interface IBuilder<out T>
    {
        T Object { get; }
    }
    
    public interface IBuilder<out TBuilder, T> : IBuilder<T>
    {
        TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value);

    }
}