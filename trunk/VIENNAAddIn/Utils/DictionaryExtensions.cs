using System.Collections.Generic;

namespace VIENNAAddIn.Utils
{
    ///<summary>
    /// Extension methods for dictionaries.
    ///</summary>
    public static class DictionaryExtensions
    {

        ///<summary>
        /// Retrieves a value from the dictionary. If the key does not yet exist, a new instance of the value type is added to the dictionary for the given key.
        ///</summary>
        ///<param name="dictionary"></param>
        ///<param name="key"></param>
        ///<typeparam name="TKey"></typeparam>
        ///<typeparam name="TValue"></typeparam>
        ///<returns></returns>
        public static TValue GetAndCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue:new()
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = new TValue();
                dictionary[key] = value;
            }
            return value;
        }
    
    }
}