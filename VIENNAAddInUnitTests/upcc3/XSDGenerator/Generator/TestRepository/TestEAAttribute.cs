using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class TestEAAttribute: TestRepositoryElement, Attribute
    {
        public override Path Type { get; set; }

        public override string LowerBound{ get; set;}
        public override string UpperBound{ get; set;}

        public bool Update()
        {
            throw new System.NotImplementedException();
        }

        public string GetLastError()
        {
            throw new System.NotImplementedException();
        }

        public string Visibility
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualAttribute.Stereotype
        {
            get { return Stereotype; }
            set { throw new System.NotImplementedException(); }
        }

        public string Containment
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsStatic
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsCollection
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsOrdered
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool AllowDuplicates
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualAttribute.LowerBound
        {
            get { return (LowerBound ?? "1"); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualAttribute.UpperBound
        {
            get { return (UpperBound ?? "1"); }
            set { throw new System.NotImplementedException(); }
        }

        public string Container
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsDerived
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int AttributeID
        {
            get { throw new System.NotImplementedException(); }
        }

        public int Pos
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Length
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Precision
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Scale
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsConst
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Style
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int ClassifierID
        {
            get { return Type.Resolve().ElementID; }
            set { throw new System.NotImplementedException(); }
        }

        public string Default
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualAttribute.Type
        {
            get { return Type.Resolve().Name; }
            set { throw new System.NotImplementedException(); }
        }

        public Collection Constraints
        {
            get { throw new System.NotImplementedException(); }
        }

        Collection IDualAttribute.TaggedValues
        {
            get { return TaggedValues; }
        }

        public string AttributeGUID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string StyleEx
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

        public string StereotypeEx
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Collection TaggedValuesEx
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}