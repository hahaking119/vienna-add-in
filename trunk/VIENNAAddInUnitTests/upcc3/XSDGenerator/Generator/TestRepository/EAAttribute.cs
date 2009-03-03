using System;
using EA;
using VIENNAAddIn.upcc3.ccts;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EAAttribute : Attribute, IEACollectionElement
    {
        private readonly Collection taggedValues = new EACollection<EATaggedValue>();
        private int classifierId;
        public Repository Repository { get; set; }
        public Path ClassifierPath { get; set; }

        #region Attribute Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public string Visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Stereotype { get; set; }

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

        public string LowerBound { get; set; }

        public string UpperBound { get; set; }

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
            get { return (ClassifierPath != null ? ClassifierPath.Resolve<Element>(Repository).ElementID : classifierId); }
            set { classifierId = value; }
        }

        public string Default
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { return ClassifierPath.Resolve<Element>(Repository).Name; }
            set { }
        }

        public Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValues
        {
            get { return taggedValues; }
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

        string IEACollectionElement.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion
    }
}