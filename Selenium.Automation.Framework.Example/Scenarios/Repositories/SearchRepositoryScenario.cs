using System.Linq;
using FluentAssertions;
using Selenium.Automation.Framework.Example.Models;
using Selenium.Automation.Framework.Example.Views;

namespace Selenium.Automation.Framework.Example.Scenarios.Repositories
{
    public class SearchRepositoryScenario : Scenario
    {
        public void Search(SearchRepositoryModel searchRepositoryModel)
        {
            var homeView = Resolve<DefaultNavigationBarView>();

            homeView.Search.Type(searchRepositoryModel.Name);

            homeView.Search.PressEnter();

            var searchResults = Resolve<SearchResultsView>();

            searchResults.Repositories
                .Where(repository => repository.Name != null)
                .Should()
                .ContainSingle(repository => repository.Name.Text == searchRepositoryModel.Name);
        }
    }
}
