using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public abstract class AbstractDT : UpccClass, IDT
    {
        protected AbstractDT(CCRepository repository, Element element, string stereotype)
            : base(repository, element, stereotype)
        {
        }

        #region IDT Members

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
    }
}