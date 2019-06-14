using Microsoft.VisualBasic;
using System;
using Aran.PANDA.Common;
namespace Aran.PANDA.Constants
{
	public class AircraftCategoryList : ConstantList<AircraftCategoryConstant>
	{
		public readonly string[] AircraftCategoryDataNames = {
				"VatMin", "VatMax", "ViafMin", "ViafMax",
				"Viaf*", "VfafMin", "VfafMax", "Vva",
				"VmaInter", "VmaFaf", "arStraightSegmen", "arObsClearance",
				"arMinOCH", "arMinVisibility", "arFAMinOCH 15°", "arFAMinOCH 30°",
				"arMaxInterAngle", "arT45-180", "arMaxOutBoundDes", "arMaxOutInBdDesc",
				"arMinHV_FAS", "arMaxHV_FAS", "arFADescent_Max", "arImHorSegLen",
				"arMinISlen00_15p", "arMinISlen16_30p", "arMinISlen31_60p", "arMinISlen61_90p",
				"arSemiSpan", "arVerticalSize", "enIAS", 
                "hldIASUpTo4250MinNormalTerminal", "hldIASUpTo4250MaxNormalTerminal", 
                "hldIASUpTo6100MinNormalTerminal", "hldIASUpTo6100MaxNormalTerminal", 
                "hldIASUpTo10350MinNormalTerminal", "hldIASUpTo10350MaxNormalTerminal", 
                "hldIASUpTo4250MinTurbulenceTerminal", "hldIASUpTo4250MaxTurbulenceTerminal", 
                "hldIASMinInitialApproachTerminal", "hldIASMaxInitialApproachTerminal",
                "hldIASEnroute", "RacetrckIASMin", "RacetrckIASMax"
                //"hldIASupTo4250", 
                //"hldIASupTo6100","hldIASupTo10350", "hldIASover10350", "hldIASupTo4250Min",
                //"hldIASupTo6100Min","hldIASupTo10350Min", "hldIASupTo4250Max", "hldIASupTo6100Max", 
                //"hldIASupTo10350Max","hldIASupTo4250Turb", "hldIASupTo6100Turb","hldIASupTo10350Turb",
                //"RacetrckIASMin","RacetrckIASMax"
            };

		public AircraftCategoryList()
			: base(Enum.GetValues(typeof(aircraftCategoryData)).Length)
		{
			aircraftCategoryNames = new EnumArray<string, aircraftCategoryData>();
			int i = -1;
			foreach (aircraftCategoryData item in Enum.GetValues(typeof(aircraftCategoryData)))
			{
				i++;
				aircraftCategoryNames[item] = AircraftCategoryDataNames[i];
			}

		}

		public AircraftCategoryConstant this[aircraftCategoryData dataType]
		{
			get { return base.ConstantByIndex((int)dataType); }
		}

		public AircraftCategoryConstant this[string name]
		{
			get { return base.ConstantByName(name); }
		}

		private EnumArray<string, aircraftCategoryData> aircraftCategoryNames;
	}

	public class AircraftCategoryListLoader : AircraftCategoryList
	{
		public void LoadFromFile(string fileName)
		{
			int i;
			int FileNumber = FileSystem.FreeFile();
			FileSystem.FileOpen(FileNumber, fileName, OpenMode.Binary, OpenAccess.Read, (Microsoft.VisualBasic.OpenShare)(-1), -1);

			string FileSign = FileSystem.InputString(FileNumber, 20);
			FileSystem.FileClose(FileNumber);
			if (FileSign != "Anplan DATABASE file")
				throw new Exception("Invalid GNSS constants DATABASE file");

			byte[] Data = System.IO.File.ReadAllBytes(fileName);
			int DataSize = Data.Length;

			Data[DataSize - 1] = (byte)(Data[DataSize - 1] ^ (int)'R');

			for (i = DataSize - 2; i >= 0; i--)
				Data[i] = (byte)(Data[i] ^ Data[i + 1]);

			short shortField;
			string stringField;
			uint index = 20;

			shortField = FileRoutines.GetShortData(Data, ref index);
			int Fields = shortField;

			shortField = FileRoutines.GetShortData(Data, ref index);
			int Records = shortField;

			for (i = 0; i < Fields; i++)
			{
				shortField = FileRoutines.GetShortData(Data, ref index);
				stringField = FileRoutines.GetStrData(Data, ref index, shortField);
			}

			AircraftCategoryConstantLoader TmpData = new AircraftCategoryConstantLoader();
			for (i = 0; i < Records; i++)
			{
				for (int j = 0; j < Fields; j++)
				{
					string FieldName;
					short FieldType;

					bool boolField;
					int intField;
					double floatField;
					//DateTime DateTimeField;
					//decimal CurrencyField;

					shortField = FileRoutines.GetShortData(Data, ref index);
					FieldName = FileRoutines.GetStrData(Data, ref index, shortField);
					shortField = FileRoutines.GetShortData(Data, ref index);
					FieldType = shortField;

					switch (FieldType)
					{
						case 1:
						case 16:
						case 18:
						case 23:
						case 24:
							shortField = FileRoutines.GetShortData(Data, ref index);
							stringField = FileRoutines.GetStrData(Data, ref index, shortField);
							switch (FieldName)
							{
								case "COMMENT":
									TmpData.SetComment(stringField);
									break;
								case "DEFINED_IN":
									TmpData.SetDefinedIn(stringField);
									break;
								case "NAME":
									TmpData.SetName(stringField);
									break;
								case "UNIT":
									TmpData.SetUnit(stringField);
									break;
							}

							break;
						case 2:
						case 4:
							// 16-bit integer field
							// 16-bit unsigned integer field
							shortField = FileRoutines.GetShortData(Data, ref index);
							break;
						case 3:
						case 25:
							// Large integer field
							// 32-bit integer field
							intField = FileRoutines.GetIntData(Data, ref index);
							break;
						case 5:
							// GetData(data, index, BoolField, 2)
							shortField = FileRoutines.GetShortData(Data, ref index);
							boolField = shortField != 0;
							break;
						case 6:
						case 8:
							floatField = FileRoutines.GetDoubleData(Data, ref index);
							switch (FieldName)
							{
								case "MULTIPLIER":
									TmpData.SetMultiplier(floatField);
									break;
								case "A":
									TmpData.SetCategoryValue(aircraftCategory.acA, floatField);
									break;
								case "B":
									TmpData.SetCategoryValue(aircraftCategory.acB, floatField);
									break;
								case "C":
									TmpData.SetCategoryValue(aircraftCategory.acC, floatField);
									break;
								case "D":
									TmpData.SetCategoryValue(aircraftCategory.acD, floatField);
									break;
								case "DL":
									TmpData.SetCategoryValue(aircraftCategory.acDL, floatField);
									break;
								case "E":
									TmpData.SetCategoryValue(aircraftCategory.acE, floatField);
									break;
								case "H":
									TmpData.SetCategoryValue(aircraftCategory.acH, floatField);
									break;
							}
							break;
						case 7:
							byte[] transTemp1; //CurrencyField
							FileRoutines.GetData(Data, ref index, out transTemp1, 8);
							// Date field
							// Time field
							// Date and time field
							break;
						case 9:
						case 10:
						case 11:
							byte[] transTemp0; //DateTimeField;
							FileRoutines.GetData(Data, ref index, out transTemp0, 8);
							break;
					}
				}
				TmpData.SetCategoryValue(aircraftCategory.acA, TmpData.Value[aircraftCategory.acA] * TmpData.Multiplier);
				TmpData.SetCategoryValue(aircraftCategory.acB, TmpData.Value[aircraftCategory.acB] * TmpData.Multiplier);
				TmpData.SetCategoryValue(aircraftCategory.acC, TmpData.Value[aircraftCategory.acC] * TmpData.Multiplier);
				TmpData.SetCategoryValue(aircraftCategory.acD, TmpData.Value[aircraftCategory.acD] * TmpData.Multiplier);
				TmpData.SetCategoryValue(aircraftCategory.acDL, TmpData.Value[aircraftCategory.acDL] * TmpData.Multiplier);
				TmpData.SetCategoryValue(aircraftCategory.acE, TmpData.Value[aircraftCategory.acE] * TmpData.Multiplier);
				TmpData.SetCategoryValue(aircraftCategory.acH, TmpData.Value[aircraftCategory.acH] * TmpData.Multiplier);
				TmpData.SetAssigned(true);

				int lastIndex = Array.FindLastIndex<string>(AircraftCategoryDataNames, name => name == TmpData.Name);
				//if (lastIndex < 0)			throw new Exception(TmpData.Name + " is not found");
				if (lastIndex >= 0)
					ReplaceItem(TmpData, lastIndex);
				//AddItem(TmpData);
			}
		}
	}

	public class AircraftCategoryConstantLoader : AircraftCategoryConstant
	{
		public AircraftCategoryConstantLoader()
			: base()
		{

		}

		public void SetName(string value)
		{
			base.Name = value;
		}

		public void SetUnit(string value)
		{
			base.Unit = value;
		}

		public void SetDefinedIn(string value)
		{
			base.DefinedIn = value;
		}

		public void SetComment(string value)
		{
			base.Comment = value;
		}

		public void SetMultiplier(double value)
		{
			base.Multiplier = value;
		}

		public void SetValue(EnumArray<double, aircraftCategoryData> value)
		{
			for (int i = 0; i < base.Value.Length; i++)
				base.Value[i] = value[i];
		}

		public void SetCategoryValue(aircraftCategory category, double value)
		{
			base.Value[category] = value;
		}
	}
}

