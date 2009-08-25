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
            root = ARepositoryItem.Build();
            model = AddChild(root, ItemId.ItemType.Package);
            bLibraryBuilder = ARepositoryItem
                .WithParentId(model.Id)
                .WithItemType(ItemId.ItemType.Package)
                .WithStereotype(Stereotype.bLibrary)
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
        private RepositoryItemBuilder bLibraryBuilder;

        [Test]
        public void ShouldAllowABInformationVAsParent()
        {
            var bLibrary = AddChild(AddSubLibrary(model, Stereotype.BInformationV), bLibraryBuilder);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldAllowABLibraryAsParent()
        {
            var bLibrary = AddChild(AddSubLibrary(model, Stereotype.bLibrary), bLibraryBuilder);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldAllowAModelAsParent()
        {
            var bLibrary = AddChild(model, bLibraryBuilder);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseUrl()
        {
            bLibraryBuilder.WithTaggedValues(new Dictionary<string, string>
                                        {
                                            {TaggedValues.uniqueIdentifier.ToString(), "foo"},
                                            {TaggedValues.versionIdentifier.ToString(), "foo"},
                                        });
            var bLibrary = AddChild(model, bLibraryBuilder);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            bLibraryBuilder.WithName(string.Empty);
            var bLibrary = AddChild(model, bLibraryBuilder);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            bLibraryBuilder.WithTaggedValues(new Dictionary<string, string>
                                        {
                                            {TaggedValues.versionIdentifier.ToString(), "foo"},
                                            {TaggedValues.baseURN.ToString(), "foo"},
                                        });
            var bLibrary = AddChild(model, bLibraryBuilder);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            bLibraryBuilder.WithTaggedValues(new Dictionary<string, string>
                                        {
                                            {TaggedValues.uniqueIdentifier.ToString(), "foo"},
                                            {TaggedValues.baseURN.ToString(), "foo"},
                                        });
            var bLibrary = AddChild(model, bLibraryBuilder);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnInvalidParent()
        {
            var bLibrary = AddChild(AddSubLibrary(model, "SOME-OTHER-KIND-OF-PACKAGE"), bLibraryBuilder);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowElementsInABLibrary()
        {
            var bLibrary = AddChild(model, bLibraryBuilder);
            var element1 = AddChild(bLibrary, ItemId.ItemType.Element);
            var element2 = AddChild(bLibrary, ItemId.ItemType.Element);
            VerifyConstraintViolations(bLibrary, element1.Id, element2.Id);
        }

        [Test]
        public void ShouldOnlyAllowBusinessLibrariesAsSubpackages()
        {
            var bLibrary = AddChild(model, bLibraryBuilder);
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
            Assert.IsTrue(new BLibraryValidator().Matches(AddChild(model, bLibraryBuilder)), "Validator do not match matching item.");
            Assert.IsFalse(new BLibraryValidator().Matches(model), "Validator match unmatching item.");
            Assert.IsFalse(new BLibraryValidator().Matches(AddChild(AddChild(model, bLibraryBuilder), ItemId.ItemType.Element)), "Validator match unmatching item.");
        }

        protected override IValidator Validator()
        {
            return new BLibraryValidator();
        }
    }
}