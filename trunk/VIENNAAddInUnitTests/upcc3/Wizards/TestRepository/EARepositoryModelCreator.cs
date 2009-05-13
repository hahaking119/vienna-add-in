using NUnit.Framework;
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
                                                  Package("CDTLibrary", Stereotype.PRIMLibrary),
                                                  Package("CCLibrary", Stereotype.CCLibrary),
                                                  Package("BIELibrary", Stereotype.BIELibrary),
                                                  Package("DOCLibrary", Stereotype.DOCLibrary))),
                       Package("Test Model 2", "")
                           .Packages(Package("bLibrary", Stereotype.BLibrary),
                       Package("Test Model 3", "")
                           .Packages(Package("bLibrary", Stereotype.BLibrary)
                                        .Packages(Package("CDTLibrary", Stereotype.PRIMLibrary),
                                                  Package("CCLibrary", Stereotype.CCLibrary),
                                                  Package("DOCLibrary", Stereotype.DOCLibrary))))                                                  
                        );
        }
    }
}