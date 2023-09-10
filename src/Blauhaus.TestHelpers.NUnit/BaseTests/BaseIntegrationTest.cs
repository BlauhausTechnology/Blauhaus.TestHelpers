using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Onsight.IntegrationTests.Base;

namespace Blauhaus.TestHelpers.NUnit.BaseTests;

public abstract class BaseIntegrationTest<TContext> 
    where TContext : BaseTestContext, new()
{

    protected abstract TContext? BaseContext { get; set; }
    protected TContext Context => BaseContext ?? throw new InvalidOperationException("Context not ininitialized");

    protected T Resolve<T>() where T : notnull => Context.ServiceProvider.GetRequiredService<T>();


    [OneTimeSetUp]
    public virtual async Task SetupAsync()
    {
        if (BaseContext == null)
        {
            BaseContext = new TContext();
            BaseContext.Setup();
            await BaseContext.InitializeAsync();
        }
    }

    [SetUp]
    public virtual void Setup()
    {
    }

    [TearDown]
    public virtual void Teardown()
    {
    }
     
}