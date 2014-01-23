using System;
using System.Configuration;

namespace Selenium.Automation.Framework
{
    public class Configuration
    {
        public Browsers Browser
        {
            get
            {
                string browser = ConfigurationManager.AppSettings["browser"];

                if (string.IsNullOrEmpty(browser))
                {
                    return Browsers.Firefox;
                }

                return (Browsers)Enum.Parse(typeof(Browsers), browser, true);
            }
        }

        public string TestResultPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings["test-result-path"];

                if (string.IsNullOrEmpty(path))
                {
                    return @"TestResults\";
                }

                return path;
            }
        }

        public string TestDataPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings["test-data-path"];

                if (string.IsNullOrEmpty(path))
                {
                    return @"TestData\";
                }

                return path;
            }
        }

        /// <summary>
        /// Wait timeout in seconds
        /// </summary>
        public int WaitTimeout
        {
            get
            {
                string timeout = ConfigurationManager.AppSettings["wait-timeout"];

                if (string.IsNullOrEmpty(timeout))
                {
                    return 20;
                }

                return int.Parse(timeout);
            }
        }
    }
}
