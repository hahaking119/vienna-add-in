using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class RepositoryItemBuilder
    {
        private static int NextId;
        protected Dictionary<TaggedValues, string> taggedValues = new Dictionary<TaggedValues, string>();

        protected ItemId id = ItemId.Null;
        protected string name;
        protected ItemId parentId = ItemId.Null;
        protected string stereotype = string.Empty;

        public RepositoryItemBuilder()
        {
        }

        private RepositoryItemBuilder(RepositoryItemBuilder other)
        {
            id = other.id;
            name = other.name;
            parentId = other.parentId;
            stereotype = other.stereotype;
            taggedValues = new Dictionary<TaggedValues, string>(other.taggedValues);
        }

        public virtual RepositoryItem Build()
        {
            return new RepositoryItem(
                id,
                parentId,
                name ?? stereotype + "_" + id.Value,
                stereotype,
                ConvertTaggedValues());
        }

        protected Dictionary<string, string> ConvertTaggedValues()
        {
            var tv = new Dictionary<string, string>();
            foreach (var keyValuePair in taggedValues)
            {
                tv[keyValuePair.Key.ToString()] = keyValuePair.Value;
            }
            return tv;
        }

        public RepositoryItemBuilder WithItemType(ItemId.ItemType itemType)
        {
            return new RepositoryItemBuilder(this)
                   {
                       id = new ItemId(itemType, ++NextId)
                   };
        }

        public RepositoryItemBuilder WithParentId(ItemId value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       parentId = value
                   };
        }

        public RepositoryItemBuilder WithName(string value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       name = value
                   };
        }

        public RepositoryItemBuilder WithStereotype(string value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       stereotype = value
                   };
        }

        public RepositoryItemBuilder WithTaggedValues(Dictionary<TaggedValues, string> value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       taggedValues = value
                   };
        }

        public RepositoryItemBuilder WithTaggedValue(TaggedValues key, string value)
        {
            var copy = new RepositoryItemBuilder(this);
            copy.taggedValues[key] = value;
            return copy;
        }

        public RepositoryItemBuilder WithoutTaggedValue(TaggedValues key)
        {
            var copy = new RepositoryItemBuilder(this);
            copy.taggedValues.Remove(key);
            return copy;
        }

        public RepositoryItemBuilder WithId(ItemId value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       id = value
                   };
        }
    }
}