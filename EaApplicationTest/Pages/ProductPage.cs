using AventStack.ExtentReports;
using EaApplicationTest.Models;
using EaFramework.Extensions;
using OpenQA.Selenium;

namespace EaApplicationTest.Pages;

public interface IProductPage
{
    void ClickCreateButton();
    void CreateProduct(Product product);
    void PerformClickOnSpecialValue(string name, string operation);
    string GetProductName();
}

public class ProductPage : IProductPage
{
    private readonly IDriverWait _driver;

    public ProductPage(IDriverWait driver)
    {
        _driver = driver;
    }
    
    public IWebElement txtName =>  _driver.FindElement(By.Id("Name")); 
    public IWebElement txtDescription =>  _driver.FindElement(By.Id("Description")); 
    public IWebElement txtPrice =>  _driver.FindElement(By.Id("Price")); 
    public IWebElement dropDownProductType =>  _driver.FindElement(By.Id("ProductType")); 
    public IWebElement lnkCreate =>  _driver.FindElement(By.LinkText("Create")); 
    public IWebElement btnCreate =>  _driver.FindElement(By.Id("Create")); 
    public IWebElement tableList =>  _driver.FindElement(By.CssSelector(".table"));

    public void ClickCreateButton()
    {
        lnkCreate.Click();
        ExtentReport._test.Log(Status.Info, "Click Product");
    }

   

    public void CreateProduct(Product product)
    {
        txtName.Clear();
        txtName.SendKeys(product.Name);
        txtDescription.SendKeys(product.Description);
        txtPrice.SendKeys(product.Price.ToString());
        dropDownProductType.SelectDropDownByText(product.ProductType.ToString());
        
        btnCreate.Click();
        ExtentReport._test.Log(Status.Info, "Create Product");///
    }

    public string GetProductName() => txtName.Text;
    public void PerformClickOnSpecialValue(string name, string operation)
    {
        tableList.PerformActionOnCell("5", "Name", name, operation);
        ExtentReport._test.Log(Status.Info, $"Click on Special Value for the name:{name}, operation:{operation}");
    }
}