using System;
using System.Collections.Generic;
using System.IO;

namespace autoRename
{
    class StructureHandler
    {
        public static string[] ListRelatives(string path)
        {
            var tree = new List<string>();
            foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                tree.Add(Path.GetRelativePath(path, file));
            return tree.ToArray();
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Please specify:");
                Console.WriteLine("- A root path");
                Console.WriteLine("- The name of the first note");
                Console.WriteLine("- The interval between each sample (in semitones)");
                return;
            }

            string rootPath = args[0];
            int interval = Convert.ToInt32(args[1]);
            string firstNote = args[2];
            string extension = (args.Length > 3) ? args[2] : "wav";
            Console.WriteLine(string.Join('\n', StructureHandler.ListRelatives(rootPath)));
        }
    }
}
