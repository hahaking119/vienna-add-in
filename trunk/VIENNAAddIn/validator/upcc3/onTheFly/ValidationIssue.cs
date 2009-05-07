using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class ValidationIssue
    {
        private static int nextId;

        private readonly int id;
        protected readonly string guid;
        protected readonly ObjectType objectType;
        protected readonly Repository repository;

        public ValidationIssue(Repository repository, string guid, ObjectType objectType)
        {
            this.repository = repository;
            this.guid = guid;
            this.objectType = objectType;
            id = nextId++;
        }

        public int Id
        {
            get { return id; }
        }

        public virtual string Message
        {
            get
            {
                switch (objectType)
                {
                    case ObjectType.otElement:
                        {
                            Element element = repository.GetElementByGuid(guid);
                            return element.Name;
                        }
                }
                return "ERROR: Unknown object type: " + objectType;
            }
        }

        public object Item
        {
            get
            {
                switch (objectType)
                {
                    case ObjectType.otElement:
                        {
                            return repository.GetElementByGuid(guid);
                        }
                }
                return "ERROR: Unknown object type: " + objectType;
            }
        }

        public virtual IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[0]; }
        }

        public string GUID
        {
            get { return guid; }
        }

        public ObjectType ObjectType
        {
            get { return objectType; }
        }
    }
}