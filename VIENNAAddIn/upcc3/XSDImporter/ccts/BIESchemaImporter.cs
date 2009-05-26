// TODO: remove all Console.WriteLine statements once implementation is complete

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    ///<summary>
    ///</summary>
    public static class BIESchemaImporter
    {
        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        public static void ImportXSD(ImporterContext context)
        {
            #region Import Preparation

            // TODO: ACC, BDT and BIE library should be configurable through the ImporterContext

            // Retrieve all ACCs stored in the ACC library that all ABIEs are based on and
            // cache them in a local dictionary. 
            ICCLibrary accLibrary = GetAccLibrary(context.Repository);
            IDictionary<string, IACC> existingAccs = CreateDictionaryFromAccLibrary(accLibrary);

            // Retrieve all BDTs stored in the BDT library that all ABIEs utilize to typify
            // the BBIEs contained in the ABIE. 
            IBDTLibrary bdtLibrary = GetBdtLibrary(context.Repository);
            IDictionary<string, IBDT> existingBdts = CreateDictionaryFromBdtLibrary(bdtLibrary);

            // Retrieve the BIE library that all ABIEs will be imported into. 
            IBIELibrary bieLibrary = GetTargetBieLibrary(context.Repository);
            
            // Even though the XML schema is stored in the importer context we need to re-read
            // the XML schema. The reason for doing so is that the impoter context pre-read the
            // XML schema using the XML schema reader class from the .net API. However, the XML
            // schema reader dropped information that is required to import the ABIE schema. 
            // Instead we want to utilitze the CustomSchemaReader class that requires an XML
            // schema document as an input. Therefore we need to re-read the XML schema file based
            // on the file name and directory location as specified in the importer context.
            XmlDocument bieSchemaDocument = GetBieSchemaDocument(context);

            #endregion

            // Create two dictionaries: The first one contains all ABIE specs of the ABIEs 
            // that are about to be created. Reason for storing the ABIE specs is that the
            // ABIEs are updated in a second processing steps with their respective ASBIEs. 
            // Besides cumulating all ABIEs contained in the XML schema we also cumulate all
            // element definitions on the complex type level of the XML schema since these
            // element definitions may be ASBIE element declarations. These element declarations
            // are then needed in the second processing step.            
            IDictionary<string, ABIESpec> allAbieSpecs = new Dictionary<string, ABIESpec>();
            IDictionary<string, string> elementDefinitions = new Dictionary<string, string>();

            // Create a set of ABIESpecs from the BIE XML schema.    
            CumulateAbieSpecsFromSchema(bieSchemaDocument, existingAccs, existingBdts, allAbieSpecs, elementDefinitions);

            // After cumulating all ABIESpecs we'll write each one of them into 
            // the BIE library.
            WriteAbieSpecsToLibrary(allAbieSpecs, bieLibrary);


            // After cumulating all ABIESpecs from the XML schema and writing the ABIEs to the
            // target BIE library we can process the XML schema again and update all ABIEs
            // with their ASBIEs. 

            #region Step 2: Update all ABIEs with their ASBIEs

            // Create a new CustomSchemaReader class that allows us to process the BIE XML
            // schema. 
            CustomSchemaReader reader = new CustomSchemaReader(bieSchemaDocument);

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

                    // Then we read the current ABIE's specification from the dictionary. The 
                    // specification 
                    ABIESpec abieSpecToBeUpdated = allAbieSpecs[abieName];

                    // Also we do not only need to the ABIE's specification but also the ABIE
                    // itself. We therefore retrieve the ABIE from the BIE library where we 
                    // previously created the ABIE in. 
                    IABIE abieToBeUpdated = bieLibrary.ElementByName(abieName);

                    IList<ASBIESpec> asbieSpecs = new List<ASBIESpec>();

                    foreach (Element element in abieComplexType.Items)
                    {
                        // Within the complex type definition we are interested in all ASBIE definitions
                        // which are typically encoded using the ref attribute of an element definition. 
                        if (!(element.Ref.Name.Equals("")))
                        {                           
                            // In case we encountered an ASBIE definition we need to resolve the ASBIE 
                            // definition. The ref attribute typically points to a global element declaration.
                            // Remember that we cached all global element declarations we encountered on 
                            // the complex type level in the dictionary named "elementDefinitions". 

                            // As a next step we need to get the associated ABIE. Therefore we first
                            // resolve the associated ABIE's name. This is achieved by taking the name
                            // referenced in the element declaration. The name is then used to lookup the 
                            // associated ABIEs name in the element declarations previously cached in the
                            // dictionary named "elementDefinitions". 
                            string associatedABIEName = elementDefinitions[element.Ref.Name];
                            associatedABIEName = associatedABIEName.Substring(0, associatedABIEName.Length - 4);
                            
                            // After resolving the associated ABIE's name we retrieve the associated ABIE
                            // from the BIE library. 
                            IABIE associatedAbie = bieLibrary.ElementByName(associatedABIEName);

                            // ASBIEs are typically based on ASCCs. However it is also possible that ASBIEs
                            // are not based on ASCCs. Therefore, we first try to find an existing ASCC on the
                            // core component level. If that works out, we'll clone the ASCC and create an 
                            // ASBIE off of it. Otherwise we create a new ASBIE from scratch. 

                            // Therefore, we first truncate the associated ABIEs name from the ASBIE name. 
                            string asbieName = (element.Ref.Name).Substring(0, (element.Ref.Name).Length - associatedABIEName.Length);

                            // Now we try to match an the ASBIE to an ASCC that the ACC, which the ABIE
                            // is based on, has.
                            ASBIESpec asbieSpec = MatchAsbieToAscc(existingAccs[abieName], asbieName, associatedAbie.Id);

                            // In case the ASBIE couldn't be matched to an existing ASCC we create a new ASBIE.
                            if (asbieSpec == null)
                            {
                                asbieSpec = new ASBIESpec();
                                asbieSpec.AssociatedABIEId = associatedAbie.Id;
                                asbieSpec.Name = asbieName;                                
                            }
                            asbieSpecs.Add(asbieSpec);
                        }
                    }

                    // Regardless whether we created a new ASBIE or based the ASBIE on an ACC we add
                    // the ASBIESpec to the ABIESpec. 
                    abieSpecToBeUpdated.ASBIEs = asbieSpecs;

                    // After we have successfully gathered all artifacts we need we can update the 
                    // ABIE in the repository. 
                    bieLibrary.UpdateElement(abieToBeUpdated, abieSpecToBeUpdated);

                }                
            }

            #endregion

        }

        #region Public Class Methods

        ///<summary>
        ///</summary>
        ///<param name="abieSpecs"></param>
        ///<param name="targetBieLibrary"></param>
        public static void WriteAbieSpecsToLibrary(IDictionary<string, ABIESpec> abieSpecs, IBIELibrary targetBieLibrary)
        {
            foreach (ABIESpec abieSpec in abieSpecs.Values)
            {
                Console.Write("Attempting to write ABIE: {0}", abieSpec.Name);
                targetBieLibrary.CreateElement(abieSpec);
                Console.WriteLine(" - Success :-)");
            }
        }

        public static ABIESpec CumulateAbieSpecFromComplexType(ComplexType abieComplexType, IDictionary<string, IACC> existingAccs, IDictionary<string, IBDT> existingBdts)
        {
            ABIESpec singleAbieSpec = new ABIESpec();

            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

            singleAbieSpec.Name = abieName;
            singleAbieSpec.BasedOn = existingAccs[abieName];

            List<BBIESpec> bbieSpecs = new List<BBIESpec>();

            foreach (object ctItem in abieComplexType.Items)
            {
                if (ctItem is Element)
                {
                    Element element = (Element)ctItem;

                    if (element.Ref.Name.Equals(""))
                    {
                        BBIESpec bbieSpec = new BBIESpec();

                        bbieSpec.Name = element.Name;
                        bbieSpec.LowerBound = ResolveMinOccurs(element.MinOccurs);
                        bbieSpec.UpperBound = ResolveMaxOccurs(element.MaxOccurs);

                        string bdtName = element.Type.Name.Substring(0, element.Type.Name.Length - 4);

                        foreach (IBDT bdt in existingBdts.Values)
                        {
                            if ((bdt.Name + bdt.CON.BasicType.DictionaryEntryName).Equals(bdtName))
                            {
                                bbieSpec.Type = bdt;
                                break;
                            }
                        }

                        bbieSpecs.Add(bbieSpec);
                    }
                }
            }
            singleAbieSpec.BBIEs = bbieSpecs;

            return singleAbieSpec;
        }

        // TODO: document
        ///<summary>
        ///</summary>
        ///<param name="maxOccurs"></param>
        ///<returns></returns>
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

        // TODO: document
        ///<summary>
        ///</summary>
        ///<param name="minOccurs"></param>
        ///<returns></returns>
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

        #endregion

        #region Private Class Methods
        
        // TODO: document
        private static void CumulateAbieSpecsFromSchema(XmlDocument bieSchemaDocument, IDictionary<string, IACC> existingAccs, IDictionary<string, IBDT> existingBdts, IDictionary<string, ABIESpec> allAbieSpecs, IDictionary<string, string> elementDefinitions)
        {
            // Create a new CustomSchemaReader class that allows us to process the BIE XML
            // schema. 
            CustomSchemaReader reader = new CustomSchemaReader(bieSchemaDocument);

            foreach (object item in reader.Items)
            {
                // In case the current Item returned by the CustomSchemaReader is a
                // complex type we know that we encountered an ABIE definition. 
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType)item;

                    // Therefore we create an ABIESpec based on the complex type 
                    // definition.
                    ABIESpec singleAbieSpec = CumulateAbieSpecFromComplexType(abieComplexType, existingAccs, existingBdts);

                    allAbieSpecs.Add(singleAbieSpec.Name, singleAbieSpec);
                }

                // Otherwise we can check if we encountered an element on the complex type 
                // level. If that's the case then we'll cache the element name and it's type
                // in our dictionary. 

                if (item is Element)
                {
                    Element element = (Element)item;
                    elementDefinitions.Add(element.Name, element.Type.Name);
                }
            }
        }

        // TODO: document
        private static ASBIESpec MatchAsbieToAscc(IACC acc, string asbieName, int associatedAbie)
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

        // TODO: document
        private static ICCLibrary GetAccLibrary(ICCRepository repository)
        {
            return repository.Libraries<ICCLibrary>().ElementAt(0);
        }

        // TODO: document
        private static IDictionary<string, IACC> CreateDictionaryFromAccLibrary(ICCLibrary accLibrary)
        {
            IDictionary<string, IACC> accs = new Dictionary<string, IACC>();

            foreach (IACC acc in accLibrary.Elements)
            {
                accs.Add(acc.Name, acc);
            }

            return accs;
        }

        // TODO: document
        private static IBDTLibrary GetBdtLibrary(ICCRepository repository)
        {
            return repository.Libraries<IBDTLibrary>().ElementAt(0);         
        }

        // TODO: document
        private static IDictionary<string, IBDT> CreateDictionaryFromBdtLibrary(IBDTLibrary bdtLibrary)
        {
            IDictionary<string, IBDT> bdts = new Dictionary<string, IBDT>();

            foreach (IBDT bdt in bdtLibrary.Elements)
            {
                bdts.Add(bdt.Name, bdt);
            }

            return bdts;
        }

        // The method returns the first BIE library found in the repository.
        private static IBIELibrary GetTargetBieLibrary(ICCRepository repository)
        {
            return repository.Libraries<IBIELibrary>().ElementAt(0);
        }

        // TODO: document
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