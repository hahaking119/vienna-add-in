using CctsRepository.PrimLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    [TestFixture]
    public partial class UpccPrimTests
    {

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueBusinessTermsIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.BusinessTerms, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueBusinessTerms()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithMultiValuedTaggedValue(TaggedValues.businessTerm, "businessTerm_Value1", "businessTerm_Value2")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.BusinessTerms, Is.EquivalentTo(new[] {"businessTerm_Value1", "businessTerm_Value2"}));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueDefinitionIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Definition, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueDefinition()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.definition, "definition_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Definition, Is.EqualTo("definition_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueDictionaryEntryNameIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.DictionaryEntryName, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueDictionaryEntryName()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.dictionaryEntryName, "dictionaryEntryName_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.DictionaryEntryName, Is.EqualTo("dictionaryEntryName_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueFractionDigitsIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.FractionDigits, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueFractionDigits()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.fractionDigits, "fractionDigits_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.FractionDigits, Is.EqualTo("fractionDigits_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueLanguageCodeIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.LanguageCode, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueLanguageCode()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.languageCode, "languageCode_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.LanguageCode, Is.EqualTo("languageCode_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueLengthIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Length, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueLength()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.length, "length_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Length, Is.EqualTo("length_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueMaximumExclusiveIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MaximumExclusive, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueMaximumExclusive()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.maximumExclusive, "maximumExclusive_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MaximumExclusive, Is.EqualTo("maximumExclusive_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueMaximumInclusiveIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MaximumInclusive, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueMaximumInclusive()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.maximumInclusive, "maximumInclusive_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MaximumInclusive, Is.EqualTo("maximumInclusive_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueMaximumLengthIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MaximumLength, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueMaximumLength()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.maximumLength, "maximumLength_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MaximumLength, Is.EqualTo("maximumLength_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueMinimumExclusiveIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MinimumExclusive, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueMinimumExclusive()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.minimumExclusive, "minimumExclusive_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MinimumExclusive, Is.EqualTo("minimumExclusive_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueMinimumInclusiveIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MinimumInclusive, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueMinimumInclusive()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.minimumInclusive, "minimumInclusive_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MinimumInclusive, Is.EqualTo("minimumInclusive_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueMinimumLengthIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MinimumLength, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueMinimumLength()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.minimumLength, "minimumLength_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.MinimumLength, Is.EqualTo("minimumLength_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValuePatternIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Pattern, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValuePattern()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.pattern, "pattern_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Pattern, Is.EqualTo("pattern_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueTotalDigitsIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.TotalDigits, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueTotalDigits()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.totalDigits, "totalDigits_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.TotalDigits, Is.EqualTo("totalDigits_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueUniqueIdentifierIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.UniqueIdentifier, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueUniqueIdentifier()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.uniqueIdentifier, "uniqueIdentifier_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.UniqueIdentifier, Is.EqualTo("uniqueIdentifier_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueVersionIdentifierIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.VersionIdentifier, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueVersionIdentifier()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.versionIdentifier, "versionIdentifier_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.VersionIdentifier, Is.EqualTo("versionIdentifier_Value"));
        }

        [Test]
        public void ShouldReturnAnEmptyStringIfTaggedValueWhiteSpaceIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder().Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.WhiteSpace, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValueWhiteSpace()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithTaggedValue(TaggedValues.whiteSpace, "whiteSpace_Value")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.WhiteSpace, Is.EqualTo("whiteSpace_Value"));
        }
    }
}
