using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Selenium.Automation.Framework.Example.Views
{
    public class SearchResultsView : View
    {
        public Element SearchInput
        {
            get { return FindElement(By.ClassName("search-page-input")); }
        }

        public ICollection<RepositoryItemView> Repositories
        {
            get
            {
                return FindElements(By.ClassName("repolist-name"))
                    .Where(element => element != null)
                    .Select(element => new RepositoryItemView(element))
                    .ToList();
            }
        }
    }
}
