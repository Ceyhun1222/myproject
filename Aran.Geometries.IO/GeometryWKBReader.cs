using System;
using System.IO;
using System.Net;

namespace Aran.Geometries.IO
{
	/// <summary>
	///  Converts a Well-known Binary string to a Geometry.
	/// </summary>
	/// <remarks>The Well-known Binary format is defined in the 
	///  OpenGIS Simple Features Specification for SQL.
	/// </remarks> 
	public class GeometryWKBReader
	{
		public GeometryWKBReader ( )
		{
			TestByteOrder testByteOrderStruct = new TestByteOrder ( );
			testByteOrderStruct.i = 1;
			if ( testByteOrderStruct.orderedBytes.ob0 == 1 )
			{
				_byteOrder = ByteOrder.LittleEndian;
			}
			else if ( testByteOrderStruct.orderedBytes.ob1 == 1 )
			{
				_byteOrder = ByteOrder.BigEndian;
			}
			else
			{
				throw new Exception ( "Not found byte order type " );
			}
		}

		#region Methods

		/// <summary>
		/// Creates a memory stream from the supplied byte array.  Then calls the other create 
		/// method passing in the new binary reader created using the new memory stream.
		/// </summary>
		/// <param name="bArray">The byte array to be used to create the memory stream.</param>
		/// <returns>A Geometry.</returns>
		public Geometry Create ( byte [] bArray )
		{
			//Create a memory stream using the suppiled byte array.
			MemoryStream memStream = new MemoryStream ( bArray );

			//Create a new binary reader using the newly created memorystream.
			BinaryReader bReader = new BinaryReader ( memStream );

			//Call the main create function.
			return Create ( bReader );
		}

		/// <summary>
		/// Converts a Well-known binary representation to a Geometry.
		/// </summary>
		/// <param name="bReader">
		/// A byte array containing the definition of the geometry to be created.
		/// </param>
		/// <returns>
		/// Returns the Geometry specified by wellKnownBinary.  Throws an exception if there is a 
		/// parsing problem.
		/// </returns>
		public Geometry Create ( BinaryReader bReader )
		{
			_binaryReader = bReader;

			//Get the first byte in the array.  This specifies if the WellKnownBinary is in
			//XDR(Big Endian) format of NDR(Little Endian) format.
			ByteOrder byteOrder = ( ByteOrder ) _binaryReader.ReadByte ( );

			//If the format type is 0 it is XDR
			if ( byteOrder == ByteOrder.BigEndian || byteOrder == ByteOrder.LittleEndian )
			{
				return Create ( byteOrder );
			}
			//If the format is neither 0 nor 1 there is a problem throw an exception
			throw new ArgumentException ( "Format type not recognized" );
		}

		/// <summary>
		/// Converts a Well-known binary representation in NDR format to a Geometry.
		/// </summary>
		/// <returns>Returns the Geometry specified by wellKnownBinary.</returns>
		private Geometry Create ( ByteOrder byteOrder )
		{
			//Get the type of this geometry.
			UInt32 typeGeom = ReadUInt32 ( byteOrder );

			// If WKB is with SRID then just read it to go on
			if ( ( typeGeom & 0x20000000 ) != 0 )
				ReadUInt32 ( byteOrder );
			switch ( typeGeom & 0xFF )
			{
				//Type 1 is a point
				case 1:

					return CreateWKBPoint ( byteOrder );
				//Type 2 is a LineString
				case 2:
					return CreateWKBLineString ( byteOrder );
				//Type 3 is a Polygon
				case 3:
					return CreateWKBPolygon ( byteOrder );
				//Type 4 is a MultiPoint
				case 4:
					return CreateWKBMultiPoint ( byteOrder );
				//Type 5 is a MultiLineString
				case 5:
					return CreateWKBMultiLineString ( byteOrder );
				//Type 6 is a MultiPolygon
				case 6:
					return CreateWKBMultiPolygon ( byteOrder );
				//If the type is not 1-6 there is a problem throw an exception
				default:
					throw new ArgumentException ( "Geometry type not recognized" );
			}
		}

		/// <summary>
		/// Creates a point from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBPoint ( ByteOrder byteOrder )
		{
			//Create the x coordinate.
			double x = ReadDouble ( byteOrder );

			//Create the y coordinate.
			double y = ReadDouble ( byteOrder );

			//Create the z coordinate
			double z = ReadDouble ( byteOrder );

			//Create the m coordinate
			double m = ReadDouble ( byteOrder );

			//Create the Point
			Point pnt = new Point ( x, y, z );
			pnt.M = m;

			return pnt;
		}

		private MultiPoint ReadMultiPoint ( ByteOrder byteOrder )
		{
			//Get the number of points in this linestring.
			UInt32 numPoints = ReadUInt32 ( byteOrder );

			//Create an Array for the coordinates.
			MultiPoint mltPnt = new MultiPoint ( );

			//Loop around the number of points.
			for ( int i = 0; i < numPoints; i++ )
			{
				//Create a new point.
				Point pnt = CreateWKBPoint ( byteOrder ) as Point;

				//Add the coordinate to the coordinates array.
				mltPnt.Add ( pnt );
			}
			return mltPnt;
		}
		/// <summary>
		/// Creates a linestring from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private LineString CreateWKBLineString ( ByteOrder byteOrder )
		{
			LineString lnString = new LineString ( );
			lnString.AddMultiPoint ( ReadMultiPoint ( byteOrder ) );
			return lnString;
		}

		/// <summary>
		/// Creates a Polygon from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBPolygon ( ByteOrder byteOrder )
		{
			Polygon polygon = new Polygon ( );

			//Get the Number of rings in this Polygon.
			UInt32 numRings = ReadUInt32 ( byteOrder );
			//Debug.Assert(numRings>=1, "Number of rings in polygon must be 1 or more.");

			Ring extRng = new Ring ( );
			extRng.AddMultiPoint ( ReadMultiPoint ( byteOrder ) );
			extRng.Remove ( extRng.Count - 1 );
			polygon.ExteriorRing = extRng;

			//Create a new array of linearrings for the interior rings.
			for ( int i = 0; i < numRings - 1; i++ )
			{
				Ring rng = new Ring ( );
				rng.AddMultiPoint ( ReadMultiPoint ( byteOrder ) );
				rng.Remove ( rng.Count - 1 );
				polygon.InteriorRingList.Add ( rng );
			}

			//Create and return the Poylgon.
			return polygon;
		}

		/// <summary>
		/// Creates a Multipoint from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBMultiPoint ( ByteOrder byteOrder )
		{
			//Get the number of points in this multipoint.
			UInt32 numPoints = ReadUInt32 ( byteOrder );

			//Create a new array for the points.
			MultiPoint mltPnt = new MultiPoint ( );

			//Loop on the number of points.
			for ( int i = 0; i < numPoints; i++ )
			{
				// read Point header
				_binaryReader.ReadByte ( );
				ReadUInt32 ( byteOrder );
				//Create the next point and add it to the point array.
				mltPnt.Add ( ( Point ) CreateWKBPoint ( byteOrder ) );
			}
			//Create and return the MultiPoint.
			return mltPnt;
		}

		/// <summary>
		/// Creates a multilinestring from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBMultiLineString ( ByteOrder byteOrder )
		{
			//Get the number of linestrings in this multilinestring.
			UInt32 numLineStrings = ReadUInt32 ( byteOrder );

			//Create a new array for the linestrings .
			MultiLineString mltLineString = new MultiLineString ( );

			//Loop on the number of linestrings.
			for ( int i = 0; i < numLineStrings; i++ )
			{
				//read Point header
				_binaryReader.ReadByte ( );
				ReadUInt32 ( byteOrder );

				//Create the next linestring and add it to the array.
				mltLineString.Add ( ( LineString ) CreateWKBLineString ( byteOrder ) );
			}
			//Create and return the MultiLineString.
			return mltLineString;
		}

		/// <summary>
		/// Creates a multipolygon from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBMultiPolygon ( ByteOrder byteOrder )
		{
			//Get the number of Polygons.
			UInt32 numPolygons = ReadUInt32 ( byteOrder );

			//Create a new array for the Polygons.
			MultiPolygon mltPolygon = new MultiPolygon ( );

			//Loop on the number of polygons.
			for ( int i = 0; i < numPolygons; i++ )
			{
				// read polygon header
				_binaryReader.ReadByte ( );
				ReadUInt32 ( byteOrder );
				//Create the next polygon and add it to the array.
				mltPolygon.Add ( ( Polygon ) CreateWKBPolygon ( byteOrder ) );
			}
			//Create and return the MultiPolygon.
			return mltPolygon;
		}

		#endregion

		private double ReadDouble ( ByteOrder byteOrder )
		{
			if ( byteOrder != _byteOrder )
			{
				DoubleInt doulbeInt = new DoubleInt ( );
				doulbeInt.f = _binaryReader.ReadDouble ( );

				if ( byteOrder == ByteOrder.BigEndian )
					doulbeInt.i = IPAddress.HostToNetworkOrder ( doulbeInt.i );
				else
					doulbeInt.i = IPAddress.NetworkToHostOrder ( doulbeInt.i );
				return doulbeInt.f;
			}
			return _binaryReader.ReadDouble ( );
		}

		private UInt32 ReadUInt32 ( ByteOrder byteOrder )
		{
			if ( byteOrder != _byteOrder )
			{
				FloatInt floaInt = new FloatInt ( );
				floaInt.ui = _binaryReader.ReadUInt32 ( );

				if ( byteOrder == ByteOrder.BigEndian )
					floaInt.i = IPAddress.HostToNetworkOrder ( floaInt.i );
				else
					floaInt.i = IPAddress.NetworkToHostOrder ( floaInt.i );
				return floaInt.ui;
			}
			return _binaryReader.ReadUInt32 ( );
		}

		private ByteOrder _byteOrder;
		private BinaryReader _binaryReader;
	}
}