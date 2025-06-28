using OpenQA.Selenium;

namespace EaFramework.Driver;

public interface IDriverWait
{
    IWebElement FindElement(By elementLocator);
    IEnumerable<IWebElement> FindElements(By elementLocator);
}