using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public abstract class CCRepositoryTest
    {
        protected static int nextId;

//        protected IEAPackage CreateSubPackage(IEAPackage parent, IEAPackage child)
//        {
//            parent.AddSubPackage(child);
//            return child;
//        }

//        protected TestPackage CreateEAPackage(int parentId)
//        {
//            return new TestPackage(ItemId.ForPackage(parentId));
//        }

//        protected static OtherElement CreateEAElement(IEAPackage package)
//        {
//            var element = new OtherElement(ItemId.ForPackage(++nextId), "Other Element " + nextId, package.Id);
//            package.AddElement(element);
//            return element;
//        }

//        protected static void VerifyValidationIssues(IValidating validatingObject, params ItemId[] validationIssueItemIds)
//        {
//            var validationIssues = new List<IValidationIssue>(validatingObject.Validate());
//            foreach (IValidationIssue validationIssue in validationIssues)
//            {
//                Console.WriteLine(validationIssue);
//            }
//            Assert.AreEqual(validationIssueItemIds.Length, validationIssues.Count);
//            for (int i = 0; i < validationIssueItemIds.Length; ++i)
//            {
//                Assert.AreEqual(validationIssueItemIds[i], validationIssues[i].ItemId);
//            }
//        }

        protected static void VerifyValidationIssues(RepositoryItem item, params ItemId[] validationIssueItemIds)
        {
            var validationIssues = new List<IValidationIssue>(item.Validate());
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
}