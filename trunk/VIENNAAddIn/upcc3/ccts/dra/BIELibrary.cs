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
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BIELibrary : BusinessLibrary, IBieLibrary
    {
        public BIELibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBieLibrary Members

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

        public void RemoveAbie(IAbie abie)
        {
            throw new NotImplementedException();
        }

        #endregion

        private ABIE WrapEaElement(Element element)
        {
            return new ABIE(repository, element);
        }

        public void Update(BieLibrarySpec specification)
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