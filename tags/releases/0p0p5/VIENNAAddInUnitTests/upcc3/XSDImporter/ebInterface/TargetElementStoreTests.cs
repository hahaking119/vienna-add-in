using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDImporter.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
{
    [TestFixture]
    public class TargetElementStoreTests
    {
        #region Setup/Teardown

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            var ccRepository = new CCRepository(new MappingTestRepository());
            ccLibrary = ccRepository.LibraryByName<ICCLibrary>("CCLibrary");
            accParty = ccLibrary.ElementByName("Party");
            bccPartyName = accParty.BCCs.First(bcc => bcc.Name == "Name");
            asccPartyResidence = accParty.ASCCs.First(ascc => ascc.Name == "Residence");
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-mapping.mfd"));
        }

        #endregion

        private ICCLibrary ccLibrary;
        private MapForceMapping mapForceMapping;
        private IACC accParty;
        private IBCC bccPartyName;
        private IASCC asccPartyResidence;

        private void ShouldContainTargetACCElement(TargetElementStore targetElementStore, string name, ICC referencedCC, string parentName, params string[] childNames)
        {
            TargetCCElement targetCCElement = ShouldContainTargetCCElement(name, targetElementStore, referencedCC, parentName, childNames);
            Assert.IsTrue(targetCCElement.IsACC);
        }

        private void ShouldContainTargetBCCElement(TargetElementStore targetElementStore, string name, ICC referencedCC, string parentName, params string[] childNames)
        {
            TargetCCElement targetCCElement = ShouldContainTargetCCElement(name, targetElementStore, referencedCC, parentName, childNames);
            Assert.IsTrue(targetCCElement.IsBCC);
        }

        private void ShouldContainTargetASCCElement(TargetElementStore targetElementStore, string name, ICC referencedCC, string parentName, params string[] childNames)
        {
            TargetCCElement targetCCElement = ShouldContainTargetCCElement(name, targetElementStore, referencedCC, parentName, childNames);
            Assert.IsTrue(targetCCElement.IsASCC);
        }

        private TargetCCElement ShouldContainTargetCCElement(string name, TargetElementStore targetElementStore, ICC referencedCC, string parentName, string[] childNames)
        {
            string entryKey = GetTargetEntryKey(name);
            TargetCCElement targetCCElement = targetElementStore.GetTargetElement(entryKey);
            Assert.That(targetCCElement, Is.Not.Null, "Target element '" + name + "' not found");
            Assert.That(targetCCElement.Reference.Id, Is.EqualTo(referencedCC.Id));
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
            ShouldContainTargetBCCElement(targetElementStore, "CityName", asccPartyResidence, "ResidenceAddress");
        }
    }
}