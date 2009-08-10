using System;
using System.Collections.Generic;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDImporter.ebInterface;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
{
    [TestFixture]
    public class MappingTest
    {
        private ICCLibrary ccl;
        private IACC accAddress;
        private IBCC bccCityName;
        private CCRepository ccRepository;
        private TemporaryFileBasedRepository temporaryFileBasedRepository;

        [Test]
        public void TestCreateSourceElementTree()
        {
            var expectedRoot = new SourceElement("Invoice");
            var address = new SourceElement("Address");
            expectedRoot.AddChild(address);
            address.AddChild(new SourceElement("Town"));

            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mappingFile);

            var mappingAdapter = new MappingAdapter(mapForceMapping, ccRepository);
            mappingAdapter.GenerateMapping();
            AssertTreesAreEqual(expectedRoot, mappingAdapter.RootSourceElement, "");
        }

        [SetUp]
        public void Context()
        {
            UseTestRepository();
//            UseFileBasedRepository();
        }

// ReSharper disable UnusedMember.Local
        private void UseTestRepository()
// ReSharper restore UnusedMember.Local
        {
            var eaRepository = new EARepository();
            Element cdtText = null;
            Element primString = null;
            eaRepository.AddModel(
                "test", m => m.AddPackage(
                                 "bLibrary", bLibrary =>
                                             {
                                                 bLibrary.Element.Stereotype = Stereotype.BLibrary;
                                                 bLibrary.AddDiagram("bLibrary", "Class");
                                                 bLibrary.AddPackage("PRIMLibrary", package =>
                                                                                    {
                                                                                        package.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                        primString = package.AddPRIM("String");
                                                                                    });
                                                 bLibrary.AddPackage(
                                                     "CDTLibrary", package =>
                                                                   {
                                                                       package.Element.Stereotype = Stereotype.CDTLibrary;
                                                                       cdtText = package.AddCDT("Text").With(e =>
                                                                                                             {
                                                                                                                 e.Stereotype = Stereotype.CDT;
                                                                                                                 e.AddCON(primString);
                                                                                                                 e.AddSUPs(primString, "Language", "Language.Locale");
                                                                                                             });
                                                                   });
                                                 bLibrary.AddPackage(
                                                     "CCL", package =>
                                                            {
                                                                package.Element.Stereotype = Stereotype.CCLibrary;
                                                                package.AddClass("Foo").With(e => e.Stereotype = Stereotype.ACC);
                                                                package.AddClass("Address")
                                                                    .With(e => e.Stereotype = Stereotype.ACC)
                                                                    .With(e => e.AddBCCs(cdtText, "StreetName", "CityName"));
                                                                package.AddClass("Person")
                                                                    .With(e => e.Stereotype = Stereotype.ACC)
                                                                    .With(e => e.AddBCCs(cdtText, "Name"));
                                                            });
                                             }));
            ccRepository = new CCRepository(eaRepository);
            ccl = ccRepository.LibraryByName<ICCLibrary>("CCL");
            accAddress = ccl.ElementByName("Address");
            bccCityName = accAddress.BCCs.FirstOrDefault(bcc => bcc.Name == "CityName");
        }

        private void UseFileBasedRepository()
        {
            temporaryFileBasedRepository = new TemporaryFileBasedRepository(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\Repository-with-CDTs-and-CCs.eap"));
            ccRepository = new CCRepository(temporaryFileBasedRepository);
            ccl = ccRepository.LibraryByName<ICCLibrary>("CCLibrary");
            accAddress = ccl.ElementByName("Address");
            bccCityName = accAddress.BCCs.FirstOrDefault(bcc => bcc.Name == "CityName");
        }

        [TearDown]
        public void Teardown()
        {
            if (temporaryFileBasedRepository != null)
            {
//                temporaryFileBasedRepository.Dispose();
                temporaryFileBasedRepository.CloseButKeepFile();
            }
        }

        [Test]
        public void TestGetMappings()
        {
            var expectedRoot = new SourceElement("Invoice");
            var address = new SourceElement("Address");
            expectedRoot.AddChild(address);
            var town = new SourceElement("Town");
            address.AddChild(town);
            var expectedMappings = new List<Mapping>
                                   {
                                       new Mapping(town, new TargetCCElement("CityName", bccCityName)),
                                       new Mapping(address, new TargetCCElement("Address", accAddress)),
                                   };
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");
            MapForceMapping mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mappingFile);
            var mappingAdapter = new MappingAdapter(mapForceMapping, ccRepository);
            mappingAdapter.GenerateMapping();
            List<Mapping> mappings = mappingAdapter.Mappings;
            Assert.AreEqual(expectedMappings.Count, mappings.Count);
            foreach (var mapping in mappings)
            {
                Console.WriteLine(mapping);
            }
            for (int i = 0; i < expectedMappings.Count; i++)
            {
                var expected = expectedMappings[i];
                var actual = mappings[i];
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void TestCreateBIELibrary()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");
            MapForceMapping mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mappingFile);
            var mappingAdapter = new MappingAdapter(mapForceMapping, ccRepository);
            mappingAdapter.GenerateMapping();

            const string docLibraryName = "ebInterface Invoice";
            const string bieLibraryName = "ebInterface";
            const string bdtLibraryName = "ebInterface Types";

            mappingAdapter.GenerateBIELibrary(bieLibraryName, bdtLibraryName);
            var bieLibrary = ccRepository.LibraryByName<IBIELibrary>(bieLibraryName);
            Assert.IsNotNull(bieLibrary, "BIELibrary not generated");
            var bieAddress = bieLibrary.ElementByName("Address");
            Assert.IsNotNull(bieAddress, "ABIE Address not generated");

            mappingAdapter.GenerateDOCLibrary(docLibraryName);
            var docLibrary = ccRepository.LibraryByName<IDOCLibrary>(docLibraryName);
            Assert.IsNotNull(docLibrary, "DOCLibrary not generated");
            var bieInvoice = docLibrary.ElementByName("Invoice");
            Assert.IsNotNull(bieInvoice, "ABIE Invoice not generated");
        }

        [Test]
        public void TestMappingWithTwoTargetComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping-2-target-components.mfd");
            MapForceMapping mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mappingFile);
            var mappingAdapter = new MappingAdapter(mapForceMapping, ccRepository);
            mappingAdapter.GenerateMapping();

            const string docLibraryName = "ebInterface Invoice";
            const string bieLibraryName = "ebInterface";
            const string bdtLibraryName = "ebInterface Types";

            mappingAdapter.GenerateBIELibrary(bieLibraryName, bdtLibraryName);
            var bieLibrary = ccRepository.LibraryByName<IBIELibrary>(bieLibraryName);
            Assert.IsNotNull(bieLibrary, "BIELibrary not generated");

            var bieAddress = bieLibrary.ElementByName("Address");
            Assert.IsNotNull(bieAddress, "ABIE Address not generated");
            Assert.IsNotNull(bieAddress.BasedOn, "BasedOn reference not specified");
            Assert.AreEqual("Address", bieAddress.BasedOn.Name);
            Assert.AreEqual(1, bieAddress.BBIEs.Count());
            Assert.AreEqual("CityName", bieAddress.BBIEs.ElementAt(0).Name);

            var biePerson = bieLibrary.ElementByName("Person");
            Assert.IsNotNull(biePerson, "ABIE Person not generated");
            Assert.IsNotNull(biePerson.BasedOn, "BasedOn reference not specified");
            Assert.AreEqual("Person", biePerson.BasedOn.Name);
            Assert.AreEqual(1, biePerson.BBIEs.Count());
            Assert.AreEqual("Name", biePerson.BBIEs.ElementAt(0).Name);

            mappingAdapter.GenerateDOCLibrary(docLibraryName);
            var docLibrary = ccRepository.LibraryByName<IDOCLibrary>(docLibraryName);
            Assert.IsNotNull(docLibrary, "DOCLibrary not generated");
            var bieInvoice = docLibrary.ElementByName("Invoice");
            Assert.IsNotNull(bieInvoice, "ABIE Invoice not generated");
            Assert.AreEqual(2, bieInvoice.ASBIEs.Count());
            Assert.AreEqual(bieAddress.Id, bieInvoice.ASBIEs.ElementAt(0).AssociatedElement.Id);
            Assert.AreEqual(biePerson.Id, bieInvoice.ASBIEs.ElementAt(1).AssociatedElement.Id);
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