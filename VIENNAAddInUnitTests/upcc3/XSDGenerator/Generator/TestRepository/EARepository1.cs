// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EARepository1 : EARepository
    {
        public EARepository1()
        {
            SetContent(
                Package("test model", "")
                    .Packages(
                    Package("blib1", Stereotype.BLibrary)
                        .Diagrams(
                        Diagram("blib1", "Package")
                        )
                        .TaggedValues(
                        TaggedValue(TaggedValues.BaseURN, "urn:test:blib1")
                        )
                        .Packages(
                        PRIMLib1(),
                        ENUMLib1(),
                        CDTLib1(),
                        BDTLib1(),
                        CCLib1(),
                        BIELib1(),
                        DOCLibrary()
                        ),
                    Package("Business Information View", Stereotype.BInformationV)
                        .Packages(
                        Package("A bLibrary nested in a bInformationV", Stereotype.BLibrary)
                            .Packages(
                            Package("Another PRIMLibrary", Stereotype.PRIMLibrary)
                            )
                        ),
                    Package("A package with an arbitrary stereotype", "Some arbitrary stereotype")
                        .Packages(
                        Package("This bLibrary should _not_ be found because it's in the wrong location in the package hierarchy", Stereotype.BLibrary)
                        )
                    )
                );
        }

        private static PackageBuilder ENUMLib1()
        {
            return Package("enumlib1", Stereotype.ENUMLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:enumlib1")
                )
                .Elements(
                Element("ABC_Codes", Stereotype.ENUM)
                    .Attributes(
                    Attribute("ABC Code 1", "", PathToString()).DefaultValue("abc1"),
                    Attribute("ABC Code 2", "", PathToString()).DefaultValue("abc2")
                    ),
                InvalidElement()
                );
        }

        private static ElementBuilder InvalidElement()
        {
            return Element("InvalidElement", "InvalidStereotype");
        }

        #region PRIMLib1

        private static PackageBuilder PRIMLib1()
        {
            return Package("primlib1", Stereotype.PRIMLibrary)
                .Diagrams(
                Diagram("primlib1", "Class")
                )
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:primlib1")
                )
                .Elements(
                Element("String", Stereotype.PRIM),
                Element("Decimal", Stereotype.PRIM),
                InvalidElement()
                );
        }

        #endregion

        #region CDTLib1

        private static PackageBuilder CDTLib1()
        {
            return Package("cdtlib1", Stereotype.CDTLibrary)
                .Diagrams(
                Diagram("cdtlib1", "Class")
                )
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:cdtlib1")
                )
                .Elements(
                CDTText(),
                CDTDate(),
                CDTCode(),
                CDTMeasure(),
                InvalidElement()
                );
        }

        private static ElementBuilder CDTMeasure()
        {
            return Element("Measure", Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToDecimal()),
                Attribute("MeasureUnit", Stereotype.SUP, PathToString()),
                Attribute("MeasureUnit.CodeListVersion", Stereotype.SUP, PathToString()).LowerBound("1").UpperBound("*")
                );
        }

        private static ElementBuilder CDTCode()
        {
            return Element("Code", Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToString()),
                Attribute("Name", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Agency", Stereotype.SUP, PathToString()),
                Attribute("CodeList.AgencyName", Stereotype.SUP, PathToString()),
                Attribute("CodeList", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Name", Stereotype.SUP, PathToString()),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Version", Stereotype.SUP, PathToString()),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP, PathToString()),
                Attribute("Language", Stereotype.SUP, PathToString())
                );
        }

        private static ElementBuilder CDTDate()
        {
            return Element("Date", Stereotype.CDT)
                .TaggedValues(
                TaggedValue(TaggedValues.Definition, "A Date."))
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToString()),
                Attribute("Format", Stereotype.SUP, PathToString())
                );
        }

        private static ElementBuilder CDTText()
        {
            return Element("Text", Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToString()),
                Attribute("Language", Stereotype.SUP, PathToString()),
                Attribute("Language.Locale", Stereotype.SUP, PathToString())
                );
        }

        #endregion

        #region BDTLib1

        private static PackageBuilder BDTLib1()
        {
            return Package("bdtlib1", Stereotype.BDTLibrary)
                .Diagrams(
                Diagram("bdtlib1", "Class")
                )
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bdtlib1")
                )
                .Elements(
                BDTText(),
                BDTDate(),
                BDTCode(),
                BdtAbcCode(),
                BDTMeasure(),
                InvalidElement()
                );
        }

        private static ElementBuilder BDTMeasure()
        {
            return Element("Measure", Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToDecimal()),
                Attribute("MeasureUnit", Stereotype.SUP, PathToString()),
                Attribute("MeasureUnit.CodeListVersion", Stereotype.SUP, PathToString()).LowerBound("1").UpperBound("*")
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToMeasure())
                );
        }

        private static ElementBuilder BDTCode()
        {
            return Element("Code", Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToString()),
                Attribute("Name", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Agency", Stereotype.SUP, PathToString()),
                Attribute("CodeList.AgencyName", Stereotype.SUP, PathToString()),
                Attribute("CodeList", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Name", Stereotype.SUP, PathToString()),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Version", Stereotype.SUP, PathToString()),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP, PathToString()),
                Attribute("Language", Stereotype.SUP, PathToString()))
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToCode())
                );
        }

        private static ElementBuilder BdtAbcCode()
        {
            return Element("ABC_Code", Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToEnumAbcCodes()),
                Attribute("Name", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Agency", Stereotype.SUP, PathToString()),
                Attribute("CodeList.AgencyName", Stereotype.SUP, PathToString()),
                Attribute("CodeList", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Name", Stereotype.SUP, PathToString()),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP, PathToString()),
                Attribute("CodeList.Version", Stereotype.SUP, PathToString()),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP, PathToString()),
                Attribute("Language", Stereotype.SUP, PathToString()))
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToCode())
                );
        }

        private static ElementBuilder BDTDate()
        {
            return Element("Date", Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToString()),
                Attribute("Format", Stereotype.SUP, PathToString())
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToDate())
                );
        }

        private static ElementBuilder BDTText()
        {
            return Element("Text", Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, PathToString())
                    .TaggedValues(TaggedValue(TaggedValues.Definition, "This is the definition of BDT Text.")),
                Attribute("Language", Stereotype.SUP, PathToString()),
                Attribute("Language.Locale", Stereotype.SUP, PathToString())
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToText())
                );
        }

        #endregion

        #region CCLib1

        private static PackageBuilder CCLib1()
        {
            return Package("cclib1", Stereotype.CCLibrary)
                .Diagrams(
                Diagram("cclib1", "Class")
                )
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:cclib1")
                )
                .Elements(
                ACCAddress(),
                ACCPerson(),
                InvalidElement()
                );
        }

        private static ElementBuilder ACCAddress()
        {
            return Element("Address", Stereotype.ACC)
                .Attributes(
                Attribute("CountryName", Stereotype.BCC, PathToText()),
                Attribute("CityName", Stereotype.BCC, PathToText()),
                Attribute("StreetName", Stereotype.BCC, PathToText()),
                Attribute("StreetNumber", Stereotype.BCC, PathToText()),
                Attribute("Postcode", Stereotype.BCC, PathToText()).LowerBound("0").UpperBound("*")
                )
                ;
        }

        private static ElementBuilder ACCPerson()
        {
            return Element("Person", Stereotype.ACC)
                .Attributes(
                Attribute("FirstName", Stereotype.BCC, PathToText()),
                Attribute("LastName", Stereotype.BCC, PathToText()),
                Attribute("NickName", Stereotype.BCC, PathToText()).LowerBound("0").UpperBound("*")
                )
                .Connectors(
                Connector("homeAddress", Stereotype.ASCC, PathToAddress()).AggregationKind(AggregationKind.Shared),
                Connector("workAddress", Stereotype.ASCC, PathToAddress()).AggregationKind(AggregationKind.Shared).
                    LowerBound("0").UpperBound("*")
                )
                ;
        }

        #endregion

        #region BIELib1

        private static PackageBuilder BIELib1()
        {
            return Package("bielib1", Stereotype.BIELibrary)
                .Diagrams(
                Diagram("bielib1", "Class")
                )
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bielib1")
                )
                .Elements(
                BIEAddress(),
                BIEPerson(),
                InvalidElement()
                );
        }

        private static ElementBuilder BIEAddress()
        {
            return Element("MyAddress", Stereotype.ABIE)
                .Attributes(
                Attribute("CountryName", Stereotype.BBIE, PathToBDTText()),
                Attribute("CityName", Stereotype.BBIE, PathToBDTText()),
                Attribute("StreetName", Stereotype.BBIE, PathToBDTText()),
                Attribute("StreetNumber", Stereotype.BBIE, PathToBDTText()),
                Attribute("Postcode", Stereotype.BBIE, PathToBDTText()).LowerBound("0").UpperBound("*")
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToAddress())
                );
        }

        private static ElementBuilder BIEPerson()
        {
            return Element("MyPerson", Stereotype.ABIE)
                .Attributes(
                Attribute("FirstName", Stereotype.BBIE, PathToBDTText()),
                Attribute("LastName", Stereotype.BBIE, PathToBDTText())
                )
                .Connectors(
                Connector("homeAddress", Stereotype.ASBIE, PathToBIEAddress()).AggregationKind(AggregationKind.Shared),
                Connector("workAddress", Stereotype.ASBIE, PathToBIEAddress()).AggregationKind(AggregationKind.Composite)
                    .LowerBound("0").UpperBound("*")
                )
                ;
        }

        #endregion

        #region DOCLibrary

        private static PackageBuilder DOCLibrary()
        {
            return Package("DOCLibrary", Stereotype.DOCLibrary)
                .Diagrams(
                Diagram("DOCLibrary", "Class")
                )
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bielib1")
                )
                .Elements(
                Invoice(),
                InvoiceInfo(),
                InvalidElement()
                );
        }

        private static ElementBuilder Invoice()
        {
            return Element("Invoice", Stereotype.ABIE)
                .Attributes(
                Attribute("Amount", Stereotype.BBIE, PathToBDTText())
                )
                .Connectors(
                Connector("info", Stereotype.ASBIE, PathToInvoiceInfo()).AggregationKind(AggregationKind.Shared)
                );
        }

        private static ElementBuilder InvoiceInfo()
        {
            return Element("InvoiceInfo", Stereotype.ABIE)
                .Attributes(
                Attribute("Info", Stereotype.BBIE, PathToBDTText())
                )
                .Connectors(
                Connector("deliveryAddress", Stereotype.ASBIE, PathToBIEAddress()).AggregationKind(
                    AggregationKind.Shared)
                );
        }

        #endregion

        #region Paths

        private static Path PathToMeasure()
        {
            return (Path) "blib1"/"cdtlib1"/"Measure";
        }

        public static Path PathToDate()
        {
            return (Path) "blib1"/"cdtlib1"/"Date";
        }

        public static Path PathToCode()
        {
            return (Path) "blib1"/"cdtlib1"/"Code";
        }

        public static Path PathToText()
        {
            return (Path) "blib1"/"cdtlib1"/"Text";
        }

        public static Path PathToBDTText()
        {
            return (Path) "blib1"/"bdtlib1"/"Text";
        }

        public static Path PathToBDTCode()
        {
            return (Path) "blib1"/"bdtlib1"/"Code";
        }

        public static Path PathToBdtAbcCode()
        {
            return (Path) "blib1"/"bdtlib1"/"ABC_Code";
        }

        public static Path PathToEnumAbcCodes()
        {
            return (Path) "blib1"/"enumlib1"/"ABC_Codes";
        }

        private static Path PathToDecimal()
        {
            return (Path) "blib1"/"primlib1"/"Decimal";
        }

        public static Path PathToString()
        {
            return (Path) "blib1"/"primlib1"/"String";
        }

        public static Path PathToAddress()
        {
            return (Path) "blib1"/"cclib1"/"Address";
        }

        public static Path PathToACCPerson()
        {
            return (Path) "blib1"/"cclib1"/"Person";
        }

        public static Path PathToBIEAddress()
        {
            return (Path) "blib1"/"bielib1"/"MyAddress";
        }

        public static Path PathToBIEPerson()
        {
            return (Path) "blib1"/"bielib1"/"MyPerson";
        }

        public static Path PathToInvoice()
        {
            return (Path) "blib1"/"DOCLibrary"/"Invoice";
        }

        public static Path PathToInvoiceInfo()
        {
            return (Path) "blib1"/"DOCLibrary"/"InvoiceInfo";
        }

        #endregion
    }
}