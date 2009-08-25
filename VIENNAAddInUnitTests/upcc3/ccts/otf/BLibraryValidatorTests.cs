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
            root = new RepositoryItem(new RootRepositoryItemData());
            model = AddChild(root, ItemId.ItemType.Package);
            bLibraryData = new MyTestRepositoryItemData(model.Id, ItemId.ItemType.Package)
                           {
                               Stereotype = Stereotype.bLibrary,
                               TaggedValues = new Dictionary<TaggedValues, string>
                                              {
                                                  {TaggedValues.uniqueIdentifier, "foo"},
                                                  {TaggedValues.versionIdentifier, "foo"},
                                                  {TaggedValues.baseURN, "foo"},
                                              }
                           };
            bLibrary = AddChild(model, bLibraryData);
        }

        #endregion

        private RepositoryItem root;
        private RepositoryItem model;
        private RepositoryItem bLibrary;
        private MyTestRepositoryItemData bLibraryData;

        [Test]
        public void ShouldAllowABInformationVAsParent()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, Stereotype.BInformationV);
            parent.AddOrReplaceChild(bLibrary);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldAllowABLibraryAsParent()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, Stereotype.bLibrary);
            parent.AddOrReplaceChild(bLibrary);
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldAllowAModelAsParent()
        {
            VerifyConstraintViolations(bLibrary);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseUrl()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.versionIdentifier, "foo"},
                                        };
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            bLibraryData.Name = "";
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.versionIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnInvalidParent()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, "Something else");
            parent.AddOrReplaceChild(bLibrary);
            VerifyConstraintViolations(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowElementsInABLibrary()
        {
            RepositoryItem element1 = AddChild(bLibrary, ItemId.ItemType.Element);
            RepositoryItem element2 = AddChild(bLibrary, ItemId.ItemType.Element);
            VerifyConstraintViolations(bLibrary, element1.Id, element2.Id);
        }

        [Test]
        public void ShouldOnlyAllowBusinessLibrariesAsSubpackages()
        {
            AddSubLibrary(bLibrary, Stereotype.bLibrary);
            AddSubLibrary(bLibrary, Stereotype.PRIMLibrary);
            AddSubLibrary(bLibrary, Stereotype.ENUMLibrary);
            AddSubLibrary(bLibrary, Stereotype.CDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.CCLibrary);
            AddSubLibrary(bLibrary, Stereotype.BDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.BIELibrary);
            AddSubLibrary(bLibrary, Stereotype.DOCLibrary);
            RepositoryItem invalidSubPackage = AddChild(bLibrary, ItemId.ItemType.Package);
            VerifyConstraintViolations(bLibrary, invalidSubPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchBLibraries()
        {
            Assert.IsTrue(new BLibraryValidator().Matches(bLibrary), "Validator do not match matching item.");
            Assert.IsFalse(new BLibraryValidator().Matches(model), "Validator match unmatching item.");
            Assert.IsFalse(new BLibraryValidator().Matches(AddChild(bLibrary, ItemId.ItemType.Element)), "Validator match unmatching item.");
        }

        protected override IValidator Validator()
        {
            return new BLibraryValidator();
        }
    }
}