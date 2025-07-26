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
using System.Reflection;
using Xunit.Abstractions;

//using Theory = Xunit.TheoryAttribute;

namespace EaApplicationTest;

[Collection("Test collection")]
public class UnitTest1
{ 
    private readonly IHomePage _homePage;
    private readonly IProductPage _productPage;
    private readonly IDriverFixture _driverFixture;
    
    public UnitTest1(IHomePage homePage, IProductPage productPage, IDriverFixture driverFixture)
    {
        _homePage = homePage;
        _productPage = productPage;
        _driverFixture = driverFixture;
    }
    
    [Theory]
    [AutoData] // AutoFixture.XUnit2 nuget package that generated data for model
    public void CreateProductAndVerifyDetails(Product product)
    {
        _driverFixture.ExecuteTest(() =>
        {
            var test = _driverFixture.Test;
            _homePage.ClickProduct();
            _productPage.ClickCreateButton(test);
            _productPage.CreateProduct(product, test);
            _productPage.PerformClickOnSpecialValue(product.Name, "Details", test);

            _productPage.GetProductName().Should().Be(product.Name);
        });
    }
}