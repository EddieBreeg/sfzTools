using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;

namespace fileNameParser.SampleMap
{

    class SampleMap
    {
        public readonly string[] MidiNotes = new string[128];
        public readonly List<SampleGroup> Groups = new List<SampleGroup>();
        //public List<string> DynamicLevels = new List<string>();
        //public List<string> Articulations = new List<string>();
        //public List<string> RoundRobins = new List<string>();
        public SampleMap()
        {
            string[] notes = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            for (int i = 0; i < MidiNotes.Length; i++)
                MidiNotes[i] = $"{notes[i % 12]} {i / 12}";
        }
        public void AddRegion(Region region, string groupName)
        {
            var index = FindGroup(groupName);
            if (index != -1)
                Groups[index].Regions.Add(region);
            else
            {
                var g = new SampleGroup(groupName, region);
                Groups.Add(g);
            }
        }
        private int FindGroup(string groupName)
        {
            for(int i=0; i<Groups.Count;i++)
            {
                if (Groups[i].Name == groupName)
                    return i;
            }
            return -1;
        }
        public void SortRegions()
        {
            foreach (var g in Groups)
                g.SortRegions();
        }
        public void StretchRegions(char mode)
        {
            foreach (var g in Groups)
            {
                switch (mode)
                {
                    case '1':
                        g.Regions[0].LoKey = 0;
                        for (var i = 1; i < g.Regions.Count; i++)
                            g.Regions[i].LoKey = g.Regions[i - 1].MidiNumber + 1;
                        break;
                    case '2':
                        g.Regions[^1].HiKey = MidiNotes.Length - 1;
                        for (var i = 0; i < g.Regions.Count-1; i++)
                            g.Regions[i].HiKey = g.Regions[i + 1].MidiNumber - 1;
                        break;
                    default:
                        break;
                }
            }
        }
        public string Render(string defaultPath)
        {
            var result = $"<control>\ndefault_path={Path.GetFileNameWithoutExtension(defaultPath)}/\n\n";
            return Groups.Aggregate(result, (current, g) => current + g.Render());
        }
    }

    class SampleGroup
    {
        public readonly List<Region> Regions = new List<Region>();
        public string Name;
        public SampleGroup(string name) { Name = name; }
        public SampleGroup(string name, Region region)
        {
            Name = name;
            Regions.Add(region);
        }
#if (DEBUG)
        public override string ToString() => Name;
#endif
        public void SortRegions() { Regions.Sort(); }
      
        public string Render()
        {
            var result = $"<group> //{Name}\n";
            
            Regions.ForEach(region => result += region.Render().Indent()+"\n\n");
            return result;
        }
    }

    class Region : IComparable
    {
        private readonly string Root;
        public int LoKey, HiKey;
        private readonly string File;
        public readonly int MidiNumber;

        public Region(string file, string root)
        {
            var m = new SampleMap();
            var midiNotes = m.MidiNotes.ToList();
            File = file;
            Root = root;
            MidiNumber = midiNotes.IndexOf(root);
            LoKey = MidiNumber;
            HiKey = MidiNumber;
            
        }
#if (DEBUG)
        public override string ToString() => File;
#endif

        public int CompareTo(object obj)
        {
            if (obj is Region other)
                return MidiNumber.CompareTo(other.MidiNumber);
            
            throw new ArgumentException("Expected a Region object");
        }
        public string Render()
        {
            var result = $"<region>\n sample={Path.GetFileName(File)}\n pitch_keycenter={Root}\n lokey={LoKey}\n hikey={HiKey}";
            return result;
        }
    }
}
