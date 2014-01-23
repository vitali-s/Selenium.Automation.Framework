using System.Collections.Generic;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.Automation.Framework.Constants;

namespace Selenium.Automation.Framework
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

        public bool Displayed
        {
            get { return _element.Displayed; }
        }

        public string Value
        {
            get { return _element.GetAttribute("value"); }
        }

        public string CssClass
        {
            get { return _element.GetAttribute("class"); }
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

        public void Click(bool useJavaScript = true)
        {
            if (useJavaScript && _element.TagName != TagNames.Link)
            {
                _browser.ExecuteJavaScript(string.Format("$(arguments[0]).{0}();", JavaScriptEvents.Click), _element);
            }
            else
            {
                // Possible: _element.Click(); or _element.SendKeys(Keys.Enter);
                new Actions(_browser.WebDriver).MoveToElement(_element).MoveByOffset(5, 5).Click().Build().Perform();
            }
        }

        public void Focus()
        {
            if (_element.TagName == TagNames.Input)
            {
                _element.SendKeys(string.Empty);
            } 
            else
            {
               new Actions(_browser.WebDriver).MoveToElement(_element).Perform();
            }
        }

        public string GetDescription()
        {
            return string.Format("<{0} class=\"{1}\" />", _element.TagName, CssClass);
        }
    }
}
