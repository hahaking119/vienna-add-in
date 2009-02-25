using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    class ABIE : IABIE
    {
        protected CCRepository repository;
        protected Element element;

        public ABIE(CCRepository repository, Element element)
        {
            this.repository = repository;
            this.element = element;
        }

        private IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        private IEnumerable<Connector> Connectors
        {
            get { return element.Connectors.AsEnumerable<Connector>(); }
        }

        public IEnumerable<IBBIE> BBIEs
        {
            get
            {
                return
                    Attributes.Convert(a => (IBBIE)new BBIE(repository, a));
            }
        }

        public IEnumerable<IASBIE> ASBIEs
        {
            get
            {
                return
                    Connectors.Where(IsASBIE).Convert(c => (IASBIE)new ASBIE(repository, c));
            }
        }

        private static bool IsASBIE(Connector con)
        {
            return con.Stereotype == "ASBIE";
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

        private static bool IsBasedOnDependency(Connector connector)
        {
            return connector.Stereotype == "basedOn";
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

        private static bool IsEquivalentToDependency(Connector con)
        {
            return con.Stereotype == "isEquivalentTo";
        }

        public int Id
        {
            get { return element.ElementID; }
        }

        public string Name
        {
            get { return element.Name; }
        }

        public IBusinessLibrary Library
        {
            get { return repository.GetLibrary(element.PackageID); }
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
    }
}