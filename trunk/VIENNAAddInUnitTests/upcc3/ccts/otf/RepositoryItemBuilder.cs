using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class RepositoryItemBuilder
    {
        private static int NextId;

        private ItemId id = ItemId.Null;
        private ItemId parentId = ItemId.Null;
        private string name;
        private string stereotype = string.Empty;
        private Dictionary<string, string> taggedValues = new Dictionary<string, string>();

        public RepositoryItem Build()
        {
            return new RepositoryItem(
                id, 
                parentId, 
                name ?? stereotype + "_" + id.Value, 
                stereotype, 
                taggedValues);
        }

        public RepositoryItemBuilder WithItemType(ItemId.ItemType itemType)
        {
            id = new ItemId(itemType, ++NextId);
            return this;
        }

        public RepositoryItemBuilder WithParentId(ItemId value)
        {
            parentId = value;
            return this;
        }

        public RepositoryItemBuilder WithName(string value)
        {
            name = value;
            return this;
        }

        public RepositoryItemBuilder WithStereotype(string value)
        {
            stereotype = value;
            return this;
        }

        public RepositoryItemBuilder WithTaggedValues(Dictionary<string, string> value)
        {
            taggedValues = value;
            return this;
        }

        public RepositoryItemBuilder WithId(ItemId value)
        {
            id = value;
            return this;
        }
    }
}