using EA;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    [TestFixture]
    public class TestEARepositoryTest
    {
        [Test]
        public void TestTestEARepository()
        {
            Repository repository = new TestEARepository1();
            Assert.AreEqual(1, repository.Models.Count);

            var model = (Package) repository.Models.GetAt(0);
            Assert.AreEqual(1, model.Packages.Count);

            var bLib1 = AssertLibrary(model, 0, "bLibrary", "blib1", "http://test/blib1", 2, 0);

            var primLib1 = AssertLibrary(bLib1, 0, "PRIMLibrary", "primlib1", "primlib1", 0, 2);
            var stringType = AssertPRIM(primLib1, 0, "String");
            var decimalType = AssertPRIM(primLib1, 1, "Decimal");

            var cdtLib1 = AssertLibrary(bLib1, 1, "CDTLibrary", "cdtlib1", "cdtlib1", 0, 2);
            var date = AssertCDT(cdtLib1, 0, "Date", stringType, 1);
            AssertSUP(date, 1, "Format", stringType, "1", "1");
            var measure = AssertCDT(cdtLib1, 1, "Measure", decimalType, 2);
            AssertSUP(measure, 1, "MeasureUnit", stringType, "1", "1");
            AssertSUP(measure, 2, "MeasureUnit.CodeListVersion", stringType, "1", "*");
        }

        private static Element AssertPRIM(Package library, short index, string name)
        {
            var prim = (Element) library.Elements.GetAt(index);
            Assert.AreEqual("PRIM", prim.Stereotype);
            Assert.AreEqual(name, prim.Name);
            return prim;
        }

        private static Element AssertCDT(Package library, short index, string name, Element contentType, int numberOfSUPs)
        {
            var cdt = (Element)library.Elements.GetAt(index);
            Assert.AreEqual("CDT", cdt.Stereotype);
            Assert.AreEqual(name, cdt.Name);
            Assert.AreEqual(numberOfSUPs + 1, cdt.Attributes.Count);
            AssertCON(cdt, contentType);
            return cdt;
        }

        private static void AssertSUP(Element dataType, short index, string name, Element type, string lowerBound, string upperBound)
        {
            AssertDataTypeComponent(dataType, index, "SUP", name, type, lowerBound, upperBound);
        }

        private static void AssertCON(Element dataType, Element type)
        {
            AssertDataTypeComponent(dataType, 0, "CON", "Content", type, "1", "1");
        }

        private static void AssertDataTypeComponent(Element dataType, short index, string stereotype, string name, Element type, string lowerBound, string upperBound)
        {
            var component = (Attribute) dataType.Attributes.GetAt(index);
            Assert.AreEqual(stereotype, component.Stereotype);
            Assert.AreEqual(name, component.Name);
            Assert.AreEqual(type.ElementID, component.ClassifierID, "wrong classifier ID\n    expected:<{0}>\n     but was:<{1}>", type.Name, component.Type);
            Assert.AreEqual(lowerBound, component.LowerBound);
            Assert.AreEqual(upperBound, component.UpperBound);
        }

        private static Package AssertLibrary(Package parentLibrary, short index, string stereotype, string name, string baseURN, int numberOfSubpackages, int numberOfElements)
        {
            var library = (Package)parentLibrary.Packages.GetAt(index);
            Assert.AreEqual(stereotype, library.Element.Stereotype);
            Assert.AreEqual(name, library.Name);
            Assert.AreEqual(numberOfSubpackages, library.Packages.Count);
            Assert.AreEqual(numberOfElements, library.Elements.Count);
            AssertTaggedValue("baseURN", baseURN, library);
            return library;
        }

        private static void AssertTaggedValue(string tvName, string tvValue, Package library)
        {
            var tv = (TaggedValue)library.Element.TaggedValues.GetByName(tvName);
            Assert.AreEqual(tvValue, tv.Value);
        }
    }
}