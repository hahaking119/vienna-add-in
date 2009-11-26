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
        IEnumerable<IPrimLibrary> GetPrimLibraries();
        IEnumerable<IEnumLibrary> GetEnumLibraries();
        IEnumerable<ICdtLibrary> GetCdtLibraries();
        IEnumerable<ICcLibrary> GetCcLibraries();
        IEnumerable<IBdtLibrary> GetBdtLibraries();
        IEnumerable<IBieLibrary> GetBieLibraries();
        IEnumerable<IDocLibrary> GetDocLibraries();

        IBLibrary CreateBLibrary(BLibrarySpec spec);
        ICdtLibrary CreateCDTLibrary(CdtLibrarySpec spec);
        ICcLibrary CreateCCLibrary(CcLibrarySpec spec);
        IBdtLibrary CreateBDTLibrary(BdtLibrarySpec spec);
        IBieLibrary CreateBIELibrary(BieLibrarySpec spec);
        IPrimLibrary CreatePRIMLibrary(PrimLibrarySpec spec);
        IEnumLibrary CreateENUMLibrary(EnumLibrarySpec spec);
        IDocLibrary CreateDOCLibrary(DocLibrarySpec spec);
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