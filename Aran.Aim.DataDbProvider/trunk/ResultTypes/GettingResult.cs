using System.Collections;
using System.Collections.Generic;
using Aran.Aim.Features;

namespace Aran.Aim.Data
{
    public class GettingResult : BaseDbResult
    {
        public GettingResult()
        {
        }

        public GettingResult (IList list)
        {
            List = list;
            IsSucceed = true;
        }

        public GettingResult(string message)
        {
            IsSucceed = string.IsNullOrEmpty(message);
            Message = message;
        }

        public GettingResult (bool isSucceed, string message = "" )
        {
            IsSucceed = isSucceed;
            Message = message;
        }

        public override bool IsSucceed
        {
            get;
            set;
        }

        public override string Message
        {
            get;
            set;
        }

        public IList List
        {
            get;
            set;
        }

        public List<T> GetListAs<T>()
        {
            if (List == null)
                return null;

            if (List is List<T>)
                return (List<T>)List;

            var list = new List<T>();
            foreach (object item in List)
                list.Add((T)item);
            return list;
        }
    }
}
