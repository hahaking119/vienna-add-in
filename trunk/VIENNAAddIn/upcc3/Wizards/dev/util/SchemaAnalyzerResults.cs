using System.Collections.Generic;
namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    class SchemaAnalyzerResults
    {
        private List<SchemaAnalyzerResult> items;
        public List<SchemaAnalyzerResult> Items
        {
            get
            {
                return items;
            }
        }

        public SchemaAnalyzerResults()
        {
            items = new List<SchemaAnalyzerResult>();
        }

        public SchemaAnalyzerResults(SchemaAnalyzerResult newItem)
        {
            items = new List<SchemaAnalyzerResult>();
            Add(newItem);
        }

        public SchemaAnalyzerResults Add(SchemaAnalyzerResult newItem)
        {
            items.Add(newItem);
            return this;
        }
    }

    public class SchemaAnalyzerResult
    {
        public string Caption { get; set; }
        public int Count { get; set; }

        public SchemaAnalyzerResult(string newCaption, int newCount)
        {
            Caption = newCaption;
            Count = newCount;
        }
    }
}
