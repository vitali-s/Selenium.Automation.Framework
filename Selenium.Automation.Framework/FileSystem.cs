using System;
using System.IO;
using Selenium.Automation.Framework.Logging;
using Selenium.Automation.Framework.Resources;

namespace Selenium.Automation.Framework
{
    public class FileSystem
    {
        public void ForceDelete(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var baseFolder = new DirectoryInfo(path);

            foreach (var item in baseFolder.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                ResetAttributes(item);
            }

            foreach (var item in baseFolder.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                ResetAttributes(item);
            }

            try
            {
                baseFolder.Delete(true);
            }
            catch (Exception exception)
            {
                Logger.LogInfrastructureError(string.Format(Messages.ErrorDirectoryDeletion, baseFolder.FullName), exception);
            }
        }

        protected void ResetAttributes(FileSystemInfo fileInfo)
        {
            try
            {
                fileInfo.Attributes = fileInfo.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            }
            catch (Exception exception)
            {
                Logger.LogInfrastructureError(string.Format(Messages.ErrorAttributesReset, fileInfo.FullName), exception);
            }
        }
    }
}
