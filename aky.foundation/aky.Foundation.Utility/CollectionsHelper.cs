namespace aky.Foundation.Utility
{
    using System;
    using System.Collections.Generic;

    public static class CollectionsHelper
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T current in source)
            {
                action(current);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int num = 0;
            foreach (T current in source)
            {
                action(current, num);
                num++;
            }
        }
    }
}
