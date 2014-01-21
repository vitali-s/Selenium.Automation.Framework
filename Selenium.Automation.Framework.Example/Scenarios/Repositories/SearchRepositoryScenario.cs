using System.Linq;
using FluentAssertions;
using Selenium.Automation.Framework.Example.Models;
using Selenium.Automation.Framework.Example.Views;

namespace Selenium.Automation.Framework.Example.Scenarios.Repositories
{
    public class SearchRepositoryScenario : Scenario
    {
        public RepositoryItemView Search(SearchRepositoryModel searchRepositoryModel)
        {
            var homeView = View<DefaultNavigationBarView>();

            homeView.Search.Type(searchRepositoryModel.Name);

            homeView.Search.PressEnter();

            var searchResults = Resolve<SearchResultsView>();

            RepositoryItemView view = searchResults.Repositories
                .Where(repository => repository.Name != null)
                .FirstOrDefault(repository => repository.Name.Text == searchRepositoryModel.Name);

            view
                .Should()
                .NotBeNull();

            return view;
        }
    }
}
