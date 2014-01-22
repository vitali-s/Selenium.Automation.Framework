SET msbuild=%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe Selenium.Automation.Framework.sln /p:Platform="Any CPU" /verbosity:quiet
SET mstest="c:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\VSTest.Console.exe" Selenium.Automation.Framework.Example\bin\
SET assembly="Selenium.Automation.Framework.Example.dll"

%msbuild% /p:Configuration=Firefox
%mstest%Firefox\\%assembly%

%msbuild% /p:Configuration=Chrome
%mstest%Chrome\\%assembly%

%msbuild% /p:Configuration=InternetExplorer
%mstest%InternetExplorer\\%assembly%

pause