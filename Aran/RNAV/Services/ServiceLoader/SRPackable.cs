using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.Contracts.Registry;

namespace ServiceLoader
{
    public class SRPackageReader : Aran.Package.PackageReader
    {
        public SRPackageReader ()
        {
        }

        public SRPackageReader (int handle)
        {
            _handle = handle;
        }

        public override byte GetByte ()
        {
            return Registry_Contract.GetByte (_handle);
        }

        public override bool GetBool ()
        {
            return Registry_Contract.GetBool (_handle);
        }

        public override short GetInt16 ()
        {
            return (short) Registry_Contract.GetInt32 (_handle);
        }

        public override int GetInt32 ()
        {
            return Registry_Contract.GetInt32 (_handle);
        }

        public override long GetInt64 ()
        {
            return Registry_Contract.GetInt64 (_handle);
        }

        public override uint GetUInt32 ()
        {
            return (uint) Registry_Contract.GetInt32 (_handle);
        }

        public override double GetDouble ()
        {
            return Registry_Contract.GetDouble (_handle);
        }

        public override string GetString ()
        {
            return Registry_Contract.GetString (_handle);
        }

        public int Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        private int _handle;
    }

    public class SRPackageWriter : Aran.Package.PackageWriter
    {
        public SRPackageWriter ()
        {
        }

        public SRPackageWriter (int handle)
        {
            _handle = handle;
        }

        public override void PutByte (byte value)
        {
            Registry_Contract.PutByte (_handle, value);
        }

        public override void PutBool (bool value)
        {
            Registry_Contract.PutBool (_handle, value);
        }

        public override void PutInt16 (short value)
        {
            Registry_Contract.PutInt32 (_handle, value);
        }

        public override void PutInt32 (int value)
        {
            Registry_Contract.PutInt32 (_handle, value);
        }

        public override void PutInt64 (long value)
        {
            Registry_Contract.PutInt64 (_handle,(int) value);
        }

        public override void PutUInt32 (uint value)
        {
            Registry_Contract.PutInt32 (_handle, (int) value);
        }

        public override void PutDouble (double value)
        {
            Registry_Contract.PutDouble (_handle,  value);
        }

        public override void PutString (string value)
        {
            Registry_Contract.PutString (_handle, value);
        }

        public int Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        private int _handle;
    }

}
