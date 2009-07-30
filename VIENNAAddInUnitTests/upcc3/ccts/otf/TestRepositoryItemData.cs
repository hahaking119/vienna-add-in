using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    internal class TestRepositoryItemData : IRepositoryItemData
    {
        private static int NextId;

        public TestRepositoryItemData(ItemId parentId)
        {
            Id = ItemId.ForPackage(++NextId);
            ParentId = parentId;
        }

        public TestRepositoryItemData(TestRepositoryItemData other)
        {
            Id = other.Id;
            ParentId = other.ParentId;
            Name = other.Name;
            SomeData = other.SomeData;
        }

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }
        public string Name { get; set; }
        public string Stereotype { get; set; }

        public string GetTaggedValue(TaggedValues key)
        {
            return null;
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return new string[0];
        }

        public string SomeData { get; set; }
    }
}