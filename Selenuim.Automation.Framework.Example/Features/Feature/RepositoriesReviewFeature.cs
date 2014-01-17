using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Selenuim.Automation.Framework.Example.Models;
using Selenuim.Automation.Framework.Example.Scenarios.Repositories;

namespace Selenuim.Automation.Framework.Example.Features.Feature
{
    /// <summary>
    /// Support both Nunit and MSTest tests
    /// </summary>
    [TestFixture]
    [TestClass]
    public class RepositoriesReviewFeature : BaseAutomationTest
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
