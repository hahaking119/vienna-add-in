using System.Collections.Generic;
using CctsRepository;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.export.mapping;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.export.mapping
{
    [TestFixture]
    public class SubsetExporterTest
    {
        [Test]
        public void Test_exporting_subset_of_complex_type_mapped_to_multiple_accs()
        {
            string schemaFileComplete = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_complex_type_mapped_to_multiple_accs\source.xsd");
            string schemaFileSubset = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_complex_type_mapped_to_multiple_accs\source_subset.xsd");            

            EARepository eaRepository = new EARepository();
            Element cdtText = null;
            Element bdtStringText = null;
            Element primString = null;
            Element accAddress = null;
            Element accPerson = null;
            Element abieAddressTypeAddress = null;
            Element abieAddressTypePerson = null;

            #region EA Repository Complete

            eaRepository.AddModel(
                "Test Model Complete", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

                    bLibrary.AddPackage("PRIMLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.PRIMLibrary;

                        primString = package.AddPRIM("String");
                    });

                    bLibrary.AddPackage("CDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.CDTLibrary;

                        cdtText = package.AddCDT("Text").With(e =>
                        {
                            e.Stereotype = Stereotype.CDT;
                            e.AddCON(primString);
                            e.AddSUPs(primString, "Language", "LanguageLocale");
                        });
                    });

                    bLibrary.AddPackage("CCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.CCLibrary;

                        accAddress = package.AddClass("Address")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "StreetName", "CityName", "AttentionOf"));

                        accPerson = package.AddClass("Person")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "Name", "Title", "Salutation"));
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;
                            e.AddBasedOnDependency(cdtText);
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                                                                                                  {
                                                                                                      e.Stereotype = Stereotype.ABIE;
                                                                                                      e.AddBasedOnDependency(accAddress);
                                                                                                      e.AddBBIE(bdtStringText, "Town_CityName");
                                                                                                  });
                        abieAddressTypePerson = package.AddClass("AddressType_Person").With(e =>
                                                                                                {
                                                                                                    e.Stereotype = Stereotype.ABIE;
                                                                                                    e.AddBasedOnDependency(accPerson);
                                                                                                    e.AddBBIE(bdtStringText, "PersonName_Name");
                                                                                                });
                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maAddressType = package.AddClass("AddressType").With(e =>
                                                                                   {
                                                                                       e.Stereotype = Stereotype.MA;
                                                                                       e.AddASMA(abieAddressTypeAddress, "Address");
                                                                                       e.AddASMA(abieAddressTypePerson, "Person");
                                                                                   });

                        Element maInvoiceType = package.AddClass("InvoiceType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maAddressType, "Address");
                        });

                        package.AddClass("Test_Invoice").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maInvoiceType, "Invoice");
                        });


                    });
                }));

#endregion

            #region EA Repository Subset

            eaRepository.AddModel(
                "Test Model Subset", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

                    bLibrary.AddPackage("PRIMLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.PRIMLibrary;

                        primString = package.AddPRIM("String");
                    });

                    bLibrary.AddPackage("CDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.CDTLibrary;

                        cdtText = package.AddCDT("Text").With(e =>
                        {
                            e.Stereotype = Stereotype.CDT;
                            e.AddCON(primString);
                            e.AddSUPs(primString, "Language", "LanguageLocale");
                        });
                    });

                    bLibrary.AddPackage("CCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.CCLibrary;

                        accAddress = package.AddClass("Address")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "StreetName", "CityName", "AttentionOf"));

                        accPerson = package.AddClass("Person")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "Name", "Title", "Salutation"));
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;
                            e.AddBasedOnDependency(cdtText);
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBasedOnDependency(accAddress);
                            e.AddBBIE(bdtStringText, "Town_CityName");
                        });
                        abieAddressTypePerson = package.AddClass("AddressType_Person").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBasedOnDependency(accPerson);
                        });
                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maAddressType = package.AddClass("AddressType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(abieAddressTypeAddress, "Address");
                            e.AddASMA(abieAddressTypePerson, "Person");
                        });

                        Element maInvoiceType = package.AddClass("InvoiceType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maAddressType, "Address");
                        });

                        package.AddClass("Test_Invoice").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maInvoiceType, "Invoice");
                        });


                    });
                }));

            #endregion

            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            // 1st step:
            // paare<ct-name, list<string> element/attribute-name>
            IDocLibrary docLibraryComplete = cctsRepository.GetDocLibraryByPath((Path)"Test Model Complete"/"bLibrary"/"DOCLibrary");
            IDocLibrary docLibrarySubset = cctsRepository.GetDocLibraryByPath((Path)"Test Model Subset"/"bLibrary"/"DOCLibrary");

            Dictionary<string, List<string>> mutatedComplexTypes = new UpccModelDiff(docLibraryComplete, docLibrarySubset).CalculateDiff();

            Dictionary<string, List<string>> expectedMutatedComplexTypes = new Dictionary<string, List<string>>();
            expectedMutatedComplexTypes.Add("AddressType", new List<string> {"PersonName"});
            
            Assert.That(mutatedComplexTypes.Keys, Is.EquivalentTo(expectedMutatedComplexTypes.Keys), "Defective Complex Type Mutation.");

            foreach (string complexTypeName in expectedMutatedComplexTypes.Keys)
            {
                Assert.That(mutatedComplexTypes[complexTypeName], Is.EquivalentTo(expectedMutatedComplexTypes[complexTypeName]), "Difference between Complex Type Mutation and expected Complex Type Mutation: " + complexTypeName + ".");
            }

            // 2nd step:
            // das xml schema durchGEHEN und nach allen complex types suchen, die 
            // den namen des schluessels des paares haben und entsprechend modifizieren.
            
            new SubsetExporter(docLibraryComplete, docLibrarySubset, schemaFileComplete, schemaFileSubset).ExportSubset();

            // 3rd step: 
            // nachdem complex types eventuell komplett entfernt wurden, muessen auch noch
            // alle elemente welche diesen complex type verwenden entfernt werden.

            // 4th step:
            // xml schema serialisieren
        }

    }
}