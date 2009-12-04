using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.upcc3.import.ebInterface
{
    [TestFixture]
    public class MappingImporterTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            ccRepository = new CCRepository(new MappingTestRepository());
//            temporaryFileBasedRepository = new TemporaryFileBasedRepository(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\Repository-with-CDTs-and-CCs.eap"));
//            ccRepository = new CCRepository(temporaryFileBasedRepository);
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

        #endregion

        private const string DOCLibraryName = "ebInterface Invoice";
        private const string BIELibraryName = "ebInterface";
        private const string BDTLibraryName = "ebInterface Types";
        private const string Qualifier = "ebInterface";
        private const string RootElementName = "Invoice";

        private CCRepository ccRepository;
        private TemporaryFileBasedRepository temporaryFileBasedRepository;

        /// <summary>
        /// Returns an array of the names of all BBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static string[] BBIENames(IAbie abie)
        {
            return (from bbie in abie.Bbies select bbie.Name).ToArray();
        }

        /// <summary>
        /// Returns an array of the associated element IDs of all ASBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static ASBIEDescriptor[] ASBIEDescriptors(IAbie abie)
        {
            return (from asbie in abie.Asbies select new ASBIEDescriptor(asbie.Name, asbie.AssociatedElement.Id)).ToArray();
        }

        private IBieLibrary ShouldContainBieLibrary(string name)
        {
            var library = ccRepository.GetBieLibraryByPath((Path) "test"/"bLibrary"/name);
            Assert.That(library, Is.Not.Null, "BIELibrary '" + name + "' not generated");
            return library;
        }

        private IDocLibrary ShouldContainDocLibrary(string name)
        {
            var library = ccRepository.GetDocLibraryByPath((Path) "test"/"bLibrary"/name);
            Assert.That(library, Is.Not.Null, "DOCLibrary '" + name + "' not generated");
            return library;
        }

        private static IAbie ShouldContainABIE(IBieLibrary bieLibrary, string name, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
        {
            IAbie abie = bieLibrary.GetAbieByName(name);
            VerifyABIE(name, abie, accName, bbieNames, asbieDescriptors);
            return abie;
        }

        private static IMa ShouldContainMa(IDocLibrary docLibrary, string name, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
        {
            var ma = docLibrary.GetMaByName(name);
            VerifyMa(name, ma, accName, bbieNames, asbieDescriptors);
            return ma;
        }

        private static void VerifyABIE(string name, IAbie abie, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
        {
            Assert.IsNotNull(abie, "ABIE '" + name + "' not generated");
            if (accName != null)
            {
                Assert.That(abie.BasedOn, Is.Not.Null, "BasedOn reference not specified");
                Assert.AreEqual(accName, abie.BasedOn.Name, "BasedOn wrong ACC");
            }
            else
            {
                Assert.That(abie.BasedOn, Is.Null, "Unexpected BasedOn reference to ACC '" + abie.BasedOn + "'");
            }
            if (bbieNames == null || bbieNames.Length == 0)
            {
                Assert.That(BBIENames(abie), Is.Empty);
            }
            else
            {
                Assert.That(BBIENames(abie), Is.EquivalentTo(bbieNames));
            }
            if (asbieDescriptors == null || asbieDescriptors.Length == 0)
            {
                Assert.That(ASBIEDescriptors(abie), Is.Empty);
            }
            else
            {
                Assert.That(ASBIEDescriptors(abie), Is.EquivalentTo(asbieDescriptors));
            }
        }

        private static void VerifyMa(string name, IMa ma, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
        {
            throw new NotImplementedException();
        }

        [Test]
        [Ignore]
        public void ShouldImportEBInterfaceDelivery()
        {
            ImportEBInterfaceMappingParts("Delivery");
        }

        [Test]
        [Ignore]
        public void ShouldImportEBInterfaceDeliveryAndPaymentConditions()
        {
            ImportEBInterfaceMappingParts("Delivery", "PaymentConditions");
        }

        [Test]
        [Ignore]
        public void ShouldImportEBInterfaceUniversalBankTransaction()
        {
            ImportEBInterfaceMappingParts("UniversalBankTransaction");
        }

        [Test]
        [Ignore]
        public void ShouldImportEBInterfacePaymentConditions()
        {
            ImportEBInterfaceMappingParts("PaymentConditions");
        }

        private static void ImportEBInterfaceMappingParts(params string[] mappingPartName)
        {
            var repoName = string.Join("_and_", mappingPartName);
            string repoPath = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\" + repoName + ".eap");
            File.Copy(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\Repository-with-CDTs-and-CCs.eap"), repoPath, true);
            var repo = new Repository();
            repo.OpenFile(repoPath);

            var mappingFiles = new List<string>();
            foreach (var part in mappingPartName)
            {
                mappingFiles.Add(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\" + part + ".mfd"));
            }
            new MappingImporter(mappingFiles, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(new CCRepository(repo));
        }

        [Test]
        [Ignore("Adapt to MAs")]
        public void TestNestedInputToFlatOutputMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-input-to-flat-output-mapping.mfd");

            new MappingImporter(new[] {mappingFile}, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BIELibraryName);
            var docLibrary = ShouldContainDocLibrary(DOCLibraryName);

            IAbie bieAddress = ShouldContainABIE(bieLibrary, "Address_Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainABIE(bieLibrary, "Person", "Person", new[] {"Name_Name"}, null);

            IMa maAddress = ShouldContainMa(docLibrary, "Address", null, null, new[]
                                                                               {
                                                                                   new ASBIEDescriptor("Address", bieAddress.Id),
                                                                                   new ASBIEDescriptor("Person", biePerson.Id),
                                                                               });
            IMa maInvoice = ShouldContainMa(docLibrary, "Invoice", null, null, new[]
                                                                               {
                                                                                   new ASBIEDescriptor("Address", maAddress.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", null, null, new[]
                                                                           {
                                                                               new ASBIEDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        [Ignore("Adapt to MAs")]
        public void TestNestedMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-mapping.mfd");

            new MappingImporter(new[] {mappingFile}, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BIELibraryName);
            var docLibrary = ShouldContainDocLibrary(DOCLibraryName);

            IAbie bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainABIE(bieLibrary, "Person", "Party", new[] {"Name_Name"}, new[]
                                                                                                    {
                                                                                                        new ASBIEDescriptor("Address_Residence", bieAddress.Id),
                                                                                                    });

            IMa maInvoice = ShouldContainMa(docLibrary, "Invoice", null, null, new[]
                                                                               {
                                                                                   new ASBIEDescriptor("Person", biePerson.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", null, null, new[]
                                                                           {
                                                                               new ASBIEDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        [Ignore("Adapt to MAs")]
        public void TestOneComplexTypeToMultipleACCsMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\one-complex-type-to-multiple-accs-mapping.mfd");

            new MappingImporter(new[] {mappingFile}, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BIELibraryName);
            var docLibrary = ShouldContainDocLibrary(DOCLibraryName);

            IAbie bieAddress = ShouldContainABIE(bieLibrary, "Address_Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainABIE(bieLibrary, "Address_Person", "Person", new[] {"PersonName_Name"}, null);

            var maAddress = ShouldContainMa(docLibrary, "Address", null, null, new[]
                                                                               {
                                                                                   new ASBIEDescriptor("Address", bieAddress.Id),
                                                                                   new ASBIEDescriptor("Person", biePerson.Id),
                                                                               });
            var maInvoice = ShouldContainMa(docLibrary, "Invoice", null, null, new[]
                                                                               {
                                                                                   new ASBIEDescriptor("Address", maAddress.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", null, null, new[]
                                                                           {
                                                                               new ASBIEDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        [Ignore("Adapt to MAs")]
        public void TestSimpleMappingWithOneTargetComponent()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");

            new MappingImporter(new[] {mappingFile}, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BIELibraryName);
            IAbie bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] {"Town_CityName"}, null);

            var docLibrary = ShouldContainDocLibrary(DOCLibraryName);
            ShouldContainMa(docLibrary, "ebInterface_Invoice", null, null, new[]
                                                                           {
                                                                               new ASBIEDescriptor("Address", bieAddress.Id),
                                                                           });
        }

        [Test]
        [Ignore("Adapt to MAs")]
        public void TestSimpleMappingWithTwoTargetComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping-2-target-components.mfd");

            new MappingImporter(new[] {mappingFile}, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BIELibraryName);
            var docLibrary = ShouldContainDocLibrary(DOCLibraryName);

            IAbie bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainABIE(bieLibrary, "Person", "Person", new[] {"Name_Name"}, null);

            var maInvoice = ShouldContainMa(docLibrary, "Invoice", null, null, new[]
                                                                               {
                                                                                   new ASBIEDescriptor("Address", bieAddress.Id),
                                                                                   new ASBIEDescriptor("Person", biePerson.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", null, null, new[]
                                                                           {
                                                                               new ASBIEDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        [Ignore("Adapt to MAs")]
        public void ShouldMapASingleSimpleElement()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_single_simple_typed_element.mfd");

            new MappingImporter(new[] {mappingFile}, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            ShouldContainBieLibrary(BIELibraryName);
            var docLibrary = ShouldContainDocLibrary(DOCLibraryName);

            ShouldContainMa(docLibrary, "ebInterface_Invoice", "Party", new[] {"PersonName_Name"}, null);
        }
    }

    internal class ASBIEDescriptor : IEquatable<ASBIEDescriptor>
    {
        private readonly int associatedElementId;
        private readonly string name;

        public ASBIEDescriptor(string name, int associatedElementId)
        {
            this.name = name;
            this.associatedElementId = associatedElementId;
        }

        #region IEquatable<ASBIEDescriptor> Members

        public bool Equals(ASBIEDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name) && other.associatedElementId == associatedElementId;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ASBIEDescriptor)) return false;
            return Equals((ASBIEDescriptor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0)*397) ^ associatedElementId;
            }
        }

        public static bool operator ==(ASBIEDescriptor left, ASBIEDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ASBIEDescriptor left, ASBIEDescriptor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, AssociatedElementId: {1}", name, associatedElementId);
        }
    }
}