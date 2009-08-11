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

        private CCRepository ccRepository;
        private readonly TemporaryFileBasedRepository temporaryFileBasedRepository;

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
        private static int[] ASBIEAssociatedElementIds(IABIE abie)
        {
            return (from asbie in abie.ASBIEs select asbie.AssociatedElement.Id).ToArray();
        }

        [Test]
        public void TestSimpleMappingWithOneTargetComponent()
        {
            string mapForceMappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");

            new MappingImporter(mapForceMappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName).ImportMapping(ccRepository);

            var bieLibrary = ccRepository.LibraryByName<IBIELibrary>(BIELibraryName);
            Assert.IsNotNull(bieLibrary, "BIELibrary not generated");
            IABIE bieAddress = bieLibrary.ElementByName("Address");
            Assert.IsNotNull(bieAddress, "ABIE Address not generated");

            var docLibrary = ccRepository.LibraryByName<IDOCLibrary>(DOCLibraryName);
            Assert.IsNotNull(docLibrary, "DOCLibrary not generated");
            IABIE bieInvoice = docLibrary.ElementByName("Invoice");
            Assert.IsNotNull(bieInvoice, "ABIE Invoice not generated");
        }

        [Test]
        public void TestSimpleMappingWithTwoTargetComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping-2-target-components.mfd");

            new MappingImporter(mappingFile, DOCLibraryName, BIELibraryName, BDTLibraryName).ImportMapping(ccRepository);

            var bieLibrary = ccRepository.LibraryByName<IBIELibrary>(BIELibraryName);
            Assert.IsNotNull(bieLibrary, "BIELibrary not generated");

            IABIE bieAddress = bieLibrary.ElementByName("Address");
            Assert.IsNotNull(bieAddress, "ABIE Address not generated");
            Assert.IsNotNull(bieAddress.BasedOn, "BasedOn reference not specified");
            Assert.AreEqual("Address", bieAddress.BasedOn.Name);
            Assert.That(BBIENames(bieAddress), Is.EquivalentTo(new[] {"CityName"}));

            IABIE biePerson = bieLibrary.ElementByName("Person");
            Assert.IsNotNull(biePerson, "ABIE Person not generated");
            Assert.IsNotNull(biePerson.BasedOn, "BasedOn reference not specified");
            Assert.AreEqual("Person", biePerson.BasedOn.Name);
            Assert.That(BBIENames(biePerson), Is.EquivalentTo(new[] {"Name"}));

            var docLibrary = ccRepository.LibraryByName<IDOCLibrary>(DOCLibraryName);
            Assert.That(docLibrary, Is.Not.Null, "DOCLibrary not generated");
            IABIE bieInvoice = docLibrary.ElementByName("Invoice");
            Assert.That(bieInvoice, Is.Not.Null, "ABIE Invoice not generated");
            Assert.That(ASBIEAssociatedElementIds(bieInvoice), Is.EquivalentTo(new[] {bieAddress.Id, biePerson.Id}));
        }
    }
}