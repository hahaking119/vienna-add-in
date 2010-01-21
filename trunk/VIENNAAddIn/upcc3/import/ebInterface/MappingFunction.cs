using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    internal class MappingFunction
    {
        public MappingFunction(List<TargetCCElement> targetCCElements)
        {
            TargetCcElements = targetCCElements.ToArray();
        }

        public bool IsSplit
        {
            get { return true; }
        }

        public TargetCCElement[] TargetCcElements { get; private set; }
    }
}