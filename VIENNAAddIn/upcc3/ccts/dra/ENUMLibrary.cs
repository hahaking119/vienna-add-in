// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.EnumLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUMLibrary : BusinessLibrary, IENUMLibrary
    {
        public ENUMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IENUMLibrary Members

        public IEnumerable<IENUM> Elements
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(util.Stereotype.ENUM))
                    {
                        yield return WrapEaElement(element);
                    }
                }
            }
        }

        public IENUM ElementByName(string name)
        {
            foreach (IENUM element in Elements)
            {
                if (((ENUM) element).Name == name)
                {
                    return element;
                }
            }
            return default(IENUM);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IENUM CreateElement(ENUMSpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.ENUM;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            ENUM cctsElement = WrapEaElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IENUM UpdateElement(IENUM element, ENUMSpec spec)
        {
            ((ENUM) element).Update(spec);
            return element;
        }

        #endregion

        private ENUM WrapEaElement(Element element)
        {
            return new ENUM(repository, element);
        }
    }
}