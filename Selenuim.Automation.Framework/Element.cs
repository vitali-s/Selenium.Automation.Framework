using System.Collections.Generic;
using FluentAssertions;
using OpenQA.Selenium;
using Selenuim.Automation.Framework.Constants;

namespace Selenuim.Automation.Framework
{
    public class Element : IElementsContainer
    {
        private readonly Browser _browser;
        private readonly IWebElement _element;

        public Element(Browser browser, IWebElement element)
        {
            _element = element;
            _browser = browser;
        }

        public string Value
        {
            get { return _element.GetAttribute("value"); }
        }

        public string Text
        {
            get { return _element.Text; }
        }

        public void Type(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Clear();

            _element.SendKeys(text);

            Value.Should().BeEquivalentTo(text, "should be similar to entered text");
        }

        public void Clear()
        {
            if (_element.TagName == TagNames.Input || _element.TagName == TagNames.TextArea)
            {
                _element.Clear();
            }
            else
            {
                _element.SendKeys(Keys.LeftControl + "a");
                _element.SendKeys(Keys.Delete);
            }
        }

        public void PressEnter()
        {
            _element.SendKeys(Keys.Return);
        }

        // Do not throws exceptions, only return null
        public IWebElement FindElement(By by)
        {
            try
            {
                return _element.FindElement(by);
            }
            catch
            {
                return null;
            }
        }

        // Do not throws exceptions, only return null
        public ICollection<IWebElement> FindElements(By by)
        {
            try
            {
                return _element.FindElements(by);
            }
            catch
            {
                return null;
            }
        }
    }
}
