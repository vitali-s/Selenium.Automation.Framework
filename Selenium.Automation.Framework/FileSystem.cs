using System.IO;

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
            catch
            {
                // TODO: Add infrastructure error logging
            }
        }

        protected void ResetAttributes(FileSystemInfo fileInfo)
        {
            try
            {
                fileInfo.Attributes = fileInfo.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            }
            catch
            {
                // TODO: Add infrastructure error logging
            }
        }
    }
}
