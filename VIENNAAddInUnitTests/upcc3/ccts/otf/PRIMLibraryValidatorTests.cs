using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class PRIMLibraryValidatorTests : ElementLibraryValidatorTests
    {
        public PRIMLibraryValidatorTests() : base(Stereotype.PRIMLibrary, Stereotype.PRIM)
        {
        }
    }
}