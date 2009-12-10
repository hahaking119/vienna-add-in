// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class CDTLibrary : BusinessLibrary, ICdtLibrary
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="package"></param>
        public CDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICdtLibrary Members

        public IEnumerable<ICdt> Cdts
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

        public ICdt GetCdtByName(string name)
        {
            foreach (ICdt element in Cdts)
            {
                if (((CDT) element).Name == name)
                {
                    return element;
                }
            }
            return default(ICdt);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public ICdt CreateCdt(CdtSpec spec)
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
        public ICdt UpdateCdt(ICdt element, CdtSpec spec)
        {
            ((CDT) element).Update(spec);
            return element;
        }

        public void RemoveCdt(ICdt cdt)
        {
            throw new NotImplementedException();
        }

        #endregion

        private CDT WrapEaElement(Element element)
        {
            return new CDT(repository, element);
        }
    }
}