using ChartServices.Services_Contract;
using System.Collections.Generic;
using System.Linq;

namespace ChartServices.Helpers
{
    public static class SessionStorage
    {
        public static readonly Dictionary<SessionData, IChartManagerServiceCallback> Sessions = new Dictionary<SessionData, IChartManagerServiceCallback>();

        public static void Add(SessionData sessionData, IChartManagerServiceCallback callback)
        {
            Sessions[sessionData] = callback;
        }

        public static void Remove(string sessionId)
        {
            var pair = Sessions.FirstOrDefault(c => c.Key.SessionId== sessionId);
            Sessions.Remove(pair.Key);

        }

        public static bool IsExist(string sessionId)
        {
            var pair = Sessions.FirstOrDefault(c => c.Key.SessionId == sessionId);
            if (pair.Key == null)
                return false;
            return Sessions.ContainsKey(pair.Key);
        }
    }
}
