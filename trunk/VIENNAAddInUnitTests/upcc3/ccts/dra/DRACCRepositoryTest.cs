// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;

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
            ccRepository = new CCRepository(eaRepository);
        }

        #endregion

        private ICCRepository ccRepository;
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
            Assert.AreEqual(expected.UsageRules, actual.UsageRules);
            Assert.AreEqual(expected.BusinessTerms, actual.BusinessTerms);
        }

        private static void AssertASBIE(string name, string lowerBound, string upperBound, IASBIE asbie)
        {
            Assert.AreEqual(name, asbie.Name);
            Assert.AreEqual(lowerBound, asbie.LowerBound);
            Assert.AreEqual(upperBound, asbie.UpperBound);
        }

        private static void AssertBBIE(IBDT type, IBCC bcc, IBBIE bbie)
        {
            Assert.AreEqual(type.Id, bbie.Type.Id);
            Assert.AreEqual(bcc.Name, bbie.Name);
            Assert.AreEqual(bcc.Definition, bbie.Definition);
            Assert.AreEqual(bcc.DictionaryEntryName, bbie.DictionaryEntryName);
            Assert.AreEqual(bcc.LanguageCode, bbie.LanguageCode);
            Assert.AreEqual(bcc.UniqueIdentifier, bbie.UniqueIdentifier);
            Assert.AreEqual(bcc.VersionIdentifier, bbie.VersionIdentifier);
            Assert.AreEqual(bcc.UsageRules, bbie.UsageRules);
            Assert.AreEqual(bcc.BusinessTerms, bbie.BusinessTerms);
            Assert.AreEqual(bcc.LowerBound, bbie.LowerBound);
            Assert.AreEqual(bcc.UpperBound, bbie.UpperBound);
        }

        [Test]
        public void TestABIEEquals()
        {
            object abie1 = ccRepository.FindByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(abie1);
            object abie2 = ccRepository.FindByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(abie2);
            Assert.AreEqual(abie1, abie2);
        }

        [Test]
        public void TestCreateABIE()
        {
            var accPerson = (IACC) ccRepository.FindByPath(EARepository1.PathToACCPerson());
            Assert.IsNotNull(accPerson, "ACC Person not found");

            var bieAddress = (IABIE) ccRepository.FindByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(bieAddress, "BIE Address not found");

            var bdtText = (IBDT) ccRepository.FindByPath(EARepository1.PathToBDTText());
            Assert.IsNotNull(bdtText, "BDT Text not found");

            IBIELibrary bieLibrary = ccRepository.Libraries<IBIELibrary>().First();

            var bccs = new List<IBCC>(accPerson.BCCs);
            var asccs = new List<IASCC>(accPerson.ASCCs);
            Assert.AreEqual(2, asccs.Count);
            var abieSpec = new ABIESpec
                           {
                               Name = "My_" + accPerson.Name,
                               DictionaryEntryName = "overriding default dictionary entry name",
                               Definition = "My specific version of an address",
                               UniqueIdentifier = "my unique identifier",
                               VersionIdentifier = "my version identifier",
                               LanguageCode = "my language code",
                               BusinessTerms = new[] {"business term 1", "business term 2"},
                               UsageRules = new[] {"usage rule 1", "usage rule 2"},
                               BasedOn = accPerson,
                               BBIEs = bccs.Convert(bcc => BBIESpec.CloneBCC(bcc, bdtText)),
                               ASBIEs = new List<ASBIESpec>
                                        {
                                            ASBIESpec.CloneASCC(asccs[0], "My_homeAddress", bieAddress.Id),
                                            ASBIESpec.CloneASCC(asccs[1], "My_workAddress", bieAddress.Id)
                                        },
                           };

            IABIE abiePerson = bieLibrary.CreateElement(abieSpec);
            Assert.IsNotNull(abiePerson, "ABIE is null");
            Assert.AreEqual(bieLibrary.Id, abiePerson.Library.Id);

            Assert.AreEqual(abieSpec.Name, abiePerson.Name);
            Assert.AreEqual(abieSpec.DictionaryEntryName, abiePerson.DictionaryEntryName);
            Assert.AreEqual(abieSpec.Definition, abiePerson.Definition);
            Assert.AreEqual(abieSpec.UniqueIdentifier, abiePerson.UniqueIdentifier);
            Assert.AreEqual(abieSpec.VersionIdentifier, abiePerson.VersionIdentifier);
            Assert.AreEqual(abieSpec.LanguageCode, abiePerson.LanguageCode);
            Assert.AreEqual(abieSpec.BusinessTerms, abiePerson.BusinessTerms);
            Assert.AreEqual(abieSpec.UsageRules, abiePerson.UsageRules);

            Assert.IsNotNull(abiePerson.BasedOn, "BasedOn is null");
            Assert.AreEqual(accPerson.Id, abiePerson.BasedOn.Id);

            Assert.AreEqual(accPerson.BCCs.Count(), abiePerson.BBIEs.Count());
            IEnumerator<IBBIE> bbies = abiePerson.BBIEs.GetEnumerator();
            foreach (IBCC bcc in accPerson.BCCs)
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
                Assert.AreEqual(bcc.UsageRules, bbie.UsageRules);
                Assert.AreEqual(bcc.BusinessTerms, bbie.BusinessTerms);
                Assert.AreEqual(bcc.LowerBound, bbie.LowerBound);
                Assert.AreEqual(bcc.UpperBound, bbie.UpperBound);
            }

            var asbies = new List<IASBIE>(abiePerson.ASBIEs);
            Assert.AreEqual(2, asbies.Count());
            AssertASBIE("My_homeAddress", "1", "1", asbies[0]);
            AssertASBIE("My_workAddress", "0", "*", asbies[1]);
        }

        [Test]
        public void TestCreateBDT()
        {
            var cdtDate = (ICDT) ccRepository.FindByPath(EARepository1.PathToDate());
            Assert.IsNotNull(cdtDate, "CDT Date not found");

            IBDTLibrary bdtLibrary = ccRepository.Libraries<IBDTLibrary>().First();

            BDTSpec bdtSpec = BDTSpec.CloneCDT(cdtDate, "My_" + cdtDate.Name);
            IBDT bdtDate = bdtLibrary.CreateElement(bdtSpec);

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
        [Category(TestCategories.FileBased)]
        public void TestCreateElementsFileBased()
        {
            using (var repository = new TemporaryFileBasedRepository(TestUtils.PathToTestResource("XSDGeneratorTest.eap")))
            {
                ccRepository = new CCRepository(repository);

                IBLibrary bLib = ccRepository.Libraries<IBLibrary>().First();
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

                var cdtText = (ICDT) ccRepository.FindByPath((Path) "Model"/"ebInterface Data Model"/"CDTLibrary"/"Text");
                Assert.IsNotNull(cdtText, "CDT 'Text' not found.");
                IBDT bdtText = bdtLib.CreateElement(BDTSpec.CloneCDT(cdtText, "My_Text"));
                Assert.IsNotNull(bdtText.BasedOn);

                var accAddress = (IACC) ccRepository.FindByPath((Path) "Model"/"ebInterface Data Model"/"CCLibrary"/"Address");
                Assert.IsNotNull(accAddress, "ACC Address not found");
                var asccs = new List<IASCC>(accAddress.ASCCs);
                Assert.AreEqual(2, asccs.Count);

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
                IABIE myAddress = bieLib.CreateElement(abieSpec);
                Element myAddressElement = repository.GetElementByID(myAddress.Id);
                var attribute = (Attribute) myAddressElement.Attributes.GetAt(0);
                Assert.AreEqual(8, attribute.TaggedValues.Count);
            }
        }

        [Test]
        public void TestCreateLibrary()
        {
            IBLibrary bLib = ccRepository.Libraries<IBLibrary>().First();
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
            foreach (ICDTLibrary library in ccRepository.Libraries<ICDTLibrary>())
            {
                IEnumerable<IGrouping<IBasicType, ICDT>> cdtByType = from cdt in library.Elements
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
            var libraries = new List<IBusinessLibrary>(ccRepository.AllLibraries());
            Assert.AreEqual(10, libraries.Count);

            IBusinessLibrary bLib1 = ccRepository.Libraries<IBLibrary>().First();
            Assert.AreEqual("blib1", bLib1.Name);
            Assert.AreEqual("urn:test:blib1", bLib1.BaseURN);

            IPRIMLibrary primLib1 = ccRepository.Libraries<IPRIMLibrary>().First();
            Assert.AreEqual("primlib1", primLib1.Name);
            Assert.AreEqual("urn:test:blib1:primlib1", primLib1.BaseURN);
            var prims = new List<IPRIM>(primLib1.Elements);
            Assert.AreEqual(2, prims.Count);
            IPRIM stringType = prims[1];
            Assert.AreEqual("String", stringType.Name);
            IPRIM decimalType = prims[0];
            Assert.AreEqual("Decimal", decimalType.Name);

            ICDTLibrary cdtLib1 = ccRepository.Libraries<ICDTLibrary>().First();
            Assert.AreEqual("cdtlib1", cdtLib1.Name);
            Assert.AreEqual("urn:test:blib1:cdtlib1", cdtLib1.BaseURN);
            var cdts = new List<ICDT>(cdtLib1.Elements);
            Assert.AreEqual(4, cdts.Count);
            ICDT date = cdts[1];
            Assert.AreEqual(stringType.Id, date.CON.BasicType.Id);
            var dateSups = new List<ISUP>(date.SUPs);
            Assert.AreEqual(1, dateSups.Count);
            ISUP dateFormat = dateSups[0];
            Assert.AreEqual("Format", dateFormat.Name);
            Assert.AreEqual(stringType.Id, dateFormat.BasicType.Id);

            ICCLibrary ccLib1 = ccRepository.Libraries<ICCLibrary>().First();
            IACC accAddress = ccLib1.Elements.First();
            var accAddressBCCs = new List<IBCC>(accAddress.BCCs);

            IBCC bccCountryName = accAddressBCCs[1];
            Assert.AreSame(accAddress, bccCountryName.Container);
            Assert.AreEqual("CountryName", bccCountryName.Name);
            Assert.AreEqual("1", bccCountryName.LowerBound);
            Assert.AreEqual("1", bccCountryName.UpperBound);

            IBCC bccPostcode = accAddressBCCs[2];
            Assert.AreEqual("Postcode", bccPostcode.Name);
            Assert.AreEqual("0", bccPostcode.LowerBound);
            Assert.AreEqual("*", bccPostcode.UpperBound);
            var cdtText = (ICDT) ccRepository.FindByPath(EARepository1.PathToText());
            Assert.AreEqual(cdtText.Id, bccCountryName.Type.Id);

            var accPerson = (IACC) ccRepository.FindByPath(EARepository1.PathToACCPerson());
            var accPersonASCCs = new List<IASCC>(accPerson.ASCCs);
            Assert.AreEqual("homeAddress", accPersonASCCs[0].Name);
            Assert.AreEqual("1", accPersonASCCs[0].LowerBound);
            Assert.AreEqual("1", accPersonASCCs[0].UpperBound);
            Assert.AreEqual("workAddress", accPersonASCCs[1].Name);
            Assert.AreEqual("0", accPersonASCCs[1].LowerBound);
            Assert.AreEqual("*", accPersonASCCs[1].UpperBound);

            var bdtText = (IBDT) ccRepository.FindByPath(EARepository1.PathToBDTText());
            Assert.AreEqual("This is the definition of BDT Text.", bdtText.Definition);

            var abieAddress = (IABIE) ccRepository.FindByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(abieAddress);
            Assert.AreEqual(accAddress.Id, abieAddress.BasedOn.Id);
            var abieAddressBBIEs = new List<IBBIE>(abieAddress.BBIEs);

            IBBIE bbieCountryName = abieAddressBBIEs[1];
            Assert.AreSame(abieAddress, bbieCountryName.Container);
            Assert.AreEqual("CountryName", bbieCountryName.Name);
            Assert.AreEqual("1", bbieCountryName.LowerBound);
            Assert.AreEqual("1", bbieCountryName.UpperBound);
            Assert.AreEqual(bdtText.Id, bbieCountryName.Type.Id);

            IBCC bbiePostcode = accAddressBCCs[2];
            Assert.AreEqual("Postcode", bbiePostcode.Name);
            Assert.AreEqual("0", bbiePostcode.LowerBound);
            Assert.AreEqual("*", bbiePostcode.UpperBound);

            var abiePerson = (IABIE) ccRepository.FindByPath(EARepository1.PathToBIEPerson());
            var abiePersonASBIEs = new List<IASBIE>(abiePerson.ASBIEs);
            Assert.AreEqual("My_homeAddress", abiePersonASBIEs[0].Name);
            Assert.AreEqual("1", abiePersonASBIEs[0].LowerBound);
            Assert.AreEqual("1", abiePersonASBIEs[0].UpperBound);
            Assert.AreEqual("My_workAddress", abiePersonASBIEs[1].Name);
            Assert.AreEqual("0", abiePersonASBIEs[1].LowerBound);
            Assert.AreEqual("*", abiePersonASBIEs[1].UpperBound);
            var biePerson = (IABIE) ccRepository.FindByPath(EARepository1.PathToBIEPerson());
            Assert.AreEqual("My_homeAddress", biePerson.ASBIEs.First().Name);

            var enumAbcCodes = (IENUM) ccRepository.FindByPath(EARepository1.PathToEnumAbcCodes());
            Assert.IsNotNull(enumAbcCodes, "enum ABC_Codes not found");
            Assert.AreEqual("ABC_Codes", enumAbcCodes.Name);
            IDictionary<string, string> enumAbcCodesValues = enumAbcCodes.Values;
            Assert.AreEqual(2, enumAbcCodesValues.Count);
            Assert.AreEqual("abc1", enumAbcCodesValues["ABC Code 1"]);
            Assert.AreEqual("abc2", enumAbcCodesValues["ABC Code 2"]);

            var docLibrary = ccRepository.LibraryByName<IDOCLibrary>("DOCLibrary");
            var docLibraryABIEs = new List<IABIE>(docLibrary.Elements);
            Assert.AreEqual(2, docLibraryABIEs.Count);
            var docLibraryRootElements = new List<IABIE>(docLibrary.RootElements);
            Assert.AreEqual(1, docLibraryRootElements.Count);
            IABIE invoice = docLibraryRootElements[0];
            Assert.AreEqual("Invoice", invoice.Name);
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
        public void TestUpdateABIE()
        {
            var accPerson = (IACC) ccRepository.FindByPath(EARepository1.PathToACCPerson());
            Assert.IsNotNull(accPerson, "ACC Person not found");

            var bieAddress = (IABIE) ccRepository.FindByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(bieAddress, "BIE Address not found");

            var bdtText = (IBDT) ccRepository.FindByPath(EARepository1.PathToBDTText());
            Assert.IsNotNull(bdtText, "BDT Text not found");

            IBIELibrary bieLibrary = ccRepository.Libraries<IBIELibrary>().First();

            var bccs = new List<IBCC>(accPerson.BCCs);
            var asccs = new List<IASCC>(accPerson.ASCCs);
            Assert.AreEqual(2, asccs.Count);
            var abieSpec = new ABIESpec
                           {
                               Name = "My_" + accPerson.Name,
                               DictionaryEntryName = "overriding default dictionary entry name",
                               Definition = "My specific version of an address",
                               UniqueIdentifier = "my unique identifier",
                               VersionIdentifier = "my version identifier",
                               LanguageCode = "my language code",
                               BusinessTerms = new[] {"business term 1", "business term 2"},
                               UsageRules = new[] {"usage rule 1", "usage rule 2"},
                               BasedOn = accPerson,
                               BBIEs = bccs.Convert(bcc => BBIESpec.CloneBCC(bcc, bdtText)),
                               ASBIEs = new List<ASBIESpec>
                                        {
                                            ASBIESpec.CloneASCC(asccs[0], "My_homeAddress", bieAddress.Id),
                                            ASBIESpec.CloneASCC(asccs[1], "My_workAddress", bieAddress.Id)
                                        },
                           };

            IABIE abiePerson = bieLibrary.CreateElement(abieSpec);
            Assert.IsNotNull(abiePerson, "ABIE is null");
            Assert.AreEqual(bieLibrary.Id, abiePerson.Library.Id);

            Assert.AreEqual(abieSpec.Name, abiePerson.Name);
            Assert.AreEqual(abieSpec.DictionaryEntryName, abiePerson.DictionaryEntryName);
            Assert.AreEqual(abieSpec.Definition, abiePerson.Definition);
            Assert.AreEqual(abieSpec.UniqueIdentifier, abiePerson.UniqueIdentifier);
            Assert.AreEqual(abieSpec.VersionIdentifier, abiePerson.VersionIdentifier);
            Assert.AreEqual(abieSpec.LanguageCode, abiePerson.LanguageCode);
            Assert.AreEqual(abieSpec.BusinessTerms, abiePerson.BusinessTerms);
            Assert.AreEqual(abieSpec.UsageRules, abiePerson.UsageRules);

            Assert.IsNotNull(abiePerson.BasedOn, "BasedOn is null");
            Assert.AreEqual(accPerson.Id, abiePerson.BasedOn.Id);

            Assert.AreEqual(accPerson.BCCs.Count(), abiePerson.BBIEs.Count());
            IEnumerator<IBBIE> bbies = abiePerson.BBIEs.GetEnumerator();
            foreach (IBCC bcc in accPerson.BCCs)
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
                Assert.AreEqual(bcc.UsageRules, bbie.UsageRules);
                Assert.AreEqual(bcc.BusinessTerms, bbie.BusinessTerms);
                Assert.AreEqual(bcc.LowerBound, bbie.LowerBound);
                Assert.AreEqual(bcc.UpperBound, bbie.UpperBound);
            }

            var asbies = new List<IASBIE>(abiePerson.ASBIEs);
            Assert.AreEqual(2, asbies.Count());
            AssertASBIE("My_homeAddress", "1", "1", asbies[0]);
            AssertASBIE("My_workAddress", "0", "*", asbies[1]);

            //-------------------

            var updatedPersonSpec = new ABIESpec(abiePerson)
                                    {
                                        Name = "Another_Person",
                                        Definition = "Another kind of person.",
                                        BusinessTerms = new[] {"human being", "living thing"}
                                    };
            updatedPersonSpec.RemoveASBIE("My_workAddress");
            updatedPersonSpec.RemoveBBIE("NickName");
            Assert.AreEqual(2, updatedPersonSpec.BBIEs.Count());

            abiePerson = bieLibrary.UpdateElement(abiePerson, updatedPersonSpec);

            Assert.IsNotNull(abiePerson, "ABIE is null");
            Assert.AreEqual(bieLibrary.Id, abiePerson.Library.Id);

            Assert.AreEqual(updatedPersonSpec.Name, abiePerson.Name);
            Assert.AreEqual(updatedPersonSpec.DictionaryEntryName, abiePerson.DictionaryEntryName);
            Assert.AreEqual(updatedPersonSpec.Definition, abiePerson.Definition);
            Assert.AreEqual(updatedPersonSpec.UniqueIdentifier, abiePerson.UniqueIdentifier);
            Assert.AreEqual(updatedPersonSpec.VersionIdentifier, abiePerson.VersionIdentifier);
            Assert.AreEqual(updatedPersonSpec.LanguageCode, abiePerson.LanguageCode);
            Assert.AreEqual(updatedPersonSpec.BusinessTerms, abiePerson.BusinessTerms);
            Assert.AreEqual(updatedPersonSpec.UsageRules.ToArray(), abiePerson.UsageRules);

            Assert.IsNotNull(abiePerson.BasedOn, "BasedOn is null");
            Assert.AreEqual(accPerson.Id, abiePerson.BasedOn.Id);

            Assert.IsNull(abiePerson.IsEquivalentTo);

            asbies = new List<IASBIE>(abiePerson.ASBIEs);
            Assert.AreEqual(1, asbies.Count());
            AssertASBIE("My_homeAddress", "1", "1", asbies[0]);

            var personBCCs = new List<IBCC>(accPerson.BCCs);
            var personBBIEs = new List<IBBIE>(abiePerson.BBIEs);
            Assert.AreEqual(3, personBCCs.Count());
            Assert.AreEqual(2, personBBIEs.Count());
            AssertBBIE(bdtText, personBCCs[0], personBBIEs[0]);
            AssertBBIE(bdtText, personBCCs[1], personBBIEs[1]);
        }

        [Test]
        public void ResolvesAbieBasedOnDependencies()
        {
            var accPerson = (IACC)ccRepository.FindByPath(EARepository1.PathToACCPerson());
            Assert.That(accPerson, Is.Not.Null, "ACC not found");

            var abiePerson = (IABIE)ccRepository.FindByPath(EARepository1.PathToBIEPerson());
            Assert.That(abiePerson, Is.Not.Null, "ABIE not found");

            Assert.That(abiePerson.BasedOn, Is.Not.Null, "ABIE basedOn dependency is null");
            Assert.That(abiePerson.BasedOn.Id, Is.EqualTo(accPerson.Id), "ABIE basedOn dependency not correctly resolved");
        }

        [Test]
        public void ResolvesBbieBasedOnDependencies()
        {
            var accPerson = (IACC)ccRepository.FindByPath(EARepository1.PathToACCPerson());
            var abiePerson = (IABIE)ccRepository.FindByPath(EARepository1.PathToBIEPerson());

            var bccFirstName = accPerson.BCCs.FirstOrDefault(bcc => bcc.Name == "FirstName");
            Assert.That(bccFirstName, Is.Not.Null, "BCC not found");

            var bbieMyFirstName = abiePerson.BBIEs.FirstOrDefault(bbie => bbie.Name == "My_FirstName");
            Assert.That(bbieMyFirstName, Is.Not.Null, "BBIE not found");

            Assert.That(bbieMyFirstName.BasedOn, Is.Not.Null, "BBIE basedOn dependency is null");
            Assert.That(bbieMyFirstName.BasedOn.Id, Is.EqualTo(bccFirstName.Id), "BBIE basedOn dependency not correctly resolved");
        }

        [Test]
        public void ResolvesAsbieBasedOnDependencies()
        {
            var accPerson = (IACC)ccRepository.FindByPath(EARepository1.PathToACCPerson());
            var abiePerson = (IABIE)ccRepository.FindByPath(EARepository1.PathToBIEPerson());

            var asccHomeAddress = accPerson.ASCCs.FirstOrDefault(ascc => ascc.Name == "homeAddress");
            Assert.That(asccHomeAddress, Is.Not.Null, "ASCC not found");

            var asbieMyHomeAddress = abiePerson.ASBIEs.FirstOrDefault(asbie => asbie.Name == "My_homeAddress");
            Assert.That(asbieMyHomeAddress, Is.Not.Null, "ASBIE not found");

            Assert.That(asbieMyHomeAddress.BasedOn, Is.Not.Null, "ASBIE basedOn dependency is null");
            Assert.That(asbieMyHomeAddress.BasedOn.Id, Is.EqualTo(asccHomeAddress.Id), "ASBIE basedOn dependency not correctly resolved");
        }

    }
}