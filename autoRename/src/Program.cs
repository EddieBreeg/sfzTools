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

            var ext = "." + InputHandler.LineInput("File extension (default is wav): ", new []{ "wav", "mp3", "flac", "ogg" }, "wav");

            var files = StructureHandler.SortByDirs(rootPath, ext);
            foreach(var dir in files)
            {
                int midiValue = map.MidiNotes.ToList().IndexOf(firstNote);
                foreach (var file in dir)
                {
                    string prefix = Path.GetRelativePath(rootPath, Path.GetDirectoryName(file)).Replace(Path.DirectorySeparatorChar, '_');
                    File.Move(file, Path.Combine(Path.GetDirectoryName(file), $"{prefix}_{map.MidiNotes[midiValue]}{ext}"));
                    midiValue += interval;
                }
            }


#if (DEBUG)
            foreach (var dir in files)
            {
                Console.WriteLine(string.Join('\n', dir));
                Console.WriteLine('\n');
            }
#endif
        }
    }

    internal class StructureHandler
    {
        public static string[][] SortByDirs(string rootPath, string extension = null)
        {
            var result = new List<string[]> {extension != null? ByExtension(rootPath, extension): Directory.GetFiles(rootPath)};
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
