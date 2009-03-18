// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
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
            repository = new CCRepository(eaRepository);
        }

        #endregion

        private static Repository GetFileBasedEARepository()
        {
            var repo = new Repository();
            string repositoryFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDGeneratorTest.eap";
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            repo.OpenFile(repositoryFile);
            return repo;
        }

        private ICCRepository repository;
        private Repository eaRepository;

        private static void AssertSUPs(IDT expectedDT, IDT actualDT)
        {
            Assert.AreEqual(expectedDT.SUPs.Count(), actualDT.SUPs.Count());
            IEnumerator<ISUP> bdtSups = actualDT.SUPs.GetEnumerator();
            foreach (ISUP cdtSup in expectedDT.SUPs)
            {
                bdtSups.MoveNext();
                ISUP bdtSup = bdtSups.Current;
                AssertSUP(cdtSup, actualDT, bdtSup);
            }
        }

        private static void AssertSUP(ISUP expectedSUP, IDT expectedDT, ISUP actualSUP)
        {
            AssertDTComponent(expectedSUP, expectedDT, expectedSUP.Name, actualSUP);
        }

        private static void AssertCON(IDT expectedDT, IDT actualDT)
        {
            AssertDTComponent(expectedDT.CON, actualDT, "Content", actualDT.CON);
        }

        private static void AssertDTComponent(IDTComponent expected, IDT expectedDT, string expectedName,
                                              IDTComponent actual)
        {
            Assert.AreEqual(expectedName, actual.Name);
            Assert.AreSame(expectedDT, actual.DT);
            Assert.AreEqual(expected.BasicType.Id, actual.BasicType.Id);
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

        public void TestCreateElementsFileBased()
        {
            repository = new CCRepository(GetFileBasedEARepository());

            IBLibrary bLib = repository.Libraries<IBLibrary>().First();
            Assert.IsNotNull(bLib, "bLib not found");
            IBDTLibrary bdtLib = bLib.CreateBDTLibrary(new LibrarySpec
                                                       {
                                                           Name = "My_BDTLibrary",
                                                           BaseURN = "my/base/urn",
                                                           BusinessTerms =
                                                               new[] {"business term 1", "business term 2"},
                                                           Copyrights = new[] {"copyright 1", "copyright 2"},
                                                           NamespacePrefix = "my_namespace_prefix",
                                                           Owners = new[] {"owner 1", "owner 2", "owner 3"},
                                                           References = new[] {"reference 1"},
                                                           Status = "my status",
                                                           UniqueIdentifier = "a unique ID",
                                                           VersionIdentifier = "a specific version",
                                                       });
            IBIELibrary bieLib = bLib.CreateBIELibrary(new LibrarySpec
                                                       {
                                                           Name = "My_BIELibrary",
                                                           BaseURN = "my/base/urn",
                                                           BusinessTerms =
                                                               new[] {"business term 1", "business term 2"},
                                                           Copyrights = new[] {"copyright 1", "copyright 2"},
                                                           NamespacePrefix = "my_namespace_prefix",
                                                           Owners = new[] {"owner 1", "owner 2", "owner 3"},
                                                           References = new[] {"reference 1"},
                                                           Status = "my status",
                                                           UniqueIdentifier = "a unique ID",
                                                           VersionIdentifier = "a specific version",
                                                       });


            IBDT bdtText = bdtLib.CreateBDT(
                BDTSpec.CloneCDT((ICDT) repository.FindByPath((Path) "ebInterface Data Model"/"CDTLibrary"/"Text"), "My"));

            var accAddress = (IACC) repository.FindByPath((Path) "ebInterface Data Model"/"CCLibrary"/"Address");
            Assert.IsNotNull(accAddress, "ACC Address not found");

            var bccs = new List<IBCC>(accAddress.BCCs);
            bieLib.CreateABIE(new ABIESpec
                              {
                                  Name = "My_" + accAddress.Name,
                                  DictionaryEntryName = "overriding default dictionary entry name",
                                  Definition = "My specific version of an address",
                                  UniqueIdentifier = "my unique identifier",
                                  VersionIdentifier = "my version identifier",
                                  LanguageCode = "my language code",
                                  BusinessTerms = new[] {"business term 1", "business term 2"},
                                  UsageRules = new[] {"usage rule 1", "usage rule 2"},
                                  BasedOn = accAddress,
                                  BBIEs = bccs.Convert(bcc => BBIESpec.CloneBCC(bcc, bdtText)),
                              });
        }

        [Test]
        public void TestCreateABIE()
        {
            var accAddress = (IACC) repository.FindByPath(EARepository1.PathToAddress());
            Assert.IsNotNull(accAddress, "ACC Address not found");

            var bdtText = (IBDT) repository.FindByPath(EARepository1.PathToBDTText());
            Assert.IsNotNull(bdtText, "BDT Text not found");

            IBIELibrary bieLibrary = repository.Libraries<IBIELibrary>().First();

            var bccs = new List<IBCC>(accAddress.BCCs);
            var abieSpec = new ABIESpec
                           {
                               Name = "My_" + accAddress.Name,
                               DictionaryEntryName = "overriding default dictionary entry name",
                               Definition = "My specific version of an address",
                               UniqueIdentifier = "my unique identifier",
                               VersionIdentifier = "my version identifier",
                               LanguageCode = "my language code",
                               BusinessTerms = new[] {"business term 1", "business term 2"},
                               UsageRules = new[] {"usage rule 1", "usage rule 2"},
                               BasedOn = accAddress,
                               BBIEs = bccs.Convert(bcc => BBIESpec.CloneBCC(bcc, bdtText)),
                           };

            IABIE abieAddress = bieLibrary.CreateABIE(abieSpec);
            Assert.IsNotNull(abieAddress, "ABIE is null");
            Assert.AreEqual(bieLibrary.Id, abieAddress.Library.Id);

            Assert.AreEqual(abieSpec.Name, abieAddress.Name);
            Assert.AreEqual(abieSpec.DictionaryEntryName, abieAddress.DictionaryEntryName);
            Assert.AreEqual(abieSpec.Definition, abieAddress.Definition);
            Assert.AreEqual(abieSpec.UniqueIdentifier, abieAddress.UniqueIdentifier);
            Assert.AreEqual(abieSpec.VersionIdentifier, abieAddress.VersionIdentifier);
            Assert.AreEqual(abieSpec.LanguageCode, abieAddress.LanguageCode);
            Assert.AreEqual(abieSpec.BusinessTerms, abieAddress.BusinessTerms);
            Assert.AreEqual(abieSpec.UsageRules, abieAddress.UsageRules);

            Assert.IsNotNull(abieAddress.BasedOn, "BasedOn is null");
            Assert.AreEqual(accAddress.Id, abieAddress.BasedOn.Id);

            Assert.AreEqual(accAddress.BCCs.Count(), abieAddress.BBIEs.Count());
            IEnumerator<IBBIE> bbies = abieAddress.BBIEs.GetEnumerator();
            foreach (IBCC bcc in accAddress.BCCs)
            {
                bbies.MoveNext();
                IBBIE bbie = bbies.Current;
                Assert.AreEqual(bcc.Name, bbie.Name);
                Assert.AreEqual(bdtText.Id, bbie.Type.Id);
                Assert.AreEqual(bcc.Definition, bbie.Definition);
                Assert.AreEqual(bcc.DictionaryEntryName, bbie.DictionaryEntryName);
                Assert.AreEqual(bcc.LanguageCode, bbie.LanguageCode);
                Assert.AreEqual(bcc.UniqueIdentifier, bbie.UniqueIdentifier);
                Assert.AreEqual(bcc.VersionIdentifier, bbie.VersionIdentifier);
                Console.WriteLine(bcc.UsageRules.JoinToString("---"));
                Console.WriteLine(bbie.UsageRules.JoinToString("---"));
                Assert.AreEqual(bcc.UsageRules, bbie.UsageRules);
                Assert.AreEqual(bcc.BusinessTerms, bbie.BusinessTerms);
            }
        }

        [Test]
        public void TestCreateBDT()
        {
            var cdtDate = (ICDT) repository.FindByPath(EARepository1.PathToDate());
            Assert.IsNotNull(cdtDate, "CDT Date not found");

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
            Assert.AreEqual(cdtDate.Id, bdtDate.BasedOn.CDT.Id);

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
        public void TestFindCDTs()
        {
            foreach (ICDTLibrary library in repository.Libraries<ICDTLibrary>())
            {
                IEnumerable<IGrouping<IBasicType, ICDT>> cdtByType = from cdt in library.CDTs
                                                                     group cdt by cdt.CON.BasicType;
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
            Assert.AreEqual(6, libraries.Count);

            IBusinessLibrary bLib1 = libraries[0];
            Assert.AreEqual("blib1", bLib1.Name);
            Assert.AreEqual("urn:test:blib1", bLib1.BaseURN);

            var primLib1 = (IPRIMLibrary) libraries[1];
            Assert.AreEqual("primlib1", primLib1.Name);
            Assert.AreEqual("urn:test:blib1:primlib1", primLib1.BaseURN);
            var prims = new List<IPRIM>(primLib1.PRIMs);
            Assert.AreEqual(2, prims.Count);
            IPRIM stringType = prims[0];
            Assert.AreEqual("String", stringType.Name);
            IPRIM decimalType = prims[1];
            Assert.AreEqual("Decimal", decimalType.Name);

            var cdtLib1 = (ICDTLibrary) libraries[2];
            Assert.AreEqual("cdtlib1", cdtLib1.Name);
            Assert.AreEqual("urn:test:blib1:cdtlib1", cdtLib1.BaseURN);
            var cdts = new List<ICDT>(cdtLib1.CDTs);
            Assert.AreEqual(4, cdts.Count);
            ICDT date = cdts[1];
            Assert.AreEqual(stringType.Id, date.CON.BasicType.Id);
            var dateSups = new List<ISUP>(date.SUPs);
            Assert.AreEqual(1, dateSups.Count);
            ISUP dateFormat = dateSups[0];
            Assert.AreEqual("Format", dateFormat.Name);
            Assert.AreEqual(stringType.Id, dateFormat.BasicType.Id);

            IBDTLibrary bdtLib1 = repository.Libraries<IBDTLibrary>().First();

            ICCLibrary ccLib1 = repository.Libraries<ICCLibrary>().First();
            IACC cdtAddress = ccLib1.ACCs.First();
            var cdtAddressBCCs = new List<IBCC>(cdtAddress.BCCs);
            IBCC bccCountryName = cdtAddressBCCs[0];
            Assert.AreSame(cdtAddress, bccCountryName.Container);
            Assert.AreEqual("CountryName", bccCountryName.Name);
            var cdtText = (ICDT) repository.FindByPath(EARepository1.PathToText());
            Assert.AreEqual(cdtText.Id, bccCountryName.Type.Id);
        }

        [Test]
        public void TestSpecTaggedValues()
        {
            var spec = new BBIESpec
                       {
                           BusinessTerms = new[] {"bt1", "bt2"},
                           Definition = "def",
                           DictionaryEntryName = "den",
                           LanguageCode = "lc",
                           Name = "name",
                           UniqueIdentifier = "ui",
                           VersionIdentifier = "vi",
                           SequencingKey = "sk",
                           UsageRules = new[] {"ur1", "ur2"},
                       };
            foreach (TaggedValueSpec tv in spec.GetTaggedValues())
            {
                Console.WriteLine(tv);
            }
        }

        [Test]
        public void TestStringConcat()
        {
            Assert.AreEqual("0123456789", GenerateNumbers(10).ConcatToString());
            Assert.AreSame(String.Empty, ((IEnumerable<int>) null).ConcatToString());
        }
    }
}