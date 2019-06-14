using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.UI.Forms;
using Aran.Panda.RNAV.RNPAR.Utils;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class RNPContext
    {
        public PreFinalPhase PreFinalPhase { get; private set; }
        public FinalPhase FinalPhase { get; private set; }
        public IntermediatePhase IntermediatePhase { get; private set; }
        public InitialPhase InitialPhase { get; private set; }
        public MissedApproachPhase MissedApproachPhase { get; private set; }
        public RNP_ARReportForm ReportForm { get; private set; }

        public readonly double  ErrorAngle = ARANMath.DegToRad(0.5);

        public  string[] OASPlaneNames => new string[] { "Zero", "W", "X", "Y", "Z", "Y", "X", "W*", "Common", "Non Prec." };
        public  string[] OFZPlaneNames => new string[] { "Zero", "Inner Approach", "Inner transitional1", "Inner transitional2", "Balking landing", "Inner transitional2", "Inner transitional1", "Common" };
        public  string[] ILSPlaneNames => new string[] { "Zero", "Approach 1", "Approach 2", "Transitional A", "Transitional B", "Transitional C", "Transitional D", "Missed Approach", "Transitional D", "Transitional C", "Transitional B", "Transitional A", "Common" };
        public  string[] RNPARPlaneNames => new string[] { "Zero", "Approach", "Missed Approach" };
        public D3DPolygone[] OFZPlanes = new D3DPolygone[8];
        public  int[] OFZPlanesElement = new int[8];
        public  bool OFZPlanesState;

        public RNPContext()
        {
            PreFinalPhase = new PreFinalPhase();
            FinalPhase = new FinalPhase();
            IntermediatePhase = new IntermediatePhase();
            InitialPhase = new InitialPhase();
            MissedApproachPhase = new MissedApproachPhase();
            
        }

        public void CreateReportForm()
        {
            ReportForm = new RNP_ARReportForm();
        }

        
    }
}
