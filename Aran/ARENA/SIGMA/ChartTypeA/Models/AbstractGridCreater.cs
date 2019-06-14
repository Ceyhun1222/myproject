using Aran.Package;
using ChartTypeA.ViewModels;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChartTypeA.Models
{
    public abstract class AbstractGridCreater:IPackable
    {
        public abstract void ReCreate();
        public abstract void Clear();

        public void Pack(PackageWriter writer)
        {
            try
            {

                writer.PutDouble(LengthRwy);

                writer.PutBool(Pnt1 != null);
                if (Pnt1 != null)
                {
                    writer.PutDouble(Pnt1.X);
                    writer.PutDouble(Pnt1.Y);
                }

                writer.PutBool(Pnt2 != null);
                if (Pnt2 != null)
                {
                    writer.PutDouble(Pnt2.X);
                    writer.PutDouble(Pnt2.Y);
                }

                writer.PutDouble(VerScale);
                writer.PutDouble(HorScale);

                writer.PutInt32(RowCount1);
                writer.PutInt32(RowCount2);

                writer.PutInt32(ColumnCount1);
                writer.PutInt32(ColumnCount2);

                writer.PutInt32(TickCount);
                writer.PutDouble(TickLength);

                writer.PutDouble(HorizontalStep);
                writer.PutDouble(VerticalStep);

                writer.PutDouble(BaseElevation);

                writer.PutDouble(Slope);
                writer.PutDouble(FrameHeight);

                writer.PutDouble(YMin);

                writer.PutDouble(ClearWay1);
                writer.PutDouble(ClearWay2);

                writer.PutDouble(OffSet);
                writer.PutInt32((int) Side);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        public void Unpack(PackageReader reader)
        {
            try
            {
                LengthRwy = reader.GetDouble();

                var hasPnt1 = reader.GetBool();
                if (hasPnt1)
                {
                    Pnt1 = new PointClass();
                    Pnt1.X = reader.GetDouble();
                    Pnt1.Y = reader.GetDouble();
                }

                var hasPnt2 = reader.GetBool();
                if (hasPnt2)
                {
                    Pnt2 = new PointClass();
                    Pnt2.X = reader.GetDouble();
                    Pnt2.Y = reader.GetDouble();
                }
                VerScale = reader.GetDouble();
                HorScale = reader.GetDouble();

                RowCount1 = reader.GetInt32();
                RowCount2 = reader.GetInt32();

                ColumnCount1 = reader.GetInt32();
                ColumnCount2 = reader.GetInt32();

                TickCount = reader.GetInt32();
                TickLength = reader.GetDouble();

                HorizontalStep = reader.GetDouble();
                VerticalStep = reader.GetDouble();

                BaseElevation = reader.GetDouble();

                Slope = reader.GetDouble();
                FrameHeight = reader.GetDouble();

                YMin = reader.GetDouble();

                ClearWay1 = reader.GetDouble();
                ClearWay2 = reader.GetDouble();

                OffSet = reader.GetDouble();
                Side = (TakeoffSide) reader.GetInt32();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        public double LengthRwy
        {
            get;
            set;
        }

        public IPoint Pnt1
        {
            get;
            set;
        }

        public IPoint Pnt2
        {
            get;
            set;
        }

        public double VerScale
        {
            get;
            set;
        }

        public double HorScale
        {
            get;
            set;
        }

        public int RowCount1
        {
            get;
            set;
        }

        public int RowCount2
        {
            get;
            set;
        }

        public int ColumnCount1
        {
            get;
            set;
        }

        public int ColumnCount2
        {
            get;
            set;
        }

        public int TickCount { get; set; }=10;

        public double TickLength
        {
            get;
            set;

        }

        public double HorizontalStep
        {
            get;
            set;

        }

        public double VerticalStep
        {
            get;
            set;

        }

        public int Color
        {
            get;
            set;
        }

        public double BaseElevation
        {
            get;
            set;
        }

        public double Slope
        {
            get;
            set;
        }

        public double FrameHeight { get; set; }

        public IGroupElement AllElements { get; set; }

        public IGroupElement ObstacleElements { get; set; }

        public List<PDM.RunwayCenterLinePoint> CenterlinePoints { get; set; }

        //public List<PDM.RunwayCenterLinePoint> CenterlinePoints2
        //{
        //    get
        //    {
        //        return _cntLnPnts2;
        //    }
        //    set
        //    {
        //        _cntLnPnts2 = value;
        //        //ClearWay2 = 0;
        //        //var startRwy = _cntLnPnts2.FirstOrDefault(pdm => pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.START);
        //        //if (startRwy == null)
        //        //    return;
        //        //var toda = startRwy.DeclDist.FirstOrDefault(pdm => pdm.DistanceType == PDM.CodeDeclaredDistance.TODA);
        //        //if (toda == null)
        //        //    return;
        //        //var tora = startRwy.DeclDist.FirstOrDefault(pdm => pdm.DistanceType == PDM.CodeDeclaredDistance.TORA);
        //        //if (tora == null)
        //        //    return;
        //        //ClearWay2 = startRwy.ConvertValueToMeter(toda.DistanceValue, toda.DistanceUOM.ToString()) -
        //        //    startRwy.ConvertValueToMeter(tora.DistanceValue, tora.DistanceUOM.ToString());
        //    }
        //}

        public double ClearWay1 { get; set; }
        public double ClearWay2 { get; set; }

        public double YMin { get; set; }

        public double OffSet { get; set; }
        public TakeoffSide Side { get; set; }
        public List<PDM.RunwayCenterLinePoint> CenterlinePoints2 { get; internal set; }
    }
}
