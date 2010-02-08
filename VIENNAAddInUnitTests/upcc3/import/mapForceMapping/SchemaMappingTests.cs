using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.import.mapForceMapping
{
    [TestFixture]
    public class SchemaMappingTests
    {
        #region Setup/Teardown

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new MappingTestRepository());

            cdtl = cctsRepository.GetCdtLibraryByPath((Path) "test"/"bLibrary"/"CDTLibrary");
            cdtText = cdtl.GetCdtByName("Text");
            supTextLanguage = cdtText.Sups.FirstOrDefault(sup => sup.Name == "Language");
            supTextLanguageLocale = cdtText.Sups.FirstOrDefault(sup => sup.Name == "LanguageLocale");

            cdtCode = cdtl.GetCdtByName("Code");
            supCodeLanguage = cdtCode.Sups.FirstOrDefault(sup => sup.Name == "Language");

            cdtDateTime = cdtl.GetCdtByName("DateTime");


            ccl = cctsRepository.GetCcLibraryByPath((Path) "test"/"bLibrary"/"CCLibrary");
            accAddress = ccl.GetAccByName("Address");
            bccCityName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CityName");
            bccCountryName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CountryName");
            bccBuildingNumber = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "BuildingNumber");
            bccStreetName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "StreetName");
            bccCountry = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "Country");

            accDocument = ccl.GetAccByName("Document");
            bccIssue = accDocument.Bccs.FirstOrDefault(bcc => bcc.Name == "Issue");

            accNote = ccl.GetAccByName("Note");
            bccContent = accNote.Bccs.FirstOrDefault(bcc => bcc.Name == "Content");



            accParty = ccl.GetAccByName("Party");
            bccPartyName = accParty.Bccs.FirstOrDefault(bcc => bcc.Name == "Name");
            asccPartyResidenceAddress = accParty.Asccs.FirstOrDefault(ascc => ascc.Name == "Residence");

            asccPartyChildren = accParty.Asccs.FirstOrDefault(ascc => ascc.Name == "Children");
        }

        #endregion

        private ICdtLibrary cdtl;
        private ICcLibrary ccl;
        private IAcc accAddress;
        private IAcc accDocument;
        private IAcc accNote;
        private IBcc bccCityName;
        private IBcc bccCountryName;
        private IBcc bccBuildingNumber;
        private IBcc bccStreetName;

        private IAcc accParty;
        private IBcc bccPartyName;
        private IAscc asccPartyResidenceAddress;

        private ICdt cdtText;
        private ICdtSup supTextLanguage;
        private ICdtSup supTextLanguageLocale;
        private ICctsRepository cctsRepository;
        private IBcc bccIssue;

        private IBcc bccContent;

        private IAscc asccPartyChildren;

        private ICdt cdtDateTime;
        private ICdt cdtCode;
        private ICdtSup supCodeLanguage;
        private IBcc bccCountry;


        [Test]
        public void Test_mapping_complex_type_with_one_simple_element_and_one_complex_element_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_single_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_single_acc\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);

            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };

            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, stringMapping),
                                                            });
            var personTypeMapping = new ComplexTypeToAccMapping("PersonType",
                                                                new List<ElementMapping>
                                                                    {
                                                                        new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("Name", ""), bccPartyName, stringMapping),
                                                                        new ComplexElementToAsccMapping(new SourceElement("HomeAddress", ""), asccPartyResidenceAddress){TargetMapping = addressTypeMapping},
                                                                    });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                       personTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Person", "")) { TargetMapping = personTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }
       
        [Test]
        public void Test_mapping_complex_type_with_simple_elements_and_attributes_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_and_attributes_to_single_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_and_attributes_to_single_acc\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CountryName", ""), bccCountryName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("StreetName", ""), bccCityName, stringMapping),
                                                            });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_choice_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_choice_to_single_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_choice_to_single_acc\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("StreetName", ""), bccCityName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CountryName", ""), bccCountryName, stringMapping),
                                                            });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_all_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_all_to_single_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_all_to_single_acc\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("StreetName", ""), bccCityName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CountryName", ""), bccCountryName, stringMapping),
                                                            });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_with_semisemantic_loss()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_with_semisemantic_loss\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_with_semisemantic_loss\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("StreetName", ""), bccCityName, stringMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("Town", ""), bccCityName, stringMapping),                                                                
                                                            });

            
            var personTypeMapping = new ComplexTypeToAccMapping("PersonType",
                                                                new List<ElementMapping>
                                                                    {
                                                                        new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("FirstName", ""), bccPartyName, stringMapping),
                                                                        new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("LastName", ""), bccPartyName, stringMapping),
                                                                        new ComplexElementToAsccMapping(new SourceElement("HomeAddress", ""), asccPartyResidenceAddress){TargetMapping = addressTypeMapping},
                                                                        new ComplexElementToAsccMapping(new SourceElement("WorkAddress", ""), asccPartyResidenceAddress){TargetMapping = addressTypeMapping},
                                                                    });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                       personTypeMapping
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Person", "")) { TargetMapping = personTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_two_simple_elements_to_two_accs()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_two_simple_elements_to_two_accs\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_two_simple_elements_to_two_accs\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };

            var addressTypeMapping = new ComplexTypeToMaMapping("AddressType",
                                                 new List<ElementMapping>
                                                     {
                                                         new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, stringMapping),
                                                         new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("PersonName", ""), bccPartyName, stringMapping),
                                                     });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_and_attributes_to_cdt()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_and_attributes_to_cdt\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_simple_elements_and_attributes_to_cdt\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var textTypeMapping = new ComplexTypeToCdtMapping("TextType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementToSupMapping(new SourceElement("LanguageLocale", ""), supTextLanguageLocale),
                                                                new AttributeOrSimpleElementToSupMapping(new SourceElement("Language", ""), supTextLanguage),
                                                            });

            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                 new List<ElementMapping>
                                                     {
                                                         new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, textTypeMapping),                                                         
                                                     });


            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       textTypeMapping,
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, new List<SimpleTypeToCdtMapping>(), expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_simple_element_and_attributes_to_acc_with_mapping_function_split()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_simple_element_and_attributes_to_acc_with_mapping_function_split\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_simple_element_and_attributes_to_acc_with_mapping_function_split\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);

            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                            new List<ElementMapping>
                                                            {
                                                                new SplitMapping(new SourceElement("Street", ""), new[] {bccStreetName, bccBuildingNumber}, new [] {stringMapping, stringMapping}),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("Town", ""), bccCityName, stringMapping),
                                                            });
            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);            
        }

        [Test]
        public void Test_mapping_complex_type_with_complex_element_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_complex_element_to_single_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_complex_element_to_single_acc\invoice\maindoc\UBL-Invoice-2.0.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>();


            var issueDateTypeMapping = new ComplexTypeToCdtMapping("IssueDateType", new List<ElementMapping>()) { TargetCdt = cdtDateTime };


            var orderReferenceTypeMapping = new ComplexTypeToAccMapping("OrderReferenceType",
                                                new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("IssueDate", ""), bccIssue, issueDateTypeMapping),
                                                            });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       issueDateTypeMapping,
                                       orderReferenceTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("OrderReference", "")) { TargetMapping = orderReferenceTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_complex_element_to_acc_and_bcc_of_other_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_complex_element_to_acc_and_bcc_of_other_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_complex_type_with_complex_element_to_acc_and_bcc_of_other_acc\invoice\maindoc\UBL-Invoice-2.0.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>();


            var issueDateTypeMapping = new ComplexTypeToCdtMapping("IssueDateType", new List<ElementMapping>()) { TargetCdt = cdtDateTime };

            var customerReferenceTypeMapping = new ComplexTypeToCdtMapping("CustomerReferenceType",
                                                new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementToSupMapping(new SourceElement("languageID", ""), supTextLanguage),
                                                            });

            var orderReferenceTypeMapping = new ComplexTypeToMaMapping("OrderReferenceType",
                                                new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("IssueDate", ""), bccIssue, issueDateTypeMapping),
                                                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CustomerReference", ""), bccContent, customerReferenceTypeMapping),
                                                            });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       issueDateTypeMapping,
                                       customerReferenceTypeMapping,
                                       orderReferenceTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("OrderReference", "")) { TargetMapping = orderReferenceTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_recursive_complex_type()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_recursive_complex_type\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_recursive_complex_type\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            List<ElementMapping> personTypeChildMappings = new List<ElementMapping>
                            {
                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("FirstName", ""), bccPartyName, stringMapping),
                                new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("LastName", ""), bccPartyName, stringMapping),
                            };
            var personTypeMapping = new ComplexTypeToAccMapping("PersonType",
                                                                personTypeChildMappings);
            personTypeMapping.AddChildMapping(new ComplexElementToAsccMapping(new SourceElement("Children", ""), asccPartyChildren) { TargetMapping = personTypeMapping });

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       personTypeMapping
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Person", "")) { TargetMapping = personTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_one_complex_type_to_two_cdts()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_one_complex_type_to_two_cdts\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\MapForceMapping\SchemaMappingTests\mapping_one_complex_type_to_two_cdts\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);


            var textTypeToTextMapping = new ComplexTypeToCdtMapping("TextType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementToSupMapping(new SourceElement("Language", ""), supTextLanguage),
                                                            });

            var textTypeToCodeMapping = new ComplexTypeToCdtMapping("TextType",
                                                            new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementToSupMapping(new SourceElement("Language", ""), supCodeLanguage),
                                                            });

            var addressTypeMapping = new ComplexTypeToAccMapping("AddressType",
                                                 new List<ElementMapping>
                                                     {
                                                         new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CityName", ""), bccCityName, textTypeToTextMapping),                                                         
                                                         new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("CountryName", ""), bccCountry, textTypeToCodeMapping),                                                         
                                                     });


            var expectedComplexTypeMappings = new List<IMapping>
                                   {
                                       textTypeToTextMapping,
                                       textTypeToCodeMapping,
                                       addressTypeMapping,
                                   };

            var expectedRootElementMapping = new AsmaMapping(new SourceElement("Address", "")) { TargetMapping = addressTypeMapping };

            AssertMappings(mappings, expectedComplexTypeMappings, new List<SimpleTypeToCdtMapping>(), expectedRootElementMapping);


        }


        private SchemaMapping CreateSchemaMapping(string mappingFileName, string xsdFileName)
        {
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFileName);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            return new SchemaMapping(mapForceMapping, xmlSchemaSet, ccl, cctsRepository);
        }

        private static void AssertMappings(SchemaMapping mappings, List<IMapping> expectedComplexTypeMappings, List<SimpleTypeToCdtMapping> expectedSimpleTypeMappings, AsmaMapping expectedRootElementMapping)
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

            var simpleTypeMappings = new List<SimpleTypeToCdtMapping>(mappings.GetSimpleTypeMappings());
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
            Assert.That(((AsmaMapping) mappings.RootElementMapping).TargetMapping, Is.EqualTo(expectedRootElementMapping.TargetMapping));
            Assert.That(mappings.RootElementMapping, Is.EqualTo(expectedRootElementMapping));
        }
    }
}