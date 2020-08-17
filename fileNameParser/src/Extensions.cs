using System.Linq;

namespace Extensions
{
    static class Extensions
    {
        public static string Indent(this string str)
        {
          var result = str.Split('\n').Select(line => $"\t{line}").ToList();
          return string.Join('\n', result);
        }
    }
}
