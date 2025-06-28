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
    public static ExtentTest _test;
    public static void InitializeExtentReport()
    {
        if (_extent == null)
        {
           // string extentReport = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/extentreport.html";
            string extentReport = Path.Combine(Directory.GetCurrentDirectory(), "TestReport.html");
            var spark = new ExtentSparkReporter(extentReport);
            _extent = new ExtentReports();
            _extent.AttachReporter(spark); 
        }

        
        /*extent.CreateTest("MyFirstTest")
            .Log(Status.Pass, "This is a logging event for MyFirstTest, and it passed!");
        extent.Flush();*/

    }

    public static void FlushReport()
    {
        _extent.Flush();
    }
}