using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.PrimLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddInUnitTests.upcc3.newrepo.ea;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc;
using ICctsRepository=CctsRepository.ICctsRepository;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    [TestFixture]
    public class NewRepoTests
    {
        [Test]
        public void ShouldLoadPrimsFromRepository()
        {
            ICctsRepository cctsRepository = new UpccRepository(new EaUmlRepository(new EARepositoryWithTwoPRIMs()));

            var primLibraries = new List<IPrimLibrary>(cctsRepository.GetPrimLibraries());
            Assert.That(primLibraries, Is.Not.Null);
            Assert.That(primLibraries.Count, Is.EqualTo(1));

            IPrimLibrary primLibrary = primLibraries[0];
            Assert.That(primLibrary, Is.Not.Null);
            Assert.That(primLibrary.Name, Is.EqualTo("PRIMLibrary"));
            Assert.That(primLibrary.NamespacePrefix, Is.EqualTo("prim"));
            Assert.That(primLibrary.Copyrights, Is.EquivalentTo(new[] {"copyright1", "copyright2"}));
            Assert.That(primLibrary.Prims.Count(), Is.EqualTo(2));

            Assert.That(primLibrary.BLibrary, Is.Not.Null);
            Assert.That(primLibrary.BLibrary.Name, Is.EqualTo("bLibrary"));

            IPrim primString = primLibrary.GetPrimByName("String");
            Assert.That(primString, Is.Not.Null);
            Assert.That(primString.Name, Is.EqualTo("String"));
            Assert.That(primString.DictionaryEntryName, Is.EqualTo("String"));
            Assert.That(primString.BusinessTerms, Is.EquivalentTo(new[] {"a sequence of characters", "a sequence of symbols"}));
            Assert.That(primString.Library.Id, Is.EqualTo(primLibrary.Id));

            IPrim primZeichenfolge = primLibrary.GetPrimByName("Zeichenfolge");
            Assert.That(primZeichenfolge.IsEquivalentTo.Id, Is.EqualTo(primString.Id));
        }
    }
}