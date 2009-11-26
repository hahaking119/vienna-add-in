// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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
    public interface ICctsRepository
    {
        IEnumerable<IBLibrary> GetBLibraries();
        IEnumerable<IPrimLibrary> GetPrimLibraries();
        IEnumerable<IEnumLibrary> GetEnumLibraries();
        IEnumerable<ICdtLibrary> GetCdtLibraries();
        IEnumerable<ICcLibrary> GetCcLibraries();
        IEnumerable<IBdtLibrary> GetBdtLibraries();
        IEnumerable<IBieLibrary> GetBieLibraries();
        IEnumerable<IDOCLibrary> GetDocLibraries();

        IBLibrary GetBLibraryById(int id);
        IPrimLibrary GetPrimLibraryById(int id);
        IEnumLibrary GetEnumLibraryById(int id);
        ICdtLibrary GetCdtLibraryById(int id);
        ICcLibrary GetCcLibraryById(int id);
        IBdtLibrary GetBdtLibraryById(int id);
        IBieLibrary GetBieLibraryById(int id);
        IDOCLibrary GetDocLibraryById(int id);

        IPrim GetPrimById(int id);
        IEnum GetEnumById(int id);
        ICdt GetCdtById(int id);
        IAcc GetAccById(int id);
        IBdt GetBdtById(int id);
        IAbie GetAbieById(int id);

        IBLibrary GetBLibraryByPath(Path path);
        IPrimLibrary GetPrimLibraryByPath(Path path);
        IEnumLibrary GetEnumLibraryByPath(Path path);
        ICdtLibrary GetCdtLibraryByPath(Path path);
        ICcLibrary GetCcLibraryByPath(Path path);
        IBdtLibrary GetBdtLibraryByPath(Path path);
        IBieLibrary GetBieLibraryByPath(Path path);
        IDOCLibrary GetDocLibraryByPath(Path path);

        IPrim GetPrimByPath(Path path);
        IEnum GetEnumByPath(Path path);
        ICdt GetCdtByPath(Path path);
        IAcc GetAccByPath(Path path);
        IBdt GetBdtByPath(Path path);
        IAbie GetAbieByPath(Path path);

        IEnumerable<Path> GetRootLocations();
        IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec spec);
    }
}