using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using EA;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDImporter.util;
using Element=VIENNAAddIn.upcc3.XSDImporter.util.Element;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    
    public class RootSchemaImporter
    {
        private static IDOCLibrary DocLibrary;
        private static ICCLibrary ExistingAccs;
        private static IBDTLibrary ExistingBdts;

        public static void ImportXSD(ImporterContext context)
        {
            ICCRepository repo = context.Repository;
            //BIESchemaImporter.InitLibraries(context);
            BIESchemaImporter.ImportXSD(context);
            XmlSchema root = context.Root.Schema;
            
            int index = context.Root.FileName.IndexOf('_');
            String docname = context.Root.FileName.Substring(0, index);

            //TODO check whether Document with the specified name does not already exist
            IBLibrary bLibrary = repo.Libraries<IBLibrary>().ElementAt(0);
            DocLibrary = bLibrary.CreateDOCLibrary(new LibrarySpec { Name = docname });

            foreach (var currentElement in root.Items)
            {
                if (currentElement.GetType().Name == "XmlSchemaElement")
                {
                    XmlSchemaElement element = (XmlSchemaElement) currentElement;
                    Console.WriteLine("Elementqualifiedname: " + element.QualifiedName.ToString() + element.SchemaTypeName.Name.ToString());
                    Console.WriteLine(element);
                }

            }
            

            XmlDocument rootDocument = new XmlDocument();
            rootDocument.Load(context.InputDirectory + context.Root.FileName);
            CustomSchemaReader reader = new CustomSchemaReader(rootDocument);
            
            IDictionary<string, string> allElementDefinitions = new Dictionary<string, string>();

            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType)item;

                    ABIESpec singleAbieSpec = BIESchemaImporter.CumulateAbieSpecFromComplexType(abieComplexType);

                    DocLibrary.CreateElement(singleAbieSpec);
                }

                if (item is Element)
                {
                    Element element = (Element)item;
                    allElementDefinitions.Add(element.Name, element.Type.Name);
                }
            }

            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType) item;

                    string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

                    IABIE abieToBeUpdated = DocLibrary.ElementByName(abieName);
                    ABIESpec updatedAbieSpec = new ABIESpec(abieToBeUpdated);

                    IList<ASBIESpec> newAsbieSpecs = BIESchemaImporter.CumulateAbiesSpecsFromComplexType(abieComplexType, allElementDefinitions);

                    foreach (ASBIESpec newAsbieSpec in newAsbieSpecs)
                    {
                        updatedAbieSpec.AddASBIE(newAsbieSpec);                        
                    }

                    DocLibrary.UpdateElement(abieToBeUpdated, updatedAbieSpec);
                }
            }

            // Diagram auto-layout
            var repository = (Repository)context.Repository;
            foreach(Package model in repository.Models)
            {
                foreach(Package package in model.Packages)
                {
                    foreach(Diagram diagram in package.Diagrams)
                    {
                        repository.GetProjectInterface().LayoutDiagramEx(diagram.DiagramGUID, 0, 4, 20, 20, true);
                    }
                }
            }
        }
    }
}
