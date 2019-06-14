using System;
using System.IO;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct PansOpsData
	{
		public string Name;
		public double Value;
		public double Multiplier;
		public string Unit;
		public string Comment;
		public string DefinedIn;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class PANS_OPS_DataBase
	{
		public static PansOpsData dpOIS;
		public static PansOpsData dpMOC;
		public static PansOpsData dpNGui_Ar1;
		public static PansOpsData dpNGui_Ar1_Wd;
		public static PansOpsData dpGui_Ar1;
		public static PansOpsData dpGui_Ar1_Wd;

		public static PansOpsData dpTr_AdjAngle;
		public static PansOpsData dpAr1_OB_TrAdj;
		public static PansOpsData dpAr1_IB_TrAdj;
		public static PansOpsData dpAr2_Bnd_TrAdj;
		public static PansOpsData dpT_Bank;
		public static PansOpsData dpWind_Speed;
		public static PansOpsData dpT_TechToleranc;
		public static PansOpsData dpT_Init;
		public static PansOpsData dpT_Init_Wd;
		public static PansOpsData dpObsClr;
		public static PansOpsData dpT_Gui_dist;
		public static PansOpsData dpH_abv_DER;
		public static PansOpsData dpOIS_abv_DER;
		public static PansOpsData dpPDG_60;
		public static PansOpsData dpPDG_Nom;
		public static PansOpsData dpStr_Gui_dist;
		public static PansOpsData dpafTrn_OSplay;
		public static PansOpsData dpafTrn_ISplay;
		public static PansOpsData dpOv_Nav_PDG;
		public static PansOpsData dpTP_by_DME_div;
		//public static PansOpsData dpNGui_Ar1_Dist;
		public static PansOpsData dpOD1_ZoneAdjA;
		public static PansOpsData dpOD2_ZoneAdjA;
		public static PansOpsData ISA;
		public static PansOpsData dpSecAreaCutAngl;
		public static PansOpsData dpMaxPosPDG;
		public static PansOpsData dpInterMinAngle;
		public static PansOpsData dpInterMaxAngle;
		public static PansOpsData dpFlightTechTol;
		public static PansOpsData dpTermMinBuffer;

		public static void InitModule()
		{
			int i, j, DataSize, Fields, Records;
			string FileName, FileSign, FieldName;

			short FieldType;
			bool BoolField;
			short SmallIntField;
			int intField;
			string stringField;
			double floatField;
			byte[] ByteArrayField;

			byte[] Data;
			uint Index;

			PansOpsData TmpData = new PansOpsData();

			FileName = GlobalVars.ConstDir + "\\" + GlobalVars.PANS_OPSdb + ".dat";

			byte[] bFileSign = new byte[20];

			FileStream fs = File.OpenRead(FileName);
			fs.Read(bFileSign, 0, 20);

			FileSign = System.Text.Encoding.Default.GetString(bFileSign);
			if (FileSign != "Anplan DATABASE file")
				throw new Exception("Invalid PANSOPS constants DATABASE file");

			DataSize = (int)fs.Length - 20;
			Data = new byte[DataSize];
			Index = 0;
			fs.Read(Data, 0, DataSize);

			Data[DataSize - 1] = (byte)(Data[DataSize - 1] ^ (int)'R');

			for (i = DataSize - 2; i >= 0; i += -1)
				Data[i] = (byte)(Data[i] ^ Data[i + 1]);

			FileRoutines.GetShortData(Data, ref Index, out SmallIntField);
			Fields = SmallIntField;
			FileRoutines.GetShortData(Data, ref Index, out SmallIntField);
			Records = SmallIntField;

			for (i = 0; i < Fields; i++)
			{
				FileRoutines.GetShortData(Data, ref Index, out SmallIntField);
				FileRoutines.GetStrData(Data, ref Index, out stringField, SmallIntField);
			}

			for (i = 0; i < Records; i++)
			{
				for (j = 0; j < Fields; j++)
				{
					FileRoutines.GetPStrData(Data, ref Index, out FieldName);
					FileRoutines.GetShortData(Data, ref Index, out FieldType);

					switch (FieldType)
					{
						case 1:
						case 16:
						case 18:
						case 23:
						case 24:
							FileRoutines.GetPStrData(Data, ref Index, out stringField);
							switch (FieldName)
							{
								case "COMMENT":
									TmpData.Comment = stringField;
									break;
								case "DEFINED_IN":
									TmpData.DefinedIn = stringField;
									break;
								case "NAME":
									TmpData.Name = stringField;
									break;
								case "UNIT":
									TmpData.Unit = stringField;
									break;
							}
							break;

						case 2:     // 16-bit integer field
						case 4:     // 16-bit unsigned integer field
							FileRoutines.GetShortData(Data, ref Index, out SmallIntField);
							break;

						case 3:     // Large integer field
						case 25:    // 32-bit integer field
							FileRoutines.GetIntData(Data, ref Index, out intField);
							break;

						case 5:     //BoolField
							FileRoutines.GetShortData(Data, ref Index, out SmallIntField);
							BoolField = SmallIntField != 0;
							break;

						case 6:
						case 8:
							FileRoutines.GetDoubleData(Data, ref Index, out floatField);
							if (FieldName == "VALUE")
								TmpData.Value = floatField;
							else if (FieldName == "MULTIPLIER")
								TmpData.Multiplier = floatField;
							break;

						case 7:         //CurrencyField
							FileRoutines.GetData(Data, ref Index, out ByteArrayField, 8);
							break;

						case 9:         // Date field
						case 10:        // Time field
						case 11:        // Date and time field
							FileRoutines.GetData(Data, ref Index, out ByteArrayField, 8);
							break;
					}
				}

				TmpData.Value = TmpData.Value * TmpData.Multiplier;
				if (TmpData.Unit == "rad" || TmpData.Unit == "°")
					TmpData.Value = Math.Round(GlobalVars.RadToDegValue * TmpData.Value, 2);

				switch (TmpData.Name)
				{
					case "dpOIS":
						dpOIS = TmpData;
						break;
					case "dpMOC":
						dpMOC = TmpData;
						break;
					case "dpNGui_Ar1":
						dpNGui_Ar1 = TmpData;
						break;
					case "dpNGui_Ar1_Wd":
						dpNGui_Ar1_Wd = TmpData;
						break;
					case "dpTr_AdjAngle":
						dpTr_AdjAngle = TmpData;
						break;
					case "dpAr1_OB_TrAdj":
						dpAr1_OB_TrAdj = TmpData;
						break;
					case "dpAr1_IB_TrAdj":
						dpAr1_IB_TrAdj = TmpData;
						break;
					case "dpAr2_Bnd_TrAdj":
						dpAr2_Bnd_TrAdj = TmpData;
						break;
					case "dpT_Bank":
						dpT_Bank = TmpData;
						break;
					case "dpWind_Speed":
						dpWind_Speed = TmpData;
						break;
					case "dpT_TechToleranc":
						dpT_TechToleranc = TmpData;
						break;
					case "dpT_Init":
						dpT_Init = TmpData;
						break;
					case "dpT_Init_Wd":
						dpT_Init_Wd = TmpData;
						break;
					case "dpObsClr":
						dpObsClr = TmpData;
						break;
					case "dpT_Gui_dist":
						dpT_Gui_dist = TmpData;
						break;
					case "dpH_abv_DER":
						dpH_abv_DER = TmpData;
						break;
					case "dpOIS_abv_DER":
						dpOIS_abv_DER = TmpData;
						break;
					case "dpPDG_60":
						dpPDG_60 = TmpData;
						break;
					case "dpPDG_Nom":
						dpPDG_Nom = TmpData;
						break;
					case "dpStr_Gui_dist":
						dpStr_Gui_dist = TmpData;
						break;
					case "dpafTrn_OSplay":
						dpafTrn_OSplay = TmpData;
						break;
					case "dpafTrn_ISplay":
						dpafTrn_ISplay = TmpData;
						break;
					case "dpOv_Nav_PDG":
						dpOv_Nav_PDG = TmpData;
						break;
					case "dpTP_by_DME_div":
						dpTP_by_DME_div = TmpData;
						break;
					case "dpOD1_ZoneAdjA":
						dpOD1_ZoneAdjA = TmpData;
						break;
					case "dpOD2_ZoneAdjA":
						dpOD2_ZoneAdjA = TmpData;
						break;
					case "dpGui_Ar1_Wd":
						dpGui_Ar1_Wd = TmpData;
						break;
					case "dpGui_Ar1":
						dpGui_Ar1 = TmpData;
						break;
					case "ISA":
						ISA = TmpData;
						break;
					case "dpSecAreaCutAngl":
						dpSecAreaCutAngl = TmpData;
						break;
					case "dpMaxPosPDG":
						dpMaxPosPDG = TmpData;
						break;
					case "dpInterMinAngle":
						dpInterMinAngle = TmpData;
						break;
					case "dpInterMaxAngle":
						dpInterMaxAngle = TmpData;
						break;
					case "dpFlightTechTol":
						dpFlightTechTol = TmpData;
						break;
					case "dpTermMinBuffer":
						dpTermMinBuffer = TmpData;
						break;
				}
			}
		}
	}
}

