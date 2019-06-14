using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.AranEnvironment
{
    public class AranException : Exception
    {
        public AranException(string message, ExceptionType exType = ExceptionType.Error) :
            base(message)
        {
            ExceptionType = exType;
            Handled = false;
        }

        public ExceptionType ExceptionType { get; set; }

        public bool Handled { get; set; }

        public void ShowMessageBox()
        {
            MessageBox.Show(Message, "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    (ExceptionType == ExceptionType.Warning ? MessageBoxIcon.Warning : MessageBoxIcon.Error));
        }
    }

    public enum ExceptionType
    {
        Error, Warning
    }
}
