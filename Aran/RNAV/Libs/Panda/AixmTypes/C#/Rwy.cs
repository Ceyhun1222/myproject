using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
	public class Rwy : AIXM
	{
        public string Aerodrome_id { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

		public Rwy()
			: base(AIXMType.RWY)
		{
		}

		public override void Assign(PandaItem source)
		{
			Rwy src;
			base.Assign((AIXM)source);
			src = ((AIXM)source).AsRwy();
			Aerodrome_id = src.Aerodrome_id;
			Width = src.Width;
			Length = src.Length;
		}

		public override Object Clone()
		{
			Rwy src = new Rwy();
			src.Assign(this);
			return src;
		}

		public override void Pack(int handle)
		{
			base.Pack(handle);
			Registry_Contract.PutString(handle, Aerodrome_id);
			Registry_Contract.PutDouble(handle, Length);
			Registry_Contract.PutDouble(handle, Width);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			Aerodrome_id = Registry_Contract.GetString(handle);
			Length = Registry_Contract.GetDouble(handle);
			Width = Registry_Contract.GetDouble(handle);
		}
	}
}
