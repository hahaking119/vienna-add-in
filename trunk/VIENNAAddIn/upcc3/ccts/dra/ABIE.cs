using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ABIE : UpccElement, IABIE
    {
        protected CCRepository repository;

        public ABIE(CCRepository repository, Element element) : base(element, "ABIE")
        {
            this.repository = repository;
        }

        private IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        private IEnumerable<Connector> Connectors
        {
            get { return element.Connectors.AsEnumerable<Connector>(); }
        }

        public IBusinessLibrary Library
        {
            get { return repository.GetLibrary(element.PackageID); }
        }

        #region IABIE Members

        public IEnumerable<IBBIE> BBIEs
        {
            get
            {
                return
                    Attributes.Convert(a => (IBBIE) new BBIE(repository, a));
            }
        }

        public IEnumerable<IASBIE> ASBIEs
        {
            get
            {
                return
                    Connectors.Where(IsASBIE).Convert(c => (IASBIE) new ASBIE(repository, c));
            }
        }

        public IACC BasedOn
        {
            get
            {
                Connector connector =
                    Connectors.FirstOrDefault(IsBasedOnDependency);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        public IABIE IsEquivalentTo
        {
            get
            {
                Connector connector =
                    Connectors.FirstOrDefault(IsEquivalentToDependency);
                return connector != null ? repository.GetABIE(connector.SupplierID) : null;
            }
        }

        public int Id
        {
            get { return element.ElementID; }
        }

        public string Name
        {
            get { return element.Name; }
        }

        public string UniqueIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public string Definition
        {
            get { return element.GetTaggedValue(TaggedValues.Definition); }
        }

        public string DictionaryEntryName
        {
            get { return element.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string LanguageCode
        {
            get { return element.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        #endregion

        private static bool IsASBIE(Connector con)
        {
            return con.Stereotype == "ASBIE";
        }

        private static bool IsBasedOnDependency(Connector connector)
        {
            return connector.Stereotype == "basedOn";
        }

        private static bool IsEquivalentToDependency(Connector con)
        {
            return con.Stereotype == "isEquivalentTo";
        }
    }
}