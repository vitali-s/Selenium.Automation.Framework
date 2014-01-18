using System.Reflection;

namespace Selenium.Automation.Framework.Factories
{
    public class Constructor
    {
        private readonly ConstructorInfo _constructorInfo;
        private readonly ParameterInfo[] _parameters;

        public Constructor(ConstructorInfo constructorInfo)
        {
            _constructorInfo = constructorInfo;
            _parameters = _constructorInfo.GetParameters();
        }

        public ConstructorInfo ConstructorInfo
        {
            get { return _constructorInfo; }
        }

        public ParameterInfo[] Parameters
        {
            get { return _parameters; }
        }
    }
}
