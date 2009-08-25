using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
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
            root = new RepositoryItem(new RootRepositoryItemData());
            model = AddChild(root, ItemId.ItemType.Package);
            bLibrary = AddSubLibrary(model, Stereotype.bLibrary);
            elementLibraryData = new MyTestRepositoryItemData(model.Id, ItemId.ItemType.Package)
            {
                Stereotype = LibraryStereotype,
                TaggedValues = new Dictionary<TaggedValues, string>
                                              {
                                                  {TaggedValues.uniqueIdentifier, "foo"},
                                                  {TaggedValues.versionIdentifier, "foo"},
                                                  {TaggedValues.baseURN, "foo"},
                                              }
            };
            elementLibrary = AddChild(bLibrary, elementLibraryData);
        }

        #endregion

        private RepositoryItem root;
        private RepositoryItem model;
        private RepositoryItem bLibrary;
        private RepositoryItem elementLibrary;
        private MyTestRepositoryItemData elementLibraryData;

        [Test]
        public void ShouldOnlyAllowABLibraryAsParent()
        {
            VerifyConstraintViolations(elementLibrary);
            bLibrary.RemoveChild(elementLibrary.Id);
            model.AddOrReplaceChild(elementLibrary);
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseUrl()
        {
            elementLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.versionIdentifier, "foo"},
                                        };
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            elementLibraryData.Name = "";
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            elementLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.versionIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            elementLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyConstraintViolations(elementLibrary, elementLibrary.Id);
        }

        [Test]
        public void ShouldOnlyAllowElementsWithTheProperStereotype()
        {
            AddElement(elementLibrary, ElementStereotype);
            var element2 = AddElement(elementLibrary, "invalid_stereotype");
            VerifyConstraintViolations(elementLibrary, element2.Id);
        }

        [Test]
        public void ShouldNotAllowAnySubpackages()
        {
            var subPackage = AddSubLibrary(elementLibrary, Stereotype.bLibrary);
            VerifyConstraintViolations(elementLibrary, subPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchPackagesWithTheProperStereotype()
        {
            Assert.IsTrue(Validator().Matches(elementLibrary), "Element library validator does not match a proper element library.");
            Assert.IsFalse(Validator().Matches(model), "Element library validator matches a model.");
            Assert.IsFalse(Validator().Matches(AddChild(bLibrary, ItemId.ItemType.Element)), "Element library validator matches an element.");
        }

        protected override IValidator Validator()
        {
            return new ElementLibaryValidator(LibraryStereotype, ElementStereotype);
        }
    }
}