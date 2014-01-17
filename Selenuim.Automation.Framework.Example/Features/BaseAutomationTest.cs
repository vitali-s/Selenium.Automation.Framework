using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Selenuim.Automation.Framework.Example.Features
{
    public abstract class BaseAutomationTest : AutomationTest
    {
        public TestContext TestContext { get; set; }

        [SetUp]
        [TestInitialize]
        public void TestInitialize()
        {
            StartTest();
        }

        [TearDown]
        public void TearDown()
        {
            EndTest(() => NUnit.Framework.TestContext.CurrentContext.Result.Status == TestStatus.Failed);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            EndTest(() => TestContext.CurrentTestOutcome.Equals(UnitTestOutcome.Failed));
        }
    }
}
