using System;
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccClassLibrary : UpccLibrary
    {
        private readonly List<UpccClass> classes = new List<UpccClass>();

        public override List<UpccLibrary> GetLibraries()
        {
            return new List<UpccLibrary>();
        }

        public override List<UpccClass> GetClasses()
        {
            return classes;
        }

        private UpccClass ClassByName(string name)
        {
            return classes.First(e => e.Name == name);
        }

        public override UpccClass ResolvePath(List<string> parts)
        {
            if (parts.Count != 1)
            {
                throw new Exception("cannot resolve path");
            }
            return ClassByName(parts[0]);
        }

        protected void AddClasses<T>(List<T> classes) where T : UpccClass
        {
            this.classes.AddRange(classes.ConvertAll(c => (UpccClass) c));
        }
    }
}