using System.Reflection;
using System.Reflection.Emit;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EaFramework.Extensions;

public static class ExtentReport
{
    public static ExtentReports _extent;
    
    public static void InitializeExtentReport()
    {
        if (_extent == null)
        {
            var timestamp = DateTime.Now.ToString("ddMMyyyy_HHmm");
            var reportFileName = $"TestReport_{timestamp}.html";
            string extentReport = Path.Combine(Directory.GetCurrentDirectory(), reportFileName);
            var spark = new ExtentSparkReporter(extentReport);
            _extent = new ExtentReports();
            _extent.AttachReporter(spark); 
        }
        
        /*extent.CreateTest("MyFirstTest")
            .Log(Status.Pass, "This is a logging event for MyFirstTest, and it passed!");
        extent.Flush();*/

    }

    public static ExtentTest CreateTest(string testName)
    {
        return _extent.CreateTest(testName);
    }

    public static void FlushReport()
    {
        _extent.Flush();
    }
}