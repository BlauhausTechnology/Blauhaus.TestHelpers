using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.TestHelpers.BaseTests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Blauhaus.TestHelpers.NUnit.BaseTests;

public abstract class BaseUnitTest<TSut> : BaseServiceTest<TSut> where TSut : class
{
    [SetUp]
    public virtual void Setup()
    {
        base.Cleanup();

        //logging
        var loggerProvider = new NUnitLoggerProvider(); 
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddProvider(loggerProvider);

        Services
            .AddSingleton<ILoggerFactory>(loggerFactory)
            .AddLogging();

    }
}