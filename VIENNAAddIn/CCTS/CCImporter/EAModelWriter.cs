using System;
using System.Collections.Generic;
using System.Text;
using EA;
using VIENNAAddIn.common.corecomponents;

namespace VIENNAAddIn.CCTS.CCLImporter
{
    class EAModelWriter
    {




        private String targetFile = "";
        private String libraryVersion = "";

        private EA.Repository r;



        /// <summary>
        /// Initialize the writer
        /// </summary>
        /// <param name="targetFile"></param>
        public EAModelWriter(String targetFile, String libraryVersion)
        {
            this.targetFile = targetFile;
            this.libraryVersion = libraryVersion;

            r = new Repository();

            //Open the target file
            try
            {
                r.OpenFile(targetFile);
            }
            catch (Exception e)
            {
                r.CreateModel(CreateModelType.cmEAPFromBase, targetFile, 0);
            }

            r.OpenFile(targetFile);


            //Delete all old elements
            EA.Collection currentModels = r.Models;
            foreach (EA.Package p in currentModels)
            {
                currentModels.DeleteAt(0, true);

            }

            //Create a new model for the core component definitions
            EA.Package newModel = (EA.Package)r.Models.AddNew(this.libraryVersion, "");
            newModel.Update();
            r.RefreshModelView(0);


        }

        /// <summary>
        /// Write the CDT definitions in the EA file
        /// </summary>
        /// <param name="cdts"></param>
        public Dictionary<string, EA.Element> writeCDT(List<CDT> cdts)
        {

            write("Starting EA Writer for CDT...");

            r.OpenFile(targetFile);
            EA.Package currentModel = (EA.Package)r.Models.GetAt(0);

            EA.Package bLibrary = (EA.Package)currentModel.Packages.AddNew("bLibrary", "");
            bLibrary.Update();
            bLibrary.Element.Stereotype = "bLibrary";


            //Create a core data type package
            EA.Package cdtPackage = (EA.Package)bLibrary.Packages.AddNew("CDTLibrary", "");
            cdtPackage.Update();
            cdtPackage.Element.Stereotype = "CDTLibrary";


            //Add a new diagram
            EA.Diagram diagram = (EA.Diagram)cdtPackage.Diagrams.AddNew("CDTLibraryDiagram", "");
            diagram.ShowDetails = 0;
            diagram.Update();


            Dictionary<string, EA.Element> savedCDTs = new Dictionary<string, Element>();

            foreach (CDT cdt in cdts)
            {

                //Create a new class for the CDT
                EA.Element cdtElem = (EA.Element)cdtPackage.Elements.AddNew(cdt.ObjectClassTerm, "Class");

                cdtElem.Stereotype = "CDT";
                //Save the definition into the notes field
                cdtElem.Notes = cdt.Definition;

                //Add the content component
                EA.Attribute con = (EA.Attribute)cdtElem.Attributes.AddNew("Content", "String");
                con.Stereotype = "CON";
                con.Notes = cdt.Con.Definition;
                con.Update();


                //Add the supplementary components
                if (cdt.Sups != null)
                {

                    foreach (SupplementaryComponent sup in cdt.Sups)
                    {
                        String name = sup.ObjectClassTerm + "." + sup.PropertyTerm;
                        //If a representation term is set add it as well
                        if (sup.RepresentationTerm == "")
                        {
                            name += "." + sup.RepresentationTerm;
                        }

                        EA.Attribute a = (EA.Attribute)cdtElem.Attributes.AddNew(name, sup.RepresentationTerm);

                        a.Stereotype = "SUP";
                        //Save the definition into the notes field
                        a.Notes = sup.Definition;
                        a.Update();
                    }
                }
                cdtElem.Update();

                //Save the saved ACCs for later use
                savedCDTs.Add(cdtElem.Name, cdtElem);


                //draw the newly created element on the diagram
                EA.Collection diagramObjects = diagram.DiagramObjects;
                EA.DiagramObject diagramObject = (EA.DiagramObject)diagramObjects.AddNew(cdtElem.Name, "Class");

                diagramObject.ElementID = cdtElem.ElementID;
                diagramObject.Update();
                diagram.Update();

                Console.WriteLine("Successfully added CDT " + cdt.ObjectClassTerm);
            }

            cdtPackage.Update();
            cdtPackage.Elements.Refresh();



            write("Finished EA Writer for CDT...");

            return savedCDTs;
        }




        /// <summary>
        /// Write the QDT definitions in the EA file
        /// </summary>
        /// <param name="cdts"></param>
        public Dictionary<string, EA.Element> writeQDT(List<QDT> qdts, Dictionary<string, EA.Element> savedCDTs)
        {

            write("Starting EA Writer for QDT...");

            r.OpenFile(targetFile);
            EA.Package currentModel = null;// 
            EA.Package cclRoot = (EA.Package)r.Models.GetAt(0);

            foreach (EA.Package p in cclRoot.Packages)
            {
                if (p.Name == "bLibrary")
                {
                    currentModel = p;
                    break;
                }
            }




            //Create a core data type package
            EA.Package qdtPackage = (EA.Package)currentModel.Packages.AddNew("QDTLibrary", "");
            qdtPackage.Update();
            qdtPackage.Element.Stereotype = "QDTLibrary";


            //Add a new diagram
            EA.Diagram diagram = (EA.Diagram)qdtPackage.Diagrams.AddNew("QDTLibraryDiagram", "");
            diagram.ShowDetails = 0;
            diagram.Update();


            Dictionary<string, EA.Element> savedQDTs = new Dictionary<string, Element>();

            foreach (QDT qdt in qdts)
            {

                //Create a new class for the QDT
                EA.Element qdtElem = (EA.Element)qdtPackage.Elements.AddNew(qdt.DataTypeQualifier + "_" + qdt.ObjectClassTerm, "Class");

                qdtElem.Stereotype = "QDT";
                //Save the definition into the notes field
                qdtElem.Notes = qdt.Definition;

                //Add a basedOn connector to the underlying CDT
                EA.Connector con = (EA.Connector)qdtElem.Connectors.AddNew("", "Dependency");

                //Get the underlying CDT
                EA.Element cdt_element = null;
                savedCDTs.TryGetValue(qdt.UnderlyingCDT, out cdt_element);

                if (cdt_element == null)
                {
                    throw new Exception("Unable to find underlying CDT for QDT " + qdt.ObjectClassTerm);
                }

                con.ClientID = cdt_element.ElementID;
                con.SupplierID = qdtElem.ElementID;

                //Set to composition                
                con.Stereotype = "basedOn";
                con.Update();



                //Add the content component
                EA.Attribute conComponent = (EA.Attribute)qdtElem.Attributes.AddNew("Content", qdt.RepresentationTerm);
                conComponent.Stereotype = "CON";
                conComponent.Notes = qdt.Con.Definition;
                conComponent.Update();


                //Add the supplementary components
                if (qdt.Sups != null)
                {

                    foreach (SupplementaryComponent sup in qdt.Sups)
                    {
                        String name = sup.ObjectClassTerm + "." + sup.PropertyTerm;
                        //If a representation term is set add it as well
                        if (sup.RepresentationTerm == "")
                        {
                            name += "." + sup.RepresentationTerm;
                        }

                        EA.Attribute a = (EA.Attribute)qdtElem.Attributes.AddNew(name, sup.RepresentationTerm);

                        a.Stereotype = "SUP";
                        //Save the definition into the notes field
                        a.Notes = sup.Definition;
                        a.Update();
                    }
                }
                qdtElem.Update();

                //Save the saved QDTs for later use
                savedQDTs.Add(qdtElem.Name, qdtElem);

                //draw the newly created element on the diagram
                EA.Collection diagramObjects = diagram.DiagramObjects;
                EA.DiagramObject diagramObject = (EA.DiagramObject)diagramObjects.AddNew(qdtElem.Name, "Class");

                diagramObject.ElementID = qdtElem.ElementID;
                diagramObject.Update();
                diagram.Update();

                Console.WriteLine("Successfully added QDT " + qdtElem.Name);
            }

            qdtPackage.Update();
            qdtPackage.Elements.Refresh();



            write("Finished EA Writer for QDT...");

            return savedQDTs;
        }









        /// <summary>
        /// Write the Core Component definitions in the EA file
        /// </summary>
        /// <param name="targetFile"></param>
        public Dictionary<string, EA.Element> writeCC(List<ACC> accs, Dictionary<string, EA.Element> savedCDTs)
        {


            write("Starting EA Writer for CC....");

            r.OpenFile(targetFile);

            EA.Package currentModel = null;// 
            EA.Package cclRoot = (EA.Package)r.Models.GetAt(0);

            foreach (EA.Package p in cclRoot.Packages)
            {
                if (p.Name == "bLibrary")
                {
                    currentModel = p;
                    break;
                }
            }

            //Create a core component package
            EA.Package ccPackage = (EA.Package)currentModel.Packages.AddNew("CCLibrary", "");
            ccPackage.Update();
            ccPackage.Element.Stereotype = "CCLibrary";

            //Add a new diagram
            EA.Diagram diagram = (EA.Diagram)ccPackage.Diagrams.AddNew("CCLibraryDiagram", "");
            diagram.ShowDetails = 0;
            diagram.Update();


            Dictionary<string, EA.Element> savedACCs = new Dictionary<string, Element>();


            foreach (ACC acc in accs)
            {

                //Create a new Class for the ACC
                EA.Element accElem = (EA.Element)ccPackage.Elements.AddNew(acc.ObjectClassTerm, "Class");
                accElem.Stereotype = "ACC";
                //Save the definition into the notes field
                accElem.Notes = acc.Definition;

                if (acc.Bccs != null)
                {

                    foreach (BCC bcc in acc.Bccs)
                    {

                        EA.Attribute a = (EA.Attribute)accElem.Attributes.AddNew(bcc.PropertyTerm, bcc.RepresentationTerm);

                        setMultiplicity(a, bcc);

                        a.Stereotype = "BCC";
                        //Save the definition into the notes field
                        a.Notes = bcc.Definition;
                        a.Update();
                    }
                }
                accElem.Update();

                //Save the saved ACCs for later use
                savedACCs.Add(accElem.Name, accElem);

                //draw the newly created element on the diagram
                EA.Collection diagramObjects = diagram.DiagramObjects;
                EA.DiagramObject diagramObject = (EA.DiagramObject)diagramObjects.AddNew(accElem.Name, "Class");

                diagramObject.ElementID = accElem.ElementID;
                diagramObject.Update();
                diagram.Update();

                Console.WriteLine("Successfully added ACC (with BCCs) " + acc.ObjectClassTerm);
            }

            ccPackage.Update();
            ccPackage.Elements.Refresh();



            //After all ACCs are added to the model we continue with the ASCCs
            foreach (ACC acc in accs)
            {


                //Get the acc as EA.Element
                //Element acc_element = getElementByName(acc.ObjectClassTerm, ccPackage);

                Element acc_element = null;

                //Get a reference on the ACC we have saved before
                savedACCs.TryGetValue(acc.ObjectClassTerm, out acc_element);


                String d = acc_element.Name;

                //Are there any ASCCs?
                if (acc.Asccs != null)
                {

                    foreach (ASCC ascc in acc.Asccs)
                    {

                        //Get the ascc as EA.Element
                        EA.Element ascc_element = null; // getElementByName(ascc.AssociatedObjectClass, ccPackage);

                        savedACCs.TryGetValue(ascc.AssociatedObjectClass, out ascc_element);

                        if (ascc_element == null)
                        {
                            String dummy = "";
                        }

                        //Now add a connector
                        EA.Connector con = (EA.Connector)acc_element.Connectors.AddNew("", "Aggregation");

                        con.ClientID = ascc_element.ElementID;
                        con.SupplierID = acc_element.ElementID;

                        con.ClientEnd.Role = ascc.PropertyTerm;
                        con.ClientEnd.Cardinality = getASCCMultiplicity(ascc);

                        //Set to composition
                        con.SupplierEnd.Aggregation = 2;
                        con.Stereotype = "ASCC";
                        //Save the definition into the notes field
                        con.Notes = ascc.Definition;
                        con.Update();
                    }
                }

                Console.WriteLine("Successfully added the ASCCs of ACC " + acc.ObjectClassTerm);

            }


            ccPackage.Update();
            ccPackage.Elements.Refresh();

            r.RefreshModelView(0);
            r.CloseFile();

            write("Finished EA Writer for CC successfully.");


            return savedACCs;

        }



        /// <summary>
        /// Write the BIE definitions in the EA file
        /// </summary>
        /// <param name="targetFile"></param>
        public void writeBIE(List<ABIE> abies, Dictionary<string, EA.Element> savedQDTs, Dictionary<string, EA.Element> savedCCs)
        {


            write("Starting EA Writer for BIE....");

            r.OpenFile(targetFile);

            EA.Package currentModel = null;// 
            EA.Package cclRoot = (EA.Package)r.Models.GetAt(0);

            foreach (EA.Package p in cclRoot.Packages)
            {
                if (p.Name == "bLibrary")
                {
                    currentModel = p;
                    break;
                }
            }

            //Create a core component package
            EA.Package biePackage = (EA.Package)currentModel.Packages.AddNew("BIELibrary", "");
            biePackage.Update();
            biePackage.Element.Stereotype = "BIELibrary";

            //Add a new diagram
            EA.Diagram diagram = (EA.Diagram)biePackage.Diagrams.AddNew("BIELibraryDiagram", "");
            diagram.ShowDetails = 0;
            diagram.Update();

            Dictionary<string, EA.Element> savedABIEs = new Dictionary<string, Element>();

            foreach (ABIE abie in abies)
            {

                //Create a new Class for the ABIE
                EA.Element abieElem = (EA.Element)biePackage.Elements.AddNew(getABIEName(abie), "Class");
                abieElem.Stereotype = "ABIE";
                //Save the definition into the notes field
                abieElem.Notes = abie.Definition;

                if (abie.Bbies != null)
                {

                    foreach (BBIE bbie in abie.Bbies)
                    {

                        EA.Attribute a = (EA.Attribute)abieElem.Attributes.AddNew(getBBIEName(bbie), bbie.RepresentationTerm);

                        setMultiplicity(a, bbie);

                        a.Stereotype = "BBIE";
                        //Save the definition into the notes field
                        a.Notes = bbie.Definition;
                        a.Update();
                    }
                }
                abieElem.Update();


                //Save the saved ABIE for later use
                savedABIEs.Add(abieElem.Name, abieElem);



                //draw the newly created element on the diagram
                EA.Collection diagramObjects = diagram.DiagramObjects;
                EA.DiagramObject diagramObject = (EA.DiagramObject)diagramObjects.AddNew(abieElem.Name, "Class");

                diagramObject.ElementID = abieElem.ElementID;
                diagramObject.Update();
                diagram.Update();

                Console.WriteLine("Successfully added ABIE (with BBIEs) " + getABIEName(abie));
            }

            biePackage.Update();
            biePackage.Elements.Refresh();



            //After all ABIEs are added to the model we continue with the ASBIEs
            foreach (ABIE abie in abies)
            {


                //Get the abie as EA.Element
                Element abie_element = null;

                //Get a reference on the ABIE we have saved before
                savedABIEs.TryGetValue(getABIEName(abie), out abie_element);

                //Are there any ASBIEs?
                if (abie.Asbies != null)
                {

                    foreach (ASBIE asbie in abie.Asbies)
                    {

                        //Get the asbie as EA.Element
                        EA.Element asbie_element = null; // getElementByName(ascc.AssociatedObjectClass, ccPackage);

                        savedABIEs.TryGetValue(getASBIEName(asbie), out asbie_element);


                        //Now add a connector
                        EA.Connector con = (EA.Connector)abie_element.Connectors.AddNew("", "Aggregation");

                        con.ClientID = asbie_element.ElementID;
                        con.SupplierID = abie_element.ElementID;

                        con.ClientEnd.Role = asbie.PropertyTerm;
                        con.ClientEnd.Cardinality = getASBIEMultiplicity(asbie);

                        //Set to composition
                        con.SupplierEnd.Aggregation = 2;
                        con.Stereotype = "ASBIE";
                        //Save the definition into the notes field
                        con.Notes = asbie.Definition;
                        con.Update();
                    }
                }

                Console.WriteLine("Successfully added the ASBIEs of ABIE " + getABIEName(abie));

            }


            biePackage.Update();
            biePackage.Elements.Refresh();

            r.RefreshModelView(0);
            r.CloseFile();

            write("Finished EA Writer for ABIE successfully.");

        }


        /// <summary>
        /// Get the name of the BBIE
        /// </summary>
        /// <param name="bbie"></param>
        /// <returns></returns>
        private String getBBIEName(BBIE bbie)
        {
            String s = "";

            if (bbie.PropertyTermQualifier != null && bbie.PropertyTermQualifier != "")
                s += bbie.PropertyTermQualifier + "_";

            return s += bbie.PropertyTerm;



        }







        /// <summary>
        /// Return the name of the ABIE
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private String getABIEName(ABIE abie)
        {

            String s = "";

            if (abie.ObjectClassTermQualifier != null && abie.ObjectClassTermQualifier != "")
                s += abie.ObjectClassTermQualifier + "_";

            return s += abie.ObjectClassTerm;
        }



        /// <summary>
        /// Return the name of the ASBIE
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private String getASBIEName(ASBIE asbie)
        {

            String s = "";

            if (asbie.AssociatedObjectClassTermQualifier != null && asbie.AssociatedObjectClassTermQualifier != "")
                s += asbie.AssociatedObjectClassTermQualifier + "_";

            return s += asbie.AssociatedObjectClass;
        }





        /// <summary>
        /// Set the multiplicity of the attribute according to the specifiction in the
        /// bcc
        /// </summary>
        /// <param name="a"></param>
        /// <param name="bcc"></param>
        private void setMultiplicity(EA.Attribute a, BBIE bbie)
        {

            if (bbie.LowerBoundMultiplicity.Trim() == "0")
            {
                a.LowerBound = "0";
            }
            else if (bbie.LowerBoundMultiplicity.Trim() == "1")
            {
                a.LowerBound = "1";
            }

            if (bbie.UpperBoundMultiplicity.Trim() == "unbounded")
            {
                a.UpperBound = "*";
            }
            else if (bbie.UpperBoundMultiplicity.Trim() == "1")
            {
                a.UpperBound = "1";
            }

        }


        /// <summary>
        /// Returns the cardinality of the ASCC
        /// </summary>
        /// <param name="ascc"></param>
        /// <returns></returns>
        internal static String getASCCMultiplicity(ASCC ascc)
        {
            String s = "";


            if (ascc.LowerBoundMultiplicity.Trim() == "0")
            {
                s += "0..";
            }
            else if (ascc.LowerBoundMultiplicity.Trim() == "1")
            {
                s += "1..";
            }

            if (ascc.UpperBoundMultiplicity.Trim() == "unbounded")
            {
                s += "*";
            }
            else if (ascc.UpperBoundMultiplicity.Trim() == "1")
            {
                s += "1";
            }


            return s;
        }



        /// <summary>
        /// Returns the cardinality of the ASBIE
        /// </summary>
        /// <param name="ascc"></param>
        /// <returns></returns>
        internal static String getASBIEMultiplicity(ASBIE asbie)
        {
            String s = "";


            if (asbie.LowerBoundMultiplicity.Trim() == "0")
            {
                s += "0..";
            }
            else if (asbie.LowerBoundMultiplicity.Trim() == "1")
            {
                s += "1..";
            }

            if (asbie.UpperBoundMultiplicity.Trim() == "unbounded")
            {
                s += "*";
            }
            else if (asbie.UpperBoundMultiplicity.Trim() == "1")
            {
                s += "1";
            }


            return s;
        }












        ///// <summary>
        ///// Set the multiplicity of the attribute according to the specifiction in the
        ///// bcc
        ///// </summary>
        ///// <param name="a"></param>
        ///// <param name="bcc"></param>
        private void setMultiplicity(EA.Attribute a, BCC bcc)
        {

            if (bcc.LowerBoundMultiplicity.Trim() == "0")
            {
                a.LowerBound = "0";
            }
            else if (bcc.LowerBoundMultiplicity.Trim() == "1")
            {
                a.LowerBound = "1";
            }

            if (bcc.UpperBoundMultiplicity.Trim() == "unbounded")
            {
                a.UpperBound = "*";
            }
            else if (bcc.UpperBoundMultiplicity.Trim() == "1")
            {
                a.UpperBound = "1";
            }

        }









        void write(String s)
        {
            Console.WriteLine(s);
        }



    }
}
