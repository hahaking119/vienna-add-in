using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDImporter.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
{
    [TestFixture]
    public class MappingsTests
    {
        private ICCLibrary ccl;
        private IACC accAddress;
        private IBCC bccCityName;

        private SourceElement expectedRoot;
        private SourceElement expectedAddress;
        private SourceElement expectedTown;
        private MapForceMapping mapForceMapping;

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            var ccRepository = new CCRepository(new MappingTestRepository());
            ccl = ccRepository.LibraryByName<ICCLibrary>("CCLibrary");
            accAddress = ccl.ElementByName("Address");
            bccCityName = accAddress.BCCs.FirstOrDefault(bcc => bcc.Name == "CityName");

            expectedRoot = new SourceElement("Invoice");
            expectedAddress = new SourceElement("Address");
            expectedRoot.AddChild(expectedAddress);
            expectedTown = new SourceElement("Town");
            expectedAddress.AddChild(expectedTown);

            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd"));
        }

        [Test]
        public void TestCreateSourceElementTree()
        {
            var mappings = new Mappings(mapForceMapping, ccl);
            AssertTreesAreEqual(expectedRoot, mappings.RootSourceElement, "");
        }

        [Test]
        public void TestGetMappings()
        {
            var expectedMappings = new List<Mapping>
                                   {
                                       new Mapping(expectedTown, new TargetCCElement("CityName", bccCityName)),
                                       new Mapping(expectedAddress, new TargetCCElement("Address", accAddress)),
                                   };
            var mappings = new Mappings(mapForceMapping, ccl);
            Assert.That(mappings.MappingList, Is.EquivalentTo(expectedMappings));
        }

        private static void AssertTreesAreEqual(SourceElement expected, SourceElement actual, string path)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Name mismatch at " + path);
            Assert.AreEqual(expected.Children.Count, actual.Children.Count, "Unequal number of children at " + path + "/" + expected.Name);
            for (int i = 0; i < expected.Children.Count; i++)
            {
                AssertTreesAreEqual(expected.Children[i], actual.Children[i], path + "/" + expected.Name);
            }
        }

    }
}