using System;
using CctsRepository.BieLibrary;

namespace CctsRepository.DocLibrary
{
    public partial class AsmaSpec
    {
        public Func<IAbie> ResolveAssociatedAbie { get; set; }
        public Func<IMa> ResolveAssociatedMa { get; set; }
    }
}