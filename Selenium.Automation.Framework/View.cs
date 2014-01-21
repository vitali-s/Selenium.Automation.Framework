using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            WaitForDisplayed(BaseBy);
        }

        protected View(Element parentElement)
        {
            _container = parentElement;

            WaitForDisplayed(BaseBy);
        }

        public Element Base
        {
            get { return FindElement(BaseBy); }
        }

        protected abstract By BaseBy { get; }

        protected Element WaitForDisplayed(By by)
        {
            Element element = null;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (element == null && stopwatch.Elapsed < TimeSpan.FromSeconds(10))
            {
                try
                {
                    element = FindElement(BaseBy);
                }
                catch
                {
                    // TODO: Implement preformance logging solution.
                }
            }

            if (element == null)
            {
                // TODO: investigate cases
                throw new NullReferenceException(string.Format("Element '{0}' is not displayed on {1} in '{2}'.", BaseBy, GetType().Name, stopwatch.Elapsed));
            }

            return element;
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
