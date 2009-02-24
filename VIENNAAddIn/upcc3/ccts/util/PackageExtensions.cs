using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class PackageExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Package package, TaggedValues taggedValue)
        {
            return package.Element.GetTaggedValues(taggedValue);
        }

        internal static string GetTaggedValue(this Package package, TaggedValues taggedValue)
        {
            return package.Element.GetTaggedValue(taggedValue);
        }
    }
}