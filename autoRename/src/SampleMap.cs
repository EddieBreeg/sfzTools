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
        public List<SampleGroup> Groups = new List<SampleGroup>();
        //public List<string> DynamicLevels = new List<string>();
        //public List<string> Articulations = new List<string>();
        //public List<string> RoundRobins = new List<string>();
        public SampleMap()
        {
            string[] names = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            for (int i=0; i<128; i++)
            {
                MidiNotes[i] = names[i % 12] + Convert.ToString(i/12);
            }
        }
        public void AddRegion(Region region, string groupName)
        {
            int index = FindGroup(groupName);
            if (index != -1)
                Groups[index].Regions.Add(region);
            else
            {
                SampleGroup g = new SampleGroup(groupName, region);
                Groups.Add(g);
            }
        }
        int FindGroup(string groupName)
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
            foreach (SampleGroup g in Groups)
                g.SortRegions();
        }
        public void StretchRegions(char mode)
        {
            foreach (SampleGroup g in Groups)
            {
                switch (mode)
                {
                    case '1':
                        g.Regions[0].LoKey = 0;
                        for (int i = 1; i < g.Regions.Count; i++)
                            g.Regions[i].LoKey = g.Regions[i - 1].MidiNumber + 1;
                        break;
                    case '2':
                        g.Regions[^1].HiKey = MidiNotes.Length - 1;
                        for (int i = 0; i < g.Regions.Count-1; i++)
                            g.Regions[i].HiKey = g.Regions[i + 1].MidiNumber - 1;
                        break;
                    default:
                        break;
                }
            }
        }
        public string Render(string defaultPath)
        {
            string result = $"<control>\ndefault_path={Path.GetFileNameWithoutExtension(defaultPath)}/\n\n";
            foreach (SampleGroup g in Groups)
                result += g.Render();
            return result;
        }
    }

    class SampleGroup
    {
        public List<Region> Regions = new List<Region>();
        public string Name;
        public SampleGroup(string name)
        {
            Name = name;
        }
        public SampleGroup(string name, Region region)
        {
            Name = name;
            Regions.Add(region);
        }
        public override string ToString()
        {
            return Name;
        }
        public void SortRegions()
        {
            Regions.Sort();
        }
        public string Render()

        {
            string result = $"<group> //{Name}\n";
            foreach (Region region in Regions)
                result += region.Render().Indent()+"\n\n";
            return result;
        }
    }

    class Region: IComparable
    {
        public readonly string Root;
        public int LoKey;
        public int HiKey;
        public readonly string File;
        public readonly int MidiNumber;

        public Region(string file, string root)
        {
            SampleMap m = new SampleMap();
            List<string> midiNotes = m.MidiNotes.ToList();
            File = file;
            Root = root;
            MidiNumber = midiNotes.IndexOf(root);
            LoKey = MidiNumber;
            HiKey = MidiNumber;
            
        }
        public override string ToString()
        {
            return File;
        }
        public int CompareTo(object obj)
        {
            if (obj is Region other)
                return this.MidiNumber.CompareTo(other.MidiNumber);
            else
                throw new ArgumentException("Excpected a Region object");
        }
        public string Render()
        {
            string result = "<region>\n";
            result += $"sample={Path.GetFileName(File)}\n";
            result += $"pitch_keycenter={Root}\n";
            result += $"lokey={LoKey}\n";
            result += $"hikey={HiKey}";

            return result;
        }
    }
    
}
