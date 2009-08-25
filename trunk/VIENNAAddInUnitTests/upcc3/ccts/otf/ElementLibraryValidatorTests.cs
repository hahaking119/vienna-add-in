using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.otf.validators;
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
            root = ARepositoryItem.Build();
            model = AddChild(root, ItemId.ItemType.Package);
            bLibrary = AddSubLibrary(model, Stereotype.bLibrary);
            elementLibraryBuilder = ARepositoryItem
                .WithParentId(model.Id)
                .WithItemType(ItemId.ItemType.Package)
                .WithStereotype(LibraryStereotype)
                .WithTaggedValues(new Dictionary<string, string>
                                  {
                                      {TaggedValues.uniqueIdentifier.ToString(), "foo"},
                                      {TaggedValues.versionIdentifier.ToString(), "foo"},
                                      {TaggedValues.baseURN.ToString(), "foo"},
                                  });
        }

        #endregion

        private RepositoryItem root;
        private RepositoryItem model;
        private RepositoryItem bLibrary;
        private RepositoryItemBuilder elementLibraryBuilder;

        [Test]
        public void ShouldOnlyAllowABLibraryAsParent()
        {
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            VerifyConstraintViolations(elementLibrary);
            bLibrary.RemoveChild(elementLibrary.Id);
            model.AddOrReplaceChild(elementLibrary);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseUrl()
        {
            elementLibraryBuilder.WithTaggedValues(new Dictionary<string, string>
                                        {
                                            {TaggedValues.uniqueIdentifier.ToString(), "foo"},
                                            {TaggedValues.versionIdentifier.ToString(), "foo"},
                                        });
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            elementLibraryBuilder.WithName(string.Empty);
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            elementLibraryBuilder.WithTaggedValues(new Dictionary<string, string>
                                        {
                                            {TaggedValues.versionIdentifier.ToString(), "foo"},
                                            {TaggedValues.baseURN.ToString(), "foo"},
                                        });
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            elementLibraryBuilder.WithTaggedValues(new Dictionary<string, string>
                                        {
                                            {TaggedValues.uniqueIdentifier.ToString(), "foo"},
                                            {TaggedValues.baseURN.ToString(), "foo"},
                                        });
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldOnlyAllowElementsWithTheProperStereotype()
        {
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            AddElement(elementLibrary, ElementStereotype);
            var element2 = AddElement(elementLibrary, "invalid_stereotype");
            VerifyConstraintViolations(elementLibrary, element2.Id);
        }

        [Test]
        public void ShouldNotAllowAnySubpackages()
        {
            var elementLibrary = AddChild(bLibrary, elementLibraryBuilder);
            var subPackage = AddSubLibrary(elementLibrary, Stereotype.bLibrary);
            VerifyConstraintViolations(elementLibrary, subPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchPackagesWithTheProperStereotype()
        {
            Assert.IsTrue(Validator().Matches(AddChild(bLibrary, elementLibraryBuilder)), "Element library validator does not match a proper element library.");
            Assert.IsFalse(Validator().Matches(model), "Element library validator matches a model.");
            Assert.IsFalse(Validator().Matches(AddChild(bLibrary, ItemId.ItemType.Element)), "Element library validator matches an element.");
        }

        protected override IValidator Validator()
        {
            return new ElementLibaryValidator(LibraryStereotype, ElementStereotype);
        }
    }
}