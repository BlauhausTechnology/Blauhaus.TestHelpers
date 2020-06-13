using System;
using System.Linq.Expressions;

namespace Blauhaus.TestHelpers.Builders._Base
{
    public interface IBuilder<TBuilder, T>
    {
        TBuilder With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value);
        T Object { get; }
    }
}