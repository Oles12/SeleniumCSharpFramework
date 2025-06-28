using AutoFixture.Xunit2;
using AventStack.ExtentReports;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;
using EaFramework.Config;
using EaFramework.Driver;
using EaFramework.Extensions;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

//using Theory = Xunit.TheoryAttribute;

namespace EaApplicationTest;

[Collection("Test collection")]
public class UnitTest1 : DriverFixture
{ 
    private readonly IHomePage _homePage;
    private readonly IProductPage _productPage;
    
    public ExtentReports _extentReport;
    protected ExtentTest test;

    /*public UnitTest1(IHomePage homePage, IProductPage productPage)
    {
       // var testSettings = ConfigReader.ReadConfig(); - the object will be created in container (Startup.cs) using Dependency Injection
       // _driverFixture = new DriverFixture(testSettings); - 
       // _driverFixture = driverFixture; // DriverFixture will get TestSettings from container automatically because of useing Dependancy injection
     //  _driverWait = new DriverWait(_driverFixture, testSettings); - is created in the container (Startup.cs) 
        _homePage = homePage;
        _productPage = productPage;
       // _extentReport = ExtentReport.InitializeExtentReport();
    }*/

    public UnitTest1(IHomePage homePage, IProductPage productPage, TestSettings testSettings,ITestOutputHelper output) : base(testSettings, output)
    {
        _homePage = homePage;
        _productPage = productPage;
    }
    
    [Theory]
    [AutoData] // AutoFixture.XUnit2 nuget package that generated data for model
    public void CreateProduct(Product product)
    {
       
      //  test = _extentReport.CreateTest("CreateProduct").Info("Test started");
        // HomePage
        /*var homePage = new HomePage(_driverWait); - created in container in Startup.cs
        var productPage = new ProductPage(_driverWait);*/

        // Click the Product Link
      //  test.Log(Status.Info, "CLick Product");
        _homePage.ClickProduct();
        
        // Create Product
        _productPage.ClickCreateButton();
        _productPage.CreateProduct(product);
        _productPage.PerformClickOnSpecialValue(product.Name, "Details");
        
        _productPage.GetProductName().Should().Be(product.Name + 1);
       // test.Log(Status.Pass, "Test executed successfully");

      try
      {
          RecordTestResult(CurrentTestName, TestResult.Pass);
      }
      catch (Exception e)
      {
          RecordTestResult(CurrentTestName, TestResult.Fail);
      }
       // RecordTestResult(CurrentTestName, TestResult.Pass);
      //  _extentReport.Flush();
    }
}