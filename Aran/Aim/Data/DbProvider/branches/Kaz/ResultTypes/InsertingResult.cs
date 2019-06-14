
namespace Aran.Aim.Data
{
    public class InsertingResult : BaseDbResult
    {
		public InsertingResult ( )
		{

		}

        public InsertingResult ( bool isSucced, string message = "")
        {
            _isSucceed = isSucced;
            if ( message != null )
                Message = message;
        }

        public override bool IsSucceed
        {
            get
            {
                return _isSucceed;
            }
            set
            {
                _isSucceed = value;
                if ( _isSucceed )
                    Message = "";
            }
        }

        public override string Message
        {
            get;
            set;
        }

        private bool _isSucceed;
    }
}
