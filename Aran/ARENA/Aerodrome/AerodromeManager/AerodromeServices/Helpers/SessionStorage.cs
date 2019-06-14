using AerodromeServices.Services_Contract;
using System.Collections.Generic;
using System.Linq;

namespace AerodromeServices.Helpers
{
    public static class SessionStorage
    {
        public static readonly Dictionary<SessionData, IAmdbManagerServiceCallback> Sessions = new Dictionary<SessionData, IAmdbManagerServiceCallback>();

        public static void Add(SessionData sessionData, IAmdbManagerServiceCallback callback)
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
