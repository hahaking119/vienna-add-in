using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class TestRepositoryElement
    {
        private static int nextId;
        protected readonly List<TestTaggedValue> taggedValues = new List<TestTaggedValue>();

        protected TestRepositoryElement()
        {
            Id = nextId++;
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public virtual Path Type
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public abstract string Stereotype { get; }

        public virtual string BaseURN
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual Collection TaggedValues
        {
            get { return new TestEACollection<TestTaggedValue>(taggedValues); }
        }

        public virtual string LowerBound
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string UpperBound
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        protected void AddTaggedValue(string name, string value)
        {
            taggedValues.Add(new TestTaggedValue(name, value));
        }

        public virtual TestRepositoryElement AddBLibrary()
        {
            throw new NotImplementedException();
        }

        public virtual TestRepositoryElement AddCDTLibrary()
        {
            throw new NotImplementedException();
        }

        public virtual TestRepositoryElement AddCDT()
        {
            throw new NotImplementedException();
        }

        public virtual TestRepositoryElement AddPRIMLibrary()
        {
            throw new NotImplementedException();
        }

        public virtual TestRepositoryElement AddPRIM()
        {
            throw new NotImplementedException();
        }

        public virtual TestRepositoryElement AddSUP()
        {
            throw new NotImplementedException();
        }
    }
}