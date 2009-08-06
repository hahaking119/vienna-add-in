using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
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

        private bool IsInputSchema(SchemaComponent component)
        {
            return IsInput(component.RootEntry);
        }

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

        private bool IsOutputSchema(SchemaComponent component)
        {
            return IsOutput(component.RootEntry);
        }

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

        public SchemaComponent GetTargetSchemaComponent()
        {
            var outputSchemas = new List<SchemaComponent>();
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                if (IsOutputSchema(schemaComponent))
                {
                    outputSchemas.Add(schemaComponent);
                }
            }
            if (outputSchemas.Count > 0)
            {
                return outputSchemas[0];
            }
            // TODO error
            return null;
        }
    }
}