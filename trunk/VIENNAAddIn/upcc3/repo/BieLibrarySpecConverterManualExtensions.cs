using System;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class BieLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(BieLibrarySpec bieLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}