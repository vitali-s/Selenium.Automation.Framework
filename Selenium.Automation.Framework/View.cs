using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Selenium.Automation.Framework
{
    public abstract class View : InfrastructureObject
    {
        private readonly IElementsContainer _container;

        protected View()
        {
            _container = Browser;
        }

        protected View(Element parentElement)
        {
            _container = parentElement;
        }

        protected virtual Element FindElement(By by)
        {
            IWebElement webElement = _container.FindElement(by);

            if (webElement == null)
            {
                return null;
            }

            return new Element(Browser, webElement);
        }

        protected virtual ICollection<Element> FindElements(By by)
        {
            return _container.FindElements(by)
                .Where(element => element != null)
                .Select(element => new Element(Browser, element))
                .ToList();
        }
    }
}
