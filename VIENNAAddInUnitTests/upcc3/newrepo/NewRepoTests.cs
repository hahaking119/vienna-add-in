using System.Collections.Generic;
using System.Linq;
using CctsRepository.PrimLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddInUnitTests.upcc3.newrepo.ccts;
using VIENNAAddInUnitTests.upcc3.newrepo.ea;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    [TestFixture]
    public class NewRepoTests
    {
        [Test]
        public void ShouldLoadPRIMsFromRepository()
        {
            ICctsRepository cctsRepository = new UpccRepository(new EaUmlRepository(new EARepositoryWithTwoPRIMs()));

            var primLibraries = new List<IPRIMLibrary>(cctsRepository.PRIMLibraries);
            Assert.That(primLibraries, Is.Not.Null);
            Assert.That(primLibraries.Count, Is.EqualTo(1));

            IPRIMLibrary primLibrary = primLibraries[0];
            Assert.That(primLibrary, Is.Not.Null);
            Assert.That(primLibrary.Name, Is.EqualTo("PRIMLibrary"));
            Assert.That(primLibrary.NamespacePrefix, Is.EqualTo("prim"));
            Assert.That(primLibrary.Copyrights, Is.EquivalentTo(new[] {"copyright1", "copyright2"}));
            Assert.That(primLibrary.Elements.Count(), Is.EqualTo(2));

            Assert.That(primLibrary.Parent, Is.Not.Null);
            Assert.That(primLibrary.Parent.Name, Is.EqualTo("bLibrary"));

            IPRIM primString = primLibrary.ElementByName("String");
            Assert.That(primString, Is.Not.Null);
            Assert.That(primString.Name, Is.EqualTo("String"));
            Assert.That(primString.DictionaryEntryName, Is.EqualTo("String"));
            Assert.That(primString.BusinessTerms, Is.EquivalentTo(new[] {"a sequence of characters", "a sequence of symbols"}));
            Assert.That(primString.Library.Id, Is.EqualTo(primLibrary.Id));

            IPRIM primZeichenfolge = primLibrary.ElementByName("Zeichenfolge");
            Assert.That(primZeichenfolge.IsEquivalentTo.Id, Is.EqualTo(primString.Id));
        }
    }
}