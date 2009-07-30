using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class Given_a_HierarchicalRepository
    {
        private HierarchicalRepository repository;

        [SetUp]
        public void Context()
        {
            repository = new HierarchicalRepository();
        }

        [Test]
        public void When_an_item_is_loaded_Then_it_should_be_retrievable_by_ID()
        {
            var itemData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(itemData);
            var item = repository.GetItemById(itemData.Id);
            Assert.IsNotNull(item, "The item is not retrievable.");
        }

        [Test]
        public void When_an_item_is_deleted_Then_it_should_no_longer_be_retrievable_by_ID()
        {
            var itemData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(itemData);
            repository.ItemDeleted(((IRepositoryItemData) itemData).Id);
            var item = repository.GetItemById(itemData.Id);
            Assert.IsNull(item, "The item is still retrievable.");
        }

        [Test]
        public void When_an_item_with_parentId_0_is_loaded_Then_it_should_be_inserted_right_below_the_root()
        {
            var itemData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(itemData);
            var root = repository.Root;
            var firstChild = root.Children.First();
            Assert.AreEqual(itemData.Id, firstChild.Id);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void When_an_item_with_an_unknown_parentId_is_loaded_Then_it_should_throw_an_ArgumentException()
        {
            var itemData = new TestRepositoryItemData(ItemId.ForPackage(2));
            repository.ItemLoaded(itemData);
        }

        [Test]
        public void When_an_item_is_loaded_Then_it_should_be_added_as_a_child_to_its_parent()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);
            var childData = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(childData);
            var parent = repository.GetItemById(parentData.Id);
            var firstChild = parent.Children.First();
            Assert.AreEqual(childData.Id, firstChild.Id);
        }

        [Test]
        public void When_an_item_is_loaded_Then_the_parents_previous_children_should_still_be_there()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);
            var parent = repository.GetItemById(parentData.Id);

            var childData1 = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(childData1);
            Assert.AreEqual(1, parent.Children.Count());
            Assert.AreEqual(childData1.Id, parent.Children.First().Id);

            var childData2 = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(childData2);
            Assert.AreEqual(2, parent.Children.Count());
            Assert.AreEqual(childData1.Id, parent.Children.First().Id);
        }

        [Test]
        public void When_an_item_is_deleted_Then_it_should_be_removed_from_its_parent()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);
            var childData = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(childData);
            repository.ItemDeleted(((IRepositoryItemData) childData).Id);
            var parent = repository.GetItemById(parentData.Id);
            Assert.AreEqual(0, parent.Children.Count(), "parent still has a child");
        }

        [Test]
        public void When_an_item_is_reloaded_Then_the_old_item_should_be_replaced()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);
            var childData = new TestRepositoryItemData(parentData.Id)
                            {
                                SomeData = "old"
                            };
            repository.ItemLoaded(childData);
            var childDataNew = new TestRepositoryItemData(childData)
                               {
                                   SomeData = "new"
                               };
            repository.ItemLoaded(childDataNew);

            var parent = repository.GetItemById(parentData.Id);
            var firstChild = parent.Children.First();
            Assert.AreEqual(childDataNew.SomeData, ((TestRepositoryItemData) firstChild.Data).SomeData);

            var child = repository.GetItemById(childData.Id);
            Assert.AreEqual(childDataNew.SomeData, ((TestRepositoryItemData) child.Data).SomeData);
        }

        [Test]
        public void When_an_item_is_reloaded_Then_it_should_retain_its_children()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);
            var parent = repository.GetItemById(parentData.Id);

            var childData = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(childData);
            Assert.AreEqual(1, parent.Children.Count());

            repository.ItemLoaded(parentData);
            var parentReloaded = repository.GetItemById(parentData.Id);
            Assert.AreEqual(1, parentReloaded.Children.Count());
        }

        [Test]
        public void When_an_item_is_loaded_Then_a_modification_event_should_be_generated_for_the_item()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);

            var childData = new TestRepositoryItemData(parentData.Id);

            bool expectedEventGenerated = false;
            repository.OnItemCreatedOrModified += item =>
                                                  {
                                                      if (item.Id == childData.Id)
                                                      {
                                                          expectedEventGenerated = true;
                                                      }
                                                  };

            repository.ItemLoaded(childData);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }

        [Test]
        public void When_an_item_is_loaded_Then_a_modification_event_should_be_generated_for_its_parent()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            repository.ItemLoaded(parentData);

            var childData = new TestRepositoryItemData(parentData.Id);

            bool expectedEventGenerated = false;
            repository.OnItemCreatedOrModified += item =>
                                                  {
                                                      if (item.Id == parentData.Id)
                                                      {
                                                          expectedEventGenerated = true;
                                                      }
                                                  };

            repository.ItemLoaded(childData);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }

        [Test]
        public void When_an_item_is_deleted_Then_a_deletion_event_should_be_generated_for_the_item()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            var childData = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(parentData);
            repository.ItemLoaded(childData);

            bool expectedEventGenerated = false;
            repository.OnItemDeleted += item =>
                                        {
                                            if (item.Id == childData.Id)
                                            {
                                                expectedEventGenerated = true;
                                            }
                                        };

            repository.ItemDeleted(((IRepositoryItemData) childData).Id);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }

        [Test]
        public void When_an_item_is_deleted_Then_a_deletion_event_should_be_generated_for_its_children()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            var childData1 = new TestRepositoryItemData(parentData.Id);
            var childData2 = new TestRepositoryItemData(parentData.Id);
            var childData3 = new TestRepositoryItemData(childData2.Id);
            repository.ItemLoaded(parentData);
            repository.ItemLoaded(childData1);
            repository.ItemLoaded(childData2);
            repository.ItemLoaded(childData3);

            var expectedItemIds = new List<ItemId>
                                  {
                                      parentData.Id,
                                      childData1.Id,
                                      childData2.Id,
                                      childData3.Id
                                  };
            repository.OnItemDeleted += item => expectedItemIds.Remove(item.Id);

            repository.ItemDeleted(((IRepositoryItemData) parentData).Id);
            Assert.AreEqual(0, expectedItemIds.Count, "the expected events were not generated");
        }

        [Test]
        public void When_an_item_is_deleted_Then_a_modification_event_should_be_generated_for_its_parent()
        {
            var parentData = new TestRepositoryItemData(ItemId.Null);
            var childData = new TestRepositoryItemData(parentData.Id);
            repository.ItemLoaded(parentData);
            repository.ItemLoaded(childData);

            bool expectedEventGenerated = false;
            repository.OnItemCreatedOrModified += item =>
                                                  {
                                                      if (item.Id == parentData.Id)
                                                      {
                                                          expectedEventGenerated = true;
                                                      }
                                                  };

            repository.ItemDeleted(((IRepositoryItemData) childData).Id);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }
    }
}