using System.Collections.Generic;
using System.Reflection;

namespace UpccModel
{
    public class UpccAbstractClasses
    {
        /// <summary>
        /// Abstract base class for PRIM, ENUM and IDSCHEME.
        /// </summary>
        public readonly MetaAbstractClass BasicType;

        /// <summary>
        /// Abstract base class for ABIE and MA.
        /// </summary>
        public readonly MetaAbstractClass BieAggregator;

        public UpccAbstractClasses()
        {
            BasicType = new MetaAbstractClass
                        {
                            Name = "BasicType",
                        };
            BieAggregator = new MetaAbstractClass
                            {
                                Name = "BieAggregator",
                            };
        }

        public IEnumerable<MetaAbstractClass> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaAbstractClass) field.GetValue(this);
                }
            }
        }
    }
}