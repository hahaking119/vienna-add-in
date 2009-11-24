using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    /// <summary>
    /// Builds a target CC element hierarchy for each target component (i.e. for each ACC).
    /// Target CC elements can be retrieved via their input/output key.
    /// 
    /// Note that the tree depth is currently limited to 2 (meaning that we do not resolve ASCCs within the ACC).
    /// </summary>
    public class TargetElementStore
    {
        private readonly ICCLibrary ccLibrary;
        private readonly Dictionary<string, TargetCCElement> targetCCElementsByKey = new Dictionary<string, TargetCCElement>();

        public TargetElementStore(MapForceMapping mapForceMapping, ICCLibrary ccLibrary)
        {
            this.ccLibrary = ccLibrary;
            var targetSchemaComponents = mapForceMapping.GetTargetSchemaComponents();
            foreach (var component in targetSchemaComponents)
            {
                var acc = GetACC(component);
                var entry = component.RootEntry;
                if (acc == null)
                {
                    Console.WriteLine("ERROR: ACC '" + entry.Name + "' not found.");
                    // TODO error
                }
                else
                {
                    var rootTargetCCElement = CreateTargetCCElement(entry, acc);
                    CreateChildren(rootTargetCCElement, entry, acc);
                }
            }
        }

        /// <exception cref="MappingError"><c>MappingError</c>.</exception>
        private void CreateChildren(TargetCCElement targetCCElement, Entry entry, IACC acc)
        {
            foreach (var subEntry in entry.SubEntries)
            {
                var bcc = GetBCC(acc, subEntry.Name);
                if (bcc != null)
                {
                    targetCCElement.AddChild(CreateTargetCCElement(subEntry, bcc));
                }
                else
                {
                    var ascc = GetASCC(acc, subEntry.Name);
                    if (ascc != null)
                    {
                        var targetASCCElement = CreateTargetCCElement(subEntry, ascc);
                        CreateChildren(targetASCCElement, subEntry, ascc.AssociatedElement);
                        targetCCElement.AddChild(targetASCCElement);
                    }
                    else
                    {
                        throw new MappingError("BCC or ASCC '" + subEntry.Name + "' not found.");
                    }
                }
            }
        }

        private TargetCCElement CreateTargetCCElement(Entry entry, ICC cc)
        {
            var targetCCElement = new TargetCCElement(entry.Name, cc);
            AddToIndex(entry, targetCCElement);
            return targetCCElement;
        }

        private IACC GetACC(SchemaComponent component)
        {
            return ccLibrary.ElementByName(component.RootEntry.Name);
        }

        private static IBCC GetBCC(IACC acc, string name)
        {
            foreach (var bcc in acc.BCCs)
            {
                if (name == NDR.GenerateBCCName(bcc))
                {
                    return bcc;
                }
            }
            return null;
        }

        private static IASCC GetASCC(IACC acc, string name)
        {
            foreach (var ascc in acc.ASCCs)
            {
                if (name == NDR.GenerateASCCName(ascc))
                {
                    return ascc;
                }
            }
            return null;
        }

        private void AddToIndex(Entry entry, TargetCCElement targetCCElement)
        {
            var key = entry.InputOutputKey.Value;
            if (key != null)
            {
                targetCCElementsByKey[key] = targetCCElement;
            }
        }

        public TargetCCElement GetTargetElement(string key)
        {
            TargetCCElement targetCCElement;
            targetCCElementsByKey.TryGetValue(key, out targetCCElement);
            return targetCCElement;
        }
    }
}