using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public static class CCItemWrapper
    {
        public static object Wrap(IRepositoryItem item)
        {
            object wrappedObject = null;
            var itemData = item.Data;
            if (!itemData.ParentId.IsNull)
            {
                switch (item.Data.Stereotype)
                {
                    case Stereotype.bLibrary:
                    {
                        wrappedObject = new BLibrary(itemData.Id,
                                                     itemData.Name,
                                                     itemData.ParentId,
                                                     itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                     itemData.GetTaggedValues(TaggedValues.copyright),
                                                     itemData.GetTaggedValues(TaggedValues.owner),
                                                     itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.PRIMLibrary:
                    {
                        wrappedObject = new PRIMLibrary(itemData.Id,
                                                        itemData.Name,
                                                        itemData.ParentId,
                                                        itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                        itemData.GetTaggedValues(TaggedValues.copyright),
                                                        itemData.GetTaggedValues(TaggedValues.owner),
                                                        itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.ENUMLibrary:
                    {
                        wrappedObject = new ENUMLibrary(itemData.Id,
                                                        itemData.Name,
                                                        itemData.ParentId,
                                                        itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                        itemData.GetTaggedValues(TaggedValues.copyright),
                                                        itemData.GetTaggedValues(TaggedValues.owner),
                                                        itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.CDTLibrary:
                    {
                        wrappedObject = new CDTLibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.CCLibrary:
                    {
                        wrappedObject = new CCLibrary(itemData.Id,
                                                      itemData.Name,
                                                      itemData.ParentId,
                                                      itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                      itemData.GetTaggedValues(TaggedValues.copyright),
                                                      itemData.GetTaggedValues(TaggedValues.owner),
                                                      itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.BDTLibrary:
                    {
                        wrappedObject = new BDTLibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.BIELibrary:
                    {
                        wrappedObject = new BIELibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.DOCLibrary:
                    {
                        wrappedObject = new DOCLibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                }
            }
            return wrappedObject;
        }
    }
}