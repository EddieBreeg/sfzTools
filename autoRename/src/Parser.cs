using System;
using System.Collections.Generic;
using System.IO;


namespace Parser
{
    class FileNameParser
    {
        public char Separator { get; set; } = '_';
        public string[] SplitName(string filename)

        
        {
            return Path.GetFileNameWithoutExtension(filename).Split(Separator);
        }
        public string[] GetValues(string[] files, int index=0)
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                string value = SplitName(Path.GetFileNameWithoutExtension(file))[index];
                if (!result.Contains(value))
                    result.Add(value);
            }
            return result.ToArray();
        }
    }
}