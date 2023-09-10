using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Blauhaus.TestHelpers.NUnit.BaseTests;

/// <summary>
/// Writes ILogger output to the test console
/// </summary>
public class NUnitLogger : ILogger
{
    private readonly string _categoryName;

    public NUnitLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
        Exception exception, Func<TState, Exception, string> formatter)
    {
        TestContext.WriteLine($"{_categoryName.Split('.').LastOrDefault()} - {logLevel} - {formatter(state, exception)}");
    }
}

public class NUnitLoggerProvider : ILoggerProvider
{
    public void Dispose() {}

    public ILogger CreateLogger(string categoryName)
    {
        return new NUnitLogger(categoryName);
    }
}