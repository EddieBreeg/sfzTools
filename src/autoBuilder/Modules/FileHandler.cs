using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace autoBuilder.Modules
{
    public class LabeledFile: IComparable
    {
        public string Path { get; set; }
        public List<int> HierachyLabels = new List<int>();
        public LabeledFile(string path) => Path = path;
        public int CompareTo(object obj)
        {
            if (obj is LabeledFile other)
            {
                var range = HierachyLabels.Count > other.HierachyLabels.Count ? HierachyLabels.Count : other.HierachyLabels.Count;
                for (int i = 0; i < range; i++)
                {
                    if (HierachyLabels[i] != other.HierachyLabels[i])
                        return HierachyLabels[i].CompareTo(other.HierachyLabels[i]);
                }
                return HierachyLabels.Count.CompareTo(other.HierachyLabels.Count);
            }
            throw new ArgumentException("Expected a Hierarchy object");
        }
        public override string ToString() => $"{string.Join(", ", HierachyLabels)}: {Path}";
    }
    public class HierarchyHandler: IEnumerable
    {
        public List<LabeledFile> Files;
        public List<int> GroupsPerLevel = new List<int>() { 1 };
        public HierarchyHandler() => Files = new List<LabeledFile>();
        public HierarchyHandler(List<string> files)
        {
            Files = new List<LabeledFile>(); 
            files.ForEach(f => Files.Add(new LabeledFile(f)));
        }
        public HierarchyHandler(List<LabeledFile> files) => Files = files;
        public void Sort() => Files.Sort();
        public override string ToString() => string.Join('\n', Files.Select(f=>f.Path));
        public void LabelFiles()
        {
            Files = SetGroups(Files, GroupsPerLevel);
        }
        private List<LabeledFile> SetGroups(List<LabeledFile> files, List<int> groupsPerLevel)
        {
            if (groupsPerLevel.Count==0)
            {
                for (int i = 0; i < files.Count; i++)
                    files[i].HierachyLabels.Add(i);
                return files;
            }
            var result = new List<LabeledFile>();
            var groups = files.Split(groupsPerLevel[0]);
            for(int i=0;i<groups.Count;i++)
            {
                groups[i].ForEach(f => f.HierachyLabels.Add(i));
                result.AddRange(SetGroups(groups[i], groupsPerLevel.WithoutIndex(0)));
            }
            return result;
        }
        public IEnumerator GetEnumerator() => Files.GetEnumerator();
        public void ForEach(Action<LabeledFile> action)
        {
            foreach (var file in Files) action(file);
        }
    }
}
