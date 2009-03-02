using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.ccts.dra
{
    [TestFixture]
    public class DRACCRepositoryTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            eaRepository = new EARepository1();
            repository = new CCRepository(eaRepository);
        }

        #endregion

        private ICCRepository repository;
        private EARepository1 eaRepository;

        private static void AssertSUPs(IDT expectedDT, IDT actualDT)
        {
            Assert.AreEqual(expectedDT.SUPs.Count(), actualDT.SUPs.Count());
            IEnumerator<IDTComponent> bdtSups = actualDT.SUPs.GetEnumerator();
            foreach (IDTComponent cdtSup in expectedDT.SUPs)
            {
                bdtSups.MoveNext();
                IDTComponent bdtSup = bdtSups.Current;
                AssertSUP(cdtSup, actualDT, bdtSup);
            }
        }

        private static void AssertSUP(IDTComponent expectedSUP, IDT expectedDT, IDTComponent actualSUP)
        {
            AssertDTComponent(expectedSUP, expectedDT, expectedSUP.Name, DTComponentType.SUP, actualSUP);
        }

        private static void AssertCON(IDT expectedDT, IDT actualDT)
        {
            AssertDTComponent(expectedDT.CON, actualDT, "Content", DTComponentType.CON, actualDT.CON);
        }

        private static void AssertDTComponent(IDTComponent expected, IDT expectedDT, string expectedName,
                                              DTComponentType expectedComponentType, IDTComponent actual)
        {
            Assert.AreEqual(expectedName, actual.Name);
            Assert.AreEqual(expectedComponentType, actual.ComponentType);
            Assert.AreSame(expectedDT, actual.DT);
            Assert.AreEqual(expected.Type.Id, actual.Type.Id);
            Assert.AreEqual(expected.Definition, actual.Definition);
            Assert.AreEqual(expected.DictionaryEntryName, actual.DictionaryEntryName);
            Assert.AreEqual(expected.LanguageCode, actual.LanguageCode);
            Assert.AreEqual(expected.UniqueIdentifier, actual.UniqueIdentifier);
            Assert.AreEqual(expected.VersionIdentifier, actual.VersionIdentifier);
            Assert.AreEqual(expected.LowerBound, actual.LowerBound);
            Assert.AreEqual(expected.ModificationAllowedIndicator, actual.ModificationAllowedIndicator);
            Assert.AreEqual(expected.UpperBound, actual.UpperBound);
            AssertCollectionsEqual(expected.UsageRules, actual.UsageRules);
            AssertCollectionsEqual(expected.BusinessTerms, actual.BusinessTerms);
        }

        private static void AssertCollectionsEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.AreEqual(expected.Count(), actual.Count(), "number of elements not equal");
            IEnumerator<T> actualEnumerator = actual.GetEnumerator();
            foreach (T e in expected)
            {
                actualEnumerator.MoveNext();
                Assert.AreEqual(e, actualEnumerator.Current);
            }
        }

        [Test]
        public void TestCreateBDT()
        {
            var stringType = (IPRIM) repository.FindByPath((Path) "blib1"/"primlib1"/"String");
            Assert.IsNotNull(stringType, "stringType is null");

            var cdtDate = (ICDT) repository.FindByPath((Path) "blib1"/"cdtlib1"/"Date");
            Assert.IsNotNull(cdtDate, "cdtDate is null");

            IBDTLibrary bdtLibrary = repository.Libraries<IBDTLibrary>().ElementAt(0);

            BDTSpec bdtSpec = BDTSpec.CloneCDT(cdtDate, "My");
            IBDT bdtDate = bdtLibrary.CreateBDT(bdtSpec);

            Assert.IsNotNull(bdtDate, "BDT is null");
            Assert.AreEqual(bdtLibrary.Id, bdtDate.Library.Id);

            Assert.AreEqual("My_" + cdtDate.Name, bdtDate.Name);

            Assert.AreEqual(cdtDate.Definition, bdtDate.Definition);
            Assert.AreEqual(cdtDate.DictionaryEntryName, bdtDate.DictionaryEntryName);
            Assert.AreEqual(cdtDate.LanguageCode, bdtDate.LanguageCode);
            Assert.AreEqual(cdtDate.UniqueIdentifier, bdtDate.UniqueIdentifier);
            Assert.AreEqual(cdtDate.VersionIdentifier, bdtDate.VersionIdentifier);
            AssertCollectionsEqual(cdtDate.BusinessTerms, bdtDate.BusinessTerms);
            AssertCollectionsEqual(cdtDate.UsageRules, bdtDate.UsageRules);

            Assert.IsNotNull(bdtDate.BasedOn, "BasedOn is null");
            Assert.AreEqual(cdtDate.Id, bdtDate.BasedOn.Id);

            AssertCON(cdtDate, bdtDate);

            AssertSUPs(cdtDate, bdtDate);
        }

        [Test]
        public void TestFindCDTs()
        {
            foreach (ICDTLibrary library in repository.Libraries<ICDTLibrary>())
            {
                IEnumerable<IGrouping<IType, ICDT>> cdtByType = from cdt in library.CDTs group cdt by cdt.CON.Type;
                foreach (var cdtGroup in cdtByType)
                {
                    Console.WriteLine(cdtGroup.Key);
                    foreach (ICDT cdt in cdtGroup)
                    {
                        Console.WriteLine("  " + cdt.Name);
                    }
                }
            }
        }

        [Test]
        public void TestReadAccess()
        {
            var libraries = new List<IBusinessLibrary>(repository.AllLibraries());
            Assert.AreEqual(5, libraries.Count);

            IBusinessLibrary bLib1 = libraries[0];
            Assert.AreEqual("blib1", bLib1.Name);
            Assert.AreEqual("http://test/blib1", bLib1.BaseURN);

            var primLib1 = (IPRIMLibrary) libraries[1];
            Assert.AreEqual("primlib1", primLib1.Name);
            Assert.AreEqual("primlib1", primLib1.BaseURN);
            var prims = new List<IPRIM>(primLib1.PRIMs);
            Assert.AreEqual(2, prims.Count);
            IPRIM stringType = prims[0];
            Assert.AreEqual("String", stringType.Name);
            IPRIM decimalType = prims[1];
            Assert.AreEqual("Decimal", decimalType.Name);

            var cdtLib1 = (ICDTLibrary) libraries[2];
            Assert.AreEqual("cdtlib1", cdtLib1.Name);
            Assert.AreEqual("cdtlib1", cdtLib1.BaseURN);
            var cdts = new List<ICDT>(cdtLib1.CDTs);
            Assert.AreEqual(2, cdts.Count);
            ICDT date = cdts[0];
            Assert.AreEqual(stringType.Id, date.CON.Type.Id);
            var dateSups = new List<IDTComponent>(date.SUPs);
            Assert.AreEqual(1, dateSups.Count);
            IDTComponent dateFormat = dateSups[0];
            Assert.AreEqual("Format", dateFormat.Name);
            Assert.AreEqual(stringType.Id, dateFormat.Type.Id);

            // TODO check CDT Measure
        }
    }
}