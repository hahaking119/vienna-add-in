using System;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class MappingError : Exception
    {
        public MappingError(string message) : base(message)
        {
        }
    }
}