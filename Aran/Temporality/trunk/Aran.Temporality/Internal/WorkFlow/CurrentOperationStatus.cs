using System;

namespace Aran.Temporality.Internal.WorkFlow
{
    [Serializable]
    public class OperationStatus
    {
        public int PublicSlotId;
        public int PrivateSlotId;//-1 means operation with publick slot
        public int Progress;
    }

    internal static class CurrentOperationStatus
    {
        public static OperationStatus Status()
        {
            return new OperationStatus
                       {
                           PublicSlotId = PublicSlotId,
                           PrivateSlotId = PrivateSlotId,
                           Progress = Progress
                       };
        }

        public static int PublicSlotId=-1;
        public static int PrivateSlotId=-1;
        public static int Progress;

        private static int TotalTasks { get; set; }
        private static int CurrentTask { get; set; }

        private static int CurrentTaskTotalOperations { get; set; }

        private static int _currentTaskOperation;
        private static int CurrentTaskOperation
        {
            get { return _currentTaskOperation; }
            set
            {
                _currentTaskOperation = value;
                if (MaxOperationsPerTask < CurrentTaskOperation)
                {
                    MaxOperationsPerTask = CurrentTaskOperation;
                }
            }
        }

        private static int MaxOperationsPerTask { get; set; }

        public static void NewJob(int totalTasks)
        {
            Progress = 0;

            MaxOperationsPerTask = 0;
            
            CurrentTask = -1;
            CurrentTaskOperation = 0;
            TotalTasks = totalTasks;
        }

        public static void EndJob()
        {
            Progress = 0;
            PrivateSlotId = -1;
            PublicSlotId = -1;
        }

        public static void NextTask(int totalOperations)
        {
            CurrentTask++;
            CurrentTaskOperation = 0;
            CurrentTaskTotalOperations = totalOperations;
            CalculateProgress();
        }

        public static void NextOperation()
        {
            CurrentTaskOperation++;
            CalculateProgress();
        }

        public static void CalculateProgress()
        {
            double p = 0;
            if (TotalTasks != 0 && CurrentTask>-1)
            {
                p = (CurrentTask + (CurrentTaskOperation+1)/(double)(CurrentTaskTotalOperations+1)) * 100 / TotalTasks;
            }

            if (p < 0) p = 0;
            if (p > 100) p = 100;
            Progress = (int)p;
        }

       
    }
}
