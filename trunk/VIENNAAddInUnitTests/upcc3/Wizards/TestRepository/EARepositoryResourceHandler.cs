using NUnit.Framework;
using VIENNAAddInUnitTests.TestRepository;


namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    [TestFixture]
    public class EARepositoryResourceHandler : EARepository
    {
        [Test]
        public void Test1()
        {
            SetContent(Package("Test Model", ""));
        }
    }
}