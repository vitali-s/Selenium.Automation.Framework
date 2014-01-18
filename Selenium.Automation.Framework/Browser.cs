using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Selenium.Automation.Framework
{
    public class Browser : IElementsContainer
    {
        private readonly Configuration _configuration;
        private readonly FileSystem _fileSystem;

        private IWebDriver _webDriver;
        private string _mainWindowHandler;

        public Browser(Configuration configuration, FileSystem fileSystem)
        {
            _configuration = configuration;
            _fileSystem = fileSystem;
        }

        public IWebDriver WebDriver
        {
            get
            {
                if (_webDriver == null)
                {
                    Start();
                }

                return _webDriver;
            }
        }

        // Do not throws exceptions, only return null
        public IWebElement FindElement(By by)
        {
            try
            {
                return WebDriver.FindElement(by);
            }
            catch
            {
                return null;
            }
        }

        // Do not throws exceptions, only return null
        public ICollection<IWebElement> FindElements(By by)
        {
            try
            {
                return WebDriver.FindElements(by);
            }
            catch
            {
                return null;
            }
        }

        public void Quit()
        {
            if (_webDriver == null)
            {
                return;
            }

            _webDriver.Quit();
            _webDriver = null;
        }

        public void Open()
        {
            WebDriver.Navigate().GoToUrl(_configuration.Location);
        }

        public void Open(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public void SaveScreenshot(string path)
        {
            ((ITakesScreenshot)WebDriver).GetScreenshot().SaveAsFile(path, ImageFormat.Jpeg);
        }

        public void WaitForReady()
        {
            var ready = new Func<bool>(() => (bool)ExecuteJavaScript("return document.readyState == 'complete'"));

            Waiter.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)).Should().BeTrue("because of document is not ready.");
        }

        public object ExecuteJavaScript(string javaScript, params object[] args)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)WebDriver;

            return javaScriptExecutor.ExecuteScript(javaScript, args);
        }

        private void Start()
        {
            switch (_configuration.Browser)
            {
                case Browsers.InternetExplorer:
                    _webDriver = StartInternetExplorer();
                    break;
                case Browsers.Firefox:
                    _webDriver = StartFirefox();
                    break;
                case Browsers.Chrome:
                    _webDriver = StartChrome();
                    break;
                default:
                    throw new Exception(string.Format("Unknown browser selected: {0}.", _configuration.Browser));
            }

            _webDriver.Manage().Window.Maximize();
            _webDriver.Manage().Cookies.DeleteAllCookies();
            _mainWindowHandler = _webDriver.CurrentWindowHandle;
        }

        private InternetExplorerDriver StartInternetExplorer()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                InitialBrowserUrl = "about:blank",
                EnableNativeEvents = true
            };

            return new InternetExplorerDriver(Directory.GetCurrentDirectory(), internetExplorerOptions);
        }

        private FirefoxDriver StartFirefox()
        {
            var firefoxProfile = new FirefoxProfile
            {
                AcceptUntrustedCertificates = true,
                EnableNativeEvents = true
            };

            return new FirefoxDriver(firefoxProfile);
        }

        private ChromeDriver StartChrome()
        {
            var chromeOptions = new ChromeOptions();
            var defaultDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\Local\Google\Chrome\User Data\Default";

            if (Directory.Exists(defaultDataFolder))
            {
                Waiter.Try(() => _fileSystem.ForceDelete(defaultDataFolder));
            }

            return new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);
        }
    }
}
