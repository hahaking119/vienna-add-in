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
using VIENNAAddInUnitTests.upcc3.import.ebInterface;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.import.ubl
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
            supLanguage = cdtText.Sups.FirstOrDefault(sup => sup.Name == "Language");
            supLanguageLocale = cdtText.Sups.FirstOrDefault(sup => sup.Name == "LanguageLocale");
            cdtDateTime = cdtl.GetCdtByName("DateTime");
            
            ccl = cctsRepository.GetCcLibraryByPath((Path) "test"/"bLibrary"/"CCLibrary");
            accAddress = ccl.GetAccByName("Address");
            bccCityName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CityName");
            bccCountryName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "CountryName");
            bccBuildingNumber = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "BuildingNumber");
            bccStreetName = accAddress.Bccs.FirstOrDefault(bcc => bcc.Name == "StreetName");

            accDocument = ccl.GetAccByName("Document");
            bccIssue = accDocument.Bccs.FirstOrDefault(bcc => bcc.Name == "Issue");

            accNote = ccl.GetAccByName("Note");
            bccContent = accNote.Bccs.FirstOrDefault(bcc => bcc.Name == "Content");

            accParty = ccl.GetAccByName("Party");
            bccPartyName = accParty.Bccs.FirstOrDefault(bcc => bcc.Name == "Name");
            asccPartyResidenceAddress = accParty.Asccs.FirstOrDefault(ascc => ascc.Name == "Residence");
        }

        #endregion

        private ICdtLibrary cdtl;
        private ICcLibrary ccl;
        private IAcc accAddress;
        private IBcc bccCityName;
        private IBcc bccCountryName;
        private IBcc bccBuildingNumber;
        private IBcc bccStreetName;

        private IAcc accDocument;
        private IBcc bccIssue;

        private IAcc accNote;
        private IBcc bccContent;

        private IAcc accParty;
        private IBcc bccPartyName;
        private IAscc asccPartyResidenceAddress;

        private ICdt cdtText;
        private ICdt cdtDateTime;
        private ICdtSup supLanguage;
        private ICdtSup supLanguageLocale;
        private ICctsRepository cctsRepository;

        [Test]
        public void Test_mapping_complex_type_with_complex_element_to_single_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\SchemaMappingTests\mapping_complex_type_with_complex_element_to_single_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\SchemaMappingTests\mapping_complex_type_with_complex_element_to_single_acc\invoice\maindoc\UBL-Invoice-2.0.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>();


            var issueDateTypeMapping = new ComplexTypeToCdtMapping("IssueDateType", new List<ElementMapping>()){TargetCdt = cdtDateTime};


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

            var expectedRootElementMapping = new AsmaMapping("OrderReference", orderReferenceTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_complex_type_with_complex_element_to_acc_and_bcc_of_other_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\SchemaMappingTests\mapping_complex_type_with_complex_element_to_acc_and_bcc_of_other_acc\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\SchemaMappingTests\mapping_complex_type_with_complex_element_to_acc_and_bcc_of_other_acc\invoice\maindoc\UBL-Invoice-2.0.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>();


            var issueDateTypeMapping = new ComplexTypeToCdtMapping("IssueDateType", new List<ElementMapping>()) { TargetCdt = cdtDateTime };

            var customerReferenceTypeMapping = new ComplexTypeToCdtMapping("CustomerReferenceType",
                                                new List<ElementMapping>
                                                            {
                                                                new AttributeOrSimpleElementToSupMapping(new SourceElement("languageID", ""), supLanguage),
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

            var expectedRootElementMapping = new AsmaMapping("OrderReference", orderReferenceTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
        }

        [Test]
        public void Test_mapping_recursive_complex_type()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\SchemaMappingTests\mapping_recursive_complex_type\mapping.mfd");
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\SchemaMappingTests\mapping_recursive_complex_type\source.xsd");

            SchemaMapping mappings = CreateSchemaMapping(mappingFileName, xsdFileName);

            SimpleTypeToCdtMapping stringMapping = new SimpleTypeToCdtMapping("String", cdtText);
            var expectedSimpleTypeMappings = new List<SimpleTypeToCdtMapping>
                                                 {
                                                     stringMapping,
                                                 };


            var personTypeMapping = new ComplexTypeToAccMapping("PersonType",
                                                                new List<ElementMapping>
                                                                    {
                                                                        new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("FirstName", ""), bccPartyName, stringMapping),
                                                                        new AttributeOrSimpleElementOrComplexElementToBccMapping(new SourceElement("LastName", ""), bccPartyName, stringMapping),
                                                                    });
            //personTypeMapping.C

            var expectedComplexTypeMappings = new List<IMapping>
                                   {
            //                           addressTypeMapping,
            //                           personTypeMapping
                                   };

            var expectedRootElementMapping = new AsmaMapping("Person", personTypeMapping);

            AssertMappings(mappings, expectedComplexTypeMappings, expectedSimpleTypeMappings, expectedRootElementMapping);
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