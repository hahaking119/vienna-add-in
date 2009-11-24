// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class CCLibrary : BusinessLibrary, ICCLibrary
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="package"></param>
        public CCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICCLibrary Members

        public IEnumerable<IACC> Elements
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(util.Stereotype.ACC))
                    {
                        yield return WrapEaElement(element);
                    }
                }
            }
        }

        public IACC ElementByName(string name)
        {
            foreach (IACC element in Elements)
            {
                if (((ACC) element).Name == name)
                {
                    return element;
                }
            }
            return default(IACC);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IACC CreateElement(ACCSpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.ACC;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            ACC cctsElement = WrapEaElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IACC UpdateElement(IACC element, ACCSpec spec)
        {
            ((ACC) element).Update(spec);
            return element;
        }

        #endregion

        private ACC WrapEaElement(Element element)
        {
            return new ACC(repository, element);
        }
    }
}