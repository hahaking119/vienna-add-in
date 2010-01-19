using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;
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
            ccRepository = CctsRepositoryFactory.CreateCctsRepository(new MappingTestRepository());
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

        private const string DocLibraryName = "ebInterface Invoice";
        private const string BieLibraryName = "ebInterface";
        private const string BdtLibraryName = "ebInterface Types";
        private const string Qualifier = "ebInterface";
        private const string RootElementName = "Invoice";

        private ICctsRepository ccRepository;
        private TemporaryFileBasedRepository temporaryFileBasedRepository;

        #region Helpers

        /// <summary>
        /// Returns an array of the names of all BBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static string[] BbieNames(IAbie abie)
        {
            return (from bbie in abie.Bbies select bbie.Name).ToArray();
        }

        /// <summary>
        /// Returns an array of the associated element IDs of all ASBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static AsbieDescriptor[] AsbieDescriptors(IAbie abie)
        {
            return (from asbie in abie.Asbies select new AsbieDescriptor(asbie.Name, asbie.AssociatedAbie.Id)).ToArray();
        }

        /// <summary>
        /// Returns an array of the associated element IDs of all ASMAs of the given MA.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static AsmaDescriptor[] AsmaDescriptors(IMa ma)
        {
            return (from asma in ma.Asmas select new AsmaDescriptor(asma.Name, asma.AssociatedBieAggregator.Id)).ToArray();
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

        private static IAbie ShouldContainAbie(IBieLibrary bieLibrary, string name, string accName, string[] bbieNames, AsbieDescriptor[] asbieDescriptors)
        {
            IAbie abie = bieLibrary.GetAbieByName(name);
            VerifyAbie(name, abie, accName, bbieNames, asbieDescriptors);
            return abie;
        }

        private static IMa ShouldContainMa(IDocLibrary docLibrary, string name, AsmaDescriptor[] asmaDescriptors)
        {
            var ma = docLibrary.GetMaByName(name);
            VerifyMa(name, ma, asmaDescriptors);
            return ma;
        }

        private static void VerifyAbie(string name, IAbie abie, string accName, string[] bbieNames, AsbieDescriptor[] asbieDescriptors)
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
                Assert.That(BbieNames(abie), Is.Empty);
            }
            else
            {
                Assert.That(BbieNames(abie), Is.EquivalentTo(bbieNames));
            }
            if (asbieDescriptors == null || asbieDescriptors.Length == 0)
            {
                Assert.That(AsbieDescriptors(abie), Is.Empty);
            }
            else
            {
                Assert.That(AsbieDescriptors(abie), Is.EquivalentTo(asbieDescriptors));
            }
        }

        private static void VerifyMa(string name, IMa ma, AsmaDescriptor[] asmaDescriptors)
        {
            Assert.IsNotNull(ma, "MA '" + name + "' not generated");

            if (asmaDescriptors == null || asmaDescriptors.Length == 0)
            {
                Assert.That(AsmaDescriptors(ma), Is.Empty);
            }
            else
            {
                Assert.That(AsmaDescriptors(ma), Is.EquivalentTo(asmaDescriptors));
            }
        }

        #endregion

        #region Manual Testing

        [Test]
        [Ignore("for manual testing")]
        public void ShouldImportEbInterfaceDelivery()
        {
            ImportEbInterfaceMappingParts("Delivery");
        }

        [Test]
        [Ignore("for manual testing")]
        public void ShouldImportEbInterfaceDeliveryAndPaymentConditions()
        {
            ImportEbInterfaceMappingParts("Delivery", "PaymentConditions");
        }

        [Test]
        [Ignore("for manual testing")]
        public void ShouldImportEbInterfaceUniversalBankTransaction()
        {
            ImportEbInterfaceMappingParts("UniversalBankTransaction");
        }

        [Test]
        [Ignore("for manual testing")]
        public void ShouldImportEbInterfacePaymentConditions()
        {
            ImportEbInterfaceMappingParts("PaymentConditions");
        }

        private static void ImportEbInterfaceMappingParts(params string[] mappingPartName)
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
            string[] schemaFiles = new[] {TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\Invoice.xsd")};
            new MappingImporter(mappingFiles, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(CctsRepositoryFactory.CreateCctsRepository(repo));
        }
        
        #endregion

        [Test]
        public void TestNestedInputToFlatOutputMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-input-to-flat-output-mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\Invoice-for-nested-input-to-flat-output-mapping.xsd") };

            new MappingImporter(new[] {mappingFile}, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IAbie biePerson = ShouldContainAbie(bieLibrary, "PersonType_Person", "Person", new[] { "Name_Name" }, null);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] {"Town_CityName"}, null);
                        
            IMa maAddressType = ShouldContainMa(docLibrary, "AddressType", new[]
                                                                               {
                                                                                    new AsmaDescriptor("Person", biePerson.Id),
                                                                                    new AsmaDescriptor("Address", bieAddress.Id),
                                                                               });

            IMa maInvoiceType = ShouldContainMa(docLibrary, "InvoiceType", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Address", maAddressType.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", new[]
                                                                           {
                                                                               new AsmaDescriptor("Invoice", maInvoiceType.Id),
                                                                           });
        }

        [Test]
        public void TestNestedMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\Invoice.xsd") };


            new MappingImporter(new[] {mappingFile}, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IAbie bieAddress = ShouldContainAbie(bieLibrary, "Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainAbie(bieLibrary, "Person", "Party", new[] {"Name_Name"}, new[]
                                                                                                    {
                                                                                                        new AsbieDescriptor("Address_Residence", bieAddress.Id),
                                                                                                    });

            IMa maInvoice = ShouldContainMa(docLibrary, "Invoice", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Person", biePerson.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", new[]
                                                                           {
                                                                               new AsmaDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        public void TestOneComplexTypeToMultipleACCsMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\one-complex-type-to-multiple-accs-mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\Invoice.xsd") };

            new MappingImporter(new[] {mappingFile}, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IAbie bieAddress = ShouldContainAbie(bieLibrary, "Address_Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainAbie(bieLibrary, "Address_Person", "Person", new[] {"PersonName_Name"}, null);

            var maAddress = ShouldContainMa(docLibrary, "Address", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Address", bieAddress.Id),
                                                                                   new AsmaDescriptor("Person", biePerson.Id),
                                                                               });
            var maInvoice = ShouldContainMa(docLibrary, "Invoice", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Address", maAddress.Id),
                                                                               });
            ShouldContainMa(docLibrary, "ebInterface_Invoice", new[]
                                                                           {
                                                                               new AsmaDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        public void TestSimpleMappingWithOneTargetComponent()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\Invoice.xsd") };

            new MappingImporter(new[] {mappingFile}, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "Address", "Address", new[] {"Town_CityName"}, null);

            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "ebInterface_Invoice", new[]
                                                                           {
                                                                               new AsmaDescriptor("Address", bieAddress.Id),
                                                                           });
        }

        [Test]
        public void TestSimpleMappingWithTwoTargetComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping-2-target-components.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\Invoice.xsd") };

            new MappingImporter(new[] {mappingFile}, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IAbie bieAddress = ShouldContainAbie(bieLibrary, "Address", "Address", new[] {"Town_CityName"}, null);
            IAbie biePerson = ShouldContainAbie(bieLibrary, "Person", "Person", new[] {"Name_Name"}, null);

            var maInvoice = ShouldContainMa(docLibrary, "Invoice", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Address", bieAddress.Id),
                                                                                   new AsmaDescriptor("Person", biePerson.Id),
                                                                               });

            ShouldContainMa(docLibrary, "ebInterface_Invoice", new[]
                                                                           {
                                                                               new AsmaDescriptor("Invoice", maInvoice.Id),
                                                                           });
        }

        [Test]
        public void ShouldMapASingleSimpleElement()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapping_single_simple_typed_element.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\ebInterface\Invoice.xsd") };

            new MappingImporter(new[] {mappingFile}, schemaFiles, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName).ImportMapping(ccRepository);

            ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            //ShouldContainMa(docLibrary, "ebInterface_Invoice", "Party", new[] {"PersonName_Name"}, null);
        }
    }

    internal class AsbieDescriptor : IEquatable<AsbieDescriptor>
    {
        private readonly int associatedElementId;
        private readonly string name;

        public AsbieDescriptor(string name, int associatedElementId)
        {
            this.name = name;
            this.associatedElementId = associatedElementId;
        }

        #region IEquatable<AsbieDescriptor> Members

        public bool Equals(AsbieDescriptor other)
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
            if (obj.GetType() != typeof (AsbieDescriptor)) return false;
            return Equals((AsbieDescriptor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0)*397) ^ associatedElementId;
            }
        }

        public static bool operator ==(AsbieDescriptor left, AsbieDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AsbieDescriptor left, AsbieDescriptor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, AssociatedElementId: {1}", name, associatedElementId);
        }
    }

    internal class AsmaDescriptor : IEquatable<AsmaDescriptor>
    {
        private readonly int associatedElementId;
        private readonly string name;

        public AsmaDescriptor(string name, int associatedElementId)
        {
            this.name = name;
            this.associatedElementId = associatedElementId;
        }

        #region IEquatable<AsmaDescriptor> Members

        public bool Equals(AsmaDescriptor other)
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
            if (obj.GetType() != typeof(AsmaDescriptor)) return false;
            return Equals((AsmaDescriptor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ associatedElementId;
            }
        }

        public static bool operator ==(AsmaDescriptor left, AsmaDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AsmaDescriptor left, AsmaDescriptor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, AssociatedElementId: {1}", name, associatedElementId);
        }
    }
}