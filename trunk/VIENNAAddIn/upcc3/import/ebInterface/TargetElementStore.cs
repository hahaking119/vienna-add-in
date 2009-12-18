using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

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
        private readonly ICcLibrary ccLibrary;
        private readonly Dictionary<string, TargetCCElement> targetCCElementsByKey = new Dictionary<string, TargetCCElement>();

        public TargetElementStore(MapForceMapping mapForceMapping, ICcLibrary ccLibrary)
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
                    var rootTargetCCElement = CreateTargetCCElementForAcc(entry, acc);
                    CreateChildren(rootTargetCCElement, entry, acc);
                }
            }
        }

        /// <exception cref="MappingError"><c>MappingError</c>.</exception>
        private void CreateChildren(TargetCCElement targetCCElement, Entry entry, IAcc acc)
        {
            foreach (var subEntry in entry.SubEntries)
            {
                var bcc = GetBCC(acc, subEntry.Name);
                if (bcc != null)
                {
                    var targetBccElement = CreateTargetCCElementForBcc(subEntry, bcc);
                    CreateChildren(targetBccElement, subEntry, bcc.Cdt);
                    targetCCElement.AddChild(targetBccElement);
                }
                else
                {
                    var ascc = GetASCC(acc, subEntry.Name);
                    if (ascc != null)
                    {
                        var targetASCCElement = CreateTargetCCElementForAscc(subEntry, ascc);
                        CreateChildren(targetASCCElement, subEntry, ascc.AssociatedAcc);
                        targetCCElement.AddChild(targetASCCElement);
                    }
                    else
                    {
                        throw new MappingError("BCC or ASCC '" + subEntry.Name + "' not found.");
                    }
                }
            }
        }

        private void CreateChildren(TargetCCElement targetCCElement, Entry entry, ICdt cdt)
        {
            foreach (var subEntry in entry.SubEntries)
            {
                var sup = GetSup(cdt, subEntry.Name);

                if (sup != null)
                {
                    targetCCElement.AddChild(CreateTargetCcElementForSup(subEntry, sup));
                }
                else
                {
                    throw new MappingError("SUP '" + subEntry.Name + "' not found.");
                }
            }
        }

        private TargetCCElement CreateTargetCCElementForBcc(Entry entry, IBcc bcc)
        {
            var targetCCElement = TargetCCElement.ForBcc(entry.Name, bcc);
            AddToIndex(entry, targetCCElement);
            return targetCCElement;
        }

        private TargetCCElement CreateTargetCCElementForAscc(Entry entry, IAscc ascc)
        {
            var targetCCElement = TargetCCElement.ForAscc(entry.Name, ascc);
            AddToIndex(entry, targetCCElement);
            return targetCCElement;
        }

        private TargetCCElement CreateTargetCCElementForAcc(Entry entry, IAcc acc)
        {
            var targetCCElement = TargetCCElement.ForAcc(entry.Name, acc);
            AddToIndex(entry, targetCCElement);
            return targetCCElement;
        }

        private TargetCCElement CreateTargetCcElementForSup(Entry entry, ICdtSup sup)
        {
            var targetCCElement = TargetCCElement.ForSup(entry.Name, sup);
            AddToIndex(entry, targetCCElement);
            return targetCCElement;
        }


        private IAcc GetACC(SchemaComponent component)
        {
            return ccLibrary.GetAccByName(component.RootEntry.Name);
        }

        private static IBcc GetBCC(IAcc acc, string name)
        {
            foreach (var bcc in acc.Bccs)
            {
                if (name == NDR.GenerateBCCName(bcc))
                {
                    return bcc;
                }
            }
            return null;
        }

        private ICdtSup GetSup(ICdt cdt, string name)
        {
            foreach (var sup in cdt.Sups)
            {
                if (name == NDR.GetXsdAttributeNameFromSup(sup))
                {
                    return sup;
                }
            }
            return null;
        }


        private static IAscc GetASCC(IAcc acc, string name)
        {
            foreach (var ascc in acc.Asccs)
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