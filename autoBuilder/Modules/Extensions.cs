using System;
using System.Collections.Generic;

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
        public static List<T> WithoutIndex(List<T> lst, int index)
        {
            var result = new List<T>();
            for (int i = 0; i < lst.Count; i++)
                if (i != index)
                    result.Add(lst[i]);
            return result;
        }
    }
}