using Xunit;

namespace Toss.Tests
{
    [CollectionDefinition(nameof(Names.ServiceCollection))]
    public class DataFixtureCollection : ICollectionFixture<DataFixture>
    {

    }
}