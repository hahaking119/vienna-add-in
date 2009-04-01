using System;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EARepository2 : EARepository
    {
        public static readonly string BLibrary = "BLibrary";
        public static readonly string PRIMLibrary = "PRIMLibrary";
        public static readonly string ENUMLibrary = "ENUMLibrary";
        public static readonly string CDTLibrary = "CDTLibrary";
        public static readonly string BDTLibrary = "BDTLibrary";

        public static readonly string String = "String";
        public static readonly string Decimal = "Decimal";
        public static readonly string Binary = "Binary";
        public static readonly string Boolean = "Boolean";
        public static readonly string Date = "Date";
        public static readonly string Integer = "Integer";

        public static readonly string SimpleString = "SimpleString";
        public static readonly string Measure = "Measure";
        public static readonly string ABCCode = "ABCCode";
        public static readonly string Code = "Code";
        public static readonly string Text = "Text";

        public static readonly string ABCCodes = "ABCCodes";

        public EARepository2()
        {
            SetContent(
                Package("test model", "")
                    .Packages(
                    Package(BLibrary, Stereotype.BLibrary)
                        .TaggedValues(
                        TaggedValue(TaggedValues.BaseURN, "urn:test:blib1")
                        )
                        .Packages(
                        BuildPrimLibrary(),
                        BuildEnumLibrary(),
                        BuildCdtLibrary(),
                        BuildBdtLibrary()
                        )
                    )
                );
        }

        #region PRIMLibrary

        private static PackageBuilder BuildPrimLibrary()
        {
            return Package(PRIMLibrary, Stereotype.PRIMLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:primlibrary")
                )
                .Elements(
                Element(String, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.BusinessTerm, "String"),
                    TaggedValue(TaggedValues.Definition, "A sequence of characters in some suitable character set."),
                    TaggedValue(TaggedValues.DictionaryEntryName, "String")
                    ),
                Element(Decimal, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.BusinessTerm, "Decimal"),
                    TaggedValue(TaggedValues.Definition,
                                "A subset of the real numbers, which can be represented by decimal numerals."),
                    TaggedValue(TaggedValues.DictionaryEntryName, "Decimal")
                    ),
                Element(Binary, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.BusinessTerm, "Binary"),
                    TaggedValue(TaggedValues.Definition, "A set of (in)finite-length sequences of binary digits."),
                    TaggedValue(TaggedValues.DictionaryEntryName, "Binary")
                    ),
                Element(Boolean, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.BusinessTerm, "Boolean"),
                    TaggedValue(TaggedValues.Definition, "A logical expression consisting of predefined values."),
                    TaggedValue(TaggedValues.DictionaryEntryName, "Boolean")
                    ),
                Element(Date, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.BusinessTerm, "Date"),
                    TaggedValue(TaggedValues.Definition,
                                "A point in time to a common resolution (year, month, day, hour,...)."),
                    TaggedValue(TaggedValues.DictionaryEntryName, "Date")
                    ),
                Element(Integer, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.BusinessTerm, "Integer"),
                    TaggedValue(TaggedValues.Definition, "An element in the infinite set (...-2,-1,0,1,...)."),
                    TaggedValue(TaggedValues.DictionaryEntryName, "Integer")
                    )
                );
        }

        #endregion

        #region ENUMLibrary

        private static PackageBuilder BuildEnumLibrary()
        {
            return Package(ENUMLibrary, Stereotype.ENUMLibrary)
                .Elements(
                Element(ABCCodes, Stereotype.ENUM)
                    .Attributes(
                    Attribute("ABC Code 1", "", (Path) BLibrary/PRIMLibrary/String).DefaultValue("abc1"),
                    Attribute("ABC Code 2", "", (Path) BLibrary/PRIMLibrary/String).DefaultValue("abc2"),
                    Attribute("ABC Code 3", "", (Path) BLibrary/PRIMLibrary/String).DefaultValue("abc3")
                    )
                );
        }

        #endregion

        #region CDTLib1

        private static PackageBuilder BuildCdtLibrary()
        {
            return Package(CDTLibrary, Stereotype.CDTLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:cdtlib1")
                )
                .Elements(
                CDTSimpleString(),
                CDTText(),
                CDTDate(),
                CDTCode(),
                CDTMeasure()
                );
        }

        private static ElementBuilder CDTSimpleString()
        {
            return Element(SimpleString, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTMeasure()
        {
            return Element(Measure, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/Decimal),
                Attribute("MeasureUnit", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("MeasureUnit.CodeListVersion", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String).LowerBound(
                    "1").UpperBound("*")
                );
        }

        private static ElementBuilder CDTCode()
        {
            return Element(Code, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Name", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Agency", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.AgencyName", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Name", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Version", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTDate()
        {
            return Element(Date, Stereotype.CDT)
                .TaggedValues(
                TaggedValue(TaggedValues.Definition, "A Date."))
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Format", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTText()
        {
            return Element(Text, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Language.Locale", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
                );
        }

        #endregion

        #region BDTLib1

        private static PackageBuilder BuildBdtLibrary()
        {
            return Package(BDTLibrary, Stereotype.BDTLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bdtlib1")
                )
                .Elements(
                BDTSimpleString(),
                BDTText(),
                BDTDate(),
                BDTCode(),
                BDTMeasure()
                );
        }

        private static ElementBuilder BDTSimpleString()
        {
            return Element(SimpleString, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String)
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CDTLibrary/SimpleString)
                );
        }

        private static ElementBuilder BDTMeasure()
        {
            return Element(Measure, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/Decimal),
                Attribute("MeasureUnit", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("MeasureUnit.CodeListVersion", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String).LowerBound(
                    "1").UpperBound("*")
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CDTLibrary/Measure)
                );
        }

        private static ElementBuilder BDTCode()
        {
            return Element(ABCCode, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/ENUMLibrary/ABCCodes),
                Attribute("Name", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Agency", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.AgencyName", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Name", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Version", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String))
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CDTLibrary/Code)
                );
        }

        private static ElementBuilder BDTDate()
        {
            return Element(Date, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Format", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CDTLibrary/Date)
                );
        }

        private static ElementBuilder BDTText()
        {
            return Element(Text, Stereotype.BDT)
                .TaggedValues(
                TaggedValue(TaggedValues.UniqueIdentifier, "234235235"),
                TaggedValue(TaggedValues.VersionIdentifier, "1.0"),
                TaggedValue(TaggedValues.Definition, "This is the definition of BDT Text."),
                TaggedValue(TaggedValues.BusinessTerm, "business term 1|business term 2")
                ).Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String),
                Attribute("Language.Locale", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CDTLibrary/Text)
                );
        }

        #endregion
    }
}