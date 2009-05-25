// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IASBIE : IBIE, ISequenced, IHasMultiplicity
    {
        IABIE AssociatingElement { get; }
        IABIE AssociatedElement { get; }
        AggregationKind AggregationKind { get; }
    }
}