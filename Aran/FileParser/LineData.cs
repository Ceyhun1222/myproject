using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using System.IO;

namespace KFileParser
{
	public class LineData
	{
		public LineData ( )
		{
			X = double.NaN;
			Y = double.NaN;
			//Z = double.NaN;
			Z_MSL = double.NaN;
		}

		private string _path;

		public string Id
		{
			get;
			set;
		}
		public double X
		{
			get;
			set;
		}
		public double Y
		{
			get;
			set;
		}
		public double Z
		{
			get;
			set;
		}
		public double Z_MSL
		{
			get;
			set;
		}

		public bool? IsUp
		{
			get;
			set;
		}

		public bool Frangible
		{
			get;
			set;
		}

		public string Colour
		{
			get;
			set;
		}

		public bool MarkingICAOStandard
		{
			get;
			set;
		}

		public double Height
		{
			get;
			set;
		}
		public string Path
		{
			get
			{
				return _path;
			}

			set
			{
				_path = value;
				int indexOfSeparator = _path.LastIndexOf ( System.IO.Path.DirectorySeparatorChar );
				FileName = _path.Substring ( indexOfSeparator + 1, _path.Length - indexOfSeparator - 5 );
			}
		}

		public CodeRunwayPointRole? CodeRunway
		{
			get;
			set;
		}
		public string ZoneCode
		{
			get;
			set;
		}
		public string Code
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public void Clear ( bool excludeId = true )
		{
			if ( !excludeId )
				Id = string.Empty;
			X = double.NaN;
			Y = double.NaN;
			Z = double.NaN;
			Z_MSL = double.NaN;
			Path = string.Empty;
			CodeRunway = null;
			ZoneCode = string.Empty;
			Code = string.Empty;
			FileName = string.Empty;
		}
	}
}