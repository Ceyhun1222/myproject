using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Common;
using Holding.Forms;
using Aran.PANDA.Rnav.Holding.Properties;
using Holding.save;

namespace Holding.Models
{
    public class Bussines_Logic
    {
        #region ::>Fields
        private double doc;
        private FlightCondition _flightCondition;
        private  DmeCoverageOperation dmeConverageOperation;
        private SpatialReferenceOperation _spatialOperation;
        private int currentIndex = -1;
        private flightReciever _recieverType;
        private BaseArea _baseArea;
        private frmHoldingReport frmHReport;
        private DmeCoverageOperation _dmeOperation;
        

        private int dmeCovGeoHandle;
        private Aran.Geometries.MultiPolygon _dmeCoverageGeom;

        #endregion

        #region ::> Constructor
        public Bussines_Logic(List<System.Windows.Forms.Control> containerList, double distance, double altitude, double radial,
                 double ias, TurnDirection side, ConditionType condType)
        {
            _spatialOperation = GlobalParams.SpatialRefOperation;
            dmeConverageOperation = new DmeCoverageOperation();
            _baseArea = new BaseArea();
            HoldingGeom = new HoldingGeometry();

            
            _flightCondition = new FlightCondition();

            ModelPBN = new ModelPBN();
            ModelPBN.CurFlightPhaseChanged += OnMaxDistance_Changed;
            ModelPBN.CurPbnChanged += OnATTXTT_Change;
            ModelPBN.CurRecieverChanged += OnNavListCan_Change;

            ModelPtChoise = new ModelPointChoise(distance, ModelPBN.MinDistance, ModelPBN.MaxDistance);
            ModelPtChoise.CurPointChanged += OnCurPoint_Changed;
            ModelPtChoise.ModelChangedEventHandler += On_ModelChangedEventHandler;

            ModelAreamParam = new ModelAreaParams(altitude, radial, ias, categories.CD, side, 300, condType,flightPhase.Enroute);

            WizardChange = new ModelWizardChange(containerList);
            WizardChange.CurrentPointChange(ModelPtChoise.CurPoint);
            WizardChange.PointChoice = ModelPtChoise;

            ProcedureType = new ModelProcedureType(1, 1.5,1);
            ProcedureType.ModelChangedEventHandler += On_ModelChangedEventHandler;
            
            ModelAreamParam.SpecialParamChanged += OnATTXTT_Change;
            ModelAreamParam.AltitudeChanged += OnNavListCan_Change;
            ModelAreamParam.ModelChangedEventHandler += On_ModelChangedEventHandler;
            ModelAreamParam.MocChanged += new EventHandler(ModelAreamParam_MocChanged);

            OnATTXTT_Change(this, new EventArgs());
            
            HoldingNavaidOperation = new HoldingNavOperation();
            HoldingNavaidOperation.DmeCoveTypeCheckChanged += OnATTXTT_Change;
            HoldingNavaidOperation.DmeCoverageChanged += OnDMECoverageDrawChanged;

            _dmeOperation = new DmeCoverageOperation();

            Validation = new ValidationClass();
            frmHReport = new frmHoldingReport();
           
        }

        void ModelAreamParam_MocChanged(object sender, EventArgs e)
        {
            if (!frmHReport.Closed)
                CreateReport();
        }

        private void On_ModelChangedEventHandler(object sender, ModelChangedEventArgs modelChangedEventArgs)
        {
            ModelPtChoise.ChangedCount += modelChangedEventArgs.Changed?1:-1;
        }

        #endregion
     
        #region ::> Properties
        public List<Aran.Aim.Features.Navaid> NavList { get; set; }
        public ModelAreaParams ModelAreamParam { get; set; }
        public ModelPointChoise ModelPtChoise { get; set; }
        public ModelPBN ModelPBN { get; set; }
        public ModelProcedureType ProcedureType { get; set; }
        public ModelWizardChange WizardChange { get; set; }
        public HoldingNavOperation HoldingNavaidOperation { get; set; }
        public ValidationClass Validation { get; set; }
        public HoldingGeometry HoldingGeom { get; set; }
        public HoldingReport HReport { get; set; }
        #endregion
      
        #region ::>Methods
        public void OnListChange(object sender, ListChangedEventArgs e)
        {
            bool newNavIsChecked = !HoldingNavaidOperation.HoldingNavList[e.NewIndex].Checked;
            if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.DMEDME])
            {               
                if (!newNavIsChecked)
                {
                    if (HoldingNavaidOperation.DmeCovType == DmeCoverageType.threeDme && HoldingNavaidOperation.CheckedNavCount <= 3)
                        return;
                    else if (HoldingNavaidOperation.DmeCovType == DmeCoverageType.twoDme && HoldingNavaidOperation.CheckedNavCount <= 2)
                        return;
                }
                
                HoldingNavaidOperation.HoldingNavList[e.NewIndex].Checked = newNavIsChecked;
                HoldingNavaidOperation.CheckedNavCount += newNavIsChecked?1:-1;
                
                currentIndex = e.NewIndex;
                OnDMECoverageDrawChanged(this, new DMECoverageDrawChangedEventArgs(HoldingNavaidOperation.DrawDME));
            }
            else if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.VORDME])
            {
                if (currentIndex == e.NewIndex)
                    return;
               
                HoldingNavaidOperation.HoldingNavList[e.NewIndex].Checked = !HoldingNavaidOperation.HoldingNavList[e.NewIndex].Checked;
                if (currentIndex != -1 && currentIndex != e.NewIndex)
                    HoldingNavaidOperation.HoldingNavList[currentIndex].Checked = false;
                currentIndex = e.NewIndex;
                HoldingNavaidOperation.CurCheckedNavIndex = currentIndex;
                
            }
            OnATTXTT_Change(null, null);
        }

        public void OnATTXTT_Change(object sender, EventArgs arg)
        {
            double tMin;
            double att = 0, xtt = 0;
            
            if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.GNSS])
            {
                if (ModelPBN.CurPBN != null)
                {
                    GNSSValues gnssValues = _flightCondition.GetGNSSValue(ModelPBN.CurPBN);
                    att = gnssValues.ATT;
                    xtt = gnssValues.XTT;
                }
                else
                {
                    att = 0;
                    xtt = 0;
                }
            }
            else if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.DMEDME])
            {
                if (!DmeCanCalculate())
                {
                    ModelPtChoise.SetATTXTTValue(0, 0);
                    ProcedureType.MinWD = 0;
                    return;
                }
                att = dmeConverageOperation.ATT(HoldingNavaidOperation.DmeCovType, ModelAreamParam.DOC, ModelPBN.CurPBN.PBNName);
                xtt = dmeConverageOperation.XTT(HoldingNavaidOperation.DmeCovType, ModelAreamParam.DOC, ModelPBN.CurPBN.PBNName);
            }
            else if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.VORDME])
            {
                if (HoldingNavaidOperation.HoldingNavList != null &&HoldingNavaidOperation.HoldingNavList.Count>0 &&
                    HoldingNavaidOperation.CheckedNavCount > 0)
                {
                    Aran.Geometries.Point ptPrj = _spatialOperation.ToPrj(ModelPtChoise.CurPoint);
                    VORDMECoverage vorDmeCoverage = new VORDMECoverage(HoldingNavaidOperation.CheckedNavaidList[0].GetNavaid, ptPrj, ModelPBN.CurPBN.PBNName, ARANMath.DegToRad(ModelAreamParam.Radial));
                    att = vorDmeCoverage.ATT;
                    xtt = vorDmeCoverage.XTT;
                }
                else
                {                                      
                    ModelPtChoise.SetATTXTTValue(0, 0);
                    ProcedureType.MinWD = 0;
                    return;
                }
            }

            ProcedureType.MinWD = Common.ConvertDistance(Shablons.CalculateWd(1.0, Common.DeConvertSpeed(ModelAreamParam.Ias), 15, Common.DeConvertHeight(ModelAreamParam.Altitude),
                                             att, xtt, out tMin), roundType.toNearest);
            ProcedureType.MaxWD = Common.ConvertDistance(Shablons.CalculateWd(2.0, Common.DeConvertSpeed(ModelAreamParam.Ias), 15, Common.DeConvertHeight(ModelAreamParam.Altitude),
                                             att, xtt, out tMin), roundType.toNearest);
            ProcedureType.WDCanActive = true;
            ModelPtChoise.SetATTXTTValue(att, xtt);
        
        }

        public void CreateDmeCoverage(bool draw=true)
        {
            
            GlobalParams.UI.SafeDeleteGraphic(dmeCovGeoHandle);
            
            List<Navaid> checkedNavList = HoldingNavaidOperation.CheckedNavaidList.Select(hNavaid=>hNavaid.GetNavaid).ToList<Navaid>();
            _dmeCoverageGeom = new Aran.Geometries.MultiPolygon();

            _dmeOperation.CreateDMECoverage(checkedNavList, HoldingNavaidOperation.DmeCovType, ModelPtChoise.CurPoint, ModelAreamParam.DOC, ref _dmeCoverageGeom);

            if (_dmeCoverageGeom!=null && _dmeCoverageGeom.Count>0 && draw)
                dmeCovGeoHandle =GlobalParams.UI.DrawMultiPolygon(_dmeCoverageGeom, eFillStyle.sfsVertical, 1);
        }

        public void CalculateBaseArea()
        {
            Aran.Geometries.Point ptGeo =  ModelPtChoise.CurPoint;
            double direction = _spatialOperation.AztToDirGeo(ptGeo,ModelAreamParam.Radial);
            _baseArea.CreateBaseArea( ProcedureType.PropType,  ProcedureType.CurDistanceType, _spatialOperation.ToPrj( ModelPtChoise.CurPoint),
                                Common.DeConvertSpeed(ModelAreamParam.Ias),Common.DeConvertHeight(ModelAreamParam.Altitude), direction,  ProcedureType.Time,
                                ModelAreamParam.Turn,  ModelPtChoise.ATT,  ModelPtChoise.XTT, Common.DeConvertDistance( ProcedureType.WD),  
                                ProcedureType.ProtectionType, HoldingGeom);

            HoldingGeom.Draw();
            
            ModelAreamParam.SetApplyParams();
            ModelPtChoise.SetApplyParams();
            ProcedureType.SetApplyParams();
            ModelPtChoise.IsApply = true;
            ModelPtChoise.ChangedCount = 0;
            _dmeCoverageGeom = null;

            if (HReport != null)
                    HReport.LastReport = false;

            if (!frmHReport.Closed)
                CreateReport();            
        }

        public void CreateReport()
        {
            if (HReport == null)
                HReport = new HoldingReport(HoldingGeom, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
            else
            {
                if (!HReport.LastReport)
                    HReport.CreateReport(HoldingGeom, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
                else
                    if (Math.Abs(HReport.Moc - ModelAreamParam.CurMoc) > 1)
                    {
                        HReport.SetHReport(Common.DeConvertHeight(ModelAreamParam.CurMoc));
                    }
            }
            HReport.LastReport = true;

            
            if (frmHReport.Closed)
            {
                frmHReport = new frmHoldingReport(HReport);
                frmHReport.Show(InitHolding.Win32Window);
            }
            else
                frmHReport.UptadeReport(HReport);
            
            bool dmeCoverageIsEnabled=true;
            if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.DMEDME])
            {
                if (_dmeCoverageGeom==null || _dmeCoverageGeom.IsEmpty)
                    CreateDmeCoverage(false);

                var geom = GlobalParams.GeomOperators.Intersect(_dmeCoverageGeom, HoldingGeom.FullArea) as Aran.Geometries.MultiPolygon;
                if (geom != null && !geom.IsEmpty && Math.Abs(geom.Area - HoldingGeom.FullArea.Area) > 10)
                {
                    dmeCoverageIsEnabled = false;
                    MessageBox.Show(Resources.dme_doesnt_cover, Resources.Holding_Caption,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (!(HReport.ObstacleReport.All(rep => rep.Validation) || HReport.ReportCount == 0))
                MessageBox.Show(Resources.no_obstacle_penetrating_holding_area, Resources.Holding_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            ModelPtChoise.SaveIsEnabled = (HReport.ObstacleReport.All(rep => rep.Validation) || HReport.ReportCount == 0)
                    && (ModelAreamParam.Altitude >= ModelAreamParam.CurMoc) && dmeCoverageIsEnabled;
                
        }

        public void CreateSave(IScreenCapture screenCapture)
        {
            if (HReport == null)
                HReport = new HoldingReport(HoldingGeom, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
            else
            {
                if (!HReport.LastReport)
                {
                    HReport.CreateReport(HoldingGeom, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
                    HReport.LastReport = true;
                }
            }
            var holdingSaveMain = new HoldingSaveMain(this);
            HoldingPattern pattern;
            byte[] repoBytes;
            try
            {
                pattern = holdingSaveMain.Save();
                repoBytes = holdingSaveMain.CreateHtmlReport();
            }
            catch (Exception)
            {
                screenCapture.Rollback();
                throw;
            }
            SaveScreenshotToDb(screenCapture, pattern);
            SaveReportToDb(pattern, repoBytes);
        }

        private void SaveReportToDb(HoldingPattern pattern, byte[] repoBytes)
        {
            if (repoBytes != null)
            {
                FeatureReport report = new FeatureReport();
                report.Identifier = pattern.Identifier;
                report.ReportType = FeatureReportType.Obstacle;
                report.HtmlZipped = repoBytes;
                GlobalParams.Database.HoldingQpi.SetFeatureReport(report);
            }
        }

        private void SaveScreenshotToDb(IScreenCapture screenCapture, HoldingPattern pattern)
        {
            Screenshot screenshot = new Screenshot();
            screenshot.DateTime = DateTime.Now;
            screenshot.Identifier = pattern.Identifier;
            screenshot.Images = screenCapture.Commit(pattern.Identifier);
            GlobalParams.Database.HoldingQpi.SetScreenshot(screenshot);
        }

        public void Dispose()
        {
            HoldingGeom.ClearGraph();
            HoldingGeom.Clear();
            ModelPtChoise.Dispose();
            if (HoldingNavaidOperation != null)
                HoldingNavaidOperation.Dispose();
            GlobalParams.UI.SafeDeleteGraphic(dmeCovGeoHandle);
        }
            
        private void OnMaxDistance_Changed(object sender, EventArgs arg)
        {
            ModelPtChoise.SetMinMaxDistance(ModelPBN.MinDistance, ModelPBN.MaxDistance);
            ModelAreamParam.FlightPhaseChanged(ModelPBN.CurFlightPhase);
        }

        private void OnCurPoint_Changed(object sender, EventArgs arg)
        {
            if (WizardChange.CurrentPointChange(ModelPtChoise.CurPoint))
                WizardChange.OnPropertyChanged("ModelPointChoise", ModelPtChoise.CurPoint);
            OnNavListCan_Change(this, null);
            HoldingNavaidOperation.DrawDME = false;

        }

        private void OnNavListCan_Change(object sender, EventArgs arg)
        {
            #region isSame
            Func<List<Navaid>, List<Navaid>, bool> IsSame = (navList1, navList2) =>
            {
                if (navList1 == null && navList2 == null)
                    return true;
                else if ((navList1==null || navList2 ==null) || (navList1.Count != navList2.Count))
                    return false;
                else
                {
                    for (int i = 0; i < navList1.Count; i++)
                    {
                        if (navList1[i] != navList2[i])
                            return false;
                    }
                }
                return true;

            };
            #endregion
            
            List<Navaid> tmpNavList = NavList;

            #region :>VorDme
            if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.VORDME] && ModelPtChoise.CurPoint != null)
            {
                tmpNavList = FeatureConvert.GetVorDmeList(ModelPtChoise.CurPoint, ModelAreamParam.DOC);
                _recieverType = flightReciever.VORDME;

                if (tmpNavList == null || tmpNavList.Count == 0)
                {
                    HoldingNavaidOperation.Dispose();
                    ModelPtChoise.HoldingAreaIsEnabled = false;
                    ModelPtChoise.SetATTXTTValue(0, 0);
                    return;
                }

                ConverToUserNavList(tmpNavList, _recieverType);
                NavList = tmpNavList;
                if (HoldingNavaidOperation.CheckedNavCount > 0)
                    ModelPtChoise.HoldingAreaIsEnabled = true;

                HoldingNavaidOperation.DmeCoverageIsEnabled = false;
                HoldingNavaidOperation.DmeCoverageChooseIsEnabled = false;
                HoldingNavaidOperation.DrawDME = false;
            }
            #endregion
        
            #region :>DmeDme
            else if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.DMEDME] && ModelPtChoise.CurPoint != null)
            {
                _recieverType = flightReciever.DMEDME;
                HoldingNavaidOperation.DmeCoverageChooseIsEnabled = true;

                HoldingNavaidOperation.CheckedNavCount = 0;

                tmpNavList = FeatureConvert.GetDmeList(ModelPtChoise.CurPoint, ModelAreamParam.DOC, ModelAreamParam.DOCMin);

                if (tmpNavList == null || tmpNavList.Count == 0)
                {
                    HoldingNavaidOperation.Dispose();
                    ModelPtChoise.SetATTXTTValue(0, 0);
                    HoldingNavaidOperation.DmeCoverageIsEnabled = false;
                    ModelPtChoise.HoldingAreaIsEnabled = false;
                    return;
                }
                ConverToUserNavList(tmpNavList, _recieverType);
                bool isEnabled = false;
                if (HoldingNavaidOperation.CheckedNavCount > 2)
                    isEnabled = true;
                else if (HoldingNavaidOperation.CheckedNavCount == 2 && HoldingNavaidOperation.DmeCovType == DmeCoverageType.twoDme)
                    isEnabled = true;
                HoldingNavaidOperation.DmeCoverageIsEnabled = isEnabled;
                ModelPtChoise.HoldingAreaIsEnabled = isEnabled;
                OnDMECoverageDrawChanged(this, new DMECoverageDrawChangedEventArgs(HoldingNavaidOperation.DrawDME));
            }
            #endregion

            else if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.GNSS] && ModelPtChoise.CurPoint != null)
            {
                HoldingNavaidOperation.Dispose();
                HoldingNavaidOperation.DmeCoverageIsEnabled = false;
                HoldingNavaidOperation.DmeCoverageChooseIsEnabled = false;
                HoldingNavaidOperation.DrawDME = false;
                ModelPtChoise.HoldingAreaIsEnabled = true;

            }
            OnATTXTT_Change(this, new EventArgs());
            doc = ModelAreamParam.DOC;
            NavList = tmpNavList;
        }

        private void OnDMECoverageDrawChanged(object sender, DMECoverageDrawChangedEventArgs drawChanged)
        {
            try
            {
                if (drawChanged.Draw)
                {
                    CreateDmeCoverage();
                }
                else
                {
                    GlobalParams.UI.SafeDeleteGraphic(dmeCovGeoHandle);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Resources.Holding_Caption);
            }
        }
            
        private void ConverToUserNavList(List<Navaid> Navlist, flightReciever recieverType)
        {
            var holdingNavList = new BindingList<HoldingNavaid>();
           
            HoldingNavaidOperation.CheckedNavCount = 0;
            
            foreach (Navaid nav in Navlist)
            {
                double dirAzimut,inverseAzimut;
                ARANFunctions.ReturnGeodesicAzimuth(GeomFunctions.Assign(nav.Location),ModelPtChoise.CurPoint,out dirAzimut,out inverseAzimut);

                double distance = ARANMath.Hypot(NativeMethods.ReturnGeodesicDistance
                    (nav.Location.Geo.X, nav.Location.Geo.Y, ModelPtChoise.CurPoint.X, ModelPtChoise.CurPoint.Y),Common.DeConvertHeight(ModelAreamParam.Altitude));
                holdingNavList.Add(new HoldingNavaid(nav,false,distance,dirAzimut));
            }

            if (recieverType == flightReciever.DMEDME)
            {
                var tmpList = new BindingList<HoldingNavaid>();
                foreach (HoldingNavaid hNav in holdingNavList)
                {
                    foreach (HoldingNavaid oldNav in HoldingNavaidOperation.HoldingNavList)
                    {
                        if (hNav.Designator == oldNav.Designator && oldNav.Checked)
                        {
                            hNav.Checked = true;
                            HoldingNavaidOperation.CheckedNavCount++;
                            break;
                        }
                    }
                }

                #region Check Action
                Action<int> check = (count) =>
                    {
                        foreach (HoldingNavaid hNav in holdingNavList)
                        {
                            if (!hNav.Checked)
                            {
                                hNav.Checked = true;
                                if (++HoldingNavaidOperation.CheckedNavCount >= count)
                                    break;
                            }
                        }
                    };
                #endregion

                if (HoldingNavaidOperation.CheckedNavCount < 2 && HoldingNavaidOperation.DmeCovType == DmeCoverageType.twoDme)
                    check(2);
                else if (HoldingNavaidOperation.CheckedNavCount < 3 && HoldingNavaidOperation.DmeCovType == DmeCoverageType.threeDme)
                    check(3);
            }
            else if (recieverType == flightReciever.VORDME)
            {
                foreach (HoldingNavaid hNav in holdingNavList)
                {
                    bool flag = false;
                    int i = -1;
                    foreach (HoldingNavaid oldNav in HoldingNavaidOperation.HoldingNavList)
                    {
                        i++;
                        if (hNav.Designator == oldNav.Designator && oldNav.Checked)
                        {
                            hNav.Checked = true;
                            flag = true;
                            HoldingNavaidOperation.CheckedNavCount++;
                            currentIndex = i; 
                            break;
                        }

                    }
                    if (flag)
                        break;
                }
                if (HoldingNavaidOperation.CheckedNavCount == 0 && holdingNavList.Count>0)
                {
                    holdingNavList[0].Checked = true;
                    HoldingNavaidOperation.CheckedNavCount++;
                    currentIndex = 0;  
                }

                if (HoldingNavaidOperation.CheckedNavCount > 0)
                    ModelPtChoise.HoldingAreaIsEnabled = true;
                else
                    ModelPtChoise.HoldingAreaIsEnabled = false;
            }

            HoldingNavaidOperation.Dispose();
            foreach (var item in holdingNavList)
            {
                HoldingNavaidOperation.HoldingNavList.Add(item);
            }
           
        }

        private bool DmeCanCalculate()
        {
            bool result = false;
            if (HoldingNavaidOperation.DmeCovType == DmeCoverageType.twoDme && HoldingNavaidOperation.CheckedNavCount > 1)
                result = true;
            else if (HoldingNavaidOperation.DmeCovType == DmeCoverageType.threeDme && HoldingNavaidOperation.CheckedNavCount > 2)
                result = true;
            return result;
        }
       
    }

        #endregion
}
