// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=CctsRepository.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    ///<typeparam name="TSpec"></typeparam>
    public abstract class AbstractDT<TSpec> : UpccClass<TSpec>, IDT where TSpec : DTSpec
    {
        protected AbstractDT(CCRepository repository, Element element, string stereotype)
            : base(repository, element, stereotype)
        {
        }

        #region IDT Members

        public override string DictionaryEntryName
        {
            get
            {
                string value = base.DictionaryEntryName;
                if (string.IsNullOrEmpty(value))
                {
                    value = Name + ". Type";
                }
                return value;
            }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.usageRule); }
        }

        public IEnumerable<ISUP> SUPs
        {
            get
            {
                return
                    Attributes.Where(EAAttributeExtensions.IsSUP).Convert(
                        a => (ISUP) new SUP(repository, a, this));
            }
        }

        public ICON CON
        {
            get
            {
                Attribute conAttribute = Attributes.FirstOrDefault(EAAttributeExtensions.IsCON);
                if (conAttribute == null)
                {
                    throw new Exception("data type contains no attribute with stereotype <<CON>>");
                }
                return new CON(repository, conAttribute, this);
            }
        }

        #endregion

        protected override IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(TSpec spec)
        {
            return spec.GetTaggedValues();
        }

        protected override IEnumerable<AttributeSpec> GetAttributeSpecs(TSpec spec)
        {
            return spec.GetAttributes();
        }

        protected override IEnumerable<ConnectorSpec> GetConnectorSpecs(TSpec spec)
        {
            return spec.GetConnectors(repository);
        }
    }
}