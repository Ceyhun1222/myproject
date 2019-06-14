using System;
using Microsoft.VisualBasic;
using Aran.PANDA.Common;

namespace Aran.PANDA.Constants
{
	public class SensorConstantList : ConstantList <SensorConstant>
	{
		public SensorConstantList()
			: base()
		{
		}

		public SensorConstant this[eFIXRole dataType]
		{
			get
			{
				if (dataType <= eFIXRole.PBN_FAF)
					return base.ConstantByIndex((int)dataType);

				return base.ConstantByIndex((int)dataType - (int)eFIXRole.PBN_IAF);
			}
		}

		public SensorConstant this[string name]
		{
			get { return base.ConstantByName(name); }
		}
		//private EnumArray<string, eFIXRole> gnssConstansEnumArray;
	}

	public class SensorConstantListLoader : SensorConstant
	{
		public SensorConstantListLoader(ePBNClass pbnClass) :
			base(pbnClass)
		{

		}

		public SensorConstantListLoader()
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

		public void SetFIXRole(string value)
		{
			base.FIXRole = value;
		}

		public void SetAccuracy(double value)
		{
			base.Accuracy = value;
		}

		public void SetThreshold(double value)
		{
			base.Threshold = value;
		}

		public void SetFTT(double value)
		{
			base.FTT = value;
		}

		public void SetATT(double value)
		{
			base.ATT = value;
		}

		public void SetXTT(double value)
		{
			base.XTT = value;
		}

		public void SetSemiWidth(double value)
		{
			base.SemiWidth = value;
		}
	}

	public class SensorContantsListLoader : SensorConstantList
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

			int Fields = FileRoutines.GetShortData(Data, ref index);
			int Records = FileRoutines.GetShortData(Data, ref index);

			for (i = 0; i < Fields; i++)
			{
				shortField = FileRoutines.GetShortData(Data, ref index);
				stringField = FileRoutines.GetStrData(Data, ref index, shortField);
			}

			SensorConstantListLoader TmpData = new SensorConstantListLoader();

			for (i = 0; i < Records; i++)
			{
				for (int j = 0; j < Fields; j++)
				{
					string FieldName;

					bool boolField;
					int intField;
					double floatField;
					//DateTime DateTimeField;
					//decimal CurrencyField;

					shortField = FileRoutines.GetShortData(Data, ref index);
					FieldName = FileRoutines.GetStrData(Data, ref index, shortField);
					short FieldType = FileRoutines.GetShortData(Data, ref index);

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
								case "FIX":
									TmpData.SetFIXRole(stringField);
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
								case "ACCURACY":
									TmpData.SetAccuracy(floatField);
									break;
								case "THRESHOLD":
									TmpData.SetThreshold(floatField);
									break;
								case "FTT":
									TmpData.SetFTT(floatField);
									break;
								case "ATT":
									TmpData.SetATT(floatField);
									break;
								case "XTT":
									TmpData.SetXTT(floatField);
									break;
								case "SEMIWIDTH":
									TmpData.SetSemiWidth(floatField);
									break;
							}
							break;
						case 7:
							//CurrencyField
							byte[] transTemp1 = null;
							FileRoutines.GetData(Data, ref index, out transTemp1, 8);
							break;
						case 9:
						case 10:
						case 11:
							// Date field
							// Time field
							// Date and time field
							byte[] transTemp0 = null; //DateTimeField;
							FileRoutines.GetData(Data, ref index, out transTemp0, 8);
							break;
					}
				}

				if (TmpData.Name == "")
				{
					switch (TmpData.FIXRole)
					{
						case "IAF>56":
							TmpData.SetName("IAF_GT_56");
							break;
						case "IAF<=56":
							TmpData.SetName("IAF_LE_56");
							break;
						case "IF":
							TmpData.SetName("IF");
							break;
						case "FAF":
							TmpData.SetName("FAF");
							break;
						case "MAPt":
							TmpData.SetName("MAPt");
							break;
						case "MATF<=56":
							TmpData.SetName("MATF_LE_56");
							break;
						case "MATF>56":
							TmpData.SetName("MATF_GT_56");
							break;
						case "MAHF<=56":
							TmpData.SetName("MAHF_LE_56");
							break;
						case "MAHF>56":
							TmpData.SetName("MAHF_GT_56");
							break;
						case "IDEP":
							TmpData.SetName("IDEP");
							break;
						case "DEP":
							TmpData.SetName("DEP");
							break;
					}
				}
				else
				{
					switch (TmpData.FIXRole)
					{
						case "IAF":
							TmpData.SetName("PBN_IAF");
							break;
						case "IF":
							TmpData.SetName("PBN_IF");
							break;
						case "FAF":
							TmpData.SetName("PBN_FAF");
							break;
						case "MAPt":
							TmpData.SetName("PBN_MAPt");
							break;
						case "MATF<28":
							TmpData.SetName("PBN_MATF_LT_28");
							break;
						case "MATF>=28":
							TmpData.SetName("PBN_MATF_GE_28");
							break;
					}
				}

				TmpData.SetAccuracy(TmpData.Accuracy * TmpData.Multiplier);
				TmpData.SetThreshold(TmpData.Threshold * TmpData.Multiplier);
				TmpData.SetFTT(TmpData.FTT * TmpData.Multiplier);
				TmpData.SetATT(TmpData.ATT * TmpData.Multiplier);
				TmpData.SetXTT(TmpData.XTT * TmpData.Multiplier);
				TmpData.SetSemiWidth(TmpData.SemiWidth * TmpData.Multiplier);
				TmpData.SetAssigned(true);

				AddItem(TmpData);
			}
		}
	}
}
