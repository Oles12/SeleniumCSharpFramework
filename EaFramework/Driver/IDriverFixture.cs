using System;
using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace EaFramework.Driver;

public interface IDriverFixture
{
    IWebDriver Driver { get; }
    ExtentTest Test { get; }

    void ExecuteTest(Action testAction);
    
   string TakeScreenshotAsPath(string fileName);
}