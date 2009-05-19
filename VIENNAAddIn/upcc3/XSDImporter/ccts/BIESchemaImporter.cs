// TODO: remove all Console.WriteLine statements once implementation is complete

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    internal class Node
    {
        internal Node()
        {
            Children = new List<Node>();
        }

        internal ABIESpec ABIESpec { get; set; }
        internal List<Node> Children { get; set; }
        internal Node Parent { get; set; }

        internal void AttachNode(ABIESpec abieSpec)
        {
            Node newChild = new Node();
            newChild.ABIESpec = abieSpec;
            newChild.Parent = this;
            Children.Add(newChild);
        }
    }

    //internal class Node
    //{
    //    internal Node()
    //    {
            
    //    }

    //    internal ABIESpec ABIESpec { get; private set; }
    //    internal List<Node> Children { get; private set; }
    //    internal Node Parent { get; private set; }
    //}

    //internal class RootNode
    //{
    //    private readonly List<Node> children;
    //    private Node currentChild;
        
    //    internal RootNode()
    //    {
    //        children = new List<Node>();
    //    }

    //    internal void AddChild(Node newChild)
    //    {
    //        children.Add(newChild);
    //    }

    //    internal Node GetNextChild()
    //    {
    //        if (currentChild == null)
    //        {
    //            currentChild = 
    //        }
    //    }
    //}

    ///<summary>
    ///</summary>
    public class BIESchemaImporter
    {
        public static ICCRepository repository;
        
        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        public static void ImportXSD(ImporterContext context)
        {
            #region Import Preparation

            // Get the repository that we import the ABIEs into. 
            repository = context.Repository;

            // Get the BIE library that we import all the ABIEs into. 
            IBIELibrary bieLibrary = GetTargetBIELibrary(repository);

            // Get the appropriate XML schema containing all ABIEs. 
            XmlSchema bieSchema = GetBIESchema(context);

            #endregion

            // PrintElementsAndComplexTypesFromSchema(bieSchema);
            // TraverseThroughComplexTypesInSchema(bieSchema);

            Node root = CumulateTreeFromSchema(bieSchema);

            WriteBIEsToLibrary(root, bieLibrary);
        }

        private static Node CumulateTreeFromSchema(XmlSchema schema)
        {
            Node root = new Node();
          

            

            Console.WriteLine("-------------");            
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(schema.Namespaces);
            XmlQualifiedName[] qualifiedNames = namespaces.ToArray();

            foreach (XmlQualifiedName x in qualifiedNames)
            {
                Console.WriteLine("prefix: {0}, namespace: {1}", x.Name, x.Namespace);                
            }

            Console.WriteLine("-------------");






            foreach (XmlSchemaObject item in schema.Items)
            {
                if (item is XmlSchemaComplexType)
                {
                    ABIESpec abieSpec = CumulateABIESpecFromComplexType((XmlSchemaComplexType) item);

                    root.AttachNode(abieSpec);
                }
            }

            return root;
        }

        private static ABIESpec CumulateABIESpecFromComplexType(XmlSchemaComplexType abieComplexType)
        {
            // Get the BDT library that contains all BDTs
            IBDTLibrary bdtLibrary = GetBDTLibrary(repository);

            ABIESpec abieSpec = new ABIESpec();

            abieSpec.Name = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);
            Console.WriteLine("\nProcessing ABIE: {0}", abieComplexType.Name);
            // TODO: abieSpec.ASBIEs = 
            // TODO: abieSpec.BasedOn = 

            // Process the sequence particle of the which equals the BBIEs to be created
            // for the current ABIE. 

            XmlSchemaObject particle = abieComplexType.Particle;

            // Ensure that the particle is a sequence.
            // Note: is it possible that complex types contain anything different than sequences?
            if (particle is XmlSchemaSequence)
            {
                XmlSchemaSequence sequence = (XmlSchemaSequence) particle;
                List<BBIESpec> bbieSpecs = new List<BBIESpec>();

                foreach (XmlSchemaElement sequenceElement in sequence.Items)
                {
                    BBIESpec bbieSpec = new BBIESpec();
                    bbieSpec.Name = sequenceElement.Name;

                    bbieSpec.LowerBound = ResolveMinOccurs(sequenceElement.MinOccursString);
                    bbieSpec.UpperBound = ResolveMaxOccurs(sequenceElement.MaxOccursString);
                    
                    Console.WriteLine("- Name: {0}", sequenceElement.Name);
                    Console.WriteLine("- Ref Name: {0}", sequenceElement.RefName);
                    Console.WriteLine("- Schema Type Name: {0}", sequenceElement.SchemaTypeName);
                    XmlQualifiedName qn = sequenceElement.SchemaTypeName;
                    Console.WriteLine("- Schema Type Name Qualified Name: " + qn.Name);
                    Console.WriteLine("- Schema Type Name Qualified Namespace: " + qn.Namespace);
                    

                    Console.WriteLine();
                   
                    

                    bbieSpecs.Add(bbieSpec);
                }

                abieSpec.BBIEs = bbieSpecs;
            }

            return abieSpec;
        }

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

        private static void WriteBIEsToLibrary(Node root, IBIELibrary bieLibrary)
        {
            foreach (Node child in root.Children)
            {
                //Console.WriteLine("Processing ABIE: {0}", child.ABIESpec.Name);
                //bieLibrary.CreateElement(child.ABIESpec);
            }
        }

        private static IBDTLibrary GetBDTLibrary(ICCRepository repository)
        {
            return repository.Libraries<IBDTLibrary>().ElementAt(0);
        }

        #region Internal Class Methods

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

        #endregion

        #region Internal Player Methods :-)

        private static void PrintElementsAndComplexTypesFromSchema(XmlSchema schema)
        {
            Console.WriteLine("Starting to read XML schema ...");

            foreach (XmlSchemaObject item in schema.Items)
            {
                if (item is XmlSchemaElement)
                {
                    Console.WriteLine("Element: {0}", ((XmlSchemaElement) item).Name);
                }
            }

            foreach (XmlSchemaObject item in schema.Items)
            {
                if (item is XmlSchemaComplexType)
                {
                    Console.WriteLine("Complex Type: {0}", ((XmlSchemaComplexType) item).Name);
                }
            }

            Console.WriteLine("Finished reading XML schema ...");
        }

        private static void TraverseThroughComplexTypesInSchema(XmlSchema schema)
        {
            foreach (XmlSchemaObject item in schema.Items)
            {
                if (item is XmlSchemaComplexType)
                {
                    XmlSchemaComplexType abieComplexType = ((XmlSchemaComplexType) item);

                    Console.WriteLine("ComplexType: {0}", abieComplexType.Name);

                    XmlSchemaObject childItem = abieComplexType.Particle;

                    if (childItem is XmlSchemaSequence)
                    {
                        XmlSchemaSequence sequence = (XmlSchemaSequence) childItem;

                        foreach (XmlSchemaElement sequenceElement in sequence.Items)
                        {
                            Console.WriteLine("Element     : {0}", sequenceElement.Name);
                            Console.WriteLine("   Type     : {0}", sequenceElement.SchemaTypeName);
                            Console.WriteLine("   MinOccurs: {0}", sequenceElement.MinOccursString);
                            Console.WriteLine("   MaxOccurs: {0}", sequenceElement.MaxOccursString);
                        }
                    }
                }
            }
        }

        #endregion
    }
}