/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using VIENNAAddIn.Utils; 

namespace VIENNAAddIn.Exceptions
{
    /// <sUMM2ary>
    /// SUMM2ary description for UnexpectedElementException.
    /// </sUMM2ary>
    internal class UnexpectedElementException : AddInException
    {


        private String sub;
        private IList affectedelements;
        internal String SubMessage
        {
            get { return sub; }
        }
        internal IList AffectedElements
        {
            get
            {
                return affectedelements;
            }
        }




        internal UnexpectedElementException()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        internal UnexpectedElementException(String message)
            : base(message)
        {
        }


        internal UnexpectedElementException(String message, String submessage)
            : base(message)
        {
            this.sub = submessage;
        }

        internal UnexpectedElementException(String message, String submessage, EA.Connector con)
            : base(message)
        {
            this.sub = submessage;
            this.affectedelements = new ArrayList();
            this.affectedelements.Add(new EAElementWrapper(con));
        }

        internal UnexpectedElementException(String message, String submessage, EA.Element affectedElement)
            : base(message)
        {
            this.sub = submessage;
            this.affectedelements = new ArrayList();
            this.affectedelements.Add(new EAElementWrapper(affectedElement));
        }

        internal UnexpectedElementException(String message, String submessage, EA.Package affectedElement)
            : base(message)
        {
            this.sub = submessage;
            this.affectedelements = new ArrayList();
            this.affectedelements.Add(new EAElementWrapper(affectedElement));
        }

        internal UnexpectedElementException(String message, String submessage, EA.Diagram affectedElement)
            : base(message)
        {
            this.sub = submessage;
            this.affectedelements = new ArrayList();
            this.affectedelements.Add(new EAElementWrapper(affectedElement));
        }

        internal UnexpectedElementException(String message, String submessage, IList affectedElements)
            : base(message)
        {
            this.sub = submessage;
            //			ArrayList list = new ArrayList();
            //			if (affectedElements != null && affectedElements.Count != 0) {
            //				foreach (object o in affectedElements) {
            //					EAElementWrapper ew = null;
            //					if (o.GetType().Equals(typeof (EA.Diagram))) { 
            //						ew = new EAElementWrapper((EA.Diagram)o);					
            //					}
            //					else if (o.GetType().Equals(typeof (EA.Element))) {
            //						ew = new EAElementWrapper((EA.Element)o);
            //					}
            //					else if (o.GetType().Equals(typeof (EA.Package))) {
            //						ew = new EAElementWrapper((EA.Package)o);
            //					}
            //					else if (o.GetType().Equals(typeof (EA.Connector))) {
            //						ew = new EAElementWrapper((EA.Connector)o);						
            //					}
            //					list.Add(ew);
            //				}
            //			}
            //			this.affectedelements = list;
            this.affectedelements = affectedElements;

        }




    }
}
