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
    void EditProduct(Product product, ExtentTest test);
    Product GetProductFromList(string name);
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
    public IWebElement inputProductType => _driver.FindElement(By.Id("ProductType"));
    public IWebElement lnkCreate =>  _driver.FindElement(By.LinkText("Create")); 
    public IWebElement btnCreate =>  _driver.FindElement(By.Id("Create"));
    public IWebElement btnSave => _driver.FindElement(By.CssSelector(".btn-primary"));
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
        test.Log(Status.Info, $"Name set to '{product.Name}'");
        txtDescription.SendKeys(product.Description);
        test.Log(Status.Info, $"Description set to '{product.Description}'");
        txtPrice.SendKeys(product.Price.ToString());
        test.Log(Status.Info, $"Price set to '{product.Price}'");
        dropDownProductType.SelectDropDownByText(product.ProductType.ToString());
        test.Log(Status.Info, $"ProductType set to '{product.ProductType}'");
        
        btnCreate.Click();
    }

    public string GetProductName() => txtName.Text;

    public void EditProduct(Product product, ExtentTest test)
    {
        test.Log(Status.Info, "Editing product");
        txtName.Clear();
        txtName.SendKeys(product.Name);
        test.Log(Status.Info, $"Name changed to '{product.Name}'");
        txtDescription.Clear();
        txtDescription.SendKeys(product.Description);
        test.Log(Status.Info, $"Description changed to '{product.Description}'");
        txtPrice.Clear();
        txtPrice.SendKeys(product.Price.ToString());
        test.Log(Status.Info, $"Price changed to '{product.Price}'");
        inputProductType.Clear();
        inputProductType.SendKeys(product.ProductType.ToString());
        test.Log(Status.Info, $"ProductType changed to '{product.ProductType}'");
        btnSave.Click();
    }

    public Product GetProductFromList(string name)
    {
        var table = tableList.GetTableData();
        var row = table.FirstOrDefault(x => x.ColumnValue == name);
        if (row != null)
        {
            return new Product
            {
                Name = row.ColumnValue,
                Description = table.FirstOrDefault(x => x.RowNumber == row.RowNumber && x.ColumnName == "Description")?.ColumnValue,
                Price = int.Parse(table.FirstOrDefault(x => x.RowNumber == row.RowNumber && x.ColumnName == "Price")?.ColumnValue ?? "0"),
                ProductType = (ProductType)Enum.Parse(typeof(ProductType), table.FirstOrDefault(x => x.RowNumber == row.RowNumber && x.ColumnName == "ProductType")?.ColumnValue ?? "CPU")
            };
        }
        return null;
    }

    public void PerformClickOnSpecialValue(string name, string operation, ExtentTest test)
    {
        test.Log(Status.Info, $"Click on Special Value for the name:{name}, operation:{operation}");
        tableList.PerformActionOnCell("5", "Name", name, operation);
    }
}