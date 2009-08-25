using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    internal class TestRepositoryItemData : MyTestRepositoryItemData
    {
        public TestRepositoryItemData(ItemId parentId):base(parentId, ItemId.ItemType.Package)
        {
        }

        public TestRepositoryItemData(TestRepositoryItemData other):base(other)
        {
            SomeData = other.SomeData;
        }

        public string SomeData { get; set; }
    }
}