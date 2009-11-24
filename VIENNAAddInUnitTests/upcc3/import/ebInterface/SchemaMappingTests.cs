using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.CcLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.ebInterface;

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
            ccl = ccRepository.LibraryByName<ICCLibrary>("CCLibrary");
            accAddress = ccl.ElementByName("Address");
            bccCityName = accAddress.BCCs.FirstOrDefault(bcc => bcc.Name == "CityName");

            accParty = ccl.ElementByName("Party");
            bccPartyName = accParty.BCCs.FirstOrDefault(bcc => bcc.Name == "Name");
            asccPartyResidenceAddress = accParty.ASCCs.FirstOrDefault(ascc => ascc.Name == "Residence");

            expectedAddress = new SourceElement("Address", "2");
            expectedTown = new SourceElement("Town", "3");
            expectedAddress.AddChild(expectedTown);

            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd"));
        }

        #endregion

        private ICCLibrary ccl;
        private IACC accAddress;
        private IBCC bccCityName;

        private SourceElement expectedAddress;
        private SourceElement expectedTown;
        private MapForceMapping mapForceMapping;
        private IACC accParty;
        private IBCC bccPartyName;
        private IASCC asccPartyResidenceAddress;

        private static void AssertTreesAreEqual(SourceElement expected, SourceElement actual, string path)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Name mismatch at " + path);
            Assert.AreEqual(expected.Children.Count, actual.Children.Count, "Unequal number of children at " + path + "/" + expected.Name);
            for (int i = 0; i < expected.Children.Count; i++)
            {
                AssertTreesAreEqual(expected.Children[i], actual.Children[i], path + "/" + expected.Name);
            }
        }

        [Test]
        public void Test_mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_one_simple_element_and_one_complex_element_to_one_acc.mfd");
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFileName);
            var mappings = new SchemaMapping(mapForceMapping, ccl);
            var complexTypeMappings = new List<ComplexTypeMapping>(mappings.GetComplexTypeMappings());
            var homeAddressMapping = new ComplexTypeMapping("HomeAddress",
                                                            new List<ElementMapping>
                                                            {
                                                                new BCCMapping(new SourceElement("CityName", InputOutputKey.PrependPrefix(mappingFileName, "78432424")), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                            });
            var expectedMappings = new List<IMapping>
                                   {
                                       homeAddressMapping,
                                       new ComplexTypeMapping("Person",
                                                              new List<ElementMapping>
                                                              {
                                                                  new BCCMapping(new SourceElement("Name", InputOutputKey.PrependPrefix(mappingFileName, "84559328")), TargetCCElement.ForBcc("Name", bccPartyName)),
                                                                  new ASCCMapping(new SourceElement("HomeAddress", InputOutputKey.PrependPrefix(mappingFileName, "84558664")), TargetCCElement.ForAscc("ResidenceAddress", asccPartyResidenceAddress), homeAddressMapping),
                                                              }),
                                   };
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
        }

        [Test]
        public void Test_mapping_complex_type_with_two_simple_elements_to_single_acc()
        {
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_two_simple_elements_to_single_acc.mfd"));
            var mappings = new SchemaMapping(mapForceMapping, ccl);
            var complexTypeMappings = new List<ComplexTypeMapping>(mappings.GetComplexTypeMappings());
            Assert.That(complexTypeMappings.Count, Is.EqualTo(1));
            Assert.That(complexTypeMappings[0], Is.TypeOf(typeof (ComplexTypeMapping)));
            ComplexTypeMapping accMapping = complexTypeMappings[0];
            Assert.That(accMapping.BCCMappings(accMapping.TargetACCs.ElementAt(0)).Count(), Is.EqualTo(2));
        }

        [Test]
        public void Test_mapping_complex_type_with_two_simple_elements_to_two_accs()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_complex_type_with_two_simple_elements_to_two_accs.mfd");
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFileName);
            var mappings = new SchemaMapping(mapForceMapping, ccl);
            var complexTypeMappings = new List<ComplexTypeMapping>(mappings.GetComplexTypeMappings());
            var expectedMappings = new List<IMapping>
                                   {
                                       new ComplexTypeMapping("Address",
                                                              new List<ElementMapping>
                                                              {
                                                                  new BCCMapping(new SourceElement("CityName", InputOutputKey.PrependPrefix(mappingFileName, "80085696")), TargetCCElement.ForBcc("CityName", bccCityName)),
                                                                  new BCCMapping(new SourceElement("PersonName", InputOutputKey.PrependPrefix(mappingFileName, "80065224")), TargetCCElement.ForBcc("Name", bccPartyName)),
                                                              }),
                                   };
            Assert.That(complexTypeMappings, Is.EquivalentTo(expectedMappings));
        }

        [Test]
        public void TestCreateSourceElementTree()
        {
            var mappings = new SchemaMapping(mapForceMapping, ccl);
            AssertTreesAreEqual(expectedAddress, mappings.RootSourceElement, string.Empty);
        }
    }
}