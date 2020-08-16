using System.Collections.Generic;

namespace Extensions
{
    static class Extensions
    {
        public static string Indent(this string str)
        {
            List<string> result = new List<string>();
            foreach (string line in str.Split('\n'))
                result.Add($"\t{line}");
            return string.Join('\n', result);
        }
    }
}
