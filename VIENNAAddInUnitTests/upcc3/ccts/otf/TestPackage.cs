using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class TestPackage : AbstractEAPackage
    {
        private static int NextId;

        public TestPackage(ItemId parentId)
            : this(ItemId.ForPackage(++NextId), parentId)
        {
        }

        private TestPackage(ItemId id, ItemId parentId)
            : base(id, "TestPackage " + NextId, parentId)
        {
        }

        internal static TestPackage Clone(TestPackage package)
        {
            return new TestPackage(package.Id, package.ParentId);
        }
    }
}