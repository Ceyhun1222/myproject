using System;
using Aran.PANDA.Common;
using Microsoft.VisualBasic;

namespace Aran.PANDA.Constants
{
	public class FteConstantList : ConstantList<FteContant>
	{
		public readonly string[] FteDataNames = new string[]
		{
			"RNP APCH","RNAV 1", "RNAV 2",
			"RNP 0.3", "RNP 1", "RNP 4","RNAV 5",
			"GNSS", "RNP APCH Final"
		};

		public FteConstantList()
			: base(Enum.GetValues(typeof(ePBNClass)).Length)
		{

			fteEnumArray = new EnumArray<string, ePBNClass>();
			int i = -1;
			foreach (ePBNClass item in Enum.GetValues(typeof(ePBNClass)))
			{
				i++;
				fteEnumArray[item] = FteDataNames[i];
			}
		}

		public FteContant this[ePBNClass dataType]
		{
			get { return base.ConstantByIndex((int)dataType); }
		}

		public FteContant this[string name]
		{
			get { return base.ConstantByName(name); }
		}


		private EnumArray<string, ePBNClass> fteEnumArray;
	}
	public class FteConstantListLoader : FteConstantList
	{
		public void LoadFromFile(string fileName)
		{
			int i;
			int FileNumber = FileSystem.FreeFile();
			FileSystem.FileOpen(FileNumber, fileName, OpenMode.Binary, OpenAccess.Read, (Microsoft.VisualBasic.OpenShare)(-1), -1);

			string FileSign = FileSystem.InputString(FileNumber, 20);
			FileSystem.FileClose(FileNumber);

			if (FileSign != "Anplan DATABASE file")
				throw new Exception("Invalid PANSOPS constants DATABASE file");

			byte[] Data = System.IO.File.ReadAllBytes(fileName);
			int dataSize = Data.Length;

			Data[dataSize - 1] = (byte)(Data[dataSize - 1] ^ (int)'R');

			for (i = dataSize - 2; i >= 0; i--)
				Data[i] = (byte)(Data[i] ^ Data[i + 1]);

			string stringField;
			short shortField;
			uint index = 20;

			int Fields = FileRoutines.GetShortData(Data, ref index);
			int Records = FileRoutines.GetShortData(Data, ref index);

			for (i = 0; i < Fields; i++)
			{
				shortField = FileRoutines.GetShortData(Data, ref index);
				stringField = FileRoutines.GetStrData(Data, ref index, shortField);
			}

			FteConstantLoader TmpData = new FteConstantLoader();
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
							// GetData(data, index, BoolField, 1)
							shortField = FileRoutines.GetShortData(Data, ref index);
							boolField = shortField != 0;
							break;
						case 6:
						case 8:
							floatField = FileRoutines.GetDoubleData(Data, ref index);
							if (FieldName == "VALUE")
								TmpData.SetValue(floatField);
							else if (FieldName == "MULTIPLIER")
								TmpData.SetMultiplier(floatField);
							break;
						case 7:
							byte[] transTemp1 = null; //CurrencyField
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

				int lastIndex = Array.FindLastIndex<string>(FteDataNames, name => name == TmpData.Name);
				//if (lastIndex < 0)		throw new Exception(TmpData.Name + " is not found");

				if (lastIndex >= 0)
				{
					TmpData.SetValue(TmpData.Value * TmpData.Multiplier);
					TmpData.SetAssigned(true);
					ReplaceItem(TmpData, lastIndex);
				}
			}
		}

		public void Merge(FteConstantList list)
		{
			Array pansopsData = Enum.GetValues(typeof(ePBNClass));
			for (int i = 0; i < pansopsData.Length; i++)
			{
				FteContant tmp = list[(ePBNClass)pansopsData.GetValue(i)];
				if (tmp != null)
					this.ReplaceItem(tmp, i);
			}
		}
	}

	public class FteConstantLoader : FteContant
	{
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

		public void SetValue(double value)
		{
			base.Value = value;
		}
	}
}
