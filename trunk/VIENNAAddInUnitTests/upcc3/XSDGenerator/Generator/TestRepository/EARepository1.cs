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
                        .TaggedValues(
                        TaggedValue(TaggedValues.BaseURN, "urn:test:blib1")
                        )
                        .Packages(
                        PRIMLib1(),
                        CDTLib1(),
                        BDTLib1(),
                        CCLib1(),
                        BIELib1()
                        )
                    )
                );
        }

        #region PRIMLib1

        private static PackageBuilder PRIMLib1()
        {
            return Package("primlib1", Stereotype.PRIMLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:primlib1")
                )
                .Elements(
                Element("String", Stereotype.PRIM),
                Element("Decimal", Stereotype.PRIM)
                );
        }

        #endregion

        #region CDTLib1

        private static PackageBuilder CDTLib1()
        {
            return Package("cdtlib1", Stereotype.CDTLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:cdtlib1")
                )
                .Elements(
                CDTText(),
                CDTDate(),
                CDTCode(),
                CDTMeasure()
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
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bdtlib1")
                )
                .Elements(
                BDTText(),
                BDTDate(),
                BDTCode(),
                BDTMeasure()
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
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:cclib1")
                )
                .Elements(
                ACCAddress(),
                ACCPerson()
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
                Attribute("Postcode", Stereotype.BCC, PathToText())
                )
                ;
        }

        private static ElementBuilder ACCPerson()
        {
            return Element("Person", Stereotype.ACC)
                .Attributes(
                Attribute("FirstName", Stereotype.BCC, PathToText()),
                Attribute("LastName", Stereotype.BCC, PathToText())
                )
                .Connectors(
                Connector("homeAddress", Stereotype.ASCC, PathToAddress())
                )
                ;
        }

        #endregion

        #region BIELib1

        private static PackageBuilder BIELib1()
        {
            return Package("bielib1", Stereotype.BIELibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bielib1")
                )
                .Elements(
                BIEAddress(),
                BIEPerson()
                );
        }

        private static ElementBuilder BIEAddress()
        {
            return Element("MyAddress", Stereotype.ABIE)
                .Attributes(
                Attribute("CountryName", Stereotype.BCC, PathToBDTText()),
                Attribute("CityName", Stereotype.BCC, PathToBDTText()),
                Attribute("StreetName", Stereotype.BCC, PathToBDTText()),
                Attribute("StreetNumber", Stereotype.BCC, PathToBDTText()),
                Attribute("Postcode", Stereotype.BCC, PathToBDTText())
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, PathToAddress()),
                Connector("basedOn", Stereotype.ASBIE, PathToAddress())
                );
        }

        private static ElementBuilder BIEPerson()
        {
            return Element("MyPerson", Stereotype.ABIE)
                .Attributes(
                Attribute("FirstName", Stereotype.BCC, PathToBDTText()),
                Attribute("LastName", Stereotype.BCC, PathToBDTText())
                )
                .Connectors(
                Connector("homeAddress", Stereotype.ASBIE, PathToBIEAddress())
                )
                ;
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

        #endregion
    }
}