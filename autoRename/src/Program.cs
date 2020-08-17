using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using filenameParser.SampleMap;
using filenameParser.Extensions;

namespace autoRename
{
    class StructureHandler
    {
        public static string[][] SortByDirs(string rootPath, string extension="")
        {
            var result = new List<string[]>();
            switch (extension)
            {
                case "":
                    Console.WriteLine("weird shit");
                    result.Add(Directory.GetFiles(rootPath));
                    break;
                default:
                    var files = new List<string>();
                    foreach (string file in Directory.GetFiles(rootPath))
                    {
                        Console.WriteLine(Path.GetExtension(file));
                        if (Path.GetExtension(file) == "." + extension)
                            files.Add(file);
                    }
                    if (files.Count > 0)
                        result.Add(files.ToArray());
                    break;
            }
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

            var map = new Map();
            var firstNote = fileNameParser.Program.LineInput("First note (default is C0): ", map.MidiNotes, "C0");
            Console.Write("Interval between the notes in semitones (default is 5): ");
            dynamic interval = Console.ReadLine();
            interval = (interval != "") ? Convert.ToInt32(interval) : 5;

            string[] extensions = { "wav", "mp3", "flac", "ogg" };
            var ext = fileNameParser.Program.LineInput("File extension (default is wav): ", extensions, "wav");
            Console.WriteLine(ext);

            var files = StructureHandler.SortByDirs(rootPath, ext);
#if DEBUG
            //foreach(var dir in files)
            //{
            //    Console.WriteLine(dir.Length);
            //    Console.WriteLine(string.Join('\n', dir));
            //}
#endif
        }
    }
}
