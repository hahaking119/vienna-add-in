// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace CctsRepository
{
    public interface IBCC : ICC
    {
        ICDT Type { get; }
        IACC Container { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        string SequencingKey { get; }
    }
}