using System;

namespace Aran.Temporality.Common.OperationResult
{
    [Serializable]
    public class CommonOperationResult
    {
        private bool _isOk = true;

        public bool IsOk
        {
            get { return _isOk; }
            set { _isOk = value; }
        }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return IsOk ? "Ok" : "Error:" + ErrorMessage;
        }

        public CommonOperationResult ReportError(string error)
        {
            IsOk = false;

            if (!String.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage += "\n" + error;
            }
            else
            {
                ErrorMessage = error;
            }

            return this;
        }

        public void Add(CommonOperationResult other)
        {
            //merge IsOk
            IsOk &= other.IsOk;

            //merge error message
            if (!String.IsNullOrWhiteSpace(other.ErrorMessage))
            {
                ReportError(other.ErrorMessage);
            }
        }
    }
}