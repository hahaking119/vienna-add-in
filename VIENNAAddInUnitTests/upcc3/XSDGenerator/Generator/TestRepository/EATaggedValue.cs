using System;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EATaggedValue : IEACollectionElement, TaggedValue
    {
        private readonly UpccTaggedValue tv;

        public EATaggedValue(UpccTaggedValue tv)
        {
            this.tv = tv;
        }

        #region IEACollectionElement Members

        public string Name
        {
            get { return tv.Name; }
        }

        #endregion

        #region TaggedValue Members

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public int PropertyID
        {
            get { throw new NotImplementedException(); }
        }

        public string PropertyGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IDualTaggedValue.Name
        {
            get { return Name; }
            set { throw new NotImplementedException(); }
        }

        public string Value
        {
            get { return tv.Value; }
            set { throw new NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ElementID
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

        #endregion
    }
}