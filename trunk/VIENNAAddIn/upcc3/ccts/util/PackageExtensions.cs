using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class PackageExtensions
    {
        internal static List<string> CollectTaggedValues(this Package package, TaggedValues taggedValue)
        {
            return package.Element.CollectTaggedValues(taggedValue);
        }

        internal static string GetTaggedValue(this Package package, TaggedValues taggedValue)
        {
            return package.Element.GetTaggedValue(taggedValue);
        }
    }
}