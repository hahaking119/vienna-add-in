// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BCC : UpccAttribute<IACC>, IBCC
    {
        public BCC(CCRepository repository, Attribute attribute, IACC container)
            : base(repository, attribute, container)
        {
        }

        #region IBCC Members

        public ICDT Type
        {
            get { return repository.GetCDT(attribute.ClassifierID); }
        }

        #endregion
    }
}