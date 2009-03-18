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
    public class Selector<T>
    {
        private readonly Action<T> defaultAction;
        private readonly bool stopAfterFirstMatch;
        private readonly List<CaseSpec> caseSpecs = new List<CaseSpec>();

        public Selector(Action<T> defaultAction, bool stopAfterFirstMatch)
        {
            this.defaultAction = defaultAction;
            this.stopAfterFirstMatch = stopAfterFirstMatch;
        }

        public void If(Predicate<T> predicate, Action<T> action)
        {
            caseSpecs.Add(new CaseSpec(predicate, action));
        }

        public void Execute(T obj)
        {
            foreach (var caseSpec in caseSpecs)
            {
                if (caseSpec.CheckAndExecute(obj) && stopAfterFirstMatch)
                {
                    return;
                }
            }
            if (defaultAction != null)
            {
                defaultAction(obj);
            }
        }

        private class CaseSpec
        {
            private readonly Predicate<T> predicate;
            private readonly Action<T> action;

            public CaseSpec(Predicate<T> predicate, Action<T> action)
            {
                this.predicate = predicate;
                this.action = action;
            }

            public bool CheckAndExecute(T obj)
            {
                if (predicate(obj))
                {
                    action(obj);
                    return true;
                }
                return false;
            }
        }
    }

    public delegate bool Predicate<T1, T2>(T1 obj1, T2 obj2);

    public class Selector<T1, T2>
    {
        private readonly Action<T1, T2> defaultAction;
        private readonly bool stopAfterFirstMatch;
        private readonly List<CaseSpec> caseSpecs = new List<CaseSpec>();

        public Selector(Action<T1, T2> defaultAction, bool stopAfterFirstMatch)
        {
            this.defaultAction = defaultAction;
            this.stopAfterFirstMatch = stopAfterFirstMatch;
        }

        public void Case(Predicate<T1, T2> predicate, Action<T1, T2> action)
        {
            caseSpecs.Add(new CaseSpec(predicate, action));
        }

        public void Execute(T1 obj1, T2 obj2)
        {
            foreach (var caseSpec in caseSpecs)
            {
                if (caseSpec.CheckAndExecute(obj1, obj2) && stopAfterFirstMatch)
                {
                    return;
                }
            }
            if (defaultAction != null)
            {
                defaultAction(obj1, obj2);
            }
        }

        private class CaseSpec
        {
            private readonly Predicate<T1, T2> predicate;
            private readonly Action<T1, T2> action;

            public CaseSpec(Predicate<T1, T2> predicate, Action<T1, T2> action)
            {
                this.predicate = predicate;
                this.action = action;
            }

            public bool CheckAndExecute(T1 obj1, T2 obj2)
            {
                if (predicate(obj1, obj2))
                {
                    action(obj1, obj2);
                    return true;
                }
                return false;
            }
        }
    }

//    public abstract class SelectorStrategy<T>
//    {
//        public bool StopAfterFirstMatch { get; private set; }
//        private readonly Action<T> defaultAction;
//
//        protected SelectorStrategy(bool stopAfterFirstMatch, Action<T> defaultAction)
//        {
//            StopAfterFirstMatch = stopAfterFirstMatch;
//            this.defaultAction = defaultAction;
//        }
//
//        public void executeDefaultAction(T obj)
//        {
//            if (defaultAction != null)
//            {
//                defaultAction(obj);
//            }
//        }
//    }
}