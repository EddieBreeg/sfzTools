using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using filenameParser;

namespace autoRename
{
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
            var firstNote = InputHandler.LineInput("First note (default is C0): ", map.MidiNotes, "C0");
            Console.Write("Interval between the notes in semitones (default is 5): ");
            dynamic interval = Console.ReadLine();
            interval = interval != string.Empty ? Convert.ToInt32(interval) : 5;

            var ext = InputHandler.LineInput("File extension (default is wav): ", new []{ "wav", "mp3", "flac", "ogg" }, "wav");

            var files = StructureHandler.SortByDirs(rootPath);
        }
    }

    public class StructureHandler
    {
        public static string[][] SortByDirs(string rootPath)
        {
            var result = new List<string[]> {Directory.GetFiles(rootPath)};
            Directory.GetDirectories(rootPath).ToList().ForEach(d => result.AddRange(SortByDirs(d)));

            return result.ToArray();
        }
    }
}
