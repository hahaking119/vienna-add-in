// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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

        public static string ConcatToString<T>(this IEnumerable<T> values)
        {
            return values == null ? String.Empty : String.Concat(ToStringArray(values));
        }

        public static string JoinToString<T>(this IEnumerable<T> values, string separator)
        {
            return values == null ? String.Empty : String.Join(separator, ToStringArray(values));
        }

        private static string[] ToStringArray<T>(IEnumerable<T> values)
        {
            return new List<string>(values.Convert(v => v.ToString())).ToArray();
        }
    }
}