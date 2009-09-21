using System;
using System.Linq;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class MappingImporter
    {
        private readonly string mapForceMappingFile;
        private readonly string docLibraryName;
        private readonly string bieLibraryName;
        private readonly string bdtLibraryName;
        private readonly string qualifier;

        /// <summary>
        /// </summary>
        /// <param name="mapForceMappingFile">The MapForce mapping file.</param>
        /// <param name="docLibraryName">The name of the DOCLibrary to be created.</param>
        /// <param name="bieLibraryName">The name of the BIELibrary to be created.</param>
        /// <param name="bdtLibraryName">The name of the BDTLibrary to be created.</param>
        /// <param name="qualifier">The qualifier for the business domain (e.g. "ebInterface").</param>
        public MappingImporter(string mapForceMappingFile, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier)
        {
            this.mapForceMappingFile = mapForceMappingFile;
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
        }

        /// <summary>
        /// We currently assume that there is exactly one CCLibrary in the repository. If none is found, we throw an exception.
        /// If more than one are found, we choose one arbitrarily.
        /// 
        /// The newly generated libraries (BDT, BIE, DOC) will be added to the CCLibrary's parent bLibrary.
        /// </summary>
        /// <param name="ccRepository"></param>
        public void ImportMapping(CCRepository ccRepository)
        {
            var ccLibrary = ccRepository.Libraries<ICCLibrary>().FirstOrDefault();
            if (ccLibrary == null)
            {
                throw new Exception("No CCLibary found in repository.");
            }
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mapForceMappingFile);
            var mappings = new Mappings(mapForceMapping, ccLibrary);
            var mappedLibraryGenerator = new MappedLibraryGenerator(ccLibrary.Parent, mappings);
            mappedLibraryGenerator.GenerateLibraries(docLibraryName, bieLibraryName, bdtLibraryName, qualifier);
        }
    }
}