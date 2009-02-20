using System;
using System.Diagnostics;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public abstract class AbstractLibraryGenerator : ILibraryGenerator
    {
        protected GenerationContext Context { get; private set; }

        protected AbstractLibraryGenerator(ccts.BusinessLibraryType businessLibraryType, GenerationContext context)
        {
            Context = context;
            BusinessLibraryType = businessLibraryType;
        }

        #region ILibraryGenerator Members

        public BusinessLibraryType BusinessLibraryType { get; private set; }

        #endregion
    }
}