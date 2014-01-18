namespace Selenium.Automation.Framework.Factories
{
    public static class Factory
    {
        private static readonly object LockObject = new object();
        private static Container _innerContainer;

        static Factory()
        {
            lock (LockObject)
            {
                _innerContainer = new Container();
            }
        }

        public static Container Container
        {
            get { return _innerContainer; }
        }

        public static void Clear()
        {
            lock (LockObject)
            {
                _innerContainer = new Container();
            }
        }
    }
}
