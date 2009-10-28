// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace UPCCRepositoryInterface
{
    public interface IASCC : ICC, ISequenced, IHasMultiplicity
    {
        IACC AssociatingElement { get; }
        IACC AssociatedElement { get; }
        EAAggregationKind AggregationKind { get; }
    }
}