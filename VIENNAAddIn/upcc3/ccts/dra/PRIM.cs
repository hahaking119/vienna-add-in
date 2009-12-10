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
using System.Linq;
using CctsRepository;
using CctsRepository.PrimLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIM : IPrim, IEquatable<PRIM>
    {
        private readonly Element element;
        private readonly CCRepository repository;

        public PRIM(CCRepository repository, Element element)
        {
            this.repository = repository;
            this.element = element;
        }

        protected IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        private IEnumerable<Connector> Connectors
        {
            get { return element.Connectors.AsEnumerable<Connector>(); }
        }

        #region IEquatable<PRIM> Members

        public bool Equals(PRIM other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.element.ElementID, element.ElementID);
        }

        #endregion

        #region IPrim Members

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName).DefaultTo(Name); }
        }

        public string Pattern
        {
            get { return GetTaggedValue(TaggedValues.pattern); }
        }

        public string FractionDigits
        {
            get { return GetTaggedValue(TaggedValues.fractionDigits); }
        }

        public string Length
        {
            get { return GetTaggedValue(TaggedValues.length); }
        }

        public string MaximumExclusive
        {
            get { return GetTaggedValue(TaggedValues.maximumExclusive); }
        }

        public string MaximumInclusive
        {
            get { return GetTaggedValue(TaggedValues.maximumInclusive); }
        }

        public string MaximumLength
        {
            get { return GetTaggedValue(TaggedValues.maximumLength); }
        }

        public string MinimumExclusive
        {
            get { return GetTaggedValue(TaggedValues.minimumExclusive); }
        }

        public string MinimumInclusive
        {
            get { return GetTaggedValue(TaggedValues.minimumInclusive); }
        }

        public string MinimumLength
        {
            get { return GetTaggedValue(TaggedValues.minimumLength); }
        }

        public string TotalDigits
        {
            get { return GetTaggedValue(TaggedValues.totalDigits); }
        }

        public string WhiteSpace
        {
            get { return GetTaggedValue(TaggedValues.whiteSpace); }
        }

        public IPrim IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetPrimById(connector.SupplierID) : null;
            }
        }

        ///<summary>
        ///</summary>
        public int Id
        {
            get { return element.ElementID; }
        }

        ///<summary>
        ///</summary>
        public string Name
        {
            get { return element.Name; }
        }

        ///<summary>
        ///</summary>
        public IPrimLibrary PrimLibrary
        {
            get { return repository.GetPrimLibraryById(element.PackageID); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.businessTerm); }
        }

        ///<summary>
        ///</summary>
        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier); }
        }

        ///<summary>
        ///</summary>
        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier); }
        }

        ///<summary>
        ///</summary>
        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.languageCode); }
        }

        #endregion

        private static IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(PrimSpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                       new TaggedValueSpec(TaggedValues.pattern, spec.Pattern),
                       new TaggedValueSpec(TaggedValues.fractionDigits, spec.FractionDigits),
                       new TaggedValueSpec(TaggedValues.length, spec.Length),
                       new TaggedValueSpec(TaggedValues.maximumExclusive, spec.MaximumExclusive),
                       new TaggedValueSpec(TaggedValues.maximumInclusive, spec.MaximumInclusive),
                       new TaggedValueSpec(TaggedValues.maximumLength, spec.MaximumLength),
                       new TaggedValueSpec(TaggedValues.minimumExclusive, spec.MinimumExclusive),
                       new TaggedValueSpec(TaggedValues.minimumInclusive, spec.MinimumInclusive),
                       new TaggedValueSpec(TaggedValues.minimumLength, spec.MinimumLength),
                       new TaggedValueSpec(TaggedValues.totalDigits, spec.TotalDigits),
                       new TaggedValueSpec(TaggedValues.whiteSpace, spec.WhiteSpace),
                   };
        }

        private static IEnumerable<ConnectorSpec> GetConnectorSpecs(PrimSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.isEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is PRIM) return Equals((PRIM) obj);
            return false;
        }

        public override int GetHashCode()
        {
            return (element != null ? element.GetHashCode() : 0);
        }

        private string GetTaggedValue(TaggedValues key)
        {
            return element.GetTaggedValue(key) ?? string.Empty;
        }

        public void Update(PrimSpec spec)
        {
            element.Name = spec.Name;
            foreach (TaggedValueSpec taggedValueSpec in GetTaggedValueSpecs(spec))
            {
                string value = taggedValueSpec.Value;

                if (string.IsNullOrEmpty(value))
                {
                    switch (taggedValueSpec.Key)
                    {
                        case TaggedValues.dictionaryEntryName:
                            value = element.Name;
                            break;

                        case TaggedValues.uniqueIdentifier:
                            value = element.ElementGUID;
                            break;
                    }
                }

                element.SetTaggedValue(taggedValueSpec.Key, value);
            }

            for (var i = (short) (element.Connectors.Count - 1); i >= 0; i--)
            {
                if (((Connector) element.Connectors.GetAt(i)).IsIsEquivalentTo())
                {
                    element.Connectors.Delete(i);
                }
            }
            element.Connectors.Refresh();
            foreach (ConnectorSpec connector in GetConnectorSpecs(spec))
            {
                element.AddConnector(connector);
            }
            element.Connectors.Refresh();

            element.Update();
            element.Refresh();
        }
    }
}