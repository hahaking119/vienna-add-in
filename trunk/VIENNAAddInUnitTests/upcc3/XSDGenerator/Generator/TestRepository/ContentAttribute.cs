namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class ContentAttribute : TestEAAttribute
    {
        public ContentAttribute()
        {
            Name = "Content";
        }

        public override string Stereotype
        {
            get { return "CON"; }
        }
    }
}