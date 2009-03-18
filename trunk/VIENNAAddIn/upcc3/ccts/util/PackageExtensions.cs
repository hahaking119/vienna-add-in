// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class PackageExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Package package, TaggedValues key)
        {
            return package.Element.GetTaggedValues(key);
        }

        internal static string GetTaggedValue(this Package package, TaggedValues key)
        {
            var tv = ElementExtensions.GetTaggedValue(package.Element, key);
            return tv ?? string.Empty;
        }

        internal static void SetTaggedValues(this Package package, TaggedValues key, IEnumerable<string> values)
        {
            package.Element.SetTaggedValues(key, values);
        }

        internal static void SetTaggedValue(this Package package, TaggedValues key, string value)
        {
            package.Element.SetTaggedValue(key, value);
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