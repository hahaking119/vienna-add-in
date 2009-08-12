﻿// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using EA;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype = VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    public class EARepositoryLibraryImporter : EARepository
    {
        public EARepositoryLibraryImporter()
        {
            this.AddModel("Test Model 1", m => m.AddPackage("bLibrary", bLibrary =>
            {
                bLibrary.Element.Stereotype = Stereotype.BLibrary;
            }));
            this.AddModel("Test Model 2", m => m.AddPackage("bLibrary", bLibrary =>
            {
                bLibrary.Element.Stereotype = Stereotype.BLibrary;
                AddLibrary(bLibrary, Stereotype.ENUMLibrary);
                AddLibrary(bLibrary, Stereotype.PRIMLibrary);
                AddLibrary(bLibrary, Stereotype.CDTLibrary);
                AddLibrary(bLibrary, Stereotype.CCLibrary);
                AddLibrary(bLibrary, Stereotype.BDTLibrary);
                AddLibrary(bLibrary, Stereotype.BIELibrary);
                AddLibrary(bLibrary, Stereotype.DOCLibrary);
            }));
        }

        private static void AddLibrary(Package bLibrary, string stereotype)
        {
            bLibrary.AddPackage(stereotype, p => p.Element.Stereotype = stereotype);
        }
    }
}