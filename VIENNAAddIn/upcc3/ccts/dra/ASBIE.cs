// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;
using CctsRepository;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASBIE : UpccAssociation<IABIE>, IASBIE
    {
        public ASBIE(CCRepository repository, Connector connector, IABIE associatingBIE)
            : base(repository, connector, associatingBIE)
        {
        }

        #region IASBIE Members

        public IABIE AssociatedElement
        {
            get { return repository.GetABIE(connector.GetAssociatedElementId(AssociatingElement.Id)); }
        }

        public IASCC BasedOn
        {
            get
            {
                if (AssociatingElement == null)
                {
                    return null;
                }
                IACC acc = AssociatingElement.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (IASCC ascc in acc.ASCCs)
                {
                    if (nameWithoutQualifiers == ascc.Name)
                    {
                        return ascc;
                    }
                }
                return null;
            }
        }

        #endregion
    }
}