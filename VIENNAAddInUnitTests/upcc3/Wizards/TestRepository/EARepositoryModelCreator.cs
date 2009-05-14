using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;


namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    public class EARepositoryModelCreator : EARepository
    {
        public EARepositoryModelCreator()
        {
            SetContent(Package("Test Model 1", "")
                           .Packages(Package("bLibrary", Stereotype.BLibrary)
                                         .Packages(Package("ENUMLibrary", Stereotype.ENUMLibrary),
                                                   Package("PRIMLibrary", Stereotype.PRIMLibrary),
                                                   Package("CDTLibrary", Stereotype.CDTLibrary),
                                                   Package("CCLibrary", Stereotype.CCLibrary),
                                                   Package("BDTLibrary", Stereotype.BDTLibrary),
                                                   Package("BIELibrary", Stereotype.BIELibrary),
                                                   Package("DOCLibrary", Stereotype.DOCLibrary))),
                        Package("Test Model 2", "")
                           .Packages(Package("bLibrary", Stereotype.BLibrary)),
                        Package("Test Model 3", ""),
                        Package("Test Model 4", "")
                           .Packages(Package("bLibrary", Stereotype.BLibrary)
                                         .Packages(Package("ENUMLibrary", Stereotype.ENUMLibrary),
                                                   Package("PRIMLibrary", Stereotype.PRIMLibrary),
                                                   Package("CDTLibrary", Stereotype.CDTLibrary),
                                                   Package("CCLibrary", Stereotype.CCLibrary),
                                                   Package("BDTLibrary", Stereotype.BDTLibrary),
                                                   Package("BIELibrary", Stereotype.BIELibrary),
                                                   Package("DOCLibrary", Stereotype.DOCLibrary))));
        }
    }
}