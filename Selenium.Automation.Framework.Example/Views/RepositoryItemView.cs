using OpenQA.Selenium;

namespace Selenium.Automation.Framework.Example.Views
{
    public class RepositoryItemView : View
    {
        public RepositoryItemView(Element element)
            : base(element)
        {
        }

        public Element FullName
        {
            get { return FindElement(By.TagName("a")); }
        }

        public Element Name
        {
            get { return FindElement(By.TagName("em")); }
        }

        protected override By BaseBy
        {
            get { return By.TagName("a"); }
        }
    }
}
