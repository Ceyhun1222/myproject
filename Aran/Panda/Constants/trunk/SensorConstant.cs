using Aran.PANDA.Common;

namespace Aran.PANDA.Constants
{
	public class SensorConstant : NamedConstantObject
	{
		public static string[] FIXRoleStyleNames = {"IAF", "IAF", "IF", "FAF", "MAPt",
		"MATF", "MATF", "MAHF", "MAHF", "IDEP", "DEP", "TP",
		"IAF", "IF", "FAF", "MAPt",
		"MATF", "MATF", "DEP", "SDF"};

		public SensorConstant()
			: base()
		{
			FIXRole = "";
			Accuracy = 0.0;
			Threshold = 0.0;
			FTT = 0.0;
			ATT = 0.0;
			XTT = 0.0;
			SemiWidth = 0.0;
		}

		public SensorConstant(ePBNClass pBNClassType)
			: base()
		{
			PBNClass = pBNClassType;
			FIXRole = "";
			Accuracy = 0.0;
			Threshold = 0.0;
			FTT = 0.0;
			ATT = 0.0;
			XTT = 0.0;
			SemiWidth = 0.0;
		}
		/*
		public SensorConstant()
            : this(ePBNClass.RNAV1)
		{
		}
		*/
		public override AranObject Clone()
		{
			SensorConstant result = new SensorConstant(PBNClass);
			result.Assign(this);
			result.FIXRole = FIXRole;
			result.Accuracy = Accuracy;
			result.Threshold = Threshold;
			result.FTT = FTT;
			result.ATT = ATT;
			result.XTT = XTT;
			result.SemiWidth = SemiWidth;
			return result;
		}

		public string FIXRole { get; protected set; }
		public double Accuracy { get; protected set; }
		public double Threshold { get; protected set; }
        public double FTT { get; protected set; }
        public double ATT { get; protected set; }
        public double XTT { get; protected set; }
        public double SemiWidth { get; protected set; }
        public ePBNClass PBNClass { get; protected set; }
	}
}
