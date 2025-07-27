using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace EaFramework.Extensions;

public static class ScreenshotExtension
{
    public static ExtentTest CaptureScreenshotAndAttachToExtentReport(this ExtentTest test, IWebDriver driver, string screenshotName)
    {
        var screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
        return test.AddScreenCaptureFromBase64String(screenshot, screenshotName);
    }
} 