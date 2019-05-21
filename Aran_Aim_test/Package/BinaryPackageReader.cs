using System.IO;
using System;

namespace Aran.Package
{
	public class BinaryPackageReader :
			PackageReader,
			IDisposable
	{
		public BinaryPackageReader(byte[] buffer)
		{
			CreatedWithVariant = 0;
			MemoryStream ms = new MemoryStream(buffer);
			_binaryReader = new BinaryReader(ms);
		}

		public BinaryPackageReader(Stream stream)
		{
			CreatedWithVariant = 1;
			_binaryReader = new BinaryReader(stream);
		}

		public BinaryPackageReader(BinaryReader reader)
		{
			CreatedWithVariant = 2;
			_binaryReader = reader;
		}

		public override byte GetByte()
		{
			return _binaryReader.ReadByte();
		}

		public override bool GetBool()
		{
			return _binaryReader.ReadBoolean();
		}

		public override short GetInt16()
		{
			return _binaryReader.ReadInt16();
		}

		public override int GetInt32()
		{
			return _binaryReader.ReadInt32();
		}

		public override long GetInt64()
		{
			return _binaryReader.ReadInt64();
		}

		public override uint GetUInt32()
		{
			return _binaryReader.ReadUInt32();
		}

		public override double GetDouble()
		{
			return _binaryReader.ReadDouble();
		}

		public override string GetString()
		{
			return _binaryReader.ReadString();
		}

		public void Dispose()
		{
			switch (CreatedWithVariant)
			{
				case 0:
					_binaryReader.BaseStream.Dispose();
					//_binaryReader.Dispose();
					break;
				case 1:
					//_binaryReader.Dispose();
					break;
				default:
					break;
			}
		}

        public BinaryReader Reader
        {
            get { return _binaryReader; }
        }

		private BinaryReader _binaryReader;
		private int CreatedWithVariant;
	}
}
