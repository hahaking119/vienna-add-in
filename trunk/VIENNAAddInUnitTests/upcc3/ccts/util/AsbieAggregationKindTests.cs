using System;
using CctsRepository;
using CctsRepository.BieLibrary;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.ccts.util
{
    [TestFixture]
    public class AsbieAggregationKindTests
    {
        [Test]
        public void MatchesIntValuesUsedByEA()
        {
            Assert.AreEqual(AsbieAggregationKind.Shared, Enum.ToObject(typeof (AsbieAggregationKind), 1));
            Assert.AreEqual(AsbieAggregationKind.Composite, Enum.ToObject(typeof (AsbieAggregationKind), 2));

            Assert.AreEqual(false, Enum.IsDefined(typeof (AsbieAggregationKind), -1));
            Assert.AreEqual(false, Enum.IsDefined(typeof (AsbieAggregationKind), 0));
            Assert.AreEqual(false, Enum.IsDefined(typeof (AsbieAggregationKind), 3));

            Assert.AreEqual(1, (int) AsbieAggregationKind.Shared);
            Assert.AreEqual(2, (int) AsbieAggregationKind.Composite);
        }
    }
}