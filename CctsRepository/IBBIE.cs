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
    public interface IBBIE : IBIE, ISequenced, IHasMultiplicity
    {
        IBDT Type { get; }
        IABIE Container { get; }

        /// <summary>
        /// Returns the BCC on which the BBIE is based or <c>null</c>, if the BCC cannot be determined.
        /// </summary>
        IBCC BasedOn { get; }
    }
}