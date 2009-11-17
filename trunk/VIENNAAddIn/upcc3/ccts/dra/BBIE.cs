// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;
using UPCCRepositoryInterface;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BBIE : UpccAttribute<IABIE>, IBBIE
    {
        public BBIE(CCRepository repository, Attribute attribute, IABIE container)
            : base(repository, attribute, container)
        {
        }

        #region IBBIE Members

        public IBDT Type
        {
            get { return repository.GetBDT(attribute.ClassifierID); }
        }

        public IBCC BasedOn
        {
            get
            {
                if (Container == null)
                {
                    return null;
                }
                IACC acc = Container.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (IBCC bcc in acc.BCCs)
                {
                    if (nameWithoutQualifiers == bcc.Name)
                    {
                        return bcc;
                    }
                }
                return null;
            }
        }

        #endregion

        protected override string GetTypeName()
        {
            return Type.Name;
        }
    }
}