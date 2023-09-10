using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Microsoft.Extensions.Options;

namespace Blauhaus.TestHelpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TOptions AddMockOptions<TOptions>(this IServiceCollection services, Func<TOptions> optionsBuilder) where TOptions : class, new()
        {
            var options = new TOptions();
            var mockOptions = new Mock<IOptions<TOptions>>();
            mockOptions.Setup(x => x.Value).Returns(optionsBuilder.Invoke);
            services.AddSingleton(mockOptions.Object);

            return options;
        }
    }
}