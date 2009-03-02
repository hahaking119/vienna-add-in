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

        internal static Package PackageByName(this Package package, string name)
        {
            foreach (Package child in package.Packages)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }

        internal static Element ElementByName(this Package package, string name)
        {
            foreach (Element element in package.Elements)
            {
                if (element.Name == name)
                {
                    return element;
                }
            }
            return null;
        }
    }
}