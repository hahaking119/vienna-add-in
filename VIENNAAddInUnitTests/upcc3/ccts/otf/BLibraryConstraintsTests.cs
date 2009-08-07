using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class BLibraryConstraintsTests : ConstraintsTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            root = new RepositoryItem(new RootRepositoryItemData());
            model = AddChild(root, ItemId.ItemType.Package);
            bLibraryData = new MyTestRepositoryItemData(model.Id, ItemId.ItemType.Package)
                           {
                               Stereotype = Stereotype.BLibrary,
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
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void ShouldAllowABLibraryAsParent()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, Stereotype.BLibrary);
            parent.AddOrReplaceChild(bLibrary);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void ShouldAllowAModelAsParent()
        {
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseUrl()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.versionIdentifier, "foo"},
                                        };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            bLibraryData.Name = "";
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.versionIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnInvalidParent()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, "Something else");
            parent.AddOrReplaceChild(bLibrary);
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowElementsInABLibrary()
        {
            RepositoryItem element1 = AddChild(bLibrary, ItemId.ItemType.Element);
            RepositoryItem element2 = AddChild(bLibrary, ItemId.ItemType.Element);
            VerifyValidationIssues(bLibrary, element1.Id, element2.Id);
        }

        [Test]
        public void ShouldOnlyAllowBusinessLibrariesAsSubpackages()
        {
            AddSubLibrary(bLibrary, Stereotype.BLibrary);
            AddSubLibrary(bLibrary, Stereotype.PRIMLibrary);
            AddSubLibrary(bLibrary, Stereotype.ENUMLibrary);
            AddSubLibrary(bLibrary, Stereotype.CDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.CCLibrary);
            AddSubLibrary(bLibrary, Stereotype.BDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.BIELibrary);
            AddSubLibrary(bLibrary, Stereotype.DOCLibrary);
            RepositoryItem invalidSubPackage = AddChild(bLibrary, ItemId.ItemType.Package);
            VerifyValidationIssues(bLibrary, invalidSubPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchBLibraries()
        {
            Assert.IsTrue(new BLibraryConstraints().Matches(bLibrary), "Constraints do not match matching item.");
            Assert.IsFalse(new BLibraryConstraints().Matches(model), "Constraints match unmatching item.");
            Assert.IsFalse(new BLibraryConstraints().Matches(AddChild(bLibrary, ItemId.ItemType.Element)), "Constraints match unmatching item.");
        }

        protected override IConstraint Constraints()
        {
            return new BLibraryConstraints();
        }
    }
}