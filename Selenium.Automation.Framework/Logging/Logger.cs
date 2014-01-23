using System;
using System.IO;

namespace Selenium.Automation.Framework.Logging
{
    public static class Logger
    {
        public static void LogInfrastructureError(string message, Exception exception)
        {
            Log("log-infrastructure.txt", DateTime.Now + " - " + message + Environment.NewLine + exception + Environment.NewLine + Environment.NewLine);
        }

        public static void LogPerformance(string message)
        {
            Log("log-perfromance.txt", DateTime.Now + " - " + message + Environment.NewLine);
        }

        private static void Log(string fileName, string message)
        {
            var currentDirectory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, new Configuration().TestResultPath));

            File.AppendAllText(Path.Combine(currentDirectory.FullName, fileName), message);
        }
    }
}
