using System;
using CctsRepository.BieLibrary;

namespace CctsRepository.DocLibrary
{
    public partial class AsmaSpec
    {
        public Func<IAbie> ResolveAssociatedAbie { get; set; }
        public Func<IMa> ResolveAssociatedMa { get; set; }

        public static AsmaSpec CloneAsma(IAsma asma)
        {
            return new AsmaSpec
                   {
                       Name = asma.Name,
                       LowerBound = asma.LowerBound,
                       UpperBound = asma.UpperBound,
                       AssociatingMa = asma.AssociatingMa,
                       AssociatedBieAggregator = asma.AssociatedBieAggregator,
                   };
        }
    }
}