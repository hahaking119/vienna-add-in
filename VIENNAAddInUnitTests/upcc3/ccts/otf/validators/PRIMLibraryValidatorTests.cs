using NUnit.Framework;
using UPCCRepositoryInterface;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    [TestFixture]
    public class PRIMLibraryValidatorTests : ElementLibraryValidatorTests
    {
        public PRIMLibraryValidatorTests() : base(Stereotype.PRIMLibrary, Stereotype.PRIM)
        {
        }
    }
}