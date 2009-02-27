using System.Collections;
using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EACollection<T> : Collection where T : IEACollectionElement
    {
        private readonly List<T> elements;

        public EACollection(List<T> elements)
        {
            this.elements = elements;
        }

        public static EACollection<EAPackage> Wrap(List<Model> models)
        {
            return new EACollection<EAPackage>(models.ConvertAll(m => new EAPackage(m)));
        }

        public static EACollection<EAPackage> Wrap(List<UpccLibrary> packages)
        {
            return new EACollection<EAPackage>(packages.ConvertAll(p => new EAPackage(p)));
        }

        public static EACollection<EAElement> Wrap(List<UpccClass> classes)
        {
            return new EACollection<EAElement>(classes.ConvertAll(c => new EAElement(c)));
        }

        public static EACollection<EAAttribute> Wrap(List<UpccAttribute> attributes)
        {
            return new EACollection<EAAttribute>(attributes.ConvertAll(a => new EAAttribute(a)));
        }

        public static EACollection<EAConnector> Wrap(List<UpccConnector> connectors)
        {
            return new EACollection<EAConnector>(connectors.ConvertAll(c => new EAConnector(c)));
        }

        public static EACollection<EATaggedValue> Wrap(List<UpccTaggedValue> taggedValues)
        {
            return new EACollection<EATaggedValue>(taggedValues.ConvertAll(tv => new EATaggedValue(tv)));
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

    internal interface IEACollectionElement
    {
        string Name { get; }
    }
}