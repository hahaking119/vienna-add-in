using System.Collections.Generic;
using System.Linq;
using CctsRepository.CcLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.import.ebInterface
{
    [TestFixture]
    public class TargetElementStoreTests
    {
        #region Setup/Teardown

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            var ccRepository = new CCRepository(new MappingTestRepository());
            ccLibrary = ccRepository.GetCcLibraryByPath((Path) "test"/"bLibrary"/"CCLibrary");
            accParty = ccLibrary.ElementByName("Party");
            bccPartyName = accParty.BCCs.First(bcc => bcc.Name == "Name");
            asccPartyResidence = accParty.ASCCs.First(ascc => ascc.Name == "Residence");
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-mapping.mfd"));
        }

        #endregion

        private ICcLibrary ccLibrary;
        private MapForceMapping mapForceMapping;
        private IAcc accParty;
        private IBcc bccPartyName;
        private IAscc asccPartyResidence;

        private void ShouldContainTargetACCElement(TargetElementStore targetElementStore, string name, IAcc referencedAcc, string parentName, params string[] childNames)
        {
            TargetCCElement targetCCElement = ShouldContainTargetCCElement(name, targetElementStore, parentName, childNames);
            Assert.That(targetCCElement.Acc.Id, Is.EqualTo(referencedAcc.Id));
            Assert.IsTrue(targetCCElement.IsACC);
        }

        private void ShouldContainTargetBCCElement(TargetElementStore targetElementStore, string name, IBcc referencedBcc, string parentName, params string[] childNames)
        {
            TargetCCElement targetCCElement = ShouldContainTargetCCElement(name, targetElementStore, parentName, childNames);
            Assert.That(targetCCElement.Bcc.Id, Is.EqualTo(referencedBcc.Id));
            Assert.IsTrue(targetCCElement.IsBCC);
        }

        private void ShouldContainTargetASCCElement(TargetElementStore targetElementStore, string name, IAscc referencedAscc, string parentName, params string[] childNames)
        {
            TargetCCElement targetCCElement = ShouldContainTargetCCElement(name, targetElementStore, parentName, childNames);
            Assert.That(targetCCElement.Ascc.Id, Is.EqualTo(referencedAscc.Id));
            Assert.IsTrue(targetCCElement.IsASCC);
        }

        private TargetCCElement ShouldContainTargetCCElement(string name, TargetElementStore targetElementStore, string parentName, string[] childNames)
        {
            string entryKey = GetTargetEntryKey(name);
            TargetCCElement targetCCElement = targetElementStore.GetTargetElement(entryKey);
            Assert.That(targetCCElement, Is.Not.Null, "Target element '" + name + "' not found");
            if (string.IsNullOrEmpty(parentName))
            {
                Assert.That(targetCCElement.Parent, Is.Null);
            }
            else
            {
                Assert.That(targetCCElement.Parent, Is.Not.Null);
                Assert.That(targetCCElement.Parent, Is.SameAs(targetElementStore.GetTargetElement(GetTargetEntryKey(parentName))));
            }
            Assert.That(targetCCElement.Name, Is.EqualTo(name));
            var children = new List<TargetCCElement>();
            foreach (string childName in childNames)
            {
                children.Add(targetElementStore.GetTargetElement(GetTargetEntryKey(childName)));
            }
            Assert.That(targetCCElement.Children, Is.EquivalentTo(children));
            return targetCCElement;
        }

        private string GetTargetEntryKey(string targetEntryName)
        {
            return FindTargetEntry(targetEntryName).InputOutputKey.Value;
        }

        private Entry FindTargetEntry(string name)
        {
            foreach (SchemaComponent component in mapForceMapping.GetTargetSchemaComponents())
            {
                Entry entry = FindEntryByName(component.RootEntry, name);
                if (entry != null)
                {
                    return entry;
                }
            }
            return null;
        }

        private static Entry FindEntryByName(Entry entry, string name)
        {
            if (entry.Name == name)
            {
                return entry;
            }
            foreach (Entry subEntry in entry.SubEntries)
            {
                Entry e = FindEntryByName(subEntry, name);
                if (e != null)
                {
                    return e;
                }
            }
            return null;
        }

        [Test]
        public void TestTargetElementStore()
        {
            var targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary);
            ShouldContainTargetACCElement(targetElementStore, "Party", accParty, null, "Name", "ResidenceAddress");
            ShouldContainTargetBCCElement(targetElementStore, "Name", bccPartyName, "Party");
            ShouldContainTargetASCCElement(targetElementStore, "ResidenceAddress", asccPartyResidence, "Party", "CityName");
        }
    }
}