using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class RepositoryItem : IRepositoryItem
    {
        private Dictionary<ItemId, RepositoryItem> children = new Dictionary<ItemId, RepositoryItem>();

        public RepositoryItem(IRepositoryItemData itemData)
        {
            Data = itemData;
        }

        public IEnumerable<RepositoryItem> Children
        {
            get { return new List<RepositoryItem>(children.Values); }
        }

        public IRepositoryItemData Data { get; set; }

        public ItemId Id
        {
            get { return Data.Id; }
        }

        public IEnumerable<IValidationIssue> Validate()
        {
            if (Stereotype.BLibrary == Data.Stereotype)
            {
                if (string.IsNullOrEmpty(Data.Name))
                {
                    yield return new LibraryNameNotSpecified(Id);
                }
                if (string.IsNullOrEmpty(Data.GetTaggedValue(TaggedValues.uniqueIdentifier)))
                {
                    yield return new LibraryMandatoryTaggedValueNotSpecified(Id, Data.Name, TaggedValues.uniqueIdentifier);
                }
                if (string.IsNullOrEmpty(Data.GetTaggedValue(TaggedValues.versionIdentifier)))
                {
                    yield return new LibraryMandatoryTaggedValueNotSpecified(Id, Data.Name, TaggedValues.versionIdentifier);
                }
                if (string.IsNullOrEmpty(Data.GetTaggedValue(TaggedValues.baseURN)))
                {
                    yield return new LibraryMandatoryTaggedValueNotSpecified(Id, Data.Name, TaggedValues.baseURN);
                }
                string parentStereotype = Parent.Data.Stereotype;
                if (!(Parent.Data.ParentId.IsNull || Stereotype.BInformationV == parentStereotype || Stereotype.BLibrary == parentStereotype))
                {
                    yield return new InvalidParentPackage(Id, Data.Name);
                }
                foreach (var child in Children)
                {
                    if (child.Id.Type == ItemId.ItemType.Element)
                    {
                        yield return new NoElementsAllowed(Id, child.Id, Data.Name);
                    }
                    else
                    {
                        if (!(Stereotype.IsBusinessLibraryStereotype(child.Data.Stereotype)))
                        {
                            yield return new InvalidSubPackageStereotype(Id, child.Id, Data.Name, child.Data.Name);
                        }
                    }
                }

            }
            else
            {
                yield break;
            }
        }

        public IRepositoryItem Parent { get; set; }

        public RepositoryItem AddOrReplaceChild(RepositoryItem item)
        {
            RepositoryItem oldItem;
            children.TryGetValue(item.Id, out oldItem);
            children[item.Id] = item;
            item.Parent = this;
            return oldItem;
        }

        public void RemoveChild(ItemId id)
        {
            children.Remove(id);
        }

        public void CopyChildren(RepositoryItem item)
        {
            if (item != null)
            {
                children = new Dictionary<ItemId, RepositoryItem>(item.children);
            }
        }
    }
}