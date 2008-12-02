/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.common;

namespace VIENNAAddIn.CCTS {


    internal class OCLMapper {

        
        internal OCLMapper() {
        }


        /// <sUMM2ary>
        /// Evaluates the constraint in the element e and adds a new fact to the restriction if necessary
        /// </sUMM2ary>
        /// <param name="restriction"></param>
        /// <param name="e"></param>
        internal static void processOCL(XmlSchemaSimpleContentRestriction restriction, EA.Element e, EA.Package p, GeneratorCallBackInterface caller, String primitiveType) {
            
            //OCLs can only be applied to String and Decimal
            if (e.Constraints.Count > 0 && !(primitiveType.ToLower().Equals("decimal") ||
                primitiveType.ToLower().Equals("string"))) {
                caller.appendMessage("warn", "OCL constraints can only be applied to QDTs where the CON attribute is either of type String or Decimal. (Element: "+e.Name+")", p.Name);
            }
            else {
                //Does the element contain constraints?
                foreach (EA.Constraint constraint in e.Constraints) {
                    if (constraint.Type.ToString().Equals("OCL")) {
                        try {
                            XmlSchemaFacet facet = getFacetForOCL(constraint.Name, primitiveType);
                            if (facet != null)
                                restriction.Facets.Add(facet);
                            else
                                caller.appendMessage("warn", "Could not evaluate the OCL constaint for the element " + e.Name + " Constraint: " + constraint.Name, p.Name);
                        }
                        catch (OCLConstraintNotApplicableException onap) {
                            caller.appendMessage("warn", "The OCL constraint is not applicable to the type of the QDT.", p.Name);
                        }
                    }
                }
            }
        }

        /// <sUMM2ary>
        /// Processes the given constraint an returns a facet if possible
        /// </sUMM2ary>
        /// <param name="ocl"></param>
        /// <returns></returns>
        private static XmlSchemaFacet getFacetForOCL(String ocl, String primitiveType) {

            primitiveType = primitiveType.ToLower();

            ocl = ocl.Trim();

            //Get the substring until the first opening bracket
            if (ocl == null || ocl == "")
                return null;

            int lowerBound = ocl.IndexOf('(');
            int upperBound = ocl.IndexOf(')');
            String value = "";
            try { value = ocl.Substring(lowerBound + 1, upperBound - lowerBound - 1); } catch { }
            //Value must be an integer
            if (!Utility.isInteger(value))
                return null;

            ocl = ocl.Substring(0, ocl.IndexOf('(')).ToLower();

            //That's all but nice - however for a "real" OCL interpreter there is simply no time
            if (ocl == "self.Content.Length".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaLengthFacet length = new XmlSchemaLengthFacet();
                length.Value = value;
                return length;
            }
            else if (ocl == "self.Content.MinimumLength".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaMinLengthFacet length = new XmlSchemaMinLengthFacet();
                length.Value = value;
                return length;
            }
            else if (ocl == "self.Content.MaximumLength".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaMaxLengthFacet length = new XmlSchemaMaxLengthFacet();
                length.Value = value;
                return length;
            }
            else if (ocl == "self.content.TotalDigits".ToLower()) {
                if (!primitiveType.Equals("decimal"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaTotalDigitsFacet length = new XmlSchemaTotalDigitsFacet();
                length.Value = value;
                return length;
                
            }
            else if (ocl == "self.content.FractionalDigits".ToLower()) {
                if (!primitiveType.Equals("decimal"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaFractionDigitsFacet length = new XmlSchemaFractionDigitsFacet();
                length.Value = value;
                return length;

            }
            else if (ocl == "self.content.MinimumInclusive".ToLower()) {
                if (!primitiveType.Equals("decimal"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaMinInclusiveFacet length = new XmlSchemaMinInclusiveFacet();
                length.Value = value;
                return length;

            }
            else if (ocl == "self.content.MaximumInclusive".ToLower()) {
                if (!primitiveType.Equals("decimal"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaMaxInclusiveFacet length = new XmlSchemaMaxInclusiveFacet();
                length.Value = value;
                return length;

            }
            else if (ocl == "self.content.MinimumExclusive".ToLower()) {
                if (!primitiveType.Equals("decimal"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaMinExclusiveFacet length = new XmlSchemaMinExclusiveFacet();
                length.Value = value;
                return length;

            }
            else if (ocl == "self.content.MaximumExclusive".ToLower()) {
                if (!primitiveType.Equals("decimal"))
                    throw new OCLConstraintNotApplicableException();

                XmlSchemaMaxExclusiveFacet length = new XmlSchemaMaxExclusiveFacet();
                length.Value = value;
                return length;

            }
            else if (ocl == "self.content.MinimumInclusiveDate".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();
                
                //TO DO
            }
            else if (ocl == "self.content.MaximumInclusiveDate".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();

                //TO DO
            }
            else if (ocl == "self.content.MinimumExclusiveDate".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();

                //TO DO
            }
            else if (ocl == "self.content.MaximumExclusiveDate".ToLower()) {
                if (!primitiveType.Equals("string"))
                    throw new OCLConstraintNotApplicableException();

                //TO DO
            }

            return null;
        }






    }
}
