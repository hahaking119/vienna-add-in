using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ElementRepositoryItem : RepositoryItem
    {
        public ElementRepositoryItem(ItemId id, ItemId parentId, string name, string stereotype, Dictionary<string, string> taggedValues) : base(id, parentId, name, stereotype, taggedValues)
        {
        }

        public static ElementRepositoryItem FromElement(Element element)
        {
            return new ElementRepositoryItem(
                ItemId.ForElement(element.ElementID),
                ItemId.ForPackage(element.PackageID),
                element.Name,
                element.Stereotype,
                new Dictionary<string, string>()
                );
        }
    }
}