﻿using System;
using System.IO;
using System.Linq;
<<<<<<< HEAD
using Parser;
using filenameParser.SampleMap;
using System.Collections.Generic;

namespace fileNameParser
=======
using System.Collections.Generic;

namespace filenameParser
>>>>>>> 4e4e0a43b961231b4760d9c949878cf7644f2556
{
    public class Program
    {
        static void Main()
        {
            Console.Write("Path: ");
            var rootPath = Console.ReadLine()?.Replace("\"", "");
            while (!Directory.Exists(rootPath))
            {
                Console.WriteLine($"{rootPath}: No such directory, please enter a valid path!");
                Console.Write("Path: ");
                rootPath = Console.ReadLine()?.Replace("\"", "");
            }
            
            var ext = InputHandler.LineInput("File extension(default is wav): ", new []{ "wav", "mp3", "flac", "ogg" }, "wav");
            var files = new List<string>();
            foreach (var file in Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file) == "."+ext)
                    files.Add(file);
            }
            var parser = new FileNameParser();

            Console.Write("Separator(default is '_'): ");
            var separator = (char)Console.Read();
            if (separator != 13)
                parser.Separator = separator;

            var map = new Map();

            var firstFile = parser.SplitName(files[0]);
            var lut = new char[firstFile.Length];
            for (var i = 0; i < lut.Length; i++)
                lut[i] = map.MidiNotes.Contains(firstFile[i]) ? '1': '2';

            if (firstFile.Length < 2)
            {
                Console.WriteLine("Fatal error: No valid token found in filenames");
                Console.ReadKey();
                Environment.Exit(1);
            }
            Console.WriteLine("The parser found the following tokens, please specify how the program should treat each one:");
            Console.WriteLine("0 = ignore");
            Console.WriteLine("1 = note name");
            Console.WriteLine("2 = group name\n");

            for(int i = 0; i < lut.Length; i++)
            {
                lut[i] = InputHandler.KeyInput($"{firstFile[i]} (default is {lut[i]}): ", new []{'0', '1', '2'}, lut[i]);
                Console.Write("\n");
            }
#if DEBUG
            Console.WriteLine(string.Join(", ", lut));
#endif
            if(!lut.Contains('1'))
            {
                Console.WriteLine("Error: no note information in the filenames");
                Console.ReadKey();
                Environment.Exit(1);
            }
            
            foreach(var file in files)
            {
                var rootNote = "";
                var groupName = new List<string>();
                var data = parser.SplitName(file);

                for (int i = 0; i < data.Length; i++)
                {
                    switch (lut[i])
                    {
                        case '1':
                            rootNote = data[i];
                            break;
                        case '0':
                            break;
                        default:
                            groupName.Add(data[i]);
                            break;
                    }
                }
                var region = new Region(file, rootNote);
                map.AddRegion(region, groupName.Count != 0 ? string.Join(' ', groupName) : "default group");
            }
            map.SortRegions();

#if (DEBUG)
            //Console.WriteLine(string.Join(", ", map.Articulations));
            //Console.WriteLine(string.Join(", ", map.DynamicLevels));
            //Console.WriteLine(string.Join(", ", map.RoundRobins));
            Console.WriteLine(string.Join(", ", map.Groups));
            Console.WriteLine(string.Join('\n', map.Groups[0].Regions));
#endif
            Console.WriteLine("Possible stretch modes: ");
            Console.WriteLine("0 = no stretch");
            Console.WriteLine("1 = stretch down");
            Console.WriteLine("2 = stretch up\n");

            var stretchMode = InputHandler.KeyInput("Please specify the stretch mode (default is 1): ", new []{'0', '1', '2'}, '1');
#if (DEBUG)
            Console.WriteLine(stretchMode);
#endif
            map.StretchRegions(stretchMode);
            foreach (var file in files)
                File.Move(file, Path.Combine(rootPath, Path.GetFileName(file)));

            var outputPath = Path.Combine(Path.GetDirectoryName(rootPath), "map.sfz");
            File.WriteAllText(outputPath, map.Render(rootPath));
            Console.WriteLine($"{outputPath} successfully created!");
            Console.ReadKey();
        }
<<<<<<< HEAD

        public static char KeyInput(string str, string choices, char defaultValue)
        {
            Console.Write(str);
            var value = Console.ReadKey().KeyChar;
            while(!choices.Contains(value) && value!='\r')
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

        public static string LineInput(string str, string[] choices, string defaultValue)
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
=======
>>>>>>> 4e4e0a43b961231b4760d9c949878cf7644f2556
    }
}
