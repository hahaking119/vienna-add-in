namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccAttribute : UpccElement
    {
        protected UpccAttribute(string name) : base(name)
        {
        }

        public Path Type { get; set; }
        public string LowerBound { get; set; }
        public string UpperBound { get; set; }
    }
}