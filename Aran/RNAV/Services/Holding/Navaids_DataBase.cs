using System;
using System.Data.OleDb;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Common;
using Aran.PANDA.Rnav.Holding.Properties;

namespace Holding
{
    public class Navaids_DataBase
    {
        public Navaids_DataBase()
        {
            double Value;
            double Multiplier;
            string ParName;
            OleDbDataReader navaidTypesReader;

            VOR = new VORData();
            DME = new DMEData();
            NDB = new NDBData();
            LLZ = new LLZData();

            if (!OpenDbfConnection(GlobalParams.Constant_G.InstallDir + "\\Navaids\\"))
                throw new Exception("Cannot open file");

            OleDbCommand navaidCmd = new OleDbCommand("Select * from Navaids", _conn);
            OleDbCommand navaidTypesCmd = _conn.CreateCommand();
            try
            {
                OleDbDataReader navaidReader = navaidCmd.ExecuteReader();
                while (navaidReader.Read())
                {
                    navaidTypesCmd.CommandText = "select * from " + navaidReader["name"].ToString();
                    navaidTypesReader = navaidTypesCmd.ExecuteReader();
                    while (navaidTypesReader.Read())
                    {
                        ParName = (string)navaidTypesReader["PARAM_NAME"];
                        Multiplier = (double)navaidTypesReader["MULTIPLIER"];
                        Value = (double)navaidTypesReader["VALUE"] * Multiplier;
                        if (navaidTypesReader["UNIT"].ToString() == "rad")
                            Value = ARANMath.RadToDeg(Value);

                        switch (navaidReader["name"].ToString())
                        {
                            case "VOR":
                                if (ParName == "Range")
                                    VOR.Range = Value;
                                else if (ParName == "FA Range")
                                    VOR.FA_Range = Value;
                                else if (ParName == "Initial width")
                                    VOR.InitWidth = Value;
                                else if (ParName == "Splay angle")
                                    VOR.SplayAngle = Value;
                                else if (ParName == "Tracking tolerance")
                                    VOR.TrackingTolerance = Value;
                                else if (ParName == "Intersecting tolerance")
                                    VOR.IntersectingTolerance = Value;
                                else if (ParName == "Cone angle")
                                    VOR.ConeAngle = Value;
                                else if (ParName == "Track accuracy")
                                    VOR.TrackAccuracy = Value;
                                else if (ParName == "Lateral deviation coef.")
                                    VOR.LateralDeviationCoef = Value;
                                else if (ParName == "EnRoute Tracking toler")
                                    VOR.EnRouteTrackingToler = Value;
                                else if (ParName == "EnRoute Tracking2 toler")
                                    VOR.EnRouteTracking2Toler = Value;
                                else if (ParName == "EnRoute Inter toler")
                                    VOR.EnRouteInterToler = Value;
                                else if (ParName == "EnRoute PrimArea With")
                                    VOR.EnRoutePrimAreaWith = Value;
                                else if (ParName == "EnRoute SecArea With")
                                    VOR.EnRouteSecAreaWith = Value;
                                break;

                            case "NDB":

                                if (ParName == "Range")
                                    NDB.Range = Value;
                                else if (ParName == "FA Range")
                                    NDB.FA_Range = Value;
                                else if (ParName == "Initial width")
                                    NDB.InitWidth = Value;
                                else if (ParName == "Splay angle")
                                    NDB.SplayAngle = Value;
                                else if (ParName == "Tracking tolerance")
                                    NDB.TrackingTolerance = Value;
                                else if (ParName == "Intersecting tolerance")
                                    NDB.IntersectingTolerance = Value;
                                else if (ParName == "Cone angle")
                                    NDB.ConeAngle = Value;
                                else if (ParName == "Track accuracy")
                                    NDB.TrackAccuracy = Value;
                                else if (ParName == "Entry into the cone accuracy")
                                    NDB.Entry2ConeAccuracy = Value;
                                else if (ParName == "Lateral deviation coef.")
                                    NDB.LateralDeviationCoef = Value;
                                else if (ParName == "EnRoute Tracking toler")
                                    NDB.EnRouteTrackingToler = Value;
                                else if (ParName == "EnRoute Tracking2 toler")
                                    NDB.EnRouteTracking2Toler = Value;
                                else if (ParName == "EnRoute Inter toler")
                                    NDB.EnRouteInterToler = Value;
                                else if (ParName == "EnRoute PrimArea With")
                                    NDB.EnRoutePrimAreaWith = Value;
                                else if (ParName == "EnRoute SecArea With")
                                    NDB.EnRouteSecAreaWith = Value;
                                break;

                            case "DME":
                                if (ParName == "Range")
                                    DME.Range = Value;
                                else if (ParName == "Minimal Error")
                                    DME.MinimalError = Value;
                                else if (ParName == "Error Scaling Up")
                                    DME.ErrorScalingUp = Value;
                                else if (ParName == "Slant Angle")
                                    DME.SlantAngle = Value;
                                else if (ParName == "TP_div")
                                    DME.TP_div = Value;
                                break;

                            case "LLZ":

                                if (ParName == "Range")
                                    LLZ.Range = Value;
                                else if (ParName == "Tracking tolerance")
                                    LLZ.TrackingTolerance = Value;
                                else if (ParName == "Intersecting tolerance")
                                    LLZ.IntersectingTolerance = Value;
                                break;
                        }
                    }
                    navaidTypesReader.Close();
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);
                System.Windows.Forms.MessageBox.Show(Resources.NavaidDb_cannt_read_data, Resources.Holding_Caption);
            }

            //	VOR.OnNAVRadius = System.Math.Sin(ARANFunctions.DegToRad(VOR.TrackAccuracy + arTrackAccuracy.Value)) / System.Math.Sin(DegToRad(90.0 - arTrackAccuracy.Value)) * arOverHeadToler.Value;
            //	NDB.OnNAVRadius = System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy + arTrackAccuracy.Value)) / System.Math.Sin(DegToRad(90.0 - arTrackAccuracy.Value)) * arOverHeadToler.Value;
            CloseConnection();

        }

        private bool OpenDbfConnection(string pathFolder)
        {
            try
            {
                _conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathFolder + ";Extended Properties=dBASE IV;");
                _conn.Open();
                return true;
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);
                return false;
            }

        }
        private void CloseConnection()
        {
            _conn.Close();
        }

        public int NavaidTypesCount;
        public VORData VOR { get; private set; }
        public NDBData NDB { get; private set; }
        public DMEData DME { get; private set; }
        public LLZData LLZ { get; private set; }

        private OleDbConnection _conn;
    }
}
