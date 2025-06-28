using System.Reflection;
using Xunit.Abstractions;

namespace EaFramework.Extensions;

public static class TestOutputHelperExtension
{
    public static string GetTestName(this ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember.GetValue(output);

        return test.DisplayName.Split("(")[0];
    }
}