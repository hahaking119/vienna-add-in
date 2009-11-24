// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace CctsRepository.CcLibrary
{
    public interface IASCC : ICC
    {
        IACC AssociatingElement { get; }
        IACC AssociatedElement { get; }
        AggregationKind AggregationKind { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        string SequencingKey { get; }
    }
}