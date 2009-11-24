// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CdtLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class CDTLibrary : BusinessLibrary, ICDTLibrary
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="package"></param>
        public CDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICDTLibrary Members

        public IEnumerable<ICDT> Elements
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(util.Stereotype.CDT))
                    {
                        yield return WrapEaElement(element);
                    }
                }
            }
        }

        public ICDT ElementByName(string name)
        {
            foreach (ICDT element in Elements)
            {
                if (((CDT) element).Name == name)
                {
                    return element;
                }
            }
            return default(ICDT);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public ICDT CreateElement(CDTSpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.CDT;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            CDT cctsElement = WrapEaElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public ICDT UpdateElement(ICDT element, CDTSpec spec)
        {
            ((CDT) element).Update(spec);
            return element;
        }

        #endregion

        private CDT WrapEaElement(Element element)
        {
            return new CDT(repository, element);
        }
    }
}