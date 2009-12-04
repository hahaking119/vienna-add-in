using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class AbstractClasses
    {
        /// <summary>
        /// Abstract base class for PRIM, ENUM and IDSCHEME.
        /// </summary>
        internal readonly MetaAbstractClass BasicType;

        /// <summary>
        /// Abstract base class for ABIE and MA.
        /// </summary>
        internal readonly MetaAbstractClass BieAggregator;

        internal AbstractClasses()
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

        internal IEnumerable<MetaAbstractClass> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaAbstractClass) field.GetValue(this);
                }
            }
        }
    }
}