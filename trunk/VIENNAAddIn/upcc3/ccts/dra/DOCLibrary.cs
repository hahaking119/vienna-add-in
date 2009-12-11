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
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DOCLibrary : BusinessLibrary, IDocLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDocLibrary Members

        public IMa DocumentRoot
        {
            get
            {
                var mas = new List<IMa>(Mas);
                // collect ASMAs
                var asmas = new List<IAsma>();
                foreach (var ma in Mas)
                {
                    if (ma.Asmas != null)
                    {
                        asmas.AddRange(ma.Asmas);
                    }
                }
                // remove all MAs that are associated via an ASMA
                foreach (var asma in asmas)
                {
                    if (asma.AssociatedBieAggregator.IsMa)
                    {
                        mas.Remove(asma.AssociatedBieAggregator.Ma);
                    }
                }
                return mas.Count == 0 ? null : mas[0];
            }
        }

        public IEnumerable<IMa> NonRootMas
        {
            get
            {
                var mas = new List<IMa>(Mas);
                mas.Remove(DocumentRoot);
                return mas;
            }
        }

        public IEnumerable<IMa> Mas
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(util.Stereotype.MA))
                    {
                        yield return WrapEaElement(element);
                    }
                }
            }
        }

        public IMa GetMaByName(string name)
        {
            foreach (IMa ma in Mas)
            {
                if (ma.Name == name)
                {
                    return ma;
                }
            }
            return null;
        }

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IMa CreateMa(MaSpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.ABIE;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            Ma ma = WrapEaElement(element);
            ma.Update(spec);
            return ma;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="ma"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public IMa UpdateMa(IMa ma, MaSpec spec)
        {
            ((Ma) ma).Update(spec);
            return ma;
        }

        public void RemoveMa(IMa ma)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Ma WrapEaElement(Element element)
        {
            return new Ma(repository, element);
        }

        public void Update(DocLibrarySpec specification)
        {
            package.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            package.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            package.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            package.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            package.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            package.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            package.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            package.Element.SetOrGenerateTaggedValue(new TaggedValueSpec(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier), package.PackageGUID);
            package.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            package.Update();
        }
    }
}