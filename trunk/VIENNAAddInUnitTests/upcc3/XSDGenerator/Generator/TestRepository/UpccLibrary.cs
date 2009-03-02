using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public abstract class UpccLibrary : UpccElement
    {
        public string BaseURN
        {
            set { AddTaggedValue("baseURN", value); }
        }

        public abstract List<UpccLibrary> GetLibraries();
        public abstract List<UpccClass> GetClasses();
        public abstract UpccClass ResolvePath(List<string> parts);
    }
}