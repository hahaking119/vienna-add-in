using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public abstract class ValidatorTests
    {
        protected static RepositoryItemBuilder APackageRepositoryItem
        {
            get { return ARepositoryItem; }
        }

        protected static RepositoryItemBuilder ARepositoryItem
        {
            get { return new RepositoryItemBuilder(); }
        }

        protected static RepositoryItem AddChild(RepositoryItem parent, RepositoryItemBuilder itemBuilder)
        {
            RepositoryItem newItem = itemBuilder.Build();
            parent.AddOrReplaceChild(newItem);
            return newItem;
        }

        protected static RepositoryItem AddChild(RepositoryItem parent, ItemId.ItemType itemType)
        {
            return itemType == ItemId.ItemType.Package
                       ? AddChild(parent, APackageRepositoryItem
                                              .WithItemType(itemType)
                                              .WithParentId(parent.Id))
                       : AddChild(parent, ARepositoryItem
                                              .WithItemType(itemType)
                                              .WithParentId(parent.Id));
        }

        protected static RepositoryItem AddSubLibrary(RepositoryItem parent, string stereotype)
        {
            return AddChild(parent, APackageRepositoryItem
                                        .WithItemType(ItemId.ItemType.Package)
                                        .WithParentId(parent.Id)
                                        .WithStereotype(stereotype));
        }

        protected static RepositoryItem AddElement(RepositoryItem library, string stereotype)
        {
            return AddChild(library, ARepositoryItem
                                         .WithItemType(ItemId.ItemType.Element)
                                         .WithParentId(library.Id)
                                         .WithStereotype(stereotype));
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
    }
}