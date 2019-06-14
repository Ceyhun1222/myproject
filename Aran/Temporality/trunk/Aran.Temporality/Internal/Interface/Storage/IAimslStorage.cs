using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IAimslStorage : ICrudStorage<AimslOperation>
    {
        int CreateOperation(AimslOperation operation);

        int CreateOperation(string jobId, string fileName, DateTime creationTime);

        bool AddPullPoint(int id, string pullPoint);

        bool AddSubscription(int id, string subscription);

        bool AppendMesages(int id, List<string> messages, string status, DateTime lastchangeTime, bool closed);

        bool ChangeStatus(int id, string status, string description, bool closed);

        bool Destroy(int id, bool timeout = false);

        IList<AimslOperation> GetAllActiveAimslOperations();

    }
}