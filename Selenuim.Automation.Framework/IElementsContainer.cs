using System.Collections.Generic;
using OpenQA.Selenium;

namespace Selenuim.Automation.Framework
{
    public interface IElementsContainer
    {
        IWebElement FindElement(By by);

        ICollection<IWebElement> FindElements(By by);
    }
}
