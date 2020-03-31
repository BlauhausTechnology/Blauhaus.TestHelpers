using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution;

namespace Blauhaus.Common.TestHelpers.Hotchocolate.Extensions
{
    public static class ObjectExtensions
    {
        public static List<OrderedDictionary> ToOrderedDictionaryList(this object objectInput)
        {
            var list = (List<object>) objectInput;
            return list.Select(x => (OrderedDictionary) x).ToList();
        }
    }
}