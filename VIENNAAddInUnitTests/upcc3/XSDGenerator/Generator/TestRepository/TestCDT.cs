namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestCDT : TestEAElement
    {
        private readonly TestEAAttribute contentAttribute = new ContentAttribute();

        public TestCDT()
        {
            AddAttribute(contentAttribute);
        }

        public override Path Type
        {
            get { return contentAttribute.Type; }
            set
            {
                contentAttribute.Type = value;
            }
        }

        public override string Stereotype
        {
            get { return "CDT"; }
        }

        public override TestRepositoryElement AddSUP()
        {
            return AddAttribute(new SupplementaryAttribute());
        }
    }
}