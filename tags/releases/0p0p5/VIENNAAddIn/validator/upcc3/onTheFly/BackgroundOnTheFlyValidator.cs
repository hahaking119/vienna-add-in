using System;
using System.Collections.Generic;
using System.Threading;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    /// <summary>
    /// An on the fly validator running in a background thread.
    /// </summary>
    internal class BackgroundOnTheFlyValidator
    {
        private readonly Repository repository;
        private readonly object syncRoot = new object();
        private readonly Queue<ProcessItemRequest> processItemRequests = new Queue<ProcessItemRequest>();
        private bool shutdown;
        private const int millisecondsTimeout = Timeout.Infinite;

        public BackgroundOnTheFlyValidator(Repository repository)
        {
            this.repository = repository;
        }

        public void ProcessItem(string guid, ObjectType objectType)
        {
            lock (syncRoot)
            {
                processItemRequests.Enqueue(new ProcessItemRequest(repository, guid, objectType));
                Monitor.PulseAll(syncRoot);
            }
        }

        public void Shutdown()
        {
            lock (syncRoot)
            {
                shutdown = true;
                Monitor.PulseAll(syncRoot);
            }
        }

        public void Execute()
        {
            lock (syncRoot)
            {
                while (!shutdown)
                {
                    while (processItemRequests.Count == 0)
                    {
                        try
                        {
                            if (!Monitor.Wait(syncRoot, millisecondsTimeout))
                            {
                                throw new Exception("timeout");
                            }
                        }
                        catch
                        {
                            Monitor.PulseAll(syncRoot);
                            throw;
                        }
                    }
                    if (!shutdown)
                    {
                        ProcessItemRequest request = processItemRequests.Dequeue();
                        request.ProcessItem();
                    }
                    Monitor.PulseAll(syncRoot);
                }
            }
        }
    }
}