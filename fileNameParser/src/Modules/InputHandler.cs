using System;
using System.Linq;

namespace filenameParser.Modules
{
    public static class InputHandler
    {
        public static char KeyInput(string str, char[] choices, char defaultValue)
        {
            Console.Write(str);
            var value = Console.ReadKey().KeyChar;
            while(!choices.Contains(value) && value != '\r')
            {
                Console.WriteLine("Invalid choice! Please try again.");
                Console.Write(str);
                value = Console.ReadKey().KeyChar;
            }
            if (value == '\r')
                return defaultValue;
            return value;
        }

        public static string LineInput(string str, string[] choices, string defaultValue)
        {
            Console.Write(str);
            var value = Console.ReadLine();
            while (!choices.Contains(value) && value != string.Empty)
            {
                Console.WriteLine("Invalid choice! Please try again.");
                Console.Write(str);
                value = Console.ReadLine();
            }
            if (value == string.Empty)
                return defaultValue;
            return value;
        }
        public static string LineInput(string str, string defaultValue = null)
        {
            Console.Write(str);
            var input = Console.ReadLine();
            if (defaultValue != null) return input != "" ? input : defaultValue;
            return input;
        }
    }
}