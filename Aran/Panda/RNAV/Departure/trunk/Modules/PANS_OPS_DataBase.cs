using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace Aran.PANDA.RNAV.Departure
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
		// Public dpNGui_Ar1_Dist As PansOpsData
		public static PansOpsData dpOD1_ZoneAdjA;
		public static PansOpsData dpOD2_ZoneAdjA;
		public static PansOpsData ISA;
		public static PansOpsData dpSecAreaCutAngl;
		public static PansOpsData dpMaxPosPDG;
		public static PansOpsData dpInterMinAngle;
		public static PansOpsData dpInterMaxAngle;
		public static PansOpsData dpFlightTechTol;

		private static byte[] Data;
		private static uint Index;

		public static void InitModule()
		{
			int I, J, DataSize;
			string FileSign;
			string FileName;

			PansOpsData TmpData = new PansOpsData();

			int Fields;
			int Records;
			int FieldType;
			int intField;

			string FieldName;

			bool BoolField;

			short SmallIntField;
			short shortInt;

			string charField;
			double floatField;
			//DateTime DateTimeField;

			FileName = GlobalVars.InstallDir + @"\constants\pansops.dat";

			char[] cbuffer = new char[20];
			StreamReader sr = System.IO.File.OpenText(FileName);
			sr.Read(cbuffer, 0, 20);
			sr.Close();
			FileSign = new string(cbuffer);

			//FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			//byte[] buffer = new byte[20];
			//fileStream.Read(buffer, 0, 20);
			//fileStream.Close();
			//FileSign = new string(buffer.Select(b => (char)b).ToArray ());

			if (FileSign != "Anplan DATABASE file")
				throw new Exception("Invalid PANSOPS constants DATABASE file");

			Data = File.ReadAllBytes(FileName);
			DataSize = Data.Length;
			Index = 20;

			Data[DataSize - 1] = (byte)(Data[DataSize - 1] ^ (int)'R');

			for (I = DataSize - 2; I >= 0; I--)
				Data[I] = (byte)(Data[I] ^ Data[I + 1]);

			FileRoutines.GetShortData(Data, ref Index, out shortInt, 2);
			Fields = shortInt;
			FileRoutines.GetShortData(Data, ref Index, out shortInt, 2);
			Records = shortInt;

			for (I = 0; I < Fields; I++)
			{
				FileRoutines.GetShortData(Data, ref Index, out shortInt, 2);
				FileRoutines.GetStrData(Data, ref Index, out charField, shortInt);
			}

			for (I = 0; I < Records ; I++)
			{
				for (J = 0; J < Fields ; J++)
				{
					FileRoutines.GetShortData(Data, ref Index, out shortInt, 2);
					FileRoutines.GetStrData(Data, ref Index, out FieldName, shortInt);
					FileRoutines.GetShortData(Data, ref Index, out shortInt, 2);
					FieldType = shortInt;

					switch (FieldType)
					{
						case 1:
						case 16:
						case 18:
						case 23:
						case 24:
							FileRoutines.GetShortData(Data, ref Index, out shortInt, 2);
							FileRoutines.GetStrData(Data, ref Index, out charField, shortInt);
							switch (FieldName)
							{
								case "COMMENT":
									TmpData.Comment = charField;
									break;
								case "DEFINED_IN":
									TmpData.DefinedIn = charField;
									break;
								case "NAME":
									TmpData.Name = charField;
									break;
								case "UNIT":
									TmpData.Unit = charField;
									break;
							}

							// 16-bit integer field
							// 16-bit unsigned integer field
							break;
						case 2:
						case 4:
							FileRoutines.GetShortData(Data, ref Index, out SmallIntField, 2);
							// Large integer field
							// 32-bit integer field
							break;
						case 3:
						case 25:
							FileRoutines.GetShortData(Data, ref Index, out shortInt, 4);
							intField = shortInt;
							break;
						case 5:
							// GetData(data, index, BoolField, 2)
							FileRoutines.GetShortData(Data, ref Index, out shortInt, 4);
							BoolField = shortInt != 0;
							break;
						case 6:
						case 8:
							FileRoutines.GetDoubleData(Data, ref Index, out floatField, 8);
							if (FieldName == "VALUE")
								TmpData.Value = floatField;
							else if (FieldName == "MULTIPLIER")
								TmpData.Multiplier = floatField;
							break;
						case 7:
							byte[] transTemp1;			//CurrencyField
							FileRoutines.GetData(Data, ref Index, out transTemp1, 8);
							// Date field
							// Time field
							// Date and time field
							break;
						case 9:
						case 10:
						case 11:
							byte[] transTemp0;			//DateTimeField;
							FileRoutines.GetData(Data, ref Index, out transTemp0, 8);
							break;
					}
				}

				TmpData.Value = TmpData.Value * TmpData.Multiplier;
				if (TmpData.Unit == "rad" || TmpData.Unit == "°")
					TmpData.Value = Math.Round(GlobalVars.RadToDegValue * TmpData.Value, 1);

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
				}
			}
		}
	}
}
