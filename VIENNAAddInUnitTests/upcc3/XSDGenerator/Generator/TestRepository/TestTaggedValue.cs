using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestTaggedValue : TestRepositoryElement, TaggedValue
    {
        private readonly string value;

        public TestTaggedValue(string name, string value)
        {
            Name = name;
            this.value = value;
        }

        public bool Update()
        {
            throw new System.NotImplementedException();
        }

        public string GetLastError()
        {
            throw new System.NotImplementedException();
        }

        public int PropertyID
        {
            get { throw new System.NotImplementedException(); }
        }

        public string PropertyGUID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualTaggedValue.Name
        {
            get { return Name; }
            set { throw new System.NotImplementedException(); }
        }

        public override string Stereotype
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Value
        {
            get { return value; }
            set { throw new System.NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int ElementID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new System.NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}