using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EARepository2 : EARepository
    {
// ReSharper disable MemberCanBePrivate.Global
        public const string ABCCode = "ABCCode";
        public const string ABCCodes = "ABCCodes";
        public const string Address = "Address";
        public const string BDTLibrary = "BDTLibrary";
        public const string BIELibrary = "BIELibrary";
        public const string Binary = "Binary";
        public const string BLibrary = "BLibrary";
        public const string Boolean = "Boolean";
        public const string CCLibrary = "CCLibrary";
        public const string CDTLibrary = "CDTLibrary";
        public const string Code = "Code";
        public const string Currency = "Currency";
        public const string Date = "Date";
        public const string Decimal = "Decimal";
        public const string DOCLibrary = "DOCLibrary";
        public const string ENUMLibrary = "ENUMLibrary";
        public const string Integer = "Integer";

        public const string Invoice = "Invoice";
        public const string InvoiceInfo = "InvoiceInfo";
        public const string Measure = "Measure";
        public const string Person = "Person";
        public const string PRIMLibrary = "PRIMLibrary";
        public const string SimpleString = "SimpleString";
        public const string String = "String";
        public const string Text = "Text";
// ReSharper restore MemberCanBePrivate.Global

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
                        BuildBdtLibrary(),
                        BuildCCLibrary(),
                        BuildBIELibrary(),
                        BuildDocLibrary()
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

        #region CDTLibrary

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
                CDTMeasure(),
                CDTCurrency()
                );
        }

        private static ElementBuilder CDTSimpleString()
        {
            return Element(SimpleString, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTCurrency()
        {
            return Element(Currency, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/Decimal),
                Attribute("CurrencyCode", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
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

        #region BDTLibrary

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
                BDTMeasure(),
                BDTCurrency()
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

        private static ElementBuilder BDTCurrency()
        {
            return Element(Currency, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) BLibrary/PRIMLibrary/Decimal),
                Attribute("CurrencyCode", Stereotype.SUP, (Path) BLibrary/PRIMLibrary/String)
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CDTLibrary/Currency)
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

        #region CCLibrary

        private static PackageBuilder BuildCCLibrary()
        {
            return Package(CCLibrary, Stereotype.CCLibrary)
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
            return Element(Address, Stereotype.ACC)
                .Attributes(
                Attribute("CountryName", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text),
                Attribute("CityName", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text),
                Attribute("StreetName", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text),
                Attribute("StreetNumber", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text),
                Attribute("Postcode", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text)
                )
                ;
        }

        private static ElementBuilder ACCPerson()
        {
            return Element(Person, Stereotype.ACC)
                .Attributes(
                Attribute("FirstName", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text),
                Attribute("LastName", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text),
                Attribute("NickName", Stereotype.BCC, (Path) BLibrary/CDTLibrary/Text).LowerBound("0").UpperBound("*")
                )
                .Connectors(
                Connector("homeAddress", Stereotype.ASCC, (Path) BLibrary/CCLibrary/Address).AggregationKind(
                    AggregationKind.Shared),
                Connector("workAddress", Stereotype.ASCC, (Path) BLibrary/CCLibrary/Address).AggregationKind(
                    AggregationKind.Composite).LowerBound("0").UpperBound(
                    "*")
                )
                ;
        }

        #endregion

        #region BIELibrary

        private static PackageBuilder BuildBIELibrary()
        {
            return Package(BIELibrary, Stereotype.BIELibrary)
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
            return Element(Address, Stereotype.ABIE)
                .Attributes(
                Attribute("CountryName", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text),
                Attribute("CityName", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text),
                Attribute("StreetName", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text),
                Attribute("StreetNumber", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text),
                Attribute("Postcode", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text)
                )
                .Connectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) BLibrary/CCLibrary/Address)
                );
        }

        private static ElementBuilder BIEPerson()
        {
            return Element(Person, Stereotype.ABIE)
                .Attributes(
                Attribute("FirstName", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text),
                Attribute("LastName", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text)
                )
                .Connectors(
                Connector("homeAddress", Stereotype.ASBIE, (Path) BLibrary/BIELibrary/Address).AggregationKind(
                    AggregationKind.Shared),
                Connector("workAddress", Stereotype.ASBIE, (Path) BLibrary/BIELibrary/Address).AggregationKind(
                    AggregationKind.Composite).LowerBound("0").
                    UpperBound("*")
                )
                ;
        }

        #endregion

        #region DOCLibrary

        private static PackageBuilder BuildDocLibrary()
        {
            return Package(DOCLibrary, Stereotype.DOCLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.BaseURN, "urn:test:blib1:bielib1")
                )
                .Elements(
                BIEInvoice(),
                BIEInvoiceInfo()
                );
        }

        private static ElementBuilder BIEInvoice()
        {
            return Element("Invoice", Stereotype.ABIE)
                .Attributes(
                Attribute("Amount", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Currency)
                )
                .Connectors(
                Connector("Info", Stereotype.ASBIE, (Path) BLibrary/DOCLibrary/InvoiceInfo).AggregationKind(
                    AggregationKind.Shared)
                );
        }

        private static ElementBuilder BIEInvoiceInfo()
        {
            return Element("InvoiceInfo", Stereotype.ABIE)
                .Attributes(
                Attribute("Info", Stereotype.BBIE, (Path) BLibrary/BDTLibrary/Text)
                )
                .Connectors(
                Connector("deliveryAddress", Stereotype.ASBIE, (Path) BLibrary/BIELibrary/Address).AggregationKind(
                    AggregationKind.Shared)
                );
        }

        #endregion
    }
}