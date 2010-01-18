using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;
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
            var ccRepository = CctsRepositoryFactory.CreateCctsRepository(new MappingTestRepository());

            cdtl = ccRepository.GetCdtLibraryByPath((Path) "test"/"bLibrary"/"CDTLibrary");
            cdtText = cdtl.GetCdtByName("Text");
            supLanguage = cdtText.Sups.FirstOrDefault(sup => sup.Name == "Language");
            supLanguageLocale = cdtText.Sups.FirstOrDefault(sup => sup.Name == "LanguageLocale");
            
            ccl = ccRepository.GetCcLibraryByPath((Path) "test"/"bLibrary"/"CCLibrary");
            accAddress = ccl.GetAccByName("Address");
            bccCityName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CityName");
            bccCountryName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CountryName");

            accParty = ccl.GetAccByName("Party");
            bccPartyName = accParty.Bccs.FirstOrDefault(bcc => bcc.Name == "Name");
            asccPartyResidenceAddress = accParty.Asccs.FirstOrDefault(ascc => ascc.Name == "Residence");

            expectedAddress = new SourceElement("Address", "2");
            expectedTown = new SourceElement("Town", "3");
            expectedAddress.AddChild(expectedTown);

            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd"));
        }

        #endregion

        private ICdtLibrary cdtl;
        private ICcLibrary ccl;
        private IAcc accAddress;
        private IBcc bccCityName;
        private IBcc bccCountryName;

        private SourceElement expectedAddress;
        private SourceElement expectedTown;
        private MapForceMapping mapForceMapping;
        private IAcc accParty;
        private IBcc bccPartyName;
        private IAscc asccPartyResidenceAddress;

        private ICdt cdtText;
        private ICdtSup supLanguage;
        private ICdtSup supLanguageLocale;


        [Test]
        public void Test_mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var addressTypeMapping = new ComplexTypeMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new BCCMapping(new SourceElement("CityName", ""), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                            });
            var personTypeMapping = new ComplexTypeMapping("PersonType",
                                                                new List<ElementMapping>
                                                                    {
                                                                        new BCCMapping(new SourceElement("Name", ""), TargetCCElement.ForBcc("Name", bccPartyName)),
                                                                        new ASCCMapping(new SourceElement("HomeAddress", ""), TargetCCElement.ForAscc("ResidenceAddress", asccPartyResidenceAddress), addressTypeMapping),
                                                                    });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                       personTypeMapping,
                                   };

            var expectedSimpleTypeMappings = new List<SimpleTypeMapping>
                                                 {
                                                     new SimpleTypeMapping("String", cdtText),
                                                 };

            var expectedRootElementMapping = new AsmaMapping("Person", personTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }
        

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_and_attributes_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_simple_elements_and_attributes_to_single_acc.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_simple_elements_and_attributes_to_single_acc.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var addressTypeMapping = new ComplexTypeMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new BCCMapping(new SourceElement("CountryName", ""), TargetCCElement.ForBcc("CountryName", bccCountryName)),
                                                                new BCCMapping(new SourceElement("CityName", ""), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                                new BCCMapping(new SourceElement("StreetName", ""), TargetCCElement.ForBcc("StreetName", bccCityName)),
                                                            });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };
            var expectedSimpleTypeMappings = new List<SimpleTypeMapping>
                                                 {
                                                     new SimpleTypeMapping("String", cdtText),
                                                 };
            var expectedRootElementMapping = new AsmaMapping("Address", addressTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
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
                                                         new BCCMapping(new SourceElement("CityName", ""), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                         new BCCMapping(new SourceElement("PersonName", ""), TargetCCElement.ForBcc("Name", bccPartyName)),
                                                     });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };

            var expectedSimpleTypeMappings = new List<SimpleTypeMapping>
                                                 {
                                                     new SimpleTypeMapping("String", cdtText),
                                                 };
            
            var expectedRootElementMapping = new AsmaMapping("Address", addressTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_and_attributes_to_cdt()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_simple_elements_and_attributes_to_cdt.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_simple_elements_and_attributes_to_cdt.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var textTypeMapping = new ComplexTypeMapping("TextType",
                                                            new List<ElementMapping>
                                                            {
                                                                new SupMapping(new SourceElement("LanguageLocale", ""), TargetCCElement.ForSup("LanguageLocaleString", supLanguageLocale)),
                                                                new SupMapping(new SourceElement("Language", ""), TargetCCElement.ForSup("LanguageString", supLanguage)),
                                                            });

            var addressTypeMapping = new ComplexTypeMapping("AddressType",
                                                 new List<ElementMapping>
                                                     {
                                                         new BCCMapping(new SourceElement("CityName", ""), TargetCCElement.ForBcc("CityName", bccCityName)),                                                         
                                                     });


            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       textTypeMapping,
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping("Address", addressTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, new List<SimpleTypeMapping>(), expectedRootElementMapping);
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

        private static void AssertMappings(SchemaMapping mappings, List<IMapping> expectedComplexTypeMappings, List<SimpleTypeMapping> expectedSimpleTypeMappings, AsmaMapping expectedRootElementMapping)
        {
            var complexTypeMappings = new List<ComplexTypeMapping>(mappings.GetComplexTypeMappings());
            Console.Out.WriteLine("=================================================================================");
            Console.Out.WriteLine("Expected Complex Type Mappings:");
            foreach (IMapping expectedMapping in expectedComplexTypeMappings)
            {
                expectedMapping.TraverseDepthFirst(new MappingPrettyPrinter(Console.Out, "  "));
            }
            Console.Out.WriteLine("=================================================================================");
            Console.Out.WriteLine("Actual Complex Type Mappings:");
            foreach (ComplexTypeMapping complexTypeMapping in complexTypeMappings)
            {
                complexTypeMapping.TraverseDepthFirst(new MappingPrettyPrinter(Console.Out, "  "));
            }

            var simpleTypeMappings = new List<SimpleTypeMapping>(mappings.GetSimpleTypeMappings());
            Console.Out.WriteLine("=================================================================================");
            Console.Out.WriteLine("Expected Simple Type Mappings:");
            foreach (IMapping expectedMapping in expectedSimpleTypeMappings)
            {
                expectedMapping.TraverseDepthFirst(new MappingPrettyPrinter(Console.Out, "  "));
            }
            Console.Out.WriteLine("=================================================================================");
            Console.Out.WriteLine("Actual Simple Type Mappings:");
            foreach (var simpleTypeMapping in simpleTypeMappings)
            {
                simpleTypeMapping.TraverseDepthFirst(new MappingPrettyPrinter(Console.Out, "  "));
            }
            Assert.That(complexTypeMappings, Is.EquivalentTo(expectedComplexTypeMappings));
            Assert.That(simpleTypeMappings, Is.EquivalentTo(expectedSimpleTypeMappings));
            Assert.That(mappings.RootElementMapping, Is.EqualTo(expectedRootElementMapping));
        }
    }
}