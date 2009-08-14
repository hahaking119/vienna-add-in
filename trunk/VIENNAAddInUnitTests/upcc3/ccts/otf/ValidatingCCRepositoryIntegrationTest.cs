using System;
using System.Collections.Generic;
using EA;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    /// <summary>
    /// These are tests for a complete ValidatingCCRepository, so in fact they are testing the integration of
    /// the various components of the ValidatingCCRepository.
    /// </summary>
    [TestFixture]
    public class ValidatingCCRepositoryIntegrationTest
    {
        private EARepository eaRepository;
        private Package bLibrary;
        private ValidationIssueHandler issueHandler;
        private ValidatingCCRepository validatingCCRepository;

        [Test]
        public void When_a_package_is_loaded_Then_it_should_be_in_the_repository()
        {
            var newBLibrary = bLibrary.AddPackage("New BLibrary", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);
            var bLibraries = new List<IBLibrary>(validatingCCRepository.Libraries<IBLibrary>());
            Assert.AreEqual(2, bLibraries.Count, "Wrong number of BLibraries");
            Assert.AreEqual(bLibrary.PackageID, bLibraries[0].Id);
            Assert.AreEqual(newBLibrary.PackageID, bLibraries[1].Id);
        }

        [Test]
        public void When_a_package_is_loaded_Then_the_package_should_be_validated()
        {
            var newBLibrary = bLibrary.AddPackage("New BLibrary", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);
            // expect issues for missing tagged values
            issueHandler.AssertReceivedIssuesTotal(3);
            issueHandler.AssertReceivedIssues(3, newBLibrary.PackageID, newBLibrary.PackageID);
        }

        [Test]
        public void When_a_package_is_loaded_Then_its_parent_package_should_be_validated()
        {
            var subPackage = bLibrary.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = "other package"; });
            validatingCCRepository.LoadPackageByID(subPackage.PackageID);
            // expect one issue for invalid sub-package
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, bLibrary.PackageID, subPackage.PackageID);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_validation_issues_should_be_removed()
        {
            var newBLibrary = bLibrary.AddPackage("New BLibrary", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);
            validatingCCRepository.ItemDeleted(ItemId.ForPackage(newBLibrary.PackageID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_descendants_validation_issues_should_be_removed()
        {
            var bLibraryWithoutIssues = bLibrary.AddPackage("BLibrary2", p =>
            {
                p.Element.Stereotype = Stereotype.bLibrary;
                p.AddTaggedValue(TaggedValues.uniqueIdentifier.ToString()).WithValue("foo");
                p.AddTaggedValue(TaggedValues.versionIdentifier.ToString()).WithValue("foo");
                p.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("foo");
            });
            validatingCCRepository.LoadPackageByID(bLibraryWithoutIssues.PackageID);

            var bLibraryWithIssues = bLibraryWithoutIssues.AddPackage("BLibrary3", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(bLibraryWithIssues.PackageID);
            // expect issues for missing tagged values
            issueHandler.AssertReceivedIssuesTotal(3);
            issueHandler.AssertReceivedIssues(3, bLibraryWithIssues.PackageID, bLibraryWithIssues.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(bLibraryWithoutIssues.PackageID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_parent_package_should_be_validated()
        {
            var subPackage = bLibrary.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = "other package"; });
            validatingCCRepository.LoadPackageByID(subPackage.PackageID);
            validatingCCRepository.ItemDeleted(ItemId.ForPackage(subPackage.PackageID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_a_package_is_deleted_Then_the_package_should_no_longer_be_in_the_repository()
        {
            var newBLibrary = bLibrary.AddPackage("New BLibrary", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);
            validatingCCRepository.ItemDeleted(ItemId.ForPackage(newBLibrary.PackageID));
            var bLibraries = new List<IBLibrary>(validatingCCRepository.Libraries<IBLibrary>());
            Assert.AreEqual(1, bLibraries.Count, "Wrong number of BLibraries");
            Assert.AreEqual(bLibrary.PackageID, bLibraries[0].Id);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_descendants_should_no_longer_be_in_the_repository()
        {
            var newBLibrary1 = bLibrary.AddPackage("New BLibrary 1", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(newBLibrary1.PackageID);
            var newBLibrary2 = newBLibrary1.AddPackage("New BLibrary 2", p => { p.Element.Stereotype = Stereotype.bLibrary; });
            validatingCCRepository.LoadPackageByID(newBLibrary2.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(newBLibrary1.PackageID));

            var bLibraries = new List<IBLibrary>(validatingCCRepository.Libraries<IBLibrary>());
            Assert.AreEqual(1, bLibraries.Count, "Wrong number of BLibraries");
            Assert.AreEqual(bLibrary.PackageID, bLibraries[0].Id);
        }

        [Test]
        public void When_an_element_is_loaded_Then_it_should_be_in_the_repository()
        {
            Assert.Ignore("Cannot be tested, because no element libraries are implemented yet.");
        }

        [Test]
        public void When_an_element_is_loaded_Then_it_should_be_validated()
        {
            Assert.Ignore("Cannot be tested, because no element validations are implemented yet.");
        }

        [Test]
        public void When_an_element_is_loaded_Then_its_package_should_be_validated()
        {
            var element = bLibrary.AddClass("An element");
            validatingCCRepository.LoadElementByID(element.ElementID);
            // expect one issue for invalid element
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, bLibrary.PackageID, element.ElementID);
        }

        [Test]
        public void When_an_element_is_deleted_Then_its_validation_issues_should_be_removed()
        {
            Assert.Ignore("Cannot be tested, because no element validations are implemented yet.");
        }

        [Test]
        public void When_an_element_is_deleted_Then_its_package_should_be_validated()
        {
            var element = bLibrary.AddClass("An element");
            validatingCCRepository.LoadElementByID(element.ElementID);
            validatingCCRepository.ItemDeleted(ItemId.ForElement(element.ElementID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_an_element_is_deleted_Then_it_should_no_longer_be_in_the_repository()
        {
            Assert.Ignore("Cannot be tested, because no element libraries are implemented yet.");
        }

        [SetUp]
        public void Context()
        {
            eaRepository = new EARepository();
            bLibrary = null;
            eaRepository.AddModel("Model",
                                  model =>
                                  {
                                      bLibrary = model.AddPackage("BLibrary", p =>
                                                                              {
                                                                                  p.Element.Stereotype = Stereotype.bLibrary;
                                                                                  p.AddTaggedValue(TaggedValues.uniqueIdentifier.ToString()).WithValue("foo");
                                                                                  p.AddTaggedValue(TaggedValues.versionIdentifier.ToString()).WithValue("foo");
                                                                                  p.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("foo");
                                                                              });
                                  });

            validatingCCRepository = new ValidatingCCRepository(eaRepository);
            issueHandler = new ValidationIssueHandler();
            validatingCCRepository.ValidationIssuesUpdated += issueHandler.ValidationIssuesUpdated;
            validatingCCRepository.LoadRepositoryContent();
            issueHandler.AssertReceivedIssuesTotal(0);
            issueHandler.Reset();
        }
    }

    public class ValidationIssueHandler
    {
        private List<IValidationIssue> issues;

        public void ValidationIssuesUpdated(IEnumerable<IValidationIssue> updatedIssues)
        {
            issues = new List<IValidationIssue>(updatedIssues);
            Console.WriteLine("received issues:");
            foreach (var issue in updatedIssues)
            {
                Console.WriteLine("  " + issue.Message);
            }
        }

        public void AssertReceivedIssuesTotal(int count)
        {
            Assert.IsNotNull(issues, "No issues have been received, but expected a total number of " + count + " issues.");
            Assert.AreEqual(count, issues.Count, "Wrong total number of issues received.");
        }

        public void AssertReceivedIssues(int count, int validatedItemId, int itemId)
        {
            Assert.AreEqual(count, issues.FindAll(AnIssueWith(itemId, validatedItemId)).Count, "The expected number of issues with ItemId=" + itemId + " and ValidatedItemId=" + validatedItemId + " has not been received.");
        }

        private Predicate<IValidationIssue> AnIssueWith(int itemId, int validatedItemId)
        {
            return issue => issue.ItemId.Value == itemId && issue.ValidatedItemId.Value == validatedItemId;
        }

        public void Reset()
        {
            issues = null;
        }
    }
}