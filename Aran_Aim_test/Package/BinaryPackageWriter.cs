using System.IO;
using System;

namespace Aran.Package
{
	public class BinaryPackageWriter :
			PackageWriter,
			IDisposable
	{
		public BinaryPackageWriter()
		{
			MemoryStream ms = new MemoryStream();
			_writer = new BinaryWriter(ms);
		}

		public BinaryPackageWriter(byte[] buffer)
		{
			CreatedWithVariant = 0;
			MemoryStream ms = new MemoryStream(buffer);
			_writer = new BinaryWriter(ms);
		}

		public BinaryPackageWriter(Stream stream)
		{
			CreatedWithVariant = 1;
			_writer = new BinaryWriter(stream);
		}

		public BinaryPackageWriter(BinaryWriter writer)
		{
			CreatedWithVariant = 2;
			_writer = writer;
		}

		public override void PutByte(byte value)
		{
			_writer.Write(value);
		}

		public override void PutBool(bool value)
		{
			_writer.Write(value);
		}

		public override void PutInt16(short value)
		{
			_writer.Write(value);
		}

		public override void PutInt32(int value)
		{
			_writer.Write(value);
		}

		public override void PutInt64(long value)
		{
			_writer.Write(value);
		}

		public override void PutUInt32(uint value)
		{
			_writer.Write(value);
		}

		public override void PutDouble(double value)
		{
			_writer.Write(value);
		}

		public override void PutString(string value)
		{
			_writer.Write(value);
		}

		public void Dispose()
		{
			switch (CreatedWithVariant)
			{
				case 0:
					//_writer.Dispose();
					_writer.BaseStream.Dispose();
					break;
				case 1:
					//_writer.Dispose();
					break;
				default:
					break;
			}
		}

		public BinaryWriter Writer
		{
			get { return _writer; }
		}

		private BinaryWriter _writer;
		private int CreatedWithVariant;
	}
}
