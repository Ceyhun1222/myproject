using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PDM
{
    [Serializable()]
    public class ExeptionMessage
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        private string _source;

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _stackTrace;

        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        public ExeptionMessage()
        {
        }
    }


    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class PDMObject : ICloneable
    {
        private string _ID;
        //[Browsable(false)]
        [ReadOnlyAttribute(true)]
        [PropertyOrder(1)]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Lat;
        [Category("Geo")]
        [Mandatory(true)]
        [PropertyOrder(2)]
        [Description("The latitude of the object")]
        public virtual string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        private string _Lon;
        [Category("Geo")]
        [Mandatory(true)]
        [PropertyOrder(3)]
        [Description("The longitude of the object")]
        public virtual string Lon
        {
            get { return _Lon; }
            set { _Lon = value;

            }
        }

        private double? _x;
        [Category("Geo")]
        [Mandatory(true)]
        [Description("The longitude of the object")]
        [Browsable(false)]
        public double? X
        {
            get { return _x; }
            set { _x = value; }
        }

        private double? _Y;
        [Category("Geo")]
        [Mandatory(true)]
        [Description("The latitude of the object")]
        [Browsable(false)]
        public double? Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        private double? _Elev = null;
        [Category("Geo")]
        [Mandatory(true)]
        [Description("The value of the object elevation")]
        [PropertyOrder(5)]
        public virtual double? Elev
        {
            get { return _Elev; }
            set { _Elev = value; }
        }

        private UOM_DIST_VERT _Elev_UOM;
        [Category("Geo")]
        [Mandatory(true)]
        [Description("The unit of measurement for vertical distances")]
        [PropertyOrder(6)]
        public virtual UOM_DIST_VERT Elev_UOM
        {
            get { return _Elev_UOM; }
            set { _Elev_UOM = value; }
        }

        private DateTime _actualDate;
        [DisplayName(@"Effective date")]
        [Description(@"Effective date")]
        [Category(@"Temporality")]
        [PropertyOrder(8)]
        [ReadOnly(true)]
        // [Browsable(false)]
        public virtual DateTime ActualDate
        {
            get { return _actualDate; }
            set { _actualDate = value; }
        }

        private bool _createdAutomatically;
        [Browsable(false)]
        [PropertyOrder(9)]
        public bool CreatedAutomatically
        {
            get { return _createdAutomatically; }
            set { _createdAutomatically = value; }
        }

        private List<string> _Notes;
        //[Browsable(false)]
        public List<string> Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }


        private ExeptionMessage _exeptionDetails;
        [Browsable(false)]
        [XmlIgnore]
        public ExeptionMessage ExeptionDetails
        {
            get { return _exeptionDetails; }
            set { _exeptionDetails = value; }
        }

        private string _sourceDetail;
        [ReadOnly(true)]
        // [Browsable(false)]
        public virtual string SourceDetail
        {
            get { return _sourceDetail; }
            set { _sourceDetail = value; }
        }

        [ReadOnly(true)]
        public dataInterpretation Interpritation { get; set; }

        public PDMObject()
        {
            //this.ID = Guid.NewGuid().ToString();
            //this.ActualDate = DateTime.Now;
            this.Lat = ""; ;
            this.Lon = ""; ;
            this.CreatedAutomatically = true;
        }

        private IGeometry _geo;
        [XmlIgnore]
        [Browsable(false)]
        public IGeometry Geo
        {
            get { return _geo; }
            set { _geo = value; }
        }

        public virtual TimePeriod ValidTime { get; set; }

        public virtual TimePeriod FeatureLifeTime { get; set; }

        public virtual GeoProperties GeoProperties { get; set; }

        public virtual FeatureMetadata Metadata { get; set; }

        [Browsable(false)]
        //[ReadOnlyAttribute(true)]
         public virtual PDM_ENUM PDM_Type
         {
             get { return PDM_ENUM.PDMObject; }
         }
       

        public virtual bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            return false;

        }

        public virtual List<string> GetExceptionMessage()
        {

            List<string> res = new List<string>();
            if (this.ExeptionDetails == null) return res;


             res.Add(this.GetType().Name + (char)9 + this.GetObjectLabel() + (char)9 + this.ID + (char)9 + this.ExeptionDetails.Message + (char)9 + this.ExeptionDetails.Source + (char)9 + this.ExeptionDetails.StackTrace);
             return res;
        }

        public virtual bool StoreToDB(ITable table)
        {
            bool res = true;
            try
            {


                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(table as FeatureClass).Workspace;
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                IRow row = table.CreateRow();

                CompileRow(ref row);

                row.Store();

                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
            }
              catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 
          



            return res;
        }

        public virtual int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            int res =0;
            if (MessageBox.Show("Delete selected Object?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) res = 1;

            return res;
        }

        public virtual bool UpdateDB(ITable table)
        {
            try
            {
                this.RebuildGeo();

                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(table as FeatureClass).Workspace;
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                //ComReleaser comReleaser = new ComReleaser();
                IQueryFilter qry = new QueryFilterClass();
                qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
                ICursor cursor = table.Update(qry, true);
                //comReleaser.ManageLifetime(cursor);

                IRow row = cursor.NextRow();

                while (row != null)
                {
                    CompileRow(ref row);
                    cursor.UpdateRow(row);

                    row = cursor.NextRow();
                }

                Marshal.ReleaseComObject(cursor);

                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                return true;
            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; return false; };
        }

        public bool UpdateVisibilityDB(ITable table)
        {
            try
            {
                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(table as FeatureClass).Workspace;
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                //ComReleaser comReleaser = new ComReleaser();
                IQueryFilter qry = new QueryFilterClass();
                qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
                ICursor cursor = table.Update(qry, true);
                //comReleaser.ManageLifetime(cursor);

                IRow row = cursor.NextRow();

                while (row != null)
                {
                    CompileVisibilityRow(ref row);
                    cursor.UpdateRow(row);

                    row = cursor.NextRow();
                }

                Marshal.ReleaseComObject(cursor);

                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                return true;
            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; return false;}
           
        }


        public void CompileVisibilityRow(ref IRow row)
        {
            int findx = -1;
            findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.CreatedAutomatically);
        }

        public virtual void CompileRow(ref IRow row)
        {
        }

        public virtual void RebuildGeo()
        {
            //AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            //this.Geo = ArnUtil.GetGeometry(this.ID, this.PDM_Type.ToString(), ArenaStatic.ArenaStaticProc.GetTargetDB());

            if(this.Geo !=null) return; 

            if (this.Lat.Trim().Length <= 0) return;
            if (this.Lon.Trim().Length <= 0) return;

            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();


            if (!this.X.HasValue || !this.Y.HasValue)
            {
                if (this.Elev.HasValue)
                    this.Geo = ArnUtil.Create_ESRI_POINT(this.Lat, this.Lon, this.Elev.ToString(), this.Elev_UOM.ToString());
                else
                    this.Geo = ArnUtil.Create_ESRI_POINT(this.Lat, this.Lon, "0", this.Elev_UOM.ToString());

            }
            else
            {

                double elevM = this.Elev.HasValue ? this.ConvertValueToMeter(this.Elev.Value, this.Elev_UOM.ToString()) : this.ConvertValueToMeter(0, this.Elev_UOM.ToString());
                this.Geo = ArnUtil.Create_ESRI_POINT2((double)this.Y, (double)this.X, elevM, "M");
            }

        }

        public virtual int GetIDfromDB()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            
            return  ArnUtil.GetIDfromDB(this.ID, this.PDM_Type.ToString(), ArenaStatic.ArenaStaticProc.GetTargetDB());
        }

        public virtual string GetObjectLabel()
        {
            return this.PDM_Type + " " + this.ID;
        }

        public virtual void DeleteAll(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];


            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "OBJECTID > 0";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            
        }

        public virtual List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
            List<string> res = new List<string>();

            this.CreatedAutomatically = Visibility;
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return null;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];

            this.UpdateDB(tbl);

            res.Add(this.ID);
            return res;


        }

        public virtual List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = new List<string>();
            res.Add(this.ID);
            return res;


        }

        public virtual double? ConvertValueToMeter(double? _VALUE, string _UOM)
        {
            if (!_VALUE.HasValue) return 0;

            double Mvalue;

            _UOM = _UOM.ToUpper();

            try
            {
                if (_VALUE == Double.NaN) Mvalue = Double.NaN;
                else
                {
                    switch (_UOM)
                    {
                        case "M":
                            Mvalue = _VALUE.Value * 1;
                            break;
                        case "KM":
                            Mvalue = _VALUE.Value * 1000;
                            break;
                        case "FT":
                            Mvalue = _VALUE.Value * 0.3048;
                            break;
                        case "NM":
                            Mvalue = _VALUE.Value * 1852;
                            break;
                        case "FL":
                            Mvalue = (_VALUE.Value * 30.48);
                            break;
                        default:
                            Mvalue = 0;
                            break;
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; Mvalue = 0; } 

            return Mvalue;
        }

        public virtual double ConvertValueToMeter(double _VALUE, string _UOM)
        {

            double Mvalue;

            _UOM = _UOM.ToUpper();

            try
            {
                if (_VALUE == Double.NaN) Mvalue = Double.NaN;
                else
                {
                    switch (_UOM)
                    {
                        case "M":
                            Mvalue = _VALUE * 1;
                            break;
                        case "KM":
                            Mvalue = _VALUE * 1000;
                            break;
                        case "FT":
                            Mvalue = _VALUE * 0.3048;
                            break;
                        case "NM":
                            Mvalue = _VALUE * 1852;
                            break;
                        case "FL":
                            Mvalue = (_VALUE * 30.48);
                            break;
                        default:
                            Mvalue = 0;
                            break;
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; Mvalue = 0; }

            return Mvalue;
        }

        public virtual double? ConvertValueToFeet(double? _VALUE, string _UOM)
        {
            if (!_VALUE.HasValue) return 0;

            double Mvalue;

            _UOM = _UOM.ToUpper();

            try
            {
                if (_VALUE == Double.NaN) Mvalue = Double.NaN;
                else
                {
                    switch (_UOM)
                    {
                        case "FT":
                            Mvalue = _VALUE.Value * 1;
                            break;
                        case "KM":
                            Mvalue = _VALUE.Value * 3280.83;
                            break;
                        case "M":
                            Mvalue = _VALUE.Value * 3.28083;
                            break;
                        case "NM":
                            Mvalue = _VALUE.Value * 6076.131;
                            break;
                        case "FL":
                            Mvalue = (_VALUE.Value * 100);
                            break;
                        default:
                            Mvalue = 0;
                            break;
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; Mvalue = 0; }

            return Mvalue;
        }

        public virtual double ConvertValueToFeet(double _VALUE, string _UOM)
        {
         
            double Mvalue;

            _UOM = _UOM.ToUpper();

            try
            {
                if (_VALUE == Double.NaN) Mvalue = Double.NaN;
                else
                {
                    switch (_UOM)
                    {
                        case "FT":
                            Mvalue = _VALUE * 1;
                            break;
                        case "KM":
                            Mvalue = _VALUE * 3280.83;
                            break;
                        case "M":
                            Mvalue = _VALUE * 3.28083;
                            break;
                        case "NM":
                            Mvalue = _VALUE * 6076.131;
                            break;
                        case "FL":
                            Mvalue = (_VALUE * 100);
                            break;
                        default:
                            Mvalue = 0;
                            break;
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; Mvalue = 0; }

            return Mvalue;
        }

        public virtual double ConvertSpeedToKilometresPerHour(double _VALUE, string _UOM)
        {
            double Svalue = 0;

            _UOM = _UOM.ToUpper();

            try
            {
                if (_VALUE == Double.NaN) Svalue = Double.NaN;
                else
                {
                    switch (_UOM)
                    {
                        case "KM/H":
                            Svalue = _VALUE * 1;
                            break;
                        case "KT":
                            Svalue = _VALUE * 1.852;
                            break;
                        case "MACH":
                            Svalue = _VALUE * 1193.256;
                            break;
                        case "M/MIN":
                            Svalue = _VALUE * 0.06;
                            break;
                        case "FT/MIN":
                            Svalue = _VALUE * 0.018288;
                            break;
                        case "M/SEC":
                            Svalue = _VALUE * 3.6;
                            break;
                        case "FT/SEC":
                            Svalue = _VALUE * 1.09728;
                            break;
                        default:
                            Svalue = 0;
                            break;
                    }
                }

            }
              catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; Svalue =0;} 
            
            return Svalue;
        }

        public virtual double ConvertSpeedToKNOT(double _VALUE, string _UOM)
        {
            double Svalue = 0;

            _UOM = _UOM.ToUpper();

            try
            {
                if (_VALUE == Double.NaN) Svalue = Double.NaN;
                else
                {
                    switch (_UOM)
                    {
                        case "KM/H":
                            Svalue = _VALUE * 0.5399568;
                            break;
                        case "KT":
                            Svalue = _VALUE * 1;
                            break;
                        case "MACH":
                            Svalue = _VALUE * 644.3067;
                            break;
                        case "M/MIN":
                            Svalue = _VALUE * 0.03239741;
                            break;
                        case "FT/MIN":
                            Svalue = _VALUE * 0.00987473;
                            break;
                        case "M/SEC":
                            Svalue = _VALUE * 1.943844;
                            break;
                        case "FT/SEC":
                            Svalue = _VALUE * 0.5924838;
                            break;
                        default:
                            Svalue = 0;
                            break;
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; Svalue = 0; }

            return Svalue;
        }

        public virtual string X_to_DDMMSS_EW_()
        {

            string res = "";

            try
            {
                if (this.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return res;
                if (this.Geo.IsEmpty) return res;

                double Coord = ((IPoint)this.Geo).X;
                this.X = ((IPoint)this.Geo).X;

                string sign = "E";
                if (Coord < 0)
                {
                    sign = "W";
                    Coord = Math.Abs(Coord);
                }

                double X = Math.Round(Coord, 10);

                int deg = (int)X;
                double delta = Math.Round((X - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 0);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? "0" + degSTR : "0";
                degSTR = deg < 100 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lon = degSTR + minSTR + secSTR + sign;

                res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = "";}

            return res;

        }

        public virtual string Y_to_DDMMSS_NS()
        {
           
            string res = "";

            try
            {
                if (this.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return res;
                if (this.Geo.IsEmpty) return res;

                double Coord = ((IPoint)this.Geo).Y;
                this.Y = ((IPoint)this.Geo).Y;

                string sign = "N";
                if (Coord < 0)
                {
                    sign = "S";
                    Coord = Math.Abs(Coord);
                }

                double Y = Math.Round(Coord, 10);
                //X = RealMode(X, 360);

                int deg = (int)Y;
                double delta = Math.Round((Y - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 2);



                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lat = degSTR + minSTR +secSTR +sign;

                res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;
            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = "";}

            return res;

        }

        public virtual string X_to_EW_DDMMSS()
        {
            string res = "";

            try
            {
                if (this.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return res;
                if (this.Geo.IsEmpty) return res;

                double Coord = ((IPoint)this.Geo).X;
                this.X = ((IPoint)this.Geo).X;

                string sign = "E";
                if (Coord < 0)
                {
                    sign = "W";
                    Coord = Math.Abs(Coord);
                }

                double X = Math.Round(Coord, 10);

                int deg = (int)X;
                double delta = Math.Round((X - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 0);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? "0" + degSTR : "0";
                degSTR = deg < 100 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lon = degSTR + minSTR + secSTR + sign;

                res = sign + degSTR + minSTR + secSTR;

                //res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = "";}
            return res;
        }

        public virtual string Y_to_NS_DDMMSS()
        {

            string res = "";

            try
            {
                if (this.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return res;
                if (this.Geo.IsEmpty) return res;

                double Coord = ((IPoint)this.Geo).Y;
                this.Y = ((IPoint)this.Geo).Y;

                string sign = "N";
                if (Coord < 0)
                {
                    sign = "S";
                    Coord = Math.Abs(Coord);
                }

                double Y = Math.Round(Coord, 10);
                //X = RealMode(X, 360);

                int deg = (int)Y;
                double delta = Math.Round((Y - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 0);



                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lat = degSTR + minSTR + secSTR + sign;

                res = sign + degSTR + minSTR + secSTR;
            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = "";}

            return res;

        }

        public virtual void StoreNotes(ITable NotesTable)
        {
            if (this.Notes == null) return;

            foreach (var item in this.Notes)
            {
                IRow row = NotesTable.CreateRow();

                int findx = -1;

                findx = row.Fields.FindField("MasterFeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
                findx = row.Fields.FindField("MasterFeatureType"); if (findx >= 0) row.set_Value(findx, this.GetType().Name);
                findx = row.Fields.FindField("Note"); if (findx >= 0) row.set_Value(findx, item);

                row.Store();
            }

            

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public virtual object Clone(bool sameID)
        {

            object pdmClone =this.MemberwiseClone();
            ((PDMObject)pdmClone).ActualDate = DateTime.Now;
            ((PDMObject)pdmClone).ID = sameID ? this.ID : Guid.NewGuid().ToString();
            ((PDMObject)pdmClone).CreatedAutomatically = false;
            return pdmClone;
        }

        public virtual object Clone()
        {

            object pdmClone = this.MemberwiseClone();
            ((PDMObject)pdmClone).ActualDate = DateTime.Now;
            ((PDMObject)pdmClone).ID = Guid.NewGuid().ToString();
            ((PDMObject)pdmClone).CreatedAutomatically = false;
            return pdmClone;
        }

        public virtual bool CompareId(string AnotherID)
        {
            //return this.ID.CompareTo(AnotherID) == 0;
            return this.ID.StartsWith(AnotherID);

        }


        public virtual List<string> GetIdList()
        {
            List<string> res = new List<string>();
            res.Add(this.ID);

            return res;
        }

    }
}
