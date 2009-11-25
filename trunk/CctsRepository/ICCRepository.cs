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
    public interface ICCRepository
    {
        IEnumerable<IBLibrary> GetBLibraries();
        IEnumerable<IPRIMLibrary> GetPrimLibraries();
        IEnumerable<IENUMLibrary> GetEnumLibraries();
        IEnumerable<ICDTLibrary> GetCdtLibraries();
        IEnumerable<ICCLibrary> GetCcLibraries();
        IEnumerable<IBDTLibrary> GetBdtLibraries();
        IEnumerable<IBIELibrary> GetBieLibraries();
        IEnumerable<IDOCLibrary> GetDocLibraries();

        IBLibrary GetBLibraryById(int id);
        IPRIMLibrary GetPrimLibraryById(int id);
        IENUMLibrary GetEnumLibraryById(int id);
        ICDTLibrary GetCdtLibraryById(int id);
        ICCLibrary GetCcLibraryById(int id);
        IBDTLibrary GetBdtLibraryById(int id);
        IBIELibrary GetBieLibraryById(int id);
        IDOCLibrary GetDocLibraryById(int id);

        ICDT GetCdtById(int id);
        IACC GetAccById(int id);
        IBDT GetBdtById(int id);
        IABIE GetAbieById(int id);

        object FindByPath(Path path);
    }
}