// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

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

        public T Resolve<T>(Repository repository) where T : class
        {
            if (Length == 0)
            {
                return default(T);
            }
            foreach (Package model in repository.Models)
            {
                foreach (Package package in model.Packages)
                {
                    if (package.Name == FirstPart)
                    {
                        return Resolve<T>(package, Rest);
                    }
                }
            }
            return default(T);
        }

        private static T Resolve<T>(Package package, Path path) where T : class
        {
            if (package == null)
            {
                return default(T);
            }
            if (path.Length == 0)
            {
                return package as T;
            }
            string firstPart = path.FirstPart;
            if (package.Element.Stereotype == "bLibrary")
            {
                return Resolve<T>(package.PackageByName(firstPart), path.Rest);
            }
            return package.ElementByName(firstPart) as T;
        }


    }
}