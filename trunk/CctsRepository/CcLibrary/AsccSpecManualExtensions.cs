using System;

namespace CctsRepository.CcLibrary
{
    public partial class AsccSpec
    {
        public AsccSpec()
        {
            ResolveAssociatedAcc = () => AssociatedAcc;
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IAcc> ResolveAssociatedAcc { get; set; }
    }
}