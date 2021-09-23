using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.TestHelpers.BaseTests
{
    public abstract class BaseServiceTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {
        private IServiceCollection? _services;
        protected IServiceCollection Services => _services ??= new ServiceCollection();

        protected void AddService<T>(Func<IServiceProvider, T> func) where T : class
        {
            Services.AddSingleton<T>(func);
        }

        protected  void AddService<T>(T service) where T : class
        {
            Services.AddSingleton<T>(service);
        }

        protected override TSut ConstructSut()
        {
            Services.TryAddTransient<TSut>();
            
            var serviceProvider = Services.BuildServiceProvider();
            BeforeConstructSut(serviceProvider);

            return serviceProvider.GetRequiredService<TSut>();
        }

        protected virtual void BeforeConstructSut(IServiceProvider serviceProvider)
        {
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _services = null;
        }
         
    }
}