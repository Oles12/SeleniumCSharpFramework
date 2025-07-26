using AventStack.ExtentReports;
using EaApplicationTest.Models;
using EaFramework.Extensions;
using OpenQA.Selenium;

namespace EaApplicationTest.Pages;

public interface IProductPage
{
    void ClickCreateButton(ExtentTest test);
    void CreateProduct(Product product, ExtentTest test);
    void PerformClickOnSpecialValue(string name, string operation, ExtentTest test);
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

    public void ClickCreateButton(ExtentTest test)
    {
        test.Log(Status.Info, "Click Create button on Product page");
        lnkCreate.Click();
    }

   

    public void CreateProduct(Product product, ExtentTest test)
    {
        test.Log(Status.Info, "Create a new product");
        txtName.Clear();
        txtName.SendKeys(product.Name);
        txtDescription.SendKeys(product.Description);
        txtPrice.SendKeys(product.Price.ToString());
        dropDownProductType.SelectDropDownByText(product.ProductType.ToString());
        
        btnCreate.Click();
    }

    public string GetProductName() => txtName.Text;
    public void PerformClickOnSpecialValue(string name, string operation, ExtentTest test)
    {
        test.Log(Status.Info, $"Click on Special Value for the name:{name}, operation:{operation}");
        tableList.PerformActionOnCell("5", "Name", name, operation);
    }
}