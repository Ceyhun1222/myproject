using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
//using System.Diagnostics.Debug;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{
   

    [TypeConverter(typeof(PropertySorter))]
    public class AirportHeliport : PDMObject
    {
        private string _designator;
        [Description("The four letter ICAO code of the aerodrome.")]
        [PropertyOrder(10)]
        [Mandatory(true)]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private string _designatorIATA;
        [PropertyOrder(20)]
        [Description("The three letter IATA code of the aerodrome.")]
        public string DesignatorIATA
        {
            get { return _designatorIATA; }
            set { _designatorIATA = value; }
        }

        private string _name;
        [Description("The full free text name of the aerodrome.")]
        [PropertyOrder(30)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private double _magneticVariation;
        [Description("The angular difference between True North and Magnetic North.")]
        [PropertyOrder(40)]
        [Mandatory(true)]
        public double MagneticVariation
        {
            get { return _magneticVariation; }
            set { _magneticVariation = value; }
        }

        //private double _speedLimit;

        //public double SpeedLimit
        //{
        //    get { return _speedLimit; }
        //    set { _speedLimit = value; }
        //}



        private double _transitionAltitude;
        [PropertyOrder(50)]
        [Description("The value of the transition altitude.")]
        public double TransitionAltitude
        {
            get { return _transitionAltitude; }
            set { _transitionAltitude = value; }
        }

        private UOM_DIST_VERT _transitionAltitudeUOM;
        [PropertyOrder(60)]
        [Description("The value of the transition altitude.")]
        public UOM_DIST_VERT TransitionAltitudeUOM
        {
            get { return _transitionAltitudeUOM; }
            set { _transitionAltitudeUOM = value; }
        }

        //private double _transitionLevel;

        //public double TransitionLevel
        //{
        //    get { return _transitionLevel; }
        //    set { _transitionLevel = value; }
        //}
        //private string _publicMilitaryIndicator;

        //public string PublicMilitaryIndicator
        //{
        //    get { return _publicMilitaryIndicator; }
        //    set { _publicMilitaryIndicator = value; }
        //}

        //private string _magneticTrueIndicator;
        //[Description("")]
        //public string MagneticTrueIndicator
        //{
        //    get { return _magneticTrueIndicator; }
        //    set { _magneticTrueIndicator = value; }
        //}


        private List<Runway> _RunwayList;
        [Description("Runways")]
        [Browsable(false)]
        [PropertyOrder(60)]
        public List<Runway> RunwayList
        {
            get { return _RunwayList; }
            set { _RunwayList = value; }
        }

 
        //[Browsable(false)]
        //public override double Elev_m
        //{
        //    get
        //    {
        //        return base.Elev_m;
        //    }
        //    set
        //    {
        //        base.Elev_m = value;
        //    }
        //}

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.AirportHeliport.ToString();
            }
        } 

        public AirportHeliport()
        {
            
        }



        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
           bool res =true;
           try
           {
               ITable tbl = AIRTRACK_TableDic[this.GetType()];

               IRow row = tbl.CreateRow();

               CompileRow(ref row);

               row.Store();


               if (this.RunwayList != null)
               {

                   foreach (Runway rwy in this.RunwayList)
                   {
                       rwy.ID_AirportHeliport = this.ID;
                       rwy.StoreToDB(AIRTRACK_TableDic);

                   }
               }

              }
           catch(Exception ex)
           {
               this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace };
               res = false;
           }

            
          
           return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            //findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, "f");
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("designatorIATA"); if (findx >= 0) row.set_Value(findx, this.DesignatorIATA);
            findx = row.Fields.FindField("magneticVariation"); if (findx >= 0) row.set_Value(findx, this.MagneticVariation);
            findx = row.Fields.FindField("name"); if (findx >= 0) row.set_Value(findx, this.Name);
            findx = row.Fields.FindField("Airport_ReferencePt_Latitude"); if (findx >= 0) row.set_Value(findx, this.Lat);
            findx = row.Fields.FindField("Airport_ReferencePt_Longitude"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("Elev"); if (findx >= 0) row.set_Value(findx, this.Elev);
            findx = row.Fields.FindField("ElevUom"); if (findx >= 0) row.set_Value(findx, this.Elev_UOM.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());
                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }

        }

        public override string GetObjectLabel()
        {
            return "ADHP " + this.Designator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {

            if (base.DeleteObject(AIRTRACK_TableDic) == 0) return 0;

            ITable tbl = AIRTRACK_TableDic[this.GetType()];


            if (this.RunwayList != null)
            {

                foreach (Runway rwy in this.RunwayList)
                {
                    if (rwy.RunwayDirectionList != null)
                    {
                        foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                        {
                            if (rdn.Related_NavaidSystem != null)
                            {
                                foreach (NavaidSystem ns in rdn.Related_NavaidSystem)
                                {
                                    ns.DeleteObject(AIRTRACK_TableDic);
                                }
                            }
                        }
                    }
                }
            }

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }
            
            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);
            
            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {

            List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

            if (this.RunwayList != null)
            {

                foreach (Runway rwy in this.RunwayList)
                {
                  List<string> part=  rwy.HideBranch(AIRTRACK_TableDic, Visibility);
                  res.AddRange(part);
                }
            }

            return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.RunwayList != null)
            {

                foreach (Runway rwy in this.RunwayList)
                {
                    List<string> part = rwy.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string ToString()
        {
            return this.Designator;
        }


    }

}
