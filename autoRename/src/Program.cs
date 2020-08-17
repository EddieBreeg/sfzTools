using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fileNameParser.SampleMap;

namespace autoRename
{
    class StructureHandler
    {
        public static string[][] SortByDirs(string rootPath)
        {
            var result = new List<string[]> {Directory.GetFiles(rootPath)};
            Directory.GetDirectories(rootPath).ToList().ForEach(d => result.AddRange(SortByDirs(d)));

            return result.ToArray();
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Path: ");
            var rootPath = Console.ReadLine()?.Replace("\"", "");
            while (!Directory.Exists(rootPath))
            {
                Console.WriteLine($"{rootPath}: No such directory, please enter a valid path!");
                Console.Write("Path: ");
                rootPath = Console.ReadLine()?.Replace("\"", "");
            }

            var map = new SampleMap();
            var firstNote = LineInput("First note (default is C0): ", map.MidiNotes, "C0");
            Console.Write("Interval between the notes in semitones (default is 5): ");
            dynamic interval = Console.ReadLine();
            interval = interval != string.Empty ? Convert.ToInt32(interval) : 5;

            string[] extensions = { "wav", "mp3", "flac", "ogg" };
            var ext = LineInput("File extension (default is wav): ", extensions, "wav");

            var files = StructureHandler.SortByDirs(rootPath);
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
            while (!choices.Contains(value) && value != string.Empty)
            {
                Console.WriteLine("Invalid choice! Please try again.");
                Console.Write(str);
                value = Console.ReadLine();
            }
            if (value == string.Empty)
                return defaultValue;
            return value;
        }
    }
}
