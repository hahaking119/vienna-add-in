using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ElementData : RepositoryItem
    {
        public ElementData(ItemId id, ItemId parentId, string name, string stereotype, Dictionary<string, string> taggedValues) : base(id, parentId, name, stereotype, taggedValues)
        {
        }

        public static ElementData FromElement(Element element)
        {
            return new ElementData(
                ItemId.ForElement(element.ElementID),
                ItemId.ForPackage(element.PackageID),
                element.Name,
                element.Stereotype,
                new Dictionary<string, string>()
                );
        }
    }
}