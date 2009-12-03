using System.Collections.Generic;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class MultiPartTaggedValue
    {
        /// <summary>
        /// Separator for multiple values.
        /// </summary>
        public const char ValueSeparator = '|';

        public static string Merge(IEnumerable<string> values)
        {
            if (values == null)
            {
                return string.Empty;
            }
            var stringArray = new List<string>(values.Convert(v => (v == null ? string.Empty : v.ToString()))).ToArray();
            return string.Join(string.Empty + ValueSeparator, stringArray);
        }

        public static IEnumerable<string> Split(string value)
        {
            return string.IsNullOrEmpty(value) ? new string[0] : value.Split(ValueSeparator);
        }
    }
}