// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.PrimLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIMLibrary : BusinessLibrary, IPrimLibrary
    {
        public PRIMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IPrimLibrary Members

        public IEnumerable<IPrim> Elements
        {
            get
            {
                foreach (Element eaElement in package.Elements)
                {
                    if (eaElement.IsA(util.Stereotype.PRIM))
                    {
                        yield return WrapEaElement(eaElement);
                    }
                }
            }
        }

        public IPrim ElementByName(string name)
        {
            Element eaElement = package.ElementByName(name);
            if (eaElement != null && eaElement.IsA(util.Stereotype.PRIM))
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
        public IPrim CreateElement(PrimSpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.PRIM;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            PRIM cctsElement = WrapEaElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IPrim UpdateElement(IPrim element, PrimSpec spec)
        {
            ((PRIM) element).Update(spec);
            return element;
        }

        #endregion

        private PRIM WrapEaElement(Element element)
        {
            return new PRIM(repository, element);
        }
    }
}