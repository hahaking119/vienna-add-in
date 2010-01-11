using System;
using CctsRepository.EnumLibrary;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class EnumLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(EnumLibrarySpec enumLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}