using System;
using System.Linq.Expressions;

namespace Blauhaus.TestHelpers.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns when the preficate is true or the timeout expires
        /// </summary>
        public static T WaitFor<T>(this T obj,  Expression<Func<T, bool>> predicate, int timeoutMs = 1000)
        {
            var startMs = DateTime.UtcNow.Ticks/10000;
            while (!predicate.Compile().Invoke(obj))
            {
                if (DateTime.UtcNow.Ticks/10000 - startMs >= timeoutMs)
                {
                    break;
                }
            }
            return obj;
        }
    }
}