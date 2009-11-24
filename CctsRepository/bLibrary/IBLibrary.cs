using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;

namespace CctsRepository.bLibrary
{
    ///<summary>
    ///</summary>
    public interface IBLibrary : IBusinessLibrary
    {
        /// <summary>
        /// Returns the direct sub-libraries of this library.
        /// </summary>
        IEnumerable<IBusinessLibrary> Children { get; }

        /// <summary>
        /// Returns all of this library's sub-libraries and their sub-libraries (depth-first).
        /// </summary>
        IEnumerable<IBusinessLibrary> AllChildren { get; }

        IBusinessLibrary FindChildByName(string name);

        IBLibrary CreateBLibrary(LibrarySpec spec);
        ICDTLibrary CreateCDTLibrary(LibrarySpec spec);
        ICCLibrary CreateCCLibrary(LibrarySpec spec);
        IBDTLibrary CreateBDTLibrary(LibrarySpec spec);
        IBIELibrary CreateBIELibrary(LibrarySpec spec);
        IPRIMLibrary CreatePRIMLibrary(LibrarySpec spec);
        IENUMLibrary CreateENUMLibrary(LibrarySpec spec);
        IDOCLibrary CreateDOCLibrary(LibrarySpec spec);
    }
}