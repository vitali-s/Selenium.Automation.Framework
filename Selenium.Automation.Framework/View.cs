using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using Selenium.Automation.Framework.Logging;
using Selenium.Automation.Framework.Resources;

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
            IWebElement element = FindElements(by, null, c => c == null, locator => _container.FindElement(locator));

            if (element == null)
            {
                return null;
            }

            return new Element(Browser, element);
        }

        protected ICollection<Element> FindElements(By by)
        {
            return FindElements(by, new List<IWebElement>(), c => c.Count == 0, locator => _container.FindElements(locator))
                .Where(element => element != null)
                .Select(element => new Element(Browser, element))
                .ToList();
        }

        protected TResult FindElements<TResult>(By by, TResult defaultValue, Func<TResult, bool> check, Func<By, TResult> find)
        {
            TResult elements = defaultValue;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (check(elements) && stopwatch.Elapsed < TimeSpan.FromSeconds(60))
            {
                try
                {
                    elements = find(by);
                }
                catch
                {
                }
            }

            stopwatch.Stop();

            Logger.LogPerformance(string.Format(Messages.TimeToDisplay, stopwatch.Elapsed, _container.GetDescription(), by));

            return elements;
        }
    }
}