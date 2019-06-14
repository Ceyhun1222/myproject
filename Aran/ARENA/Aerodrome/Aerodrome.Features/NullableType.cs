using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aerodrome.Enums;


namespace Aerodrome.Features
{
    public class AM_Nullable<T>
	{
		private NilReason? _nilReasson;
		private T _value;
		private bool _hasValue;

        public AM_Nullable()
        {
           
        }
        public AM_Nullable(T value)
        {
            this._value = (T)value;

            //if (!EqualityComparer<T>.Default.Equals(value, default(T)))
            //this._hasValue = true;
            //this._nilReasson = null;
        }

		public static implicit operator AM_Nullable<T> ( T value )
		{
			return new AM_Nullable<T> ( value );
		}

		public NilReason? NilReason
		{
			get
			{
				return _nilReasson;
			}
			set
			{
				_nilReasson = value;
               
			}
		}

		public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                //this._hasValue = true;
                //this._nilReasson = null;
            }
        }

		public bool IsNull
		{
			get => !_hasValue;
            set
            {
                _hasValue = value;
            }
        }

        public override string ToString()
        {           
                return Value?.ToString();
        }
	}	
}