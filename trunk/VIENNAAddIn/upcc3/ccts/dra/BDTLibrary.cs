// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDTLibrary : BusinessLibrary, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBDTLibrary Members

        public IEnumerable<IBDT> Elements
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(util.Stereotype.BDT))
                    {
                        yield return WrapEaElement(element);
                    }
                }
            }
        }

        public IBDT ElementByName(string name)
        {
            Element eaElement = package.ElementByName(name);
            if (eaElement != null && eaElement.IsA(util.Stereotype.BDT))
            {
                return WrapEaElement(eaElement);
            }
            return null;
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IBDT CreateElement(BDTSpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.BDT;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            BDT cctsElement = WrapEaElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IBDT UpdateElement(IBDT element, BDTSpec spec)
        {
            ((BDT) element).Update(spec);
            return element;
        }

        #endregion

        private BDT WrapEaElement(Element element)
        {
            return new BDT(repository, element);
        }
    }
}