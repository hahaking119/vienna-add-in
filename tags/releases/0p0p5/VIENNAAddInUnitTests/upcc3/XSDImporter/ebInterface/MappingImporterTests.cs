using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDImporter.ebInterface;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
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

        private CCRepository ccRepository;
        private TemporaryFileBasedRepository temporaryFileBasedRepository;

        /// <summary>
        /// Returns an array of the names of all BBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static string[] BBIENames(IABIE abie)
        {
            return (from bbie in abie.BBIEs select bbie.Name).ToArray();
        }

        /// <summary>
        /// Returns an array of the associated element IDs of all ASBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static ASBIEDescriptor[] ASBIEDescriptors(IABIE abie)
        {
            return (from asbie in abie.ASBIEs select new ASBIEDescriptor(asbie.Name, asbie.AssociatedElement.Id)).ToArray();
        }

        private TLibrary ShouldContainLibrary<TLibrary>(string name) where TLibrary : IBusinessLibrary
        {
            var docLibrary = ccRepository.LibraryByName<TLibrary>(name);
            Assert.That(docLibrary, Is.Not.Null, typeof (TLibrary).Name + " '" + name + "' not generated");
            return docLibrary;
        }

        private static IABIE ShouldContainABIE(IBIELibrary bieLibrary, string name, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
        {
            IABIE abie = bieLibrary.ElementByName(name);
            VerifyABIE(name, abie, accName, bbieNames, asbieDescriptors);
            return abie;
        }

        private static IABIE ShouldContainABIE(IDOCLibrary docLibrary, string name, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
        {
            IABIE abie = docLibrary.ElementByName(name);
            VerifyABIE(name, abie, accName, bbieNames, asbieDescriptors);
            return abie;
        }

        private static void VerifyABIE(string name, IABIE abie, string accName, string[] bbieNames, ASBIEDescriptor[] asbieDescriptors)
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

        [Test]
        public void TestNestedInputToFlatOutputMappingWithDepthTwo()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-input-to-flat-output-mapping-depth-2.mfd");
            new MappingImporter(mappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainLibrary<IBIELibrary>(BIELibraryName);
            IABIE bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] { "CityName" }, null);
            IABIE biePerson = ShouldContainABIE(bieLibrary, "Person", "Person", null, null);
            IABIE bieAccountingVoucher = ShouldContainABIE(bieLibrary, "AccountingVoucher", "AccountingVoucher", null, null);

            var docLibrary = ShouldContainLibrary<IDOCLibrary>(DOCLibraryName);
            IABIE bieEbInterfaceChild2 = ShouldContainABIE(docLibrary, "ebInterface_Child2", null, null, new[]
                                                                                                         {
                                                                                                             new ASBIEDescriptor("self", bieAccountingVoucher.Id), 
                                                                                                             new ASBIEDescriptor("Child3", bieAddress.Id), 
                                                                                                         });
            IABIE bieEbInterfaceChild1 = ShouldContainABIE(docLibrary, "ebInterface_Child1", null, null, new[]
                                                                                                         {
                                                                                                             new ASBIEDescriptor("Child2", bieEbInterfaceChild2.Id), 
                                                                                                             new ASBIEDescriptor("self", biePerson.Id), 
                                                                                                         });
            ShouldContainABIE(docLibrary, "Root", null, null, new[]
                                                              {
                                                                  new ASBIEDescriptor("Child1", bieEbInterfaceChild1.Id), 
                                                              });
        }

        [Test]
        public void TestNestedInputToFlatOutputMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-input-to-flat-output-mapping.mfd");

            new MappingImporter(mappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainLibrary<IBIELibrary>(BIELibraryName);
            IABIE bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] { "CityName" }, null);
            IABIE biePerson = ShouldContainABIE(bieLibrary, "Person", "Person", new[] {"Name"}, null);

            var docLibrary = ShouldContainLibrary<IDOCLibrary>(DOCLibraryName);
            IABIE bieEbInterfaceAddress = ShouldContainABIE(docLibrary, "ebInterface_Address", null, null, new[]
                                                                                                           {
                                                                                                               new ASBIEDescriptor("self", bieAddress.Id), 
                                                                                                               new ASBIEDescriptor("Person", biePerson.Id), 
                                                                                                           });
            ShouldContainABIE(docLibrary, "Invoice", null, null, new[]
                                                                 {
                                                                     new ASBIEDescriptor("Address", bieEbInterfaceAddress.Id), 
                                                                 });
        }

        [Test]
        [Ignore]
        public void TestOneComplexTypeToMultipleACCsMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\one-complex-type-to-multiple-accs-mapping.mfd");

            new MappingImporter(mappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainLibrary<IBIELibrary>(BIELibraryName);
            IABIE bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] { "CityName" }, null);
            IABIE biePerson = ShouldContainABIE(bieLibrary, "Person", "Person", new[] {"Name"}, null);

            var docLibrary = ShouldContainLibrary<IDOCLibrary>(DOCLibraryName);
            IABIE bieEbInterfaceAddress = ShouldContainABIE(docLibrary, "ebInterface_Address", null, null, new[]
                                                                                                           {
                                                                                                               new ASBIEDescriptor("self", bieAddress.Id), 
                                                                                                               new ASBIEDescriptor("Person", biePerson.Id), 
                                                                                                           });
            ShouldContainABIE(docLibrary, "Invoice", null, null, new[]
                                                                 {
                                                                     new ASBIEDescriptor("Address", bieEbInterfaceAddress.Id), 
                                                                 });
        }

        [Test]
        public void TestNestedMapping()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-mapping.mfd");

            new MappingImporter(mappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainLibrary<IBIELibrary>(BIELibraryName);
            IABIE bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] {"CityName"}, null);
            IABIE bieParty = ShouldContainABIE(bieLibrary, "Party", "Party", new[] {"Name"}, new[]
                                                                                             {
                                                                                                 new ASBIEDescriptor("Residence", bieAddress.Id), 
                                                                                             });

            var docLibrary = ShouldContainLibrary<IDOCLibrary>(DOCLibraryName);
            ShouldContainABIE(docLibrary, "Invoice", null, null, new[]
                                                                 {
                                                                     new ASBIEDescriptor("Person", bieParty.Id), 
                                                                 });
        }

        [Test]
        public void TestSimpleMappingWithOneTargetComponent()
        {
            string mapForceMappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");

            new MappingImporter(mapForceMappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainLibrary<IBIELibrary>(BIELibraryName);
            var bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] { "CityName" }, null);

            var docLibrary = ShouldContainLibrary<IDOCLibrary>(DOCLibraryName);
            ShouldContainABIE(docLibrary, "Invoice", null, null, new[]
                                                                 {
                                                                     new ASBIEDescriptor("Address", bieAddress.Id), 
                                                                 });
        }

        [Test]
        public void TestSimpleMappingWithTwoTargetComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping-2-target-components.mfd");

            new MappingImporter(mappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName, Qualifier).ImportMapping(ccRepository);

            var bieLibrary = ShouldContainLibrary<IBIELibrary>(BIELibraryName);
            IABIE bieAddress = ShouldContainABIE(bieLibrary, "Address", "Address", new[] {"CityName"}, null);
            IABIE biePerson = ShouldContainABIE(bieLibrary,"Person", "Person", new[] {"Name"}, null);

            var docLibrary = ShouldContainLibrary<IDOCLibrary>(DOCLibraryName);
            ShouldContainABIE(docLibrary, "Invoice", null, null, new[]
                                                                 {
                                                                     new ASBIEDescriptor("Address", bieAddress.Id), 
                                                                     new ASBIEDescriptor("Person", biePerson.Id), 
                                                                 });
        }
    }

    internal class ASBIEDescriptor : IEquatable<ASBIEDescriptor>
    {
        private readonly string name;
        private readonly int associatedElementId;

        public ASBIEDescriptor(string name, int associatedElementId)
        {
            this.name = name;
            this.associatedElementId = associatedElementId;
        }

        public bool Equals(ASBIEDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name) && other.associatedElementId == associatedElementId;
        }

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