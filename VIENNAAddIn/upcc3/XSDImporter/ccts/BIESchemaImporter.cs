// TODO: remove all Console.WriteLine statements once implementation is complete

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    ///<summary>
    ///</summary>
    public class BIESchemaImporter
    {
        //public static ICCRepository repository;

        public static void ImportXSD(ImporterContext context)
        {
            #region Import Preparation

            // Get the ACC library that contains all the ACCs that the
            // ABIEs to be imported are based on.
            ICCLibrary accLibrary = GetACCLibrary(context.Repository);
            // Get all ACCs from the library and cache them in a local dictionary.
            IDictionary<string, IACC> existingACCs = GetACCsFromACCLibrary(accLibrary);


            // Get the BDT library that contains all BDTs
            IBDTLibrary bdtLibrary = GetBDTLibrary(context.Repository);
            // Get all BDTs from the library and cache them in a local dictionary.
            IDictionary<string, IBDT> existingBDTs = GetBDTsFromBDTLibrary(bdtLibrary);


            // Get the BIE library that we import all the ABIEs into. 
            IBIELibrary bieLibrary = GetTargetBIELibrary(context.Repository);
            
            // NOTE: BEGIN temporary code
            // TODO: we need to adapt the importer context to use XML document objects
            // instead of XML schema objects. Otherwise using the XML schema objects in the
            // intermediate step will cause all information that we need to be lost. 
            
            //// Get the appropriate XML schema containing all ABIEs. 
            //XmlSchema bieSchema = GetBIESchema(context);

            //// Load the XML schema document into an XML document object
            //// since the CustomSchemaReader requires an XML document 
            //// object as input.
            //XmlDocument bieSchemaDocument = new XmlDocument();
            //bieSchemaDocument.LoadXml(bieSchema.ToString());

            XmlDocument bieSchemaDocument = GetBIESchemaDocument(context);

            // NOTE: END temporary code
            
            #endregion

            // NOTE: BEGIN temporary code that needs to be refactored

            CustomSchemaReader reader = new CustomSchemaReader(bieSchemaDocument);

            IDictionary<string, ABIESpec> allAbieSpecs = new Dictionary<string, ABIESpec>();
            IDictionary<string, string> elementDefinitions = new Dictionary<string, string>();

            #region Step 1: Create all ABIEs in the BIE library             
                
            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    ABIESpec singleAbieSpec = new ABIESpec();
                    ComplexType abieComplexType = (ComplexType) item;

                    string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

                    singleAbieSpec.Name = abieName;
                    
                    singleAbieSpec.BasedOn = existingACCs[abieName];

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

                                foreach (IBDT bdt in existingBDTs.Values)
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
                    
                    allAbieSpecs.Add(singleAbieSpec.Name, singleAbieSpec);
                }

                if (item is Element)
                {
                    Element element = (Element) item;
                    elementDefinitions.Add(element.Name, element.Type.Name);
                }
            }

            // The set of ABIESpecs in the dictionary is now used to create ABIEs 
            // in the BIE library. 

            foreach (ABIESpec abieSpec in allAbieSpecs.Values)
            {
                Console.Write("Attempting to write ABIE: {0}", abieSpec.Name);
                bieLibrary.CreateElement(abieSpec);
                Console.WriteLine(" - Success :-)");
            }

            #endregion

            #region Step 2: Update all ABIEs with their ASBIEs

            CustomSchemaReader reader2 = new CustomSchemaReader(bieSchemaDocument);

            foreach (object item in reader2.Items)
            {
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType) item;

                    foreach (Element element in abieComplexType.Items)
                    {
                        if (!(element.Ref.Name.Equals("")))
                        {
                            // Voraussetzungen:
                            //  - ABIE: Address
                            //          --> hat eine ASBIE auf Person
                            //  - ABIE: Person

                            // Was brauche ich um den ABIE Address richtig upzudaten?
                            //  - ABIESpec für den Address
                            //  - ASCC welche die Verbindung zwischen den ACCs Address und Person ausdrückt
                            //  - den ABIE Person
                            
                            // Read the ABIESpec of the ABIE that is going to be updated
                            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);
                            ABIESpec abieSpecToBeUpdated = allAbieSpecs[abieName];
                            IABIE abieToBeUpdated = null;
                            
                            // Read the ABIE from the BIE library 
                            foreach (IABIE abie in bieLibrary.Elements)
                            {
                                if (abie.Name.Equals(abieName))
                                {
                                    abieToBeUpdated = abie;
                                    break;
                                }
                            }

                            // Resolve the name of the associated ABIE
                            string associatedABIEName = elementDefinitions[element.Ref.Name];
                            associatedABIEName = associatedABIEName.Substring(0, associatedABIEName.Length - 4);
                            IABIE associatedAbie = null;

                            // Read the associated ABIE from the BIE library 
                            foreach (IABIE abie in bieLibrary.Elements)
                            {
                                if (abie.Name.Equals(associatedABIEName))
                                {
                                    associatedAbie = abie;
                                    break;
                                }
                            }

                            // Wie muss ich dann weitermachen wenn ich die gesamte Information zur Verfügung habe?
                            // - Ich muss die ASCC clonen
                            // - Ich muss die ABIESpec für Address entsprechend updaten
                            // - Ich muss das Update auf den ABIE ausführen. 


                            // Furthermore we need to read the ASCC that the ASBIE is based on                                                        
                            IList<ASBIESpec> asbieSpecs = new List<ASBIESpec>();
                            foreach (IASCC ascc in existingACCs[abieName].ASCCs)
                            {
                                // we've found the matching ASCC
                                if (ascc.Name.Equals(element.Ref.Name))
                                {
                                    // TODO: remove ABIE name from ASBIE name (e.g. remove "Person" from "Included_PersonPerson")

                                    // TODO: if matching is successful we base the ASBIE on an ASCC, otherwise we skip and create the ASBIE only
                                    //ASBIESpec asbieSpec = ASBIESpec.CloneASCC(ascc, ascc.Name, associatedAbie.Id);
                                    //asbieSpecs.Add(asbieSpec);
                                                                       
                                    break;
                                }
                            }

                            // NOTE: temporary assumption that no ASBIEs are based on ASCCs. All ASBIEs are created from scratch!
                            ASBIESpec asbieSpec = new ASBIESpec();
                            asbieSpec.AssociatedABIEId = associatedAbie.Id;
                            asbieSpec.Name = element.Ref.Name;
                            asbieSpecs.Add(asbieSpec);


                            abieSpecToBeUpdated.ASBIEs = asbieSpecs;

                            bieLibrary.UpdateElement(abieToBeUpdated, abieSpecToBeUpdated);
                            break;
                        }
                    }                    
                }                
            }

            #endregion

            //// NOTE: END temporary code that needs to be refactored
        }

        #region unused code

        //private static IDictionary<string, ABIESpec> CumulateABIESpecsFromSchema(XmlDocument bieSchemaDocument, IDictionary<string, IACC> accLibrary, IDictionary<string, IBDT> bdtLibrary)
        //{
        //    IDictionary<string, ABIESpec> abieSpecs = new Dictionary<string, ABIESpec>();

        //    CustomSchemaReader reader = new CustomSchemaReader(bieSchemaDocument);

        //    foreach (object item in reader.Items)
        //    {
        //        if (item is ComplexType)
        //        {
        //            ABIESpec abieSpec = CumulateABIESpecFromComplexType((ComplexType) item, accLibrary, bdtLibrary);     
        //            abieSpecs.Add(abieSpec.Name, abieSpec);
        //        }
        //    }

        //    return abieSpecs;
        //}


        //private static ABIESpec CumulateABIESpecFromComplexType(ComplexType abieComplexType, IDictionary<string, IACC> accLibrary, IDictionary<string, IBDT> bdtLibrary)
        //{
        //    ABIESpec abieSpec = new ABIESpec();

        //    string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

        //    abieSpec.Name = abieName;
        //    abieSpec.BasedOn = accLibrary[abieName];

        //    List<BBIESpec> bbieSpecs = new List<BBIESpec>();

        //    foreach (object item in abieComplexType.Items)
        //    {
        //        if (item is Element)
        //        {
        //            Element element = (Element) item;
                    
        //            if (element.Ref.Name.Equals(""))
        //            {
        //                BBIESpec bbieSpec = new BBIESpec();

        //                bbieSpec.Name = element.Name;
        //                bbieSpec.LowerBound = ResolveMinOccurs(element.MinOccurs);
        //                bbieSpec.UpperBound = ResolveMaxOccurs(element.MaxOccurs);

        //                string bdtName = element.Type.Name.Substring(0, element.Type.Name.Length - 4);

        //                foreach (IBDT bdt in bdtLibrary.Values)
        //                {
        //                    if ((bdt.Name + bdt.CON.BasicType.DictionaryEntryName).Equals(bdtName))
        //                    {
        //                        bbieSpec.Type = bdt;
        //                        break;
        //                    }
        //                }

        //                bbieSpecs.Add(bbieSpec);                        
        //            }
        //        }
        //    }
        //    abieSpec.BBIEs = bbieSpecs;


        //    return abieSpec;
        //}
        #endregion

        #region Internal Class Methods

        // TODO: document
        private static ICCLibrary GetACCLibrary(ICCRepository repository)
        {
            return repository.Libraries<ICCLibrary>().ElementAt(0);
        }

        // TODO: document
        private static IDictionary<string, IACC> GetACCsFromACCLibrary(ICCLibrary accLibrary)
        {
            IDictionary<string, IACC> accs = new Dictionary<string, IACC>();

            foreach (IACC acc in accLibrary.Elements)
            {
                accs.Add(acc.Name, acc);
            }

            return accs;
        }

        // TODO: document
        private static IBDTLibrary GetBDTLibrary(ICCRepository repository)
        {
            return repository.Libraries<IBDTLibrary>().ElementAt(0);
        }

        // TODO: document
        private static IDictionary<string, IBDT> GetBDTsFromBDTLibrary(IBDTLibrary bdtLibrary)
        {
            IDictionary<string, IBDT> bdts = new Dictionary<string, IBDT>();

            foreach (IBDT bdt in bdtLibrary.Elements)
            {
                bdts.Add(bdt.Name, bdt);
            }

            return bdts;
        }

        // The method returns the first BIE library found in the repository.
        private static IBIELibrary GetTargetBIELibrary(ICCRepository repository)
        {
            return repository.Libraries<IBIELibrary>().ElementAt(0);
        }

        // The method iterates through all XML schemas available in the 
        // ImporterContext. Once an XML schema whose file name starts with 
        // "BusinessInformationEntity" is found the XML schema is returned. 
        // In case no suitable XML schema was found in the context, then an 
        // empty XmlSchema is returned.
        private static XmlSchema GetBIESchema(ImporterContext context)
        {
            XmlSchema bieSchema = new XmlSchema();

            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                if (schemaInfo.FileName.StartsWith("BusinessInformationEntity"))
                {
                    bieSchema = schemaInfo.Schema;
                    break;
                }
            }

            return bieSchema;
        }

        // TODO: document
        private static XmlDocument GetBIESchemaDocument(ImporterContext context)
        {
            XmlDocument bieSchemaDocument = new XmlDocument();

            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                if (schemaInfo.FileName.StartsWith("BusinessInformationEntity"))
                {
                    // NOTE: temporary fix for the first version
                    string schemaDirectory = Directory.GetCurrentDirectory() +
                        "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";
                    
                    bieSchemaDocument.Load(schemaDirectory + schemaInfo.FileName);
                    
                    break;
                }
            }

            return bieSchemaDocument;
        }

        // TODO: document
        private static string ResolveMaxOccurs(string maxOccurs)
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
        private static string ResolveMinOccurs(string minOccurs)
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

        //// TODO: document
        //private static void WriteBIEsToLibrary(IDictionary<string, ABIESpec> abieSpecs, IBIELibrary bieLibrary)
        //{
        //    foreach (ABIESpec abieSpec in abieSpecs.Values)
        //    {
        //        // NOTE: currently (temporarily) disabled 
        //        //Console.WriteLine("Writing ABIE: {0}   (NOT YET IMPLEMENTED)", abieSpec.Name);

        //        Console.WriteLine("Attempting to write ABIE: {0}", abieSpec.Name);
        //        bieLibrary.CreateElement(abieSpec);
        //        Console.WriteLine("Successfully wrote ABIE: {0}", abieSpec.Name);
        //    }
        //}

        #endregion
    }
}