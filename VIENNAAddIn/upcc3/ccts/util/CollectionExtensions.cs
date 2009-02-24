using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this Collection collection)
        {
            foreach (T item in collection)
            {
                yield return item;
            }
        }
    }
}