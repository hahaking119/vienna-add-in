using System;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class PrimLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(PrimLibrarySpec primLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}