using System;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.TestHelpers.BaseTests
{
    public class BaseServiceTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {
        protected IServiceCollection Services;

        protected override TSut ConstructSut()
        {
            return Services.BuildServiceProvider().GetRequiredService<TSut>();
        }


        protected override void Cleanup()
        {
            base.Cleanup();
            Services = new ServiceCollection();
        }
    }
}