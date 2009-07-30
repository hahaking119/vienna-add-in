﻿using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class Given_a_bLibrary : CCRepositoryTest
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

        private static RepositoryItem AddChild(RepositoryItem parent, MyTestRepositoryItemData itemData)
        {
            var newItem = new RepositoryItem(itemData);
            parent.AddOrReplaceChild(newItem);
            return newItem;
        }

        private static RepositoryItem AddChild(RepositoryItem parent, ItemId.ItemType itemType)
        {
            return AddChild(parent, new MyTestRepositoryItemData(parent.Id, itemType));
        }

        private static RepositoryItem AddSubLibrary(RepositoryItem parent, string stereotype)
        {
            return AddChild(parent, new MyTestRepositoryItemData(parent.Id, ItemId.ItemType.Package)
                                    {
                                        Stereotype = stereotype,
                                    });
        }

        [Test]
        public void When_it_contains_elements_Then_report_an_error_for_each_element()
        {
            RepositoryItem element1 = AddChild(bLibrary, ItemId.ItemType.Element);
            RepositoryItem element2 = AddChild(bLibrary, ItemId.ItemType.Element);
            VerifyValidationIssues(bLibrary, element1.Id, element2.Id);
        }

        [Test]
        public void When_it_contains_invalid_subpackages_Then_report_an_error_for_each_case()
        {
            RepositoryItem subPackage = AddChild(bLibrary, ItemId.ItemType.Package);
            VerifyValidationIssues(bLibrary, subPackage.Id);
        }

        [Test]
        public void When_it_is_valid_Then_it_should_not_report_any_validation_issues()
        {
            AddSubLibrary(bLibrary, Stereotype.BLibrary);
            AddSubLibrary(bLibrary, Stereotype.PRIMLibrary);
            AddSubLibrary(bLibrary, Stereotype.ENUMLibrary);
            AddSubLibrary(bLibrary, Stereotype.CDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.CCLibrary);
            AddSubLibrary(bLibrary, Stereotype.BDTLibrary);
            AddSubLibrary(bLibrary, Stereotype.BIELibrary);
            AddSubLibrary(bLibrary, Stereotype.DOCLibrary);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_baseURN_is_empty_Then_report_an_error()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.versionIdentifier, "foo"},
                                        };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_name_is_empty_Then_report_an_error()
        {
            bLibraryData.Name = "";
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_parent_is_a_bInformationV_Then_do_not_report_an_error()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, Stereotype.BInformationV);
            parent.AddOrReplaceChild(bLibrary);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_parent_is_a_bLibrary_Then_do_not_report_an_error()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, Stereotype.BLibrary);
            parent.AddOrReplaceChild(bLibrary);
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_parent_is_a_Model_Then_do_not_report_an_error()
        {
            VerifyValidationIssues(bLibrary);
        }

        [Test]
        public void When_its_parent_is_neither_a_Model_nor_a_bInformationV_nor_a_bLibrary_Then_do_report_an_error()
        {
            model.RemoveChild(bLibrary.Id);
            RepositoryItem parent = AddSubLibrary(model, "Something else");
            parent.AddOrReplaceChild(bLibrary);
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_uniqueIdentifier_is_empty_Then_report_an_error()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.versionIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }

        [Test]
        public void When_its_versionIdentifier_is_empty_Then_report_an_error()
        {
            bLibraryData.TaggedValues = new Dictionary<TaggedValues, string>
                                        {
                                            {TaggedValues.uniqueIdentifier, "foo"},
                                            {TaggedValues.baseURN, "foo"},
                                        };
            VerifyValidationIssues(bLibrary, bLibrary.Id);
        }
    }

    internal class MyTestRepositoryItemData : IRepositoryItemData
    {
        private static int NextId;
        private string name;
        private bool overrideDefaultName;

        public MyTestRepositoryItemData(ItemId parentId, ItemId.ItemType itemType)
        {
            ParentId = parentId;
            Id = new ItemId(itemType, ++NextId);
        }

        public Dictionary<TaggedValues, string> TaggedValues { get; set; }

        #region IRepositoryItemData Members

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }

        public string Name
        {
            get { return overrideDefaultName ? name : Stereotype + "_" + Id; }
            set
            {
                overrideDefaultName = true;
                name = value;
            }
        }

        public string Stereotype { get; set; }

        public string GetTaggedValue(TaggedValues key)
        {
            string value;
            if (TaggedValues != null && TaggedValues.TryGetValue(key, out value))
            {
                return value;
            }
            return string.Empty;
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return new[] {GetTaggedValue(key)};
        }

        #endregion
    }
}