using System.Collections.Generic;
using System.IO;


namespace filenameParser
{
    class FileNameParser
    {
        public char Separator { get; set; } = '_';

        public string[] SplitName(string filename) => Path.GetFileNameWithoutExtension(filename).Split(Separator);

        public string FindLongestName(List<string> filenames)
        {
            var longest = filenames[0];
            foreach (var file in filenames)
                if (SplitName(file).Length > SplitName(longest).Length) longest = file;
            
            return longest;
        }
    }
}