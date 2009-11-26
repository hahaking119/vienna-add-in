using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class MappingImporter
    {
        private readonly string[] mapForceMappingFiles;
        private readonly string docLibraryName;
        private readonly string bieLibraryName;
        private readonly string bdtLibraryName;
        private readonly string qualifier;
        private readonly string rootElementName;

        /// <summary>
        /// </summary>
        /// <param name="mapForceMappingFiles">The MapForce mapping file.</param>
        /// <param name="docLibraryName">The name of the DOCLibrary to be created.</param>
        /// <param name="bieLibraryName">The name of the BIELibrary to be created.</param>
        /// <param name="bdtLibraryName">The name of the BDTLibrary to be created.</param>
        /// <param name="qualifier">The qualifier for the business domain (e.g. "ebInterface").</param>
        public MappingImporter(IEnumerable<string> mapForceMappingFiles, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier, string rootElementName)
        {
            this.mapForceMappingFiles = new List<string>(mapForceMappingFiles).ToArray();
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
            this.rootElementName = rootElementName;
        }

        /// <summary>
        /// We currently assume that there is exactly one CCLibrary in the repository. If none is found, we throw an exception.
        /// If more than one are found, we choose one arbitrarily.
        /// 
        /// The newly generated libraries (BDT, BIE, DOC) will be added to the CCLibrary's parent bLibrary.
        /// </summary>
        /// <param name="cctsRepository"></param>
        public void ImportMapping(ICctsRepository cctsRepository)
        {
            var ccLibrary = cctsRepository.GetCcLibraries().FirstOrDefault();
            if (ccLibrary == null)
            {
                throw new Exception("No CCLibary found in repository.");
            }
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mapForceMappingFiles);
            var mappings = new SchemaMapping(mapForceMapping, ccLibrary);
            var mappedLibraryGenerator = new MappedLibraryGenerator(mappings, ccLibrary.Parent, docLibraryName, bieLibraryName, bdtLibraryName, qualifier, rootElementName);
            mappedLibraryGenerator.GenerateLibraries();
        }
    }
}