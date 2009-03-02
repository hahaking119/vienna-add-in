using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class ACC : UpccElement, IACC
    {
        private readonly CCRepository repository;

        public ACC(CCRepository repository, Element element) : base(element, "ACC")
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

        #region IACC Members

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

        public string UniqueIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public IEnumerable<IBCC> BCCs
        {
            get
            {
                return
                    Attributes.Convert(a => (IBCC) new BCC(repository, a));
            }
        }

        public IEnumerable<IASCC> ASCCs
        {
            get
            {
                return
                    Connectors.Where(IsASCC).Convert(c => (IASCC) new ASCC(repository, c));
            }
        }

        public IACC IsEquivalentTo
        {
            get
            {
                Connector connector =
                    Connectors.FirstOrDefault(IsEquivalentToDependency);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        #endregion

        private static bool IsASCC(Connector con)
        {
            return con.Stereotype == "ASCC";
        }

        private static bool IsEquivalentToDependency(Connector con)
        {
            return con.Stereotype == "isEquivalentTo";
        }
    }
}