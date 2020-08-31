using System;
using System.Collections.Generic;
using System.IO;
using filenameParser;
using filenameParser.Modules;
using autoRename.Modules;
using autoBuilder.Modules;


namespace autoBuilder
{
    class Program
    {
        public static void Main()
        {
            var path = InputHandler.LineInput("Path: ").Replace("\"", "");
            while (!Directory.Exists(path))
            {
                Console.WriteLine($"{path}: no such directory!");
                path = InputHandler.LineInput("Path: ").Replace("\"", "");
            }
            var extension = "." + InputHandler.LineInput(
                "Extension(default is wav): ", new string[] { "wav", "mp3", "ogg", "flac" }, "wav");

            var map = new SampleMap();
            int articulationsCount = Convert.ToInt32(InputHandler.LineInput(
                "Number of articulations(default is 1): ", "1"));
            List<string> articulations = new List<string>();
            for (int i = 1; i <= articulationsCount; i++)
            {
                articulations.Add(InputHandler.LineInput(
                    $"articulation{i} name: ", $"articulation{i}"));
            }

            int dynLevelsCount = Convert.ToInt32(InputHandler.LineInput(
                "Number of dynamic layers(default is 1): ", "1"));
            var dynLayers = new List<string>();
            for (int i = 1; i <= dynLevelsCount; i++)
            {
                dynLayers.Add(InputHandler.LineInput(
                    $"level{i} name: ", $"level{i}"));
            }

            int rrCount = Convert.ToInt32(InputHandler.LineInput(
                "Number of round robins(default is 1): ", "1"));
            var roundRobins = new List<string>();
            for (int i = 1; i <= rrCount; i++)
            {
                roundRobins.Add(InputHandler.LineInput(
                    $"RR{i} name: ", $"RR{i}"));
            }

            foreach (var articulation in articulations)
            {
                foreach(var dynLayer in dynLayers)
                {
                    foreach (var rr in roundRobins)
                        map.Groups.Add(new SampleGroup(articulation, dynLayer, rr));
                }
            }
            var files = new HierarchyHandler(StructureHandler.ByExtension(path, extension));
            
        }
    }
}
