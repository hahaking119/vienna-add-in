using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public abstract class ValidationIssue
    {
        private static int nextId;

        private readonly int id;

        public ValidationIssue(string guid)
        {
            GUID = guid;
            id = nextId++;
        }

        public int Id
        {
            get { return id; }
        }

        public virtual IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[0]; }
        }

        public string GUID { get; private set; }

        public abstract object GetItem(Repository repository);
    }
}