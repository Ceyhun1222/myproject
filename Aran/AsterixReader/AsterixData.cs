using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsterixReader
{
	public class AsterixData
	{
		public AsterixData ( )
		{
			AltitudeSource = "No source information provided";
			Altitude = double.NaN;
			XGeo = double.NaN;
			YGeo = double.NaN;
			XPrj = double.NaN;
			YPrj = double.NaN;
			VelocityX = double.NaN;
			VelocityY = double.NaN;
			AccelerationX = double.NaN;
			AccelerationY = double.NaN;
			BarometricVerticalRate = double.NaN;
			TAS = double.NaN;
			IAS = double.NaN;
			FinalStateAltitude = double.NaN;
			BarometricVerticalRate = double.NaN;
			MachNumber = double.NaN;
			TrackAge = double.NaN;
			PSR_Age = double.NaN;
			SSR_Age = double.NaN;
			Mode_S_Age = double.NaN;
			ADS_C_Age = double.NaN;
			ES_Age = double.NaN;
			VDL_Age = double.NaN;
			UAT_Age = double.NaN;
			Loop_Age = double.NaN;
			Multilateration_Age = double.NaN;
			Measured_Flight_Level_Age = double.NaN;
			Mode_1_Age = double.NaN;
			Mode_2_Age = double.NaN;
			Mode_3_A_Age = double.NaN;
			Mode_4_Age = double.NaN;
			Mode_5_Age = double.NaN;
			MagneticHeading_Age = double.NaN;
			IAS_Mach_Age = double.NaN;
			TAS_Age = double.NaN;
			SelectedAltitude_Age = double.NaN;
			FinalStateAltitude_Age = double.NaN;
			TrajectoryIntent_Age = double.NaN;
			ACAS_Age = double.NaN;
			StatusReported_ADS_B_Age = double.NaN;
			BarometricVerticalRate_Age = double.NaN;
			ACAS_Resolution_Advisory_Report_Age = double.NaN;
			IAS_Data_Age = double.NaN;
			MachNumber_Data_Age = double.NaN;
			BarometricPressureSettingsDataAge = double.NaN;
			Measured_Flight_Level = double.NaN;
			CalculatedTrackGeometrciAltitude = double.NaN;
			BarometricAltitude = double.NaN;
			RateClimb_Descent = double.NaN;
			Orientation = double.NaN;
		}

		public double XGeo
		{
			get;
			set;
		}

		public double YGeo
		{
			get;
			set;
		}

		public double XPrj
		{
			get;
			set;
		}

		public double YPrj
		{
			get;
			set;
		}

		public double VelocityX
		{
			get;
			set;
		}

		public double VelocityY
		{
			get;
			set;
		}

		public double AccelerationX
		{
			get;
			set;
		}

		public double AccelerationY
		{
			get;
			set;
		}

		public bool TrackMode3CodeChanged
		{
			get;
			set;
		}

		public string TrackMode3Code
		{
			get;
			set;
		}

		public DateTime Time
		{
			get;
			set;
		}

		public string TargetIdentification
		{
			get;
			set;
		}

		public double TargetAddress
		{
			get;
			set;
		}

		public int SAC
		{
			get;
			set;
		}

		public int SIC
		{
			get;
			set;
		}

		public double TAS
		{
			get;
			set;
		}

		public double IAS
		{
			get;
			set;
		}

		public string ServiceIdentification
		{
			get;
			set;
		}

		public double Altitude
		{
			get;
			set;
		}

		public string AltitudeSource
		{
			get;
			set;
		}

		public string ManageVerticalMode
		{
			get;
			set;
		}

		public string AltitudeHold
		{
			get;
			set;
		}

		public string ApproachMode
		{
			get;
			set;
		}

		public double FinalStateAltitude
		{
			get;
			set;
		}

		public string TrajectoryIntentDataAvailability
		{
			get;
			set;
		}

		public string TrajectoryIntentDataValidity
		{
			get;
			set;
		}

		public string CommunicationCapability
		{
			get;
			set;
		}

		public string FlightStatus
		{
			get;
			set;
		}

		public string SpecificServiceCapability
		{
			get;
			set;
		}

		public string AltitudeReportingCapability
		{
			get;
			set;
		}

		public string AircraftIdentificationCapability
		{
			get;
			set;
		}

		public double BarometricVerticalRate
		{
			get; set;
		}

		public double MagneticHeading
		{
			get;
			set;
		}

		public double MachNumber
		{
			get;
			internal set;
		}
		
		public double TrackNumber
		{
			get;
			internal set;
		}

		public double TrackAge
		{
			get;
			set;
		}

		public double PSR_Age
		{
			get;
			set;
		}

		public double SSR_Age
		{
			get;
			set;
		}

		public double Mode_S_Age
		{
			get;
			set;
		}

		public double ADS_C_Age
		{
			get;
			set;
		}

		public double ES_Age
		{
			get;
			set;
		}

		public double VDL_Age
		{
			get;
			set;
		}

		public double UAT_Age
		{
			get;
			set;
		}

		public double Loop_Age
		{
			get;
			set;
		}

		public double Multilateration_Age
		{
			get;
			set;
		}

		public string TRANS
		{
			get;
			set;
		}

		public string LONG
		{
			get;
			set;
		}

		public string VERT
		{
			get;
			set;
		}

		public string ADF
		{
			get;
			set;
		}

		public double Measured_Flight_Level_Age
		{
			get;
			set;
		}

		public double Mode_1_Age
		{
			get;
			set;
		}

		public double Mode_2_Age
		{
			get;
			set;
		}

		public double Mode_3_A_Age
		{
			get;
			set;
		}

		public double Mode_4_Age
		{
			get;
			set;
		}

		public double Mode_5_Age
		{
			get;
			set;
		}

		public double MagneticHeading_Age
		{
			get;
			set;
		}

		public double IAS_Mach_Age
		{
			get;
			set;
		}

		public double TAS_Age
		{
			get;
			set;
		}

		public double SelectedAltitude_Age
		{
			get;
			set;
		}

		public double FinalStateAltitude_Age
		{
			get;
			set;
		}

		public double TrajectoryIntent_Age
		{
			get;
			set;
		}

		public double ACAS_Age
		{
			get;
			set;
		}

		public double StatusReported_ADS_B_Age
		{
			get;
			set;
		}

		public double BarometricVerticalRate_Age
		{
			get;
			set;
		}

		public double ACAS_Resolution_Advisory_Report_Age
		{
			get;
			set;
		}

		public double IAS_Data_Age
		{
			get;
			set;
		}

		public double MachNumber_Data_Age
		{
			get;
			set;
		}

		public double BarometricPressureSettingsDataAge
		{
			get;
			set;
		}

		public double Measured_Flight_Level
		{
			get;
			set;
		}

		public double CalculatedTrackGeometrciAltitude
		{
			get;
			set;
		}

		public string QNH_Corection
		{
			get;
			set;
		}

		public double BarometricAltitude
		{
			get;
			set;
		}

		public double RateClimb_Descent
		{
			get;
			set;
		}

		public double Length
		{
			get;
			set;
		}

		public double Orientation
		{
			get;
			set;
		}

		public double Width
		{
			get;
			set;
		}

		public string VehicleFleetId
		{
			get;
			set;
		}

		public string TrackMode2Code
		{
			get;
			set;
		}

		public List<ComposedTrackNumber> TrackNumbers
		{
			get;
			set;
		}
	}

	public class TrackStatus
	{		
		public string CNF
		{
			get; set;
		}

		public string SourceTrackAltitude
		{
			get; set;
		}

		public string MostReliableHeight
		{
			get; set;
		}

		public string SPI
		{
			get; set;
		}

		public string SensorType
		{
			get; set;
		}
		public string SIM
		{
			get;
			internal set;
		}
		public string TSE
		{
			get;
			internal set;
		}
		public string TSB
		{
			get;
			internal set;
		}
		public string FPC
		{
			get;
			internal set;
		}
		public string AFF
		{
			get;
			internal set;
		}
		public string STP
		{
			get;
			internal set;
		}
		public string KOS
		{
			get;
			internal set;
		}
		public string AMA
		{
			get;
			internal set;
		}
		public string MD4
		{
			get;
			internal set;
		}
		public string ME
		{
			get;
			internal set;
		}
		public string MI
		{
			get;
			internal set;
		}
		public string MD5
		{
			get;
			internal set;
		}
		public string CST
		{
			get;
			internal set;
		}
		public string PSR
		{
			get;
			internal set;
		}
		public string SSR
		{
			get;
			internal set;
		}
		public string MDS
		{
			get;
			internal set;
		}
		public string ADS
		{
			get;
			internal set;
		}
		public string SUC
		{
			get;
			internal set;
		}
		public string AAC
		{
			get;
			internal set;
		}
		public string SDS
		{
			get;
			internal set;
		}
		public string EMS
		{
			get;
			internal set;
		}
		public string PFT
		{
			get;
			internal set;
		}
		public string FPLT
		{
			get;
			internal set;
		}
		public string DUPT
		{
			get;
			internal set;
		}
		public string DUPF
		{
			get;
			internal set;
		}
		public string DUPM
		{
			get;
			internal set;
		}
	}

	public class ComposedTrackNumber
	{
		public int SystemUnitId
		{
			get;
			set;
		}

		public int TrackNumber
		{
			get;
			set;
		}
	}


}
