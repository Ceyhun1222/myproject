using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using Aran.PANDA.Common;

namespace Aran.PANDA.Constants
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct VORData
	{
		public double Range;
		public double FA_Range;
		public double InitWidth;
		public double SplayAngle;
		public double TrackingTolerance;
		public double IntersectingTolerance;
		public double ConeAngle;
		public double TrackAccuracy;
		public double LateralDeviationCoef;
		public double EnRouteTrackingToler;
		public double EnRouteTracking2Toler;
		public double EnRouteInterToler;
		public double EnRoutePrimAreaWith;
		public double EnRouteSecAreaWith;
		public double OnNAVRadius;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NDBData
	{
		public double Range;
		public double FA_Range;
		public double InitWidth;
		public double SplayAngle;
		public double TrackingTolerance;
		public double IntersectingTolerance;
		public double ConeAngle;
		public double TrackAccuracy;
		public double Entry2ConeAccuracy;
		public double LateralDeviationCoef;
		public double EnRouteTrackingToler;
		public double EnRouteTracking2Toler;
		public double EnRouteInterToler;
		public double EnRoutePrimAreaWith;
		public double EnRouteSecAreaWith;
		public double OnNAVRadius;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct DMEData
	{
		public double Range;
		public double MinimalError;
		public double ErrorScalingUp;
		public double SlantAngle;
		public double TP_div;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct LLZData
	{
		public double Range;
		public double TrackingTolerance;
		public double IntersectingTolerance;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class NameAndIndex
	{
		public string TypeName_Renamed { get; set; }
		public int TypeIndex { get; set; }
	}


	public class NavaidsConstant
	{

	    private string[] NavTypeNames = {"VOR", "DME", "NDB", "LLZ", "TACAN", "Radar FIX"};
		public NavaidsConstant()
		{
		}

		public void LoadFromFile(string fileName)
		{
			double Value;
			double Multiplier;
			string ParName;
			OleDbDataReader navaidTypesReader;

			_VOR = new VORData();
			_DME = new DMEData();
			_NDB = new NDBData();
			_LLZ = new LLZData();

			if (!OpenDbfConnection(fileName))
				throw new Exception("Cannot open file");

			OleDbCommand navaidCmd = new OleDbCommand("Select * from Navaids", _conn);
			OleDbCommand navaidTypesCmd = _conn.CreateCommand();
			try
			{
				OleDbDataReader navaidReader = navaidCmd.ExecuteReader();
				while (navaidReader.Read())
				{
					navaidTypesCmd.CommandText = "select * from " + navaidReader["name"].ToString();
					navaidTypesReader = navaidTypesCmd.ExecuteReader();
					while (navaidTypesReader.Read())
					{
						ParName = (string)navaidTypesReader["PARAM_NAME"];
						Multiplier = (double)navaidTypesReader["MULTIPLIER"];
						Value = (double)navaidTypesReader["VALUE"] * Multiplier;
						if (navaidTypesReader["UNIT"].ToString() == "rad")
							Value = ARANMath.RadToDeg(Value);

						switch (navaidReader["name"].ToString())
						{
							case "VOR":
								if (ParName == "Range")
									_VOR.Range = Value;
								else if (ParName == "FA Range")
									_VOR.FA_Range = Value;
								else if (ParName == "Initial width")
									_VOR.InitWidth = Value;
								else if (ParName == "Splay angle")
									_VOR.SplayAngle = Value;
								else if (ParName == "Tracking tolerance")
									_VOR.TrackingTolerance = Value;
								else if (ParName == "Intersecting tolerance")
									_VOR.IntersectingTolerance = Value;
								else if (ParName == "Cone angle")
									_VOR.ConeAngle = Value;
								else if (ParName == "Track accuracy")
									_VOR.TrackAccuracy = Value;
								else if (ParName == "Lateral deviation coef.")
									_VOR.LateralDeviationCoef = Value;
								else if (ParName == "EnRoute Tracking toler")
									_VOR.EnRouteTrackingToler = Value;
								else if (ParName == "EnRoute Tracking2 toler")
									_VOR.EnRouteTracking2Toler = Value;
								else if (ParName == "EnRoute Inter toler")
									_VOR.EnRouteInterToler = Value;
								else if (ParName == "EnRoute PrimArea With")
									_VOR.EnRoutePrimAreaWith = Value;
								else if (ParName == "EnRoute SecArea With")
									_VOR.EnRouteSecAreaWith = Value;
								break;

							case "NDB":

								if (ParName == "Range")
									_NDB.Range = Value;
								else if (ParName == "FA Range")
									_NDB.FA_Range = Value;
								else if (ParName == "Initial width")
									_NDB.InitWidth = Value;
								else if (ParName == "Splay angle")
									_NDB.SplayAngle = Value;
								else if (ParName == "Tracking tolerance")
									_NDB.TrackingTolerance = Value;
								else if (ParName == "Intersecting tolerance")
									_NDB.IntersectingTolerance = Value;
								else if (ParName == "Cone angle")
									_NDB.ConeAngle = Value;
								else if (ParName == "Track accuracy")
									_NDB.TrackAccuracy = Value;
								else if (ParName == "Entry into the cone accuracy")
									_NDB.Entry2ConeAccuracy = Value;
								else if (ParName == "Lateral deviation coef.")
									_NDB.LateralDeviationCoef = Value;
								else if (ParName == "EnRoute Tracking toler")
									_NDB.EnRouteTrackingToler = Value;
								else if (ParName == "EnRoute Tracking2 toler")
									_NDB.EnRouteTracking2Toler = Value;
								else if (ParName == "EnRoute Inter toler")
									_NDB.EnRouteInterToler = Value;
								else if (ParName == "EnRoute PrimArea With")
									_NDB.EnRoutePrimAreaWith = Value;
								else if (ParName == "EnRoute SecArea With")
									_NDB.EnRouteSecAreaWith = Value;
								break;

							case "DME":
								if (ParName == "Range")
									_DME.Range = Value;
								else if (ParName == "Minimal Error")
									_DME.MinimalError = Value;
								else if (ParName == "Error Scaling Up")
									_DME.ErrorScalingUp = Value;
								else if (ParName == "Slant Angle")
									_DME.SlantAngle = Value;
								else if (ParName == "TP_div")
									_DME.TP_div = Value;
								break;

							case "LLZ":
								if (ParName == "Range")
									_LLZ.Range = Value;
								else if (ParName == "Tracking tolerance")
									_LLZ.TrackingTolerance = Value;
								else if (ParName == "Intersecting tolerance")
									_LLZ.IntersectingTolerance = Value;
								break;
						}
					}
					navaidTypesReader.Close();
				}
			}
			catch (Exception)
			{
                System.Windows.Forms.MessageBox.Show("Eror NavaidDatabase cann't read data");
			}

			//	VOR.OnNAVRadius = System.Math.Sin(ARANFunctions.DegToRad(VOR.TrackAccuracy + arTrackAccuracy.Value)) / System.Math.Sin(DegToRad(90.0 - arTrackAccuracy.Value)) * arOverHeadToler.Value;
			//	NDB.OnNAVRadius = System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy + arTrackAccuracy.Value)) / System.Math.Sin(DegToRad(90.0 - arTrackAccuracy.Value)) * arOverHeadToler.Value;
			CloseConnection();

		}

	    public string GetNavTypeName(eNavaidType eNavaidType)
	    {
	        if (eNavaidType == Common.eNavaidType.NONE)
	            return "WPT";
	        else
	            return NavTypeNames[(int)eNavaidType];
	    }

        public double OnNAVShift(eNavaidType NavType, double Hrel, Constants cons)
        {
            double onNavShift;
            if (NavType == eNavaidType.VOR)
            {
                onNavShift = VOR.OnNAVRadius / cons.Pansops[ePANSOPSData.arOverHeadToler].Value * Hrel * System.Math.Tan(ARANMath.DegToRad(VOR.ConeAngle));
            }
            else if ((NavType == eNavaidType.NDB) | (NavType == eNavaidType.LLZ) | (NavType == eNavaidType.TACAN))
            {
                onNavShift = NDB.OnNAVRadius / cons.Pansops[ePANSOPSData.arOverHeadToler].Value * Hrel * System.Math.Tan(ARANMath.DegToRad(NDB.ConeAngle));
            }
            else
            {
                onNavShift = -1000000.0;
            }
            return onNavShift;
        }

		private bool OpenDbfConnection(string pathFolder)
		{
			try
			{
				_conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathFolder + ";Extended Properties=dBASE IV;");
				_conn.Open();
				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}

		private void CloseConnection()
		{
			_conn.Close();
		}

		private VORData _VOR;
		private NDBData _NDB;
		private DMEData _DME;
		private LLZData _LLZ;
		private OleDbConnection _conn;

		public VORData VOR { get { return _VOR; } }
		public NDBData NDB { get { return _NDB; } }
		public DMEData DME { get { return _DME; } }
		public LLZData LLZ { get { return _LLZ; } }
		public int NavaidTypesCount;
	}
}
