using System.Collections.Generic;

namespace Extensions
{
    static class Extensions
    {
        public static string Indent(this string str)
        {
            var result = new List<string>();
            foreach (var line in str.Split('\n'))
                result.Add($"\t{line}");
            return string.Join('\n', result);
        }
    }
}
