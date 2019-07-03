using System;
using System.IO;
using System.Net;

namespace Aran.Geometries.IO
{
	/// <summary>
	///  Converts a Well-known Binary string to a Geometry.
	/// </summary>
	/// <remarks>The Well-known
	///  <para>Binary format is defined in the 
	///  OpenGIS Simple Features Specification for SQL</para>
	/// </remarks> 
	public class GeometryWKBWriter
	{
		public GeometryWKBWriter ( )
		{
			MemoryStream memoryStream = new MemoryStream ( );
			_binaryWriter = new BinaryWriter ( memoryStream );
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
		/// The binary write method.
		/// </summary>
		/// <param name="geometry">The geometry to be written.</param>
		/// <param name="byteOrder">The format employed (big/little endian).</param>
		public BinaryWriter Write ( Geometry geometry, ByteOrder byteOrder )
		{
			//Write the format.
			_binaryWriter.Write ( ( byte ) byteOrder );

			//Write the type of this geometry
			WriteType ( geometry, byteOrder );

			//Write the geometry
			WriteGeometry ( geometry, byteOrder );

			return _binaryWriter;
		}

		public byte [] GetByteArray ( )
		{
			return ( _binaryWriter.BaseStream as MemoryStream ).ToArray ( );
		}

		/// <summary>
		/// Writes the type number for this geometry.
		/// </summary>
		/// <param name="geometry">The geometry to determine the type of.</param>
		private void WriteType ( Geometry geometry, ByteOrder byteOrder )
		{
			//Determine the type of the geometry.
			UInt32 typeword = ( UInt32 ) geometry.Type;
			typeword |= 0xC0000000;
			WriteUInt32 ( typeword, byteOrder );
		}

		/// <summary>
		/// Writes the geometry to the binary writer.
		/// </summary>
		/// <param name="geometry">The geometry to be written.</param>
		private void WriteGeometry ( Geometry geometry, ByteOrder byteOrder )
		{
			switch ( geometry.Type )
			{
				//Write the point.
				case GeometryType.Point:
					Point point = ( Point ) geometry;
					WritePoint ( point, byteOrder );
					break;
				//Write the Linestring.
				case GeometryType.LineString:
					LineString ls = ( LineString ) geometry;
					WriteLineString ( ls, byteOrder );
					break;
				//Write the Polygon.
				case GeometryType.Polygon:
					Polygon poly = ( Polygon ) geometry;
					WritePolygon ( poly, byteOrder );
					break;
				//Write the Multipoint.
				case GeometryType.MultiPoint:
					MultiPoint mp = ( MultiPoint ) geometry;
					WriteMultiPoint ( mp, byteOrder );
					break;
				//Write the Multilinestring.
				case GeometryType.MultiLineString:
					MultiLineString mls = ( MultiLineString ) geometry;
					WriteMultiLineString ( mls, byteOrder );
					break;
				//Write the Multipolygon.
				case GeometryType.MultiPolygon:
					MultiPolygon mPoly = ( MultiPolygon ) geometry;
					WriteMultiPolygon ( mPoly, byteOrder );
					break;
				default:
					throw new ArgumentException ( "Invalid Geometry Type" );
			}
		}

		/// <summary>
		/// Writes a point.
		/// </summary>
		/// <param name="point">The point to be written.</param>
		private void WritePoint ( Point point, ByteOrder byteOrder )
		{
			//Write the x coordinate.
			WriteDouble ( point.X, byteOrder );

			//Write the y coordinate.
			WriteDouble ( point.Y, byteOrder );

			//Write the x coordinate.
			WriteDouble ( point.Z, byteOrder );

			////Write the y coordinate.
			WriteDouble ( point.M, byteOrder );
		}

		/// <summary>
		/// Writes a linestring.
		/// </summary>
		/// <param name="ls">The linestring to be written.</param>
		private void WriteLineString ( LineString ls, ByteOrder byteOrder )
		{
			//Write the number of points in this linestring.
			WriteUInt32 ( ( uint ) ls.Count, byteOrder );

			//Loop on each set of coordinates.
			for ( int i = 0; i <= ls.Count - 1; i++ )
			{
				WritePoint ( ls [ i ], byteOrder );
			}
		}

		/// <summary>
		/// Writes a polygon.
		/// </summary>
		/// <param name="poly">The polygon to be written.</param>
		private void WritePolygon ( Polygon poly, ByteOrder byteOrder )
		{
			//Get the number of rings in this polygon.
			UInt32 numRings = ( UInt32 ) poly.InteriorRingList.Count + 1;

			//Write the number of rings to the stream (add one for the shell)
			WriteUInt32 ( numRings, byteOrder );

			//Get the shell of this polygon.
			Ring tmpRng = ( Ring ) poly.ExteriorRing.Clone ( );
			// Add first Point to Ring so that ring is closed in PostGIS v1.5
			tmpRng.Add ( tmpRng [ 0 ] );
			WriteLineString ( ( LineString ) tmpRng, byteOrder );

			//Loop on the number of rings - 1 because we already wrote the shell.
			for ( int i = 0; i < numRings - 1; i++ )
			{
				//Write the (lineString)LinearRing.
				tmpRng = ( Ring ) poly.InteriorRingList [ i ].Clone ( );
				// Add first Point to Ring so that ring is closed in PostGIS v1.5
				tmpRng.Add ( tmpRng [ 0 ] );
				WriteLineString ( ( LineString ) tmpRng, byteOrder );
			}
		}

		/// <summary>
		/// Writes a multipoint.
		/// </summary>
		/// <param name="mp">The multipoint to be written.</param>
		private void WriteMultiPoint ( MultiPoint mp, ByteOrder byteOrder )
		{
			//Get the number of points in this multipoint.
			UInt32 numPoints = ( uint ) mp.Count;

			//Write the number of points.
			WriteUInt32 ( numPoints, byteOrder );

			//Loop on the number of points.
			for ( int i = 0; i < numPoints; i++ )
			{
				//write the multipoint header
				_binaryWriter.Write ( ( byte ) byteOrder );
				WriteUInt32 ( 1 + 0xC0000000, byteOrder );

				//Write each point.
				WritePoint ( mp [ i ], byteOrder );
			}
		}

		/// <summary>
		/// Writes a multilinestring.
		/// </summary>
		/// <param name="mls">The multilinestring to be written.</param>
		private void WriteMultiLineString ( MultiLineString mls, ByteOrder byteOrder )
		{
			//Get the number of linestrings in this multilinestring.
			UInt32 numLineStrings = ( uint ) mls.Count;

			//Write the number of linestrings.
			WriteUInt32 ( numLineStrings, byteOrder );

			//Loop on the number of linestrings.
			for ( int i = 0; i < numLineStrings; i++ )
			{
				_binaryWriter.Write ( ( byte ) byteOrder );
				WriteUInt32 ( 2 + 0xC0000000, byteOrder );

				//Write each linestring.
				WriteLineString ( mls [ i ], byteOrder );
			}
		}

		/// <summary>
		/// Writes a multipolygon.
		/// </summary>
		/// <param name="mp">The mulitpolygon to be written.</param>
		private void WriteMultiPolygon ( MultiPolygon mp, ByteOrder byteOrder )
		{
			//Get the number of polygons in this multipolygon.
			UInt32 numpolygons = ( uint ) mp.Count;

			//Write the number of polygons.
			WriteUInt32 ( numpolygons, byteOrder );

			//Loop on the number of polygons.
			for ( int i = 0; i < numpolygons; i++ )
			{
				//Write the polygon header
				_binaryWriter.Write ( ( byte ) byteOrder );
				WriteUInt32 ( 3 + 0xC0000000, byteOrder );

				//Write each polygon.
				WritePolygon ( mp [ i ], byteOrder );
			}
		}

		private void WriteDouble ( double value, ByteOrder byteOrder )
		{
			if ( _byteOrder != byteOrder )
			{
				DoubleInt doulbeInt = new DoubleInt ( );
				doulbeInt.f = value;

				if ( byteOrder == ByteOrder.BigEndian )
					doulbeInt.i = IPAddress.HostToNetworkOrder ( doulbeInt.i );
				else
					doulbeInt.i = IPAddress.NetworkToHostOrder ( doulbeInt.i );
				_binaryWriter.Write ( doulbeInt.ui );
				return;
			}
			_binaryWriter.Write ( value );
		}

		private void WriteUInt32 ( UInt32 value, ByteOrder byteOrder )
		{
			if ( _byteOrder != byteOrder )
			{
				FloatInt floaInt = new FloatInt ( );
				floaInt.ui = value;

				if ( byteOrder == ByteOrder.BigEndian )
					floaInt.i = IPAddress.HostToNetworkOrder ( floaInt.i );
				else
					floaInt.i = IPAddress.NetworkToHostOrder ( floaInt.i );
				_binaryWriter.Write ( floaInt.ui );
				return;
			}
			_binaryWriter.Write ( value );
		}

		#endregion

		private ByteOrder _byteOrder;
		private BinaryWriter _binaryWriter;
	}
}