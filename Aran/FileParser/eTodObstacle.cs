using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace KFileParser
{
	public class eTodObstacle
	{
		public eTodObstacle ( )
		{

		}

		public string ObjectID
		{
			get;
			set;
		}

		public Aran.Geometries.Geometry Geom
		{
			get;
			set;
		}

		public CodeVerticalStructure? Type
		{
			get;
			set;
		}

		public ValDistanceVertical Elevation
		{
			get;
			set;
		}

		public string GroupNumber
		{
			get;
			set;
		}

		public bool? Light
		{
			get;
			set;
		}

		public bool? Mark
		{
			get;
			set;
		}

		public CodeVerticalStructure? PartType
		{
			get;
			set;
		}

		public ValDistanceVertical PartElevation
		{
			get;
			set;
		}

		public ValDistance HorizontalAccuracy
		{
			get;
			set;
		}

		public ValDistance VerticalAccuracy
		{
			get;
			set;
		}

		public CodeVerticalDatum? VerticalDatumType
		{
			get;
			set;
		}

		public bool Mobile
		{
			get;
			set;
		}
	}
}
