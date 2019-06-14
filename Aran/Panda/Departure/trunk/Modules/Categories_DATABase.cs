using System;
using System.IO;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct CategoriesData
	{
		public string Name;
		public double[] Values;
		public double A;
		public double B;
		public double C;
		public double D;
		//public double DL;
		public double E;
		public double H;
		public double Multiplier;
		public string Unit;
		public string Comment;
		public string DefinedIn;

		public void Initialize()
		{
			Values = new double[6];
		}
	}

	public static class Categories_DATABase
	{
		public static CategoriesData cVatMin;
		public static CategoriesData cVatMax;
		public static CategoriesData cViafMin;
		public static CategoriesData cViafMax;
		public static CategoriesData cViafStar;
		public static CategoriesData cVfafMin;
		public static CategoriesData cVfafMax;
		public static CategoriesData cVva;
		public static CategoriesData cVmaInter;
		public static CategoriesData cVmaFaf;
		public static CategoriesData carStraightSegmen;
		public static CategoriesData arObsClearance;
		public static CategoriesData arMinOCH;
		public static CategoriesData arMinVisibility;
		public static CategoriesData arFAMinOCH15;
		public static CategoriesData arFAMinOCH30;
		public static CategoriesData arMaxInterAngle;
		public static CategoriesData arT45_180;
		public static CategoriesData arMaxOutBoundDes;
		public static CategoriesData arMaxInBoundDesc;
		public static CategoriesData arMinHV_FAS;
		public static CategoriesData arMaxHV_FAS;
		public static CategoriesData arFADescent_Max;
		public static CategoriesData arImHorSegLen;
		public static CategoriesData arMinISlen00_15p;
		public static CategoriesData arMinISlen16_30p;
		public static CategoriesData arMinISlen31_60p;
		public static CategoriesData arMinISlen61_90p;
		public static CategoriesData arSemiSpan;
		public static CategoriesData arVerticalSize;

		public static void InitModule()
		{
			int i, j, DataSize;
			int Records, Fields;

			string FileSign;
			string FileName;

			short FieldType;
			bool BoolField;
			short SmallIntField;
			int intField;
			string stringField;
			double floatField;
			byte[] ByteArrayField;

			string FieldName;

			byte[] Data;
			uint Index;


		CategoriesData TmpData = new CategoriesData();
			TmpData.Initialize();

			FileName = GlobalVars.ConstDir + "\\categories.dat";

			byte[] bFileSign = new byte[20];

			FileStream fs = File.OpenRead(FileName);
			fs.Read(bFileSign, 0, 20);

			FileSign = System.Text.Encoding.Default.GetString(bFileSign);
			if (FileSign != "Anplan DATABASE file")
				throw new Exception("Invalid categories constants DATABASE file");

			DataSize = (int)fs.Length - 20;
			Data = new byte[DataSize];
			Index = 0;
			fs.Read(Data, 0, DataSize);

			Data[DataSize - 1] = (byte)(Data[DataSize - 1] ^ (int)'R');

			for (i = DataSize - 2; i >= 0; i += -1)
				Data[i] = (byte)(Data[i] ^ Data[i + 1]);

			//	On Error GoTo ErrorHandler

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
						//Fixed character field
						//Wide string field
						//Text memo field
						//Formatted text memo field
						//Character or string field
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

						//	====

						case 6:     //Floating-point numeric field
						case 8:
							FileRoutines.GetDoubleData(Data, ref Index, out floatField);
							switch (FieldName)
							{
								case "A":
									TmpData.A = floatField;
									break;
								case "B":
									TmpData.B = floatField;
									break;
								case "C":
									TmpData.C = floatField;
									break;
								case "D":
									TmpData.D = floatField;
									break;
								//case "DL":
								//	TmpData.DL = floatField;
								//	break;
								case "E":
									TmpData.E = floatField;
									break;
								case "H":
									TmpData.H = floatField;
									break;
								case "MULTIPLIER":
									TmpData.Multiplier = floatField;
									break;
							}
							break;

						case 7:         //CurrencyField //Money field
							FileRoutines.GetData(Data, ref Index, out ByteArrayField, 8);
							break;

						case 9:         // Date field
						case 10:        // Time field
						case 11:        // Date and time field
							FileRoutines.GetData(Data, ref Index, out ByteArrayField, 8);
							break;
					}

				}   //next j

				if (TmpData.Unit == "knot" || TmpData.Unit == "km/h")
					TmpData.Multiplier *= 3.6;

				if (TmpData.Unit == "rad" || TmpData.Unit == "°")
					TmpData.Multiplier *= GlobalVars.RadToDegValue;

				TmpData.A = TmpData.A * TmpData.Multiplier;
				TmpData.B = TmpData.B * TmpData.Multiplier;
				TmpData.C = TmpData.C * TmpData.Multiplier;
				TmpData.D = TmpData.D * TmpData.Multiplier;
				//TmpData.DL = TmpData.DL * TmpData.Multiplier;
				TmpData.E = TmpData.E * TmpData.Multiplier;
				TmpData.H = TmpData.H * TmpData.Multiplier;


				//if (TmpData.Unit == "rad" || TmpData.Unit == "°")
				//{
				//	TmpData.A = Math.Round(GlobalVars.RadToDegValue * TmpData.A, 2);
				//	TmpData.B = Math.Round(GlobalVars.RadToDegValue * TmpData.B, 2);
				//	TmpData.C = Math.Round(GlobalVars.RadToDegValue * TmpData.C, 2);
				//	TmpData.D = Math.Round(GlobalVars.RadToDegValue * TmpData.D, 2);
				//	//TmpData.DL = Math.Round(GlobalVars.RadToDegValue * TmpData.DL, 2);
				//	TmpData.E = Math.Round(GlobalVars.RadToDegValue * TmpData.E, 2);
				//	TmpData.H = Math.Round(GlobalVars.RadToDegValue * TmpData.H, 2);
				//}


				TmpData.Values[0] = TmpData.A;
				TmpData.Values[1] = TmpData.B;
				TmpData.Values[2] = TmpData.C;
				TmpData.Values[3] = TmpData.D;
				//TmpData.Values[4] = TmpData.DL;
				TmpData.Values[4] = TmpData.E;
				TmpData.Values[5] = TmpData.H;

				switch (TmpData.Name)
				{
					case "VatMin":
						cVatMin = TmpData;
						cVatMin.Initialize();
						System.Array.Copy(TmpData.Values, cVatMin.Values, 6); break;

					case "VatMax":
						cVatMax = TmpData;
						cVatMax.Initialize();
						System.Array.Copy(TmpData.Values, cVatMax.Values, 6); break;

					case "ViafMin":
						cViafMin = TmpData;
						cViafMin.Initialize();
						System.Array.Copy(TmpData.Values, cViafMin.Values, 6); break;

					case "ViafMax":
						cViafMax = TmpData;
						cViafMax.Initialize();
						System.Array.Copy(TmpData.Values, cViafMax.Values, 6); break;

					case "Viaf*":
						cViafStar = TmpData;
						cViafStar.Initialize();
						System.Array.Copy(TmpData.Values, cViafStar.Values, 6); break;

					case "VfafMin":
						cVfafMin = TmpData;
						cVfafMin.Initialize();
						System.Array.Copy(TmpData.Values, cVfafMin.Values, 6); break;

					case "VfafMax":
						cVfafMax = TmpData;
						cVfafMax.Initialize();
						System.Array.Copy(TmpData.Values, cVfafMax.Values, 6); break;

					case "Vva":
						cVva = TmpData;
						cVva.Initialize();
						System.Array.Copy(TmpData.Values, cVva.Values, 6); break;

					case "VmaInter":
						cVmaInter = TmpData;
						cVmaInter.Initialize();
						System.Array.Copy(TmpData.Values, cVmaInter.Values, 6); break;

					case "VmaFaf":
						cVmaFaf = TmpData;
						cVmaFaf.Initialize();
						System.Array.Copy(TmpData.Values, cVmaFaf.Values, 6); break;

					case "arStraightSegmen":
						carStraightSegmen = TmpData;
						carStraightSegmen.Initialize();
						System.Array.Copy(TmpData.Values, carStraightSegmen.Values, 6); break;

					case "arObsClearance":
						arObsClearance = TmpData;
						arObsClearance.Initialize();
						System.Array.Copy(TmpData.Values, arObsClearance.Values, 6); break;

					case "arMinOCH":
						arMinOCH = TmpData;
						arMinOCH.Initialize();
						System.Array.Copy(TmpData.Values, arMinOCH.Values, 6); break;

					case "arMinVisibility":
						arMinVisibility = TmpData;
						arMinVisibility.Initialize();
						System.Array.Copy(TmpData.Values, arMinVisibility.Values, 6); break;

					case "arFAMinOCH 15°":
						arFAMinOCH15 = TmpData;
						arFAMinOCH15.Initialize();
						System.Array.Copy(TmpData.Values, arFAMinOCH15.Values, 6); break;

					case "arFAMinOCH 30°":
						arFAMinOCH30 = TmpData;
						arFAMinOCH30.Initialize();
						System.Array.Copy(TmpData.Values, arFAMinOCH30.Values, 6); break;

					case "arMaxInterAngle":
						arMaxInterAngle = TmpData;
						arMaxInterAngle.Initialize();
						System.Array.Copy(TmpData.Values, arMaxInterAngle.Values, 6); break;

					case "arT45-180":
						arT45_180 = TmpData;
						arT45_180.Initialize();
						System.Array.Copy(TmpData.Values, arT45_180.Values, 6); break;

					case "arMaxOutBoundDes":
						arMaxOutBoundDes = TmpData;
						arMaxOutBoundDes.Initialize();
						System.Array.Copy(TmpData.Values, arMaxOutBoundDes.Values, 6); break;

					case "arMaxInBoundDesc":
						arMaxInBoundDesc = TmpData;
						arMaxInBoundDesc.Initialize();
						System.Array.Copy(TmpData.Values, arMaxInBoundDesc.Values, 6); break;

					case "arMinHV_FAS":
						arMinHV_FAS = TmpData;
						arMinHV_FAS.Initialize();
						System.Array.Copy(TmpData.Values, arMinHV_FAS.Values, 6); break;

					case "arMaxHV_FAS":
						arMaxHV_FAS = TmpData;
						arMaxHV_FAS.Initialize();
						System.Array.Copy(TmpData.Values, arMaxHV_FAS.Values, 6); break;

					case "arFADescent_Max":
						arFADescent_Max = TmpData;
						arFADescent_Max.Initialize();
						System.Array.Copy(TmpData.Values, arFADescent_Max.Values, 6); break;

					case "arImHorSegLen":
						arImHorSegLen = TmpData;
						arImHorSegLen.Initialize();
						System.Array.Copy(TmpData.Values, arImHorSegLen.Values, 6); break;

					case "arMinISlen00_15p":
						arMinISlen00_15p = TmpData;
						arMinISlen00_15p.Initialize();
						System.Array.Copy(TmpData.Values, arMinISlen00_15p.Values, 6); break;

					case "arMinISlen16_30p":
						arMinISlen16_30p = TmpData;
						arMinISlen16_30p.Initialize();
						System.Array.Copy(TmpData.Values, arMinISlen16_30p.Values, 6); break;

					case "arMinISlen31_60p":
						arMinISlen31_60p = TmpData;
						arMinISlen31_60p.Initialize();
						System.Array.Copy(TmpData.Values, arMinISlen31_60p.Values, 6); break;

					case "arMinISlen61_90p":
						arMinISlen61_90p = TmpData;
						arMinISlen61_90p.Initialize();
						System.Array.Copy(TmpData.Values, arMinISlen61_90p.Values, 6); break;

					case "arSemiSpan":
						arSemiSpan = TmpData;
						arSemiSpan.Initialize();
						System.Array.Copy(TmpData.Values, arSemiSpan.Values, 6); break;

					case "arVerticalSize":
						arVerticalSize = TmpData;
						arVerticalSize.Initialize();
						System.Array.Copy(TmpData.Values, arVerticalSize.Values, 6); break;
				}
			}   //Next I
		}   // end of function
	}
}
