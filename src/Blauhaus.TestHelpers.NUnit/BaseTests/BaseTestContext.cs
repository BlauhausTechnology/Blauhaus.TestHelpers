using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blauhaus.TestHelpers.NUnit.BaseTests;
 
public abstract class BaseTestContext : IAsyncInitializable, IAsyncDisposable
{
    public IServiceProvider ServiceProvider => _serviceProvider ?? throw new InvalidOperationException("Service Provider not initialized yet");

    private ServiceProvider? _serviceProvider;
    protected T Resolve<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

    public void Setup()
    {
        if (_serviceProvider is null)
        {
            var services = new ServiceCollection();

            //logging
            var loggerProvider = new NUnitLoggerProvider(); 
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(loggerProvider);
            services
                .AddSingleton<ILoggerFactory>(loggerFactory)
                .AddLogging();

            services
                .AddServiceLocator();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }
    }

    protected MockContainer Mocks => _mocks ??= new MockContainer();
    private MockContainer? _mocks;
    
    protected Func<TBuilder> AddMock<TBuilder, TMock>()
        where TMock : class
        where TBuilder : BaseMockBuilder<TBuilder, TMock>, new() 
        => Mocks.AddMock<TBuilder, TMock>();

    protected Func<MockBuilder<TMock>> AddMock<TMock>() where TMock : class
        => AddMock<MockBuilder<TMock>, TMock>();
     
    protected virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services;
    }
    
    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual ValueTask DisposeAsync()
    {
        _serviceProvider?.Dispose();
        _serviceProvider = null;

        return ValueTask.CompletedTask;
    }

}