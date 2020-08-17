using System.IO;


namespace Parser
{
    class FileNameParser
    {
        public char Separator { get; set; } = '_';

        public string[] SplitName(string filename) { return Path.GetFileNameWithoutExtension(filename).Split(Separator); }
    }
}