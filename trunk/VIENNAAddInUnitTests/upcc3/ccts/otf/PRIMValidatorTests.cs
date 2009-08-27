using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.otf.validators;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class PRIMValidatorTests : ValidatorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            RepositoryItem root = APackageRepositoryItem.Build();
            RepositoryItem model = AddChild(root, ItemId.ItemType.Package);
            bLibrary = AddSubLibrary(model, Stereotype.bLibrary);
            primLibrary = AddSubLibrary(bLibrary, Stereotype.PRIMLibrary);
            defaultPRIM = ARepositoryItem
                .WithParentId(primLibrary.Id)
                .WithItemType(ItemId.ItemType.Element)
                .WithStereotype(Stereotype.PRIM)
                .WithTaggedValues(new Dictionary<TaggedValues, string>
                                  {
                                      {TaggedValues.businessTerm, ""},
                                      {TaggedValues.definition, "foo"},
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

        #endregion

        private RepositoryItem bLibrary;
        private RepositoryItem primLibrary;
        private RepositoryItemBuilder defaultPRIM;

        protected override IValidator Validator()
        {
            return new PRIMValidator();
        }

        private void ShouldNotAllowUndefinedTaggedValue(TaggedValues taggedValue)
        {
            var prim = AddChild(primLibrary, defaultPRIM.WithoutTaggedValue(taggedValue));
            VerifyConstraintViolations(prim, prim.Id);
            primLibrary.RemoveChild(prim.Id);
        }

        private void ShouldNotAllowEmptyTaggedValue(TaggedValues taggedValue)
        {
            var prim = AddChild(primLibrary, defaultPRIM.WithTaggedValue(taggedValue, string.Empty));
            VerifyConstraintViolations(prim, prim.Id);
            primLibrary.RemoveChild(prim.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            var prim = AddChild(primLibrary, defaultPRIM.WithName(string.Empty));
            VerifyConstraintViolations(prim, prim.Id);
        }

        [Test]
        public void ShouldNotAllowEmptyTaggedValues()
        {
            ShouldNotAllowEmptyTaggedValue(TaggedValues.definition);
        }

        [Test]
        public void ShouldNotAllowUndefinedTaggedValues()
        {
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.businessTerm);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.definition);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.dictionaryEntryName);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.pattern);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.fractionDigits);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.length);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.maxExclusive);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.maxInclusive);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.maxLength);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.minExclusive);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.minInclusive);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.minLength);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.totalDigits);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.whiteSpace);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.uniqueIdentifier);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.versionIdentifier);
            ShouldNotAllowUndefinedTaggedValue(TaggedValues.languageCode);
        }

        [Test]
        public void ShouldOnlyAllowAPRIMLibraryAsParent()
        {
            RepositoryItem prim = AddChild(primLibrary, defaultPRIM);
            VerifyConstraintViolations(prim);

            primLibrary.RemoveChild(prim.Id);
            bLibrary.AddOrReplaceChild(prim);
            VerifyConstraintViolations(prim, prim.Id);
        }

        [Test]
        public void ShouldOnlyMatchElementsWithThePRIMStereotype()
        {
            Assert.IsTrue(Validator().Matches(AddChild(primLibrary, defaultPRIM)), "PRIM validator does not match a proper PRIM element.");
            Assert.IsFalse(Validator().Matches(AddChild(primLibrary, ItemId.ItemType.Element)), "PRIM validator matches an element without a PRIM stereotype.");
            Assert.IsFalse(Validator().Matches(bLibrary), "PRIM validator matches a bLibrary.");
        }
    }
}