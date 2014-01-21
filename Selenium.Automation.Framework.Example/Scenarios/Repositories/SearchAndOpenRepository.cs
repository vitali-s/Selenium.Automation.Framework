using FluentAssertions;
using Selenium.Automation.Framework.Example.Models;
using Selenium.Automation.Framework.Example.Views;

namespace Selenium.Automation.Framework.Example.Scenarios.Repositories
{
    public class SearchAndOpenRepository : SearchRepositoryScenario
    {
        public void Open(SearchRepositoryModel searchRepositoryModel)
        {
            RepositoryItemView view = Search(searchRepositoryModel);

            view.FullName.Click();

            View<RepositoryView>().Description.Text.Should().Be(searchRepositoryModel.Name);
        }
    }
}
