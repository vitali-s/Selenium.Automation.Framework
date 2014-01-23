Selenium Automation Framework
=============================

Frameworks provides base infrastrcutre for your acceptances tests. That will help to reduce start time and allows to separation of concerns from the very beggining.

The framework are available via nuget package: TODO

The default structure of the project:

  - Features
  - Models
  - Scenarios
  - TestData
  - Views

QuickStarts:
  - Load nuget package
  - Create base test class, for example:

 ```C#
    [TestFixture]
    public abstract class BaseAutomationTest : AutomationTest
    {
        public TestContext TestContext { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            StartTest();
        }

        [TearDown]
        public void TearDown()
        {
            EndTest(() => NUnit.Framework.TestContext.CurrentContext.Result.Status == TestStatus.Failed);
        }
    }
 ```
 
   - Create View: 
 ```C#
    public class RepositoryView : View
    {
        public Element Description
        {
            get { return FindElement(By.ClassName("repository-description")); }
        }

        protected override By BaseBy
        {
            get { return By.ClassName("repohead"); }
        }
    }
```  

   - Create Scenario:
 ```C#
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
```  

   - Create Feature:
 ```C#
    [TestFixture]
    public class RepositoriesReviewFeatureTest : BaseAutomationTest
    {
        [Test]
        public void Search_For_Automation_Framework_Repository()
        {
            Scenario<SearchRepositoryScenario>()
                .Search(Model<SearchRepositoryModel>());
        }
    }
```