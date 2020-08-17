using System.Linq;

namespace filenameParser.Extensions
{
    public static class Extensions
    {
        public static string Indent(this string str)
        {
          var result = str.Split('\n').Select(line => $"\t{line}").ToList();
          return string.Join('\n', result);
        }
    }
}
