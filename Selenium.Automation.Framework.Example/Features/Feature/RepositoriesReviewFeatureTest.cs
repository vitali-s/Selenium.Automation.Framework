using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Selenium.Automation.Framework.Example.Models;
using Selenium.Automation.Framework.Example.Scenarios.Repositories;

namespace Selenium.Automation.Framework.Example.Features.Feature
{
    /// <summary>
    /// Support both Nunit and MSTest tests
    /// </summary>
    [TestFixture]
    [TestClass]
    public class RepositoriesReviewFeatureTest : BaseAutomationTest
    {
        [Test]
        [TestMethod]
        public void Search_For_Repository()
        {
            Scenario<SearchRepositoryScenario>()
                .Search(Model<SearchRepositoryModel>());
        }

        [Test]
        [TestMethod]
        public void Search_For_Another_Repository()
        {
            Scenario<SearchRepositoryScenario>()
                .Search(Model<SearchRepositoryModel>("case-1"));
        }
    }
}
