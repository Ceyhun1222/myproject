using System;


namespace Aran.Package
{
    public abstract class PackageWriter
    {
        public abstract void PutByte (byte value);
        public abstract void PutBool (bool value);
        public abstract void PutInt16 (short value);
        public abstract void PutInt32 (Int32 value);
        public abstract void PutInt64 (Int64 value);
        public abstract void PutUInt32 (UInt32 value);
        public abstract void PutDouble (double value);
        public abstract void PutString (String value);

        public virtual void PutDateTime (DateTime dateTime)
        {
            PutInt64 (dateTime.Ticks);
        }

        public void PutEnum<T> (T value) where T : struct
        {
            PutInt32 ((int) (object) value);
        }
    }
}
