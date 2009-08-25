using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class PackageData : RepositoryItem
    {
        public PackageData(ItemId id, ItemId parentId, string name, string stereotype, Dictionary<string, string> taggedValues) : base(id, parentId, name, stereotype, taggedValues)
        {
        }

        public static PackageData FromPackage(Package package)
        {
            ItemId id = ItemId.ForPackage(package.PackageID);
            ItemId parentId = ItemId.ForPackage(package.ParentID);
            string name = package.Name;
            string stereotype;
            var taggedValues = new Dictionary<string, string>();
            if (package.Element != null)
            {
                stereotype = package.Element.Stereotype;
                foreach (TaggedValue taggedValue in package.Element.TaggedValues)
                {
                    taggedValues[taggedValue.Name] = taggedValue.Value;
                }
            }
            else
            {
                stereotype = string.Empty;
            }
            return new PackageData(id, parentId, name, stereotype, taggedValues);
        }
    }
}