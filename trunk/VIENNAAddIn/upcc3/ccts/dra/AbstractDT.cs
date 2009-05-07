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
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

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
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public IEnumerable<ISUP> SUPs
        {
            get
            {
                return
                    Attributes.Where(Stereotype.IsSUP).Convert(
                        a => (ISUP) new SUP(repository, a, this));
            }
        }

        public ICON CON
        {
            get
            {
                Attribute conAttribute = Attributes.FirstOrDefault(Stereotype.IsCON);
                if (conAttribute == null)
                {
                    throw new Exception("data type contains no attribute with stereotype <<CON>>");
                }
                return new CON(repository, conAttribute, this);
            }
        }

        #endregion

        protected override void AddAttributes(TSpec spec)
        {
            if (spec.CON != null)
            {
                element.AddAttribute(Stereotype.CON, "Content", spec.CON.BasicType.Name, spec.CON.BasicType.Id,
                                     spec.CON.LowerBound, spec.CON.UpperBound, spec.CON.GetTaggedValues());
            }
            if (spec.SUPs != null)
            {
                foreach (SUPSpec sup in spec.SUPs)
                {
                    element.AddAttribute(Stereotype.SUP, sup.Name, sup.BasicType.Name, sup.BasicType.Id, sup.LowerBound,
                                         sup.UpperBound, sup.GetTaggedValues());
                }
            }
        }
    }
}