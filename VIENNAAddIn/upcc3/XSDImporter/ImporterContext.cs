using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter
{
    ///<summary>
    ///</summary>
    public class ImporterContext
    {
        #region Fields

        private readonly List<SchemaInfo> schemas = new List<SchemaInfo>();

        #endregion

        #region Constructor

        ///<summary>
        ///</summary>
        ///<param name="ccRepository"></param>
        ///<param name="inputDirectory"></param>
        ///<param name="schemaFiles"></param>
        public ImporterContext(ICCRepository ccRepository, string inputDirectory, List<SchemaInfo> schemaFiles)
        {
            Repository = ccRepository;
            InputDirectory = inputDirectory;
            schemas = schemaFiles;
            Root = null;
        }

        public ImporterContext(ICCRepository ccRepository, string inputDirectory, List<SchemaInfo> schemaFiles, SchemaInfo root)
        {
            InputDirectory = inputDirectory;
            Repository = ccRepository;
            schemas = schemaFiles;
            Root = root;
        }

        #endregion

        #region Properties

        ///<summary>
        ///</summary>
        public ICCRepository Repository { get; private set; }

        ///<summary>
        ///</summary>
        public string InputDirectory { get; private set; }

        ///<summary>
        ///</summary>
        public List<SchemaInfo> Schemas
        {
            get { return schemas; }
        }

        public SchemaInfo Root { get; private set; }

        #endregion
    }
}
