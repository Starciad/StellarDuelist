using System;
using System.IO;

namespace StardustDefender.Core.IO
{
    public static class SFile
    {
        public static string WriteException(Exception exception)
        {
            string dateTimeString = DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
            string directory = SDirectory.GetDirectoryPath("Logs");
            string filename = $"StardustDefender_Log_{dateTimeString}.txt";
            string fullname = Path.Combine(directory, filename);

            File.WriteAllText(fullname, exception.ToString());
            return fullname;
        }
    }
}
