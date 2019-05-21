using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Package
{
    public abstract class PackageReader
    {
        public abstract byte GetByte ( );
        public abstract bool GetBool ( );
        public abstract short GetInt16 ( );
        public abstract Int32 GetInt32 ( );
        public abstract Int64 GetInt64 ( );
        public abstract UInt32 GetUInt32 ( );
        public abstract double GetDouble ( );
        public abstract String GetString ( );

        public virtual DateTime GetDateTime ( )
        {
            long ticks = GetInt64 ();
            return new DateTime (ticks);
        }

        public T GetEnum<T> ( ) where T : struct
        {
            return ( T ) ( object ) GetInt32 ();
        }
    }
}
