using EaFramework.Extensions;

namespace EaApplicationTest;

[CollectionDefinition("Test collection", DisableParallelization = true)]
public class TestCollection : ICollectionFixture<TestHooks>
{
    
}