using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class ListExtensions
    {
        public static void Each<T>(this IList<T> list, Action<T> action)
        {
            foreach (var element in list)
            {
                action(element);
            }
        }
    }
}