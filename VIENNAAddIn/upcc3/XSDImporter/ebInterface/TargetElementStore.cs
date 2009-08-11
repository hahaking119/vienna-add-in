using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
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
                if (acc == null)
                {
                    Console.WriteLine("ERROR: ACC '" + component.RootEntry.Name + "' not found.");
                    // TODO error
                }
                else
                {
                    CreateTargetElementTree(acc, component.RootEntry, true);
                }
            }
        }
        private IACC GetACC(SchemaComponent component)
        {
            return ccLibrary.ElementByName(component.RootEntry.Name);
        }

        private static ICC GetBCCOrASCC(IACC acc, string name)
        {
            var bccsAndASCCsByName = new Dictionary<string, ICC>();
            foreach (var bcc in acc.BCCs)
            {
                bccsAndASCCsByName[NDR.GenerateBCCName(bcc)] = bcc;
            }
            foreach (var ascc in acc.ASCCs)
            {
                bccsAndASCCsByName[NDR.GenerateASCCName(ascc)] = ascc;
            }
            return bccsAndASCCsByName[name];
        }

        private TargetCCElement CreateTargetElementTree(IACC acc, Entry entry, bool isRoot)
        {
            ICC reference = isRoot ? acc : GetBCCOrASCC(acc, entry.Name);
            var targetCCElement = new TargetCCElement(entry.Name, reference);
            AddToIndex(entry, targetCCElement);
            foreach (var subEntry in entry.SubEntries)
            {
                targetCCElement.AddChild(CreateTargetElementTree(acc, subEntry, false));
            }
            return targetCCElement;
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