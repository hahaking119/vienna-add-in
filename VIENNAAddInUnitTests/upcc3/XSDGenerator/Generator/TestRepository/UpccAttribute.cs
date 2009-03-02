using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public abstract class UpccAttribute : UpccElement
    {
        public Path Type { get; set; }
        private string lowerBound = "1";
        public string LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        private string upperBound = "1";
        public string UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }
    }
}