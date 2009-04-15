using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.upcc3
{
    class BIELibraryValidator : AbstractValidator
    {






        /// <summary>
        /// Validate the given BIELibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Get all BDTs from the given package
            Dictionary<Int32, EA.Element> bies = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, bies, UPCC.ABIE.ToString());

            checkC514c(context, package);

            String n = package.Name;

            checkC514l(context, package);

            checkC574a(context, package, bies);

            checkC574b(context, package, bies);

            checkC574c(context, package, bies);

            checkC574d(context, package, bies);

            checkC574e(context, package, bies);

        }



        /// <summary>
        /// A BIELibrary shall only contain ABIEs, BBIEs, and ASBIEs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514c(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ABIE.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in BIELibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A BIELibrary shall only contain ABIEs, BBIEs, and ASBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }




        /// <summary>
        /// A BIELibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514l(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in BIELibrary.", "A BIELibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }


        /// <summary>
        /// An ABIE shall not contain – directly or at any nested level – a mandatory ASBIE whose associated ABIE is the same as the top level ABIE.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574a(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            //Will be implemented with a DFS

        }


        /// <summary>
        /// An ABIE shall contain only BBIEs and ASBIEs. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574b(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                EA.Element element = e.Value;
                int count_asbie = 0;
                int count_bbie = 0;

                foreach (EA.Connector con in element.Connectors)
                {
                    if (con.Stereotype == UPCC.ASBIE.ToString() && con.ClientID == element.ElementID && (con.Type == AssociationTypes.Association.ToString() || con.Type == AssociationTypes.Aggregation.ToString()))
                    {
                        count_asbie++;
                    }
                }

                foreach (EA.Attribute bbie in element.Attributes)
                {
                    if (bbie.Stereotype == UPCC.BBIE.ToString())
                    {
                        count_bbie++;
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("An ABIE shall contain only BBIEs and ASBIEs.", "An ABIE shall contain only BBIEs and ASBIEs. There shall be at least one BBIE or at least one ASBIE. \nABIE " + element.Name + " has an attribute which is not stereotyped as BBIE.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }


                if (count_asbie == 0 && count_bbie == 0)
                {
                    context.AddValidationMessage(new ValidationMessage("No ASBIEs/BBIEs found for an ABIE.", "An ABIE shall contain only BBIEs and ASBIEs. There shall be at least one BBIE or at least one ASBIE. \nABIE " + element.Name + " has neither any ASBIEs nor any BBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }

        }




        /// <summary>
        /// An ABIE shall have exactly one dependency of stereotype <<basedOn>> with an ACC as the target.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574c(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                EA.Element element = e.Value;
                bool foundBasedOn = false;

                foreach (EA.Connector con in element.Connectors)
                {

                    if (con.Stereotype == UPCC.basedOn.ToString() && con.ClientID == element.ElementID)
                    {
                        //Get the supplier (target) of the based on dependency
                        EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                        //Is the supplier an ACC? If not raise an error
                        if (supplier.Stereotype == UPCC.ACC.ToString())
                        {
                            foundBasedOn = true;
                        }
                    }
                }

                //No based on dependency found - raise an error
                if (!foundBasedOn)
                {
                    context.AddValidationMessage(new ValidationMessage("Missing basedOn dependency found in an ABIE.", "An ABIE shall have exactly one dependency of stereotype <<basedOn>> with an ACC as the target. Errorneous ABIE: " + element.Name + " in BIELibrary " + p.Name, "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }
        }




        /// <summary>
        /// The name of an ABIE shall be unique for a given BIE library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574d(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {
            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {

                if (names.ContainsValue(e.Value.Name))
                {
                    context.AddValidationMessage(new ValidationMessage("Two BIEs with the same name found.", "Each BIE shall have a unique name within the BIE library of which it is part of. Two BIEs with the name " + e.Value.Name + " found in BIELibrary " + p.Name + ".", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

                names.Add(e.Value.ElementID, e.Value.Name);
            }

        }





        /// <summary>
        /// The name on an ABIE shall be the same as the name of the ACC on which it is based.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574e(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                //Getthe ACC on which this BIE is based on 
                EA.Element targetElement = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);

                if (targetElement != null)
                {
                    //The unqualified name of the BIE
                    String unqualifiedName = Utility.getUnqualifiedName(e.Value.Name);

                    //Compare the unqualified name of the BIE to the name of the underlying ACC
                    if (targetElement.Name != unqualifiedName)
                    {
                        context.AddValidationMessage(new ValidationMessage("Mismatch between ABIE and ACC name found.", "The name on an ABIE shall be the same as the name of the ACC on which it is based. The ABIE " + e.Value.Name + " found in BIELibrary " + p.Name + " does not have the same name as the ACC " + targetElement.Name + " it is based on.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                    }
                }
            }

        }





















        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }












    }
}
