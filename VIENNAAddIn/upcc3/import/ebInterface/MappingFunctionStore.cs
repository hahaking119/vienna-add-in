using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    internal class MappingFunctionStore
    {
        private readonly Dictionary<string, MappingFunction> mappingFunctions = new Dictionary<string, MappingFunction>();

        public MappingFunctionStore(MapForceMapping mapForceMapping, Dictionary<string, string> edges, TargetElementStore targetElementStore)
        {
            foreach (FunctionComponent functionComponent in mapForceMapping.FunctionComponents)
            {
                switch (functionComponent.FunctionType)
                {
                    case "split":
                        {
                            List<object> targetCcElements = new List<object>();

                            foreach (InputOutputKey outputKey in functionComponent.OutputKeys)
                            {
                                string targetElementKey;

                                if (edges.TryGetValue(outputKey.Value, out targetElementKey))
                                {
                                    targetCcElements.Add(targetElementStore.GetTargetCc(targetElementKey));
                                }
                            }

                            MappingFunction mappingFunction = new MappingFunction(targetCcElements);

                            foreach (InputOutputKey inputKey in functionComponent.InputKeys)
                            {
                                mappingFunctions[inputKey.Value] = mappingFunction;
                            }
                        }
                        break;
                }
            }     
        }

        public MappingFunction GetMappingFunction(string mappingFunctionKey)
        {
            MappingFunction mappingFunction;
            if (mappingFunctions.TryGetValue(mappingFunctionKey, out mappingFunction))
            {
                return mappingFunction;
            }
            return null;
        }
    }
}