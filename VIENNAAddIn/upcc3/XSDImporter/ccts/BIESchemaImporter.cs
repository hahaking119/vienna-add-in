// TODO: remove all Console.WriteLine statements once implementation is complete
// TODO: complete the documentation of the entire BIE importer class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    public static class BIESchemaImporter
    {
        private static ICCLibrary existingAccs;
        private static IBDTLibrary existingBdts;
        private static IBIELibrary bieLibrary;

        public static void ImportXSD(ImporterContext context)
        {
            #region Import Preparation

            // TODO: ACC, BDT and BIE library should be configurable through the ImporterContext
            existingAccs = context.Repository.Libraries<ICCLibrary>().ElementAt(0);
            existingBdts = context.Repository.Libraries<IBDTLibrary>().ElementAt(0);
            bieLibrary = context.Repository.Libraries<IBIELibrary>().ElementAt(0);
            
            // Even though the XML schema is stored in the importer context we need to re-read
            // the XML schema. The reason for doing so is that the impoter context pre-read the
            // XML schema using the XML schema reader class from the .net API. However, the XML
            // schema reader dropped information that is required to import the ABIE schema. 
            // Instead we want to utilitze the CustomSchemaReader class that requires an XML
            // schema document as an input. Therefore we need to re-read the XML schema file based
            // on the file name and directory location as specified in the importer context.
            XmlDocument bieSchemaDocument = GetBieSchemaDocument(context);

            // Create a new CustomSchemaReader class that allows us to process the BIE XML
            // schema. 
            CustomSchemaReader reader = new CustomSchemaReader(bieSchemaDocument);

            #endregion
                   
            IDictionary<string, string> allElementDefinitions = new Dictionary<string, string>();

            #region Processing Step 1: Cumulate all ABIEs and create them in the BIE library

            // Iterate through all the items contained in the XML schema contained in the 
            // Items property of the reader object. 
            foreach (object item in reader.Items)
            {
                // In case the current Item returned by the CustomSchemaReader is a
                // complex type we know that we encountered an ABIE definition. 
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType)item;

                    // Therefore we create an ABIESpec based on the complex type 
                    // definition.
                    ABIESpec singleAbieSpec = CumulateAbieSpecFromComplexType(abieComplexType);

                    // Having an ABIESpec representing the complex type currently 
                    // processed we can create the corresponding ABIE in the BIE library. 

                    //Console.Write("Attempting to <<write>> ABIE: {0}", singleAbieSpec.Name);
                    bieLibrary.CreateElement(singleAbieSpec);
                    //Console.WriteLine("  ->  Success");

                }

                // In case the element was not a complex type we can check if we
                // encountered an element definition on the complex type level. These
                // either include element definitions for ABIEs or global element defintions
                // for ASBIEs. Regardless, we'll cache the element definitions for further 
                // processing of the ASBIEs. 
                if (item is Element)
                {
                    Element element = (Element)item;
                    allElementDefinitions.Add(element.Name, element.Type.Name);
                }
            }

            #endregion

            #region Processing Step 2: Update all ABIEs with their ASBIEs

            // Iterate through all the items contained in the XML schema contained in the 
            // Items property of the reader object. 
            foreach (object item in reader.Items)
            {
                // In case the current Item returned by the CustomSchemaReader is a
                // complex type we know that we encountered an ABIE definition. 
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType) item;

                    // As a first step we need to resolve the name of the ABIE that the we encountered 
                    // the ASBIE in. First we truncate the suffix "Type" from the ABIE's name. 
                    string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

                    // For updating an existing ABIE in the library utilizing the library's UpdateElement
                    // method we need to provide the original ABIE as well as the ABIESpec of the updated
                    // ABIE. Therefore we first read the ABIE from the library and then create the ABIESpec
                    // for the ABIE read. 
                    IABIE abieToBeUpdated = bieLibrary.ElementByName(abieName);
                    ABIESpec updatedAbieSpec = new ABIESpec(abieToBeUpdated);

                    // Create a list of ASBIESpecs based on the complex type definition of the ABIE
                    IList<ASBIESpec> newAsbieSpecs = CumulateAbiesSpecsFromComplexType(abieComplexType, allElementDefinitions);

                    // After resolving all ASBIE declarations for the current ABIE we assign the 
                    // new ASBIEs to the ABIE and update the ABIE accordingly utilizing the
                    // updated ABIESpec. 
                    foreach (ASBIESpec newAsbieSpec in newAsbieSpecs)
                    {
                        updatedAbieSpec.AddASBIE(newAsbieSpec);
                        
                    }

                    //Console.Write("Attempting to <<update>> ABIE: {0}", abieToBeUpdated.Name);
                    bieLibrary.UpdateElement(abieToBeUpdated, updatedAbieSpec);
                    //Console.WriteLine("  ->  Success");
                }
            }

            #endregion
        }

        #region Public Class Methods

        public static string ResolveMaxOccurs(string maxOccurs)
        {
            string resolvedMaxOccurs;

            switch (maxOccurs)
            {
                case "0":
                    resolvedMaxOccurs = "0";
                    break;
                case "1":
                    resolvedMaxOccurs = "1";
                    break;
                case "unbounded":
                    resolvedMaxOccurs = "*";
                    break;
                default:
                    resolvedMaxOccurs = "1";
                    break;
            }

            return resolvedMaxOccurs;
        }

        public static string ResolveMinOccurs(string minOccurs)
        {
            string resolvedMinOccurs;

            switch (minOccurs)
            {
                case "0":
                    resolvedMinOccurs = "0";
                    break;
                case "1":
                    resolvedMinOccurs = "1";
                    break;
                default:
                    resolvedMinOccurs = "1";
                    break;
            }

            return resolvedMinOccurs;
        }

        public static ASBIESpec MatchAsbieToAscc(IACC acc, string asbieName, int associatedAbie)
        {
            foreach (IASCC ascc in acc.ASCCs)
            {
                // we've found the matching ASCC
                if (ascc.Name.Equals(asbieName))
                {
                    return ASBIESpec.CloneASCC(ascc, asbieName, associatedAbie);
                }
            }

            return null;
        }

        public static ABIESpec CumulateAbieSpecFromComplexType(ComplexType abieComplexType)
        {
            ABIESpec singleAbieSpec = new ABIESpec();

            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

            singleAbieSpec.Name = abieName;
            singleAbieSpec.BasedOn = existingAccs.ElementByName(abieName);

            List<BBIESpec> bbieSpecs = new List<BBIESpec>();

            foreach (object ctItem in abieComplexType.Items)
            {
                if (ctItem is Element)
                {
                    Element element = (Element)ctItem;

                    if (element.Ref.Name.Equals(""))
                    {
                        if (element.Type.Prefix.Equals("bdt"))
                        {
                            BBIESpec bbieSpec = new BBIESpec();

                            bbieSpec.Name = element.Name;
                            bbieSpec.LowerBound = ResolveMinOccurs(element.MinOccurs);
                            bbieSpec.UpperBound = ResolveMaxOccurs(element.MaxOccurs);

                            string bdtName = element.Type.Name.Substring(0, element.Type.Name.Length - 4);

                            foreach (IBDT bdt in existingBdts.Elements)
                            {
                                if ((bdt.Name + bdt.CON.BasicType.DictionaryEntryName).Equals(bdtName))
                                {
                                    bbieSpec.Type = bdt;
                                    break;
                                }
                            }

                            bbieSpecs.Add(bbieSpec);
                        }
                        else
                        {
                            // TODO: we need to handle local ASBIE declarations
                        }
                    }
                }
            }
            singleAbieSpec.BBIEs = bbieSpecs;

            return singleAbieSpec;
        }

        private static IList<ASBIESpec> CumulateAbiesSpecsFromComplexType(ComplexType abieComplexType, IDictionary<string, string> allElementDefinitions)
        {
            IList<ASBIESpec> newAsbieSpecs = new List<ASBIESpec>();

            // As a first step we need to resolve the name of the ABIE that the we encountered 
            // the ASBIE in. First we truncate the suffix "Type" from the ABIE's name. 
            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

            foreach (Element element in abieComplexType.Items)
            {
                // Within the complex type definition we are interested in all ASBIE definitions.
                // The "Ref" attritbute of the element tells us whether we are dealing with an
                // ASBIE or not. In case the "Ref" attribute is not empty we can be certain
                // that the element represents an ASBIE utilizing a global ASBIE element declaration.
                if (!(element.Ref.Name.Equals("")))
                {
                    // As a next step we need to get the associated ABIE. Therefore we first
                    // resolve the associated ABIE's name. This is achieved by taking the name
                    // referenced in the element declaration. The name is then used to lookup the 
                    // associated ABIEs name in the element declarations previously cached in the
                    // dictionary named "elementDefinitions". 
                    string associatedABIEName = allElementDefinitions[element.Ref.Name];
                    associatedABIEName = associatedABIEName.Substring(0, associatedABIEName.Length - 4);

                    // After resolving the associated ABIE's name we retrieve the associated ABIE
                    // from the BIE library. 
                    IABIE associatedAbie = bieLibrary.ElementByName(associatedABIEName);

                    // Next we we truncate the associated ABIE's name from the ASBIE element 
                    // declaration in order to get the name of the ASBIE
                    string asbieName = (element.Ref.Name).Substring(0,
                                                                    (element.Ref.Name).Length -
                                                                    associatedABIEName.Length);

                    // ASBIEs are typically based on ASCCs. However it is also possible that ASBIEs
                    // are not based on ASCCs. Therefore, we first try to find an existing ASCC on the
                    // core component level. If that works out, we'll clone the ASCC and create an 
                    // ASBIE off of it. Otherwise we create a new ASBIE from scratch. 
                    ASBIESpec asbieSpec = MatchAsbieToAscc(existingAccs.ElementByName(abieName), asbieName,
                                                           associatedAbie.Id);

                    // In case no matching ASCC was found we create a new ASBIE ourselves. 
                    if (asbieSpec == null)
                    {
                        asbieSpec = new ASBIESpec();
                        asbieSpec.AssociatedABIEId = associatedAbie.Id;
                        asbieSpec.Name = asbieName;
                        asbieSpec.LowerBound = ResolveMinOccurs(element.MinOccurs);
                        asbieSpec.UpperBound = ResolveMaxOccurs(element.MaxOccurs);
                        asbieSpec.AggregationKind = EAAggregationKind.Shared;
                    }

                    // Finally we add our new ASBIE to the list of new ASBIEs for our current ABIE. 
                    newAsbieSpecs.Add(asbieSpec);
                }

            }

            return newAsbieSpecs;
        }

        
        #endregion

        #region Private Class Methods       

        private static XmlDocument GetBieSchemaDocument(ImporterContext context)
        {
            XmlDocument bieSchemaDocument = new XmlDocument();

            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                if (schemaInfo.FileName.StartsWith("BusinessInformationEntity"))
                {
                    bieSchemaDocument.Load(context.InputDirectory + schemaInfo.FileName);
                    
                    break;
                }
            }

            return bieSchemaDocument;
        }

        #endregion
    }
}