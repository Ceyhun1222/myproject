using System;
using System.Data.OleDb;
using System.Diagnostics;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{		
	public class NavaidsDataBase
	{	
		
		public NavaidsDataBase(string constsInstallDir)
		{
			Vor = new VorData();
			Dme = new DmeData();
			Ndb = new NdbData();
			Llz = new LlzData();

			if ( !OpenDbfConnection ( constsInstallDir + "\\Navaids\\" ) )
				throw new Exception("Cannot open file");

			OleDbCommand navaidCmd = new OleDbCommand("Select * from Navaids", _conn);
			OleDbCommand navaidTypesCmd = _conn.CreateCommand();
			try
			{
				OleDbDataReader navaidReader = navaidCmd.ExecuteReader();
				while (navaidReader.Read())
				{
					navaidTypesCmd.CommandText = "select * from " + navaidReader["name"].ToString();
					var navaidTypesReader = navaidTypesCmd.ExecuteReader();
					while (navaidTypesReader.Read())
					{
						var parName = (string)navaidTypesReader["PARAM_NAME"];
						var multiplier = (double)navaidTypesReader["MULTIPLIER"];
						double tmp = ( double ) navaidTypesReader [ "VALUE" ];
						var value = (double)navaidTypesReader["VALUE"] * multiplier;
						if (navaidTypesReader["UNIT"].ToString() == "rad")
							value = ARANMath.RadToDeg(value);

						switch (navaidReader["name"].ToString())
						{
							case "VOR":
								if (parName == "Range")
									Vor.Range = value;
								else if (parName == "FA Range")
									Vor.FaRange = value;
								else if (parName == "Initial width")
									Vor.InitWidth = value;
								else if (parName == "Splay angle")
									Vor.SplayAngle = value;
								else if (parName == "Tracking tolerance")
									Vor.TrackingTolerance = value;
								else if (parName == "Intersecting tolerance")
									Vor.IntersectingTolerance = value;
								else if (parName == "Cone angle")
									Vor.ConeAngle = value;
								else if (parName == "Track accuracy")
									Vor.TrackAccuracy = value;
								else if (parName == "Lateral deviation coef.")
									Vor.LateralDeviationCoef = value;
								else if (parName == "EnRoute Tracking toler")
									Vor.EnRouteTrackingToler = value;
								else if (parName == "EnRoute Tracking2 toler")
									Vor.EnRouteTracking2Toler = value;
								else if (parName == "EnRoute Inter toler")
									Vor.EnRouteInterToler = value;
								else if (parName == "EnRoute PrimArea With")
									Vor.EnRoutePrimAreaWith = value;
								else if (parName == "EnRoute SecArea With")
									Vor.EnRouteSecAreaWith = value;
								break;

							case "NDB":

								if (parName == "Range")
									Ndb.Range = value;
								else if (parName == "FA Range")
									Ndb.FaRange = value;
								else if (parName == "Initial width")
									Ndb.InitWidth = value;
								else if (parName == "Splay angle")
									Ndb.SplayAngle = value;
								else if (parName == "Tracking tolerance")
									Ndb.TrackingTolerance = value;
								else if (parName == "Intersecting tolerance")
									Ndb.IntersectingTolerance = value;
								else if (parName == "Cone angle")
									Ndb.ConeAngle = value;
								else if (parName == "Track accuracy")
									Ndb.TrackAccuracy = value;
								else if (parName == "Entry into the cone accuracy")
									Ndb.Entry2ConeAccuracy = value;
								else if (parName == "Lateral deviation coef.")
									Ndb.LateralDeviationCoef = value;
								else if (parName == "EnRoute Tracking toler")
									Ndb.EnRouteTrackingToler = value;
								else if (parName == "EnRoute Tracking2 toler")
									Ndb.EnRouteTracking2Toler = value;
								else if (parName == "EnRoute Inter toler")
									Ndb.EnRouteInterToler = value;
								else if (parName == "EnRoute PrimArea With")
									Ndb.EnRoutePrimAreaWith = value;
								else if (parName == "EnRoute SecArea With")
									Ndb.EnRouteSecAreaWith = value;
								break;

							case "DME":
								if (parName == "Range")
									Dme.Range = value;
								else if (parName == "Minimal Error")
									Dme.MinimalError = value;
								else if (parName == "Error Scaling Up")
									Dme.ErrorScalingUp = value;
								else if (parName == "Slant Angle")
									Dme.SlantAngle = value;
								else if (parName == "TP_div")
									Dme.TpDiv = value;
								break;

							case "LLZ":

								if (parName == "Range")
									Llz.Range = value;
								else if (parName == "Tracking tolerance")
									Llz.TrackingTolerance = value;
								else if (parName == "Intersecting tolerance")
									Llz.IntersectingTolerance = value;
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
		
		//public double OnNAVShift(ref int NavType, ref double Hrel)
		//{
		//    double onNavShift;
		//    if (NavType == (int)eNavaidTupes.CodeVOR)
		//    {
		//        onNavShift = VOR.OnNAVRadius / InitHolding.Constants.PANSOPS.ConstantByIndex((int)ePANSOPSData.arOverHeadToler).value * Hrel * System.Math.Tan(ARANFunctions.DegToRad(VOR.ConeAngle));
		//    }
		//    else if ((NavType == (int)eNavaidTupes.CodeNDB) | (NavType == (int)eNavaidTupes.CodeLLZ) | (NavType ==(int)eNavaidTupes.CodeTACAN))
		//    {
		//        onNavShift = NDB.OnNAVRadius / InitHolding.Constants.PANSOPS.ConstantByIndex((int)ePANSOPSData.arOverHeadToler).value * Hrel * System.Math.Tan(ARANFunctions.DegToRad(NDB.ConeAngle));
		//    }
		//    else
		//    {
		//        onNavShift = -1000000.0;
		//    }
		//    return onNavShift;
		//}

		private  bool OpenDbfConnection(string pathFolder)
		{
			try
			{
				_conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pathFolder+";Extended Properties=dBASE IV;");
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

		public int NavaidTypesCount;
		public VorData Vor { get; }
		public NdbData Ndb { get; }
		public DmeData Dme { get; }
		private LlzData Llz { get; }
							   
		private OleDbConnection _conn;
		 
		//Nav.Name = pWPT.Name;
		//        TurnNav.ID = pWPT.ID;
		//        TurnNav.TypeCode = pWPT.TypeCode;
		//        TurnNav.TypeName_Renamed = pWPT.TypeName_Renamed;
		//        Tur// NavaidData2FixableNavaid(ref NavaidType Val_Renamed)
		//{
		//    FIXableNavaidType Res;

		//    Res.pPtGeo = Val_Renamed.pPtGeo;
		//    Res.pPtPrj = Val_Renamed.pPtPrj;
		//    Res.Name = Val_Renamed.Name;
		//    Res.CallSign = Val_Renamed.CallSign;
		//    Res.ID = Val_Renamed.ID;
		//    Res.MagVar = Val_Renamed.MagVar;

		//    Res.TypeName_Renamed = Val_Renamed.TypeName_Renamed;
		//    Res.TypeCode = Val_Renamed.TypeCode;
		//    Res.Range = Val_Renamed.Range;
		//    Res.Index = Val_Renamed.Index;
		//    Res.PairNavaidIndex = Val_Renamed.PairNavaidIndex;

		//    //    Res.GPAngle = Val.GPAngle
		//    Res.GP_RDH = Val_Renamed.GP_RDH;

		//    Res.Course = Val_Renamed.Course;
		//    Res.LLZ_THR = Val_Renamed.LLZ_THR;
		//    Res.SecWidth = Val_Renamed.SecWidth;
		//    //    Res.AngleWidth = Val.AngleWidth
		//    Res.ValCnt = -1;
		//    Res.pSignificantPoint = Val_Renamed.pSignificantPoint;
		//    Res.Tag = Val_Renamed.Tag;

		//    NavaidData2FixableNavaid = Res;
		//}

		//NavaidType FixableNavaid2NavaidData(ref FIXableNavaidType Val_Renamed)
		//{
		//    NavaidType Res;

		//    Res.pPtGeo = Val_Renamed.pPtGeo;
		//    Res.pPtPrj = Val_Renamed.pPtPrj;
		//    Res.Name = Val_Renamed.Name;
		//    Res.CallSign = Val_Renamed.CallSign;
		//    Res.ID = Val_Renamed.ID;
		//    Res.MagVar = Val_Renamed.MagVar;

		//    Res.TypeName_Renamed = Val_Renamed.TypeName_Renamed;
		//    Res.TypeCode = Val_Renamed.TypeCode;
		//    Res.Range = Val_Renamed.Range;
		//    Res.Index = Val_Renamed.Index;
		//    Res.PairNavaidIndex = Val_Renamed.PairNavaidIndex;

		//    //    Res.GPAngle = Val.GPAngle
		//    Res.GP_RDH = Val_Renamed.GP_RDH;

		//    Res.Course = Val_Renamed.Course;
		//    Res.LLZ_THR = Val_Renamed.LLZ_THR;
		//    Res.SecWidth = Val_Renamed.SecWidth;
		//    //    Res.AngleWidth = Val.AngleWidth
		//    Res.pSignificantPoint = Val_Renamed.pSignificantPoint;
		//    Res.Tag = Val_Renamed.Tag;

		//    FixableNavaid2NavaidData = Res;
		//}

		//public NavaidType TurnWPTToTurnNav(bool bSameFIX, ref FIXableNavaidTypE pNAV, ref WPT_FIXType pWPT)
		//{
		//    NavaidType TurnNav;

		//    if (bSameFIX)
		//    {
		//        TurnNav = FixableNavaid2NavaidData(pNAV);
		//    }
		//    else
		//    {
		//        TurnNav.pPtGeo = pWPT.pPtGeo;
		//        TurnNav.pPtPrj = pWPT.pPtPrj;

		//        TurnNav.CallSign = pWPT.Name;
		//        TurnnNav.MagVar = pWPT.MagVar;
		//    }
		//    TurnWPTToTurnNav = TurnNav;
		//}

		//Public Function NavaidTypeToWPT_FIXType(ByRef Val_Renamed As NavaidType) As WPT_FIXType
		//	Dim Result As WPT_FIXType
		//	Result.pPtGeo = Val_Renamed.pPtGeo
		//	Result.pPtPrj = Val_Renamed.pPtPrj

		//	Result.Name = Val_Renamed.Name
		//	Result.ID = Val_Renamed.ID

		//	Result.MagVar = Val_Renamed.MagVar

		//	Result.TypeName_Renamed = Val_Renamed.TypeName_Renamed
		//	Result.TypeCode = Val_Renamed.TypeCode

		//	Result.GuidanceNav = Val_Renamed
		//	Result.TypeOfGuidNav = Val_Renamed.TypeCode
		//	Result.IntersectNav = Val_Renamed.CallSign
		//	Result.TypeOfIntersectNav = Val_Renamed.TypeCode

		//	'    Val.CallSign
		//	NavaidTypeToWPT_FIXType = Result
		//End Function


		//Public Function GetNavData(ByVal NavCallSign As String, ByVal NavType As String, ByRef Navaid As NavaidType) As Integer
		//	Dim I As Integer
		//	Dim N As Integer

		//	Navaid.TypeCode = -1
		//	Navaid.CallSign = ""

		//	If NavType = "LLZ" Then
		//		N = UBound(NavaidList)
		//		For I = 0 To N
		//			If (NavaidList(I).TypeName_Renamed = NavType) And (NavaidList(I).CallSign = NavCallSign) Then
		//				Navaid = NavaidList(I)
		//				Return 0
		//			End If
		//		Next I

		//		''        FillILSList I2L
		//		'        RWYName = Mid(NavName, 4)
		//		'
		//		'        N = -1 'UBound(I2L)
		//		'        For I = 0 To N
		//		''            CallSign = I2L(I).CallSign
		//		'            CurrRWYName = I2L(I).RWY_ID
		//		'            If CurrRWYName = RWYName Then
		//		'                Set Navaid.pPtGeo = I2L(I).pPtGeo
		//		'                Set Navaid.pPtPrj = I2L(I).pPtPrj
		//		'
		//		'                Navaid.CallSign = I2L(I).CallSign
		//		'                Navaid.Name = I2L(I).Name
		//		'                Navaid.ID = I2L(I).ID
		//		'                Navaid.Course = I2L(I).Course
		//		'                Navaid.GP_RDH = I2L(I).GP_RDH
		//		'                Navaid.LLZ_THR = I2L(I).LLZ_THR
		//		'                Navaid.Sec_Width = I2L(I).Sec_Width
		//		'                Navaid.Range = LLZ.Range
		//		'
		//		''                Navaid.MagVar = CurADHP.MagVar
		//		'                Navaid.TypeName = "LLZ"
		//		'                Navaid.TypeCode = 3
		//		'                Navaid.Index = I
		//		'
		//		'                GetNavData = 0
		//		'                Exit For
		//		'            End If
		//		'        Next I
		//	ElseIf NavType = "DME" Then 
		//		N = UBound(DMEList)
		//		For I = 0 To N
		//			If DMEList(I).CallSign = NavCallSign Then
		//				Navaid = DMEList(I)
		//				Return 0
		//			End If
		//		Next I
		//	Else
		//		N = UBound(NavaidList)
		//		For I = 0 To N
		//			If (NavaidList(I).TypeName_Renamed = NavType) And (NavaidList(I).CallSign = NavCallSign) Then
		//				Navaid = NavaidList(I)
		//				Return 0
		//			End If
		//		Next I
		//	End If
		//	Return -1
		//End Function

		//	Public Function GetFIXableNavData(ByRef NavCallSign As String, ByRef NavType As String, ByRef Navaid As FIXableNavaidType) As Integer
		//		Dim I As Integer
		//		Dim N As Integer

		//'		Dim RWYName As String
		//'		Dim CurrRWYName As String


		//		'    If NavType = "LLZ" Then
		//		'        FillILSList I2L
		//		'        RWYName = Mid(NavName, 4)
		//		'
		//		'        N = UBound(I2L)
		//		'        For I = 0 To N
		//		''            CallSign = I2L(I).CallSign
		//		'            CurrRWYName = I2L(I).RWY_ID
		//		'            If CurrRWYName = RWYName Then
		//		'                Set Navaid.pPtGeo = I2L(I).pPtGeo
		//		'                Set Navaid.pPtPrj = I2L(I).pPtPrj
		//		'
		//		'                Navaid.CallSign = I2L(I).CallSign
		//		'                Navaid.Name = I2L(I).CallSign
		//		'
		//		'                Navaid.Course = I2L(I).Course
		//		'                Navaid.GP_RDH = I2L(I).GP_RDH
		//		'                Navaid.LLZ_THR = I2L(I).LLZ_THR
		//		'                Navaid.Sec_Width = I2L(I).Sec_Width
		//		'                Navaid.Range = LLZ.Range   '46000# '1000# *
		//		'
		//		'                Navaid.TypeName = NavType
		//		'                Navaid.TypeCode = 3
		//		'                Navaid.Index = -1
		//		'                GetFIXableNavData = 0
		//		'                Exit For
		//		'            End If
		//		'        Next I
		//		'    Else

		//		If NavType = "DME" Then
		//			N = UBound(DMEList)
		//			For I = 0 To N
		//				If DMEList(I).CallSign = NavCallSign Then
		//					Navaid.pPtGeo = DMEList(I).pPtGeo
		//					Navaid.pPtPrj = DMEList(I).pPtPrj

		//					Navaid.Name = DMEList(I).Name
		//					Navaid.ID = DMEList(I).ID
		//					Navaid.CallSign = NavCallSign

		//					Navaid.MagVar = 0
		//					Navaid.TypeName_Renamed = NavType
		//					Navaid.TypeCode = DMEList(I).TypeCode

		//					Navaid.Range = DMEList(I).Range
		//					Navaid.Index = DMEList(I).Index
		//					Navaid.PairNavaidIndex = DMEList(I).PairNavaidIndex
		//					Navaid.pSignificantPoint = DMEList(I).pSignificantPoint
		//					Return 0
		//				End If
		//			Next I
		//		Else
		//			N = UBound(NavaidList)
		//			For I = 0 To N
		//				If (NavaidList(I).TypeName_Renamed = NavType) And (NavaidList(I).CallSign = NavCallSign) Then
		//					Navaid.pPtGeo = NavaidList(I).pPtGeo
		//					Navaid.pPtPrj = NavaidList(I).pPtPrj

		//					Navaid.Name = NavaidList(I).Name
		//					Navaid.ID = NavaidList(I).ID
		//					Navaid.CallSign = NavCallSign

		//					Navaid.MagVar = NavaidList(I).MagVar
		//					Navaid.TypeName_Renamed = NavType
		//					Navaid.TypeCode = NavaidList(I).TypeCode

		//					Navaid.Range = NavaidList(I).Range
		//					Navaid.Index = NavaidList(I).Index
		//					Navaid.PairNavaidIndex = NavaidList(I).PairNavaidIndex
		//					Navaid.pSignificantPoint = NavaidList(I).pSignificantPoint
		//					Return 0
		//				End If
		//			Next I
		//		End If
		//		Return -1
		//	End Function
	}

	public class NameAndIndex
	{
		public string TypeNameRenamed{get;set;}
		public int TypeIndex{get;set;}
	}

	public enum ENavaidTupes
	{
		CodeNone = -1,
		CodeVor = 0,
		CodeDme = 1,
		CodeNdb = 2,
		CodeLlz = 3,
		CodeTacan = 4
	}
}
