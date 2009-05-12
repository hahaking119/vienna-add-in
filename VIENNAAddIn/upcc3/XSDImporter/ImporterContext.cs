﻿using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter
{
    public class ImporterContext
    {
        #region Fields

        private readonly List<SchemaInfo> schemas = new List<SchemaInfo>();

        #endregion

        #region Constructor

        public ImporterContext(ICCRepository ccRepository, List<SchemaInfo> schemaFiles)
        {
            Repository = ccRepository;
            schemas = schemaFiles;
        }

        #endregion

        #region Properties

        ///<summary>
        ///</summary>
        public ICCRepository Repository { get; private set; }

        ///<summary>
        ///</summary>
        public List<SchemaInfo> Schemas
        {
            get { return schemas; }
        }

        #endregion
    }
}
