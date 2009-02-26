namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CON : EAAttribute
    {
        public CON(Path type) : base("Content")
        {
            Type = type;
        }

        public override string Stereotype
        {
            get { return "CON"; }
        }

        public static implicit operator CON(Path typePath)
        {
            return new CON(typePath);
        }
    }
}