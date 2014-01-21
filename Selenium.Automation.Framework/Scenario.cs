namespace Selenium.Automation.Framework
{
    public abstract class Scenario : InfrastructureObject
    {
        protected TView View<TView>()
        {
            return Resolve<TView>();
        }
    }
}
