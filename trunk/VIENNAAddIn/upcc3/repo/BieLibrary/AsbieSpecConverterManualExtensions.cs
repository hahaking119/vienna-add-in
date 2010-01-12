using System;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal static partial class AsbieSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(AsbieSpec asbieSpec)
        {
            return asbieSpec.AssociatingAbie.Name + ". " + asbieSpec.Name + ". " + asbieSpec.AssociatedAbie.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(AsbieSpec asbieSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
