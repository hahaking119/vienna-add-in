// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using CctsRepository.cc;
using CctsRepository.cdt;
using Attribute=EA.Attribute;

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

        protected override string GetTypeName()
        {
            return Type.Name;
        }
    }
}