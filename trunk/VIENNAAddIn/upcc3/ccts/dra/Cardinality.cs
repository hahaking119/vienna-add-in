using System;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class Cardinality
    {
        public Cardinality(string lowerBound, string upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public Cardinality(string cardinality)
        {
            string[] parts = cardinality.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            switch (parts.Length)
            {
                case 1:
                    LowerBound = (parts[0] == "*" ? "0" : parts[0]);
                    UpperBound = parts[0];
                    break;
                case 2:
                    LowerBound = parts[0];
                    UpperBound = parts[1];
                    break;
                default:
                    LowerBound = "1";
                    UpperBound = "1";
                    break;
            }
        }

        public string LowerBound { get; private set; }
        public string UpperBound { get; private set; }
    }
}