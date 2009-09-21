using System;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    internal class ProcessItemRequest
    {
        private readonly Repository repository;
        public string GUID { get; private set; }
        public ObjectType ObjectType { get; private set; }

        public ProcessItemRequest(Repository repository, string guid, ObjectType objectType)
        {
            this.repository = repository;
            GUID = guid;
            ObjectType = objectType;
        }

        public void ProcessItem()
        {
            throw new NotImplementedException();
        }
    }
}