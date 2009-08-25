using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class RepositoryItem
    {
        private readonly Dictionary<string, string> taggedValues;
        private Dictionary<ItemId, RepositoryItem> children = new Dictionary<ItemId, RepositoryItem>();

        public RepositoryItem(ItemId id, ItemId parentId, string name, string stereotype, Dictionary<string, string> taggedValues)
        {
            this.taggedValues = taggedValues;
            Id = id;
            ParentId = parentId;
            Name = name;
            Stereotype = stereotype;
        }

        public RepositoryItem Parent { get; set; }

        public IEnumerable<RepositoryItem> Children
        {
            get { return new List<RepositoryItem>(children.Values); }
        }

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }
        public string Name { get; private set; }
        public string Stereotype { get; private set; }

        public string GetTaggedValue(TaggedValues key)
        {
            string value;
            taggedValues.TryGetValue(key.ToString(), out value);
            return value.DefaultTo(string.Empty);
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return MultiPartTaggedValue.Split(GetTaggedValue(key));
        }

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