using System;
using System.Linq;
using System.Collections.Generic;

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
        public static int NumberInput(string str, int? defaultValue = null)
        {
            Console.Write(str);
            string? input = Console.ReadLine();
            input = input != "" ? input : Convert.ToString(defaultValue);
            while (defaultValue == null && input == Convert.ToString(defaultValue))
            {
                Console.WriteLine("Invalid choice!");
                input = Convert.ToString(NumberInput(str, defaultValue));
            }
            return Convert.ToInt32(input);
        }
        public static List<int> ListInput(string str, List<int> defaultValue=null, Func<List<int>, bool> condition=null)
        {
            Console.Write(str);
            if (condition == null) condition = x => true;
            string input = Console.ReadLine();
            List<int> result;
            try
            {
                result = input.Length > 0 ? input.Split(' ').Select(x => Convert.ToInt32(x)).ToList(): defaultValue;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid choice!");
                return ListInput(str, defaultValue, condition);
            }
            if(!condition(result) || result==null)
            {
                Console.WriteLine("Invalid choice!");
                return ListInput(str, defaultValue, condition);
            }
            return result;
        }
        public static List<string> ListInput(string str, List<string> defaultValue = null, Func<List<string>, bool> condition = null)
        {
            Console.Write(str);
            if (condition == null) condition = x => true;
            string input = Console.ReadLine();
            List<string> result;
            try
            {
                result = input.Length > 0 ? input.Split(' ').ToList() : defaultValue;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid choice!");
                return ListInput(str, defaultValue, condition);
            }
            if (!condition(result) || result == null)
            {
                Console.WriteLine("Invalid choice!");
                return ListInput(str, defaultValue, condition);
            }
            return result;
        }
    }
}