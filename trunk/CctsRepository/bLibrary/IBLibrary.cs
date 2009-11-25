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
    public interface IBLibrary
    {
        IEnumerable<IBLibrary> GetBLibraries();
        IEnumerable<IPRIMLibrary> GetPrimLibraries();
        IEnumerable<IENUMLibrary> GetEnumLibraries();
        IEnumerable<ICDTLibrary> GetCdtLibraries();
        IEnumerable<ICCLibrary> GetCcLibraries();
        IEnumerable<IBDTLibrary> GetBdtLibraries();
        IEnumerable<IBIELibrary> GetBieLibraries();
        IEnumerable<IDOCLibrary> GetDocLibraries();

        IBLibrary CreateBLibrary(BLibrarySpec spec);
        ICDTLibrary CreateCDTLibrary(CdtLibrarySpec spec);
        ICCLibrary CreateCCLibrary(CcLibrarySpec spec);
        IBDTLibrary CreateBDTLibrary(BdtLibrarySpec spec);
        IBIELibrary CreateBIELibrary(BieLibrarySpec spec);
        IPRIMLibrary CreatePRIMLibrary(PrimLibrarySpec spec);
        IENUMLibrary CreateENUMLibrary(EnumLibrarySpec spec);
        IDOCLibrary CreateDOCLibrary(DocLibrarySpec spec);
        int Id { get; }
        string Name { get; }
        IBLibrary Parent { get; }
        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> Copyrights { get; }
        IEnumerable<string> Owners { get; }
        IEnumerable<string> References { get; }
    }
}