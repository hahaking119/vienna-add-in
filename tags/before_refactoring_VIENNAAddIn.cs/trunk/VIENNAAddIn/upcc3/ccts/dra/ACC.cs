// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class ACC : UpccClass, IACC
    {
        public ACC(CCRepository repository, Element element) : base(repository, element, "ACC")
        {
        }

        #region IACC Members

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public IEnumerable<IBCC> BCCs
        {
            get { return Attributes.Convert(a => (IBCC) new BCC(repository, a, this)); }
        }

        public IEnumerable<IASCC> ASCCs
        {
            get { return Connectors.Where(Stereotype.IsASCC).Convert(c => (IASCC) new ASCC(repository, c, this)); }
        }

        public IACC IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        #endregion
    }
}