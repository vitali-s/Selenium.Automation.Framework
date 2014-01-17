using Selenuim.Automation.Framework.Factories;

namespace Selenuim.Automation.Framework
{
    public abstract class InfrastructureObject
    {
        protected Browser Browser
        {
            get { return Resolve<Browser>(); }
        }

        protected Configuration Configuration
        {
            get { return Resolve<Configuration>(); }
        }

        protected TModel Model<TModel>(string caseName = null)
        {
            return Resolve<ModelBuilder>().ReadModel<TModel>(caseName);
        }

        protected TDependency Resolve<TDependency>(params object[] parameters)
        {
            return Factory.Container.Resolve<TDependency>(parameters);
        }
    }
}
