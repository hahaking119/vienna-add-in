using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class PRIMLibraryConstraintsTests : ConstraintsTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            root = new RepositoryItem(new RootRepositoryItemData());
            model = AddChild(root, ItemId.ItemType.Package);
            bLibrary = AddSubLibrary(model, Stereotype.BLibrary);
            primLibraryData = new MyTestRepositoryItemData(model.Id, ItemId.ItemType.Package)
            {
                Stereotype = Stereotype.PRIMLibrary,
                TaggedValues = new Dictionary<TaggedValues, string>
                                              {
                                                  {TaggedValues.uniqueIdentifier, "foo"},
                                                  {TaggedValues.versionIdentifier, "foo"},
                                                  {TaggedValues.baseURN, "foo"},
                                              }
            };
            primLibrary = AddChild(bLibrary, primLibraryData);
        }

        #endregion

        private RepositoryItem root;
        private RepositoryItem model;
        private RepositoryItem bLibrary;
        private RepositoryItem primLibrary;
        private MyTestRepositoryItemData primLibraryData;

        [Test]
        public void ShouldOnlyAllowABLibraryAsParent()
        {
            VerifyValidationIssues(primLibrary);
            bLibrary.RemoveChild(primLibrary.Id);
            model.AddOrReplaceChild(primLibrary);
            VerifyValidationIssues(primLibrary, primLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyBaseUrl()
        {
            primLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.versionIdentifier, "foo"},
                                        };
            VerifyValidationIssues(primLibrary, primLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            primLibraryData.Name = "";
            VerifyValidationIssues(primLibrary, primLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyUniqueIdentifier()
        {
            primLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.versionIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyValidationIssues(primLibrary, primLibrary.Id);
        }

        [Test]
        public void ShouldNotAllowAnEmptyVersionIdentifier()
        {
            primLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyValidationIssues(primLibrary, primLibrary.Id);
        }

        [Test]
        public void ShouldOnlyAllowPRIMElements()
        {
            AddElement(primLibrary, Stereotype.PRIM);
            var element2 = AddElement(primLibrary, "stereotype not allowed for elements in this library");
            VerifyValidationIssues(primLibrary, element2.Id);
        }

        [Test]
        public void ShouldNotAllowAnySubpackages()
        {
            var subPackage = AddSubLibrary(primLibrary, Stereotype.BLibrary);
            VerifyValidationIssues(primLibrary, subPackage.Id);
        }

        [Test]
        public void ShouldOnlyMatchPRIMLibraries()
        {
            Assert.IsTrue(new PRIMLibraryConstraints().Matches(primLibrary), "Constraints do not match matching item.");
            Assert.IsFalse(new PRIMLibraryConstraints().Matches(model), "Constraints match unmatching item.");
            Assert.IsFalse(new PRIMLibraryConstraints().Matches(AddChild(bLibrary, ItemId.ItemType.Element)), "Constraints match unmatching item.");
        }

        protected override IConstraint Constraints()
        {
            return new PRIMLibraryConstraints();
        }
    }
}