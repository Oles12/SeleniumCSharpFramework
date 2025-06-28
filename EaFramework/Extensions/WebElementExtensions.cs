using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EaFramework.Extensions;

public static class WebElementExtensions
{
    public static void SelectDropDownByText(this IWebElement element, string text)
    {
        SelectElement select = new SelectElement(element);
        select.SelectByText(text);
    }
    
    public static void SelectDropDownByValue(this IWebElement element, string value)
    {
        SelectElement select = new SelectElement(element);
        select.SelectByValue(value);
    }
    
    public static void SelectDropDownByIndex(this IWebElement element, int index)
    {
        SelectElement select = new SelectElement(element);
        select.SelectByIndex(index);
    }

    public static void ClearAndEnterText(this IWebElement element, string text)
    {
        element.Clear();
        element.SendKeys(text);
    }
}