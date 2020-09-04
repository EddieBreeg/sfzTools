using System;
using System.Collections.Generic;
using System.IO;
using filenameParser;
using autoRename.Modules;
using autoBuilder.Modules;
using System.Linq;

namespace autoBuilder
{
    public class Program
    {
        public static int[] SplitRange(int min, int max, int divider)
        {
            var result = new int[divider * 2];
            result[0] = min;
            for(int i =0; i<divider; i++)
                result[2 * i + 1] = min + (max - min) / divider * (i+1);
            for (int i = 2; i < divider * 2 - 1; i += 2)
                result[i] = result[i - 1] + 1;
            result[^1] = max;
            return result;
        }
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
            var filesList = StructureHandler.ByExtension(path, extension);
            filesList.Sort(new NumericalStringComparer());

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
            var totalGroupCount = rrCount * articulationsCount * dynLevelsCount;
            int noteCountPerGroup = InputHandler.IntInput("Number of notes per group: ");

            int firstNote = map.MidiNotes.ToList().IndexOf(
                InputHandler.LineInput("First note in each group (default is C0): ", map.MidiNotes, "C0"));

            int interval = InputHandler.IntInput("Interval between notes (in semitones, default is 5): ", 5);
            char stretchMode = 
                InputHandler.KeyInput("Stretch mode (default is 1): ", new char[] { '0', '1', '2' }, '1');

            Console.WriteLine("\nFinally I need to know what hierarchy you have used to sort the samples: ");
            Console.WriteLine("0 = articulation\n" +
                                "1 = dynamic level\n" +
                                "2 = round robin\n" +
                                "3 = note name");
            Console.WriteLine("Please read the documentation for full details! ");
            var lut = InputHandler.IntListInput("Hierarchy (default is \"0 1 2 3\"): ",
                new List<int>() { 0, 1, 2, 3 }, 
                l => l.IsPermutation() && l.Count == 4);

            for(int i=0; i<articulationsCount; i++)
                for(int j=0; j<dynLevelsCount;j++)
                    for(int k=0; k<rrCount;k++)
                    {
                        int loVel = SplitRange(0, 127, dynLevelsCount)[2 * j];
                        int hiVel = SplitRange(0, 127, dynLevelsCount)[2 * j + 1];
                        int sequencePos = k+1;
                        SampleGroup group = new SampleGroup(articulations[i], dynLayers[j], roundRobins[k]);
                        group.SequencePosition = sequencePos;
                        group.LoVel = loVel;
                        group.HiVel = hiVel;
                        map.Groups.Add(group);
                    }
            var files = new HierarchyHandler(filesList.GetRange(0, totalGroupCount * noteCountPerGroup));
            var groupCounts = new List<int>() { articulationsCount, dynLevelsCount, rrCount, noteCountPerGroup };
            files.GroupsPerLevel =groupCounts.Permute(lut);
            files.LabelFiles();
            foreach(LabeledFile file in files)
            {
                file.HierachyLabels.RemoveAt(4);
                file.HierachyLabels = file.HierachyLabels.Permute( lut.FlipPermutation());
            }
            files.Sort();
            foreach(LabeledFile file in files)
            {
                string groupName = articulations[file.HierachyLabels[0]] + " " +
                    dynLayers[file.HierachyLabels[1]] + " " +
                    roundRobins[file.HierachyLabels[2]];
                string rootNote = map.MidiNotes[firstNote + interval * file.HierachyLabels[3]];
                string newName = $"{groupName.Replace(' ', '_')}_{rootNote}{extension}";
                map.AddRegion(new Region(newName, rootNote), groupName);
                File.Move(file.Path,
                    Path.Join(path, newName));
            }
            var remainingFiles = filesList.GetRange(
                totalGroupCount * noteCountPerGroup,
                filesList.Count % (totalGroupCount * noteCountPerGroup));
            if(remainingFiles.Count!=0)
            {
                var remainingGroup = new SampleGroup("remaining");
                for (int i = 0; i < remainingFiles.Count; i++)
                    remainingGroup.Regions.Add(new Region(
                        Path.GetRelativePath(path, remainingFiles[i]),
                        map.MidiNotes[firstNote + i * interval]));
            }
            map.StretchRegions(stretchMode);
            map.SequenceLength = rrCount;
            map.CreateVars();
            string outputPath = Path.Join(Path.GetDirectoryName(path), "map.sfz");
            File.WriteAllText(outputPath, map.Render(path));
        }
    }
}
