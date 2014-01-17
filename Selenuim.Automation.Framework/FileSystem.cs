using System.IO;

namespace Selenuim.Automation.Framework
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
                item.Attributes = ResetAttributes(item.Attributes);
            }

            foreach (var item in baseFolder.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                item.Attributes = ResetAttributes(item.Attributes);
            }

            baseFolder.Delete(true);
        }

        private FileAttributes ResetAttributes(FileAttributes attributes)
        {
            return attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
        }
    }
}
