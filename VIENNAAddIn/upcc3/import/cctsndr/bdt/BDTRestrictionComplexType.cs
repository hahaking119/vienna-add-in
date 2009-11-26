using System;
using System.Collections.Generic;
using System.Xml.Schema;
using CctsRepository.BdtLibrary;

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
                string baseTypeName = Restriction.BaseTypeName.Name;
                return bdtSchema.GetBDTXsdType(baseTypeName);
            }
        }

        private XmlSchemaSimpleContentRestriction Restriction
        {
            get
            {
                return (XmlSchemaSimpleContentRestriction) ComplexType.ContentModel.Content;
            }
        }

        protected override IEnumerable<BdtSupSpec> SpecifySUPs()
        {            
            string baseTypeName = Restriction.BaseTypeName.Name;
            var supAttributes = new SUPXsdAttributes(Restriction.Attributes);
            IBdt parentBDT = GetBDTByXsdTypeName(baseTypeName);
            foreach (IBdtSup parentSUP in parentBDT.Sups)
            {
                var supSpec = BdtSupSpec.CloneBdtSup(parentSUP);
                if (!supAttributes.IsProhibited(supSpec))
                {
                    supAttributes.ApplyRestrictions(supSpec);
                    yield return supSpec;
                }
            }
        }

        private BdtConSpec ApplyCONRestrictions(BdtConSpec conSpec)
        {
            foreach (var facet in Restriction.Facets)
            {
                if (facet is XmlSchemaLengthFacet)
                {
                    conSpec.Length = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMinLengthFacet)
                {
                    conSpec.MinLength = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaMaxLengthFacet)
                {
                    conSpec.MaxLength = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaPatternFacet)
                {
                    conSpec.Pattern = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaMaxInclusiveFacet)
                {
                    conSpec.MaxInclusive = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaMaxExclusiveFacet)
                {
                    conSpec.MaxExclusive = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaMinInclusiveFacet)
                {
                    conSpec.MinInclusive = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaMinExclusiveFacet)
                {
                    conSpec.MinExclusive = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaFractionDigitsFacet)
                {
                    conSpec.FractionDigits = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaTotalDigitsFacet)
                {
                    conSpec.TotalDigits = ((XmlSchemaFacet)facet).Value;
                }
                else if (facet is XmlSchemaWhiteSpaceFacet)
                {
                    conSpec.WhiteSpace = ((XmlSchemaFacet)facet).Value;
                }
            }

            return conSpec;
        }

        protected override BdtConSpec SpecifyCON()
        {
            IBdt parentBDT = GetBDTByXsdTypeName(Restriction.BaseTypeName.Name);

            return ApplyCONRestrictions(BdtConSpec.CloneBdtCon(parentBDT.Con));
        }
    }
}