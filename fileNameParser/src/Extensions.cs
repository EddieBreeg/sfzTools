using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    static class Extensions
    {
        public static string Indent(this string str) 
            => string.Join('\n', str.Split('\n').Select(line => $"\t{line}").ToList());
    }
}
