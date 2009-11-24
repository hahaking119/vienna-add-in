// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DOCLibrary : BusinessLibrary, IDOCLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDOCLibrary Members

        public IEnumerable<IABIE> RootElements
        {
            get
            {
                var abies = new List<IABIE>(Elements);
                // collect ASBIES
                var asbies = new List<IASBIE>();
                foreach (IABIE abie in Elements)
                {
                    asbies.AddRange(abie.ASBIEs);
                }
                // remove all abies that are associated via an ASBIE
                foreach (IASBIE asbie in asbies)
                {
                    IABIE associatedABIE = asbie.AssociatedElement;
                    abies.Remove(associatedABIE);
                }
                return abies;
            }
        }

        public IEnumerable<IABIE> Elements
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(util.Stereotype.ABIE))
                    {
                        yield return WrapEaElement(element);
                    }
                }
            }
        }

        public IABIE ElementByName(string name)
        {
            foreach (IABIE element in Elements)
            {
                if (((ABIE) element).Name == name)
                {
                    return element;
                }
            }
            return default(IABIE);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IABIE CreateElement(ABIESpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.ABIE;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            ABIE cctsElement = WrapEaElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IABIE UpdateElement(IABIE element, ABIESpec spec)
        {
            ((ABIE) element).Update(spec);
            return element;
        }

        #endregion

        private ABIE WrapEaElement(Element element)
        {
            return new ABIE(repository, element);
        }
    }
}