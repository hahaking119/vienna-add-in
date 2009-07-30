using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class HierarchicalRepository
    {
        private readonly Dictionary<ItemId, RepositoryItem> itemsById = new Dictionary<ItemId, RepositoryItem>();

        public HierarchicalRepository()
        {
            Root = new RepositoryItem(new RootRepositoryItemData());
        }

        public RepositoryItem Root { get; private set; }

        /// <summary>
        /// Throws ArgumentException if itemData.ParentId is an unknown ID.
        /// </summary>
        /// <param name="itemData"></param>
        public void ItemLoaded(IRepositoryItemData itemData)
        {
            var parent = GetItemById(itemData.ParentId);
            if (parent == null)
            {
                throw new ArgumentException("itemData.ParentId is not a known item ID");
            }
            var item = new RepositoryItem(itemData);
            var oldItem = parent.AddOrReplaceChild(item);
            item.CopyChildren(oldItem);
            itemsById[item.Id] = item;
            if (OnItemCreatedOrModified != null)
            {
                OnItemCreatedOrModified(parent);
                OnItemCreatedOrModified(item);
            }
        }

        public RepositoryItem GetItemById(ItemId id)
        {
            if (id.IsNull)
            {
                return Root;
            }
            RepositoryItem item;
            itemsById.TryGetValue(id, out item);
            return item;
        }

        public event Action<IRepositoryItem> OnItemCreatedOrModified;
        public event Action<IRepositoryItem> OnItemDeleted;

        public void ItemDeleted(ItemId id)
        {
            var item = GetItemById(id);
            if (item != null)
            {
                RemoveChildFromParent(item);
                DeleteItem(item);
            }
        }

        private void RemoveChildFromParent(RepositoryItem item)
        {
            var parent = GetItemById(item.Data.ParentId);
            if (parent != null)
            {
                parent.RemoveChild(item.Id);
                if (OnItemCreatedOrModified != null)
                {
                    OnItemCreatedOrModified(parent);
                }
            }
        }

        private void DeleteItem(RepositoryItem item)
        {
            itemsById.Remove(item.Id);
            if (OnItemDeleted != null)
            {
                OnItemDeleted(item);
            }
            foreach (var child in item.Children)
            {
                DeleteItem(child);
            }
        }

        public IEnumerable<IRepositoryItem> AllItems()
        {
            return AllChildren(Root);
        }

        private static IEnumerable<IRepositoryItem> AllChildren(IRepositoryItem item)
        {
            foreach (var child in item.Children)
            {
                yield return child;
                foreach (var descendant in AllChildren(child))
                {
                    yield return descendant;
                }
            }
        }
        public IEnumerable<IRepositoryItem> FindAllMatching(Predicate<IRepositoryItem> predicate)
        {
            return FindAllMatching(predicate, Root);
        }

        private static IEnumerable<IRepositoryItem> FindAllMatching(Predicate<IRepositoryItem> predicate, IRepositoryItem item)
        {
            if (predicate(item))
            {
                yield return item;
            }
            foreach (var child in item.Children)
            {
                foreach (var match in FindAllMatching(predicate, child))
                {
                    yield return match;
                }
            }
        }
    }

    public class RootRepositoryItemData : IRepositoryItemData
    {
        #region IRepositoryItemData Members

        public ItemId Id
        {
            get { return ItemId.Null; }
        }

        public ItemId ParentId
        {
            get { return ItemId.Null; }
        }

        public string Name
        {
            get { return "root"; }
        }

        public string Stereotype
        {
            get { return null; }
        }

        public string GetTaggedValue(TaggedValues key)
        {
            return null;
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return new string[0];
        }

        #endregion
    }
}