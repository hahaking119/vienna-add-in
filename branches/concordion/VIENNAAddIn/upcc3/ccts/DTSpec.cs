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
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public abstract class DTSpec : CCTSElementSpec
    {
        protected DTSpec(IDT dt) : base(dt)
        {
            UsageRules = new List<string>(dt.UsageRules);
            CON = new CONSpec(dt.CON);
            SUPs = new List<SUPSpec>(dt.SUPs.Convert(sup => new SUPSpec(sup)));
        }

        protected DTSpec()
        {
            SUPs = new List<SUPSpec>();
        }

        [TaggedValue]
        public IEnumerable<string> UsageRules { get; set; }

        public List<SUPSpec> SUPs { get; set; }
        public CONSpec CON { get; set; }

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }

        public override IEnumerable<AttributeSpec> GetAttributes()
        {
            if (CON != null)
            {
                yield return new AttributeSpec(Stereotype.CON, "Content", CON.BasicType.Name, CON.BasicType.Id, CON.LowerBound, CON.UpperBound, CON.GetTaggedValues());
            }
            if (SUPs != null)
            {
                foreach (SUPSpec sup in SUPs)
                {
                    yield return new AttributeSpec(Stereotype.SUP, sup.Name, sup.BasicType.Name, sup.BasicType.Id, sup.LowerBound, sup.UpperBound, sup.GetTaggedValues());
                }
            }

        }
    }
}