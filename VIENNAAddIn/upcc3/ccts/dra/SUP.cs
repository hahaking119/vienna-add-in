// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using UPCCRepositoryInterface;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class SUP : DTComponent, ISUP
    {
        public SUP(CCRepository repository, Attribute attribute, IDT dt) : base(repository, attribute, dt)
        {
        }

        public override string DictionaryEntryName
        {
            get
            {
                var value = GetTaggedValue(TaggedValues.dictionaryEntryName);
                if (string.IsNullOrEmpty(value))
                {
                    value = DT.Name + ". " + Name + ". " + BasicType.Name;
                }
                return value;
            }
        }
    }
}