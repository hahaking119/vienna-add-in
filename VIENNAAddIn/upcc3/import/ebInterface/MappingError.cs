using System;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class MappingError : Exception
    {
        public MappingError(string message) : base(message)
        {
        }
    }
}