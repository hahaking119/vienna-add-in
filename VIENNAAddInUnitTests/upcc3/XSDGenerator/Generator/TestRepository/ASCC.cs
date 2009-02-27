namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class ASCC: UpccConnector
    {
        public ASCC(Path pathToACC) : base(pathToACC)
        {
        }

        public override string GetStereotype()
        {
            return "ASCC";
        }
    }
}