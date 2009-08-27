using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.otf.validators;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class BLibraryValidatorTests : ValidatorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            root = APackageRepositoryItem.Build();
            model = AddChild(root, ItemId.ItemType.Package);
            defaultBLibrary = APackageRepositoryItem
                .WithParentId(model.Id)
                .WithItemType(ItemId.ItemType.Package)
                .WithStereotype(Stereotype.bLibrary)
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
        private RepositoryItemBuilder defaultBLibrary;

        [Test]
        public void ShouldAllowABInformationVAsParent()
        {
            var bLibrary = AddChild(AddSubLibrary(model, Stereotype.BInformationV), defaultBLibrary);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldAllowABLibraryAsParent()
        {
            var bLibrary = AddChild(AddSubLibrary(model, Stereotype.bLibrary), defaultBLibrary);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldAllowAModelAsParent()
        {
            var bLibrary = AddChild(model, defaultBLibrary);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseURN()
        {
            var bLibrary = AddChild(model, defaultBLibrary.WithoutTaggedValue(TaggedValues.baseURN));
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            var bLibrary = AddChild(model, defaultBLibrary.WithName(string.Empty));
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            var bLibrary = AddChild(model, defaultBLibrary.WithoutTaggedValue(TaggedValues.uniqueIdentifier));
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            var bLibrary = AddChild(model, defaultBLibrary.WithoutTaggedValue(TaggedValues.versionIdentifier));
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnInvalidParent()
        {
            var bLibrary = AddChild(AddSubLibrary(model, "SOME-OTHER-KIND-OF-PACKAGE"), defaultBLibrary);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowElementsInABLibrary()
        {
            var bLibrary = AddChild(model, defaultBLibrary);
            var element1 = AddChild(bLibrary, ItemId.ItemType.Element);
            var element2 = AddChild(bLibrary, ItemId.ItemType.Element);
            VerifyConstraintViolations(bLibrary, element1.Id, element2.Id);
        }

        [Test]
        public void ShouldOnlyAllowBusinessLibrariesAsSubpackages()
        {
            var bLibrary = AddChild(model, defaultBLibrary);
            AddSubLibrary(bLibrary, Stereotype.bLibrary);
            AddSubLibrary(bLibrary, Stereotype.PRIMLibrary);
            AddSubLibrary(bLibrary, Stereotype.ENUMLibrary);
            AddSubLibrary(bLibrary, Stereotype.CDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.CCLibrary);
            AddSubLibrary(bLibrary, Stereotype.BDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.BIELibrary);
            AddSubLibrary(bLibrary, Stereotype.DOCLibrary);
            var invalidSubPackage = AddChild(bLibrary, ItemId.ItemType.Package);
            VerifyConstraintViolations(bLibrary, invalidSubPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchBLibraries()
        {
            Assert.IsTrue(new BLibraryValidator().Matches(AddChild(model, defaultBLibrary)), "Validator do not match matching item.");
            Assert.IsFalse(new BLibraryValidator().Matches(model), "Validator match unmatching item.");
            Assert.IsFalse(new BLibraryValidator().Matches(AddChild(AddChild(model, defaultBLibrary), ItemId.ItemType.Element)), "Validator match unmatching item.");
        }

        protected override IValidator Validator()
        {
            return new BLibraryValidator();
        }
    }
}