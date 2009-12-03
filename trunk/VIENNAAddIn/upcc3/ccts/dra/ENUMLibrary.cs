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
using CctsRepository.EnumLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUMLibrary : BusinessLibrary, IEnumLibrary
    {
        public ENUMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IEnumLibrary Members

        public IEnumerable<IEnum> Enums
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

        public IEnum GetEnumByName(string name)
        {
            foreach (IEnum element in Enums)
            {
                if (((ENUM) element).Name == name)
                {
                    return element;
                }
            }
            return default(IEnum);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IEnum CreateEnum(EnumSpec spec)
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
        public IEnum UpdateEnum(IEnum element, EnumSpec spec)
        {
            ((ENUM) element).Update(spec);
            return element;
        }

        public IEnumerable<IIdScheme> IdSchemes
        {
            get { throw new NotImplementedException(); }
        }

        public IIdScheme GetIdSchemeByName(string name)
        {
            throw new NotImplementedException();
        }

        public IIdScheme CreateIdScheme(IdSchemeSpec specification)
        {
            throw new NotImplementedException();
        }

        public IIdScheme UpdateIdScheme(IIdScheme idScheme, IdSchemeSpec specification)
        {
            throw new NotImplementedException();
        }

        #endregion

        private ENUM WrapEaElement(Element element)
        {
            return new ENUM(repository, element);
        }
    }
}