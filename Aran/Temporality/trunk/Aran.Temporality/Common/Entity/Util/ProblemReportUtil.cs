using System;
using Aran.Aim;

namespace Aran.Temporality.Common.Entity.Util
{
	[Serializable]
	public class ProblemReportUtil
	{
		public FeatureType FeatureType;
		public Guid Guid;
		public int Flag;
	}

	[Serializable]
	public class BusinessRuleProblemReportUtil : ProblemReportUtil
	{
		public int RuleId;
	}

	[Serializable]
	public class LinkProblemReportUtil : ProblemReportUtil
	{
		public string PropertyPath;
		public string ReferenceFeatureType;
		public Guid ReferenceFeatureIdentifier;
	}

    [Serializable]
    public class SyntaxProblemReportUtil : ProblemReportUtil
    {
        public string PropertyPath;
        public string StringValue;
        public string Violation;
    }
}