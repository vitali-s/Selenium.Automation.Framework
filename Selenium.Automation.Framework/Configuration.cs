using System;
using System.Configuration;

namespace Selenium.Automation.Framework
{
    public class Configuration
    {
        public Browsers Browser
        {
            get { return (Browsers)Enum.Parse(typeof(Browsers), ConfigurationManager.AppSettings["browser"], true); }
        }

        public string Location
        {
            get { return ConfigurationManager.AppSettings["location"]; }
        }

        public string TestResultPath
        {
            get { return ConfigurationManager.AppSettings["test-result-path"]; }
        }

        public string TestDataPath
        {
            get { return ConfigurationManager.AppSettings["test-data-path"]; }
        }
    }
}
