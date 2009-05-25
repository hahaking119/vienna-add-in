// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EARepository1 : EARepository
    {
        private Element accAddress;
        private Element accPerson;
        private Element bdtABCCode;
        private Element bdtCode;
        private Element bdtDate;
        private Element bdtMeasure;
        private Element bdtText;
        private Element bieInvoice;
        private Element bieInvoiceInfo;
        private Element bieMyAddress;
        private Element bieMyPerson;
        private Element cdtCode;
        private Element cdtDate;
        private Element cdtMeasure;
        private Element cdtText;
        private Element primDecimal;
        private Element primString;

        public EARepository1()
        {
            this.AddModel("test model", InitTestModel);

            AddBasedOnDependency(bdtMeasure, cdtMeasure);
            AddBasedOnDependency(bdtCode, cdtCode);
            AddBasedOnDependency(bdtABCCode, cdtCode);
            AddBasedOnDependency(bdtDate, cdtDate);
            AddBasedOnDependency(bdtText, cdtText);
            AddBasedOnDependency(bieMyAddress, accAddress);

            AddASCC(accPerson, accAddress, "homeAddress");
            AddASCC(accPerson, accAddress, "workAddress", "0", "*");

            AddASBIE(bieMyPerson, bieMyAddress, "homeAddress", EAAggregationKind.Shared);
            AddASBIE(bieMyPerson, bieMyAddress, "workAddress", EAAggregationKind.Composite, "0", "*");
            AddASBIE(bieInvoice, bieInvoiceInfo, "info", EAAggregationKind.Shared);
            AddASBIE(bieInvoiceInfo, bieMyAddress, "deliveryAddress", EAAggregationKind.Shared);
        }

        private void InitTestModel(Package m)
        {
            m.AddPackage("blib1", InitBLib1);
            m.AddPackage("Business Information View", InitBusinessInformationView);
            m.AddPackage("A package with an arbitrary stereotype",
                         p =>
                         {
                             p.Element.Stereotype = "Some arbitrary stereotype";
                             p.AddPackage("This bLibrary should _not_ be found because it's in the wrong location in the package hierarchy",
                                          p1 => { p1.Element.Stereotype = Stereotype.BLibrary; });
                         });
        }

        private static void InitBusinessInformationView(Package p)
        {
            p.Element.Stereotype = Stereotype.BInformationV;
            p.AddPackage(
                "A bLibrary nested in a bInformationV",
                bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.BLibrary;
                    bLibrary.AddPackage("Another PRIMLibrary",
                                        primLibrary =>
                                        {
                                            primLibrary.Element.Stereotype =
                                                Stereotype.PRIMLibrary;
                                        });
                });
        }

        private void InitBLib1(Package bLib1)
        {
            bLib1.Element.Stereotype = Stereotype.BLibrary;
            bLib1.AddDiagram("blib1", "Package");
            bLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1");
            bLib1.AddPackage("primlib1", InitPRIMLib1);
            bLib1.AddPackage("enumlib1", InitENUMLib1);
            bLib1.AddPackage("cdtlib1", InitCDTLib1);
            bLib1.AddPackage("bdtlib1", InitBDTLib1);
            bLib1.AddPackage("cclib1", InitCCLib1);
            bLib1.AddPackage("bielib1", InitBIELib1);
            bLib1.AddPackage("DOCLibrary", InitDOCLibrary);
        }

        private void InitPRIMLib1(Package primLib1)
        {
            primLib1.Element.Stereotype = Stereotype.PRIMLibrary;
            primLib1.AddDiagram("primlib1", "Class");
            primLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:primlib1");
            primString = AddPRIM(primLib1, "String");
            primDecimal = AddPRIM(primLib1, "Decimal");
            AddInvalidElement(primLib1);
        }

        private void InitENUMLib1(Package enumLib1)
        {
            enumLib1.Element.Stereotype = Stereotype.ENUMLibrary;
            enumLib1.AddDiagram("enumlib1", "Class");
            enumLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:enumlib1");
            AddENUM(enumLib1, "ABC_Codes", primString, "ABC Code 1", "abc1",
                    "ABC Code 2", "abc2");
            AddInvalidElement(enumLib1);
        }

        private void InitCDTLib1(Package cdtLib1)
        {
            cdtLib1.Element.Stereotype = Stereotype.CDTLibrary;
            cdtLib1.AddDiagram("cdtlib1", "Class");
            cdtLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cdtlib1");
            cdtMeasure = cdtLib1.AddElement("Measure", "Class").With(e =>
                                                                     {
                                                                         e.Stereotype = Stereotype.CDT;
                                                                         AddCON(e, primDecimal);
                                                                         AddSUP(e, primString, "MeasureUnit");
                                                                         AddAttribute(e, "MeasureUnit.CodeListVersion", primString, Stereotype.SUP,
                                                                                      a =>
                                                                                      {
                                                                                          a.LowerBound = "1";
                                                                                          a.UpperBound = "*";
                                                                                      });
                                                                     });
            cdtCode = cdtLib1.AddElement("Code", "Class").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.CDT;
                                                                   AddCON(e, primString);
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
            cdtDate = cdtLib1.AddElement("Date", "Class").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.CDT;
                                                                   e.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("A Date.");
                                                                   AddCON(e, primString);
                                                                   AddSUPs(e, primString, "Format");
                                                               });
            cdtText = cdtLib1.AddElement("Text", "Class").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.CDT;
                                                                   AddCON(e, primString);
                                                                   AddSUPs(e, primString, "Language", "Language.Locale");
                                                               });
            AddInvalidElement(cdtLib1);
        }

        private void InitBDTLib1(Package bdtLib1)
        {
            bdtLib1.Element.Stereotype = Stereotype.BDTLibrary;
            bdtLib1.AddDiagram("bdtlib1", "Class");
            bdtLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bdtlib1");
            bdtMeasure = bdtLib1.AddElement("Measure", "Class").With(e =>
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
            bdtCode = bdtLib1.AddElement("Code", "Class").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.BDT;
                                                                   AddCON(e, primString);
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
            bdtABCCode = bdtLib1.AddElement("ABC_Code", "Class").With(e =>
                                                                      {
                                                                          e.Stereotype = Stereotype.BDT;
                                                                          AddCON(e, primString);
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
            bdtDate = bdtLib1.AddElement("Date", "Class").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.BDT;
                                                                   AddCON(e, primString);
                                                                   AddSUPs(e, primString, "Format");
                                                               });
            bdtText = bdtLib1.AddElement("Text", "Class").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.BDT;
                                                                   e.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("This is the definition of BDT Text.");
                                                                   AddCON(e, primString);
                                                                   AddSUPs(e, primString, "Language", "Language.Locale");
                                                               });
            AddInvalidElement(bdtLib1);
        }

        private void InitCCLib1(Package ccLib1)
        {
            ccLib1.Element.Stereotype = Stereotype.CCLibrary;
            ccLib1.AddDiagram("cclib1", "Class");
            ccLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cclib1");
            accAddress = ccLib1.AddElement("Address", "Class").With(e =>
                                                                    {
                                                                        e.Stereotype = Stereotype.ACC;
                                                                        AddAttribute(e, "CountryName", cdtText, Stereotype.BCC);
                                                                        AddAttribute(e, "CityName", cdtText, Stereotype.BCC);
                                                                        AddAttribute(e, "StreetName", cdtText, Stereotype.BCC);
                                                                        AddAttribute(e, "StreetNumber", cdtText, Stereotype.BCC);
                                                                        AddAttribute(e, "Postcode", cdtText, Stereotype.BCC, postcode =>
                                                                                                                             {
                                                                                                                                 postcode.LowerBound = "0";
                                                                                                                                 postcode.UpperBound = "*";
                                                                                                                             });
                                                                    });
            accPerson = ccLib1.AddElement("Person", "Class").With(e =>
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
            AddInvalidElement(ccLib1);
        }

        private void InitBIELib1(Package bieLib1)
        {
            bieLib1.Element.Stereotype = Stereotype.BIELibrary;
            bieLib1.AddDiagram("bielib1", "Class");
            bieLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bielib1");
            bieMyAddress = bieLib1.AddElement("MyAddress", "Class").With(e =>
                                                                         {
                                                                             e.Stereotype = Stereotype.ABIE;
                                                                             AddAttribute(e, "CountryName", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "CityName", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "StreetName", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "StreetNumber", bdtText, Stereotype.BBIE);
                                                                             AddAttribute(e, "Postcode", bdtText, Stereotype.BBIE, postcode =>
                                                                                                                                   {
                                                                                                                                       postcode.LowerBound = "0";
                                                                                                                                       postcode.UpperBound = "*";
                                                                                                                                   });
                                                                         });
            bieMyPerson = bieLib1.AddElement("MyPerson", "Class").With(e =>
                                                                       {
                                                                           e.Stereotype = Stereotype.ABIE;
                                                                           AddAttribute(e, "FirstName", bdtText, Stereotype.BBIE);
                                                                           AddAttribute(e, "LastName", bdtText, Stereotype.BBIE);
                                                                       });
            AddInvalidElement(bieLib1);
        }

        private void InitDOCLibrary(Package docLibrary)
        {
            docLibrary.Element.Stereotype = Stereotype.DOCLibrary;
            docLibrary.AddDiagram("DOCLibrary", "Class");
            docLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:doclibrary");
            bieInvoice = docLibrary.AddElement("Invoice", "Class").With(e =>
                                                                        {
                                                                            e.Stereotype = Stereotype.ABIE;
                                                                            AddAttribute(e, "Amount", bdtText, Stereotype.BBIE);
                                                                        });
            bieInvoiceInfo = docLibrary.AddElement("InvoiceInfo", "Class").With(e =>
                                                                                {
                                                                                    e.Stereotype = Stereotype.ABIE;
                                                                                    AddAttribute(e, "Info", bdtText, Stereotype.BBIE);
                                                                                });
            AddInvalidElement(docLibrary);
        }

        private static void AddInvalidElement(Package package)
        {
            package.AddElement("InvalidElement", "Class").With(e => { e.Stereotype = "InvalidStereotype"; });
        }

        #region Paths

        public static Path PathToDate()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Date";
        }

        public static Path PathToCode()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Code";
        }

        public static Path PathToText()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Text";
        }

        public static Path PathToBDTText()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Text";
        }

        public static Path PathToBDTCode()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Code";
        }

        public static Path PathToBdtAbcCode()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"ABC_Code";
        }

        public static Path PathToBdtDate()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Date";
        }

        public static Path PathToBdtMeasure()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Measure";
        }

        public static Path PathToEnumAbcCodes()
        {
            return (Path) "test model"/"blib1"/"enumlib1"/"ABC_Codes";
        }

        public static Path PathToDecimal()
        {
            return (Path) "test model"/"blib1"/"primlib1"/"Decimal";
        }

        public static Path PathToString()
        {
            return (Path) "test model"/"blib1"/"primlib1"/"String";
        }

        public static Path PathToAddress()
        {
            return (Path) "test model"/"blib1"/"cclib1"/"Address";
        }

        public static Path PathToACCPerson()
        {
            return (Path) "test model"/"blib1"/"cclib1"/"Person";
        }

        public static Path PathToBIEAddress()
        {
            return (Path) "test model"/"blib1"/"bielib1"/"MyAddress";
        }

        public static Path PathToBIEPerson()
        {
            return (Path) "test model"/"blib1"/"bielib1"/"MyPerson";
        }

        public static Path PathToInvoice()
        {
            return (Path) "test model"/"blib1"/"DOCLibrary"/"Invoice";
        }

        public static Path PathToInvoiceInfo()
        {
            return (Path) "test model"/"blib1"/"DOCLibrary"/"InvoiceInfo";
        }

        #endregion
    }
}