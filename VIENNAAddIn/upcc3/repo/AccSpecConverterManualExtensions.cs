using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class AccSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(AccSpec accSpec)
        {
            return accSpec.Name + ". Details";
        }

        private static string GenerateUniqueIdentifierDefaultValue(AccSpec accSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}