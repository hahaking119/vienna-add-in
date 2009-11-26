using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class SourceElementStore
    {
        private readonly Dictionary<string, SourceElement> sourceElementsByKey = new Dictionary<string, SourceElement>();

        public SourceElementStore(MapForceMapping mapping)
        {
            RootSourceElement = CreateSourceElementTree(mapping.GetRootSchemaComponent().RootEntry);
        }

        public SourceElement RootSourceElement { get; private set; }

        private SourceElement CreateSourceElementTree(Entry entry)
        {
            var sourceElement = new SourceElement(entry.Name);
            AddToIndex(entry, sourceElement);
            foreach (var subEntry in entry.SubEntries)
            {
                sourceElement.AddChild(CreateSourceElementTree(subEntry));
            }
            return sourceElement;
        }

        private void AddToIndex(Entry entry, SourceElement sourceElement)
        {
            var key = entry.InputOutputKey.Value;
            if (key != null)
            {
                sourceElementsByKey[key] = sourceElement;
            }
        }

        public SourceElement GetSourceElement(string key)
        {
            SourceElement element;
            sourceElementsByKey.TryGetValue(key, out element);
            return element;
        }
    }
}