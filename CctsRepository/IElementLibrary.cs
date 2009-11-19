using System.Collections.Generic;

namespace CctsRepository
{
    ///<summary>
    ///</summary>
    ///<typeparam name="TICCTSElement"></typeparam>
    ///<typeparam name="TCCTSElementSpec"></typeparam>
    public interface IElementLibrary<TICCTSElement, TCCTSElementSpec> : IBusinessLibrary
    {
        ///<summary>
        ///</summary>
        IEnumerable<TICCTSElement> Elements { get; }

        ///<summary>
        /// Retrieves an element by name.
        ///</summary>
        ///<param name="name"></param>
        ///<returns></returns>
        TICCTSElement ElementByName(string name);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        TICCTSElement CreateElement(TCCTSElementSpec spec);

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        TICCTSElement UpdateElement(TICCTSElement element, TCCTSElementSpec spec);
    }
}