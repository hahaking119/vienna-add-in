using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    /// <summary>
    /// Note: This file cannot be named 'CON' after the class, because 'CON' is a reserved name for windows file systems.
    /// </summary>
    internal class CON : UpccAttribute
    {
        public CON()
        {
            Name = "Content";
        }

        public override string GetStereotype()
        {
            return "CON";
        }

        public static implicit operator CON(Path typePath)
        {
            return new CON {Type = typePath};
        }
    }
}