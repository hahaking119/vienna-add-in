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
        private readonly Dictionary<string, TargetCcElement> targetCCElementsByKey = new Dictionary<string, TargetCcElement>();

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
                    CreateTargetCCElementForAcc(entry, acc);
                    CreateChildren(entry, acc);
                }
            }
        }

        /// <exception cref="MappingError"><c>MappingError</c>.</exception>
        private void CreateChildren(Entry entry, IAcc acc)
        {
            foreach (var subEntry in entry.SubEntries)
            {
                var bcc = GetBcc(acc, subEntry.Name);
                if (bcc != null)
                {
                    CreateTargetCCElementForBcc(subEntry, bcc);
                    CreateChildren(subEntry, bcc.Cdt);
                }
                else
                {
                    var ascc = GetAscc(acc, subEntry.Name);
                    if (ascc != null)
                    {
                        CreateTargetCCElementForAscc(subEntry, ascc);
                        CreateChildren(subEntry, ascc.AssociatedAcc);
                    }
                    else
                    {
                        throw new MappingError("BCC or ASCC '" + subEntry.Name + "' not found.");
                    }
                }
            }
        }

        private void CreateChildren(Entry entry, ICdt cdt)
        {
            foreach (var subEntry in entry.SubEntries)
            {
                var sup = GetSup(cdt, subEntry.Name);

                if (sup != null)
                {
                    CreateTargetCcElementForSup(subEntry, sup);
                }
                else
                {
                    throw new MappingError("SUP '" + subEntry.Name + "' not found.");
                }
            }
        }

        private void CreateTargetCCElementForBcc(Entry entry, IBcc bcc)
        {
            var targetCCElement = TargetCcElement.ForBcc(bcc);
            AddToIndex(entry, targetCCElement);
        }

        private void CreateTargetCCElementForAscc(Entry entry, IAscc ascc)
        {
            var targetCCElement = TargetCcElement.ForAscc(ascc);
            AddToIndex(entry, targetCCElement);
        }

        private void CreateTargetCCElementForAcc(Entry entry, IAcc acc)
        {
            var targetCCElement = TargetCcElement.ForAcc(acc);
            AddToIndex(entry, targetCCElement);
        }

        private void CreateTargetCcElementForSup(Entry entry, ICdtSup sup)
        {
            var targetCCElement = TargetCcElement.ForSup(sup);
            AddToIndex(entry, targetCCElement);
        }


        private IAcc GetACC(SchemaComponent component)
        {
            return ccLibrary.GetAccByName(component.RootEntry.Name);
        }

        private static IBcc GetBcc(IAcc acc, string name)
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

        private static ICdtSup GetSup(ICdt cdt, string name)
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


        private static IAscc GetAscc(IAcc acc, string name)
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

        private void AddToIndex(Entry entry, TargetCcElement targetCCElement)
        {
            var key = entry.InputOutputKey.Value;
            if (key != null)
            {
                targetCCElementsByKey[key] = targetCCElement;
            }
        }

        public TargetCcElement GetTargetElement(string key)
        {
            TargetCcElement targetCCElement;
            targetCCElementsByKey.TryGetValue(key, out targetCCElement);
            return targetCCElement;
        }
    }
}