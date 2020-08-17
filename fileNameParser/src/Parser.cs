using System.IO;


namespace filenameParser
{
    class FileNameParser
    {
        public char Separator { get; set; } = '_';

        public string[] SplitName(string filename) => Path.GetFileNameWithoutExtension(filename).Split(Separator);
    }
}