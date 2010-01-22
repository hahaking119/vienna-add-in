using System.Collections.Generic;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class TargetElementStore
    {
        private readonly Dictionary<string, object> targetCsByKey = new Dictionary<string, object>();

        public TargetElementStore(MapForceMapping mapForceMapping, ICcLibrary ccLibrary)
        {
            IEnumerable<SchemaComponent> targetSchemaComponents = mapForceMapping.GetTargetSchemaComponents();
            foreach (SchemaComponent component in targetSchemaComponents)
            {
                Entry entry = component.RootEntry;
                IAcc acc = ccLibrary.GetAccByName(entry.Name);
                if (acc == null)
                {
                    throw new MappingError("ACC '" + entry.Name + "' not found.");
                }
                AddToIndex(entry, acc);
                CreateChildren(entry, acc);
            }
        }

        /// <exception cref="MappingError"><c>MappingError</c>.</exception>
        private void CreateChildren(Entry entry, IAcc acc)
        {
            foreach (Entry subEntry in entry.SubEntries)
            {
                IBcc bcc = GetBcc(acc, subEntry.Name);
                if (bcc != null)
                {
                    AddToIndex(subEntry, bcc);
                    CreateChildren(subEntry, bcc.Cdt);
                }
                else
                {
                    IAscc ascc = GetAscc(acc, subEntry.Name);
                    if (ascc != null)
                    {
                        AddToIndex(subEntry, ascc);
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
            foreach (Entry subEntry in entry.SubEntries)
            {
                ICdtSup sup = GetSup(cdt, subEntry.Name);

                if (sup != null)
                {
                    AddToIndex(subEntry, sup);
                }
                else
                {
                    throw new MappingError("SUP '" + subEntry.Name + "' not found.");
                }
            }
        }

        private static IBcc GetBcc(IAcc acc, string name)
        {
            foreach (IBcc bcc in acc.Bccs)
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
            foreach (ICdtSup sup in cdt.Sups)
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
            foreach (IAscc ascc in acc.Asccs)
            {
                if (name == NDR.GenerateASCCName(ascc))
                {
                    return ascc;
                }
            }
            return null;
        }

        private void AddToIndex(Entry entry, object targetCc)
        {
            string key = entry.InputOutputKey.Value;
            if (key != null)
            {
                targetCsByKey[key] = targetCc;
            }
        }

        public object GetTargetCc(string key)
        {
            object targetCc;
            targetCsByKey.TryGetValue(key, out targetCc);
            return targetCc;
        }
    }
}