using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class AsccSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(AsccSpec asccSpec)
        {
            return asccSpec.AssociatingAcc.Name + ". " + asccSpec.Name + ". " + asccSpec.AssociatedAcc.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(AsccSpec asccSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}