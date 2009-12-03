using System;
using CctsRepository.BieLibrary;

namespace CctsRepository.DocLibrary
{
    public interface IAsma
    {
        IMa AssociatedElement { get; }
    }

    public class AsmaSpec
    {
        public Func<IAbie> ResolveAssociatedAbie { get; set; }
        public Func<IMa> ResolveAssociatedMa { get; set; }

        public string Name { get; set; }
    }
}