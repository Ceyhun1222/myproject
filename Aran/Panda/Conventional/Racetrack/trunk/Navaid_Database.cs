using System;
using System.Data.OleDb;
using Aran.Panda.Conventional.Common;


namespace Aran.Panda.Conventional.Racetrack
{
	public enum eNavaidTupes
	{
		CodeNONE = -1,
		CodeVOR = 0,
		CodeDME = 1,
		CodeNDB = 2,
		CodeLLZ = 3,
		CodeTACAN = 4
	}
		
	public class Navaids_DataBase
	{	
		
		public Navaids_DataBase()
		{
			double Value;
			double Multiplier;
			string ParName;
			OleDbDataReader navaidTypesReader;

			VOR = new VORData();
			DME = new DMEData();
			NDB = new NDBData();
			LLZ = new LLZData();

			if (!OpenDbfConnection(InitHolding.PandaInstallDir+"\\Navaids\\"))
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
						double tmp = ( double ) navaidTypesReader [ "VALUE" ];
						Value = (double)navaidTypesReader["VALUE"] * Multiplier;
						if (navaidTypesReader["UNIT"].ToString() == "rad")
							Value = ARANFunctions.RadToDeg(Value);

						switch (navaidReader["name"].ToString())
						{
							case "VOR":
								if (ParName == "Range")
									VOR.Range = Value;
								else if (ParName == "FA Range")
									VOR.FA_Range = Value;
								else if (ParName == "Initial width")
									VOR.InitWidth = Value;
								else if (ParName == "Splay angle")
									VOR.SplayAngle = Value;
								else if (ParName == "Tracking tolerance")
									VOR.TrackingTolerance = Value;
								else if (ParName == "Intersecting tolerance")
									VOR.IntersectingTolerance = Value;
								else if (ParName == "Cone angle")
									VOR.ConeAngle = Value;
								else if (ParName == "Track accuracy")
									VOR.TrackAccuracy = Value;
								else if (ParName == "Lateral deviation coef.")
									VOR.LateralDeviationCoef = Value;
								else if (ParName == "EnRoute Tracking toler")
									VOR.EnRouteTrackingToler = Value;
								else if (ParName == "EnRoute Tracking2 toler")
									VOR.EnRouteTracking2Toler = Value;
								else if (ParName == "EnRoute Inter toler")
									VOR.EnRouteInterToler = Value;
								else if (ParName == "EnRoute PrimArea With")
									VOR.EnRoutePrimAreaWith = Value;
								else if (ParName == "EnRoute SecArea With")
									VOR.EnRouteSecAreaWith = Value;
								break;

							case "NDB":

								if (ParName == "Range")
									NDB.Range = Value;
								else if (ParName == "FA Range")
									NDB.FA_Range = Value;
								else if (ParName == "Initial width")
									NDB.InitWidth = Value;
								else if (ParName == "Splay angle")
									NDB.SplayAngle = Value;
								else if (ParName == "Tracking tolerance")
									NDB.TrackingTolerance = Value;
								else if (ParName == "Intersecting tolerance")
									NDB.IntersectingTolerance = Value;
								else if (ParName == "Cone angle")
									NDB.ConeAngle = Value;
								else if (ParName == "Track accuracy")
									NDB.TrackAccuracy = Value;
								else if (ParName == "Entry into the cone accuracy")
									NDB.Entry2ConeAccuracy = Value;
								else if (ParName == "Lateral deviation coef.")
									NDB.LateralDeviationCoef = Value;
								else if (ParName == "EnRoute Tracking toler")
									NDB.EnRouteTrackingToler = Value;
								else if (ParName == "EnRoute Tracking2 toler")
									NDB.EnRouteTracking2Toler = Value;
								else if (ParName == "EnRoute Inter toler")
									NDB.EnRouteInterToler = Value;
								else if (ParName == "EnRoute PrimArea With")
									NDB.EnRoutePrimAreaWith = Value;
								else if (ParName == "EnRoute SecArea With")
									NDB.EnRouteSecAreaWith = Value;
								break;

							case "DME":
								if (ParName == "Range")
									DME.Range = Value;
								else if (ParName == "Minimal Error")
									DME.MinimalError = Value;
								else if (ParName == "Error Scaling Up")
									DME.ErrorScalingUp = Value;
								else if (ParName == "Slant Angle")
									DME.SlantAngle = Value;
								else if (ParName == "TP_div")
									DME.TP_div = Value;
								break;

							case "LLZ":

								if (ParName == "Range")
									LLZ.Range = Value;
								else if (ParName == "Tracking tolerance")
									LLZ.TrackingTolerance = Value;
								else if (ParName == "Intersecting tolerance")
									LLZ.IntersectingTolerance = Value;
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
		public VORData VOR { get; private set; }
		public NDBData NDB { get; private set; }
		public DMEData DME { get; private set; }
		public LLZData LLZ { get; private set; }
							   
		private OleDbConnection _conn;
		 
#region
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

#endregion
	public class NDBData
	{

		public double Range { get; set; }
		public double FA_Range { get; set; }
		public double InitWidth { get; set; }
		public double SplayAngle { get; set; }
		public double TrackingTolerance { get; set; }
		public double IntersectingTolerance { get; set; }
		public double ConeAngle { get; set; }
		public double TrackAccuracy { get; set; }
		public double Entry2ConeAccuracy { get; set; }
		public double LateralDeviationCoef { get; set; }
		public double EnRouteTrackingToler { get; set; }
		public double EnRouteTracking2Toler { get; set; }
		public double EnRouteInterToler { get; set; }
		public double EnRoutePrimAreaWith { get; set; }
		public double EnRouteSecAreaWith { get; set; }
		public double OnNAVRadius { get; set; }

	}

	public class VORData
	{
		public double Range { get; set; }
		public double FA_Range { get; set; }
		public double InitWidth { get; set; }
		public double SplayAngle { get; set; }
		public double TrackingTolerance { get; set; }
		public double IntersectingTolerance { get; set; }
		public double ConeAngle { get; set; }
		public double TrackAccuracy { get; set; }
		public double LateralDeviationCoef { get; set; }
		public double EnRouteTrackingToler { get; set; }
		public double EnRouteTracking2Toler { get; set; }
		public double EnRouteInterToler { get; set; }
		public double EnRoutePrimAreaWith { get; set; }
		public double EnRouteSecAreaWith { get; set; }
		public double OnNAVRadius { get; set; }
		
	}

	public class DMEData
	{
		public double Range { get; set; }
		public double MinimalError { get; set; }
		public double ErrorScalingUp { get; set; }
		public double SlantAngle { get; set; }
		public double TP_div { get; set; }
		
	}

	public class LLZData
	{
		public double Range { get; set; }
		public double TrackingTolerance { get; set; }
		public double IntersectingTolerance { get; set; }
	}

	public class NameAndIndex
	{
		public string TypeName_Renamed{get;set;}
		public int TypeIndex{get;set;}
	}

}
