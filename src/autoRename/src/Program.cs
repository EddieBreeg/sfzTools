using System;
using System.IO;
using System.Linq;
using filenameParser;
using autoRename.Modules;

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
                var midiValue = map.MidiNotes.ToList().IndexOf(firstNote);
                foreach (var file in dir)
                {
                    var suffix = Path.GetRelativePath(rootPath, Path.GetDirectoryName(file)).Replace(Path.DirectorySeparatorChar, '_');
                    File.Move(file, Path.Combine(Path.GetDirectoryName(file), $"{map.MidiNotes[midiValue]}_{suffix}{ext}"));
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
}
