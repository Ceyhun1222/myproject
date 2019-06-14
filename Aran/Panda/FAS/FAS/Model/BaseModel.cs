using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAS.Model
{
    public class BaseModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public BaseModel()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual string Validate(string propertyName)
        {
            return null;
        }


        #region IDataErrorInfo

        string IDataErrorInfo.Error
        {
            get { return "error"; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get { return Validate(columnName); }
        }

        #endregion
    }
}
