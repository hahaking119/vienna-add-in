using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.CcLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.import.ebInterface
{
    [TestFixture]
    public class SchemaMappingTests
    {
        #region Setup/Teardown

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            var ccRepository = new CCRepository(new MappingTestRepository());
            ccl = ccRepository.GetCcLibraryByPath((Path) "test"/"bLibrary"/"CCLibrary");
            accAddress = ccl.GetAccByName("Address");
            bccCityName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CityName");

            accParty = ccl.GetAccByName("Party");
            bccPartyName = accParty.Bccs.FirstOrDefault(bcc => bcc.Name == "Name");
            asccPartyResidenceAddress = accParty.Asccs.FirstOrDefault(ascc => ascc.Name == "Residence");

            expectedAddress = new SourceElement("Address", "2");
            expectedTown = new SourceElement("Town", "3");
            expectedAddress.AddChild(expectedTown);

            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd"));
        }

        #endregion

        private ICcLibrary ccl;
        private IAcc accAddress;
        private IBcc bccCityName;

        private SourceElement expectedAddress;
        private SourceElement expectedTown;
        private MapForceMapping mapForceMapping;
        private IAcc accParty;
        private IBcc bccPartyName;
        private IAscc asccPartyResidenceAddress;

        [Test]
        public void Test_mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var addressTypeMapping = new ComplexTypeMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new BCCMapping(new SourceElement("CityName", InputOutputKey.PrependPrefix(mappingFileName, "78432424")), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                            });
            var personTypeMapping = new ComplexTypeMapping("PersonType",
                                                                new List<ElementMapping>
                                                                    {
                                                                        new BCCMapping(new SourceElement("Name", InputOutputKey.PrependPrefix(mappingFileName, "84559328")), TargetCCElement.ForBcc("Name", bccPartyName)),
                                                                        new ASCCMapping(new SourceElement("HomeAddress", InputOutputKey.PrependPrefix(mappingFileName, "84558664")), TargetCCElement.ForAscc("ResidenceAddress", asccPartyResidenceAddress), addressTypeMapping),
                                                                    });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                       personTypeMapping,
                                   };
            var expectedRootElementMapping = new AsmaMapping("Person", personTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_two_simple_elements_to_single_acc()
        {
            string mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_two_simple_elements_to_single_acc.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_two_simple_elements_to_single_acc.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var addressTypeMapping = new ComplexTypeMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new BCCMapping(new SourceElement("CityName", InputOutputKey.PrependPrefix(mappingFileName, "93036432")), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                                new BCCMapping(new SourceElement("StreetName", InputOutputKey.PrependPrefix(mappingFileName, "93041400")), TargetCCElement.ForBcc("StreetName", bccCityName)),
                                                            });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };
            var expectedRootElementMapping = new AsmaMapping("Address", addressTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_two_simple_elements_to_two_accs()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_two_simple_elements_to_two_accs.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_two_simple_elements_to_two_accs.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var addressTypeMapping = new ComplexTypeMapping("AddressType",
                                                 new List<ElementMapping>
                                                     {
                                                         new BCCMapping(new SourceElement("CityName", InputOutputKey.PrependPrefix(mappingFileName, "80085696")), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                         new BCCMapping(new SourceElement("PersonName", InputOutputKey.PrependPrefix(mappingFileName, "80065224")), TargetCCElement.ForBcc("Name", bccPartyName)),
                                                     });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };
            
            var expectedRootElementMapping = new AsmaMapping("Address", addressTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void TestCreateSourceElementTree()
        {
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\Invoice.xsd");            
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            var mappings = new SchemaMapping(mapForceMapping, xmlSchemaSet, ccl);
            AssertTreesAreEqual(expectedAddress, mappings.RootSourceElement, string.Empty);
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

        private SchemaMapping CreateSchemaMapping(string mappingFileName, string xsdFileName)
        {
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFileName);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            return new SchemaMapping(mapForceMapping, xmlSchemaSet, ccl);
        }

        private static void AssertMappings(SchemaMapping mappings, List<IMapping> expectedMappings, AsmaMapping expectedRootElementMapping)
        {
            var complexTypeMappings = new List<ComplexTypeMapping>(mappings.GetComplexTypeMappings());
            Console.Out.WriteLine("=================================================================================");
            Console.Out.WriteLine("Expected Mappings:");
            foreach (IMapping expectedMapping in expectedMappings)
            {
                expectedMapping.TraverseDepthFirst(new MappingPrettyPrinter(Console.Out, "  "));
            }
            Console.Out.WriteLine("=================================================================================");
            Console.Out.WriteLine("Actual Mappings:");
            foreach (ComplexTypeMapping complexTypeMapping in complexTypeMappings)
            {
                complexTypeMapping.TraverseDepthFirst(new MappingPrettyPrinter(Console.Out, "  "));
            }
            Assert.That(complexTypeMappings, Is.EquivalentTo(expectedMappings));
            Assert.That(mappings.RootElementMapping, Is.EqualTo(expectedRootElementMapping));
        }
    }
}