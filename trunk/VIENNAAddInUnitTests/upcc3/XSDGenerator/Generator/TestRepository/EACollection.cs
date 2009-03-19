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

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class EACollection<TCollectionElement> : Collection
        where TCollectionElement : class, IEACollectionElement, new()
    {
        private readonly List<TCollectionElement> elements = new List<TCollectionElement>();

        public List<TCollectionElement> Elements
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
            var element = elements.Find(e => e.Name == name);
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
            var element = new TCollectionElement {Name = Name};
            elements.Add(element);
            return element;
        }

        public void Delete(short index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IDualCollection.GetEnumerator()
        {
            return GetEnumerator();
        }

        public short Count
        {
            get { return (short) elements.Count; }
        }

        public ObjectType ObjectType
        {
            get
            {
                if (typeof (TCollectionElement) == typeof (EAAttribute))
                {
                    return ObjectType.otAttribute;
                }
                if (typeof (TCollectionElement) == typeof (EAConnector))
                {
                    return ObjectType.otConnector;
                }
                if (typeof (TCollectionElement) == typeof (EAElement))
                {
                    return ObjectType.otElement;
                }
                if (typeof (TCollectionElement) == typeof (EAPackage))
                {
                    return ObjectType.otPackage;
                }
                if (typeof (TCollectionElement) == typeof (EATaggedValue))
                {
                    return ObjectType.otTaggedValue;
                }
                if (typeof (TCollectionElement) == typeof (EAAttributeTag))
                {
                    return ObjectType.otAttributeTag;
                }
                if (typeof (TCollectionElement) == typeof (EAConnectorTag))
                {
                    return ObjectType.otConnectorTag;
                }
                return ObjectType.otNone;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        #endregion
    }

    public interface IEACollectionElement
    {
        string Name { get; set; }
    }
}