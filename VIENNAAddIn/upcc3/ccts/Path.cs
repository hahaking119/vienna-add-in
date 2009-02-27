using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class Path
    {
        public static readonly Path EmptyPath = new Path();

        private readonly List<string> parts = new List<string>();

        public Path()
        {
        }

        public Path(string firstPart) : this()
        {
            parts.Add(firstPart);
        }

        public Path(IEnumerable<string> parts) : this()
        {
            this.parts.AddRange(parts);
        }

        public string FirstPart
        {
            get { return parts[0]; }
        }

        public Path Rest
        {
            get
            {
                return Length < 2 ? EmptyPath : new Path(parts.GetRange(1, Length - 1));
            }
        }

        public int Length
        {
            get { return parts.Count; }
        }

        public Path Append(string part)
        {
            parts.Add(part);
            return this;
        }

        public static Path operator /(Path lhs, string rhs)
        {
            return lhs.Append(rhs);
        }

        public static explicit operator Path(string firstPart)
        {
            return new Path(firstPart);
        }
    }
}