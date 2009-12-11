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
using CctsRepository.BdtLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDTLibrary : BusinessLibrary, IBdtLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBdtLibrary Members

        public IEnumerable<IBdt> Bdts
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

        public IBdt GetBdtByName(string name)
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
        public IBdt CreateBdt(BdtSpec spec)
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
        public IBdt UpdateBdt(IBdt element, BdtSpec spec)
        {
            ((BDT) element).Update(spec);
            return element;
        }

        public void RemoveBdt(IBdt bdt)
        {
            throw new NotImplementedException();
        }

        #endregion

        private BDT WrapEaElement(Element element)
        {
            return new BDT(repository, element);
        }

        public void Update(BdtLibrarySpec specification)
        {
            package.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            package.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            package.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            package.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            package.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            package.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            package.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            package.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            package.Element.SetOrGenerateTaggedValue(new TaggedValueSpec(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier), package.PackageGUID);
            package.Update();
        }
    }
}