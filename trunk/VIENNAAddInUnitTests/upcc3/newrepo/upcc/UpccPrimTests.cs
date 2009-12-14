using CctsRepository.PrimLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    [TestFixture]
    public class UpccPrimTests
    {
        [Test]
        public void ShouldReturnId()
        {
            var umlDataType = new UmlDataTypeBuilder()
                .WithId(7)
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.Id, Is.EqualTo(umlDataType.Id));
        }

        [Test]
        public void ShouldReturnName()
        {
            var umlDataType = new UmlDataTypeBuilder()
                .WithName("aName")
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.Name, Is.EqualTo(umlDataType.Name));
        }

	    [Test]
        public void ShouldReturnPrimLibrary()
        {
            var umlPackage = new UmlPackageBuilder()
                .WithId(6)
                .Build();
            var umlDataType = new UmlDataTypeBuilder()
                .WithPackage(umlPackage)
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.PrimLibrary, Is.Not.Null);
            Assert.That(prim.PrimLibrary.Id, Is.EqualTo(umlPackage.Id));
        }

        [Test]
        public void ShouldResolveIsEquivalentToDependency()
        {
            var targetUmlDataType = new UmlDataTypeBuilder()
                .WithId(5)
                .Build();
            var umlDataType = new UmlDataTypeBuilder()
                .WithDependencies(Stereotype.isEquivalentTo, targetUmlDataType)
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.IsEquivalentTo.Id, Is.Not.Null);
            Assert.That(prim.IsEquivalentTo.Id, Is.EqualTo(targetUmlDataType.Id));
        }

        [Test]
        public void ShouldResolveFirstOfMultipleIsEquivalentToDependencies()
        {
            var targetUmlDataType1 = new UmlDataTypeBuilder()
                .WithId(5)
                .Build();
            var targetUmlDataType2 = new UmlDataTypeBuilder()
                .WithId(6)
                .Build();
            var umlDataType = new UmlDataTypeBuilder()
                .WithDependencies(Stereotype.isEquivalentTo, targetUmlDataType1, targetUmlDataType2)
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.IsEquivalentTo.Id, Is.Not.Null);
            Assert.That(prim.IsEquivalentTo.Id, Is.EqualTo(targetUmlDataType1.Id));
        }

        [Test]
        public void ShouldResolveNullIsEquivalentToDependencyToNull()
        {
            var umlDataType = new UmlDataTypeBuilder()
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.IsEquivalentTo, Is.Null);
        }

		[Test]
        public void ShouldReturnEmptyValuesForNullTaggedValues()
        {
            var umlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.BusinessTerms, Is.Not.Null);
            Assert.That(prim.BusinessTerms, Is.Empty);
            Assert.That(prim.Definition, Is.Not.Null);
            Assert.That(prim.Definition, Is.Empty);
            Assert.That(prim.DictionaryEntryName, Is.Not.Null);
            Assert.That(prim.DictionaryEntryName, Is.Empty);
            Assert.That(prim.FractionDigits, Is.Not.Null);
            Assert.That(prim.FractionDigits, Is.Empty);
            Assert.That(prim.LanguageCode, Is.Not.Null);
            Assert.That(prim.LanguageCode, Is.Empty);
            Assert.That(prim.Length, Is.Not.Null);
            Assert.That(prim.Length, Is.Empty);
            Assert.That(prim.MaximumExclusive, Is.Not.Null);
            Assert.That(prim.MaximumExclusive, Is.Empty);
            Assert.That(prim.MaximumInclusive, Is.Not.Null);
            Assert.That(prim.MaximumInclusive, Is.Empty);
            Assert.That(prim.MaximumLength, Is.Not.Null);
            Assert.That(prim.MaximumLength, Is.Empty);
            Assert.That(prim.MinimumExclusive, Is.Not.Null);
            Assert.That(prim.MinimumExclusive, Is.Empty);
            Assert.That(prim.MinimumInclusive, Is.Not.Null);
            Assert.That(prim.MinimumInclusive, Is.Empty);
            Assert.That(prim.MinimumLength, Is.Not.Null);
            Assert.That(prim.MinimumLength, Is.Empty);
            Assert.That(prim.Pattern, Is.Not.Null);
            Assert.That(prim.Pattern, Is.Empty);
            Assert.That(prim.TotalDigits, Is.Not.Null);
            Assert.That(prim.TotalDigits, Is.Empty);
            Assert.That(prim.UniqueIdentifier, Is.Not.Null);
            Assert.That(prim.UniqueIdentifier, Is.Empty);
            Assert.That(prim.VersionIdentifier, Is.Not.Null);
            Assert.That(prim.VersionIdentifier, Is.Empty);
            Assert.That(prim.WhiteSpace, Is.Not.Null);
            Assert.That(prim.WhiteSpace, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValues()
        {
            var umlDataType = new UmlDataTypeBuilder()
                .WithMultiValuedTaggedValue(TaggedValues.businessTerm, "businessTerm_Value1", "businessTerm_Value2")
                .WithTaggedValue(TaggedValues.definition, "definition_Value")
                .WithTaggedValue(TaggedValues.dictionaryEntryName, "dictionaryEntryName_Value")
                .WithTaggedValue(TaggedValues.fractionDigits, "fractionDigits_Value")
                .WithTaggedValue(TaggedValues.languageCode, "languageCode_Value")
                .WithTaggedValue(TaggedValues.length, "length_Value")
                .WithTaggedValue(TaggedValues.maximumExclusive, "maximumExclusive_Value")
                .WithTaggedValue(TaggedValues.maximumInclusive, "maximumInclusive_Value")
                .WithTaggedValue(TaggedValues.maximumLength, "maximumLength_Value")
                .WithTaggedValue(TaggedValues.minimumExclusive, "minimumExclusive_Value")
                .WithTaggedValue(TaggedValues.minimumInclusive, "minimumInclusive_Value")
                .WithTaggedValue(TaggedValues.minimumLength, "minimumLength_Value")
                .WithTaggedValue(TaggedValues.pattern, "pattern_Value")
                .WithTaggedValue(TaggedValues.totalDigits, "totalDigits_Value")
                .WithTaggedValue(TaggedValues.uniqueIdentifier, "uniqueIdentifier_Value")
                .WithTaggedValue(TaggedValues.versionIdentifier, "versionIdentifier_Value")
                .WithTaggedValue(TaggedValues.whiteSpace, "whiteSpace_Value")
                .Build();
            IPrim prim = new UpccPrim(umlDataType);
            Assert.That(prim.BusinessTerms, Is.EquivalentTo(new[] {"businessTerm_Value1", "businessTerm_Value2"}));
            Assert.That(prim.Definition, Is.EqualTo("definition_Value"));
            Assert.That(prim.DictionaryEntryName, Is.EqualTo("dictionaryEntryName_Value"));
            Assert.That(prim.FractionDigits, Is.EqualTo("fractionDigits_Value"));
            Assert.That(prim.LanguageCode, Is.EqualTo("languageCode_Value"));
            Assert.That(prim.Length, Is.EqualTo("length_Value"));
            Assert.That(prim.MaximumExclusive, Is.EqualTo("maximumExclusive_Value"));
            Assert.That(prim.MaximumInclusive, Is.EqualTo("maximumInclusive_Value"));
            Assert.That(prim.MaximumLength, Is.EqualTo("maximumLength_Value"));
            Assert.That(prim.MinimumExclusive, Is.EqualTo("minimumExclusive_Value"));
            Assert.That(prim.MinimumInclusive, Is.EqualTo("minimumInclusive_Value"));
            Assert.That(prim.MinimumLength, Is.EqualTo("minimumLength_Value"));
            Assert.That(prim.Pattern, Is.EqualTo("pattern_Value"));
            Assert.That(prim.TotalDigits, Is.EqualTo("totalDigits_Value"));
            Assert.That(prim.UniqueIdentifier, Is.EqualTo("uniqueIdentifier_Value"));
            Assert.That(prim.VersionIdentifier, Is.EqualTo("versionIdentifier_Value"));
            Assert.That(prim.WhiteSpace, Is.EqualTo("whiteSpace_Value"));
        }
    }
}
