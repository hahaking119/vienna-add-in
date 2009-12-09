using System;

namespace CctsRepository.DocLibrary
{
    public partial class AsmaSpec
    {
        public AsmaSpec()
        {
            ResolveAssociatingMa = () => AssociatingMa;
            ResolveAssociatedBieAggregator = () => AssociatedBieAggregator;
        }

        public Func<IMa> ResolveAssociatingMa { get; set; }
        public Func<BieAggregator> ResolveAssociatedBieAggregator { get; set; }
    }
}