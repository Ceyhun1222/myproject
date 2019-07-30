
namespace Aran.Aim.Data
{
    public class InsertingResult : BaseDbResult
    {
		public InsertingResult ( )
		{
            _isSucceed = true;
		}

        public InsertingResult(string message)
        {
            _isSucceed = string.IsNullOrEmpty(message);
            Message = message;
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
