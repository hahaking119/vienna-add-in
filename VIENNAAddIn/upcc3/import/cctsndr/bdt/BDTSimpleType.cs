using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    internal class BDTSimpleType : BDTXsdType
    {
        public BDTSimpleType(XmlSchemaSimpleType simpleType, BDTSchema bdtSchema) : base(simpleType, bdtSchema)
        {
        }

        private XmlSchemaSimpleType SimpleType
        {
            get { return (XmlSchemaSimpleType) XsdType; }
        }

        public override string ContentComponentXsdTypeName
        {
            get
            {
                var parent = Parent;
                if (parent != null)
                {
                    return parent.ContentComponentXsdTypeName;
                }
                return Restriction.BaseTypeName.Name;
            }
        }

        private BDTXsdType Parent
        {
            get
            {
                return NDR.IsXsdDataTypeName(Restriction.BaseTypeName) ? null : bdtSchema.GetBDTXsdType(Restriction.BaseTypeName.Name);
            }
        }

        private XmlSchemaSimpleTypeRestriction Restriction
        {
            get
            {
                return (XmlSchemaSimpleTypeRestriction)SimpleType.Content;
            }
        }

        protected override IEnumerable<SUPSpec> SpecifySUPs()
        {
            yield break;
        }

        private CONSpec ApplyCONRestrictions(CONSpec conSpec)
        {
            foreach (var facet in Restriction.Facets)
            {
                if (facet is XmlSchemaLengthFacet)
                {
                    conSpec.Length = ((XmlSchemaFacet)facet).Value;
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

        protected override CONSpec SpecifyCON()
        {
            CONSpec conSpec;
            var parent = Parent;
            if (parent != null)
            {
                conSpec = new CONSpec(parent.BDT.CON);
            }
            else
            {
                conSpec = new CONSpec
                              {
                                  BasicType =
                                      FindPRIM(NDR.ConvertXsdTypeNameToBasicTypeName(ContentComponentXsdTypeName))
                              };
            }
            return ApplyCONRestrictions(conSpec);
        }
    }
}