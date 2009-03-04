using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ABIE : UpccClass, IABIE
    {
        public ABIE(CCRepository repository, Element element) : base(repository, element, "ABIE")
        {
        }

        #region IABIE Members

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public IEnumerable<IBBIE> BBIEs
        {
            get
            {
                return
                    Attributes.Convert(a => (IBBIE) new BBIE(repository, a, this));
            }
        }

        public IEnumerable<IASBIE> ASBIEs
        {
            get
            {
                return
                    Connectors.Where(Stereotype.IsASBIE).Convert(c => (IASBIE) new ASBIE(repository, c, this));
            }
        }

        public IACC BasedOn
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsBasedOn);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        public IABIE IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetABIE(connector.SupplierID) : null;
            }
        }

        #endregion
    }
}