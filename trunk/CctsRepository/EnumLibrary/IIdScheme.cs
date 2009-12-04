using System.Collections.Generic;

namespace CctsRepository.EnumLibrary
{
    public interface IIdScheme
    {
        string Name { get; }
        IEnumerable<string> BusinessTerms { get; }
        string UniqueIdentifier { get; set; }
        string VersionIdentifier { get; set; }
        string DictionaryEntryName { get; }
        int Id { get; }
    }
}