using Blauhaus.TestHelpers.Builders.Base;
using Moq;

namespace Blauhaus.TestHelpers.MockBuilders;

public interface IMockBuilder<TBuilder, TMock> : IBuilder<TBuilder, TMock> where TMock : class
{
    public Mock<TMock> Mock { get; }
     
}