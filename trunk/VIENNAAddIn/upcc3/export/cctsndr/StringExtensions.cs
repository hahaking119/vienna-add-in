// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Text;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    public static class StringExtensions
    {
        public static string Minus(this string str, string suffix)
        {
            return str.Substring(0, str.Length - suffix.Length);
        }

        public static string Times(this string str, int times)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < times; i++)
            {
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }

        public static string DefaultTo(this string str, string defaultValue)
        {
            return String.IsNullOrEmpty(str) ? defaultValue : str;
        }

        public static string ToXmlName(this string n)
        {
            if (String.IsNullOrEmpty(n))
                return "";
            n = n.Replace(' ', '_');
            n = n.Replace('/', '_');
            return n;
        }
    }
}