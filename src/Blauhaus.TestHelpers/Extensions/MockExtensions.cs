using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Blauhaus.TestHelpers.Extensions;

public static class MockExtensions
{
     public static TInvocation GetInvocation<TInvocation>(this Mock mock, string? method = null, int invocationIndex = 0) 
    {

        List<IInvocation> invocations;

        if (method != null)
        {
            invocations = mock.Invocations.Where(x => x.Method.Name == method).ToList();
            if (invocations.Count == 0)
            {
                throw new Exception($"Method named {method} was not invoked on {mock.Object.GetType().Name}");
            }
        }
        else
        {
            invocations = mock.Invocations.ToList();
        }

        var invocationsMatchingType = invocations.Where(x => x.Arguments.FirstOrDefault(y => y != null &&  y.GetType() == typeof(TInvocation)) != null).ToList();
        if (invocationsMatchingType.Count == 0)
        { 
            throw new Exception($"{mock.Object.GetType().Name} was not invoked with an argument type of {typeof(TInvocation)}");
        }
        else if (invocationIndex > invocationsMatchingType.Count)
        {
            throw new Exception($"{mock.Object.GetType().Name} was invoked with an argument type of {typeof(TInvocation)} {invocationsMatchingType.Count} so there is no call sequence {invocationIndex}");
        }
        else
        {
            return (TInvocation)invocationsMatchingType[invocationIndex].Arguments.First(x => x.GetType() == typeof(TInvocation));
        }

        throw new Exception("Could not find invocation");
    }

    public static IReadOnlyList<TInvocation> GetInvocations<TInvocation>(this Mock mock, string? method = null) 
    {

        List<IInvocation> invocations;

        if (method != null)
        {
            invocations = mock.Invocations.Where(x => x.Method.Name == method).ToList();
            if (invocations.Count == 0)
            {
                throw new Exception($"Method named {method} was not invoked on {mock.Object.GetType().Name}");
            }
        }
        else
        {
            invocations = mock.Invocations.ToList();
        }

        var invocationsMatchingType = invocations.Where(x => x.Arguments.FirstOrDefault(y => y != null &&  y.GetType() == typeof(TInvocation)) != null).ToList();
        if (invocationsMatchingType.Count == 0)
        {
            throw new Exception($"{mock.Object.GetType().Name} was not invoked with an argument type of {typeof(TInvocation)}");
        }

        return invocationsMatchingType.Select(x => (TInvocation)x).ToArray();

    }
}