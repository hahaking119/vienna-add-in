// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddInUtils;

namespace CctsRepository
{
    public interface ICCRepository
    {
        IEnumerable<IBLibrary> GetBLibraries();
        IEnumerable<IPRIMLibrary> GetPrimLibraries();
        IEnumerable<IENUMLibrary> GetEnumLibraries();
        IEnumerable<ICDTLibrary> GetCdtLibraries();
        IEnumerable<ICCLibrary> GetCcLibraries();
        IEnumerable<IBdtLibrary> GetBdtLibraries();
        IEnumerable<IBIELibrary> GetBieLibraries();
        IEnumerable<IDOCLibrary> GetDocLibraries();

        IBLibrary GetBLibraryById(int id);
        IPRIMLibrary GetPrimLibraryById(int id);
        IENUMLibrary GetEnumLibraryById(int id);
        ICDTLibrary GetCdtLibraryById(int id);
        ICCLibrary GetCcLibraryById(int id);
        IBdtLibrary GetBdtLibraryById(int id);
        IBIELibrary GetBieLibraryById(int id);
        IDOCLibrary GetDocLibraryById(int id);

        IPRIM GetPrimById(int id);
        IENUM GetEnumById(int id);
        ICDT GetCdtById(int id);
        IACC GetAccById(int id);
        IBdt GetBdtById(int id);
        IABIE GetAbieById(int id);

        IBLibrary GetBLibraryByPath(Path path);
        IPRIMLibrary GetPrimLibraryByPath(Path path);
        IENUMLibrary GetEnumLibraryByPath(Path path);
        ICDTLibrary GetCdtLibraryByPath(Path path);
        ICCLibrary GetCcLibraryByPath(Path path);
        IBdtLibrary GetBdtLibraryByPath(Path path);
        IBIELibrary GetBieLibraryByPath(Path path);
        IDOCLibrary GetDocLibraryByPath(Path path);

        IPRIM GetPrimByPath(Path path);
        IENUM GetEnumByPath(Path path);
        ICDT GetCdtByPath(Path path);
        IACC GetAccByPath(Path path);
        IBdt GetBdtByPath(Path path);
        IABIE GetAbieByPath(Path path);

        IEnumerable<Path> GetRootLocations();
        IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec spec);
    }
}