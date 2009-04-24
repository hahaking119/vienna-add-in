using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    [TestFixture]
    public class EARepositoryCache : EARepository
    {
        [Test]
        public void Test1()
        {
            SetContent();
        }
    }
}