using System;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public class EAException : Exception
    {
        public EAException(string errorMessage):base(errorMessage)
        {
        }
    }
}