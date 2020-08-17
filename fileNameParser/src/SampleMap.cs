using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;

namespace fileNameParser.SampleMap
{

    class SampleMap
    {
        public readonly string[] midiNotes = new string[128];
        public readonly List<SampleGroup> groups = new List<SampleGroup>();
        //public List<string> DynamicLevels = new List<string>();
        //public List<string> Articulations = new List<string>();
        //public List<string> RoundRobins = new List<string>();
        public SampleMap()
        {
            string[] notes = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            for (int i = 0; i < midiNotes.Length; i++)
                midiNotes[i] = $"{notes[i % 12]} {i / 12}";
        }
        public void AddRegion(Region region, string groupName)
        {
            var index = FindGroup(groupName);
            if (index != -1)
                groups[index].regions.Add(region);
            else
            {
                var g = new SampleGroup(groupName, region);
                groups.Add(g);
            }
        }
        private int FindGroup(string groupName)
        {
            for(var i=0; i<groups.Count;i++)
            {
                if (groups[i].name == groupName)
                    return i;
            }
            return -1;
        }
        public void SortRegions()
        {
            foreach (var g in groups)
                g.SortRegions();
        }
        public void StretchRegions(char mode)
        {
            foreach (var g in groups)
            {
                switch (mode)
                {
                    case '1':
                        g.regions[0].loKey = 0;
                        for (var i = 1; i < g.regions.Count; i++)
                            g.regions[i].loKey = g.regions[i - 1].midiNumber + 1;
                        break;
                    case '2':
                        g.regions[^1].hiKey = midiNotes.Length - 1;
                        for (var i = 0; i < g.regions.Count-1; i++)
                            g.regions[i].hiKey = g.regions[i + 1].midiNumber - 1;
                        break;
                    default:
                        break;
                }
            }
        }
        public string Render(string defaultPath)
        {
            var result = $"<control>\ndefault_path={Path.GetFileNameWithoutExtension(defaultPath)}/\n\n";
            groups.ForEach(group => result += group.Render());
            return result;
        }
    }

    class SampleGroup
    {
        public readonly List<Region> regions = new List<Region>();
        public readonly string name;
        public SampleGroup(string name) { this.name = name; }
        public SampleGroup(string name, Region region)
        {
            this.name = name;
            regions.Add(region);
        }
#if (DEBUG)
        public override string ToString() => name;
#endif
        public void SortRegions() { regions.Sort(); }
      
        public string Render()
        {
            var result = $"<group> //{name}\n";
            regions.ForEach(r => result += r.Render().Indent() + "\n\n");
            return result;
        }
    }

    class Region : IComparable
    {
        private readonly string _root;
        public int loKey, hiKey;
        private readonly string _file;
        public readonly int midiNumber;

        public Region(string file, string root)
        {
            var m = new SampleMap();
            var midiNotes = m.midiNotes.ToList();
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
