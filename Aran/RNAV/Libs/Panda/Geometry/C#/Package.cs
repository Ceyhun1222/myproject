using System;

namespace ARAN.Package
{
    public interface IPackable
    {
        void Pack(PackageWriter writer);
        void Unpack(PackageReader reader);
    }

    public abstract class PackageWriter
    {
        public abstract void PutByte(byte value);
        public abstract void PutBool(bool value);
        public abstract void PutInt16(short value);
        public abstract void PutInt32(Int32 value);
        public abstract void PutInt64(Int64 value);
        public abstract void PutDouble(double value);
        public abstract void PutString(String value);
        public abstract void PutObject(object value);

        public virtual void PutDateTime(DateTime dateTime)
        {
            PutInt16((short)(dateTime.Year - 1900));
            PutByte((byte)dateTime.Month);
            PutByte((byte)dateTime.Day);
            PutByte((byte)dateTime.Hour);
            PutByte((byte)dateTime.Minute);
            PutByte((byte)dateTime.Second);
        }

        public void PutEnum<T>(T value) where T : struct
        {
            PutInt32((int)(object)value);
        }
    }

    public abstract class PackageReader
    {
        public abstract byte GetByte();
        public abstract bool GetBool();
        public abstract short GetInt16();
        public abstract Int32 GetInt32();
        public abstract Int64 GetInt64();
        public abstract double GetDouble();
        public abstract String GetString();
        public abstract object GetObject();

        public virtual DateTime GetDateTime()
        {
            int year = GetInt16() + 1900;
            int month = GetByte();
            int day = GetByte();
            int hour = GetByte();
            int minute = GetByte();
            int second = GetByte();

            return new DateTime(year, month, day, hour, minute, second, 0);
        }

        public T GetEnum<T>() where T : struct
        {
            return (T)(object)GetInt32();
        }
    }
}
