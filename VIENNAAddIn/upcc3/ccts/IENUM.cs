using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IENUM : IBasicType
    {
        string AgencyIdentifier { get; }
        string AgencyName { get; }
        string EnumerationURI { get; }
    }
}