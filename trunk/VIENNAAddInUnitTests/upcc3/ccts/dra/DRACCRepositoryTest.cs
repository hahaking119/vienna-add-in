using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;
using Path=VIENNAAddIn.upcc3.ccts.Path;

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
//            eaRepository = GetFileBasedEARepository();
            repository = new CCRepository(eaRepository);
        }
        private static Repository GetFileBasedEARepository()
        {
            var repo = new Repository();
            string repositoryFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDGeneratorTest.eap";
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            repo.OpenFile(repositoryFile);
            return repo;
        }

        #endregion

        private ICCRepository repository;
        private Repository eaRepository;

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
            Console.WriteLine(expected.UsageRules.JoinToString("---"));
            Console.WriteLine(actual.UsageRules.JoinToString("---"));
            Assert.AreEqual(expected.UsageRules, actual.UsageRules);
            Assert.AreEqual(expected.BusinessTerms, actual.BusinessTerms);
        }

        private static IEnumerable<int> GenerateNumbers(int limit)
        {
            for (int i = 0; i < limit; ++i)
            {
                yield return i;
            }
        }

        [Test]
        public void TestCreateBDT()
        {
            var stringType = (IPRIM) repository.FindByPath((Path) "blib1"/"primlib1"/"String");
            Assert.IsNotNull(stringType, "stringType is null");

            var cdtDate = (ICDT) repository.FindByPath((Path) "blib1"/"cdtlib1"/"Date");
            Assert.IsNotNull(cdtDate, "cdtDate is null");

            IBDTLibrary bdtLibrary = repository.Libraries<IBDTLibrary>().First();

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
            Assert.AreEqual(cdtDate.BusinessTerms, bdtDate.BusinessTerms);
            Assert.AreEqual(cdtDate.UsageRules, bdtDate.UsageRules);

            Assert.IsNotNull(bdtDate.BasedOn, "BasedOn is null");
            Assert.AreEqual(cdtDate.Id, bdtDate.BasedOn.Id);

            AssertCON(cdtDate, bdtDate);

            AssertSUPs(cdtDate, bdtDate);
        }

        [Test]
        public void TestCreateBDTFileBased()
        {
            repository = new CCRepository(GetFileBasedEARepository());
            var cdtDate = (ICDT) repository.FindByPath((Path) "ebInterface Data Model"/"CDTLibrary"/"Date");
            Assert.IsNotNull(cdtDate, "cdtDate is null");

            IBDTLibrary bdtLibrary = repository.Libraries<IBDTLibrary>().First();

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
            Assert.AreEqual(cdtDate.BusinessTerms, bdtDate.BusinessTerms);
            Assert.AreEqual(cdtDate.UsageRules, bdtDate.UsageRules);

            Assert.IsNotNull(bdtDate.BasedOn, "BasedOn is null");
            Assert.AreEqual(cdtDate.Id, bdtDate.BasedOn.Id);

            AssertCON(cdtDate, bdtDate);

            AssertSUPs(cdtDate, bdtDate);
        }

        [Test]
        public void TestCreateLibrary()
        {
            IBLibrary bLib = repository.Libraries<IBLibrary>().First();
            Assert.IsNotNull(bLib, "bLib not found");
            var spec = new LibrarySpec
                       {
                           Name = "MyBDTLibrary",
                           BaseURN = "my/base/urn",
                           BusinessTerms = new[] {"business term 1", "business term 2"},
                           Copyrights = new[] {"copyright 1", "copyright 2"},
                           NamespacePrefix = "my_namespace_prefix",
                           Owners = new[] {"owner 1", "owner 2", "owner 3"},
                           References = new[] {"reference 1"},
                           Status = "my status",
                           UniqueIdentifier = "a unique ID",
                           VersionIdentifier = "a specific version",
                       };
            IBDTLibrary bdtLib = bLib.CreateBDTLibrary(spec);
            Assert.AreEqual(bLib.Id, bdtLib.Parent.Id);
            Assert.AreEqual(spec.Name, bdtLib.Name);
            Assert.AreEqual(spec.BaseURN, bdtLib.BaseURN);
            Assert.AreEqual(spec.BusinessTerms, bdtLib.BusinessTerms);
            Assert.AreEqual(spec.Copyrights, bdtLib.Copyrights);
            Assert.AreEqual(spec.NamespacePrefix, bdtLib.NamespacePrefix);
            Assert.AreEqual(spec.Owners, bdtLib.Owners);
            Assert.AreEqual(spec.References, bdtLib.References);
            Assert.AreEqual(spec.Status, bdtLib.Status);
            Assert.AreEqual(spec.UniqueIdentifier, bdtLib.UniqueIdentifier);
            Assert.AreEqual(spec.VersionIdentifier, bdtLib.VersionIdentifier);
        }

        [Test]
        public void TestCreateLibraryFileBased()
        {
            repository = new CCRepository(GetFileBasedEARepository());
            IBLibrary bLib = repository.Libraries<IBLibrary>().First();
            Assert.IsNotNull(bLib, "bLib not found");
            var spec = new LibrarySpec
                       {
                           Name = "MyBDTLibrary",
                           BaseURN = "my/base/urn",
                           BusinessTerms = new[] {"business term 1", "business term 2"},
                           Copyrights = new[] {"copyright 1", "copyright 2"},
                           NamespacePrefix = "my_namespace_prefix",
                           Owners = new[] {"owner 1", "owner 2", "owner 3"},
                           References = new[] {"reference 1"},
                           Status = "my status",
                           UniqueIdentifier = "a unique ID",
                           VersionIdentifier = "a specific version",
                       };
            IBDTLibrary bdtLib = bLib.CreateBDTLibrary(spec);
            Assert.AreEqual(bLib.Id, bdtLib.Parent.Id);
            Assert.AreEqual(spec.Name, bdtLib.Name);
            Assert.AreEqual(spec.BaseURN, bdtLib.BaseURN);
            Assert.AreEqual(spec.BusinessTerms, bdtLib.BusinessTerms);
            Assert.AreEqual(spec.Copyrights, bdtLib.Copyrights);
            Assert.AreEqual(spec.NamespacePrefix, bdtLib.NamespacePrefix);
            Assert.AreEqual(spec.Owners, bdtLib.Owners);
            Assert.AreEqual(spec.References, bdtLib.References);
            Assert.AreEqual(spec.Status, bdtLib.Status);
            Assert.AreEqual(spec.UniqueIdentifier, bdtLib.UniqueIdentifier);
            Assert.AreEqual(spec.VersionIdentifier, bdtLib.VersionIdentifier);
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

        [Test]
        public void TestStringConcat()
        {
            Assert.AreEqual("0123456789", GenerateNumbers(10).ConcatToString());
            Assert.AreSame(String.Empty, ((IEnumerable<int>) null).ConcatToString());
        }
    }
}