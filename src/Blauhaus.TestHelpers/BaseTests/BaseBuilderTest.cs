using Blauhaus.TestHelpers.Builders.Base;

namespace Blauhaus.TestHelpers.BaseTests
{
    public abstract class BaseBuilderTest<TSut, TBuilder> : BaseUnitTest<TSut> 
        where TSut : class
        where TBuilder : BaseRecordFixtureBuilder<TBuilder, TSut>, new()
    {
        protected readonly TBuilder Builder;

        protected BaseBuilderTest()
        {
            Builder = new TBuilder();
        }

        protected override TSut ConstructSut()
        {
            return Builder.Object;
        }
    }
}