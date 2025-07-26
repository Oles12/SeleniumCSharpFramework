using System.Reflection;
using AventStack.ExtentReports;
using EaFramework.Config;
using EaFramework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
    public IWebDriver Driver { get; }
    public ExtentTest Test { get; private set; }
    private readonly TestSettings _testSettings;
    private readonly ITestOutputHelper _output;
    private static readonly Dictionary<string, TestResult> _testResults = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DriverFixture"/> class.
    /// </summary>
    /// <param name="testSettings">The test settings.</param>
    /// <param name="output">The xUnit test output helper.</param>
    public DriverFixture(TestSettings testSettings, ITestOutputHelper output)
    {
        _output = output;
        _testSettings = testSettings;
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        Driver.Navigate().GoToUrl(testSettings.ApplicationUrl); //"http://localhost:8000" // for remote:  "http://localhost:5001"
    }

    
    /// <summary>
    /// Gets the name of the current test.
    /// </summary>
    /// <returns>The test name.</returns>
    public string GetTestName()
    {
        var type = _output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(_output)!;
        return test.TestCase.TestMethod.Method.Name;  // The methodName variable now holds the name of the test method.
    }
    
    public void ExecuteTest(Action testAction)
    {
        var testName = GetTestName();// Use method to extract test name
        Test = ExtentReport.CreateTest(testName);
        Test.Log(Status.Info, "Test started");
        
        try
        {
            testAction();
            _testResults[testName] = TestResult.Pass;
            Test.Log(Status.Pass, "Test executed successfully");
        }
        catch (Exception e)
        {
            _testResults[testName] = TestResult.Fail;
            Test.Fail(e.Message);
            Test.CaptureScreenshotAndAttachToExtentReport(Driver, $"{testName}_failure.png");
            throw; 
        }
    }

    /// <summary>
    /// Represents the result of a test.
    /// </summary>
    public enum TestResult
    {
        Pass,
        Fail,
        Unknown
    }
    
    /// <summary>
    /// Gets the local WebDriver instance based on the browser type specified in the test settings.
    /// </summary>
    /// <returns>An <see cref="IWebDriver"/> instance.</returns>
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
    
    /// <summary>
    /// Gets the remote WebDriver instance for Selenium Grid execution.
    /// </summary>
    /// <returns>An <see cref="IWebDriver"/> instance for remote execution.</returns>
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

    /// <summary>
    /// Takes a screenshot and saves it to the specified path.
    /// </summary>
    /// <param name="fileName">The name of the screenshot file.</param>
    /// <returns>The full path of the saved screenshot.</returns>
    public string TakeScreenshotAsPath(string fileName)
    {
        var screenshot = Driver.TakeScreenshot();
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}//{fileName}.png)";
        screenshot.SaveAsFile(path);
        return path;
    }

    /// <summary>
    /// Represents the browser type for test execution.
    /// </summary>
    public enum BrowserType
    {
        Chrome,
        Firefox,
        Safari,    
        EdgeChromium  
    }

    /// <summary>
    /// Disposes the WebDriver instance.
    /// </summary>
    public void Dispose()
    {
       Driver.Quit();
    }
}