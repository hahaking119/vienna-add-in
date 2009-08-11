using System;
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
        private ICCLibrary ccLibrary;
        private MapForceMapping mapForceMapping;
        private IACC accParty;

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            var ccRepository = new CCRepository(new MappingTestRepository());
            ccLibrary = ccRepository.LibraryByName<ICCLibrary>("CCLibrary");
            accParty = ccLibrary.ElementByName("Party");
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\nested-mapping.mfd"));
        }
        [Test]
        public void TestTargetElementStore()
        {
            var targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary);
            var partyKey = GetTargetEntryKey("Party");
            var party = targetElementStore.GetTargetElement(partyKey);
            Assert.That(party, Is.Not.Null, "Target element 'Party' not found");
            Assert.IsTrue(party.IsACC);
            Assert.That(party.Reference.Id, Is.EqualTo(accParty.Id));
            Assert.That(party.Parent, Is.Null);
            Assert.That(party.Name, Is.EqualTo("Party"));
            Assert.That(party.Children, Is.EquivalentTo(new[]
                                                        {
                                                            targetElementStore.GetTargetElement(GetTargetEntryKey("Name")), 
                                                            targetElementStore.GetTargetElement(GetTargetEntryKey("ResidenceAddress"))
                                                        }));
        }

        private string GetTargetEntryKey(string targetEntryName)
        {
            return FindTargetEntry(targetEntryName).InputOutputKey.Value;
        }

        private Entry FindTargetEntry(string name)
        {
            foreach (var component in mapForceMapping.GetTargetSchemaComponents())
            {
                var entry = FindEntryByName(component.RootEntry, name);
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
            foreach (var subEntry in entry.SubEntries)
            {
                var e = FindEntryByName(subEntry, name);
                if (e != null)
                {
                    return e;
                }
            }
            return null;
        }
    }
}