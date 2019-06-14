using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.Common;
using System.ComponentModel;
using Delib.Classes.Features.Navaid;
using System.Collections;
using System.Windows.Forms;

namespace Holding
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

        #endregion

        #region ::> Constructor
        public Bussines_Logic(List<System.Windows.Forms.Control> containerList, double distance, double altitude, double radial,
                 double ias, SideDirection side, ConditionType condType)
        {
            _spatialOperation = GlobalParams.SpatialRefOperation;

            dmeConverageOperation = new DmeCoverageOperation();
            _baseArea = new BaseArea();
            HoldingGeom = new HoldingGeometry();

            ModelAreamParam = new ModelAreaParams(altitude, radial, ias, categories.C, side, 300, condType);
            _flightCondition = new FlightCondition();

            ModelPBN = new ModelPBN();
            ModelPBN.CurFlightPhaseChanged += OnMaxDistance_Changed;
            ModelPBN.CurPbnChanged += OnATTXTT_Change;
            ModelPBN.CurRecieverChanged += OnNavListCan_Change;

            ModelPtChoise = new ModelPointChoise(distance, ModelPBN.MinDistance, ModelPBN.MaxDistance);
            ModelPtChoise.CurPointChanged += OnCurPoint_Changed;
            ModelPtChoise.ModelChangedEventHandler += On_ModelChangedEventHandler;

            WizardChange = new ModelWizardChange(containerList);
            WizardChange.CurrentPointChange(ModelPtChoise.CurPoint);

            ProcedureType = new ModelProcedureType(0.5, 3,2);
            ProcedureType.ModelChangedEventHandler += On_ModelChangedEventHandler;
            
            ModelAreamParam.SpecialParamChanged += OnATTXTT_Change;
            ModelAreamParam.AltitudeChanged += OnNavListCan_Change;
            ModelAreamParam.ModelChangedEventHandler += On_ModelChangedEventHandler;

            OnATTXTT_Change(this, new EventArgs());
            
            HoldingNavaidOperation = new HoldingNavOperation();
            HoldingNavaidOperation.DmeCoveTypeCheckChanged += OnATTXTT_Change;
            HoldingNavaidOperation.DmeCoverageChanged += OnDMECoverageDrawChanged;

            _dmeOperation = new DmeCoverageOperation();

            Validation = new ValidationClass();
           
        }

        private void On_ModelChangedEventHandler(object sender, ModelChangedEventArgs modelChangedEventArgs)
        {
            ModelPtChoise.ChangedCount += modelChangedEventArgs.Changed?1:-1;
        }

        #endregion
     
        #region ::> Properties
        public List<Navaid> _navList { get; set; }
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
                
            }
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
                    ARAN.GeometryClasses.Point ptPrj = _spatialOperation.GeoToPrjPoint(ModelPtChoise.CurPoint);
                    VORDMECoverage vorDmeCoverage = new VORDMECoverage(_navList[HoldingNavaidOperation.CurrentCheckedNavaid], ptPrj, ModelPBN.CurPBN.PBNName, ARANFunctions.DegToRad(ModelAreamParam.Radial));
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

            ProcedureType.MinWD = Common.ConvertDistance(Shablons.CalculateWd(ProcedureType.Time, Common.DeConvertSpeed(ModelAreamParam.Ias), 15, Common.DeConvertHeight(ModelAreamParam.Altitude),
                                             att, xtt, out tMin), roundType.toNearest);
            ProcedureType.WDCanActive = true;
            ModelPtChoise.SetATTXTTValue(att, xtt);
        
        }

        public void CreateDmeCoverage()
        {
            
            GlobalParams.UI.SafeDeleteGraphic(ref dmeCovGeoHandle);
            
            List<Navaid> checkedNavList = HoldingNavaidOperation.CheckedNavaidList.Select(hNavaid=>hNavaid.GetNavaid()).ToList<Navaid>();
            ARAN.GeometryClasses.Polygon dmeCoverageGeom = new ARAN.GeometryClasses.Polygon();

            _dmeOperation.CreateDMECoverage(checkedNavList, HoldingNavaidOperation.DmeCovType, ModelPtChoise.CurPoint, ModelAreamParam.DOC, ref dmeCoverageGeom);

            if (dmeCoverageGeom!=null && dmeCoverageGeom.Count>0)
                dmeCovGeoHandle =GlobalParams.UI.DrawPolygon(dmeCoverageGeom, 1, ARAN.Contracts.UI.eFillStyle.sfsVertical);
        }

        public void CalculateBaseArea()
        {
            ARAN.GeometryClasses.Point ptGeo =  ModelPtChoise.CurPoint;
            double direction = _spatialOperation.AzToDirection(ARANFunctions.DegToRad(ModelAreamParam.Radial), ptGeo);
            _baseArea.CreateBaseArea( ProcedureType.PropType,  ProcedureType.CurDistanceType, _spatialOperation.GeoToPrjPoint( ModelPtChoise.CurPoint),
                                Common.DeConvertSpeed(ModelAreamParam.Ias),Common.DeConvertHeight(ModelAreamParam.Altitude), direction,  ProcedureType.Time,
                                ModelAreamParam.Turn,  ModelPtChoise.ATT,  ModelPtChoise.XTT, Common.DeConvertDistance( ProcedureType.WD),  
                                ProcedureType.ProtectionType, HoldingGeom);

            HoldingGeom.Draw();
            
            ModelAreamParam.SetApplyParams();
            ModelPtChoise.SetApplyParams();
            ProcedureType.SetApplyParams();
            ModelPtChoise.IsApply = true;
            ModelPtChoise.ChangedCount = 0;
            if (HReport != null)
                HReport.LastReport = false;

            
        }

        public void CreateReport()
        {
            if (HReport == null)
                HReport = new HoldingReport(HoldingGeom.FullArea, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
            else
            {
                if (!HReport.LastReport)
                    HReport.CreateReport(HoldingGeom.HoldingArea, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
                else
                    if (Math.Abs(HReport.Moc - ModelAreamParam.CurMoc) > 1)
                    {
                        HReport.SetHReport(Common.DeConvertHeight(ModelAreamParam.CurMoc));
                    }
            }
            HReport.LastReport = true;
            
            frmHReport = new frmHoldingReport(HReport);
            frmHReport.Show(InitHolding.win32Window);
        }

        public void CreateSave()
        {
            if (HReport == null)
                HReport = new HoldingReport(HoldingGeom.FullArea, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
            else
            {
                if (!HReport.LastReport)
                {
                    HReport.CreateReport(HoldingGeom.FullArea, Common.DeConvertHeight(ModelAreamParam.Altitude), Common.DeConvertHeight(ModelAreamParam.CurMoc));
                    HReport.LastReport = true;
                }
            }
            frmHoldingSave frmSave = new frmHoldingSave(this);
            frmSave.ShowDialog();
           
        }

        public void Dispose()
        {
            HoldingGeom.ClearGraph();
            HoldingGeom.Clear();
            ModelPtChoise.Dispose();
            GlobalParams.UI.SafeDeleteGraphic(ref dmeCovGeoHandle);
        }
             
        private void OnMaxDistance_Changed(object sender, EventArgs arg)
        {
            ModelPtChoise.SetMinMaxDistance(ModelPBN.MinDistance, ModelPBN.MaxDistance);
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
            
            List<Navaid> tmpNavList = _navList;

            #region :>VorDme
            if (ModelPBN.CurReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.VORDME] && ModelPtChoise.CurPoint != null)
            {
                tmpNavList = FeatureConvert.GetVorDmeList(ModelPtChoise.CurPoint, ModelAreamParam.DOC);
                _recieverType = flightReciever.VORDME;

                if (tmpNavList == null || tmpNavList.Count == 0)
                {
                    HoldingNavaidOperation.HoldingNavList.Clear();
                    ModelPtChoise.HoldingAreaIsEnabled = false;
                    ModelPtChoise.SetATTXTTValue(0, 0);
                    return;
                }

                ConverToUserNavList(tmpNavList, _recieverType);
                _navList = tmpNavList;
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
                    HoldingNavaidOperation.HoldingNavList.Clear();
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
                HoldingNavaidOperation.HoldingNavList.Clear();
                HoldingNavaidOperation.DmeCoverageIsEnabled = false;
                HoldingNavaidOperation.DmeCoverageChooseIsEnabled = false;
                HoldingNavaidOperation.DrawDME = false;
                ModelPtChoise.HoldingAreaIsEnabled = true;

            }
            OnATTXTT_Change(this, new EventArgs());
            doc = ModelAreamParam.DOC;
            _navList = tmpNavList;
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
                    GlobalParams.UI.SafeDeleteGraphic(ref dmeCovGeoHandle);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
            
        private void ConverToUserNavList(List<Navaid> Navlist, flightReciever recieverType)
        {
            BindingList<HoldingNavaid> holdingNavList = new BindingList<HoldingNavaid>();

           
            HoldingNavaidOperation.CheckedNavCount = 0;
            
            foreach (Navaid nav in Navlist)
            {
                double dirAzimut,inverseAzimut;
                ARANFunctions.ReturnGeodesicAzimuth(GeomFunctions.Assign(nav.location),ModelPtChoise.CurPoint,out dirAzimut,out inverseAzimut);

                double distance = ARANFunctions.Hypot(ARANFunctions.ReturnGeodesicDistance
                    (nav.location.x, nav.location.y, ModelPtChoise.CurPoint.X, ModelPtChoise.CurPoint.Y),Common.DeConvertHeight(ModelAreamParam.Altitude));
                holdingNavList.Add(new HoldingNavaid(nav,false,distance,dirAzimut));
            }

            if (recieverType == flightReciever.DMEDME)
            {
                BindingList<HoldingNavaid> tmpList = new BindingList<HoldingNavaid>();
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

            HoldingNavaidOperation.HoldingNavList.Clear();
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

    public class HoldingNavComparer : IEqualityComparer<HoldingNavaid>
    {
        public bool Equals(HoldingNavaid x, HoldingNavaid y)
        {
            if (x.Designator == y.Designator)
                return true;
            else
                return false;
        }

        public int GetHashCode(HoldingNavaid obj)
        {
            return obj.GetHashCode();
        }
    }

}
