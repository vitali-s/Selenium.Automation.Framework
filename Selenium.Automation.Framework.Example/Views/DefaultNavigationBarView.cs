using OpenQA.Selenium;

namespace Selenium.Automation.Framework.Example.Views
{
    public class DefaultNavigationBarView : View
    {
        public Element Search
        {
            get { return FindElement(By.Id("js-command-bar-field")); }
        }
    }
}
