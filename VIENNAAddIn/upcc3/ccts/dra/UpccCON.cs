// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using CctsRepository;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    /// <summary>
    /// Note: This file cannot be named 'CON' after the class, because 'CON' is a reserved name for windows file systems.
    /// </summary>
    internal class CON : DTComponent, ICON
    {
        public CON(CCRepository repository, Attribute attribute, IDT dt) : base(repository, attribute, dt)
        {
        }

        public override string DictionaryEntryName
        {
            get
            {
                var value = GetTaggedValue(TaggedValues.dictionaryEntryName);
                if (string.IsNullOrEmpty(value))
                {
                    value = DT.Name + ". Content";
                }
                return value;
            }
        }

    }
}