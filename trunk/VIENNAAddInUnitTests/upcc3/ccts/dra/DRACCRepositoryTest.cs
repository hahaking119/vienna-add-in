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
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
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
            cctsRepository = new CCRepository(eaRepository);
        }

        #endregion

        private ICctsRepository cctsRepository;
        private Repository eaRepository;

        private static void AssertCDTSUPs(ICdt expectedCDT, ICdt actualCDT)
        {
            Assert.AreEqual(expectedCDT.SUPs.Count(), actualCDT.SUPs.Count());
            var actualSUPs = actualCDT.SUPs.GetEnumerator();
            foreach (var expectedSUP in expectedCDT.SUPs)
            {
                actualSUPs.MoveNext();
                AssertCDTSUP(expectedSUP, actualCDT, actualSUPs.Current);
            }
        }

        private static void AssertCDTSUP(ICdtSup expectedSUP, ICdt actualCDT, ICdtSup actualSUP)
        {
            Assert.AreSame(actualCDT, actualSUP.CDT);
            Assert.AreEqual(expectedSUP.Name, actualSUP.Name);
            Assert.AreEqual(expectedSUP.BasicType.Id, actualSUP.BasicType.Id);
            Assert.AreEqual(expectedSUP.Definition, actualSUP.Definition);
            Assert.AreEqual(expectedSUP.DictionaryEntryName, actualSUP.DictionaryEntryName);
            Assert.AreEqual(expectedSUP.LanguageCode, actualSUP.LanguageCode);
            Assert.AreEqual(expectedSUP.UniqueIdentifier, actualSUP.UniqueIdentifier);
            Assert.AreEqual(expectedSUP.VersionIdentifier, actualSUP.VersionIdentifier);
            Assert.AreEqual(expectedSUP.LowerBound, actualSUP.LowerBound);
            Assert.AreEqual(expectedSUP.ModificationAllowedIndicator, actualSUP.ModificationAllowedIndicator);
            Assert.AreEqual(expectedSUP.UpperBound, actualSUP.UpperBound);
            Assert.AreEqual(expectedSUP.UsageRules, actualSUP.UsageRules);
            Assert.AreEqual(expectedSUP.BusinessTerms, actualSUP.BusinessTerms);
        }

        private static void AssertCDTCON(ICdt expectedCDT, ICdt actualBDT)
        {
            ICdtCon expectedCON = expectedCDT.CON;
            ICdtCon actualCON = actualBDT.CON;
            Assert.AreSame(actualBDT, actualCON.CDT);
            Assert.AreEqual("Content", actualCON.Name);
            Assert.AreEqual(expectedCON.BasicType.Id, actualCON.BasicType.Id);
            Assert.AreEqual(expectedCON.Definition, actualCON.Definition);
            Assert.AreEqual(expectedCON.DictionaryEntryName, actualCON.DictionaryEntryName);
            Assert.AreEqual(expectedCON.LanguageCode, actualCON.LanguageCode);
            Assert.AreEqual(expectedCON.UniqueIdentifier, actualCON.UniqueIdentifier);
            Assert.AreEqual(expectedCON.VersionIdentifier, actualCON.VersionIdentifier);
            Assert.AreEqual(expectedCON.LowerBound, actualCON.LowerBound);
            Assert.AreEqual(expectedCON.ModificationAllowedIndicator, actualCON.ModificationAllowedIndicator);
            Assert.AreEqual(expectedCON.UpperBound, actualCON.UpperBound);
            Assert.AreEqual(expectedCON.UsageRules, actualCON.UsageRules);
            Assert.AreEqual(expectedCON.BusinessTerms, actualCON.BusinessTerms);
        }

        private static void AssertBDTSUPs(ICdt cdt, IBdt bdt)
        {
            Assert.AreEqual(cdt.SUPs.Count(), bdt.Sups.Count());
            IEnumerator<IBdtSup> bdtSups = bdt.Sups.GetEnumerator();
            foreach (ICdtSup cdtSup in cdt.SUPs)
            {
                bdtSups.MoveNext();
                AssertBDTSUP(cdtSup, bdt, bdtSups.Current);
            }
        }

        private static void AssertBDTSUP(ICdtSup cdtSUP, IBdt expectedBDT, IBdtSup bdtSUP)
        {
            Assert.AreEqual(cdtSUP.Name, bdtSUP.Name);
            Assert.AreSame(expectedBDT, bdtSUP.Bdt);
            Assert.AreEqual(cdtSUP.BasicType.Id, bdtSUP.BasicType.Id);
            Assert.AreEqual(cdtSUP.Definition, bdtSUP.Definition);
            Assert.AreEqual(cdtSUP.DictionaryEntryName, bdtSUP.DictionaryEntryName);
            Assert.AreEqual(cdtSUP.LanguageCode, bdtSUP.LanguageCode);
            Assert.AreEqual(cdtSUP.UniqueIdentifier, bdtSUP.UniqueIdentifier);
            Assert.AreEqual(cdtSUP.VersionIdentifier, bdtSUP.VersionIdentifier);
            Assert.AreEqual(cdtSUP.LowerBound, bdtSUP.LowerBound);
            Assert.AreEqual(cdtSUP.ModificationAllowedIndicator, bdtSUP.ModificationAllowedIndicator);
            Assert.AreEqual(cdtSUP.UpperBound, bdtSUP.UpperBound);
            Assert.AreEqual(cdtSUP.UsageRules, bdtSUP.UsageRules);
            Assert.AreEqual(cdtSUP.BusinessTerms, bdtSUP.BusinessTerms);
        }

        private static void AssertBDTCON(ICdt cdt, IBdt bdt)
        {
            ICdtCon expectedCON = cdt.CON;
            IBdtCon actualCON = bdt.Con;
            Assert.AreSame(bdt, actualCON.Bdt);
            Assert.AreEqual(expectedCON.BasicType.Id, actualCON.BasicType.Id);
            Assert.AreEqual(expectedCON.Definition, actualCON.Definition);
            Assert.AreEqual(expectedCON.DictionaryEntryName, actualCON.DictionaryEntryName);
            Assert.AreEqual(expectedCON.LanguageCode, actualCON.LanguageCode);
            Assert.AreEqual(expectedCON.UniqueIdentifier, actualCON.UniqueIdentifier);
            Assert.AreEqual(expectedCON.VersionIdentifier, actualCON.VersionIdentifier);
            Assert.AreEqual(expectedCON.LowerBound, actualCON.LowerBound);
            Assert.AreEqual(expectedCON.ModificationAllowedIndicator, actualCON.ModificationAllowedIndicator);
            Assert.AreEqual(expectedCON.UpperBound, actualCON.UpperBound);
            Assert.AreEqual(expectedCON.UsageRules, actualCON.UsageRules);
            Assert.AreEqual(expectedCON.BusinessTerms, actualCON.BusinessTerms);
        }

        private static void AssertASBIE(string name, string lowerBound, string upperBound, IAsbie asbie)
        {
            Assert.AreEqual(name, asbie.Name);
            Assert.AreEqual(lowerBound, asbie.LowerBound);
            Assert.AreEqual(upperBound, asbie.UpperBound);
        }

        private static void AssertBBIE(IBdt type, IBcc bcc, IBbie bbie)
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
        public void ResolvesAbieBasedOnDependencies()
        {
            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            Assert.That(accPerson, Is.Not.Null, "ACC not found");

            var abiePerson = cctsRepository.GetAbieByPath(EARepository1.PathToBIEPerson());
            Assert.That(abiePerson, Is.Not.Null, "ABIE not found");

            Assert.That(abiePerson.BasedOn, Is.Not.Null, "ABIE basedOn dependency is null");
            Assert.That(abiePerson.BasedOn.Id, Is.EqualTo(accPerson.Id), "ABIE basedOn dependency not correctly resolved");
        }

        [Test]
        public void ResolvesAsbieBasedOnDependencies()
        {
            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            var abiePerson = cctsRepository.GetAbieByPath(EARepository1.PathToBIEPerson());

            IAscc asccHomeAddress = accPerson.ASCCs.FirstOrDefault(ascc => ascc.Name == "homeAddress");
            Assert.That(asccHomeAddress, Is.Not.Null, "ASCC not found");

            IAsbie asbieMyHomeAddress = abiePerson.ASBIEs.FirstOrDefault(asbie => asbie.Name == "My_homeAddress");
            Assert.That(asbieMyHomeAddress, Is.Not.Null, "ASBIE not found");

            Assert.That(asbieMyHomeAddress.BasedOn, Is.Not.Null, "ASBIE basedOn dependency is null");
            Assert.That(asbieMyHomeAddress.BasedOn.Id, Is.EqualTo(asccHomeAddress.Id), "ASBIE basedOn dependency not correctly resolved");
        }

        [Test]
        public void ResolvesBbieBasedOnDependencies()
        {
            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            var abiePerson = cctsRepository.GetAbieByPath(EARepository1.PathToBIEPerson());

            IBcc bccFirstName = accPerson.BCCs.FirstOrDefault(bcc => bcc.Name == "FirstName");
            Assert.That(bccFirstName, Is.Not.Null, "BCC not found");

            IBbie bbieMyFirstName = abiePerson.BBIEs.FirstOrDefault(bbie => bbie.Name == "My_FirstName");
            Assert.That(bbieMyFirstName, Is.Not.Null, "BBIE not found");

            Assert.That(bbieMyFirstName.BasedOn, Is.Not.Null, "BBIE basedOn dependency is null");
            Assert.That(bbieMyFirstName.BasedOn.Id, Is.EqualTo(bccFirstName.Id), "BBIE basedOn dependency not correctly resolved");
        }

        [Test]
        public void TestABIEEquals()
        {
            var abie1 = cctsRepository.GetAbieByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(abie1);
            var abie2 = cctsRepository.GetAbieByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(abie2);
            Assert.AreEqual(abie1, abie2);
        }

        [Test]
        public void TestCreateABIE()
        {
            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            Assert.IsNotNull(accPerson, "ACC Person not found");

            var bieAddress = cctsRepository.GetAbieByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(bieAddress, "BIE Address not found");

            var bdtText = cctsRepository.GetBdtByPath(EARepository1.PathToBDTText());
            Assert.IsNotNull(bdtText, "BDT Text not found");

            IBieLibrary bieLibrary = cctsRepository.GetBieLibraries().First();

            var bccs = new List<IBcc>(accPerson.BCCs);
            var asccs = new List<IAscc>(accPerson.ASCCs);
            Assert.AreEqual(2, asccs.Count);
            var abieSpec = new AbieSpec
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
                               Bbies = bccs.Convert(bcc => BbieSpec.CloneBCC(bcc, bdtText)),
                               Asbies = new List<AsbieSpec>
                                        {
                                            AsbieSpec.CloneASCC(asccs[0], "My_homeAddress", bieAddress.Id),
                                            AsbieSpec.CloneASCC(asccs[1], "My_workAddress", bieAddress.Id)
                                        },
                           };

            IAbie abiePerson = bieLibrary.CreateAbie(abieSpec);
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
            IEnumerator<IBbie> bbies = abiePerson.BBIEs.GetEnumerator();
            foreach (IBcc bcc in accPerson.BCCs)
            {
                bbies.MoveNext();
                IBbie bbie = bbies.Current;
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

            var asbies = new List<IAsbie>(abiePerson.ASBIEs);
            Assert.AreEqual(2, asbies.Count());
            AssertASBIE("My_homeAddress", "1", "1", asbies[0]);
            AssertASBIE("My_workAddress", "0", "*", asbies[1]);
        }

        [Test]
        public void TestCreateACC()
        {
            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            Assert.IsNotNull(accPerson, "ACC Person not found");

            var accAddress = cctsRepository.GetAccByPath(EARepository1.PathToACCAddress());
            Assert.IsNotNull(accAddress, "ACC Address not found");

            var cdtText = cctsRepository.GetCdtByPath(EARepository1.PathToCDTText());
            Assert.IsNotNull(cdtText, "CDT Text not found");

            var cdtDate = cctsRepository.GetCdtByPath(EARepository1.PathToCDTDate());
            Assert.IsNotNull(cdtDate, "CDT Date not found");

            ICcLibrary ccLibrary = cctsRepository.GetCcLibraries().First();

            var accSpec = new AccSpec
                          {
                              Name = "TestPerson",
                              DictionaryEntryName = "overriding default dictionary entry name",
                              Definition = "a test person",
                              UniqueIdentifier = "my unique identifier",
                              VersionIdentifier = "my version identifier",
                              LanguageCode = "my language code",
                              BusinessTerms = new[] {"business term 1", "business term 2"},
                              UsageRules = new[] {"usage rule 1", "usage rule 2"},
                              IsEquivalentTo = accPerson,
                          };
            var bccFirstNameSpec = new BccSpec
                                   {
                                       Name = "FirstName",
                                       Type = cdtText,
                                       BusinessTerms = new[] {"businessTerms"},
                                       Definition = "definition",
                                       DictionaryEntryName = "dictionaryEntryName",
                                       LanguageCode = "languageCode",
                                       SequencingKey = "sequencingKey",
                                       UniqueIdentifier = "uniqueIdentifier",
                                       UsageRules = new[] {"usageRules"},
                                       VersionIdentifier = "versionIdentifier",
                                       LowerBound = "1",
                                       UpperBound = "3",
                                   };
            accSpec.AddBCC(bccFirstNameSpec);
            accSpec.AddBCC(new BccSpec
                           {
                               Name = "LastName",
                               Type = cdtText
                           });
            accSpec.AddBCC(new BccSpec
                           {
                               Name = "Some",
                               Type = cdtText
                           });
            accSpec.AddBCC(new BccSpec
                           {
                               Name = "Some",
                               Type = cdtDate
                           });

            var asccHomeAddressSpec = new AsccSpec
                                      {
                                          Name = "HomeAddress",
                                          ResolveAssociatedACC = () => accAddress,
                                          BusinessTerms = new[] {"businessTerms"},
                                          Definition = "definition",
                                          DictionaryEntryName = "dictionaryEntryName",
                                          LanguageCode = "languageCode",
                                          SequencingKey = "sequencingKey",
                                          UniqueIdentifier = "uniqueIdentifier",
                                          UsageRules = new[] {"usageRules"},
                                          VersionIdentifier = "versionIdentifier",
                                          LowerBound = "1",
                                          UpperBound = "3",
                                      };
            accSpec.AddASCC(asccHomeAddressSpec);
            accSpec.AddASCC(new AsccSpec
                            {
                                Name = "WorkAddress",
                                ResolveAssociatedACC = () => accAddress,
                            });
            accSpec.AddASCC(new AsccSpec
                            {
                                Name = "Some",
                                ResolveAssociatedACC = () => accAddress,
                            });
            accSpec.AddASCC(new AsccSpec
                            {
                                Name = "Some",
                                ResolveAssociatedACC = () => accPerson,
                            });

            IAcc accTestPerson = ccLibrary.CreateAcc(accSpec);
            Assert.IsNotNull(accTestPerson, "ACC is null");
            Assert.AreEqual(ccLibrary.Id, accTestPerson.Library.Id);

            Assert.AreEqual(accSpec.Name, accTestPerson.Name);
            Assert.AreEqual(accSpec.DictionaryEntryName, accTestPerson.DictionaryEntryName);
            Assert.AreEqual(accSpec.Definition, accTestPerson.Definition);
            Assert.AreEqual(accSpec.UniqueIdentifier, accTestPerson.UniqueIdentifier);
            Assert.AreEqual(accSpec.VersionIdentifier, accTestPerson.VersionIdentifier);
            Assert.AreEqual(accSpec.LanguageCode, accTestPerson.LanguageCode);
            Assert.AreEqual(accSpec.BusinessTerms, accTestPerson.BusinessTerms);
            Assert.AreEqual(accSpec.UsageRules, accTestPerson.UsageRules);

            Assert.IsNotNull(accTestPerson.IsEquivalentTo, "IsEquivalentTo is null");
            Assert.AreEqual(accPerson.Id, accTestPerson.IsEquivalentTo.Id);

            Assert.AreEqual(accSpec.BCCs.Count(), accTestPerson.BCCs.Count());
            IBcc bccFirstName = accTestPerson.BCCs.FirstOrDefault(bcc => bcc.Name == bccFirstNameSpec.Name);
            Assert.That(bccFirstName, Is.Not.Null, "BCC FirstName not generated");
            Assert.AreEqual(cdtText.Id, bccFirstName.Cdt.Id);
            Assert.AreEqual(asccHomeAddressSpec.Definition, bccFirstName.Definition);
            Assert.AreEqual(asccHomeAddressSpec.DictionaryEntryName, bccFirstName.DictionaryEntryName);
            Assert.AreEqual(asccHomeAddressSpec.LanguageCode, bccFirstName.LanguageCode);
            Assert.AreEqual(asccHomeAddressSpec.UniqueIdentifier, bccFirstName.UniqueIdentifier);
            Assert.AreEqual(asccHomeAddressSpec.VersionIdentifier, bccFirstName.VersionIdentifier);
            Assert.AreEqual(asccHomeAddressSpec.UsageRules, bccFirstName.UsageRules);
            Assert.AreEqual(asccHomeAddressSpec.BusinessTerms, bccFirstName.BusinessTerms);
            Assert.AreEqual(asccHomeAddressSpec.LowerBound, bccFirstName.LowerBound);
            Assert.AreEqual(asccHomeAddressSpec.UpperBound, bccFirstName.UpperBound);

            Assert.That(accTestPerson.BCCs.FirstOrDefault(bcc => bcc.Name == "LastName"), Is.Not.Null, "BCC 'LastName' not generated");
            Assert.That(accTestPerson.BCCs.FirstOrDefault(bcc => bcc.Name == "SomeText"), Is.Not.Null, "Type not appended to BCC 'Some' with type 'Text'");
            Assert.That(accTestPerson.BCCs.FirstOrDefault(bcc => bcc.Name == "SomeDate"), Is.Not.Null, "Type not appended to BCC 'Some' with type 'Date'");

            Assert.AreEqual(accSpec.ASCCs.Count(), accTestPerson.ASCCs.Count());
            IAscc asccHomeAddress = accTestPerson.ASCCs.FirstOrDefault(ascc => ascc.Name == asccHomeAddressSpec.Name);
            Assert.That(asccHomeAddress, Is.Not.Null, "ASCC HomeAddress not generated");
            Assert.AreEqual(accAddress.Id, asccHomeAddress.AssociatedElement.Id);
            Assert.AreEqual(asccHomeAddress.Definition, asccHomeAddress.Definition);
            Assert.AreEqual(asccHomeAddress.DictionaryEntryName, asccHomeAddress.DictionaryEntryName);
            Assert.AreEqual(asccHomeAddress.LanguageCode, asccHomeAddress.LanguageCode);
            Assert.AreEqual(asccHomeAddress.UniqueIdentifier, asccHomeAddress.UniqueIdentifier);
            Assert.AreEqual(asccHomeAddress.VersionIdentifier, asccHomeAddress.VersionIdentifier);
            Assert.AreEqual(asccHomeAddress.UsageRules, asccHomeAddress.UsageRules);
            Assert.AreEqual(asccHomeAddress.BusinessTerms, asccHomeAddress.BusinessTerms);
            Assert.AreEqual(asccHomeAddress.LowerBound, asccHomeAddress.LowerBound);
            Assert.AreEqual(asccHomeAddress.UpperBound, asccHomeAddress.UpperBound);

            Assert.That(accTestPerson.ASCCs.FirstOrDefault(ascc => ascc.Name == "WorkAddress"), Is.Not.Null, "ASCC 'WorkAddress' not generated");
            Assert.That(accTestPerson.ASCCs.FirstOrDefault(ascc => ascc.Name == "SomeAddress"), Is.Not.Null, "Type not appended to ASCC 'Some' with type 'Address'");
            Assert.That(accTestPerson.ASCCs.FirstOrDefault(ascc => ascc.Name == "SomePerson"), Is.Not.Null, "Type not appended to ASCC 'Some' with type 'Person'");
        }

        [Test]
        public void TestCreateCDT()
        {
            var cdtDate = cctsRepository.GetCdtByPath(EARepository1.PathToCDTDate());
            Assert.IsNotNull(cdtDate, "CDT Date not found");

            var cdtLibrary = cctsRepository.GetCdtLibraries().First();

            var cdtSpec = new CdtSpec(cdtDate)
                          {
                              Name = "Datum",
                              IsEquivalentTo = cdtDate
                          };
            var cdtDatum = cdtLibrary.CreateCdt(cdtSpec);

            Assert.IsNotNull(cdtDatum, "CDT is null");
            Assert.AreEqual(cdtLibrary.Id, cdtDatum.Library.Id);

            Assert.AreEqual(cdtSpec.Name, cdtDatum.Name);

            Assert.AreEqual(cdtDate.Definition, cdtDatum.Definition);
            Assert.AreEqual(cdtDate.DictionaryEntryName, cdtDatum.DictionaryEntryName);
            Assert.AreEqual(cdtDate.LanguageCode, cdtDatum.LanguageCode);
            Assert.AreEqual(cdtDate.UniqueIdentifier, cdtDatum.UniqueIdentifier);
            Assert.AreEqual(cdtDate.VersionIdentifier, cdtDatum.VersionIdentifier);
            Assert.AreEqual(cdtDate.BusinessTerms, cdtDatum.BusinessTerms);
            Assert.AreEqual(cdtDate.UsageRules, cdtDatum.UsageRules);

            Assert.IsNotNull(cdtDatum.IsEquivalentTo, "IsEquivalentTo is null");
            Assert.AreEqual(cdtDate.Id, cdtDatum.IsEquivalentTo.Id);

            AssertCDTCON(cdtDate, cdtDatum);
            AssertCDTSUPs(cdtDate, cdtDatum);
        }

        [Test]
        public void TestCreateBDT()
        {
            var cdtDate = cctsRepository.GetCdtByPath(EARepository1.PathToCDTDate());
            Assert.IsNotNull(cdtDate, "CDT Date not found");

            IBdtLibrary bdtLibrary = cctsRepository.GetBdtLibraries().First();

            BdtSpec bdtSpec = BdtSpec.CloneCdt(cdtDate, "My_" + cdtDate.Name);
            IBdt bdtDate = bdtLibrary.CreateBdt(bdtSpec);

            Assert.IsNotNull(bdtDate, "BDT is null");
            Assert.AreEqual(bdtLibrary.Id, bdtDate.BdtLibrary.Id);

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

            AssertBDTCON(cdtDate, bdtDate);
            AssertBDTSUPs(cdtDate, bdtDate);
        }

        [Test]
        [Category(TestCategories.FileBased)]
        public void TestCreateElementsFileBased()
        {
            using (var repository = new TemporaryFileBasedRepository(TestUtils.PathToTestResource("XSDGeneratorTest.eap")))
            {
                cctsRepository = new CCRepository(repository);

                IBLibrary bLib = cctsRepository.GetBLibraries().First();
                Assert.IsNotNull(bLib, "bLib not found");
                IBdtLibrary bdtLib = bLib.CreateBDTLibrary(new BdtLibrarySpec
                                                           {
                                                               Name = "My_BDTLibrary",
                                                               BaseUrn = "my/base/urn",
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
                IBieLibrary bieLib = bLib.CreateBIELibrary(new BieLibrarySpec
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

                var cdtText = cctsRepository.GetCdtByPath((Path) "Model"/"ebInterface Data Model"/"CDTLibrary"/"Text");
                Assert.IsNotNull(cdtText, "CDT 'Text' not found.");
                IBdt bdtText = bdtLib.CreateBdt(BdtSpec.CloneCdt(cdtText, "My_Text"));
                Assert.IsNotNull(bdtText.BasedOn);

                var accAddress = cctsRepository.GetAccByPath((Path) "Model"/"ebInterface Data Model"/"CCLibrary"/"Address");
                Assert.IsNotNull(accAddress, "ACC Address not found");
                var asccs = new List<IAscc>(accAddress.ASCCs);
                Assert.AreEqual(2, asccs.Count);

                var bccs = new List<IBcc>(accAddress.BCCs);
                var abieSpec = new AbieSpec
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
                                   Bbies = bccs.Convert(bcc => BbieSpec.CloneBCC(bcc, bdtText)),
                               };
                IAbie myAddress = bieLib.CreateAbie(abieSpec);
                Element myAddressElement = repository.GetElementByID(myAddress.Id);
                var attribute = (Attribute) myAddressElement.Attributes.GetAt(0);
                Assert.AreEqual(8, attribute.TaggedValues.Count);
            }
        }

        [Test]
        public void TestCreateENUM()
        {
            var enumAbcCodes = cctsRepository.GetEnumByPath(EARepository1.PathToEnumAbcCodes());
            Assert.IsNotNull(enumAbcCodes, "ENUM ABC_Codes not found");

            IEnumLibrary enumLibrary = cctsRepository.GetEnumLibraries().First();

            var enumSpec = new EnumSpec
                           {
                               Name = "My_ABC_Codes",
                               DictionaryEntryName = "overriding default dictionary entry name",
                               Definition = "a test enum",
                               UniqueIdentifier = "my unique identifier",
                               VersionIdentifier = "my version identifier",
                               LanguageCode = "my language code",
                               BusinessTerms = new[] {"business term 1", "business term 2"},
                               CodeListAgencyIdentifier = "code list agency identifier",
                               CodeListAgencyName = "code list agency name",
                               CodeListIdentifier = "code list identifier",
                               CodeListName = "code list name",
                               EnumerationURI = "enumeration URI",
                               ModificationAllowedIndicator = true,
                               RestrictedPrimitive = "String",
                               Status = "status",
                               IsEquivalentTo = enumAbcCodes,
                           };
            enumSpec.AddCodelistEntry(new CodelistEntrySpec
                                      {
                                          Name = "a",
                                          CodeName = "aa",
                                          Status = "status",
                                      });
            enumSpec.AddCodelistEntry(new CodelistEntrySpec
                                      {
                                          Name = "b",
                                          CodeName = "bb",
                                          Status = "status",
                                      });

            IEnum enumMyAbcCodes = enumLibrary.CreateEnum(enumSpec);
            Assert.IsNotNull(enumMyAbcCodes, "ENUM is null");
            Assert.AreEqual(enumLibrary.Id, enumMyAbcCodes.Library.Id);

            Assert.AreEqual(enumSpec.Name, enumMyAbcCodes.Name);
            Assert.AreEqual(enumSpec.DictionaryEntryName, enumMyAbcCodes.DictionaryEntryName);
            Assert.AreEqual(enumSpec.Definition, enumMyAbcCodes.Definition);
            Assert.AreEqual(enumSpec.UniqueIdentifier, enumMyAbcCodes.UniqueIdentifier);
            Assert.AreEqual(enumSpec.VersionIdentifier, enumMyAbcCodes.VersionIdentifier);
            Assert.AreEqual(enumSpec.LanguageCode, enumMyAbcCodes.LanguageCode);
            Assert.AreEqual(enumSpec.BusinessTerms, enumMyAbcCodes.BusinessTerms);
            Assert.AreEqual(enumSpec.CodeListAgencyIdentifier, enumMyAbcCodes.CodeListAgencyIdentifier);
            Assert.AreEqual(enumSpec.CodeListAgencyName, enumMyAbcCodes.CodeListAgencyName);
            Assert.AreEqual(enumSpec.CodeListIdentifier, enumMyAbcCodes.CodeListIdentifier);
            Assert.AreEqual(enumSpec.CodeListName, enumMyAbcCodes.CodeListName);
            Assert.AreEqual(enumSpec.EnumerationURI, enumMyAbcCodes.EnumerationURI);
            Assert.AreEqual(enumSpec.ModificationAllowedIndicator, enumMyAbcCodes.ModificationAllowedIndicator);
            Assert.AreEqual(enumSpec.RestrictedPrimitive, enumMyAbcCodes.RestrictedPrimitive);
            Assert.AreEqual(enumSpec.Status, enumMyAbcCodes.Status);

            Assert.IsNotNull(enumMyAbcCodes.IsEquivalentTo, "IsEquivalentTo is null");
            Assert.AreEqual(enumAbcCodes.Id, enumMyAbcCodes.IsEquivalentTo.Id);

            Assert.AreEqual(enumSpec.CodelistEntries.Count(), enumMyAbcCodes.CodelistEntries.Count());
            IEnumerator<CodelistEntrySpec> expectedCodelistEntries = enumSpec.CodelistEntries.GetEnumerator();
            foreach (ICodelistEntry codelistEntry in enumMyAbcCodes.CodelistEntries)
            {
                expectedCodelistEntries.MoveNext();
                CodelistEntrySpec expectedCodelistEntry = expectedCodelistEntries.Current;
                Assert.AreEqual(expectedCodelistEntry.Name, codelistEntry.Name);
                Assert.AreEqual(expectedCodelistEntry.CodeName, codelistEntry.CodeName);
                Assert.AreEqual(expectedCodelistEntry.Status, codelistEntry.Status);
            }
        }

        [Test]
        public void TestCreateLibrary()
        {
            IBLibrary bLib = cctsRepository.GetBLibraries().First();
            Assert.IsNotNull(bLib, "bLib not found");
            var spec = new BdtLibrarySpec
                       {
                           Name = "MyBDTLibrary",
                           BaseUrn = "my/base/urn",
                           BusinessTerms = new[] {"business term 1", "business term 2"},
                           Copyrights = new[] {"copyright 1", "copyright 2"},
                           NamespacePrefix = "my_namespace_prefix",
                           Owners = new[] {"owner 1", "owner 2", "owner 3"},
                           References = new[] {"reference 1"},
                           Status = "my status",
                           UniqueIdentifier = "a unique ID",
                           VersionIdentifier = "a specific version",
                       };
            IBdtLibrary bdtLib = bLib.CreateBDTLibrary(spec);
            Assert.AreEqual(bLib.Id, bdtLib.BLibrary.Id);
            Assert.AreEqual(spec.Name, bdtLib.Name);
            Assert.AreEqual(spec.BaseUrn, bdtLib.BaseURN);
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
        public void TestCreatePRIM()
        {
            var primString = cctsRepository.GetPrimByPath(EARepository1.PathToString());
            Assert.IsNotNull(primString, "PRIM String not found");

            IPrimLibrary primLibrary = cctsRepository.GetPrimLibraries().First();

            var primSpec = new PrimSpec
                           {
                               Name = "Zeichenkette",
                               DictionaryEntryName = "overriding default dictionary entry name",
                               Definition = "a test person",
                               UniqueIdentifier = "my unique identifier",
                               VersionIdentifier = "my version identifier",
                               LanguageCode = "my language code",
                               BusinessTerms = new[] {"business term 1", "business term 2"},
                               Pattern = "pattern",
                               FractionDigits = "3",
                               Length = "4",
                               MaxExclusive = "5",
                               MaxInclusive = "6",
                               MaxLength = "7",
                               MinExclusive = "8",
                               MinInclusive = "9",
                               MinLength = "10",
                               TotalDigits = "11",
                               WhiteSpace = "preserve",
                               IsEquivalentTo = primString,
                           };

            IPrim primZeichenkette = primLibrary.CreatePrim(primSpec);
            Assert.IsNotNull(primZeichenkette, "PRIM is null");
            Assert.AreEqual(primLibrary.Id, primZeichenkette.Library.Id);

            Assert.AreEqual(primSpec.Name, primZeichenkette.Name);
            Assert.AreEqual(primSpec.DictionaryEntryName, primZeichenkette.DictionaryEntryName);
            Assert.AreEqual(primSpec.Definition, primZeichenkette.Definition);
            Assert.AreEqual(primSpec.UniqueIdentifier, primZeichenkette.UniqueIdentifier);
            Assert.AreEqual(primSpec.VersionIdentifier, primZeichenkette.VersionIdentifier);
            Assert.AreEqual(primSpec.LanguageCode, primZeichenkette.LanguageCode);
            Assert.AreEqual(primSpec.BusinessTerms, primZeichenkette.BusinessTerms);
            Assert.AreEqual(primSpec.Pattern, primZeichenkette.Pattern);
            Assert.AreEqual(primSpec.FractionDigits, primZeichenkette.FractionDigits);
            Assert.AreEqual(primSpec.Length, primZeichenkette.Length);
            Assert.AreEqual(primSpec.MaxExclusive, primZeichenkette.MaxExclusive);
            Assert.AreEqual(primSpec.MaxInclusive, primZeichenkette.MaxInclusive);
            Assert.AreEqual(primSpec.MaxLength, primZeichenkette.MaxLength);
            Assert.AreEqual(primSpec.MinExclusive, primZeichenkette.MinExclusive);
            Assert.AreEqual(primSpec.MinInclusive, primZeichenkette.MinInclusive);
            Assert.AreEqual(primSpec.MinLength, primZeichenkette.MinLength);
            Assert.AreEqual(primSpec.TotalDigits, primZeichenkette.TotalDigits);
            Assert.AreEqual(primSpec.WhiteSpace, primZeichenkette.WhiteSpace);

            Assert.IsNotNull(primZeichenkette.IsEquivalentTo, "IsEquivalentTo is null");
            Assert.AreEqual(primString.Id, primZeichenkette.IsEquivalentTo.Id);
        }

        [Test]
        public void TestFindCDTs()
        {
            foreach (ICdtLibrary library in cctsRepository.GetCdtLibraries())
            {
                IEnumerable<IGrouping<IBasicType, ICdt>> cdtByType = from cdt in library.Cdts
                                                                     group cdt by cdt.CON.BasicType;
                foreach (var cdtGroup in cdtByType)
                {
                    Console.WriteLine(cdtGroup.Key);
                    foreach (ICdt cdt in cdtGroup)
                    {
                        Console.WriteLine("  " + cdt.Name);
                    }
                }
            }
        }

        [Test]
        public void TestReadAccess()
        {
            var bLib1 = cctsRepository.GetBLibraries().First();
            Assert.AreEqual("blib1", bLib1.Name);
            Assert.AreEqual("urn:test:blib1", bLib1.BaseURN);

            IPrimLibrary primLib1 = cctsRepository.GetPrimLibraries().First();
            Assert.AreEqual("primlib1", primLib1.Name);
            Assert.AreEqual("urn:test:blib1:primlib1", primLib1.BaseURN);
            var prims = new List<IPrim>(primLib1.Prims);
            Assert.AreEqual(2, prims.Count);
            IPrim stringType = prims[1];
            Assert.AreEqual("String", stringType.Name);
            IPrim decimalType = prims[0];
            Assert.AreEqual("Decimal", decimalType.Name);

            ICdtLibrary cdtLib1 = cctsRepository.GetCdtLibraries().First();
            Assert.AreEqual("cdtlib1", cdtLib1.Name);
            Assert.AreEqual("urn:test:blib1:cdtlib1", cdtLib1.BaseURN);
            var cdts = new List<ICdt>(cdtLib1.Cdts);
            Assert.AreEqual(4, cdts.Count);
            ICdt date = cdts[1];
            Assert.AreEqual(stringType.Id, date.CON.BasicType.Id);
            var dateSups = new List<ICdtSup>(date.SUPs);
            Assert.AreEqual(1, dateSups.Count);
            var dateFormat = dateSups[0];
            Assert.AreEqual("Format", dateFormat.Name);
            Assert.AreEqual(stringType.Id, dateFormat.BasicType.Id);

            ICcLibrary ccLib1 = cctsRepository.GetCcLibraries().First();
            IAcc accAddress = ccLib1.Accs.First();
            var accAddressBCCs = new List<IBcc>(accAddress.BCCs);

            IBcc bccCountryName = accAddressBCCs[1];
            Assert.AreSame(accAddress, bccCountryName.Acc);
            Assert.AreEqual("CountryName", bccCountryName.Name);
            Assert.AreEqual("1", bccCountryName.LowerBound);
            Assert.AreEqual("1", bccCountryName.UpperBound);

            IBcc bccPostcode = accAddressBCCs[2];
            Assert.AreEqual("Postcode", bccPostcode.Name);
            Assert.AreEqual("0", bccPostcode.LowerBound);
            Assert.AreEqual("*", bccPostcode.UpperBound);
            var cdtText = cctsRepository.GetCdtByPath(EARepository1.PathToCDTText());
            Assert.AreEqual(cdtText.Id, bccCountryName.Cdt.Id);

            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            var accPersonASCCs = new List<IAscc>(accPerson.ASCCs);
            Assert.AreEqual("homeAddress", accPersonASCCs[0].Name);
            Assert.AreEqual("1", accPersonASCCs[0].LowerBound);
            Assert.AreEqual("1", accPersonASCCs[0].UpperBound);
            Assert.AreEqual("workAddress", accPersonASCCs[1].Name);
            Assert.AreEqual("0", accPersonASCCs[1].LowerBound);
            Assert.AreEqual("*", accPersonASCCs[1].UpperBound);

            var bdtText = cctsRepository.GetBdtByPath(EARepository1.PathToBDTText());
            Assert.AreEqual("This is the definition of BDT Text.", bdtText.Definition);

            var abieAddress = cctsRepository.GetAbieByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(abieAddress);
            Assert.AreEqual(accAddress.Id, abieAddress.BasedOn.Id);
            var abieAddressBBIEs = new List<IBbie>(abieAddress.BBIEs);

            IBbie bbieCountryName = abieAddressBBIEs[1];
            Assert.AreSame(abieAddress, bbieCountryName.Container);
            Assert.AreEqual("CountryName", bbieCountryName.Name);
            Assert.AreEqual("1", bbieCountryName.LowerBound);
            Assert.AreEqual("1", bbieCountryName.UpperBound);
            Assert.AreEqual(bdtText.Id, bbieCountryName.Type.Id);

            IBcc bbiePostcode = accAddressBCCs[2];
            Assert.AreEqual("Postcode", bbiePostcode.Name);
            Assert.AreEqual("0", bbiePostcode.LowerBound);
            Assert.AreEqual("*", bbiePostcode.UpperBound);

            var abiePerson = cctsRepository.GetAbieByPath(EARepository1.PathToBIEPerson());
            var abiePersonASBIEs = new List<IAsbie>(abiePerson.ASBIEs);
            Assert.AreEqual("My_homeAddress", abiePersonASBIEs[0].Name);
            Assert.AreEqual("1", abiePersonASBIEs[0].LowerBound);
            Assert.AreEqual("1", abiePersonASBIEs[0].UpperBound);
            Assert.AreEqual("My_workAddress", abiePersonASBIEs[1].Name);
            Assert.AreEqual("0", abiePersonASBIEs[1].LowerBound);
            Assert.AreEqual("*", abiePersonASBIEs[1].UpperBound);
            var biePerson = cctsRepository.GetAbieByPath(EARepository1.PathToBIEPerson());
            Assert.AreEqual("My_homeAddress", biePerson.ASBIEs.First().Name);

            var enumAbcCodes = cctsRepository.GetEnumByPath(EARepository1.PathToEnumAbcCodes());
            Assert.IsNotNull(enumAbcCodes, "enum ABC_Codes not found");
            Assert.AreEqual("ABC_Codes", enumAbcCodes.Name);
            var enumAbcCodesValues = new List<ICodelistEntry>(enumAbcCodes.CodelistEntries);
            Assert.AreEqual(2, enumAbcCodesValues.Count);
            Assert.AreEqual("ABC Code 1", enumAbcCodesValues[0].Name);
            Assert.AreEqual("ABC Code 2", enumAbcCodesValues[1].Name);

            var docLibrary = cctsRepository.GetDocLibraryByPath((Path) "test model"/"blib1"/"DOCLibrary");
            var docLibraryABIEs = new List<IAbie>(docLibrary.Abies);
            Assert.AreEqual(2, docLibraryABIEs.Count);
            IAbie invoice = docLibrary.RootAbie;
            Assert.That(invoice, Is.Not.Null);
            Assert.AreEqual("Invoice", invoice.Name);
        }

        [Test]
        public void TestUpdateABIE()
        {
            var accPerson = cctsRepository.GetAccByPath(EARepository1.PathToACCPerson());
            Assert.IsNotNull(accPerson, "ACC Person not found");

            var bieAddress = cctsRepository.GetAbieByPath(EARepository1.PathToBIEAddress());
            Assert.IsNotNull(bieAddress, "BIE Address not found");

            var bdtText = cctsRepository.GetBdtByPath(EARepository1.PathToBDTText());
            Assert.IsNotNull(bdtText, "BDT Text not found");

            IBieLibrary bieLibrary = cctsRepository.GetBieLibraries().First();

            var bccs = new List<IBcc>(accPerson.BCCs);
            var asccs = new List<IAscc>(accPerson.ASCCs);
            Assert.AreEqual(2, asccs.Count);
            var abieSpec = new AbieSpec
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
                               Bbies = bccs.Convert(bcc => BbieSpec.CloneBCC(bcc, bdtText)),
                               Asbies = new List<AsbieSpec>
                                        {
                                            AsbieSpec.CloneASCC(asccs[0], "My_homeAddress", bieAddress.Id),
                                            AsbieSpec.CloneASCC(asccs[1], "My_workAddress", bieAddress.Id)
                                        },
                           };

            IAbie abiePerson = bieLibrary.CreateAbie(abieSpec);
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
            IEnumerator<IBbie> bbies = abiePerson.BBIEs.GetEnumerator();
            foreach (IBcc bcc in accPerson.BCCs)
            {
                bbies.MoveNext();
                IBbie bbie = bbies.Current;
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

            var asbies = new List<IAsbie>(abiePerson.ASBIEs);
            Assert.AreEqual(2, asbies.Count());
            AssertASBIE("My_homeAddress", "1", "1", asbies[0]);
            AssertASBIE("My_workAddress", "0", "*", asbies[1]);

            //-------------------

            var updatedPersonSpec = AbieSpec.CloneAbie(abiePerson);
            updatedPersonSpec.Name = "Another_Person";
            updatedPersonSpec.Definition = "Another kind of person.";
            updatedPersonSpec.BusinessTerms = new[] {"human being", "living thing"};
            updatedPersonSpec.RemoveAsbie("My_workAddress");
            updatedPersonSpec.RemoveBbie("NickName");
            Assert.AreEqual(2, updatedPersonSpec.Bbies.Count());

            abiePerson = bieLibrary.UpdateAbie(abiePerson, updatedPersonSpec);

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

            asbies = new List<IAsbie>(abiePerson.ASBIEs);
            Assert.AreEqual(1, asbies.Count());
            AssertASBIE("My_homeAddress", "1", "1", asbies[0]);

            var personBCCs = new List<IBcc>(accPerson.BCCs);
            var personBBIEs = new List<IBbie>(abiePerson.BBIEs);
            Assert.AreEqual(3, personBCCs.Count());
            Assert.AreEqual(2, personBBIEs.Count());
            AssertBBIE(bdtText, personBCCs[0], personBBIEs[0]);
            AssertBBIE(bdtText, personBCCs[1], personBBIEs[1]);
        }
    }
}