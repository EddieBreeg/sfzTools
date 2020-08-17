using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Extensions;

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
            Console.Write("Path: ");
            var path = Console.ReadLine();
            while (Directory.Exists(path))
            {
                Console.WriteLine($"");
            }
        }

        private static char KeyInput(string str, string choices, char defaultValue)
        {
            Console.Write(str);
            var value = Console.ReadKey().KeyChar;
            while (!choices.Contains(value) && value != '\r')
            {
                Console.WriteLine("Invalid choice! Please try again.");
                Console.Write(str);
                value = Console.ReadKey().KeyChar;
                //Console.WriteLine(value);
            }
            if (value == '\r')
                return defaultValue;
            return value;
        }

        private static string LineInput(string str, string[] choices, string defaultValue)
        {
            Console.Write(str);
            var value = Console.ReadLine();
            while (!choices.Contains(value) && value != "")
            {
                Console.WriteLine("Invalid choice! Please try again.");
                Console.Write(str);
                value = Console.ReadLine();
            }
            if (value == "")
                return defaultValue;
            return value;
        }
    }
}
