using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.TestRepository
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
        public const string TestModel = "test model";
        public const string Text = "Text";
// ReSharper restore MemberCanBePrivate.Global

        public EARepository2()
        {
            SetContent(
                Package(TestModel, "")
                    .Packages(
                    Package(BLibrary, Stereotype.BLibrary)
                        .TaggedValues(
                        TaggedValue(TaggedValues.baseURN, "urn:test:blib1")
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
            SetConnectors(
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BDTLibrary/SimpleString,
                          (Path) TestModel/BLibrary/CDTLibrary/SimpleString),
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BDTLibrary/Currency,
                          (Path) TestModel/BLibrary/CDTLibrary/Currency),
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BDTLibrary/Measure,
                          (Path) TestModel/BLibrary/CDTLibrary/Measure),
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BDTLibrary/Code,
                          (Path) TestModel/BLibrary/CDTLibrary/Code),
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BDTLibrary/Date,
                          (Path) TestModel/BLibrary/CDTLibrary/Date),
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BDTLibrary/Text,
                          (Path) TestModel/BLibrary/CDTLibrary/Text),
                Connector("homeAddress", Stereotype.ASCC, (Path) TestModel/BLibrary/CCLibrary/Person,
                          (Path) TestModel/BLibrary/CCLibrary/Address).AggregationKind(AggregationKind.Shared),
                Connector("workAddress", Stereotype.ASCC, (Path) TestModel/BLibrary/CCLibrary/Person,
                          (Path) TestModel/BLibrary/CCLibrary/Address).AggregationKind(AggregationKind.Composite).
                    LowerBound("0").UpperBound(
                    "*"),
                Connector("basedOn", Stereotype.BasedOn, (Path) TestModel/BLibrary/BIELibrary/Address,
                          (Path) TestModel/BLibrary/CCLibrary/Address),
                Connector("homeAddress", Stereotype.ASBIE, (Path) TestModel/BLibrary/BIELibrary/Person,
                          (Path) TestModel/BLibrary/BIELibrary/Address).AggregationKind(AggregationKind.Shared),
                Connector("workAddress", Stereotype.ASBIE, (Path) TestModel/BLibrary/BIELibrary/Person,
                          (Path) TestModel/BLibrary/BIELibrary/Address).AggregationKind(AggregationKind.Composite).
                    LowerBound("0").UpperBound("*"),
                Connector("Info", Stereotype.ASBIE, (Path) TestModel/BLibrary/DOCLibrary/Invoice,
                          (Path) TestModel/BLibrary/DOCLibrary/InvoiceInfo).AggregationKind(AggregationKind.Shared),
                Connector("deliveryAddress", Stereotype.ASBIE, (Path) TestModel/BLibrary/DOCLibrary/InvoiceInfo,
                          (Path) TestModel/BLibrary/BIELibrary/Address).AggregationKind(AggregationKind.Shared)
                );
        }

        #region PRIMLibrary

        private static PackageBuilder BuildPrimLibrary()
        {
            return Package(PRIMLibrary, Stereotype.PRIMLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.baseURN, "urn:test:blib1:primlibrary")
                )
                .Elements(
                Element(String, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.businessTerm, "String"),
                    TaggedValue(TaggedValues.definition, "A sequence of characters in some suitable character set."),
                    TaggedValue(TaggedValues.dictionaryEntryName, "String")
                    ),
                Element(Decimal, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.businessTerm, "Decimal"),
                    TaggedValue(TaggedValues.definition,
                                "A subset of the real numbers, which can be represented by decimal numerals."),
                    TaggedValue(TaggedValues.dictionaryEntryName, "Decimal")
                    ),
                Element(Binary, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.businessTerm, "Binary"),
                    TaggedValue(TaggedValues.definition, "A set of (in)finite-length sequences of binary digits."),
                    TaggedValue(TaggedValues.dictionaryEntryName, "Binary")
                    ),
                Element(Boolean, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.businessTerm, "Boolean"),
                    TaggedValue(TaggedValues.definition, "A logical expression consisting of predefined values."),
                    TaggedValue(TaggedValues.dictionaryEntryName, "Boolean")
                    ),
                Element(Date, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.businessTerm, "Date"),
                    TaggedValue(TaggedValues.definition,
                                "A point in time to a common resolution (year, month, day, hour,...)."),
                    TaggedValue(TaggedValues.dictionaryEntryName, "Date")
                    ),
                Element(Integer, Stereotype.PRIM)
                    .TaggedValues(
                    TaggedValue(TaggedValues.businessTerm, "Integer"),
                    TaggedValue(TaggedValues.definition, "An element in the infinite set (...-2,-1,0,1,...)."),
                    TaggedValue(TaggedValues.dictionaryEntryName, "Integer")
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
                    Attribute("ABC Code 1", "", (Path) TestModel/BLibrary/PRIMLibrary/String).DefaultValue("abc1"),
                    Attribute("ABC Code 2", "", (Path) TestModel/BLibrary/PRIMLibrary/String).DefaultValue("abc2"),
                    Attribute("ABC Code 3", "", (Path) TestModel/BLibrary/PRIMLibrary/String).DefaultValue("abc3")
                    )
                );
        }

        #endregion

        #region CDTLibrary

        private static PackageBuilder BuildCdtLibrary()
        {
            return Package(CDTLibrary, Stereotype.CDTLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.baseURN, "urn:test:blib1:cdtlib1")
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
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTCurrency()
        {
            return Element(Currency, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/Decimal),
                Attribute("CurrencyCode", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTMeasure()
        {
            return Element(Measure, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/Decimal),
                Attribute("MeasureUnit", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("MeasureUnit.CodeListVersion", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String).
                    LowerBound(
                    "1").UpperBound("*")
                );
        }

        private static ElementBuilder CDTCode()
        {
            return Element(Code, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Name", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Agency", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.AgencyName", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Name", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP,
                          (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Version", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP,
                          (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTDate()
        {
            return Element(Date, Stereotype.CDT)
                .TaggedValues(
                TaggedValue(TaggedValues.definition, "A Date."))
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Format", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder CDTText()
        {
            return Element(Text, Stereotype.CDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Language.Locale", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        #endregion

        #region BDTLibrary

        private static PackageBuilder BuildBdtLibrary()
        {
            return Package(BDTLibrary, Stereotype.BDTLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.baseURN, "urn:test:blib1:bdtlib1")
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
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder BDTCurrency()
        {
            return Element(Currency, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/Decimal),
                Attribute("CurrencyCode", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder BDTMeasure()
        {
            return Element(Measure, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/Decimal),
                Attribute("MeasureUnit", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("MeasureUnit.CodeListVersion", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String).
                    LowerBound(
                    "1").UpperBound("*")
                );
        }

        private static ElementBuilder BDTCode()
        {
            return Element(Code, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/ENUMLibrary/ABCCodes),
                Attribute("Name", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Agency", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.AgencyName", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Name", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.UniformResourceIdentifier", Stereotype.SUP,
                          (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeList.Version", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("CodeListScheme.UniformResourceIdentifier", Stereotype.SUP,
                          (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder BDTDate()
        {
            return Element(Date, Stereotype.BDT)
                .Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Format", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        private static ElementBuilder BDTText()
        {
            return Element(Text, Stereotype.BDT)
                .TaggedValues(
                TaggedValue(TaggedValues.uniqueIdentifier, "234235235"),
                TaggedValue(TaggedValues.versionIdentifier, "1.0"),
                TaggedValue(TaggedValues.definition, "This is the definition of BDT Text."),
                TaggedValue(TaggedValues.businessTerm, "business term 1|business term 2")
                ).Attributes(
                Attribute("Content", Stereotype.CON, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Language", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String),
                Attribute("Language.Locale", Stereotype.SUP, (Path) TestModel/BLibrary/PRIMLibrary/String)
                );
        }

        #endregion

        #region CCLibrary

        private static PackageBuilder BuildCCLibrary()
        {
            return Package(CCLibrary, Stereotype.CCLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.baseURN, "urn:test:blib1:cclib1")
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
                Attribute("CountryName", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text),
                Attribute("CityName", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text),
                Attribute("StreetName", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text),
                Attribute("StreetNumber", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text),
                Attribute("Postcode", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text)
                )
                ;
        }

        private static ElementBuilder ACCPerson()
        {
            return Element(Person, Stereotype.ACC)
                .Attributes(
                Attribute("FirstName", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text),
                Attribute("LastName", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text),
                Attribute("NickName", Stereotype.BCC, (Path) TestModel/BLibrary/CDTLibrary/Text).LowerBound("0").
                    UpperBound("*")
                );
        }

        #endregion

        #region BIELibrary

        private static PackageBuilder BuildBIELibrary()
        {
            return Package(BIELibrary, Stereotype.BIELibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.baseURN, "urn:test:blib1:bielib1")
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
                Attribute("CountryName", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text),
                Attribute("CityName", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text),
                Attribute("StreetName", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text),
                Attribute("StreetNumber", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text),
                Attribute("Postcode", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text)
                );
        }

        private static ElementBuilder BIEPerson()
        {
            return Element(Person, Stereotype.ABIE)
                .Attributes(
                Attribute("FirstName", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text),
                Attribute("LastName", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text)
                );
        }

        #endregion

        #region DOCLibrary

        private static PackageBuilder BuildDocLibrary()
        {
            return Package(DOCLibrary, Stereotype.DOCLibrary)
                .TaggedValues(
                TaggedValue(TaggedValues.baseURN, "urn:test:blib1:bielib1")
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
                Attribute("Amount", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Currency)
                );
        }

        private static ElementBuilder BIEInvoiceInfo()
        {
            return Element("InvoiceInfo", Stereotype.ABIE)
                .Attributes(
                Attribute("Info", Stereotype.BBIE, (Path) TestModel/BLibrary/BDTLibrary/Text)
                );
        }

        #endregion
    }
}