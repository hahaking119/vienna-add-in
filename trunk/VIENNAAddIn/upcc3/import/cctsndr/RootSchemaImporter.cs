// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.import.util;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    ///<summary>
    /// The purpose of the static class RootSchemaImporter is to import an XML schema, 
    /// complying with CCTS and UN/CEFACT's NDRs, into an existing DOC library. The 
    /// importer is invoked using the static method "ImportXsd". For more information
    /// on the method "ImportXsd" please refer to the documentation of the method
    /// itself. Furthermore, the class provides a set of methods allowing to cumulate
    /// different artifacts in the CCTS XML schema such as cumulating an ABIE from 
    /// a complex type in the XML schema. 
    ///</summary>
    public class RootSchemaImporter
    {
        private static IDOCLibrary DocLibrary;
        private static IBDTLibrary ExistingBdts;

        ///<summary>
        /// The static method imports an XML schema containing Aggregated Business Information 
        /// Entities into an existing DOC library. The method has one input parameter
        /// of type ImporterContext specifying different settings utilized while 
        /// importing the XML schema.
        ///</summary>
        ///<param name="context">
        /// The parameter provides the necessary context information required by the
        /// DOC XML schema importer. An example would be the directory location where
        /// all XML schemas to be imported are located, and the repository which contains
        /// the DOC library that all ABIEs should be imported into. Fore more information
        /// plese refer to the documentation of the ImporterContext class. 
        /// </param>
        public static void ImportXSD(ImporterContext context)
        {
            ExistingBdts = context.BDTLibrary;

            int index = context.RootSchemaFileName.IndexOf('_');
            String docname = context.RootSchemaFileName.Substring(0, index);

            //TODO check whether Document with the specified name does not already exist
            IBLibrary bLibrary = context.BLibrary;
            DocLibrary = bLibrary.CreateDOCLibrary(new LibrarySpec {Name = docname});

            var rootDocument = new XmlDocument();
            rootDocument.Load(context.RootSchemaPath);
            var reader = new CustomSchemaReader(rootDocument);

            IDictionary<string, string> allElementDefinitions = new Dictionary<string, string>();

            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    var abieComplexType = (ComplexType) item;

                    ABIESpec singleAbieSpec = CumulateAbieSpecFromComplexType(abieComplexType);

                    DocLibrary.CreateElement(singleAbieSpec);
                }

                if (item is Element)
                {
                    var element = (Element) item;
                    allElementDefinitions.Add(element.Name, element.Type.Name);
                }
            }

            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    var abieComplexType = (ComplexType) item;

                    string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

                    IABIE abieToBeUpdated = DocLibrary.ElementByName(abieName);
                    if (abieToBeUpdated == null)
                    {
                        abieToBeUpdated = BIESchemaImporter.getElementByName(abieName);
                        var updatedAbieSpec = new ABIESpec(abieToBeUpdated);

                        IList<ASBIESpec> newAsbieSpecs = CumulateAsbieSpecsFromComplexType(abieComplexType,
                                                                                           allElementDefinitions);

                        foreach (ASBIESpec newAsbieSpec in newAsbieSpecs)
                        {
                            updatedAbieSpec.AddASBIE(newAsbieSpec);
                        }
                        DocLibrary.UpdateElement(abieToBeUpdated, updatedAbieSpec);
                    }
                    else
                    {
                        var updatedAbieSpec = new ABIESpec(abieToBeUpdated);

                        IList<ASBIESpec> newAsbieSpecs = CumulateAsbieSpecsFromComplexType(abieComplexType,
                                                                                           allElementDefinitions);

                        foreach (ASBIESpec newAsbieSpec in newAsbieSpecs)
                        {
                            updatedAbieSpec.AddASBIE(newAsbieSpec);
                        }

                        DocLibrary.UpdateElement(abieToBeUpdated, updatedAbieSpec);
                    }
                }
            }

            // Diagram auto-layout does not work .. remains for future purposes
            //var repository = (Repository)context.Repository;
            //foreach(Package model in repository.Models)
            //{
            //    foreach(Package package in model.Packages)
            //    {
            //        foreach(Diagram diagram in package.Diagrams)
            //        {
            //            repository.GetProjectInterface().LayoutDiagramEx(diagram.DiagramGUID, 0, 4, 20, 20, true);
            //        }
            //    }
            //}
        }

        ///<summary>
        ///</summary>
        ///<param name="abieComplexType">
        ///</param>
        ///<param name="allElementDefinitions">
        ///</param>
        ///<returns>
        ///</returns>
        public static IList<ASBIESpec> CumulateAsbieSpecsFromComplexType(ComplexType abieComplexType,
                                                                         IDictionary<string, string>
                                                                             allElementDefinitions)
        {
            IList<ASBIESpec> newAsbieSpecs = new List<ASBIESpec>();

            foreach (Element element in abieComplexType.Items)
            {
                if (!(element.Ref.Name.Equals("")))
                {
                    string associatedABIEName = allElementDefinitions[element.Ref.Name];
                    associatedABIEName = associatedABIEName.Substring(0, associatedABIEName.Length - 4);

                    IABIE associatedAbie = DocLibrary.ElementByName(associatedABIEName) ??
                                           BIESchemaImporter.getElementByName(associatedABIEName);

                    //Cross Reference to BIESchemaImporter!
                    string asbieName = (element.Ref.Name).Substring(0,
                                                                    (element.Ref.Name).Length -
                                                                    associatedABIEName.Length);

                    ASBIESpec asbieSpec = CumulateAsbieSpec(element, asbieName, associatedAbie, AsbieAggregationKind.Shared);

                    newAsbieSpecs.Add(asbieSpec);
                }
                else
                {
                    //TODO: add parameter which holds list of known prefixes (propblems may occur later on when additional prefixes are introduced e.g. codelist)
                    if (!element.Type.Prefix.Equals("bdt"))
                    {
                        string associatedAbieName = element.Type.Name;
                        associatedAbieName = associatedAbieName.Substring(0, associatedAbieName.Length - 4);

                        IABIE associatedAbie = DocLibrary.ElementByName(associatedAbieName) ??
                                               BIESchemaImporter.getElementByName(associatedAbieName);

                        string asbieName = element.Name.Substring(0, element.Name.Length - associatedAbieName.Length);
                        ASBIESpec asbieSpec = CumulateAsbieSpec(element, asbieName, associatedAbie,
                                                                AsbieAggregationKind.Composite);

                        newAsbieSpecs.Add(asbieSpec);
                    }
                }
            }

            return newAsbieSpecs;
        }

        ///<summary>
        /// The method creates an ASBIESpec based on an element, a Name, the ABIE it is associated to 
        /// and the type of aggregation. A corresponding ASBIESpec of the association between the element
        /// and it's associated ABIE is then returned through the return parameter. 
        ///</summary>
        ///<param name="element">
        /// The XSD element holding the ASBIE.
        ///</param>
        ///<param name="asbieName">
        /// The Name of the ASBIE to be created.
        ///</param>
        ///<param name="associatedAbie">
        /// The associated ABIE.
        ///</param>
        ///<param name="aggregationKind">
        /// The aggregation Type can be either composite or shared.
        ///</param>
        ///<returns>
        /// The method returns an ASBIESpec holding the association between 
        /// <paramref name="element"/> and <paramref name="associatedAbie"/>.
        ///</returns>
        public static ASBIESpec CumulateAsbieSpec(Element element, string asbieName, IABIE associatedAbie,
                                                  AsbieAggregationKind aggregationKind)
        {
            var asbieSpec = new ASBIESpec
                            {
                                AssociatedABIEId = associatedAbie.Id,
                                Name = asbieName,
                                LowerBound = ResolveMinOccurs(element.MinOccurs),
                                UpperBound = ResolveMaxOccurs(element.MaxOccurs),
                                AggregationKind = aggregationKind
                            };

            return asbieSpec;
        }

        ///<summary>
        /// The method creates an ABIESpec based on the complex type definition of an ABIE
        /// as it occurs in an XML schema. Therefore, the method requires on input parameter
        /// containing the complex type definition. A corresponding ABIESpec of the complex
        /// type definition is then returned through the return parameter. Realize that the 
        /// method does not process any ASBIE declarations. Therefore, only element declarations
        /// representing BBIEs within the ABIE are processed. To distinguish between ASBIEs and 
        /// BBIEs the prefix of the type in the element declaration is used. The prefix of the type
        /// used in BBIE element delcarations is assumed to be "bdt". Hence an element declaration
        /// for a BBIE must use the prefix "bdt" in order to be processed properly. Realize that for 
        /// processing ASBIEs within a complex type the method CumulateAsbieSpecsFromComplexType
        /// must be used. 
        ///</summary>
        ///<param name="abieComplexType">
        /// The input parameter contains the complex type declaration of the ABIE to be processed. An 
        /// example of a complex type definition is illustrated in the following.
        /// <code>
        ///   &lt;xsd:complexType name="AddressType"&gt;
        ///     &lt;xsd:sequence&gt;
        ///       &lt;xsd:element name="StreetName" type="bdt:TextStringType" minOccurs="0" maxOccurs="unbounded"/&gt;
        ///       &lt;xsd:element name="ZIP_PostcodeCode" type="bdt:CodeStringType" minOccurs="0" maxOccurs="unbounded"/&gt;
        ///       &lt;xsd:element ref="tns:Included_PersonPerson"/&gt;
        ///     &lt;/xsd:sequence&gt;
        ///   &lt;/xsd:complexType&gt;
        /// </code>
        ///</param>
        ///<returns>
        /// The method returns an ABIESpec for the complex type passed to the method through the 
        /// parameter <paramref name="abieComplexType"/>
        ///</returns>
        public static ABIESpec CumulateAbieSpecFromComplexType(ComplexType abieComplexType)
        {
            var singleAbieSpec = new ABIESpec();

            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

            singleAbieSpec.Name = abieName;

            var bbieSpecs = new List<BBIESpec>();

            foreach (object ctItem in abieComplexType.Items)
            {
                if (ctItem is Element)
                {
                    var element = (Element) ctItem;

                    if (element.Ref.Name.Equals(""))
                    {
                        if (element.Type.Prefix.Equals("bdt"))
                        {
                            var bbieSpec = new BBIESpec
                                           {
                                               Name = element.Name,
                                               LowerBound = ResolveMinOccurs(element.MinOccurs),
                                               UpperBound = ResolveMaxOccurs(element.MaxOccurs)
                                           };

                            string bdtName = element.Type.Name.Substring(0, element.Type.Name.Length - 4);

                            foreach (IBDT bdt in ExistingBdts.Elements)
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
            }
            singleAbieSpec.BBIEs = bbieSpecs;

            return singleAbieSpec;
        }

        ///<summary>
        /// The methods resolves the cardinality expressed in XML schema terms, such as 
        /// "1" and "unbounded", and returns a CCTS compliant representation of the
        /// cardinality. 
        ///</summary>
        ///<param name="maxOccurs">
        /// The input parameter specifys the cardinality to be resolved whereas the cardinality
        /// is specified in XML schema terms. Examples include "1" and "unbounded".
        ///</param>
        ///<returns>
        /// The method returns a CCTS compliant representation of the cardinality. In particular 
        /// the method returns "0" for "0", "1" for "1", "*" for "unbounded" and "1" for all other
        /// cardinalities including an empty cardinality. 
        ///</returns>
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

        ///<summary>
        /// The methods resolves the cardinality expressed in XML schema terms, such as 
        /// "1", and returns a CCTS compliant representation of the
        /// cardinality. 
        ///</summary>
        ///<param name="minOccurs">
        /// The input parameter specifys the cardinality to be resolved whereas the cardinality
        /// is specified in XML schema terms. Examples include "0" and "1".
        ///</param>
        ///<returns>
        /// The method returns a CCTS compliant representation of the cardinality. In particular 
        /// the method returns "0" for "0", "1" for "1", and "1" for all other
        /// cardinalities including an empty cardinality. 
        ///</returns>
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
    }
}