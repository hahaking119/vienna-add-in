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
using CctsRepository.CcLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class CCLibrary : BusinessLibrary, ICcLibrary
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="package"></param>
        public CCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICcLibrary Members

        public IEnumerable<IAcc> Accs
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

        public IAcc GetAccByName(string name)
        {
            foreach (IAcc element in Accs)
            {
                if (((ACC) element).Name == name)
                {
                    return element;
                }
            }
            return default(IAcc);
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IAcc CreateAcc(AccSpec spec)
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
        public IAcc UpdateAcc(IAcc element, AccSpec spec)
        {
            ((ACC) element).Update(spec);
            return element;
        }

        public void RemoveAcc(IAcc acc)
        {
            throw new NotImplementedException();
        }

        #endregion

        private ACC WrapEaElement(Element element)
        {
            return new ACC(repository, element);
        }

        public void Update(CcLibrarySpec specification)
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