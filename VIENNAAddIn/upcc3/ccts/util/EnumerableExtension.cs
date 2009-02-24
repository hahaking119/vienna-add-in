using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TOutput> Convert<TInput, TOutput>(this IEnumerable<TInput> inputs,
                                                                    Converter<TInput, TOutput> convert)
        {
            foreach (TInput input in inputs)
            {
                yield return convert(input);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

    }
}