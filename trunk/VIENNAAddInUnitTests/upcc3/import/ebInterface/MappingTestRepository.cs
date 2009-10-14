using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.import.ebInterface
{
    internal class MappingTestRepository : EARepository
    {
        public MappingTestRepository()
        {
            Element cdtText = null;
            Element primString = null;
            Element accAddress = null;
            this.AddModel(
                "test", m => m.AddPackage("bLibrary", bLibrary =>
                                                      {
                                                          bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                          bLibrary.AddDiagram("bLibrary", "Class");
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
                                                                                                                                          e.AddSUPs(primString, "Language", "Language.Locale");
                                                                                                                                      });
                                                                                            });
                                                          bLibrary.AddPackage("CCLibrary", package =>
                                                                                           {
                                                                                               package.Element.Stereotype = Stereotype.CCLibrary;
                                                                                               package.AddClass("Foo").With(e => e.Stereotype = Stereotype.ACC);
                                                                                               accAddress = package.AddClass("Address")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "StreetName", "CityName"));
                                                                                               package.AddClass("AccountingVoucher")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC);
                                                                                               package.AddClass("Person")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "Name"));
                                                                                               package.AddClass("Party")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "Name"))
                                                                                                   .With(e => e.AddASCC(accAddress, "Residence"));
                                                                                           });
                                                      }));
        }
    }
}