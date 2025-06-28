using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using EaFramework.Config;
using EaFramework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V136.DOM;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.Extensions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EaFramework.Driver;

public class DriverFixture : IDriverFixture, IDisposable
{
    private readonly TestSettings _testSettings;
    
    public ITestOutputHelper _output; //
   // public ExtentReports _extentReport;
    public ExtentTest CurrentTest { get; private set; } // to add the test name into the report
    private static readonly Dictionary<string, TestResult> _testResults = new Dictionary<string, TestResult>();
    protected string CurrentTestName;
    
    public IWebDriver Driver { get;}
    public DriverFixture(TestSettings testSettings)
    {
        _output = new TestOutputHelper();
        _testSettings = testSettings;
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        Driver.Navigate().GoToUrl(testSettings.ApplicationUrl); //"http://localhost:8000"
      //  _extentReport = ExtentReport.InitializeExtentReport();
       // CreateTests();
    }
    
    public DriverFixture(TestSettings testSettings, ITestOutputHelper output)
    {
        _output = output; 
        _testSettings = testSettings;
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        Driver.Navigate().GoToUrl(testSettings.ApplicationUrl); //"http://localhost:8000"
       //  _extentReport = ExtentReport.InitializeExtentReport();
        // CreateTests();

        CurrentTestName = _output.GetTestName(); // Use method to extract test name
        CurrentTest = ExtentReport._extent.CreateTest(CurrentTestName);
        ExtentReport._test = CurrentTest;
        RecordTestResult(CurrentTestName, TestResult.Unknown);
    }

    private ExtentTest CreateTests() => ExtentReport._extent.CreateTest(GetTestName());
    
    public string GetTestName()//////
    {
        var type = _output.GetType();
        var testMember = type.GetField("test", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(_output)!;
        var methodName = test.TestCase.TestMethod.Method.Name;
        // The methodName variable now holds the name of the test method.

        return methodName;
    }

    protected void RecordTestResult(string testName, TestResult result)
    {
        // Store the test result in the static dictionary
        _testResults[testName] = result;
    }

    public enum TestResult
    {
        Pass,
        Fail,
        Unknown
    }
    private IWebDriver GetWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome=> new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            BrowserType.Safari => new SafariDriver(),
            BrowserType.EdgeChromium => new EdgeDriver(),
            _ => new ChromeDriver()
        };
    }
    
    // To perform operation in Selenium Web Driver Grid
    private IWebDriver GetRemoteWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome=> new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions()),
            BrowserType.Firefox => new RemoteWebDriver(_testSettings.GridUri, new FirefoxOptions()),
            BrowserType.Safari => new RemoteWebDriver(_testSettings.GridUri, new SafariOptions()),
            BrowserType.EdgeChromium => new RemoteWebDriver(_testSettings.GridUri, new EdgeOptions()),
            _ => new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions())
        };
    }

    public string TakeScreenshotAsPath(string fileName)
    {
        var screenshot = Driver.TakeScreenshot();
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}//{fileName}.png)";
        screenshot.SaveAsFile(path);
        return path;
    }

    public enum BrowserType
    {
        Chrome,
        Firefox,
        Safari,    
        EdgeChromium  
    }

    public void Dispose()
    {
       //_extentReport.Flush();
       Driver.Quit();
       string path = System.AppDomain.CurrentDomain.BaseDirectory + "screenshot";
       bool exists = Directory.Exists(path); 
       if (!exists)
       {
           Directory.CreateDirectory(path);
       }

       TestResult currentTestResult = _testResults.GetValueOrDefault(CurrentTestName);

       
       switch (currentTestResult)
       {
           case TestResult.Pass:
               CurrentTest.Log(Status.Pass, CurrentTestName + " run successfully");
               break;
           case TestResult.Fail:
           case TestResult.Unknown:
               _output.WriteLine(CurrentTestName + " Test Failed");
               var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
               var screenshotFileName = $"{CurrentTestName}.png"; // $"Guid.NewGuid().png"
               var screenshotFilePath  = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), screenshotFileName);//
               //var screenshotFilePath  = path + "\" + screenshotFileName;
               screenshot.SaveAsFile(screenshotFilePath);//
               CurrentTest.AddScreenCaptureFromPath(screenshotFilePath, "Test failure screenshot");
               break;
       }
   
    }
}