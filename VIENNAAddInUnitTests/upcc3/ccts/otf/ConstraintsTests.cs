using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public abstract class ConstraintsTests
    {
        protected static RepositoryItem AddChild(RepositoryItem parent, MyTestRepositoryItemData itemData)
        {
            var newItem = new RepositoryItem(itemData);
            parent.AddOrReplaceChild(newItem);
            return newItem;
        }

        protected static RepositoryItem AddChild(RepositoryItem parent, ItemId.ItemType itemType)
        {
            return AddChild(parent, new MyTestRepositoryItemData(parent.Id, itemType));
        }

        protected static RepositoryItem AddSubLibrary(RepositoryItem parent, string stereotype)
        {
            return AddChild(parent, new MyTestRepositoryItemData(parent.Id, ItemId.ItemType.Package)
                                    {
                                        Stereotype = stereotype,
                                    });
        }

        protected void VerifyValidationIssues(RepositoryItem item, params ItemId[] validationIssueItemIds)
        {
            var validationIssues = new List<IValidationIssue>(Constraints().Check(item));
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

        protected abstract IConstraint Constraints();

        protected static RepositoryItem AddElement(RepositoryItem library, string stereotype)
        {
            return AddChild(library, new MyTestRepositoryItemData(library.Id, ItemId.ItemType.Element){Stereotype = stereotype});
        }
    }
}