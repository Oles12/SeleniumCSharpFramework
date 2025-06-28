using Xunit;

namespace EaFramework.Extensions;

// IAsyncLifetime:
// Our report methods are async and we can't make constructors async/
// By using xUnit's IAsyncLifetime we can make our test nethod async/ IAymcLifetime will force to implement 2 methods:
// InitializeAsync() and DisposeAsync()

// ITestOutputHelper:
// Used to write test Diagnostic Message
// This will write to test output, not in the console output
// xUnit will insert it in the test class constructor

// Collection fixtures: It allows you to create a single test context and share it among tests in several test classes

public class TestHooks : IAsyncLifetime
{
    public Task InitializeAsync()
    {
        ExtentReport.InitializeExtentReport();
        // Nothing initialize ar the start of the test run
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
       ExtentReport.FlushReport(); // only flush once after all tests are  done
       return Task.CompletedTask;
    }
}