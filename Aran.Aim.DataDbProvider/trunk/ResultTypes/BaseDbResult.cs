
namespace Aran.Aim.Data
{
    public abstract class BaseDbResult
    {
        public abstract bool IsSucceed
        {
            get;
            set;
        }

        public abstract string Message
        {
            get;
            set;
        }
    }
}
