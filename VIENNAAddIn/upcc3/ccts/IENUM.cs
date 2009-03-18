// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace VIENNAAddIn.upcc3.ccts
{
    public interface IENUM : IBasicType
    {
        string AgencyIdentifier { get; }
        string AgencyName { get; }
        string EnumerationURI { get; }
        IENUM IsEquivalentTo { get; }
    }
}