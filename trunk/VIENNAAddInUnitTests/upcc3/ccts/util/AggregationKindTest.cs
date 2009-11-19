using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using CctsRepository;

namespace VIENNAAddInUnitTests.upcc3.ccts.util
{
    [TestFixture]
    public class AggregationKindTest
    {
        [Test]
        public void TestEnumFromInt()
        {
            Assert.AreEqual(EAAggregationKind.None, (EAAggregationKind)Enum.ToObject(typeof(EAAggregationKind), 0));
            Assert.AreEqual(EAAggregationKind.Shared, Enum.ToObject(typeof(EAAggregationKind), 1));
            Assert.AreEqual(EAAggregationKind.Composite, Enum.ToObject(typeof(EAAggregationKind), 2));
            Assert.AreEqual(false, Enum.IsDefined(typeof (EAAggregationKind), -1));
            Assert.AreEqual(false, Enum.IsDefined(typeof (EAAggregationKind), 3));
        }

        [Test]
        public void TestEnumToInt()
        {
            Assert.AreEqual(0, (int) EAAggregationKind.None);
            Assert.AreEqual(1, (int) EAAggregationKind.Shared);
            Assert.AreEqual(2, (int) EAAggregationKind.Composite);
        }
    }
}