using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    /// <summary>
    /// Basically represents a MapForce mapping attribute of the form:
    /// <mapping version="11">
    /// with all necessary information used for the import of an XML Schema document model.
    /// </summary>
    public class MapForceMapping : IEquatable<MapForceMapping>
    {
        public Graph Graph { get; private set; }

        public MapForceMapping(IEnumerable<SchemaComponent> schemaComponents, IEnumerable<ConstantComponent> constantComponents, Graph graph)
        {
            Graph = graph;
            SchemaComponents = new List<SchemaComponent>(schemaComponents);
            ConstantComponents = new List<ConstantComponent>(constantComponents);
        }

        public IEnumerable<ConstantComponent> ConstantComponents { get; private set; }
        public IEnumerable<SchemaComponent> SchemaComponents { get; private set; }

        #region IEquatable<MapForceMapping> Members

        public bool Equals(MapForceMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || (other.SchemaComponents.IsEqualTo(SchemaComponents) && other.ConstantComponents.IsEqualTo(ConstantComponents));
        }

        #endregion

        public void PrettyPrint(TextWriter writer, string indent)
        {
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                schemaComponent.PrettyPrint(writer, indent);
            }
            foreach (var constantComponent in ConstantComponents)
            {
                constantComponent.PrettyPrint(writer, indent);
            }
            Graph.PrettyPrint(writer, indent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(MapForceMapping)) return false;
            return Equals((MapForceMapping)obj);
        }

        public override int GetHashCode()
        {
            return (SchemaComponents != null ? SchemaComponents.GetHashCode() : 0);
        }

        public string GetConstant(string name)
        {
            foreach (ConstantComponent constantComponent in ConstantComponents)
            {
                if (constantComponent.Value.StartsWith(name + ":"))
                {
                    return constantComponent.Value.Substring(name.Length + 1).Trim();
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieve the schema component containing the input schemas' root element.
        /// 
        /// If there is only one input schema component, then this component is the root schema component.
        /// 
        /// If there are more than one input schema components, we look for a constant component with value "Root: *", where "*" must
        /// be the name of the root XSD element of the input schemas. We then return the schema component containing this element as its root.
        /// </summary>
        /// <returns>The input schema component containing the XSD root element.</returns>
        public SchemaComponent GetRootSchemaComponent()
        {
            var inputSchemas = new List<SchemaComponent>();
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                if (IsInputSchema(schemaComponent))
                {
                    inputSchemas.Add(schemaComponent);
                }
            }
            if (inputSchemas.Count > 1)
            {
                var rootElementName = GetConstant("Root");
                if (rootElementName == null)
                {
                    // TODO error
                    return null;
                }
                foreach (SchemaComponent inputSchema in inputSchemas)
                {
                    if (inputSchema.RootEntry.Name == rootElementName)
                    {
                        return inputSchema;
                    }
                }
                // TODO error
                return null;
            }
            if (inputSchemas.Count == 1)
            {
                return inputSchemas[0];
            }
            // TODO error
            return null;
        }

        /// <summary>
        /// Takes a SchemaComponent as input and returns true if this SchemaComponent is an Input Schema, i.e. a 
        /// source Schema, and false otherwise.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private bool IsInputSchema(SchemaComponent component)
        {
            return IsInput(component.RootEntry);
        }

        /// <summary>
        /// Takes an Entry as input and returns true if this Entry or its child Entries have an OutputKey specified,
        /// false otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool IsInput(Entry entry)
        {
            if (entry.IsInput)
            {
                return true;
            }
            if (entry.IsOutput)
            {
                return false;
            }
            foreach (Entry subEntry in entry.SubEntries)
            {
                var isInput = IsInput(subEntry);
                if (isInput)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Takes a SchemaComponent as input and returns true if this SchemaComponent is an Output Schema, i.e. a 
        /// target Schema, and false otherwise.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private bool IsOutputSchema(SchemaComponent component)
        {
            return IsOutput(component.RootEntry);
        }

        /// <summary>
        /// Takes an Entry as input and returns true if this Entry or its child Entries have an InputKey specified,
        /// false otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool IsOutput(Entry entry)
        {
            if (entry.IsInput)
            {
                return false;
            }
            if (entry.IsOutput)
            {
                return true;
            }
            foreach (Entry subEntry in entry.SubEntries)
            {
                var isOutput = IsOutput(subEntry);
                if (isOutput)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a set of TargetSchemaComponents, i.e, OutputSchemas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SchemaComponent> GetTargetSchemaComponents()
        {
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                if (IsOutputSchema(schemaComponent))
                {
                    yield return schemaComponent;
                }
            }
        }
    }
}