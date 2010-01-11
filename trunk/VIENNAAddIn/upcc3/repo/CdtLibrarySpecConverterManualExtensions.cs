using System;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class CdtLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(CdtLibrarySpec cdtLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}