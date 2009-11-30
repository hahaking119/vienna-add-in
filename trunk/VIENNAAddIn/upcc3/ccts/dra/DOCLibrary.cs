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
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DOCLibrary : BusinessLibrary, IDocLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDocLibrary Members

        public IAbie RootAbie
        {
            get
            {
                var abies = new List<IAbie>(Abies);
                // collect ASBIES
                var asbies = new List<IAsbie>();
                foreach (IAbie abie in Abies)
                {
                    asbies.AddRange(abie.ASBIEs);
                }
                // remove all abies that are associated via an ASBIE
                foreach (IAsbie asbie in asbies)
                {
                    IAbie associatedABIE = asbie.AssociatedElement;
                    abies.Remove(associatedABIE);
                }
                if (abies.Count == 0)
                {
                    return null;
                }
                return abies[0];
            }
        }

        public IEnumerable<IAbie> NonRootAbies
        {
            get {
                var allAbies = new List<IAbie>(Abies);
                allAbies.Remove(RootAbie);
                return allAbies;
            }
        }

        public IEnumerable<IAbie> Abies
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

        public IAbie GetAbieByName(string name)
        {
            foreach (IAbie element in Abies)
            {
                if (((ABIE) element).Name == name)
                {
                    return element;
                }
            }
            return default(IAbie);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IAbie CreateAbie(AbieSpec spec)
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
        public IAbie UpdateAbie(IAbie element, AbieSpec spec)
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