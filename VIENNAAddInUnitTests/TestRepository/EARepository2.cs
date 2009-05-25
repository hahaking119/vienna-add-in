using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EARepository2 : EARepository
    {
        private Element abieAddress;
        private Element abieInvoice;
        private Element abieInvoiceInfo;
        private Element abiePerson;
        private Element accAddress;
        private Element accPerson;
        private Element bdtCode;
        private Element bdtCurrency;
        private Element bdtDate;
        private Element bdtMeasure;
        private Element bdtSimpleString;
        private Element bdtText;
        private Element cdtCode;
        private Element cdtCurrency;
        private Element cdtDate;
        private Element cdtMeasure;
        private Element cdtSimpleString;
        private Element cdtText;
        private Element enumABCCodes;
        private Element primBinary;
        private Element primBoolean;
        private Element primDate;
        private Element primDecimal;
        private Element primInteger;
        private Element primString;

        public EARepository2()
        {
            this.AddModel("test model",
                          m => m.AddPackage("BLibrary",
                                            bLibrary =>
                                            {
                                                bLibrary.Element.Stereotype = Stereotype.BLibrary;
                                                bLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1");
                                                bLibrary.AddPackage("PRIMLibrary", InitPRIMLibrary);
                                                bLibrary.AddPackage("ENUMLibrary", InitENUMLibrary);
                                                bLibrary.AddPackage("CDTLibrary", InitCDTLibrary);
                                                bLibrary.AddPackage("BDTLibrary", InitBDTLibrary);
                                                bLibrary.AddPackage("CCLibrary", InitCCLibrary);
                                                bLibrary.AddPackage("BIELibrary", InitBIELibrary);
                                                bLibrary.AddPackage("DOCLibrary", InitDOCLibrary);
                                            }));

            AddBasedOnDependency(bdtSimpleString, cdtSimpleString);
            AddBasedOnDependency(bdtCurrency, cdtCurrency);
            AddBasedOnDependency(bdtMeasure, cdtMeasure);
            AddBasedOnDependency(bdtCode, cdtCode);
            AddBasedOnDependency(bdtDate, cdtDate);
            AddBasedOnDependency(bdtText, cdtText);
            AddBasedOnDependency(abieAddress, accAddress);

            AddASCC(accPerson, accAddress, "homeAddress");
            AddASCC(accPerson, accAddress, "workAddress", "0", "*");

            AddASBIE(abiePerson, abieAddress, "homeAddress", EAAggregationKind.Shared);
            AddASBIE(abiePerson, abieAddress, "workAddress", EAAggregationKind.Composite, "0", "*");
            AddASBIE(abieInvoice, abieInvoiceInfo, "info", EAAggregationKind.Shared);
            AddASBIE(abieInvoiceInfo, abieAddress, "deliveryAddress", EAAggregationKind.Shared);
        }

        private void InitDOCLibrary(Package docLibrary)
        {
            docLibrary.Element.Stereotype = Stereotype.DOCLibrary;
            docLibrary.AddDiagram("DOCLibrary", "Class");
            docLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:doclibrary");
            abieInvoice = docLibrary.AddElement("Invoice", "Class").With(e =>
                                                                         {
                                                                             e.Stereotype = Stereotype.ABIE;
                                                                             AddAttribute(e, "Amount", bdtCurrency, Stereotype.BBIE);
                                                                         });
            abieInvoiceInfo = docLibrary.AddElement("InvoiceInfo", "Class").With(e =>
                                                                                 {
                                                                                     e.Stereotype = Stereotype.ABIE;
                                                                                     AddAttribute(e, "Info", bdtText, Stereotype.BBIE);
                                                                                 });
        }

        private void InitBIELibrary(Package bieLibrary)
        {
            bieLibrary.Element.Stereotype = Stereotype.BIELibrary;
            bieLibrary.AddDiagram("BIELibrary", "Class");
            bieLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bielib1");
            abieAddress = bieLibrary.AddElement("Address", "Class").With(e =>
                                                                         {
                                                                             e.Stereotype = Stereotype.ABIE;
                                                                             AddAttribute(e, "CountryName", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "CityName", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "StreetName", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "StreetNumber", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "Postcode", bdtText, Stereotype.BBIE);
                                                                         });
            abiePerson = bieLibrary.AddElement("Person", "Class").With(e =>
                                                                       {
                                                                           e.Stereotype = Stereotype.ABIE;
                                                                           AddAttribute(e, "FirstName", bdtText, Stereotype.BBIE);
                                                                           AddAttribute(e, "LastName", bdtText, Stereotype.BBIE);
                                                                       });
        }

        private void InitCCLibrary(Package ccLibrary)
        {
            ccLibrary.Element.Stereotype = Stereotype.CCLibrary;
            ccLibrary.AddDiagram("CCLibrary", "Class");
            ccLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cclib1");
            accAddress = ccLibrary.AddElement("Address", "Class").With(e =>
                                                                       {
                                                                           e.Stereotype = Stereotype.ACC;
                                                                           AddAttribute(e, "CountryName", cdtText, Stereotype.BCC);
                                                                           AddAttribute(e, "CityName", cdtText, Stereotype.BCC);
                                                                           AddAttribute(e, "StreetName", cdtText, Stereotype.BCC);
                                                                           AddAttribute(e, "StreetNumber", cdtText, Stereotype.BCC);
                                                                           AddAttribute(e, "Postcode", cdtText, Stereotype.BCC);
                                                                       });
            accPerson = ccLibrary.AddElement("Person", "Class").With(e =>
                                                                     {
                                                                         e.Stereotype = Stereotype.ACC;
                                                                         AddAttribute(e, "FirstName", cdtText, Stereotype.BCC);
                                                                         AddAttribute(e, "LastName", cdtText, Stereotype.BCC);
                                                                         AddAttribute(e, "NickName", cdtText, Stereotype.BCC, nickName =>
                                                                                                                              {
                                                                                                                                  nickName.LowerBound = "0";
                                                                                                                                  nickName.UpperBound = "*";
                                                                                                                              });
                                                                     });
        }

        private void InitBDTLibrary(Package bdtLibrary)
        {
            bdtLibrary.Element.Stereotype = Stereotype.BDTLibrary;
            bdtLibrary.AddDiagram("BDTLibrary", "Class");
            bdtLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bdtlib1");
            bdtSimpleString = bdtLibrary.AddElement("SimpleString", "Class").With(bdt =>
                                                                                  {
                                                                                      bdt.Stereotype = Stereotype.BDT;
                                                                                      AddCON(bdt, primString);
                                                                                  });
            bdtText = bdtLibrary.AddElement("Text", "Class").With(e =>
                                                                  {
                                                                      e.Stereotype = Stereotype.BDT;
                                                                      e.AddTaggedValue(TaggedValues.uniqueIdentifier.ToString()).WithValue("234235235");
                                                                      e.AddTaggedValue(TaggedValues.versionIdentifier.ToString()).WithValue("1.0");
                                                                      e.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("This is the definition of BDT Text.");
                                                                      e.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("business term 1|business term 2");
                                                                      AddCON(e, primString);
                                                                      AddSUPs(e, primString, "Language", "Language.Locale");
                                                                  });
            bdtDate = bdtLibrary.AddElement("Date", "Class").With(e =>
                                                                  {
                                                                      e.Stereotype = Stereotype.BDT;
                                                                      AddCON(e, primString);
                                                                      AddSUPs(e, primString, "Format");
                                                                  });
            bdtCode = bdtLibrary.AddElement("Code", "Class").With(e =>
                                                                  {
                                                                      e.Stereotype = Stereotype.BDT;
                                                                      AddCON(e, enumABCCodes);
                                                                      AddSUPs(e, primString,
                                                                              "Name",
                                                                              "CodeList.Agency",
                                                                              "CodeList.AgencyName",
                                                                              "CodeList",
                                                                              "CodeList.Name",
                                                                              "CodeList.UniformResourceIdentifier",
                                                                              "CodeList.Version",
                                                                              "CodeListScheme.UniformResourceIdentifier",
                                                                              "Language");
                                                                  });
            bdtMeasure = bdtLibrary.AddElement("Measure", "Class").With(e =>
                                                                        {
                                                                            e.Stereotype = Stereotype.BDT;
                                                                            AddCON(e, primDecimal);
                                                                            AddSUP(e, primString, "MeasureUnit");
                                                                            AddAttribute(e, "MeasureUnit.CodeListVersion", primString, Stereotype.SUP,
                                                                                         a =>
                                                                                         {
                                                                                             a.LowerBound = "1";
                                                                                             a.UpperBound = "*";
                                                                                         });
                                                                        });
            bdtCurrency = bdtLibrary.AddElement("Currency", "Class").With(e =>
                                                                          {
                                                                              e.Stereotype = Stereotype.BDT;
                                                                              AddCON(e, primDecimal);
                                                                              AddSUPs(e, primString, "CurrencyCode");
                                                                          });
        }

        private void InitCDTLibrary(Package cdtLibrary)
        {
            cdtLibrary.Element.Stereotype = Stereotype.CDTLibrary;
            cdtLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cdtlibrary");
            cdtLibrary.AddDiagram("CDTLibrary", "Class");
            cdtSimpleString = cdtLibrary.AddElement("SimpleString", "Class").With(cdt =>
                                                                                  {
                                                                                      cdt.Stereotype = Stereotype.CDT;
                                                                                      AddCON(cdt, primString);
                                                                                  });
            cdtText = cdtLibrary.AddElement("Text", "Class").With(cdt =>
                                                                  {
                                                                      cdt.Stereotype = Stereotype.CDT;
                                                                      AddCON(cdt, primString);
                                                                      AddSUPs(cdt, primString, "Language", "Language.Locale");
                                                                  });
            cdtDate = cdtLibrary.AddElement("Date", "Class").With(cdt =>
                                                                  {
                                                                      cdt.Stereotype = Stereotype.CDT;
                                                                      cdt.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("A Date.");
                                                                      AddCON(cdt, primString);
                                                                      AddSUPs(cdt, primString, "Format");
                                                                  });
            cdtCode = cdtLibrary.AddElement("Code", "Class").With(cdt =>
                                                                  {
                                                                      cdt.Stereotype = Stereotype.CDT;
                                                                      AddCON(cdt, primString);
                                                                      AddSUPs(cdt, primString,
                                                                              "Name",
                                                                              "CodeList.Agency",
                                                                              "CodeList.AgencyName",
                                                                              "CodeList",
                                                                              "CodeList.Name",
                                                                              "CodeList.UniformResourceIdentifier",
                                                                              "CodeList.Version",
                                                                              "CodeListScheme.UniformResourceIdentifier",
                                                                              "Language");
                                                                  });
            cdtMeasure = cdtLibrary.AddElement("Measure", "Class").With(cdt =>
                                                                        {
                                                                            cdt.Stereotype = Stereotype.CDT;
                                                                            AddCON(cdt, primDecimal);
                                                                            AddSUP(cdt, primString, "MeasureUnit");
                                                                            AddAttribute(cdt, "MeasureUnit.CodeListVersion", primString, Stereotype.SUP).With(
                                                                                a =>
                                                                                {
                                                                                    a.LowerBound = "1";
                                                                                    a.UpperBound = "*";
                                                                                });
                                                                        });
            cdtCurrency = cdtLibrary.AddElement("Currency", "Class").With(cdt =>
                                                                          {
                                                                              cdt.Stereotype = Stereotype.CDT;
                                                                              AddCON(cdt, primDecimal);
                                                                              AddSUPs(cdt, primString, "CurrencyCode");
                                                                          });
        }

        private void InitENUMLibrary(Package enumLibrary)
        {
            enumLibrary.Element.Stereotype = Stereotype.ENUMLibrary;
            enumLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:enumlibrary");
            enumLibrary.AddDiagram("ENUMLibrary", "Class");
            enumABCCodes = AddENUM(enumLibrary, "ABCCodes", primString,
                                   "ABC Code 1", "abc1",
                                   "ABC Code 2", "abc2",
                                   "ABC Code 3", "abc3"
                );
        }

        private void InitPRIMLibrary(Package primLibrary)
        {
            primLibrary.Element.Stereotype = Stereotype.PRIMLibrary;
            primLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:primlibrary");
            primLibrary.AddDiagram("PRIMLibrary", "Class");
            primString = AddPRIM(primLibrary, "String", "A sequence of characters in some suitable character set.");
            primDecimal = AddPRIM(primLibrary, "Decimal", "A subset of the real numbers, which can be represented by decimal numerals.");
            primBinary = AddPRIM(primLibrary, "Binary", "A set of (in)finite-length sequences of binary digits.");
            primBoolean = AddPRIM(primLibrary, "Boolean", "A logical expression consisting of predefined values.");
            primDate = AddPRIM(primLibrary, "Date", "A point in time to a common resolution (year, month, day, hour,...).");
            primInteger = AddPRIM(primLibrary, "Integer", "An element in the infinite set (...-2,-1,0,1,...).");
        }

        private static Element AddPRIM(Package primLibrary, string name, string definition)
        {
            return primLibrary.AddElement(name, "Class").With(e => { }).With(prim =>
                                                                             {
                                                                                 prim.Stereotype = Stereotype.PRIM;
                                                                                 prim.AddTaggedValue(TaggedValues.businessTerm.ToString()).WithValue(name);
                                                                                 prim.AddTaggedValue(TaggedValues.definition.ToString()).WithValue(definition);
                                                                                 prim.AddTaggedValue(TaggedValues.dictionaryEntryName.ToString()).WithValue(name);
                                                                             });
        }
    }
}