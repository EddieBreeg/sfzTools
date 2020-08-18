using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using filenameParser.Modules;

namespace filenameParser
{

    public class SampleMap
    {
        public string[] MidiNotes { get; } = new string[128];
        public List<SampleGroup> Groups { get; } = new List<SampleGroup>();
        //public List<string> DynamicLevels = new List<string>();
        //public List<string> Articulations = new List<string>();
        //public List<string> RoundRobins = new List<string>();
        public SampleMap()
        {
            string[] notes = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            for (int i = 0; i < MidiNotes.Length; i++)
                MidiNotes[i] = $"{notes[i % 12]}{i / 12}";
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
            for(var i=0; i<Groups.Count;i++)
            {
                if (Groups[i].name == groupName)
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
                        //g.Regions[0].loKey = 0;
                        for (var i = 1; i < g.Regions.Count; i++)
                            g.Regions[i].loKey = g.Regions[i - 1].midiNumber + 1;
                        break;
                    case '2':
                        //g.Regions[^1].hiKey = MidiNotes.Length - 1;
                        for (var i = 0; i < g.Regions.Count-1; i++)
                            g.Regions[i].hiKey = g.Regions[i + 1].midiNumber - 1;
                        break;
                    default:
                        break;
                }
            }
        }
        public string Render(string defaultPath)
        {
            var result = $"<control>\ndefault_path={Path.GetFileNameWithoutExtension(defaultPath)}/\n\n";
            Groups.ForEach(group => result += group.Render());
            return result;
        }
    }

    public class SampleGroup
    {
        public List<Region> Regions { get; } = new List<Region>();
        public readonly string name;
        public SampleGroup(string name) => this.name = name;
        public SampleGroup(string name, Region region)
        {
            this.name = name;
            Regions.Add(region);
        }
#if (DEBUG)
        public override string ToString() => name;
#endif
        public void SortRegions() => Regions.Sort();
      
        public string Render()
        {
            var result = $"<group> //{name}\n";
            Regions.ForEach(r => result += r.Render().Indent() + "\n\n");
            return result;
        }
    }

    public class Region : IComparable
    {
        private readonly string _root;
        internal int loKey, hiKey;
        private readonly string _file;
        internal readonly int midiNumber;

        public Region(string file, string root)
        {
            var map = new SampleMap();
            var midiNotes = map.MidiNotes.ToList();
            _file = file;
            _root = root;
            midiNumber = midiNotes.IndexOf(root);
            loKey = midiNumber;
            hiKey = midiNumber;
            
        }
#if (DEBUG)
        public override string ToString() => _file;
#endif

        public int CompareTo(object obj)
        {
            if (obj is Region other)
                return midiNumber.CompareTo(other.midiNumber);
            
            throw new ArgumentException("Expected a Region object");
        }
        public string Render()
        {
            var result = $"<region>\n sample={Path.GetFileName(_file)}\n pitch_keycenter={_root}\n lokey={loKey}\n hikey={hiKey}";
            return result;
        }
    }
}
