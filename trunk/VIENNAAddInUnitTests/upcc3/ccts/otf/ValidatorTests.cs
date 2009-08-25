using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public abstract class ValidatorTests
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

        protected void VerifyConstraintViolations(RepositoryItem item, params ItemId[] offendingItemIds)
        {
            var constraintViolations = new List<ConstraintViolation>(Validator().Validate(item));
            foreach (ConstraintViolation constraintViolation in constraintViolations)
            {
                Console.WriteLine(constraintViolation);
            }
            Assert.AreEqual(offendingItemIds.Length, constraintViolations.Count);
            for (int i = 0; i < offendingItemIds.Length; ++i)
            {
                Assert.AreEqual(offendingItemIds[i], constraintViolations[i].OffendingItemId);
            }
        }

        protected abstract IValidator Validator();

        protected static RepositoryItem AddElement(RepositoryItem library, string stereotype)
        {
            return AddChild(library, new MyTestRepositoryItemData(library.Id, ItemId.ItemType.Element){Stereotype = stereotype});
        }
    }
}