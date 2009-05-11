using NUnit.Framework;
using VIENNAAddInUnitTests.TestRepository;


namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    [TestFixture]
    public class EARepositoryModelCreator : EARepository
    {
        [Test]
        public void Test1()
        {
            SetContent(Package("Test Model 1", ""), 
                       Package("Test Model 2", ""));
        }
    }
}