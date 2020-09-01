using System;
using System.Collections.Generic;
using System.Linq;

namespace autoBuilder.Modules
{
    public class ListModules<T>
    {
        public static List<List<T>> SplitList(List<T> lst, int count)
        {
            var result = new List<List<T>>();
            for (int i = 0; i < count; i++)
                result.Add(lst.GetRange(i * lst.Count / count, lst.Count / count));
            return result;
        }
        public static List<T> Sorted(List<T> lst)
        {
            var result = DeepCopy(lst);
            result.Sort();
            return result;
        }
        public static List<T> DeepCopy(List<T> lst)
        {
            var result = new List<T>();
            lst.ForEach(x => result.Add(x));
            return result;
        }
        public static List<T> WithoutIndex(List<T> lst, int index)
        {
            var result = new List<T>();
            for (int i = 0; i < lst.Count; i++)
                if (i != index)
                    result.Add(lst[i]);
            return result;
        }
        public static List<T> Permute(List<T> lst, List<int> lut)
        {
            if (lst.Count != lut.Count || !lut.IsPermutation()) throw new ArgumentException();
            return lut.Select(x => lst[x]).ToList();
        }
    }
    public static class ListExtensions
    {
        public static bool IsPermutation(this List<int> lst)
        {
            var copy = ListModules<int>.Sorted(lst);
            for (int i = 0; i < copy.Count; i++)
                if (copy[i] != i) return false;
            return true;
        }
        public static List<int> FlipPermutation(this List<int> lut)
        {
            if (!lut.IsPermutation()) throw new FormatException("List is not a permutation");
            return lut.Select(x => lut[x]).ToList();
        }
        public static int CompareTo(this int[] a, int[] b)
        {
            var range = a.Length < b.Length ? a.Length : b.Length;
            for (int i = 0; i < range; i++)
                if (a[i] != b[i]) return a[i].CompareTo(b[i]);
            return a.Length.CompareTo(b.Length);
        }
    }
    public static class StringExtensions
    {
        public static List<int> FindNumbers(this string str)
        {
            var copy = "";
            var digits = "0123456789";
            foreach (char c in str)
                copy += digits.Contains(c) ? c : ' ';
            var result = new List<int>();
            return copy.Split(' ').Select(x => Convert.ToInt32(x)).ToList();
        }
        internal static int[] ToIntArray(this string s)
        {
            var result = new List<int>();
            for(int i = 0; i < s.Length; i++)
            {
                if (!"0123456789".Contains(s[i])) result.Add((int)s[i]);
                else
                {
                    string n = "";
                    while(i < s.Length && "0123456789".Contains(s[i]))
                    {
                        n += s[i];
                        i++;
                    }
                    result.Add(Convert.ToInt32(n));
                }
            }
            return result.ToArray();
        }
        public static int CompareNumerically(this string str, string other)
        {
            int[] a = str.ToIntArray();
            int[] b = other.ToIntArray();
            return a.CompareTo(b);
        }
    }
    public class NumericalStringComparer: IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return a.CompareNumerically(b);
        }
    }
}