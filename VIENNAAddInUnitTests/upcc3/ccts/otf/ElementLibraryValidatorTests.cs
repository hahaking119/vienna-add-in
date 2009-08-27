using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.otf.validators;
using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class ElementLibraryValidatorTests : ValidatorTests
    {
        private const string LibraryStereotype = Stereotype.PRIMLibrary;
        private const string ElementStereotype = Stereotype.PRIM;

        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            root = APackageRepositoryItem.Build();
            model = AddChild(root, ItemId.ItemType.Package);
            bLibrary = AddSubLibrary(model, Stereotype.bLibrary);
            defaultElementLibrary = APackageRepositoryItem
                .WithParentId(model.Id)
                .WithItemType(ItemId.ItemType.Package)
                .WithStereotype(LibraryStereotype)
                .WithTaggedValues(new Dictionary<TaggedValues, string>
                                  {
                                      {TaggedValues.uniqueIdentifier, "foo"},
                                      {TaggedValues.versionIdentifier, "foo"},
                                      {TaggedValues.baseURN, "foo"},
                                  });
        }

        #endregion

        private RepositoryItem root;
        private RepositoryItem model;
        private RepositoryItem bLibrary;
        private RepositoryItemBuilder defaultElementLibrary;

        [Test]
        public void ShouldOnlyAllowABLibraryAsParent()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary);
            VerifyConstraintViolations(elementLibrary);
            bLibrary.RemoveChild(elementLibrary.Id);
            model.AddOrReplaceChild(elementLibrary);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseURN()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary.WithoutTaggedValue(TaggedValues.baseURN));
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary.WithName(string.Empty));
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary.WithoutTaggedValue(TaggedValues.uniqueIdentifier));
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary.WithoutTaggedValue(TaggedValues.versionIdentifier));
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldOnlyAllowElementsWithTheProperStereotype()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary);
            AddElement(elementLibrary, ElementStereotype);
            var element2 = AddElement(elementLibrary, "invalid_stereotype");
            VerifyConstraintViolations(elementLibrary, element2.Id);
        }

        [Test]
        public void ShouldNotAllowAnySubpackages()
        {
            var elementLibrary = AddChild(bLibrary, defaultElementLibrary);
            var subPackage = AddSubLibrary(elementLibrary, Stereotype.bLibrary);
            VerifyConstraintViolations(elementLibrary, subPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchPackagesWithTheProperStereotype()
        {
            Assert.IsTrue(Validator().Matches(AddChild(bLibrary, defaultElementLibrary)), "Element library validator does not match a proper element library.");
            Assert.IsFalse(Validator().Matches(model), "Element library validator matches a model.");
            Assert.IsFalse(Validator().Matches(AddChild(bLibrary, ItemId.ItemType.Element)), "Element library validator matches an element.");
        }

        protected override IValidator Validator()
        {
            return new ElementLibaryValidator(LibraryStereotype, ElementStereotype);
        }
    }
}