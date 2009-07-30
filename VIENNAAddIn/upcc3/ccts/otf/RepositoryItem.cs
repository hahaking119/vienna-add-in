using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class RepositoryItem : IRepositoryItem
    {
        private Dictionary<ItemId, RepositoryItem> children = new Dictionary<ItemId, RepositoryItem>();

        public RepositoryItem(IRepositoryItemData itemData)
        {
            Data = itemData;
        }

        #region IRepositoryItem Members

        public IRepositoryItem Parent { get; set; }

        public IEnumerable<RepositoryItem> Children
        {
            get { return new List<RepositoryItem>(children.Values); }
        }

        public IRepositoryItemData Data { get; set; }

        public ItemId Id
        {
            get { return Data.Id; }
        }

        #endregion

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