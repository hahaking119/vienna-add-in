using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    internal class BDTRestrictionComplexType : BDTComplexType
    {
        public BDTRestrictionComplexType(XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
        }

        public override string ContentComponentXsdTypeName
        {
            get { return Parent.ContentComponentXsdTypeName; }
        }

        private BDTXsdType Parent
        {
            get
            {
                var restriction = (XmlSchemaSimpleContentRestriction) ComplexType.ContentModel.Content;
                string baseTypeName = restriction.BaseTypeName.Name;
                return bdtSchema.GetBDTXsdType(baseTypeName);
            }
        }

        protected override IEnumerable<SUPSpec> SpecifySUPs()
        {
            var restriction = (XmlSchemaSimpleContentRestriction)ComplexType.ContentModel.Content;
            string baseTypeName = restriction.BaseTypeName.Name;
            var supAttributes = new SUPXsdAttributes(restriction.Attributes);
            IBDT parentBDT = GetBDTByXsdTypeName(baseTypeName);
            foreach (ISUP parentSUP in parentBDT.SUPs)
            {
                var supSpec = new SUPSpec(parentSUP);
                if (!supAttributes.IsProhibited(supSpec))
                {
                    supAttributes.ApplyRestrictions(supSpec);
                    yield return supSpec;
                }
            }
        }
    }
}