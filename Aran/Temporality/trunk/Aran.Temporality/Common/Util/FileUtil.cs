using System.IO;

namespace Aran.Temporality.Common.Util
{
    public class FileUtil
    {
        public static void DeleteDirectory(string targetDir)
        {
            if (!Directory.Exists(targetDir)) return;
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }
    }
}