using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public interface ISourceElementTree
    {
        SourceElement GetSourceElement(string key);
        SourceElement RootSourceElement { get; }
    }

    public class MapForceSourceElementTree: ISourceElementTree
    {
        private readonly Dictionary<string, SourceElement> sourceElementsByKey = new Dictionary<string, SourceElement>();

        /// <exception cref="ArgumentException">The MapForce mapping does not contain any input schema components.</exception>
        public MapForceSourceElementTree(MapForceMapping mapping)
        {
            /// Retrieve the schema component containing the input schemas' root element.
            /// 
            /// If there is only one input schema component, then this component is the root schema component.
            /// 
            /// If there are more than one input schema components, we look for a constant component with value "Root: *", where "*" must
            /// be the name of the root XSD element of the input schemas. We then return the schema component containing this element as its root.

            var inputSchemaComponents = new List<SchemaComponent>(mapping.GetInputSchemaComponents());
            if (inputSchemaComponents.Count == 0)
            {
                throw new ArgumentException("The MapForce mapping does not contain any input schema components.");
            }
            if (inputSchemaComponents.Count == 1)
            {
                RootSourceElement = CreateSourceElementTree(inputSchemaComponents[0].RootEntry);
            } else
            {
                var rootElementName = mapping.GetConstant("Root");
                if (string.IsNullOrEmpty(rootElementName))
                {
                    throw  new ArgumentException("The MapForce mapping does not specify the root source element name.");
                }
                var nonRootElementTrees = new List<SourceElement>();
                foreach (SchemaComponent inputSchemaComponent in inputSchemaComponents)
                {
                    if (inputSchemaComponent.RootEntry.Name == rootElementName)
                    {
                        RootSourceElement = CreateSourceElementTree(inputSchemaComponent.RootEntry);
                    } else
                    {
                        nonRootElementTrees.Add(CreateSourceElementTree(inputSchemaComponent.RootEntry));
                    }
                }
                if (RootSourceElement == null)
                {
                    throw new ArgumentException("The MapForce mapping does not contain an input schema component with the specified root element: " + rootElementName);
                }
                foreach (var nonRootElementTree in nonRootElementTrees)
                {
                    AttachSubTree(nonRootElementTree, RootSourceElement);
                }
            }
        }

        /// <summary>
        /// Perform name matching between the sub-tree's root element name and element names in the sourceElementTree.
        /// 
        /// When a matching element is found, attach the sub-tree root's children to the element in the source tree.
        /// </summary>
        /// <param name="subTreeRoot"></param>
        /// <param name="sourceElementTree"></param>
        private static void AttachSubTree(SourceElement subTreeRoot, SourceElement sourceElementTree)
        {
            if (subTreeRoot.Name == sourceElementTree.Name)
            {
                foreach (var child in subTreeRoot.Children)
                {
                    sourceElementTree.AddChild(child);
                }
            }
            foreach (var child in sourceElementTree.Children)
            {
                AttachSubTree(subTreeRoot, child);
            }
        }

        public SourceElement RootSourceElement { get; private set; }

        private SourceElement CreateSourceElementTree(Entry entry)
        {
            var sourceElement = new SourceElement(entry.Name, entry.InputOutputKey.Value);
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