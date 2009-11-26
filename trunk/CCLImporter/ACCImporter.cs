using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CCLImporter
{
    public static class ACCImporter
    {
        public static void ImportACCs(ICcLibrary ccLibrary, IEnumerable<AccSpec> accSpecs)
        {
            // need two passes:
            //  (1) create the ACCs
            //  (2) create the ASCCs
            var accs = new Dictionary<string, IAcc>();
            foreach (AccSpec spec in accSpecs)
            {
                Console.WriteLine("INFO: Importing ACC " + spec.Name + ".");
                var specWithoutASCCs = new AccSpec
                                       {
                                           BusinessTerms = spec.BusinessTerms,
                                           Definition = spec.Definition,
                                           DictionaryEntryName = spec.DictionaryEntryName,
                                           IsEquivalentTo = spec.IsEquivalentTo,
                                           LanguageCode = spec.LanguageCode,
                                           Name = spec.Name,
                                           UniqueIdentifier = spec.UniqueIdentifier,
                                           UsageRules = spec.UsageRules,
                                           VersionIdentifier = spec.VersionIdentifier
                                       };
                foreach (BccSpec bccSpec in spec.BCCs)
                {
                    specWithoutASCCs.AddBCC(bccSpec);
                }
                accs[spec.Name] = ccLibrary.CreateElement(specWithoutASCCs);
            }
            foreach (AccSpec spec in accSpecs)
            {
                Console.WriteLine("INFO: Importing ASCCs for ACC " + spec.Name + ".");
                IAcc acc = accs[spec.Name];
                ccLibrary.UpdateElement(acc, spec);
            }
        }
    }
}