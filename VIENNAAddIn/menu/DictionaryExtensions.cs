using System.Collections.Generic;

namespace VIENNAAddIn.menu
{
    public static class DictionaryExtensions
    {

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