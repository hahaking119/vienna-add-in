using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.otf.validators;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    [TestFixture]
    public class PRIMValidatorTests : ValidatorTests
    {
        private const string LibraryStereotype = Stereotype.PRIMLibrary;
        private const string ElementStereotype = Stereotype.PRIM;

        private static readonly TaggedValues[] DefinedTaggedValues = new[]
                                                                     {
                                                                         TaggedValues.businessTerm,
                                                                         TaggedValues.dictionaryEntryName,
                                                                         TaggedValues.pattern,
                                                                         TaggedValues.fractionDigits,
                                                                         TaggedValues.length,
                                                                         TaggedValues.maxExclusive,
                                                                         TaggedValues.maxInclusive,
                                                                         TaggedValues.maxLength,
                                                                         TaggedValues.minExclusive,
                                                                         TaggedValues.minInclusive,
                                                                         TaggedValues.minLength,
                                                                         TaggedValues.totalDigits,
                                                                         TaggedValues.whiteSpace,
                                                                         TaggedValues.uniqueIdentifier,
                                                                         TaggedValues.versionIdentifier,
                                                                         TaggedValues.languageCode,
                                                                     };

        private static readonly TaggedValues[] RequiredTaggedValues = new[]
                                                                      {
                                                                          TaggedValues.definition,
                                                                      };

        public PRIMValidatorTests() : base(DefinedTaggedValues, RequiredTaggedValues)
        {
        }

        protected override IValidator Validator()
        {
            return new PRIMValidator();
        }

        protected override RepositoryItemBuilder DefaultItem
        {
            get
            {
                return AnElement
                    .WithStereotype(ElementStereotype)
                    .WithParent(APackage.WithStereotype(LibraryStereotype))
                    .WithTaggedValues(new Dictionary<TaggedValues, string>
                                      {
                                          {TaggedValues.definition, "foo"},
                                          {TaggedValues.businessTerm, ""},
                                          {TaggedValues.dictionaryEntryName, ""},
                                          {TaggedValues.pattern, ""},
                                          {TaggedValues.fractionDigits, ""},
                                          {TaggedValues.length, ""},
                                          {TaggedValues.maxExclusive, ""},
                                          {TaggedValues.maxInclusive, ""},
                                          {TaggedValues.maxLength, ""},
                                          {TaggedValues.minExclusive, ""},
                                          {TaggedValues.minInclusive, ""},
                                          {TaggedValues.minLength, ""},
                                          {TaggedValues.totalDigits, ""},
                                          {TaggedValues.whiteSpace, ""},
                                          {TaggedValues.uniqueIdentifier, ""},
                                          {TaggedValues.versionIdentifier, ""},
                                          {TaggedValues.languageCode, ""},
                                      });
            }
        }

        private void AssertValidatorDoesNotMatch(RepositoryItemBuilder repositoryItemBuilder)
        {
            RepositoryItem repositoryItem = repositoryItemBuilder.Build();
            Assert.IsFalse(Validator().Matches(repositoryItem), ElementStereotype + " validator wrongly matches a " + repositoryItem.Id.Type + " with stereotype " + repositoryItem.Stereotype + ".");
        }

        [Test]
        public void ShouldNotMatchPackages()
        {
            AssertValidatorDoesNotMatch(APackage.WithStereotype(ElementStereotype));
        }

        [Test]
        public void ShouldOnlyAllowAPRIMLibraryAsParent()
        {
            RepositoryItemBuilder prim = DefaultItem.WithParent(APackage.WithStereotype("other than " + LibraryStereotype));
            VerifyConstraintViolations(prim, prim);
        }

        [Test]
        public void ShouldOnlyMatchElementsWithTheProperStereotype()
        {
            AssertValidatorDoesNotMatch(AnElement.WithStereotype("other than " + ElementStereotype));
        }
    }
}