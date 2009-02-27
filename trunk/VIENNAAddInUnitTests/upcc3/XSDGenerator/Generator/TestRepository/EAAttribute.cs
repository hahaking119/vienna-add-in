using System;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EAAttribute : Attribute, IEACollectionElement
    {
        private readonly UpccAttribute attribute;

        public EAAttribute(UpccAttribute attribute)
        {
            this.attribute = attribute;
        }

        #region Attribute Members

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        string IDualAttribute.Name
        {
            get { return Name; }
            set { throw new NotImplementedException(); }
        }

        public string Visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IDualAttribute.Stereotype
        {
            get { return attribute.GetStereotype(); }
            set { throw new NotImplementedException(); }
        }

        public string Containment
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsStatic
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsCollection
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsOrdered
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool AllowDuplicates
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IDualAttribute.LowerBound
        {
            get { return (attribute.LowerBound ?? "1"); }
            set { throw new NotImplementedException(); }
        }

        string IDualAttribute.UpperBound
        {
            get { return (attribute.UpperBound ?? "1"); }
            set { throw new NotImplementedException(); }
        }

        public string Container
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsDerived
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int AttributeID
        {
            get { throw new NotImplementedException(); }
        }

        public int Pos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Length
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Precision
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Scale
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsConst
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Style
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ClassifierID
        {
            get { return attribute.Type.Resolve().ElementID; }
            set { throw new NotImplementedException(); }
        }

        public string Default
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IDualAttribute.Type
        {
            get { return attribute.Type.Resolve().Name; }
            set { throw new NotImplementedException(); }
        }

        public Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        Collection IDualAttribute.TaggedValues
        {
            get { return EACollection<EATaggedValue>.Wrap(attribute.GetTaggedValues()); }
        }

        public string AttributeGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection TaggedValuesEx
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEACollectionElement Members

        public string Name
        {
            get { return attribute.GetName(); }
        }

        #endregion
    }
}