using System.Collections;
using System.Collections.Generic;
using Aran.Aim.Features;

namespace Aran.Aim.Data
{
    public class GettingResult : BaseDbResult
    {
        public GettingResult ()
        {
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

        public List<T> GetListAs<T> () where T : Feature
        {
            if (List is List<T>)
                return (List<T>) List;

            return ConvertListTo<T> ();
        }

        public List<T> ConvertListTo<T> () where T : Feature
        {
            List<T> list = new List<T> ();
            foreach (object item in List)
                list.Add (item as T);
            return list;
        }
    }
}
