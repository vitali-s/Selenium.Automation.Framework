using System;

namespace Selenium.Automation.Framework.Factories
{
    public class Registration
    {
        private readonly Type _type;
        private readonly LifeTypes _lifeType;

        public Registration(Type type, LifeTypes lifeType)
        {
            _type = type;
            _lifeType = lifeType;
        }

        public Type Type
        {
            get { return _type; }
        }

        public LifeTypes LifeType
        {
            get { return _lifeType; }
        }
    }
}
