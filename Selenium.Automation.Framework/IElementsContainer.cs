using System.Collections.Generic;
using OpenQA.Selenium;

namespace Selenium.Automation.Framework
{
    public interface IElementsContainer
    {
        IWebElement FindElement(By by);

        ICollection<IWebElement> FindElements(By by);
    }
}
