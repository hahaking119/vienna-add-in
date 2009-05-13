// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    public abstract class EACollection : Collection
    {
        #region Delegates

        public delegate IEACollectionElement ElementFactory(string name, string type, int containerId);

        #endregion

        private readonly int containerId;
        private readonly ElementFactory elementFactory;
        private readonly List<IEACollectionElement> elements = new List<IEACollectionElement>();

        protected EACollection(ObjectType objectType, ElementFactory elementFactory, int containerId)
        {
            ObjectType = objectType;
            this.elementFactory = elementFactory;
            this.containerId = containerId;
        }

        public List<IEACollectionElement> Elements
        {
            get { return elements; }
        }

        #region Collection Members

        public object GetAt(short index)
        {
            return elements[index];
        }

        public void DeleteAt(short index, bool Refresh)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public object GetByName(string name)
        {
            if (elements.Count == 0)
            {
                return null;
            }
            IEACollectionElement element = elements.Find(e => e.Name == name);
            if (element == null)
            {
                throw new IndexOutOfRangeException(name);
            }
            return element;
        }

        public void Refresh()
        {
            // do nothing
        }

        public object AddNew(string Name, string Type)
        {
            IEACollectionElement element = elementFactory(Name, Type, containerId);
            elements.Add(element);
            return element;
        }

        public void Delete(short index)
        {
            elements.RemoveAt(index);
        }

        IEnumerator IDualCollection.GetEnumerator()
        {
            return GetEnumerator();
        }

        public short Count
        {
            get { return (short) elements.Count; }
        }

        public ObjectType ObjectType { get; set; }

        public IEnumerator GetEnumerator()
        {
            return new List<IEACollectionElement>(elements).GetEnumerator();
        }

        #endregion
    }

    internal class EATaggedValueCollection : EACollection
    {
        public EATaggedValueCollection(EARepository repository, EAElement element)
            : base(ObjectType.otTaggedValue, repository.CreateTaggedValue, element.ElementID)
        {
        }
    }

    internal class EADiagramObjectCollection : EACollection
    {
        public EADiagramObjectCollection(EARepository repository, EADiagram diagram)
            : base(ObjectType.otDiagramObject, repository.CreateDiagramObject, diagram.DiagramID)
        {
        }
    }

    internal class EAAttributeTagCollection : EACollection
    {
        public EAAttributeTagCollection(EARepository repository, EAAttribute attribute)
            : base(ObjectType.otAttributeTag, repository.CreateAttributeTag, attribute.AttributeID)
        {
        }
    }

    internal class EAConnectorTagCollection : EACollection
    {
        public EAConnectorTagCollection(EARepository repository, EAConnector connector)
            : base(ObjectType.otConnectorTag, repository.CreateConnectorTag, connector.ConnectorID)
        {
        }
    }

    internal class EAPackageCollection : EACollection
    {
        public EAPackageCollection(EARepository repository, EAPackage parent)
            : base(ObjectType.otPackage, repository.CreatePackage, parent != null ? parent.PackageID : 0)
        {
        }
    }

    internal class EAAttributeCollection : EACollection
    {
        public EAAttributeCollection(EARepository repository, EAElement element)
            : base(ObjectType.otAttribute, repository.CreateAttribute, element.ElementID)
        {
        }
    }

    internal class EAConnectorCollection : EACollection
    {
        public EAConnectorCollection(EARepository repository, EAElement element)
            : base(ObjectType.otConnector, repository.CreateConnector, element.ElementID)
        {
        }
    }

    internal class EAElementCollection : EACollection
    {
        public EAElementCollection(EARepository repository, EAPackage package)
            : base(ObjectType.otElement, repository.CreateElement, package.PackageID)
        {
        }
    }

    internal class EADiagramCollection : EACollection
    {
        public EADiagramCollection(EARepository repository, EAPackage package)
            : base(ObjectType.otDiagram, repository.CreateDiagram, package.PackageID)
        {
        }
    }

    public interface IEACollectionElement
    {
        string Name { get; set; }
    }
}