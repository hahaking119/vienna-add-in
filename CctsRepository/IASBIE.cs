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
    public interface IASBIE : IBIE, ISequenced, IHasMultiplicity
    {
        IABIE AssociatingElement { get; }
        IABIE AssociatedElement { get; }
        EAAggregationKind AggregationKind { get; }

        /// <summary>
        /// Returns the ASCC on which the ASBIE is based or <c>null</c>, if the ASCC cannot be determined.
        /// </summary>
        IASCC BasedOn { get; }
    }
}