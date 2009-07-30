using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public static class AssertCollections
    {
        public static void AreEqual<T>(IEnumerable<T> expectedCollection, IEnumerable<T> actualCollection, Func<T, int> comp)
        {
            var expectedList = new List<T>(expectedCollection.OrderBy(comp));
            var actualList = new List<T>(actualCollection.OrderBy(comp));
            Assert.AreEqual(expectedList.Count, actualList.Count, "Collections do not have the same number of elements");
            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.AreEqual(expectedList[i], actualList[i], "Elements at index " + i + " are not equal");
            }
        }
    }
}