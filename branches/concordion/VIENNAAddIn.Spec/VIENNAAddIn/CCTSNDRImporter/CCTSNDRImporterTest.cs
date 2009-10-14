using Concordion.Integration;

namespace VIENNAAddIn.Spec.VIENNAAddIn.CCTSNDRImporter
{
    [ConcordionTest]
    public class CCTSNDRImporterTest
    {
        public long firstOperand;
        public long secondOperand;

        public long Addition(long first, long second)
        {
            return -1;
        }

        public long Subtraction(long first, long second)
        {
            return -1;
        }

        public long Multiplication(long first, long second)
        {
            return first * second;
        }

        public long Division(long first, long second)
        {
            return -1;
        }
    }
}