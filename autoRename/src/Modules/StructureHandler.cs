using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace autoRename
{
    public class StructureHandler
    {
        public static string[][] SortByDirs(string rootPath, string extension = null)
        {
            var result = new List<string[]> { extension != null ? ByExtension(rootPath, extension) : Directory.GetFiles(rootPath) };
            Directory.GetDirectories(rootPath).ToList().ForEach(d => result.AddRange(SortByDirs(d, extension)));

            return result.ToArray();
        }

        public static string[] ByExtension(string path, string extension)
        {
            var files = new List<string>();
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file).ToLower() == extension.ToLower())
                    files.Add(file);
            }
            return files.ToArray();
        }
    }
}