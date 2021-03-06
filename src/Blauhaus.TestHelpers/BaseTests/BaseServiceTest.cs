﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.TestHelpers.BaseTests
{
    public abstract class BaseServiceTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {
        protected IServiceCollection Services;

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
            Services.AddSingleton<TSut>();
            return Services.BuildServiceProvider().GetRequiredService<TSut>();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            Services = new ServiceCollection();
        }
    }
}