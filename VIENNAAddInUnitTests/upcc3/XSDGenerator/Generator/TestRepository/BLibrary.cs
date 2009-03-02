using System;
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class BLibrary : UpccLibrary
    {
        private List<UpccLibrary> libraries = new List<UpccLibrary>();

        public List<UpccLibrary> Libraries
        {
            get { return libraries; }
            set { libraries = value; }
        }

        public override string GetStereotype()
        {
            return "bLibrary";
        }

        public override List<UpccLibrary> GetLibraries()
        {
            return Libraries;
        }

        public override List<UpccClass> GetClasses()
        {
            return new List<UpccClass>();
        }

        private UpccLibrary LibraryByName(string name)
        {
            return libraries.First(l => l.Name == name);
        }

        public override UpccClass ResolvePath(List<string> parts)
        {
            if (parts.Count < 2)
            {
                throw new Exception("cannot resolve path");
            }
            return LibraryByName(parts[0]).ResolvePath(parts.GetRange(1, parts.Count - 1));
        }
    }
}