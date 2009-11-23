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
            Assert.AreEqual(AggregationKind.None, (AggregationKind)Enum.ToObject(typeof(AggregationKind), 0));
            Assert.AreEqual(AggregationKind.Shared, Enum.ToObject(typeof(AggregationKind), 1));
            Assert.AreEqual(AggregationKind.Composite, Enum.ToObject(typeof(AggregationKind), 2));
            Assert.AreEqual(false, Enum.IsDefined(typeof (AggregationKind), -1));
            Assert.AreEqual(false, Enum.IsDefined(typeof (AggregationKind), 3));
        }

        [Test]
        public void TestEnumToInt()
        {
            Assert.AreEqual(0, (int) AggregationKind.None);
            Assert.AreEqual(1, (int) AggregationKind.Shared);
            Assert.AreEqual(2, (int) AggregationKind.Composite);
        }
    }
}