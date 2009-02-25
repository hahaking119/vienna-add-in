using System.Collections;
using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestEACollection<T> : Collection where T : TestRepositoryElement
    {
        private readonly List<T> elements;

        public TestEACollection(List<T> elements)
        {
            this.elements = elements;
        }

        public object GetAt(short index)
        {
            return elements[index];
        }

        public void DeleteAt(short index, bool Refresh)
        {
            throw new System.NotImplementedException();
        }

        public string GetLastError()
        {
            throw new System.NotImplementedException();
        }

        public object GetByName(string name)
        {
            return elements.Find(e => e.Name == name);
        }

        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        public object AddNew(string Name, string Type)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(short index)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IDualCollection.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public short Count
        {
            get { return (short) elements.Count; }
        }

        public ObjectType ObjectType
        {
            get { throw new System.NotImplementedException(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }
}