using System.Collections.Generic;

namespace CctsRepository.DocLibrary
{
    public interface IMa
    {
        string Name { get; }
        IEnumerable<IAsma> Asmas { get; }
        int Id { get; }
    }
}