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
using CctsRepository.PrimLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIMLibrary : BusinessLibrary, IPrimLibrary
    {
        public PRIMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IPrimLibrary Members

        public IEnumerable<IPrim> Prims
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

        public IPrim GetPrimByName(string name)
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
        public IPrim CreatePrim(PrimSpec spec)
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
        public IPrim UpdatePrim(IPrim element, PrimSpec spec)
        {
            ((PRIM) element).Update(spec);
            return element;
        }

        public void RemovePrim(IPrim prim)
        {
            throw new NotImplementedException();
        }

        #endregion

        private PRIM WrapEaElement(Element element)
        {
            return new PRIM(repository, element);
        }

        public void Update(PrimLibrarySpec specification)
        {
            package.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            package.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            package.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            package.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            package.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            package.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            package.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            package.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier.DefaultTo(package.PackageGUID));
            package.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            package.Update();
        }
    }
}