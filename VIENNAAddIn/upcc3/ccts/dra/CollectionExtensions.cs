using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public static class CollectionExtensions
    {
        public static IList<OutputType> Convert<InputType, OutputType>(this Collection collection,
                                                                       Func<InputType, OutputType> convert)
        {
            var result = new List<OutputType>();
            foreach (object input in collection)
            {
                result.Add(convert((InputType) input));
            }
            return result;
        }
    }
}