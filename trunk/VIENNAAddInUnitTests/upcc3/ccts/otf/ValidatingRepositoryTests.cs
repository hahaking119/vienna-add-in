using System;
using System.Collections.Generic;
using System.Linq;
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
    [TestFixture]
    public class RepositoryContentLoaderTest
    {
        private readonly EARepository eaRepository;
        private readonly Package model;
        private readonly Package otherPackage;
        private readonly Package bInformationV;
        private readonly Package bLibrary;
        private readonly Package primLibrary;
        private readonly Package enumLibrary;
        private readonly Package cdtLibrary;
        private readonly Package ccLibrary;
        private readonly Package bdtLibrary;
        private readonly Package bieLibrary;
        private readonly Package docLibrary;
        private readonly Element otherElement;
        private readonly RepositoryContentLoader contentLoader;
        private IEAPackage loadedPackage;
        private IEAElement loadedElement;

        public RepositoryContentLoaderTest()
        {
            eaRepository = new EARepository();

            model = eaRepository.AddModel("Model", m => { });

            otherPackage = CreatePackage(model, "other package");

            bInformationV = CreatePackage(model, Stereotype.BInformationV);

            bLibrary = CreatePackage(bInformationV, Stereotype.BLibrary);

            primLibrary = CreatePackage(bLibrary, Stereotype.PRIMLibrary);
            enumLibrary = CreatePackage(bLibrary, Stereotype.ENUMLibrary);
            cdtLibrary = CreatePackage(bLibrary, Stereotype.CDTLibrary);
            ccLibrary = CreatePackage(bLibrary, Stereotype.CCLibrary);
            bdtLibrary = CreatePackage(bLibrary, Stereotype.BDTLibrary);
            bieLibrary = CreatePackage(bLibrary, Stereotype.BIELibrary);
            docLibrary = CreatePackage(bLibrary, Stereotype.DOCLibrary);

            otherElement = CreateElement(otherPackage, "other element");

            contentLoader = new RepositoryContentLoader(eaRepository);
            contentLoader.PackageLoaded += HandlePackageLoaded;
            contentLoader.ElementLoaded += HandleElementLoaded;
        }

        private void HandlePackageLoaded(IEAPackage package)
        {
            loadedPackage = package;
        }

        private void HandleElementLoaded(IEAElement element)
        {
            loadedElement = element;
        }

        private static Package CreatePackage(Package parent, string stereotype)
        {
            return parent.AddPackage(stereotype, p1 => { p1.Element.Stereotype = stereotype; });
        }

        private static Element CreateElement(Package parent, string stereotype)
        {
            return parent.AddClass(stereotype).With(element => element.Stereotype = stereotype);
        }

        private void LoadAndVerifyPackage<T>(Package eaPackage) where T : IEAPackage
        {
            contentLoader.LoadPackageByID(eaPackage.PackageID);
            VerifyPackage<T>(eaPackage, loadedPackage);
        }

        private static void VerifyPackage<T>(Package expectedPackage, IEAPackage actualPackage)
        {
            Assert.IsTrue(actualPackage is T, "wrong package class:\n expected " + typeof (T) + "\n but was " + actualPackage.GetType());
            Assert.AreEqual(expectedPackage.PackageID, actualPackage.Id, "wrong package ID");
        }

        private void LoadAndVerifyElement<T>(Element eaElement) where T : IEAElement
        {
            contentLoader.LoadElementByID(eaElement.ElementID);
            Assert.IsTrue(loadedElement is T, "wrong element class:\n expected " + typeof (T) + "\n but was " + loadedElement.GetType());
            Assert.AreEqual(eaElement.ElementID, loadedElement.Id, "wrong element ID");
        }

        [Test]
        public void TestLoadAll()
        {
            var loadedPackages = new List<IEAPackage>();
            contentLoader.PackageLoaded += loadedPackages.Add;
            contentLoader.LoadRepositoryContent();

            var orderedLoadedPackages = new Queue<IEAPackage>(from package in loadedPackages orderby package.Name select package);
            IOrderedEnumerable<Package> orderedExpectedPackages = from eaPackage in new[] {model, bInformationV, bLibrary, bdtLibrary, bieLibrary, ccLibrary, cdtLibrary, docLibrary, enumLibrary, primLibrary, otherPackage} orderby eaPackage.Name select eaPackage;

            Assert.AreEqual(orderedExpectedPackages.Count(), orderedLoadedPackages.Count);
            foreach (Package eaPackage in orderedExpectedPackages)
            {
                string expectedPackageType = (eaPackage.Element != null ? eaPackage.Element.Stereotype : "Model");
                IEAPackage actualPackage = orderedLoadedPackages.Dequeue();
                string actualPackageType = actualPackage.GetType().Name;
                Assert.AreEqual(eaPackage.PackageID, actualPackage.Id, "wrong package ID:\n expected package: " + expectedPackageType + "\n actual package: " + actualPackageType);
            }
        }

        [Test]
        public void TestLoadElement()
        {
            LoadAndVerifyElement<OtherElement>(otherElement);
        }

        [Test]
        public void TestLoadPackage()
        {
            LoadAndVerifyPackage<EAModel>(model);
            LoadAndVerifyPackage<OtherPackage>(otherPackage);
            LoadAndVerifyPackage<BInformationV>(bInformationV);
            LoadAndVerifyPackage<BLibrary>(bLibrary);
            LoadAndVerifyPackage<PRIMLibrary>(primLibrary);
            LoadAndVerifyPackage<ENUMLibrary>(enumLibrary);
            LoadAndVerifyPackage<CDTLibrary>(cdtLibrary);
            LoadAndVerifyPackage<CCLibrary>(ccLibrary);
            LoadAndVerifyPackage<BDTLibrary>(bdtLibrary);
            LoadAndVerifyPackage<BIELibrary>(bieLibrary);
            LoadAndVerifyPackage<DOCLibrary>(docLibrary);
        }
    }

    public abstract class CCRepositoryTest
    {
        protected static int nextId;

        protected static void VerifyLibraries<TLibrary>(ValidatingCCRepository ccRepository, params string[] expectedLibraryNames) where TLibrary : IBusinessLibrary
        {
            var libraries = new List<TLibrary>(ccRepository.Libraries<TLibrary>());
            Assert.AreEqual(expectedLibraryNames.Count(), libraries.Count);
            int i = 0;
            foreach (string libraryName in expectedLibraryNames)
            {
                Assert.AreEqual(libraryName, libraries[i++].Name);
            }
        }

        protected IEAPackage CreateSubPackage(IEAPackage parent, IEAPackage child)
        {
            parent.AddSubPackage(child);
            return child;
        }

        protected TestPackage CreateEAPackage(int parentId)
        {
            return new TestPackage(parentId);
        }

        protected static OtherElement CreateEAElement(IEAPackage package)
        {
            var element = new OtherElement(++nextId, "Other Element " + nextId, package.Id);
            package.AddElement(element);
            return element;
        }

        protected static void VerifyValidationIssues(IValidating validatingObject, params int[] validationIssueItemIds)
        {
            var validationIssues = new List<IValidationIssue>(validatingObject.Validate());
            foreach (IValidationIssue validationIssue in validationIssues)
            {
                Console.WriteLine(validationIssue);
            }
            Assert.AreEqual(validationIssueItemIds.Length, validationIssues.Count);
            for (int i = 0; i < validationIssueItemIds.Length; ++i)
            {
                Assert.AreEqual(validationIssueItemIds[i], validationIssues[i].ItemId);
            }
        }

        protected static void VerifyValidationIssues(ValidatingCCRepository ccRepository, params int[] validationIssueItemIds)
        {
            List<IValidationIssue> validationIssues = ccRepository.ValidationIssues;
            foreach (IValidationIssue validationIssue in validationIssues)
            {
                Console.WriteLine(validationIssue);
            }
            Assert.AreEqual(validationIssueItemIds.Length, validationIssues.Count);
            for (int i = 0; i < validationIssueItemIds.Length; ++i)
            {
                Assert.AreEqual(validationIssueItemIds[i], validationIssues[i].ItemId);
            }
        }
    }

    [TestFixture]
    public class Given_a_new_CCRepository : CCRepositoryTest
    {
        [Test]
        public void When_it_is_created_Then_it_should_load_all_EA_repository_contents_and_build_an_internal_model()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", model =>
                                           {
                                               model.AddPackage("Package 1", package_1 =>
                                                                             {
                                                                                 package_1.Element.Stereotype = Stereotype.BLibrary;
                                                                                 package_1.AddPackage("Package 1.1", package_1_1 =>
                                                                                                                     {
                                                                                                                         package_1_1.Element.Stereotype = Stereotype.BLibrary;
                                                                                                                         package_1_1.AddPackage("Package 1.1.1", p => { p.Element.Stereotype = Stereotype.PRIMLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.2", p => { p.Element.Stereotype = Stereotype.ENUMLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.3", p => { p.Element.Stereotype = Stereotype.CDTLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.4", p => { p.Element.Stereotype = Stereotype.CCLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.5", p => { p.Element.Stereotype = Stereotype.BDTLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.6", p => { p.Element.Stereotype = Stereotype.BIELibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.7", p => { p.Element.Stereotype = Stereotype.DOCLibrary; });
                                                                                                                     });
                                                                                 package_1.AddPackage("Package 1.2", package_1_2 => { package_1_2.Element.Stereotype = "Foo bar 1"; });
                                                                             });
                                               model.AddPackage("Package 2", package_2 => { package_2.Element.Stereotype = "Foo bar 2"; });
                                               model.AddPackage("Package 3", package_3 =>
                                                                             {
                                                                                 package_3.Element.Stereotype = Stereotype.BInformationV;
                                                                                 package_3.AddPackage("Package 3.1", package_3_1 =>
                                                                                                                     {
                                                                                                                         package_3_1.Element.Stereotype = Stereotype.BLibrary;
                                                                                                                         package_3_1.AddPackage("Package 3.1.1", package_3_1_1 => { package_3_1_1.Element.Stereotype = Stereotype.BLibrary; });
                                                                                                                         package_3_1.AddPackage("Package 3.1.2", package_3_1_2 => { package_3_1_2.Element.Stereotype = "Foo bar 3"; });
                                                                                                                     });
                                                                             });
                                           });
            var ccRepository = new ValidatingCCRepository();
            var contentLoader = new RepositoryContentLoader(eaRepository);
            contentLoader.PackageLoaded += ccRepository.HandlePackageCreatedOrModified;
            contentLoader.ElementLoaded += ccRepository.HandleElementCreatedOrModified;
            contentLoader.LoadRepositoryContent();
            VerifyLibraries<IBLibrary>(ccRepository, "Package 1", "Package 1.1", "Package 3.1", "Package 3.1.1");
            VerifyLibraries<IPRIMLibrary>(ccRepository, "Package 1.1.1");
            VerifyLibraries<IENUMLibrary>(ccRepository, "Package 1.1.2");
            VerifyLibraries<ICDTLibrary>(ccRepository, "Package 1.1.3");
            VerifyLibraries<ICCLibrary>(ccRepository, "Package 1.1.4");
            VerifyLibraries<IBDTLibrary>(ccRepository, "Package 1.1.5");
            VerifyLibraries<IBIELibrary>(ccRepository, "Package 1.1.6");
            VerifyLibraries<IDOCLibrary>(ccRepository, "Package 1.1.7");
        }
    }

    [TestFixture]
    public class Given_a_CCRepository : CCRepositoryTest
    {
        private static void AssertItems(ValidatingCCRepository ccRepository, params IEAItem[] items)
        {
            var itemList = new Queue<IEAItem>(items);
            ccRepository.WithAllItemsDo(item =>
                                        {
                                            Assert.IsTrue(itemList.Count > 0, "Too many items");
                                            Assert.AreSame(itemList.Dequeue(), item);
                                        });
            Assert.AreEqual(0, itemList.Count, "Too few items");
        }

        [Test]
        public void When_a_package_is_added_Then_the_package_and_its_parent_package_should_need_validation()
        {
            var ccRepository = new ValidatingCCRepository();
            var parentPackage = new TestPackage(0);
            ccRepository.HandlePackageCreatedOrModified(parentPackage);
            var newPackage = new TestPackage(parentPackage.Id);
            ccRepository.HandlePackageCreatedOrModified(newPackage);
            Assert.AreEqual(0, newPackage.ValidationCounter);
            Assert.AreEqual(0, parentPackage.ValidationCounter);
            AssertItems(ccRepository, parentPackage, newPackage);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, newPackage.ValidationCounter);
            Assert.AreEqual(1, parentPackage.ValidationCounter);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_validation_issues_should_be_removed_and_its_parent_package_should_be_validated()
        {
            var ccRepository = new ValidatingCCRepository();
            var parentPackage = new TestPackage(0);
            var subPackage = new TestPackage(parentPackage.Id);
            ccRepository.HandlePackageCreatedOrModified(parentPackage);
            ccRepository.HandlePackageCreatedOrModified(subPackage);
            AssertItems(ccRepository, parentPackage, subPackage);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, subPackage.ValidationCounter);
            Assert.AreEqual(1, parentPackage.ValidationCounter);
            VerifyValidationIssues(ccRepository, parentPackage.Id, subPackage.Id);

            ccRepository.HandlePackageDeleted(subPackage.Id);
            AssertItems(ccRepository, parentPackage);
            ccRepository.ValidateAll();
            Assert.AreEqual(2, parentPackage.ValidationCounter);
            VerifyValidationIssues(ccRepository, parentPackage.Id);
        }

        [Test]
        public void When_a_package_is_deleted_Then_the_package_and_all_its_descendants_and_their_validation_issues_should_be_removed()
        {
            var ccRepository = new ValidatingCCRepository();
            var parentPackage = new TestPackage(0);
            var package = new TestPackage(parentPackage.Id);
            var subPackage = new TestPackage(package.Id);
            ccRepository.HandlePackageCreatedOrModified(parentPackage);
            ccRepository.HandlePackageCreatedOrModified(package);
            ccRepository.HandlePackageCreatedOrModified(subPackage);
            AssertItems(ccRepository, parentPackage, package, subPackage);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, subPackage.ValidationCounter);
            Assert.AreEqual(1, package.ValidationCounter);
            Assert.AreEqual(1, parentPackage.ValidationCounter);
            VerifyValidationIssues(ccRepository, parentPackage.Id, package.Id, subPackage.Id);

            ccRepository.HandlePackageDeleted(package.Id);
            AssertItems(ccRepository, parentPackage);
            ccRepository.ValidateAll();
            Assert.AreEqual(2, parentPackage.ValidationCounter);
            VerifyValidationIssues(ccRepository, parentPackage.Id);
        }

        [Test]
        public void When_a_package_is_modified_Then_the_package_and_its_parent_package_should_be_validated()
        {
            var ccRepository = new ValidatingCCRepository();
            var parentPackage = new TestPackage(0);
            var subPackage = new TestPackage(parentPackage.Id);
            ccRepository.HandlePackageCreatedOrModified(parentPackage);
            ccRepository.HandlePackageCreatedOrModified(subPackage);
            AssertItems(ccRepository, parentPackage, subPackage);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, subPackage.ValidationCounter);
            Assert.AreEqual(1, parentPackage.ValidationCounter);
            TestPackage newPackage = TestPackage.Clone(subPackage);
            ccRepository.HandlePackageCreatedOrModified(newPackage);
            ccRepository.ValidateAll();
            AssertItems(ccRepository, parentPackage, newPackage);
            Assert.AreEqual(1, newPackage.ValidationCounter);
            Assert.AreEqual(2, parentPackage.ValidationCounter);
        }

        [Test]
        public void When_an_element_is_added_Then_the_element_and_its_package_should_be_validated()
        {
            var ccRepository = new ValidatingCCRepository();
            var package = new TestPackage(0);
            ccRepository.HandlePackageCreatedOrModified(package);
            var element = new TestElement(package.Id);
            ccRepository.HandleElementCreatedOrModified(element);
            AssertItems(ccRepository, package, element);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, package.ValidationCounter);
            Assert.AreEqual(1, element.ValidationCounter);
        }

        [Test]
        public void When_an_element_is_deleted_Then_its_validation_issues_should_be_removed_and_its_package_should_be_validated()
        {
            var ccRepository = new ValidatingCCRepository();
            var package = new TestPackage(0);
            var element = new TestElement(package.Id);
            ccRepository.HandlePackageCreatedOrModified(package);
            ccRepository.HandleElementCreatedOrModified(element);
            AssertItems(ccRepository, package, element);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, package.ValidationCounter);
            Assert.AreEqual(1, element.ValidationCounter);
            VerifyValidationIssues(ccRepository, package.Id, element.Id);

            ccRepository.HandleElementDeleted(element.Id);
            AssertItems(ccRepository, package);
            ccRepository.ValidateAll();
            Assert.AreEqual(2, package.ValidationCounter);
            VerifyValidationIssues(ccRepository, package.Id);
        }

        [Test]
        public void When_an_element_is_modified_Then_the_element_and_its_package_should_be_validated()
        {
            var ccRepository = new ValidatingCCRepository();
            var package = new TestPackage(0);
            var element = new TestElement(package.Id);
            ccRepository.HandlePackageCreatedOrModified(package);
            ccRepository.HandleElementCreatedOrModified(element);
            AssertItems(ccRepository, package, element);
            ccRepository.ValidateAll();
            Assert.AreEqual(1, package.ValidationCounter);
            Assert.AreEqual(1, element.ValidationCounter);

            TestElement newElement = TestElement.Clone(element);
            ccRepository.HandleElementCreatedOrModified(newElement);
            AssertItems(ccRepository, package, newElement);
            ccRepository.ValidateAll();
            Assert.AreEqual(2, package.ValidationCounter);
            Assert.AreEqual(1, newElement.ValidationCounter);
        }
    }

    public class TestPackage : AbstractEAPackage
    {
        private static int NextId;

        public TestPackage(int parentId)
            : this(++NextId, parentId)
        {
        }

        private TestPackage(int id, int parentId)
            : base(id, "TestPackage " + NextId, parentId)
        {
        }

        public int ValidationCounter { get; private set; }

        internal static TestPackage Clone(TestPackage package)
        {
            return new TestPackage(package.Id, package.ParentId);
        }

        protected override IEnumerable<IValidationIssue> PerformValidation()
        {
            ++ValidationCounter;
            yield return new TestValidationIssue(Id, Id);
        }
    }

    public class TestElement : AbstractEAElement
    {
        private static int NextId;

        public TestElement(int packageId)
            : this(++NextId, packageId)
        {
        }

        private TestElement(int id, int parentId)
            : base(id, "TestElement " + NextId, parentId)
        {
        }

        public int ValidationCounter { get; private set; }

        internal static TestElement Clone(TestElement element)
        {
            return new TestElement(element.Id, element.PackageId);
        }

        protected override IEnumerable<IValidationIssue> PerformValidation()
        {
            ++ValidationCounter;
            yield return new TestValidationIssue(Id, Id);
        }
    }

    public class TestValidationIssue : AbstractValidationIssue
    {
        public TestValidationIssue(int validatedItemId, int itemId) : base(validatedItemId, itemId)
        {
        }

        public override string Message
        {
            get { return "test validation issue"; }
        }

        public override object ResolveItem(Repository repository)
        {
            return null;
        }
    }

    [TestFixture]
    public class Given_a_bLibrary : CCRepositoryTest
    {
        private static BLibrary CreateDefaultBLibrary()
        {
            return new BLibrary(++nextId, "BLibrary", 0, "", "1234567", "1", "urn:foo:bar", "", new string[0], new string[0], new string[0], new string[0]);
        }

        /// <summary>
        /// Create a BLibrary and add it as a sub-package to the given parent package.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static BLibrary CreateBLibrary(IEAPackage parent)
        {
            var bLibrary = new BLibrary(++nextId, "BLibrary", parent.Id, "", "1234567", "1", "urn:foo:bar", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(bLibrary);
            return bLibrary;
        }

        private static PRIMLibrary CreatePRIMLibrary(IEAPackage parent)
        {
            var library = new PRIMLibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        private static ENUMLibrary CreateENUMLibrary(IEAPackage parent)
        {
            var library = new ENUMLibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        private static CDTLibrary CreateCDTLibrary(IEAPackage parent)
        {
            var library = new CDTLibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        private static CCLibrary CreateCCLibrary(IEAPackage parent)
        {
            var library = new CCLibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        private static BDTLibrary CreateBDTLibrary(IEAPackage parent)
        {
            var library = new BDTLibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        private static BIELibrary CreateBIELibrary(IEAPackage parent)
        {
            var library = new BIELibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        private static DOCLibrary CreateDOCLibrary(IEAPackage parent)
        {
            var library = new DOCLibrary(++nextId, "", parent.Id, "", "", "", "", "", new string[0], new string[0], new string[0], new string[0]);
            parent.AddSubPackage(library);
            return library;
        }

        /// <summary>
        /// Create an OtherPackage and add it as a sub-package to the given parent package.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static OtherPackage CreateOtherPackage(IEAPackage parent)
        {
            var otherPackage = new OtherPackage(++nextId, "OtherPackage " + nextId, parent.Id);
            parent.AddSubPackage(otherPackage);
            return otherPackage;
        }

        [Test]
        public void When_a_subpackage_is_added_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            bLibrary.Validate();
            bLibrary.AddSubPackage(new TestPackage(bLibrary.Id));
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_a_subpackage_is_removed_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            var subPackage = new TestPackage(bLibrary.Id);
            bLibrary.AddSubPackage(subPackage);
            bLibrary.Validate();
            bLibrary.RemoveSubPackage(subPackage.Id);
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_a_subpackage_is_replaced_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            var subPackage = new TestPackage(bLibrary.Id);
            bLibrary.AddSubPackage(subPackage);
            bLibrary.Validate();
            bLibrary.ReplaceSubPackage(subPackage);
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_an_element_is_added_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            bLibrary.Validate();
            bLibrary.AddElement(new TestElement(bLibrary.Id));
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_an_element_is_removed_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            var element = new TestElement(bLibrary.Id);
            bLibrary.AddElement(element);
            bLibrary.Validate();
            bLibrary.RemoveElement(element.Id);
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_an_element_is_replaced_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            var element = new TestElement(bLibrary.Id);
            bLibrary.AddElement(element);
            bLibrary.Validate();
            bLibrary.ReplaceElement(element);
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_it_contains_elements_Then_report_an_error_for_each_element()
        {
            BLibrary parent = CreateDefaultBLibrary();
            BLibrary bLibrary = CreateBLibrary(parent);
            OtherElement element1 = CreateEAElement(bLibrary);
            OtherElement element2 = CreateEAElement(bLibrary);
            VerifyValidationIssues(bLibrary, element1.Id, element2.Id);
        }

        [Test]
        public void When_it_contains_invalid_subpackages_Then_report_an_error_for_each_case()
        {
            BLibrary parent = CreateDefaultBLibrary();
            BLibrary bLibrary = CreateBLibrary(parent);
            OtherPackage subPackage1 = CreateOtherPackage(bLibrary);
            OtherPackage subPackage2 = CreateOtherPackage(bLibrary);
            VerifyValidationIssues(bLibrary, subPackage1.Id, subPackage2.Id);
        }

        [Test]
        public void When_it_is_newly_created_Then_it_should_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            Assert.IsTrue(bLibrary.NeedsValidation, "should need validation, but does not");
        }

        [Test]
        public void When_it_is_valid_Then_it_should_not_report_any_validation_issues()
        {
            BLibrary parent = CreateDefaultBLibrary();
            BLibrary bLibrary = CreateBLibrary(parent);
            CreateBLibrary(bLibrary);
            CreatePRIMLibrary(bLibrary);
            CreateENUMLibrary(bLibrary);
            CreateCDTLibrary(bLibrary);
            CreateCCLibrary(bLibrary);
            CreateBDTLibrary(bLibrary);
            CreateBIELibrary(bLibrary);
            CreateDOCLibrary(bLibrary);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_it_is_validated_Then_it_should_no_longer_need_validation()
        {
            BLibrary bLibrary = CreateDefaultBLibrary();
            bLibrary.Validate();
            Assert.IsFalse(bLibrary.NeedsValidation, "should not need validation, but does");
        }

        [Test]
        public void When_its_name_is_empty_Then_report_an_error()
        {
            var parent = new EAModel(0, "Model", 0);
            var bLibrary = new BLibrary(++nextId, "", parent.Id, "", "1234567", "1", "urn:foo:bar", "", new string[0], new string[0], new string[0], new string[0]);
            bLibrary.ParentPackage = parent;
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_uniqueIdentifier_is_empty_Then_report_an_error()
        {
            var parent = new EAModel(0, "Model", 0);
            var bLibrary = new BLibrary(++nextId, "BLibrary", parent.Id, "", "", "1", "urn:foo:bar", "", new string[0], new string[0], new string[0], new string[0])
                           {
                               ParentPackage = parent
                           };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_versionIdentifier_is_empty_Then_report_an_error()
        {
            var parent = new EAModel(0, "Model", 0);
            var bLibrary = new BLibrary(++nextId, "BLibrary", parent.Id, "", "1234567", "", "urn:foo:bar", "", new string[0], new string[0], new string[0], new string[0])
                           {
                               ParentPackage = parent
                           };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_baseURN_is_empty_Then_report_an_error()
        {
            var parent = new EAModel(0, "Model", 0);
            var bLibrary = new BLibrary(++nextId, "BLibrary", parent.Id, "", "1234567", "1", "", "", new string[0], new string[0], new string[0], new string[0])
                           {
                               ParentPackage = parent
                           };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_parent_is_a_bInformationV_Then_do_not_report_an_error()
        {
            var parent = new BInformationV(++nextId, "BInformationV", 0);
            BLibrary bLibrary = CreateBLibrary(parent);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_parent_is_a_bLibrary_Then_do_not_report_an_error()
        {
            BLibrary parent = CreateDefaultBLibrary();
            BLibrary bLibrary = CreateBLibrary(parent);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_parent_is_a_Model_Then_do_not_report_an_error()
        {
            var parent = new EAModel(++nextId, "Model", 0);
            BLibrary bLibrary = CreateBLibrary(parent);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_parent_is_neither_a_Model_nor_a_bInformationV_nor_a_bLibrary_Then_do_report_an_error()
        {
            var parent = new OtherPackage(++nextId, "OtherPackage " + nextId, 0);
            BLibrary bLibrary = CreateBLibrary(parent);
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }
    }
}