﻿using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class MyTestRepositoryItemData : IRepositoryItemData
    {
        private static int NextId;
        private string name;
        private bool overrideDefaultName;

        public MyTestRepositoryItemData(ItemId parentId, ItemId.ItemType itemType)
        {
            ParentId = parentId;
            Id = new ItemId(itemType, ++NextId);
        }

        public MyTestRepositoryItemData(MyTestRepositoryItemData other)
        {
            ParentId = other.ParentId;
            Id = other.Id;
            Name = other.Name;
        }

        public Dictionary<TaggedValues, string> TaggedValues { get; set; }

        #region IRepositoryItemData Members

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }

        public string Name
        {
            get { return overrideDefaultName ? name : Stereotype + "_" + Id.Value; }
            set
            {
                overrideDefaultName = true;
                name = value;
            }
        }

        public string Stereotype { get; set; }

        public string GetTaggedValue(TaggedValues key)
        {
            string value;
            if (TaggedValues != null && TaggedValues.TryGetValue(key, out value))
            {
                return value;
            }
            return string.Empty;
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            string value = GetTaggedValue(key);
            return string.IsNullOrEmpty(value) ? new string[0] : new[] {value};
        }

        #endregion
    }
}