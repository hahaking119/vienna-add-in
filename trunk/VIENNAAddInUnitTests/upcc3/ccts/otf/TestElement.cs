using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class TestElement : AbstractEAElement
    {
        private static int NextId;

        public TestElement(ItemId packageId)
            : this(ItemId.ForElement(++NextId), packageId)
        {
        }

        private TestElement(ItemId id, ItemId parentId)
            : base(id, "TestElement " + NextId, parentId)
        {
        }

        internal static TestElement Clone(TestElement element)
        {
            return new TestElement(element.Id, element.PackageId);
        }
    }
}