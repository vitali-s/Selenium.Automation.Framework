using OpenQA.Selenium;

namespace Selenium.Automation.Framework.Example.Views
{
    public class RepositoryItemView : View
    {
        public RepositoryItemView(Element element)
            : base(element)
        {
        }

        public Element User
        {
            get { return FindElement(By.TagName("a")); }
        }

        public Element Name
        {
            get { return FindElement(By.TagName("em")); }
        }
    }
}
