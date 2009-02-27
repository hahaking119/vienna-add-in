using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccLibrary : UpccElement
    {
        protected UpccLibrary(string name) : base(name)
        {
        }

        public string BaseURN
        {
            set { GetTaggedValues().Add(new UpccTaggedValue("baseURN", value)); }
        }

        public abstract List<UpccLibrary> GetLibraries();
        public abstract List<UpccClass> GetClasses();

        public abstract UpccClass ResolvePath(List<string> parts);
    }
}