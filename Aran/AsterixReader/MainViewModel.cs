using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ESRI.ArcGIS;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Microsoft.Win32;

namespace AsterixReader
{
	public class MainViewModel
	{
		private List<int> aircraftSubFields;

		public MainViewModel ( string sourceFileName, DateTime dateTime, string resultFeatClass )
		{
			aircraftSubFields = new List<int> ( );
			//DateTime time1 = DateTime.Now;
			ReadData ( sourceFileName, dateTime, resultFeatClass );
			//DateTime time2 = DateTime.Now;
			//var diff = time2 - time1;
			//Console.WriteLine ( "Full time : " + diff.ToString ( ) );
			//Console.ReadLine ( );
		}

		private void ReadData ( string fileName, DateTime dateTime, string resultFeatClass )
		{
			DateTime time1 = DateTime.Now;
			byte[] bytes = File.ReadAllBytes ( fileName );
			List<int> frnList = new List<int> ( );
			int index;
			int len;
			int startIndex = 6;
			List<AsterixData> dataList = new List<AsterixData> ( );
			while ( startIndex < bytes.Length )
			{
				index = startIndex;
				if ( bytes[ index ] != 62 )
					return;
				len = bytes[ index + 2 ] | ( bytes[ index + 1 ] << 8 );
				index += 3;
				int frn = 1;
				frnList.Clear ( );
				for ( ; index < bytes.Length - 3; index++ )
				{
					byte test = bytes[ index ];
					int mask = 128;
					for ( int j = 1; j < 8; j++, frn++ )
					{
						bool hasFspec = ( test & mask ) != 0;
						if ( hasFspec )
							frnList.Add ( frn );
						mask >>= 1;
					}
					if ( ( test & 1 ) == 0 )
						break;
				}
				index++;
				AsterixData data = new AsterixData ( );
				for ( int i = 0; i < frnList.Count; i++ )
				{
					switch ( frnList[ i ] )
					{
						case 1:
							GetSourceIdentifier ( bytes, data, ref index );
							break;
						case 3:
							GetServiceIdentification ( bytes, data, ref index );
							break;
						case 4:
							data.Time = dateTime + GetTime ( bytes, ref index );
							break;
						case 5:
							GetGeoCoords ( bytes, data, ref index );
							break;
						case 6:
							GetPrjCoords ( bytes, data, ref index );
							break;
						case 7:
							GetTrackVelocity ( bytes, data, ref index );
							break;
						case 8:
							GetAcceleration ( bytes, data, ref index );
							break;
						case 9:
							GetTrackMode3ACode ( bytes, data, ref index );
							break;
						case 10:
							data.TargetIdentification = GetTargetIdentification ( bytes, 1, ref index );
							break;
						case 11:
							GetAircraftDerivedData ( bytes, data, ref index );
							break;
						case 12:
							GetTrackNumber ( bytes, data, ref index );
							break;
						case 13:
							GetTrackStatus ( bytes, data, ref index );
							break;
						case 14:
							GetSystemTrackUpdateAges ( bytes, data, ref index );
							break;	
						case 15:
							GetModeOfMovement ( bytes, data, ref index );
							break;
						case 16:
							GetTrackDataAges ( bytes, data, ref index );
							break;
						case 17 :
							GetMeasuredFlightLevel ( bytes, data, ref index );
							break;
						case 18:
							GetCalculatedTrackGeometricAltitude ( bytes, data, ref index );
							break;
						case 19:
							GetCalculatedTrackBarometricAltitude ( bytes, data, ref index );
							break;
						case 20:
							GetRateOfClimb_Descent ( bytes, data, ref index );
							break;
						case 21:
							GetFlightPlanRelatedData ( bytes, data, ref index );
							break;
						case 22:
							GetTargetSizeAndOrientation ( bytes, data, ref index );
							break;
						case 23:
							GetVehicleFleetIdentification ( bytes, data, ref index );
							break;
						case 24:
							GetMode5DataReports_ExtendedMode1Code ( bytes, data, ref index );
							break;
						case 25:
							GetTrackMode2Code ( bytes, data, ref index );
							break;
						case 26:
							GetComposedTrackNumber ( bytes, data, ref index );
							break;
						default:
							break;
					}
				}
				dataList.Add ( data );
				startIndex += 6 + len;
			}
			//DateTime time2 = DateTime.Now;
			//var diff = time2 - time1;
			//Console.WriteLine ( "Read : " + diff.ToString ( ) );
			WriteToGdb ( resultFeatClass, dataList );			
		}

		private void GetComposedTrackNumber ( byte[] bytes, AsterixData data, ref int index )
		{
			ComposedTrackNumber trackNumb = new ComposedTrackNumber ( );
			trackNumb.SystemUnitId = bytes[ index ];
			index++;
			trackNumb.TrackNumber = ( bytes[ index ] << 8 ) | ( bytes[ index + 1 ] & 254 );
			index+=2;
			data.TrackNumbers = new List<ComposedTrackNumber> ( );
			data.TrackNumbers.Add ( trackNumb );
			while ( ( ( bytes[ index - 1 ] & 1 ) != 0 ) )
			{
				data.TrackNumbers.Add(GetSlaveTrackNumber ( bytes, ref index ));
			}
		}

		private ComposedTrackNumber GetSlaveTrackNumber ( byte[] bytes, ref int index )
		{
			ComposedTrackNumber trackNumb = new ComposedTrackNumber ( );
			trackNumb.SystemUnitId = bytes[ index ];
			index++;
			trackNumb.TrackNumber = ( bytes[ index ] << 8 ) | ( bytes[ index + 1 ] & 254 );
			index += 2;
			return trackNumb;
		}

		private void GetTrackMode2Code ( byte[] bytes, AsterixData data, ref int index )
		{
			int tmp = ( bytes[ index ] << 8 ) | bytes[ index + 1 ];
			int t = 0;
			for ( int i = 0; i < 4; i++ )
			{
				t <<= 3;
				t += tmp & 7;
				tmp >>= 3;
			}
			data.TrackMode2Code = System.Convert.ToString ( t, 8 );
			index += 2;
		}

		private void GetMode5DataReports_ExtendedMode1Code ( byte[] bytes, AsterixData data, ref int index )
		{
			List<int> olanlar = new List<int> ( );
			int mask;
			int indexField = 1;
			byte test;
			for ( int i = 0; i < 3; i++ )
			{
				mask = 128;
				test = bytes[ index ];
				for ( int j = 1; j < 8; j++, indexField++ )
				{
					bool hasField = ( test & mask ) != 0;
					if ( hasField )
						olanlar.Add ( indexField );
					mask >>= 1;
				}
				index++;
				if ( ( test & 1 ) == 0 )
					break;
			}
			for ( int i = 0; i < olanlar.Count; i++ )
			{
				switch ( olanlar[ i ] )
				{
					default:
						throw new Exception ( "Not implemented - Get Mode 5 Data reports & Extended Mode 1 Code, Subfield" + olanlar[ i ] );
				}
			}
		}

		private void GetVehicleFleetIdentification ( byte[] bytes, AsterixData data, ref int index )
		{
			switch ( bytes[index] )
			{
				case 0:
					data.VehicleFleetId = "Unknown";
					break;
				case 1:
					data.VehicleFleetId = "ATC equipment maintenance";
					break;
				case 2:
					data.VehicleFleetId = "Airport maintenance";
					break;
				case 3:
					data.VehicleFleetId = "Fire";
					break;
				case 4:
					data.VehicleFleetId = "Bird scarer";
					break;
				case 5:
					data.VehicleFleetId = "Snow plough";
					break;
				case 6:
					data.VehicleFleetId = "Runway sweeper";
					break;
				case 7:
					data.VehicleFleetId = "Emergency";
					break;
				case 8:
					data.VehicleFleetId = "Police";
					break;
				case 9:
					data.VehicleFleetId = "Bus";
					break;
				case 10:
					data.VehicleFleetId = "Tug (push/tow)";
					break;
				case 11:
					data.VehicleFleetId = "Grass cutter";
					break;
				case 12:
					data.VehicleFleetId = "Fuel";
					break;
				case 13:
					data.VehicleFleetId = "Baggage";
					break;
				case 14:
					data.VehicleFleetId = "Catering";
					break;
				case 15:
					data.VehicleFleetId = "Aircraft maintenance";
					break;					
				case 16:
					data.VehicleFleetId = "Flyco (follow me)";
					break;
				default:
					throw new Exception ( "Vehicle Fleet Identification couldn't be more than 16(Real value is " + bytes[ index ] + ")" );;
			}
			index++;
		}

		private void GetTargetSizeAndOrientation ( byte[] bytes, AsterixData data, ref int index )
		{
			data.Length = ( bytes[ index ] & 254 ) << 1;
			index++;
			if ( ( bytes[ index - 1] & 1 ) != 0 )
			{
				data.Orientation = ( ( bytes[ index ] & 254 ) << 1 ) * 2.81;
				index++;
				if ( ( bytes[ index - 1 ] & 1 ) != 0 )
				{
					data.Width = ( ( bytes[ index ] & 254 ) << 1 );
					index++;
					if ( ( bytes[ index - 1 ] & 1 ) != 0 )
					{
						throw new Exception ( "Not implemented - Target Size and Orientation, 2-nd Part" );
					}
				}
			}
		}

		private void GetFlightPlanRelatedData ( byte[] bytes, AsterixData data, ref int index )
		{
			List<int> olanlar = new List<int> ( );
			int mask;
			int indexField = 1;
			byte test;
			for ( int i = 0; i < 3; i++ )
			{
				mask = 128;
				test = bytes[ index ];
				for ( int j = 1; j < 8; j++, indexField++ )
				{
					bool hasField = ( test & mask ) != 0;
					if ( hasField )
						olanlar.Add ( indexField );
					mask >>= 1;
				}
				index++;
				if ( ( test & 1 ) == 0 )
					break;
			}
			for ( int i = 0; i < olanlar.Count; i++ )
			{
				switch ( olanlar[ i ] )
				{
					default:
						throw new Exception ( "Not implemented - Flight Plan Related Data, Subfield" + olanlar[ i ] );
				}
			}
		}

		private void GetRateOfClimb_Descent ( byte[] bytes, AsterixData data, ref int index )
		{
			int sign = bytes[ index ] & 128;
			data.RateClimb_Descent = ( ( ( bytes[ index ] & 127 ) << 8 ) | bytes[ index + 1 ] );
			if ( sign != 0 )
				data.RateClimb_Descent = data.RateClimb_Descent - 65536;
			data.RateClimb_Descent *= 6.25;
			index += 2;
		}

		private void GetCalculatedTrackBarometricAltitude ( byte[] bytes, AsterixData data, ref int index )
		{
			if ( ( bytes[ index ] & 128 ) != 0 )
				data.QNH_Corection = "QNH correction applied";
			else
				data.QNH_Corection = "No QNH correction applied";
			int sign = bytes[ index ] & 64;
			data.BarometricAltitude = ( ( bytes[ index ] & 63 ) << 8 ) | bytes[ index + 1 ];
			if ( sign != 0 )
				data.BarometricAltitude = data.BarometricAltitude - 32768;
			data.BarometricAltitude *= 0.25;
			index += 2;
		}

		private void GetCalculatedTrackGeometricAltitude ( byte[] bytes, AsterixData data, ref int index )
		{
			int sign = bytes[ index ] & 128;
			data.CalculatedTrackGeometrciAltitude = ( ( ( bytes[ index ] & 127 ) << 8 ) | bytes[ index + 1 ] );
			if ( sign != 0 )
				data.CalculatedTrackGeometrciAltitude = data.CalculatedTrackGeometrciAltitude - 65536;
			data.CalculatedTrackGeometrciAltitude *= 6.25;
			index += 2;
		}

		private void GetMeasuredFlightLevel ( byte[] bytes, AsterixData data, ref int index )
		{
			int sign = bytes[ index ] & 128;
			data.Measured_Flight_Level = ( ( ( bytes[ index ] & 127 ) << 8 ) | bytes[ index + 1 ] );
			if ( sign != 0 )
				data.Measured_Flight_Level = data.Measured_Flight_Level - 65536;
			data.Measured_Flight_Level *= 0.25;
			index += 2;
		}

		private void GetTrackDataAges ( byte[] bytes, AsterixData data, ref int index )
		{
			List<int> olanlar = new List<int> ( );
			int mask;
			int indexField = 1;
			byte test;
			for ( int i = 0; i < 5; i++ )
			{
				mask = 128;
				test = bytes[ index ];
				for ( int j = 1; j < 8; j++, indexField++ )
				{
					bool hasField = ( test & mask ) != 0;
					if ( hasField )
						olanlar.Add ( indexField );
					mask >>= 1;
				}
				index++;
				if ( ( test & 1 ) == 0 )
					break;
			}
			for ( int i = 0; i < olanlar.Count; i++ )
			{
				switch ( olanlar[i] )
				{
					case 1:
						data.Measured_Flight_Level_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 2:
						data.Mode_1_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 3:
						data.Mode_2_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 4:
						data.Mode_3_A_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 5:
						data.Mode_4_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 6:
						data.Mode_5_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 7:
						data.MagneticHeading_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 8:
						data.IAS_Mach_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 9:
						data.TAS_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 10:
						data.SelectedAltitude_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 11:
						data.FinalStateAltitude_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 12:
						data.TrajectoryIntent_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 13:
						data.ACAS_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 14:
						data.StatusReported_ADS_B_Age = bytes[ index ] * 0.252;
						index++;
						break;
					case 15:
						data.ACAS_Resolution_Advisory_Report_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 16:
						data.BarometricVerticalRate_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 17:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 17" );
					case 18:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 18" );
					case 19:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 19" );
					case 20:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 20" );
					case 21:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 21" );
					case 22:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 22" );
					case 23:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 23" );
					case 24:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 24" );
					case 25:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 25" );
					case 26:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 26" );
					case 27:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 27" );
					case 28:
						throw new Exception ( "Not implemented - Track Data Ages, Subfield = 28" );
					case 29:
						data.IAS_Data_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 30:
						data.MachNumber_Data_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 31:
						data.BarometricPressureSettingsDataAge = bytes[ index ] * 0.25;
						index++;
						break;
				}
			}
		}

		private void GetModeOfMovement ( byte[] bytes, AsterixData data, ref int index )
		{
			int t = ( bytes[ index ] & 192 ) << 6;
			switch ( t )
			{
				case 0:
					data.TRANS = "Constant Course";
					break;
				case 1:
					data.TRANS = "Right Turn";
					break;
				case 2:
					data.TRANS = "Left Turn";
					break;
				case 3:
					data.TRANS = "Undetermined";
					break;
			}

			t = ( bytes[ index ] & 48 ) << 4;
			switch ( t )
			{
				case 0:
					data.LONG = "Constant Groundspeed";
					break;
				case 1:
					data.LONG = "Increasing Groundspeed";
					break;
				case 2:
					data.LONG = "Decreasing Groundspeed";
					break;
				case 3:
					data.LONG = "Undetermined";
					break;
			}

			t = ( bytes[ index ] & 12 ) << 2;
			switch ( t )
			{
				case 0:
					data.VERT = "Level";
					break;
				case 1:
					data.VERT = "Climb";
					break;
				case 2:
					data.VERT = "Descent";
					break;
				case 3:
					data.VERT = "Undetermined";
					break;
			}

			if ( ( bytes[ index ] & 2 ) != 0 )
				data.ADF = "Altitude discrepancy";
			else
				data.ADF = "No altitude discrepancy";
			index++;
		}

		private void GetSystemTrackUpdateAges ( byte[] bytes, AsterixData data, ref int index )
		{
			List<int> olanlar = new List<int> ( );
			int mask;
			int indexField = 1;
			byte test;
			for ( int i = 0; i < 2; i++ )
			{
				mask = 128;
				test = bytes[ index ];
				for ( int j = 1; j < 8; j++, indexField++ )
				{
					bool hasField = ( test & mask ) != 0;
					if ( hasField )
						olanlar.Add ( indexField );
					mask >>= 1;
				}
				index++;
				if ( ( test & 1 ) == 0 )
					break;
			}
			for ( int i = 0; i < olanlar.Count; i++ )
			{
				switch ( olanlar[ i ] )
				{
					case 1:
						data.TrackAge = bytes[ index ] * 0.25;
						index++;
						break;
					case 2:
						data.PSR_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 3:
						data.SSR_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 4:
						data.Mode_S_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 5:
						data.ADS_C_Age = ( ( bytes[ index ] << 8 ) | bytes[ index + 1] ) * 0.25;
						index += 2;
						break;
					case 6:
						data.ES_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 7:
						data.VDL_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 8:
						data.UAT_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 9:
						data.Loop_Age = bytes[ index ] * 0.25;
						index++;
						break;
					case 10:
						data.Multilateration_Age = bytes[ index ] * 0.25;
						index++;
						break;
				}
			}
		}

		private void GetTrackStatus ( byte[] bytes, AsterixData data, ref int index )
		{
			TrackStatus trackStatus = new TrackStatus ( );
			if ( ( bytes[ index ] & 128 ) != 0 )
				trackStatus.SensorType = "Monosensor track";
			else
				trackStatus.SensorType = "Multisensor track";

			if ( ( bytes[ index ] & 64 ) != 0 )
				trackStatus.SPI = "SPI present in the last report received from a sensor capable of decoding this data";
			else
				trackStatus.SPI = "Default value";

			if ( ( bytes[ index ] & 32 ) != 0 )
				trackStatus.MostReliableHeight = "Geometric altitude";
			else
				trackStatus.MostReliableHeight = "Barometric altitude";

			int test = ( bytes[ index ] & 28 ) >> 2;
			switch ( test )
			{
				case 0:
					trackStatus.SourceTrackAltitude = "No source";
					break;
				case 1:
					trackStatus.SourceTrackAltitude = "GNSS";
					break;
				case 2:
					trackStatus.SourceTrackAltitude = "3D radar";
					break;
				case 3:
					trackStatus.SourceTrackAltitude = "Triangulation";
					break;
				case 4:
					trackStatus.SourceTrackAltitude = "Height from coverage";
					break;
				case 5:
					trackStatus.SourceTrackAltitude = "Speed look-up table";
					break;
				case 6:
					trackStatus.SourceTrackAltitude = "Default height";
					break;
				case 7:
					trackStatus.SourceTrackAltitude = "Multilateration";
					break;
			}
			if ( ( bytes[ index ] & 2 ) != 0 )
				trackStatus.CNF = "Tentative track";
			else
				trackStatus.CNF = "Confirmed track";

			index++;
			if ( ( bytes[ index -1 ] & 1 ) != 0 )
				GetTrackStatusExtent1 ( bytes, trackStatus, ref index );
		}

		private void GetTrackStatusExtent1 ( byte[] bytes, TrackStatus trackStatus, ref int index )
		{
			if ( ( bytes[ index ] & 128 ) != 0 )
				trackStatus.SIM = "Simulated track";
			else
				trackStatus.SIM = "Actual track";

			if ( ( bytes[ index ] & 64 ) != 0 )
				trackStatus.TSE = "Last message transmitted to the user for the track";
			else
				trackStatus.TSE = "Default value";

			if ( ( bytes[ index ] & 32 ) != 0 )
				trackStatus.TSB = "First message transmitted to the user for the track";
			else
				trackStatus.TSB = "Default value";

			if ( ( bytes[ index ] & 16 ) != 0 )
				trackStatus.FPC = "Flight plan correlated";
			else
				trackStatus.FPC = "Not flight-plan correlated";

			if ( ( bytes[ index ] & 8 ) != 0 )
				trackStatus.AFF = "ADS-B data inconsistent with other surveillance information";
			else
				trackStatus.AFF = "Default value";

			if ( ( bytes[ index ] & 4 ) != 0 )
				trackStatus.STP = "Slave Track Promotion";
			else
				trackStatus.STP = "Default value";

			if ( ( bytes[ index ] & 2 ) != 0 )
				trackStatus.KOS = "Background service used";
			else
				trackStatus.KOS = "Complementary service used";

			index++;
			if ( ( bytes[ index - 1] & 1 ) != 0 )
			{
				GetTrackStatusExtent2 ( bytes, trackStatus, ref index );
			}
		}

		private void GetTrackStatusExtent2 ( byte[] bytes, TrackStatus trackStatus, ref int index )
		{
			if ( ( bytes[ index ] & 128 ) != 0 )
				trackStatus.AMA = "Track resulting from amalgamation process";
			else
				trackStatus.AMA = "Track not resulting from amalgamation process";

			int t = ( bytes[ index ] & 96 ) >> 6;
			switch ( t )
			{
				case 0:
					trackStatus.MD4 = "No Mode 4 interrogation";
					break;
				case 1:
					trackStatus.MD4 = "Friendly target";
					break;
				case 2:
					trackStatus.MD4 = "Unknown target";
					break;
				case 3:
					trackStatus.MD4 = "No reply";
					break;
			}
			if ( ( bytes[ index ] & 16 ) != 0 )
				trackStatus.ME = "Military Emergency present in the last report received from a sensor capable of decoding this data";
			else
				trackStatus.ME = "Default value";

			if ( ( bytes[ index ] & 8 ) != 0 )
				trackStatus.MI = "Military Identification present in the last report received from a sensor capable of decoding this data";
			else
				trackStatus.MI = "Default value";

			t = ( bytes[ index ] & 6 ) << 1;
			switch ( t )
			{
				case 0:
					trackStatus.MD5 = "No Mode 5 interrogation";
					break;
				case 1:
					trackStatus.MD5 = "Friendly target";
					break;
				case 2:
					trackStatus.MD5 = "Unknown target";
					break;
				case 3:
					trackStatus.MD5 = "No reply";
					break;
			}
			index++;
			if ( ( bytes[ index - 1 ] & 1 ) != 0 )
				GetTrackStatusExtent3 ( bytes, trackStatus, ref index );
		}

		private void GetTrackStatusExtent3 ( byte[] bytes, TrackStatus trackStatus, ref int index )
		{
			if ( ( bytes[ index ] & 128 ) != 0 )
				trackStatus.CST = "Age of the last received track update is higher than system dependent threshold ( coasting )";
			else
				trackStatus.CST = "Default value";

			if ( ( bytes[ index ] & 64 ) != 0 )
				trackStatus.PSR = "Age of the last received PSR track update is higher than system dependent threshold";
			else
				trackStatus.PSR = "Default value";

			if ( ( bytes[ index ] & 32 ) != 0 )
				trackStatus.SSR = "Age of the last received SSR track update is higher than system dependent threshold";
			else
				trackStatus.SSR = "Default value";

			if ( ( bytes[ index ] & 16 ) != 0 )
				trackStatus.MDS = "Age of the last received Mode S track update is higher than system dependent threshold";
			else
				trackStatus.MDS = "Default value";

			if ( ( bytes[ index ] & 8 ) != 0 )
				trackStatus.ADS = "Age of the last received ADS-B track update is higher than system dependent threshold";
			else
				trackStatus.ADS = "Default value";

			if ( ( bytes[ index ] & 4 ) != 0 )
				trackStatus.SUC = "Special Used Code (Mode A codes to be defined in the system to mark a track with special interest)";
			else
				trackStatus.SUC = "Default value";

			if ( ( bytes[ index ] & 2 ) != 0 )
				trackStatus.AAC = "Assigned Mode A Code Conflict (same discrete Mode A Code assigned to another track)";
			else
				trackStatus.AAC = "Default value";

			index++;
			if ( ( bytes[ index - 1 ] & 1 ) != 0 )
				GetTrackStatusExtent4 ( bytes, trackStatus, ref index );
		}

		private void GetTrackStatusExtent4 ( byte[] bytes, TrackStatus trackStatus, ref int index )
		{
			int t = ( bytes[ index ] & 192 ) << 6;
			switch ( t )
			{
				case 0:
					trackStatus.SDS = "Combined";
					break;
				case 1:
					trackStatus.SDS = "Co-operative only";
					break;
				case 2:
					trackStatus.SDS = "Non-Cooperative only";
					break;
				case 3:
					trackStatus.SDS = "Not defined";
					break;
			}
			t = ( bytes[ index ] & 56 ) << 3;
			switch ( t )
			{
				case 0:
					trackStatus.EMS = "No emergency";
					break;
				case 1:
					trackStatus.EMS = "General emergency";
					break;
				case 2:
					trackStatus.EMS = "Lifeguard / medical";
					break;
				case 3:
					trackStatus.EMS = "Minimum fuel";
					break;
				case 4:
					trackStatus.EMS = "No communications";
					break;
				case 5:
					trackStatus.EMS = "Unlawful interference";
					break;
				case 6:
					trackStatus.EMS = "“Downed” Aircraft";
					break;
				case 7:
					trackStatus.EMS = "Undefined";
					break;
			}

			if ( ( bytes[ index ] & 4 ) != 0 )
				trackStatus.PFT = "Potential False Track Indication";
			else
				trackStatus.PFT = "No indication";

			if ( ( bytes[ index ] & 2 ) != 0 )
				trackStatus.FPLT = "Track created / updated with FPL data";
			else
				trackStatus.FPLT = "Default value";

			index++;
			if ( ( bytes[ index - 1 ] & 1 ) != 0 )
				GetTrackStatusExtent5 ( bytes, trackStatus, ref index );
		}

		private void GetTrackStatusExtent5 ( byte[] bytes, TrackStatus trackStatus, ref int index )
		{
			trackStatus.DUPT = "";
			trackStatus.DUPF = "";
			trackStatus.DUPM = "";

			if ( ( bytes[ index ] & 128 ) != 0 )
				trackStatus.DUPT = "Duplicate Mode 3/A Code";
			else
				trackStatus.DUPT = "Default value";

			if ( ( bytes[ index ] & 64 ) != 0 )
				trackStatus.DUPF = "Duplicate Flight Plan";
			else
				trackStatus.DUPF = "Default value";

			if ( ( bytes[ index ] & 32 ) != 0 )
				trackStatus.DUPM = "Duplicate Flight Plan due to manual correlation";
			else
				trackStatus.DUPM = "Default value";

			index++;
			if ( ( bytes[ index - 1] & 1 ) != 0 )
			{
			}
		}

		private void GetTrackNumber ( byte[] bytes, AsterixData data, ref int index )
		{
			data.TrackNumber = ( bytes[ index + 1 ] << 8 ) | bytes[ index ];
			index += 2;
		}

		private void WriteToGdb ( string fileName, List<AsterixData> dataList )
		{
			//ITable trackNumb;
			//DateTime time1 = DateTime.Now;
			IFeatureClass featureClass = CreateResultFile ( fileName);
			//DateTime time2 = DateTime.Now;
			//var diff = time2 - time1;
			//Console.WriteLine ( "Create feat class : " + diff.ToString ( ) );

			int indexSAC = featureClass.FindField ( "SAC" );
			int indexSIC = featureClass.FindField ( "SIC" );
			int indexServiceIdentifier = featureClass.FindField ( "ServiceIdentifier" );
			int timeIndex = featureClass.FindField ( "TimeOfTrack" );
			int indexCartesianPosX = featureClass.FindField ( "TrackPosCartesianX" );
			int indexCartesianPosY = featureClass.FindField ( "TrackPosCartesianY" );
			int indexTargetAddress = featureClass.FindField ( "TargetAddress" );
			int indexTargetIdentification = featureClass.FindField ( "TargetIdentification" );
			int indexMagneticHeading = featureClass.FindField ( "MagneticHeading" );
			int indexVeloX = featureClass.FindField ( "VelocityPosX" );
			int indexVeloY = featureClass.FindField ( "VelocityPosY" );
			int indexAccX = featureClass.FindField ( "AccelerationX" );
			int indexAccY = featureClass.FindField ( "AccelerationY" );
			int indexTrackModeCodeChanged = featureClass.FindField ( "TrackMode3CodeChanged" );
			int indexTrackModeCode = featureClass.FindField ( "TrackMode3Code" );
			int indexTas = featureClass.FindField ( "TAS" );
			int indexIas = featureClass.FindField ( "IAS" );
			int indexAltitudeSource = featureClass.FindField ( "Altitude_Source" );
			int indexAltitude = featureClass.FindField ( "Altitude" );
			int indexManagedVertMode = featureClass.FindField ( "Managed_Vertical_Mode" );
			int indexAltitudeHold = featureClass.FindField ( "Altitude_Hold" );
			int indexApproachMode = featureClass.FindField ( "Approach_Mode" );
			int indexFinalAltitude = featureClass.FindField ( "Final_State_Altitude" );
			int indexTrajDataIntentAvailability = featureClass.FindField ( "Trajectory_Intent_Data_Availability" );
			int indexTrajDataIntentValidity = featureClass.FindField ( "Trajecotry_Intent_Data_Validity" );
			int indexComCapability = featureClass.FindField ( "Communications_Capability_Transponder" );
			int indexFlightStatus = featureClass.FindField ( "Flight_Status" );
			int indexSSC = featureClass.FindField ( "Specific_Service_Capability" );
			int indexARC = featureClass.FindField ( "Altitude_reporting_capability" );
			int indexAIC = featureClass.FindField ( "Aircraft_identification_capability" );
			int indexBarometricVertRate = featureClass.FindField ( "Barometric_Vertical_Rate" );
			int indexMachNumber = featureClass.FindField ( "Mach_number" );
			int indexBarometricPressSettings = featureClass.FindField ( "Bar_Press_Data_Age" );
			int indexTrackNumber = featureClass.FindField ( "Track_number" );
			int indexTrackAge = featureClass.FindField ( "Track_Age" );
			int indexPSRAge = featureClass.FindField ( "PSR_Age" );
			int indexSSRAge = featureClass.FindField ( "SSR_Age" );
			int indexMode_S_Age = featureClass.FindField ( "Mode_S_Age" );
			int indexADS_C_Age = featureClass.FindField ( "ADS_C_Age" );
			int indexES_Age = featureClass.FindField ( "ES_Age" );
			int indexVDL_Age = featureClass.FindField ( "VDL_Age" );
			int indexUAT_Age = featureClass.FindField ( "UAT_Age" );
			int indexLoop_Age = featureClass.FindField ( "Loop_Age" );
			int indexMultilateration_Age = featureClass.FindField ( "Multilateration_Age" );
			int indexTrans = featureClass.FindField ( "Transversal_Acceleration" );
			int indexLong = featureClass.FindField ( "Longitudinal_Acceleration" );
			int indexVert = featureClass.FindField ( "Vertical_Rate" );
			int indexAdf = featureClass.FindField ( "Altitude_Discrepancy_Flag" );
			int indexMeasFL_Age = featureClass.FindField ( "Measured_Flight_Level_Age" );
			int indexMode1_Age = featureClass.FindField ( "Mode_1_Age" );
			int indexMode2_Age = featureClass.FindField ( "Mode_2_Age" );
			int indexMode3_A_Age = featureClass.FindField ( "Mode_3_A_Age" );
			int indexMode4_Age = featureClass.FindField ( "Mode_4_Age" );
			int indexMode5_Age = featureClass.FindField ( "Mode_5_Age" );
			int indexMagHeadingAge = featureClass.FindField ( "Magnetic_Heading_Age" );
			int indexIas_Mach_Age = featureClass.FindField ( "IAS_Mach_Age" );
			int indexTas_Age = featureClass.FindField ( "TAS_Age" );
			int indexSelectedAlt_Age = featureClass.FindField ( "Selected_Altitude_Age" );
			int indexFinalStateAlt_Age = featureClass.FindField ( "Final_State_Selected_Altitude_Age" );
			int indexTrajIntentData_Age = featureClass.FindField ( "Trajectory_Intent_Data_Age" );
			int indexAcas_Age = featureClass.FindField ( "ACAS_Age" );
			int indexStatusReported_Age = featureClass.FindField ( "Status_Reported_Age" );
			int indexBar_Vert_Rate_Age = featureClass.FindField ( "Barometric_Vert_Rate_Age" );
			int indexAcasResReportAge = featureClass.FindField ( "ACAS_Resolution_Report_Age" );
			int indexIasData_Age = featureClass.FindField ( "IAS_Data_Age" );
			int indexMachNumb_Age = featureClass.FindField ( "Mach_Numb_Data_Age" );
			int indexMeasFL = featureClass.FindField ( "Measured_Flight_Level" );
			int indexTrackGeomAltitude = featureClass.FindField ( "Calc_Track_Geom_Altitude" );
			int indexQnH_Corr = featureClass.FindField ( "QNH_Correction" );
			int indexBarAltitude = featureClass.FindField ( "Barometric_Altitude" );
			int indexRateClimbDescent = featureClass.FindField ( "Calc_Rate_Climb_Descent" );
			int indexLength = featureClass.FindField ( "Length" );
			int indexOrientation = featureClass.FindField ( "Orientation" );
			int indexWidth = featureClass.FindField ( "Width" );
			int indexVehicleFleetId = featureClass.FindField ( "Vehicle_Fleet_Identification" );
			int indexTrackMode2Code = featureClass.FindField ( "Track_Mode_2_Code" );

			//time1 = DateTime.Now;
			IFeatureCursor featureCursor = featureClass.Insert ( true );
			IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer ( );
			foreach ( var data in dataList )
			{
				IPoint point = new ESRI.ArcGIS.Geometry.Point ( );
				point.PutCoords ( data.YGeo, data.XGeo );
				featureBuffer.Shape = point;

				if ( !double.IsNaN ( data.XPrj ) )
					featureBuffer.set_Value ( indexCartesianPosX, data.XPrj );
				if ( !double.IsNaN ( data.YPrj ) )
					featureBuffer.set_Value ( indexCartesianPosY, data.YPrj );
				if ( !double.IsNaN ( data.VelocityX ) )
					featureBuffer.set_Value ( indexVeloX, data.VelocityX );
				if ( !double.IsNaN ( data.VelocityY ) )
					featureBuffer.set_Value ( indexVeloY, data.VelocityY );
				if ( !double.IsNaN ( data.AccelerationX ) )
					featureBuffer.set_Value ( indexAccX, data.AccelerationX );
				if ( !double.IsNaN ( data.AccelerationY ) )
					featureBuffer.set_Value ( indexAccY, data.AccelerationY );

				string boolStr = data.TrackMode3CodeChanged ? "Yes" : "No";
				featureBuffer.set_Value ( indexTrackModeCodeChanged, boolStr );
				featureBuffer.set_Value ( indexTrackModeCode, data.TrackMode3Code );
				featureBuffer.set_Value ( timeIndex, data.Time );
				featureBuffer.set_Value ( indexTargetIdentification, data.TargetIdentification );

				featureBuffer.set_Value ( indexTargetAddress, data.TargetAddress );
				featureBuffer.set_Value ( indexSAC, data.SAC );
				featureBuffer.set_Value ( indexSIC, data.SIC );
				if ( !double.IsNaN ( data.TAS ) )
					featureBuffer.set_Value ( indexTas, data.TAS );
				if ( !double.IsNaN ( data.IAS ) )
					featureBuffer.set_Value ( indexIas, data.IAS );
				featureBuffer.set_Value ( indexServiceIdentifier, data.ServiceIdentification );
				if ( !double.IsNaN ( data.Altitude ) )
					featureBuffer.set_Value ( indexAltitude, data.Altitude );
				featureBuffer.set_Value ( indexAltitudeSource, data.AltitudeSource );
				featureBuffer.set_Value ( indexManagedVertMode, data.ManageVerticalMode );
				featureBuffer.set_Value ( indexAltitudeHold, data.AltitudeHold );

				featureBuffer.set_Value ( indexApproachMode, data.ApproachMode );
				if ( !double.IsNaN ( data.FinalStateAltitude ) )
					featureBuffer.set_Value ( indexFinalAltitude, data.FinalStateAltitude );

				featureBuffer.set_Value ( indexTrajDataIntentAvailability, data.TrajectoryIntentDataAvailability );
				featureBuffer.set_Value ( indexTrajDataIntentValidity, data.TrajectoryIntentDataValidity );
				featureBuffer.set_Value ( indexComCapability, data.CommunicationCapability );
				featureBuffer.set_Value ( indexFlightStatus, data.FlightStatus );
				featureBuffer.set_Value ( indexSSC, data.SpecificServiceCapability );
				featureBuffer.set_Value ( indexARC, data.AltitudeReportingCapability );
				featureBuffer.set_Value ( indexAIC, data.AircraftIdentificationCapability );
				if ( !double.IsNaN ( data.BarometricVerticalRate ) )
					featureBuffer.set_Value ( indexBarometricVertRate, data.BarometricVerticalRate );
				if ( !double.IsNaN ( data.MagneticHeading ) )
					featureBuffer.set_Value ( indexMagneticHeading, data.MagneticHeading );

				if ( !double.IsNaN ( data.MachNumber ) )
					featureBuffer.set_Value ( indexMachNumber, data.MachNumber );

				featureBuffer.set_Value ( indexTrackNumber, data.TrackNumber );
				if ( !double.IsNaN ( data.BarometricPressureSettingsDataAge ) )
					featureBuffer.set_Value ( indexBarometricPressSettings, data.BarometricPressureSettingsDataAge );

				if ( !double.IsNaN ( data.TrackAge ) )
					featureBuffer.set_Value ( indexTrackAge, data.TrackAge );
				if ( !double.IsNaN ( data.PSR_Age ) )
					featureBuffer.set_Value ( indexPSRAge, data.PSR_Age );
				if ( !double.IsNaN ( data.SSR_Age ) )
					featureBuffer.set_Value ( indexSSRAge, data.SSR_Age );
				if ( !double.IsNaN ( data.Mode_S_Age ) )
					featureBuffer.set_Value ( indexMode_S_Age, data.Mode_S_Age );
				if ( !double.IsNaN ( data.ADS_C_Age ) )
					featureBuffer.set_Value ( indexADS_C_Age, data.ADS_C_Age );
				if ( !double.IsNaN ( data.ES_Age ) )
					featureBuffer.set_Value ( indexES_Age, data.ES_Age );
				if ( !double.IsNaN ( data.VDL_Age ) )
					featureBuffer.set_Value ( indexVDL_Age, data.VDL_Age );

				if ( !double.IsNaN ( data.UAT_Age ) )
				featureBuffer.set_Value ( indexUAT_Age, data.UAT_Age );
				if ( !double.IsNaN ( data.Loop_Age) )
					featureBuffer.set_Value ( indexLoop_Age, data.Loop_Age );
				if ( !double.IsNaN ( data.Multilateration_Age ) )
					featureBuffer.set_Value ( indexMultilateration_Age, data.Multilateration_Age );

				featureBuffer.set_Value ( indexTrans, data.TRANS );
				featureBuffer.set_Value ( indexLong, data.LONG );
				featureBuffer.set_Value ( indexVert, data.VERT );
				featureBuffer.set_Value ( indexAdf, data.ADF );
				if ( !double.IsNaN ( data.Measured_Flight_Level_Age ) )
					featureBuffer.set_Value ( indexMeasFL_Age, data.Measured_Flight_Level_Age );
				if ( !double.IsNaN ( data.Mode_1_Age ) )
					featureBuffer.set_Value ( indexMode1_Age, data.Mode_1_Age );
				if ( !double.IsNaN ( data.Mode_2_Age ) )
					featureBuffer.set_Value ( indexMode2_Age, data.Mode_2_Age );

				if ( !double.IsNaN ( data.Mode_3_A_Age ) )
					featureBuffer.set_Value ( indexMode3_A_Age, data.Mode_3_A_Age );
				if ( !double.IsNaN ( data.Mode_4_Age ) )
					featureBuffer.set_Value ( indexMode4_Age, data.Mode_4_Age );
				if ( !double.IsNaN ( data.Mode_5_Age ) )
					featureBuffer.set_Value ( indexMode5_Age, data.Mode_5_Age );
				if ( !double.IsNaN ( data.MagneticHeading_Age) )
					featureBuffer.set_Value ( indexMagneticHeading, data.MagneticHeading_Age );
				if ( !double.IsNaN ( data.IAS_Mach_Age ))
					featureBuffer.set_Value ( indexIas_Mach_Age, data.IAS_Mach_Age );
				if ( !double.IsNaN ( data.TAS_Age ) )
					featureBuffer.set_Value ( indexTas_Age, data.TAS_Age );
				if ( !double.IsNaN ( data.SelectedAltitude_Age ) )
					featureBuffer.set_Value ( indexSelectedAlt_Age, data.SelectedAltitude_Age );
				if ( !double.IsNaN ( data.FinalStateAltitude_Age ) )
					featureBuffer.set_Value ( indexFinalStateAlt_Age, data.FinalStateAltitude_Age );
				if ( !double.IsNaN ( data.TrajectoryIntent_Age ) )
					featureBuffer.set_Value ( indexTrajIntentData_Age, data.TrajectoryIntent_Age );
				if ( !double.IsNaN ( data.ACAS_Age ) )
					featureBuffer.set_Value ( indexAcas_Age, data.ACAS_Age );

				if ( !double.IsNaN ( data.StatusReported_ADS_B_Age ) )
					featureBuffer.set_Value ( indexStatusReported_Age, data.StatusReported_ADS_B_Age );
				if ( !double.IsNaN ( data.BarometricVerticalRate_Age ) )
					featureBuffer.set_Value ( indexBar_Vert_Rate_Age, data.BarometricVerticalRate_Age );
				if ( !double.IsNaN ( data.ACAS_Resolution_Advisory_Report_Age ) )
					featureBuffer.set_Value ( indexAcasResReportAge, data.ACAS_Resolution_Advisory_Report_Age );
				if ( !double.IsNaN ( data.IAS_Data_Age ) )
					featureBuffer.set_Value ( indexIasData_Age, data.IAS_Data_Age );
				if ( !double.IsNaN ( data.MachNumber_Data_Age ) )
					featureBuffer.set_Value ( indexMachNumb_Age, data.MachNumber_Data_Age );
				if ( !double.IsNaN ( data.Measured_Flight_Level ) )
					featureBuffer.set_Value ( indexMeasFL, data.Measured_Flight_Level );
				if ( !double.IsNaN ( data.CalculatedTrackGeometrciAltitude ) )
					featureBuffer.set_Value ( indexTrackGeomAltitude, data.CalculatedTrackGeometrciAltitude );
				featureBuffer.set_Value ( indexQnH_Corr, data.QNH_Corection );
				if ( !double.IsNaN ( data.BarometricAltitude ) )
					featureBuffer.set_Value ( indexBarAltitude, data.BarometricAltitude );
				if ( !double.IsNaN ( data.RateClimb_Descent ) )
					featureBuffer.set_Value ( indexRateClimbDescent, data.RateClimb_Descent );

				if ( !double.IsNaN ( data.Length ) )
					featureBuffer.set_Value ( indexLength, data.Length );
				if ( !double.IsNaN ( data.Orientation ) )
					featureBuffer.set_Value ( indexOrientation, data.Orientation );
				if ( !double.IsNaN ( data.Width ) )
					featureBuffer.set_Value ( indexWidth, data.Width );
				featureBuffer.set_Value ( indexVehicleFleetId, data.VehicleFleetId );
				featureBuffer.set_Value ( indexTrackMode2Code, data.TrackMode2Code );

				featureCursor.InsertFeature ( featureBuffer );
			}
			featureCursor.Flush ( );
			Marshal.ReleaseComObject ( featureCursor );
			//time2 = DateTime.Now;
			//diff = time2 - time1;
			//Console.WriteLine ( "Wrote : " + diff.ToString ( ) );
		}

		private IFeatureClass CreateResultFile ( string fileName)
		{
			RuntimeManager.BindLicense ( ProductCode.EngineOrDesktop );
			IAoInitialize ao = new AoInitialize ( );
			ao.Initialize ( esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB );

			IWorkspaceFactory2 workspaceFactory = ( IWorkspaceFactory2 ) new FileGDBWorkspaceFactory ( );
			string dir = System.IO.Path.GetDirectoryName ( fileName );
			string dirDir = System.IO.Path.GetDirectoryName(dir);
			string path = dir.Substring ( dirDir.Length + 1);
			if ( Directory.Exists ( dir ) )
			{
				Directory.Delete ( dir, true );
				//throw new Exception ( "File exists" );
			}          			
            IWorkspaceName workspaceName = workspaceFactory.Create ( dirDir, path, null, 0 );
			IName name = ( IName ) workspaceName;
			IFeatureWorkspace featWorkspace = ( IFeatureWorkspace ) name.Open ( );

			//trackNumbFeatClass = CreateTrackNumbFeatClass ( null, featWorkspace );
			return CreateMainFeatureClass ( fileName.Substring ( dir.Length + 1 ), null, featWorkspace );
		}

		public IFeatureClass CreateMainFeatureClass ( String featureClassName, UID classExtensionUID,
	IFeatureWorkspace featureWorkspace )
		{
			// Create a fields collection for the feature class.
			IFields fields = new FieldsClass ( );
			IFieldsEdit fieldsEdit = ( IFieldsEdit ) fields;

			// Add an object ID field to the fields collection. This is mandatory for feature classes.
			IField oidField = new FieldClass ( );
			IFieldEdit oidFieldEdit = ( IFieldEdit ) oidField;
			oidFieldEdit.Name_2 = "OID";
			oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
			fieldsEdit.AddField ( oidField );

			// Create a geometry definition (and spatial reference) for the feature class.
			IGeometryDef geometryDef = new GeometryDefClass ( );
			IGeometryDefEdit geometryDefEdit = ( IGeometryDefEdit ) geometryDef;
			geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
			ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass ( );
			ISpatialReference spatialReference =
			spatialReferenceFactory.CreateGeographicCoordinateSystem ( ( int ) esriSRGeoCSType.esriSRGeoCS_WGS1984 );
			ISpatialReferenceResolution spatialReferenceResolution = ( ISpatialReferenceResolution ) spatialReference;
			//spatialReferenceResolution.ConstructFromHorizon ( );
			ISpatialReferenceTolerance spatialReferenceTolerance = ( ISpatialReferenceTolerance ) spatialReference;
			spatialReferenceTolerance.SetDefaultXYTolerance ( );
			geometryDefEdit.SpatialReference_2 = spatialReference;

			// Add a geometry field to the fields collection. This is where the geometry definition is applied.
			IField geometryField = new FieldClass ( );
			IFieldEdit geometryFieldEdit = ( IFieldEdit ) geometryField;
			geometryFieldEdit.Name_2 = "Shape";
			geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
			geometryFieldEdit.GeometryDef_2 = geometryDef;
			fieldsEdit.AddField ( geometryField );

			// Create a text field called "SAC" for the fields collection.
			IField sacField = new FieldClass ( );
			IFieldEdit sacFieldEdit = ( IFieldEdit ) sacField;
			sacFieldEdit.Name_2 = "SAC";
			sacFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
			sacFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( sacField );

			// Create a text field called "SIC" for the fields collection.
			IField sicField = new FieldClass ( );
			IFieldEdit sicFieldEdit = ( IFieldEdit ) sicField;
			sicFieldEdit.Name_2 = "SIC";
			sicFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
			sicFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( sicFieldEdit );

			// Create a text field called "ServiceIdentifier" for the fields collection.
			IField serviceIdField = new FieldClass ( );
			IFieldEdit serviceIdFieldEdit = ( IFieldEdit ) serviceIdField;
			serviceIdFieldEdit.Name_2 = "ServiceIdentifier";
			serviceIdFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			serviceIdFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( serviceIdFieldEdit );

			// Create a text field called "TimeOfTrack" for the fields collection.
			IField timeField = new FieldClass ( );
			IFieldEdit timeFieldEdit = ( IFieldEdit ) timeField;
			timeFieldEdit.Name_2 = "TimeOfTrack";
			timeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
			timeFieldEdit.Length_2 = 30;
			fieldsEdit.AddField ( timeFieldEdit );

			// Create a text field called "TrackPosCartesianX" for the fields collection.
			IField cartPosXField = new FieldClass ( );
			IFieldEdit cartPosXFieldEdit = ( IFieldEdit ) cartPosXField;
			cartPosXFieldEdit.Name_2 = "TrackPosCartesianX";
			cartPosXFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			cartPosXFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( cartPosXFieldEdit );

			// Create a text field called "TrackPosCartesianY" for the fields collection.
			IField cartPosYField = new FieldClass ( );
			IFieldEdit cartPosYFieldEdit = ( IFieldEdit ) cartPosYField;
			cartPosYFieldEdit.Name_2 = "TrackPosCartesianY";
			cartPosYFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			cartPosYFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( cartPosYFieldEdit );

			// Create a text field called "VelocityPosX" for the fields collection.
			IField velocityXField = new FieldClass ( );
			IFieldEdit velocityXFieldEdit = ( IFieldEdit ) velocityXField;
			velocityXFieldEdit.Name_2 = "VelocityPosX";
			velocityXFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			velocityXFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( velocityXFieldEdit );

			// Create a text field called "VelocityPosY" for the fields collection.
			IField velocityYField = new FieldClass ( );
			IFieldEdit velocityYFieldEdit = ( IFieldEdit ) velocityYField;
			velocityYFieldEdit.Name_2 = "VelocityPosY";
			velocityYFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			velocityYFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( velocityYFieldEdit );

			// Create a text field called "AccelerationX" for the fields collection.
			IField accXField = new FieldClass ( );
			IFieldEdit accXFieldEdit = ( IFieldEdit ) accXField;
			accXFieldEdit.Name_2 = "AccelerationX";
			accXFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			accXFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( accXFieldEdit );

			// Create a text field called "AccelerationY" for the fields collection.
			IField accYField = new FieldClass ( );
			IFieldEdit accYFieldEdit = ( IFieldEdit ) accYField;
			accYFieldEdit.Name_2 = "AccelerationY";
			accYFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			accYFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( accYFieldEdit );

			// Create a text field called "trackMode3CodeChangedField" for the fields collection.
			IField trackMode3CodeChangedField = new FieldClass ( );
			IFieldEdit trackModeChangedFieldEdit = ( IFieldEdit ) trackMode3CodeChangedField;
			trackModeChangedFieldEdit.Name_2 = "TrackMode3CodeChanged";
			trackModeChangedFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			trackModeChangedFieldEdit.Length_2 = 6;
			fieldsEdit.AddField ( trackModeChangedFieldEdit );

			// Create a text field called "TrackMode3Code" for the fields collection.
			IField trackModeCodeField = new FieldClass ( );
			IFieldEdit trackModeFieldEdit = ( IFieldEdit ) trackModeCodeField;
			trackModeFieldEdit.Name_2 = "TrackMode3Code";
			trackModeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			trackModeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trackModeFieldEdit );

			// Create a text field called "TAS" for the fields collection.
			IField tasField = new FieldClass ( );
			IFieldEdit tasFieldEdit = ( IFieldEdit ) tasField;
			tasFieldEdit.Name_2 = "TAS";
			tasFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			tasFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( tasFieldEdit );

			// Create a text field called "IAS" for the fields collection.
			IField iasField = new FieldClass ( );
			IFieldEdit iasFieldEdit = ( IFieldEdit ) iasField;
			iasFieldEdit.Name_2 = "IAS";
			iasFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			iasFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( iasFieldEdit );

			// Create a text field called "Mach number" for the fields collection.
			IField machNumberField = new FieldClass ( );
			IFieldEdit machNumberFieldEdit = ( IFieldEdit ) machNumberField;
			machNumberFieldEdit.Name_2 = "Mach_number";
			machNumberFieldEdit.AliasName_2 = "Mach number";
			machNumberFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			machNumberFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( machNumberFieldEdit );

			// Create a text field called "Track number" for the fields collection.
			IField trackNumberField = new FieldClass ( );
			IFieldEdit trackNumberFieldEdit = ( IFieldEdit ) trackNumberField;
			trackNumberFieldEdit.Name_2 = "Track_number";
			trackNumberFieldEdit.AliasName_2 = "Track number";
			trackNumberFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			trackNumberFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trackNumberFieldEdit );

			// Create a text field called "Barometric Pressure Setting" for the fields collection.
			IField barometricPressSettingField = new FieldClass ( );
			IFieldEdit barometricPressSettingFieldEdit = ( IFieldEdit ) barometricPressSettingField;
			barometricPressSettingFieldEdit.Name_2 = "Barometric_Pressure_Setting";
			barometricPressSettingFieldEdit.AliasName_2 = "Barometric Pressure Setting";
			barometricPressSettingFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			barometricPressSettingFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( barometricPressSettingFieldEdit );

			// Create a text field called "Altitude Source" for the fields collection.
			IField altitudeSourceField = new FieldClass ( );
			IFieldEdit altitudeSourceFieldEdit = ( IFieldEdit ) altitudeSourceField;
			altitudeSourceFieldEdit.Name_2 = "Altitude_Source";
			altitudeSourceFieldEdit.AliasName_2 = "Altitude Source";
			altitudeSourceFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			altitudeSourceFieldEdit.Length_2 = 40;
			fieldsEdit.AddField ( altitudeSourceFieldEdit );

			// Create a text field called "Altitude (ft)" for the fields collection.
			IField altitudeField = new FieldClass ( );
			IFieldEdit altitudeFieldEdit = ( IFieldEdit ) altitudeField;
			altitudeFieldEdit.Name_2 = "Altitude";
			altitudeFieldEdit.AliasName_2 = "Altitude (ft)";
			altitudeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			altitudeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( altitudeFieldEdit );

			// Create a text field called "Managed Vertical Mode" for the fields collection.
			IField managedVertModeField = new FieldClass ( );
			IFieldEdit managedVertModeFieldEdit = ( IFieldEdit ) managedVertModeField;
			managedVertModeFieldEdit.Name_2 = "Managed_Vertical_Mode";
			managedVertModeFieldEdit.AliasName_2 = "Managed Vertical Mode";
			managedVertModeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			managedVertModeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( managedVertModeFieldEdit );

			// Create a text field called "Altitude hold" for the fields collection.
			IField altitudeHoldField = new FieldClass ( );
			IFieldEdit altitudeHoldFieldEdit = ( IFieldEdit ) altitudeHoldField;
			altitudeHoldFieldEdit.Name_2 = "Altitude_Hold";
			altitudeHoldFieldEdit.AliasName_2 = "Altitude hold";
			altitudeHoldFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			altitudeHoldFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( altitudeHoldFieldEdit );

			// Create a text field called "Approach Mode" for the fields collection.
			IField approachModeField = new FieldClass ( );
			IFieldEdit approachModeFieldEdit = ( IFieldEdit ) approachModeField;
			approachModeFieldEdit.Name_2 = "Approach_Mode";
			approachModeFieldEdit.AliasName_2 = "Approach Mode";
			approachModeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			approachModeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( approachModeFieldEdit );

			// Create a text field called "Final State Altitude (ft)" for the fields collection.
			IField finalAltitudeField = new FieldClass ( );
			IFieldEdit finalAltitudeFieldEdit = ( IFieldEdit ) finalAltitudeField;
			finalAltitudeFieldEdit.Name_2 = "Final_State_Altitude";
			finalAltitudeFieldEdit.AliasName_2 = "Final State Altitude (ft)";
			finalAltitudeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			finalAltitudeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( finalAltitudeFieldEdit );

			// Create a text field called "Trajectory Intent Data availability" for the fields collection.
			IField trajectoryIntentDataAvailabilityField = new FieldClass ( );
			IFieldEdit trajectoryIntentDataAvailabilityFieldEdit = ( IFieldEdit ) trajectoryIntentDataAvailabilityField;
			trajectoryIntentDataAvailabilityFieldEdit.Name_2 = "Trajectory_Intent_Data_Availability";
			trajectoryIntentDataAvailabilityFieldEdit.AliasName_2 = "Trajectory Intent Data Availability";
			trajectoryIntentDataAvailabilityFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			trajectoryIntentDataAvailabilityFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trajectoryIntentDataAvailabilityFieldEdit );

			// Create a text field called "Communications capability" for the fields collection.
			IField comCapField = new FieldClass ( );
			IFieldEdit comCapFieldEdit = ( IFieldEdit ) comCapField;
			comCapFieldEdit.Name_2 = "Communications_Capability_Transponder";
			comCapFieldEdit.AliasName_2 = "Communications capability of the transponder";
			comCapFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			comCapFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( comCapFieldEdit );

			// Create a text field called "Flight Status" for the fields collection.
			IField flightStatusField = new FieldClass ( );
			IFieldEdit flightStatusFieldEdit = ( IFieldEdit ) flightStatusField;
			flightStatusFieldEdit.Name_2 = "Flight_Status";
			flightStatusFieldEdit.AliasName_2 = "Flight Status";
			flightStatusFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			flightStatusFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( flightStatusFieldEdit );

			// Create a text field called "Specific service capability" for the fields collection.
			IField specServiceCapField = new FieldClass ( );
			IFieldEdit specServiceCapFieldEdit = ( IFieldEdit ) specServiceCapField;
			specServiceCapFieldEdit.Name_2 = "Specific_Service_Capability";
			specServiceCapFieldEdit.AliasName_2 = "Specific service capability";
			specServiceCapFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			specServiceCapFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( specServiceCapFieldEdit );

			// Create a text field called "Altitude reporting capability" for the fields collection.
			IField altitudeReportCapField = new FieldClass ( );
			IFieldEdit altitudeReportCapFieldEdit = ( IFieldEdit ) altitudeReportCapField;
			altitudeReportCapFieldEdit.Name_2 = "Altitude_reporting_capability";
			altitudeReportCapFieldEdit.AliasName_2 = "Altitude reporting capability";
			altitudeReportCapFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			altitudeReportCapFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( altitudeReportCapFieldEdit );

			// Create a text field called "Aircraft identification capability" for the fields collection.
			IField aircraftIdCapField = new FieldClass ( );
			IFieldEdit aircraftIdCapFieldEdit = ( IFieldEdit ) aircraftIdCapField;
			aircraftIdCapFieldEdit.Name_2 = "Aircraft_identification_capability";
			aircraftIdCapFieldEdit.AliasName_2 = "Aircraft identification capability";
			aircraftIdCapFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			aircraftIdCapFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( aircraftIdCapFieldEdit );

			// Create a text field called "Trajectory Intent Data Validity" for the fields collection.
			IField trajIntentDataValidityField = new FieldClass ( );
			IFieldEdit trajIntentDataValidityFieldEdit = ( IFieldEdit ) trajIntentDataValidityField;
			trajIntentDataValidityFieldEdit.Name_2 = "Trajecotry_Intent_Data_Validity";
			trajIntentDataValidityFieldEdit.AliasName_2 = "Trajectory Intent Data Validity";
			trajIntentDataValidityFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			trajIntentDataValidityFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trajIntentDataValidityFieldEdit );

			// Create a text field called "TargetIdentification" for the fields collection.
			IField targetIdField = new FieldClass ( );
			IFieldEdit targetIdFieldEdit = ( IFieldEdit ) targetIdField;
			targetIdFieldEdit.Name_2 = "TargetIdentification";
			targetIdFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			targetIdFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( targetIdFieldEdit );

			// Create a text field called "TargetAddress" for the fields collection.
			IField targedAddressField = new FieldClass ( );
			IFieldEdit targetAddressFieldEdit = ( IFieldEdit ) targedAddressField;
			targetAddressFieldEdit.Name_2 = "TargetAddress";
			targetAddressFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			targetAddressFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( targetAddressFieldEdit );

			// Create a text field called "MagneticHeading" for the fields collection.
			IField magnetHeadingField = new FieldClass ( );
			IFieldEdit magnetHeadingFieldEdit = ( IFieldEdit ) magnetHeadingField;
			magnetHeadingFieldEdit.Name_2 = "MagneticHeading";
			magnetHeadingFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			magnetHeadingFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( magnetHeadingFieldEdit );

			// Create a text field called "Barometric Vertical Rate" for the fields collection.
			IField barometricVertRateField = new FieldClass ( );
			IFieldEdit barometricVertRateFieldEdit = ( IFieldEdit ) barometricVertRateField;
			barometricVertRateFieldEdit.Name_2 = "Barometric_Vertical_Rate";
			barometricVertRateFieldEdit.AliasName_2 = "Barometric Vertical Rate";
			barometricVertRateFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			barometricVertRateFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( barometricVertRateFieldEdit );

			// Create a text field called "TrackAge" for the fields collection.
			IField trackAgeField = new FieldClass ( );
			IFieldEdit trackAgeFieldEdit = ( IFieldEdit ) trackAgeField;
			trackAgeFieldEdit.Name_2 = "Track_Age";
			trackAgeFieldEdit.AliasName_2 = "Track Age";
			trackAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			trackAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trackAgeFieldEdit );

			// Create a text field called "PSR Age" for the fields collection.
			IField psrAgeField = new FieldClass ( );
			IFieldEdit psrAgeFieldEdit = ( IFieldEdit ) psrAgeField;
			psrAgeFieldEdit.Name_2 = "PSR_Age";
			psrAgeFieldEdit.AliasName_2 = "PSR Age";
			psrAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			psrAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( psrAgeFieldEdit );

			// Create a text field called "SSR Age" for the fields collection.
			IField ssrAgeField = new FieldClass ( );
			IFieldEdit ssrAgeFieldEdit = ( IFieldEdit ) ssrAgeField;
			ssrAgeFieldEdit.Name_2 = "SSR_Age";
			ssrAgeFieldEdit.AliasName_2 = "SSR Age";
			ssrAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			ssrAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( ssrAgeFieldEdit );

			// Create a text field called "Mode S Age" for the fields collection.
			IField modeSAgeField = new FieldClass ( );
			IFieldEdit modeSAgeFieldEdit = ( IFieldEdit ) modeSAgeField;
			modeSAgeFieldEdit.Name_2 = "Mode_S_Age";
			modeSAgeFieldEdit.AliasName_2 = "Mode S Age";
			modeSAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			modeSAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( modeSAgeFieldEdit );

			// Create a text field called "ADS C Age" for the fields collection.
			IField adsCAgeField = new FieldClass ( );
			IFieldEdit adsCAgeFieldEdit = ( IFieldEdit ) adsCAgeField;
			adsCAgeFieldEdit.Name_2 = "ADS_C_Age";
			adsCAgeFieldEdit.AliasName_2 = "ADS-C Age";
			adsCAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			adsCAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( adsCAgeFieldEdit );

			// Create a text field called "ES Age" for the fields collection.
			IField esAgeField = new FieldClass ( );
			IFieldEdit esAgeFieldEdit = ( IFieldEdit ) esAgeField;
			esAgeFieldEdit.Name_2 = "ES_Age";
			esAgeFieldEdit.AliasName_2 = "ES Age";
			esAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			esAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( esAgeFieldEdit );

			// Create a text field called "VDL Age" for the fields collection.
			IField vdlAgeField = new FieldClass ( );
			IFieldEdit vdlAgeFieldEdit = ( IFieldEdit ) vdlAgeField;
			vdlAgeFieldEdit.Name_2 = "VDL_Age";
			vdlAgeFieldEdit.AliasName_2 = "VDL Age";
			vdlAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			vdlAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( vdlAgeFieldEdit );

			// Create a text field called "UAT Age" for the fields collection.
			IField uatAgeField = new FieldClass ( );
			IFieldEdit uatAgeFieldEdit = ( IFieldEdit ) uatAgeField;
			uatAgeFieldEdit.Name_2 = "UAT_Age";
			uatAgeFieldEdit.AliasName_2 = "UAT Age";
			uatAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			uatAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( uatAgeFieldEdit );

			// Create a text field called "Loop Age" for the fields collection.
			IField loopAgeField = new FieldClass ( );
			IFieldEdit loopAgeFieldEdit = ( IFieldEdit ) loopAgeField;
			loopAgeFieldEdit.Name_2 = "Loop_Age";
			loopAgeFieldEdit.AliasName_2 = "Loop Age";
			loopAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			loopAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( loopAgeFieldEdit );

			// Create a text field called "Multilateration Age" for the fields collection.
			IField mltAgeField = new FieldClass ( );
			IFieldEdit mltAgeFieldEdit = ( IFieldEdit ) mltAgeField;
			mltAgeFieldEdit.Name_2 = "Multilateration_Age";
			mltAgeFieldEdit.AliasName_2 = "Multilateration Age";
			mltAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			mltAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( mltAgeFieldEdit );

			// Create a text field called "Transversal Acceleration" for the fields collection.
			IField transField = new FieldClass ( );
			IFieldEdit transFieldEdit = ( IFieldEdit ) transField;
			transFieldEdit.Name_2 = "Transversal_Acceleration";
			transFieldEdit.AliasName_2 = "Transversal Acceleration";
			transFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			transFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( transFieldEdit );

			// Create a text field called "Longitudinal Acceleration" for the fields collection.
			IField longField = new FieldClass ( );
			IFieldEdit longFieldEdit = ( IFieldEdit ) longField;
			longFieldEdit.Name_2 = "Longitudinal_Acceleration";
			longFieldEdit.AliasName_2 = "Longitudinal Acceleration";
			longFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			longFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( longFieldEdit );

			// Create a text field called "Vertical Rate" for the fields collection.
			IField vertRateField = new FieldClass ( );
			IFieldEdit vertRateFieldEdit = ( IFieldEdit ) vertRateField;
			vertRateFieldEdit.Name_2 = "Vertical_Rate";
			vertRateFieldEdit.AliasName_2 = "Vertical Rate";
			vertRateFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			vertRateFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( vertRateFieldEdit );

			// Create a text field called "Altitude Discrepancy Flag Rate" for the fields collection.
			IField adfField = new FieldClass ( );
			IFieldEdit adfFieldEdit = ( IFieldEdit ) adfField;
			adfFieldEdit.Name_2 = "Altitude_Discrepancy_Flag";
			adfFieldEdit.AliasName_2 = "Altitude Discrepancy Flag";
			adfFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			adfFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( adfFieldEdit );

			// Create a text field called "Measured Flight Level" for the fields collection.
			IField measFlField = new FieldClass ( );
			IFieldEdit measFLFieldEdit = ( IFieldEdit ) measFlField;
			measFLFieldEdit.Name_2 = "Measured_Flight_Level";
			measFLFieldEdit.AliasName_2 = "Measured Flight Level(FL)";
			measFLFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			measFLFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( measFLFieldEdit );

			// Create a text field called "Measured Flight Level Age" for the fields collection.
			IField measFlAgeField = new FieldClass ( );
			IFieldEdit measFLAgeFieldEdit = ( IFieldEdit ) measFlAgeField;
			measFLAgeFieldEdit.Name_2 = "Measured_Flight_Level_Age";
			measFLAgeFieldEdit.AliasName_2 = "Measured Flight Level Age";
			measFLAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			measFLAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( measFLAgeFieldEdit );

			// Create a text field called "Mode 1 Age" for the fields collection.
			IField mode1AgeField = new FieldClass ( );
			IFieldEdit mode1AgeFieldEdit = ( IFieldEdit ) mode1AgeField;
			mode1AgeFieldEdit.Name_2 = "Mode_1_Age";
			mode1AgeFieldEdit.AliasName_2 = "Mode 1 Age";
			mode1AgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			mode1AgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( mode1AgeFieldEdit );

			// Create a text field called "Mode 2 Age" for the fields collection.
			IField mode2AgeField = new FieldClass ( );
			IFieldEdit mode2AgeFieldEdit = ( IFieldEdit ) mode2AgeField;
			mode2AgeFieldEdit.Name_2 = "Mode_2_Age";
			mode2AgeFieldEdit.AliasName_2 = "Mode 2 Age";
			mode2AgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			mode2AgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( mode2AgeFieldEdit );

			// Create a text field called "Mode 3/A Age" for the fields collection.
			IField mode3A_AgeField = new FieldClass ( );
			IFieldEdit mode3A_AgeFieldEdit = ( IFieldEdit ) mode3A_AgeField;
			mode3A_AgeFieldEdit.Name_2 = "Mode_3_A_Age";
			mode3A_AgeFieldEdit.AliasName_2 = "Mode 3/A Age";
			mode3A_AgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			mode3A_AgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( mode3A_AgeFieldEdit );

			// Create a text field called "Mode 4 Age" for the fields collection.
			IField mode4AgeField = new FieldClass ( );
			IFieldEdit mode4AgeFieldEdit = ( IFieldEdit ) mode4AgeField;
			mode4AgeFieldEdit.Name_2 = "Mode_4_Age";
			mode4AgeFieldEdit.AliasName_2 = "Mode 4 Age";
			mode4AgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			mode4AgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( mode4AgeFieldEdit );

			// Create a text field called "Mode 5 Age" for the fields collection.
			IField mode5AgeField = new FieldClass ( );
			IFieldEdit mode5AgeFieldEdit = ( IFieldEdit ) mode5AgeField;
			mode5AgeFieldEdit.Name_2 = "Mode_5_Age";
			mode5AgeFieldEdit.AliasName_2 = "Mode 5 Age";
			mode5AgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			mode5AgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( mode5AgeFieldEdit );

			// Create a text field called "Magnetic Heading Age" for the fields collection.
			IField magHeadingAgeField = new FieldClass ( );
			IFieldEdit magHeadingAgeFieldEdit = ( IFieldEdit ) magHeadingAgeField;
			magHeadingAgeFieldEdit.Name_2 = "Magnetic_Heading_Age";
			magHeadingAgeFieldEdit.AliasName_2 = "Magnetic Heading Age";
			magHeadingAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			magHeadingAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( magHeadingAgeFieldEdit );

			// Create a text field called "IAS Mach Age" for the fields collection.
			IField iasMachAgeField = new FieldClass ( );
			IFieldEdit iasMachAgeFieldEdit = ( IFieldEdit ) iasMachAgeField;
			iasMachAgeFieldEdit.Name_2 = "IAS_Mach_Age";
			iasMachAgeFieldEdit.AliasName_2 = "IAS/Mach Age";
			iasMachAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			iasMachAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( iasMachAgeFieldEdit );

			// Create a text field called "Tas Age" for the fields collection.
			IField tasAgeField = new FieldClass ( );
			IFieldEdit tasAgeFieldEdit = ( IFieldEdit ) tasAgeField;
			tasAgeFieldEdit.Name_2 = "TAS_Age";
			tasAgeFieldEdit.AliasName_2 = "TAS Age";
			tasAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			tasAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( tasAgeFieldEdit );

			// Create a text field called "Selected Altitude Age" for the fields collection.
			IField selAltAgeField = new FieldClass ( );
			IFieldEdit selAltAgeFieldEdit = ( IFieldEdit ) selAltAgeField;
			selAltAgeFieldEdit.Name_2 = "Selected_Altitude_Age";
			selAltAgeFieldEdit.AliasName_2 = "Selected Altitude Age";
			selAltAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			selAltAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( selAltAgeFieldEdit );

			// Create a text field called "Final State Selected Altitude Age" for the fields collection.
			IField finalAltitudeAgeField = new FieldClass ( );
			IFieldEdit finalAltitudeAgeFieldEdit = ( IFieldEdit ) finalAltitudeAgeField;
			finalAltitudeAgeFieldEdit.Name_2 = "Final_State_Selected_Altitude_Age";
			finalAltitudeAgeFieldEdit.AliasName_2 = "Final State Selected Altitude Age";
			finalAltitudeAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			finalAltitudeAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( finalAltitudeAgeFieldEdit );

			// Create a text field called "Trajectory Intent Data Age" for the fields collection.
			IField trajIntentDataAgeField = new FieldClass ( );
			IFieldEdit trajIntentDataAgeFieldEdit = ( IFieldEdit ) trajIntentDataAgeField;
			trajIntentDataAgeFieldEdit.Name_2 = "Trajectory_Intent_Data_Age";
			trajIntentDataAgeFieldEdit.AliasName_2 = "Trajectory Intent Data Age";
			trajIntentDataAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			trajIntentDataAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trajIntentDataAgeFieldEdit );

			// Create a text field called "Communications/ACAS Capability Age" for the fields collection.
			IField acasAgeField = new FieldClass ( );
			IFieldEdit acasAgeFieldEdit = ( IFieldEdit ) acasAgeField;
			acasAgeFieldEdit.Name_2 = "ACAS_Age";
			acasAgeFieldEdit.AliasName_2 = "Communications/ACAS Capability Age";
			acasAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			acasAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( acasAgeFieldEdit );

			// Create a text field called "Status Reported by ADS-B Age" for the fields collection.
			IField statusReportedAgeField = new FieldClass ( );
			IFieldEdit statusReportedAgeFieldEdit = ( IFieldEdit ) statusReportedAgeField;
			statusReportedAgeFieldEdit.Name_2 = "Status_Reported_Age";
			statusReportedAgeFieldEdit.AliasName_2 = "Status Reported by ADS-B Age";
			statusReportedAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			statusReportedAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( statusReportedAgeFieldEdit );

			// Create a text field called "ACAS resolution advisory report Age" for the fields collection.
			IField acasResReportedAgeField = new FieldClass ( );
			IFieldEdit acasResReportedAgeFieldEdit = ( IFieldEdit ) acasResReportedAgeField;
			acasResReportedAgeFieldEdit.Name_2 = "ACAS_Resolution_Report_Age";
			acasResReportedAgeFieldEdit.AliasName_2 = "ACAS Resolution Advisory Report Age";
			acasResReportedAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			acasResReportedAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( acasResReportedAgeFieldEdit );

			// Create a text field called "Barometric Vertical Rate Age" for the fields collection.
			IField barometricVertRateAgeField = new FieldClass ( );
			IFieldEdit barometricVertRateAgeFieldEdit = ( IFieldEdit ) barometricVertRateAgeField;
			barometricVertRateAgeFieldEdit.Name_2 = "Barometric_Vert_Rate_Age";
			barometricVertRateAgeFieldEdit.AliasName_2 = "Barometric Vertical Rate Age";
			barometricVertRateAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			barometricVertRateAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( barometricVertRateAgeFieldEdit );

			// Create a text field called "IAS Data Age" for the fields collection.
			IField iasDataAgeField = new FieldClass ( );
			IFieldEdit iasDataAgeFieldEdit = ( IFieldEdit ) iasDataAgeField;
			iasDataAgeFieldEdit.Name_2 = "IAS_Data_Age";
			iasDataAgeFieldEdit.AliasName_2 = "IAS Data Age";
			iasDataAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			iasDataAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( iasDataAgeFieldEdit );

			// Create a text field called "Mach Number Data Age" for the fields collection.
			IField machNumbDataAgeField = new FieldClass ( );
			IFieldEdit machNumbDataAgeFieldEdit = ( IFieldEdit ) machNumbDataAgeField;
			machNumbDataAgeFieldEdit.Name_2 = "Mach_Numb_Data_Age";
			machNumbDataAgeFieldEdit.AliasName_2 = "Mach Number Data Age";
			machNumbDataAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			machNumbDataAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( machNumbDataAgeFieldEdit );

			// Create a text field called "Barometric Pressure Settings Data Age" for the fields collection.
			IField barometricPressSetingDataAgeField = new FieldClass ( );
			IFieldEdit barometricPressSettingDataAgeFieldEdit = ( IFieldEdit ) barometricPressSetingDataAgeField;
			barometricPressSettingDataAgeFieldEdit.Name_2 = "Bar_Press_Data_Age";
			barometricPressSettingDataAgeFieldEdit.AliasName_2 = "Barometric Pressure Settings Data Age";
			barometricPressSettingDataAgeFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			barometricPressSettingDataAgeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( barometricPressSettingDataAgeFieldEdit );

			// Create a text field called "Calculated Track Geometric Altitude" for the fields collection.
			IField calcTrackGeomAltField = new FieldClass ( );
			IFieldEdit calcTrackGeomAltFieldEdit = ( IFieldEdit ) calcTrackGeomAltField;
			calcTrackGeomAltFieldEdit.Name_2 = "Calc_Track_Geom_Altitude";
			calcTrackGeomAltFieldEdit.AliasName_2 = "Calculated Track Geometric Altitude";
			calcTrackGeomAltFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			calcTrackGeomAltFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( calcTrackGeomAltFieldEdit );

			// Create a text field called "QNH Correction" for the fields collection.
			IField qnhCorrField = new FieldClass ( );
			IFieldEdit qnhCorrFieldEdit = ( IFieldEdit ) qnhCorrField;
			qnhCorrFieldEdit.Name_2 = "QNH_Correction";
			qnhCorrFieldEdit.AliasName_2 = "QNH Correction";
			qnhCorrFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			qnhCorrFieldEdit.Length_2 = 45;
			fieldsEdit.AddField ( qnhCorrFieldEdit );

			// Create a text field called "Barometric Altitude" for the fields collection.
			IField barAltField = new FieldClass ( );
			IFieldEdit barAltFieldEdit = ( IFieldEdit ) barAltField;
			barAltFieldEdit.Name_2 = "Barometric_Altitude";
			barAltFieldEdit.AliasName_2 = "Barometric Altitude";
			barAltFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			barAltFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( barAltFieldEdit );

			// Create a text field called "Calculated Rate of Climb/Descent" for the fields collection.
			IField rateClimbDescentField = new FieldClass ( );
			IFieldEdit rateClimbDescentFieldEdit = ( IFieldEdit ) rateClimbDescentField;
			rateClimbDescentFieldEdit.Name_2 = "Calc_Rate_Climb_Descent";
			rateClimbDescentFieldEdit.AliasName_2 = "Calculated Rate of Climb/Descent";
			rateClimbDescentFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			rateClimbDescentFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( rateClimbDescentFieldEdit );

			// Create a text field called "Length" for the fields collection.
			IField lengthField = new FieldClass ( );
			IFieldEdit lengthFieldEdit = ( IFieldEdit ) lengthField;
			lengthFieldEdit.Name_2 = "Length";
			lengthFieldEdit.AliasName_2 = "Length";
			lengthFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			lengthFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( lengthFieldEdit );

			// Create a text field called "Orientation" for the fields collection.
			IField orientationField = new FieldClass ( );
			IFieldEdit orientationFieldEdit = ( IFieldEdit ) orientationField;
			orientationFieldEdit.Name_2 = "Orientation";
			orientationFieldEdit.AliasName_2 = "Orientation";
			orientationFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			orientationFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( orientationFieldEdit );

			// Create a text field called "Width" for the fields collection.
			IField widthField = new FieldClass ( );
			IFieldEdit widthFieldEdit = ( IFieldEdit ) widthField;
			widthFieldEdit.Name_2 = "Width";
			widthFieldEdit.AliasName_2 = "Width";
			widthFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
			widthFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( widthFieldEdit );

			// Create a text field called "Vehicle Fleet Identification" for the fields collection.
			IField vehicleFleetIdField = new FieldClass ( );
			IFieldEdit vehicleFleetIdFieldEdit = ( IFieldEdit ) vehicleFleetIdField;
			vehicleFleetIdFieldEdit.Name_2 = "Vehicle_Fleet_Identification";
			vehicleFleetIdFieldEdit.AliasName_2 = "Vehicle Fleet Identification";
			vehicleFleetIdFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			vehicleFleetIdFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( vehicleFleetIdFieldEdit );

			// Create a text field called "Track Mode 2 Code" for the fields collection.
			IField trackMode2CodeField = new FieldClass ( );
			IFieldEdit trackMode2CodeFieldEdit = ( IFieldEdit ) trackMode2CodeField;
			trackMode2CodeFieldEdit.Name_2 = "Track_Mode_2_Code";
			trackMode2CodeFieldEdit.AliasName_2 = "Track Mode 2 Code";
			trackMode2CodeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			trackMode2CodeFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( trackMode2CodeFieldEdit );

			// Use IFieldChecker to create a validated fields collection.
			IFieldChecker fieldChecker = new FieldCheckerClass ( );
			IEnumFieldError enumFieldError = null;
			IFields validatedFields = null;
			fieldChecker.ValidateWorkspace = ( IWorkspace ) featureWorkspace;
			fieldChecker.Validate ( fields, out enumFieldError, out validatedFields );

			// The enumFieldError enumerator can be inspected at this point to determine 
			// which fields were modified during validation.

			// Create the feature class. Note that the CLSID parameter is null - this indicates to use the
			// default CLSID, esriGeodatabase.Feature (acceptable in most cases for feature classes).
			IFeatureClass featureClass = featureWorkspace.CreateFeatureClass ( featureClassName, validatedFields,
			  null, classExtensionUID, esriFeatureType.esriFTSimple, "Shape", "" );

			return featureClass;
		}

		private ITable CreateTrackNumbFeatClass ( UID classExtensionUID, IFeatureWorkspace featureWorkspace )
		{
			// Create a fields collection for the feature class.
			IFields fields = new FieldsClass ( );
			IFieldsEdit fieldsEdit = ( IFieldsEdit ) fields;

			// Add an object ID field to the fields collection. This is mandatory for feature classes.
			IField oidField = new FieldClass ( );
			IFieldEdit oidFieldEdit = ( IFieldEdit ) oidField;
			oidFieldEdit.Name_2 = "OID";
			oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
			fieldsEdit.AddField ( oidField );

			// Create a text field called "SAC" for the fields collection.
			IField sacField = new FieldClass ( );
			IFieldEdit sacFieldEdit = ( IFieldEdit ) sacField;
			sacFieldEdit.Name_2 = "Data_Id";
			sacFieldEdit.AliasName_2 = "Primary Id";
			sacFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
			sacFieldEdit.Length_2 = 20;
			fieldsEdit.AddField ( sacField );

			// The enumFieldError enumerator can be inspected at this point to determine 
			// which fields were modified during validation.

			// Create the feature class. Note that the CLSID parameter is null - this indicates to use the
			// default CLSID, esriGeodatabase.Feature (acceptable in most cases for feature classes).
			return  featureWorkspace.CreateTable( "TrackNumb", fields, null, null, "");
		}

		private void GetAircraftDerivedData ( byte[] bytes, AsterixData data, ref int index )
		{
			List<int> olanlar = new List<int> ( );
			int mask;
			int indexField = 1;
			byte test = bytes[ index ];
			for ( int i = 0; i < 4; i++ )
			{
				mask = 128;
				test = bytes[ index ];
				for ( int j = 1; j < 8; j++, indexField++ )
				{
					bool hasField = ( test & mask ) != 0;
					if ( hasField )
					{
						olanlar.Add ( indexField );
						if ( !aircraftSubFields.Contains ( indexField ) )
							aircraftSubFields.Add ( indexField );
					}
					mask >>= 1;
				}
				index++;
				if ( ( test & 1 ) == 0 )
					break;
			}
			for ( int i = 0; i < olanlar.Count; i++ )
			{
				switch ( olanlar[ i ] )
				{
					case 1:
						data.TargetAddress = bytes[ index ] | bytes[ index + 1 ] | bytes[ index + 2 ];
						index += 3;
						break;
					case 2:
						data.TargetIdentification = GetTargetIdentification ( bytes, 0, ref index );
						break;
					case 3:
						data.MagneticHeading = GetMagneticHeading(bytes, ref index);
						break;
					case 4:
						GetAirspeed ( bytes, data, ref index );
						break;
					case 5:
						GetTrueAirSpeed ( bytes, data, ref index );
						break;
					case 6:
						GetSelectedAltitude ( bytes, data, ref index );
						break;
					case 7:
						GetFinalStateAltitude ( bytes, data, ref index );
						break;
					case 10:
						GetACAS_FlightStatus ( bytes, data, ref index );
						break;
					case 13:
						GetBarometricVerticalRate ( bytes, data, ref index );
						break;					
					case 26:
						GetIndicatedAirspeed ( bytes, data, ref index );
						break;
					case 27:
						GetMachNumber ( bytes, data, ref index );
						break;
					case 28:
						GetBarometricPressureSetting ( bytes, data, ref index );
						break;											
					default:
						throw new Exception ( "Aircraft Derived Data - Field no: " + olanlar[ i ] );
				}
			}
		}

		private void GetBarometricPressureSetting ( byte[] bytes, AsterixData data, ref int index )
		{
			data.BarometricVerticalRate = ( ( ( bytes[ index ] & 8 ) << 8 ) | bytes[ index + 1 ] ) * 0.1;
			index += 2;
		}

		private void GetMachNumber ( byte[] bytes, AsterixData data, ref int index )
		{
			data.MachNumber = ( ( bytes[ index ] << 8 ) | ( bytes[ index + 1 ] ) ) * 0.008;
			index += 2;
		}

		private void GetIndicatedAirspeed ( byte[] bytes, AsterixData data, ref int index )
		{
			data.IAS = ( bytes[ index ] << 8 ) | ( bytes[ index + 1 ] );
			index += 2;
		}

		private void GetBarometricVerticalRate ( byte[] bytes, AsterixData data, ref int index )
		{
			int sign = bytes[ index ] & 128;
			data.BarometricVerticalRate = ( ( ( bytes[ index ] & 127 ) << 8 ) | bytes[ index + 1 ] );
			if ( sign != 0 )
				data.BarometricVerticalRate = data.BarometricVerticalRate - 65536;
			data.BarometricVerticalRate *= 6.25;
			index += 2;
		}

		private void GetACAS_FlightStatus ( byte[] bytes, AsterixData data, ref int index )
		{
			int t = ( bytes[ index ] & 224 ) >> 5;
			switch ( t )
			{
				case 0:
					data.CommunicationCapability = "No communications capability";
					break;
				case 1:
					data.CommunicationCapability = "Comm. A and Comm. B capability";
					break;
				case 2:
					data.CommunicationCapability = "Comm. A, Comm. B and Uplink ELM";
					break;
				case 3:
					data.CommunicationCapability = "Comm. A, Comm. B, Uplink ELM and Downlink ELM";
					break;
				case 4:
					data.CommunicationCapability = "Level 5 Transponder capability";
					break;
			}
			t = ( bytes[ index ] & 28 ) >> 2;
			switch ( t )
			{
				case 0:
					data.FlightStatus = "No alert, no SPI, aircraft airborne";
					break;
				case 1:
					data.FlightStatus = "No alert, no SPI, aircraft on ground";
					break;
				case 2:
					data.FlightStatus = "Alert, no SPI, aircraft airborne";
					break;
				case 3:
					data.FlightStatus = "Alert, no SPI, aircraft on ground";
					break;
				case 4:
					data.FlightStatus = "Alert, SPI, aircraft airborne or on ground";
					break;
				case 5:
					data.FlightStatus = "No alert, SPI, aircraft airborne or on ground";
					break;
			}
			index++;
			data.SpecificServiceCapability = "No";
			if ( ( bytes[ index ] & 128 ) != 0 )
				data.SpecificServiceCapability = "Yes";
			data.AltitudeReportingCapability = "100 ft resolution";
			if ( ( bytes[ index ] & 64 ) != 0 )
				data.AltitudeReportingCapability = "25 ft resolution";
			data.AircraftIdentificationCapability = "No";
			if ( ( bytes[ index ] & 32 ) != 0 )
				data.AircraftIdentificationCapability = "Yes";
			index++;
		}

		private void GetTrajectoryIntentStatus ( byte[] bytes, AsterixData data, ref int index )
		{
			data.TrajectoryIntentDataAvailability = "Available";
			if ( ( bytes[ index ] & 128 ) != 0 )
				data.TrajectoryIntentDataAvailability = "Not available";
			data.TrajectoryIntentDataValidity = "Valid";
			if ( ( bytes[ index ] & 64 ) != 0 )
				data.TrajectoryIntentDataValidity = "Not valid";
			if ( ( bytes[ index ] & 1 ) != 0 )
			{

			}
			index++;
		}

		private void GetFinalStateAltitude ( byte[] bytes, AsterixData data, ref int index )
		{
			data.ManageVerticalMode = "Not active";
			if ( ( bytes[ index ] & 128 ) != 0 )
				data.ManageVerticalMode = "Active";

			data.AltitudeHold = "Not active";
			if ( ( bytes[ index ] & 64 ) != 0 )
				data.AltitudeHold = "Active";

			data.ApproachMode = "Not active";
			if ( ( bytes[ index ] & 32 ) != 0 )
				data.ApproachMode = "Active";
			int sign = bytes[ index ] & 16;
			data.FinalStateAltitude = ( ( ( bytes[ index ] & 15 ) << 8 ) | bytes[ index + 1 ] );
			if ( sign != 0 )
				data.FinalStateAltitude = data.FinalStateAltitude - 8192;
			data.FinalStateAltitude *= 25;
			index += 2;
		}

		private void GetSelectedAltitude ( byte[] bytes, AsterixData data, ref int index )
		{
			if ( ( bytes[ index ] & 128 ) != 0 )
			{
				int source = ( bytes[ index ] & 96 );
				int t = source >> 5;
				int res = t & 3;
				switch ( res )
				{
					case 0:
						data.AltitudeSource = "Unknown";
						break;
					case 1:
						data.AltitudeSource = "Aircraft Altitude";
						break;
					case 2:
						data.AltitudeSource = "FCU/MCP";
						break;
					case 3:
						data.AltitudeSource = "FMS";
						break;
				}
			}
			int sign = bytes[ index ] & 16;
			data.Altitude = ( ( ( bytes[ index ] & 15 ) << 8 ) | bytes[ index + 1 ] );
			if ( sign != 0 )
				data.Altitude = data.Altitude - 8192;
			data.Altitude *= 25;
			index += 2;
		}

		private void GetTrueAirSpeed ( byte[] bytes, AsterixData data, ref int index )
		{
			data.TAS = ( bytes[ index ] << 8 ) | ( bytes[ index + 1 ] );
			index += 2;
		}

		private void GetAirspeed ( byte[] bytes, AsterixData data, ref int index )
		{
			//data.isIAS = ( bytes[ index ] & 128 ) != 0;
			//int test = ( bytes[ index + 1 ] << 8 ) | ( bytes[ index ] & 127 );
			//double coef = 0.0055;
			//double result = test * coef;
			index += 2;
		}

		private double GetMagneticHeading ( byte[] bytes, ref int index )
		{
			Int64 test = ( bytes[ index ] << 8 ) | ( bytes[ index + 1] );
			double coef = 0.0055;
			double result = test * coef;
			index += 2;
			return result;
		}

		private string GetTargetIdentification ( byte[] bytes, int offset, ref int index )
		{
			Int64 test = ( bytes[ index + offset ] << 40 ) | ( bytes[ index + offset + 1 ] << 32 ) | ( bytes[ index + offset + 2 ] << 24 ) | ( bytes[ index + offset + 3 ] << 16 ) | ( bytes[ index + offset + 4 ] << 8 ) | bytes[ index + offset + 5 ];
			string str = "";
			byte code;
			char tmp = ' ';
			for ( int i = 0; i < 8; i++ )
			{
				byte t = ( byte ) ( test & 63 );//( cedvel[ test & 63 ] >> 2 );
				byte res = ( byte ) ( ( t & 48 ) >> 4 );
				switch ( res )
				{
					case 0:
					case 1:
						if ( res == 0 && t == 0 )
							str += ' ';
						else
						{
							//str += ( char ) ( ( test & 31 ) + 'A' - 1 );
							if ( res == 0 )
								tmp = ( char ) ( ( t & 15 ) + 'A' - 1 );
							else if ( res == 1 )
								tmp = ( char ) ( ( t & 15 ) + 'P' );
							//tmp = ( char ) ( ( t & 15 ) + 'A' - 1 );
							str += tmp;
						}
						break;
					case 2:
						str += ' ';
						break;
					case 3:
						code = ( byte ) ( t & 15 );
						str += ( char ) ( ( t & 15 ) + '0' );
						break;
				}
				test >>= 6;
			}

			index += ( 6 + offset );
			return str;
		}

		private void GetTrackMode3ACode ( byte[] bytes, AsterixData data, ref int index )
		{
			bool changed = (bytes[ index ] & 32) != 0;			
			if ( changed )
				data.TrackMode3CodeChanged = true;
			int tmp = bytes[ index + 1 ] | ( bytes[ index ] << 8 );
			int t = 0;
			
			for ( int i = 0; i < 4; i++ )
			{
				t <<= 3;
				t += tmp & 7;
				tmp >>= 3;
			}
			data.TrackMode3Code = System.Convert.ToString ( t, 8 );
			index+=2;
		}

		private void GetAcceleration ( byte[] bytes, AsterixData data, ref int index )
		{
			double coef = 0.25;
			Int64 itmp = bytes[ index ];
			index ++;
			data.AccelerationX = itmp * coef;

			itmp = bytes[ index ];
			index ++;
			data.AccelerationY = itmp * coef;
		}

		private void GetTrackVelocity ( byte[] bytes, AsterixData data, ref int index )
		{
			double coef = 0.25;
			Int64 itmp = bytes[ index + 1 ] | ( bytes[ index ] << 8 );
			index += 2;
			data.VelocityX = itmp * coef;

			itmp = bytes[ index + 1 ] | ( bytes[ index ] << 8 );
			index += 2;
			data.VelocityY = itmp * coef;
		}

		private TimeSpan GetTime ( byte[] bytes, ref int index )
		{
			Int64 itmp = bytes[ index + 2 ] | ( bytes[ index + 1 ] << 8 ) | ( bytes[ index ] << 16 );
			index += 3;
			double result = itmp / 128.0;
			int mSec = ( int ) ( ( result - ( int ) result ) * 1000 );
			TimeSpan time = new TimeSpan ( 0, 0, 0, ( int ) result, mSec );
			//TimeSpan t = new TimeSpan ( itmp );
			//System.DateTime dtDateTime = new DateTime ( 1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc );
			//dtDateTime = dtDateTime.AddSeconds ( result );
			return time;
		}

		private void GetServiceIdentification ( byte[] bytes, AsterixData data, ref int index )
		{
			data.ServiceIdentification = bytes[ index ].ToString ( "X" );
			index++;
		}

		private void GetSourceIdentifier ( byte[] bytes, AsterixData data, ref int index )
		{
			data.SAC = int.Parse ( bytes[ index ].ToString ( "X" ) );
			data.SIC = int.Parse ( bytes[ index + 1 ].ToString ( "X" ) );
			index += 2;
		}

		public void GetPrjCoords ( byte[] bytes, AsterixData data, ref int index )
		{
			double coef = 0.5;
			Int64 itmp = bytes[ index + 2 ] | ( bytes[ index + 1 ] << 8 ) | ( bytes[ index ] << 16 );
			index += 3;
			data.XPrj = itmp * coef;

			itmp = bytes[ index + 2 ] | ( bytes[ index + 1 ] << 8 ) | ( bytes[ index ] << 16 );
			index += 3;
			data.YPrj = itmp * coef;
		}

		public void GetGeoCoords ( byte[] bytes, AsterixData data, ref int index )
		{
			double coef = 180.0 / 0x2000000;
			Int64 itmp = bytes[ index + 3 ] | ( bytes[ index + 2 ] << 8 ) | ( bytes[ index + 1 ] << 16 ) | ( bytes[ index ] << 24 );
			index += 4;
			data.XGeo = itmp * coef;

			itmp = bytes[ index + 3 ] | ( bytes[ index + 2 ] << 8 ) | ( bytes[ index + 1 ] << 16 ) | ( bytes[ index ] << 24 );
			index += 4;
			data.YGeo = itmp * coef;
		}		
	}
}