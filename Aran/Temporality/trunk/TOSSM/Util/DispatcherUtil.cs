using System;
using System.Threading;
using System.Windows.Threading;

namespace TOSSM.Util
{
    public static class DispatcherUtil
    {

        public static void AsyncWorkAndUIThreadUpdate<T>(this Dispatcher currentDispatcher, Func<T> threadWork, Action<T> guiUpdate)
        {
            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                T resultAfterThreadWork = threadWork();
                currentDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<T>(delegate (T result) {
                    guiUpdate(resultAfterThreadWork);
                }), resultAfterThreadWork);

            });
        }

        public static void AsyncWorkAndUIThreadUpdate<T>(this Dispatcher currentDispatcher, DispatcherPriority priority, Func<T> threadWork, Action<T> guiUpdate)
        {
            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                T resultAfterThreadWork = threadWork();
                currentDispatcher.BeginInvoke(priority, new Action<T>(delegate (T result) {
                    guiUpdate(resultAfterThreadWork);
                }), resultAfterThreadWork);

            });
        }

        public static void AsyncWorkAndUIThreadUpdate(this Dispatcher currentDispatcher, DispatcherPriority priority,  Action guiUpdate)
        {
            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                currentDispatcher.BeginInvoke(priority, new Action(guiUpdate));

            });
        }

        public static void AsyncWorkAndUIThreadUpdate(this Dispatcher currentDispatcher, Action guiUpdate)
        {
            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                currentDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(guiUpdate));

            });
        }

    }
}
