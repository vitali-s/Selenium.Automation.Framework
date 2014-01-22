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

            FindElement(BaseBy);
        }

        protected View(Element parentElement)
        {
            _container = parentElement;

            FindElement(BaseBy);
        }

        public Element Base
        {
            get { return FindElement(BaseBy); }
        }

        protected abstract By BaseBy { get; }

        protected Element FindElement(By by)
        {
            IWebElement element = null;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (element == null && stopwatch.Elapsed < TimeSpan.FromSeconds(10))
            {
                try
                {
                    element = _container.FindElement(by);
                }
                catch
                {
                    // TODO: Implement preformance logging solution.
                }
            }

            if (element == null)
            {
                return null;
            }

            return new Element(Browser, element);
        }

        protected ICollection<Element> FindElements(By by)
        {
            ICollection<IWebElement> elements = new List<IWebElement>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (elements.Count == 0 && stopwatch.Elapsed < TimeSpan.FromSeconds(10))
            {
                try
                {
                    elements = _container.FindElements(by);
                }
                catch
                {
                    // TODO: Implement preformance logging solution.
                }
            }

            return elements
                .Where(element => element != null)
                .Select(element => new Element(Browser, element))
                .ToList();
        }
    }
}