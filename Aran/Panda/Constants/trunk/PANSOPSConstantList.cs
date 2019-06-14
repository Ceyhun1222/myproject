using System;
using Microsoft.VisualBasic;
using Aran.PANDA.Common;

namespace Aran.PANDA.Constants
{
	public class PANSOPSConstantList : ConstantList<PansopsConstant>
	{
		public const double PBNInternalTriggerDistance = 15 * 1852.2;
		public const double PBNTerminalTriggerDistance = 30 * 1852.2;
		public const double SBASTriggerDistance = 25 * 1852.2;	//46000.0;
		//public const double GNSSTriggerDistance = 46000.0;

		public readonly string[] PANSOPSDataNames = new string[]
		{
		            "dpOIS", "dpMOC", "dpNGui_Ar1", "dpNGui_Ar1_Wd", "dpTr_AdjAngle",
		            "dpAr1_OB_TrAdj", "dpAr1_IB_TrAdj", "dpAr2_Bnd_TrAdj", "dpT_Bank",
		            "dpWind_Speed", "dpT_TechToleranc", "dpT_Init", "dpT_Init_Wd",
		            "dpObsClr", "dpT_Gui_dist", "dpH_abv_DER", "dpOIS_abv_DER", "dpPDG_60",
		            "dpPDG_Nom", "dpStr_Gui_dist", "dpafTrn_OSplay", "dpafTrn_ISplay",
		            "dpOv_Nav_PDG", "dpTP_by_DME_div", "dpNGui_Ar1_Dist", "dpOD1_ZoneAdjA",
		            "dpOD2_ZoneAdjA", "dpGui_Ar1_Wd", "dpGui_Ar1", "ISA", "dpSecAreaCutAngl",
		            "dpMaxPosPDG", "dpInterMinAngle", "dpInterMaxAngle", "dpFlightTechTol",

		            // ================ Arrival =======================
		            "arBufferMSA", "arMSARoundThresh", "arMinInterDist", "arMinInterToler",
		            "arStrInAlignment", "arMaxRangeFAS", "arCirclAprShift", "arHoldAreaEdge",
		            "arHoldingBuffer", "arIASegmentMOC", "arISegmentMOC", "arFASeg_FAF_MOC",
		            "arFASegmentMOC", "arSOCdelayTime", "arNearTerrWindSp", "arISAmax",
		            "arISAmin", "arMA_InterMOC", "arMA_FinalMOC", "arMAS_Climb_Min", "arMAS_Climb_Max",
		            "arMAS_Climb_Nom", "arMATurnAlt", "arT_TechToleranc", "arTP_by_DME_div",
		            "arT_Gui_dist", "arafTrn_OSplay", "arafTrn_ISplay", "arSecAreaCutAngl",
		            "arMATurnTrshAngl","arFAFLenght", "arMinRangeFAS", "arAbv_Treshold", "arFADescent_Min",
		            "arFADescent_Nom", "arImRange_Min", "arImRange_Nom", "arImRange_Max", "arImMaxIntercept",
		            "arIFHalfWidth", "arMinISlen00_90", "arMinISlen91_96", "arMinISlen97_02",
		            "arMinISlen03_08", "arMinISlen09_14", "arMinISlen15_20", "arFAFTolerance",
		            "arIFTolerance", "arImDescent_Max", "arFAPMaxRange", "arOverHeadToler",
		            "arTrackAccuracy", "arMAPilotToleran", "arMA_SplayAngle","arSecAr", "arOptimalFAFRang",
		            "arCurvatureCoeff", "arFixMaxIgnorGrd", "arMOCChangeDist", "arAddMOCCoef",
		            "arFIX15PlaneRang", "arFAFOptimalDist", "arIAFMinDMERange", "arIAFMaxTurnAngl",
		            "arIAFMinTurnAngl", "arIAFMinGuadLen", "arVisAverBank", "EnRouteSplayAngl", "arMABankAngle",
		            "arIADescent_Nom", "arIADescent_Max", "EnTechTolerance", "arIBankTolerance",
		            "arBankAngle", "arIPilotToleranc", "arMSAMOC", "rnvFlyOInterBank", "rnvFlyByTechTol",
		            "rnvFlyOTechTol", "rnvImMinDist", "enrMaxTurnAngle", "rnvIFMaxTurnAngl","enBankTolerance",
		            "dpPilotTolerance", "dpBankTolerance", "enrMOC", "arArrivalMOC","arDRNomAltitude",
                    "arRDH", "arILSSectorWidth", "arMinGPAngle", "arOptGPAngle", "arMaxGPAngleCat1", "arMaxGPAngleCat2",
                    "arMinMACGrad", "arStdMACGrad", "arMaxMACGrad",
					//=============== en-route
					"TurnIAS", "BankAngle"
                };

		public PANSOPSConstantList()
			: base(Enum.GetValues(typeof(ePANSOPSData)).Length)
		{
			pansopsEnumArray = new EnumArray<string, ePANSOPSData>();
			int i = -1;
			foreach (ePANSOPSData item in Enum.GetValues(typeof(ePANSOPSData)))
			{
				i++;
				pansopsEnumArray[item] = PANSOPSDataNames[i];
			}
		}

		public PansopsConstant this[ePANSOPSData dataType]
		{
			get { return base.ConstantByIndex((int)dataType); }
		}

		//public double this[ePANSOPSData dataType]
		//{
		//	get { return base.ConstantByIndex((int)dataType).Value; }
		//}

		public PansopsConstant this[string name]
		{
			get { return base.ConstantByName(name); }
		}

		private EnumArray<string, ePANSOPSData> pansopsEnumArray;
	}

	public class PANSOPSConstantListLoader : PANSOPSConstantList
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

			PansopsConstantLoader TmpData = new PansopsConstantLoader();
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

				int lastIndex = Array.FindLastIndex<string>(PANSOPSDataNames, name => name == TmpData.Name);

				if (lastIndex >= 0)
				{
					TmpData.SetValue(TmpData.Value * TmpData.Multiplier);
					TmpData.SetAssigned(true);
					ReplaceItem(TmpData, lastIndex);
				}
				//System.Windows.Forms.MessageBox.Show(i.ToString() + ": " + TmpData.Name + " is not found");
				//throw new Exception(TmpData.Name + " is not found");
			}
		}

		public void Merge(PANSOPSConstantList list, ePANSOPSData from, ePANSOPSData to = (ePANSOPSData)(-1))
		{
			Array pansopsData = Enum.GetValues(typeof(ePANSOPSData));
			int toIndex = (int)to;

			if (toIndex < 0)
				toIndex = pansopsData.Length;

			for (int i = (int)from; i < toIndex; i++)
			{
				//ePANSOPSData indx = (ePANSOPSData)pansopsData.GetValue(i);
				PansopsConstant tmp = list[(ePANSOPSData)pansopsData.GetValue(i)];

				if (tmp != null)
					this.ReplaceItem(tmp, i);
			}
		}
	}

	public class PansopsConstantLoader : PansopsConstant
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
