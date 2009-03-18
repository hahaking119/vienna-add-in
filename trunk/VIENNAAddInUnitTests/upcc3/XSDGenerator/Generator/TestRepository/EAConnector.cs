// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using EA;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EAConnector : IEACollectionElement, Connector
    {
        private int supplierID;
        public EARepository Repository { get; set; }

        public Path SupplierPath { get; set; }

        #region Connector Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public int ConnectorID
        {
            get { throw new NotImplementedException(); }
        }

        public string Name { get; set; }

        public string Direction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Subtype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ClientID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int SupplierID
        {
            get
            {
                return SupplierPath != null ? SupplierPath.Resolve<Element>(Repository).ElementID : supplierID;
            }
            set { supplierID = value; }
        }

        public int SequenceNo
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Stereotype { get; set; }

        public string VirtualInheritance
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ConnectorGUID
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRoot
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsLeaf
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsSpec
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValues
        {
            get { throw new NotImplementedException(); }
        }

        public ConnectorEnd ClientEnd
        {
            get { throw new NotImplementedException(); }
        }

        public ConnectorEnd SupplierEnd
        {
            get { throw new NotImplementedException(); }
        }

        public int RouteStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string EventFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string TransitionEvent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string TransitionGuard
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string TransitionAction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int Color
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Width
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection CustomProperties
        {
            get { throw new NotImplementedException(); }
        }

        public int DiagramID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int StartPointX
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int StartPointY
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int EndPointX
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int EndPointY
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string StateFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Properties Properties
        {
            get { throw new NotImplementedException(); }
        }

        public string MetaType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Alias
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string get_MiscData(int index)
        {
            throw new NotImplementedException();
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