// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class TypeBasedSelector<TResult>
    {
        private readonly List<ISelectorCase<TResult>> cases = new List<ISelectorCase<TResult>>();
        private Func<object, TResult> defaultAction;

        public void Default(Func<object, TResult> defaultAction)
        {
            this.defaultAction = defaultAction;
        }

        public void Case<T>(Func<T, TResult> action)
        {
            cases.Add(new SelectorCase<T, TResult>(action));
        }

        public TResult Execute(object o)
        {
            foreach (var selectorCase in cases)
            {
                if (selectorCase.Matches(o))
                {
                    return selectorCase.Execute(o);
                }
            }
            if (defaultAction != null)
            {
                return defaultAction(o);
            }
            throw new ArgumentException("no match found and no default action specified");
        }

        #region Nested type: ISelectorCase

        internal interface ISelectorCase<SResult>
        {
            bool Matches(Object o);
            SResult Execute(Object o);
        }

        #endregion

        #region Nested type: SelectorCase

        internal class SelectorCase<S, SResult> : ISelectorCase<SResult>
        {
            private readonly Func<S, SResult> action;

            public SelectorCase(Func<S, SResult> action)
            {
                this.action = action;
            }

            #region ISelectorCase<SResult> Members

            public bool Matches(Object o)
            {
                return o is S;
            }

            public SResult Execute(Object o)
            {
                return action((S) o);
            }

            #endregion
        }

        #endregion
    }
}