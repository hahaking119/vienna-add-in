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
    public interface IBasicType
    {
        ///<summary>
        ///</summary>
        int Id { get; }

        ///<summary>
        ///</summary>
        string Name { get; }

        ///<summary>
        ///</summary>
        string DictionaryEntryName { get; }
    }
}