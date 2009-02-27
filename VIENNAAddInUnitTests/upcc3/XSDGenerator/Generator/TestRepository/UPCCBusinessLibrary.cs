using System;
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccPackage : UpccElement
    {
        protected UpccPackage(string name) : base(name)
        {
            Classes = new List<UpccClass>();
            Libraries = new List<UpccPackage>();
        }

        public string BaseURN
        {
            set { TaggedValues.Add(new UpccTaggedValue("baseURN", value)); }
        }

        public List<UpccPackage> Libraries { get; set; }

        public List<UpccClass> Classes { get; set; }

        protected UpccPackage LibraryByName(string name)
        {
            return Libraries.First(l => l.Name == name);
        }

        protected UpccClass ClassByName(string name)
        {
            return Classes.First(e => e.Name == name);
        }

        public UpccClass ResolvePath(List<string> parts)
        {
            if (parts.Count == 0)
            {
                throw new Exception("cannot resolve path");
            }
            if (parts.Count == 1)
            {
                return ClassByName(parts[0]);
            }
            return LibraryByName(parts[0]).ResolvePath(parts.GetRange(1, parts.Count - 1));
        }

        protected void AddClasses<T>(List<T> classes) where T:UpccClass
        {
            Classes.AddRange(classes.ConvertAll(c => (UpccClass) c));
        }
    }
}