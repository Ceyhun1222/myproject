using ANCOR.MapElements;
using ArenaStatic;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SigmaChart
{
    public class SigmaTerminal_EnrouteChartExamination : SigmaChartExamination
    {
        public override void ChartExamination()
        {
            ////     EnrouteChart_Type = 1,
            ////SIDChart_Type = 2,
            ////ChartTypeA =3,
            ////STARChart_Type = 4,
            ////IAPChart_Type = 5,

            this.ExaminationResults = new List<ExaminationMessage>();
            List<PDM.PDMObject> _objLst = new List<PDMObject>();
            List<PDM.PDMObject> _tmpLst = null;
            AirspaceType[] arspcColloutTypes = { AirspaceType.TMA, AirspaceType.TMA_P, AirspaceType.CTR, AirspaceType.CTR_P, AirspaceType.ATZ, AirspaceType.ATZ_P };
            AirspaceType[] arspcNotColloutTypes = { AirspaceType.FIR, AirspaceType.FIR_P, AirspaceType.D, AirspaceType.P, AirspaceType.R, AirspaceType.UIR, AirspaceType.UIR_P };


            foreach (KeyValuePair<string, List<PDMObject>> _data in this.ExaminationData)
            {
                string rFn = _data.Key;
                _tmpLst = _data.Value;


                List<AbstractChartElement> protoItemList = (from element in SigmaDataCash.prototype_anno_lst where (element != null) && (element.RelatedFeature.StartsWith(rFn)) select element).ToList();

                //ExaminationSection _sec = new ExaminationSection { PdmFeatureType = rFn, Section = new List<ExaminationMessage>() };

                foreach (var protoItem in protoItemList)
                {
                    _objLst.Clear();
                    #region Data preparation

                    switch (protoItem.Name)
                    {
                        case ("Airspace_Simple"):

                            foreach (var item in _tmpLst)
                            {
                                Airspace arsps = (Airspace)item;
                                if (arspcNotColloutTypes.Contains(arsps.CodeType))
                                    _objLst.AddRange(arsps.AirspaceVolumeList);

                            }

                            break;

                        case ("SigmaCollout_Airspace"):
                        case ("Airspace_Class"):
                        case ("ATZ_ATZP_Airspace"):
                        case ("CTR_CTRP_Airspace"):
                        case ("TMA_TMAP_Airspace"):
                        case ("TIZ_Airspace"):
                        case ("TIA_Airspace"):
                        case ("SECTOR_SECTORC_Airspace"):
                        case ("R_D_P_Airspace"):
                        case ("R_D_P_AMC_Airspace"):
                        case ("TRA_TSA_Airspace"):
                        case ("PROTECT_Airspace"):
                        case ("AOR_Airspace"):
                        case ("FIS_Airspace"):
                            foreach (var item in _tmpLst)
                            {
                                Airspace arsps = (Airspace)item;
                                if (arspcColloutTypes.Contains(arsps.CodeType) && _objLst.IndexOf(arsps) < 0)
                                {
                                    _objLst.Add(arsps);
                                    _objLst.AddRange(arsps.AirspaceVolumeList);
                                }

                            }

                            break;

                        case ("SigmaCollout_Navaid"):

                            foreach (var item in _tmpLst)
                            {
                                NavaidSystem navSys = (NavaidSystem)item;

                                _objLst.Add(navSys);
                                _objLst.AddRange(navSys.Components);


                            }

                            break;

                        case ("Airport"):
                        case ("GeoBorder_name"):
                        case ("ProcedureLegLength"):
                        case ("SigmaCollout_Designatedpoint"):
                        case ("ProcedureLegSpeed"):
                        case ("ProcedureLegCourse"):
                        case ("ProcedureLegName"):
                        case ("ProcedureLegHeight"):
                        case ("AngleIndication"):
                        case ("DistanceIndication"):
                        case ("HoldingPattern"):
                        case ("HoldingPatternInboundCource"):
                        case ("HoldingPatternOutboundCource"):
                        case ("RouteSegment_ValMagTrack"):
                        case ("RouteSegment_ValReversMagTrack"):
                        case ("RouteSegment_sign"):
                        case ("RouteSegment_UpperLowerLimit"):
                            _objLst.AddRange(_tmpLst);

                            break;
                        default:
                            break;
                    }

                    #endregion

                    #region Validate
                    List<string> lll = new List<string>();
                    if (protoItem is ChartElement_SimpleText)
                    {
                        ChartElement_SimpleText txt = (ChartElement_SimpleText)protoItem;


                        foreach (var pdmOBJ in _objLst)
                        {
                            if (lll.Contains(rFn)) continue;
                            lll.Add(rFn);

                            ExaminationMessage _mes = new ExaminationMessage { PdmFeatureType = rFn,  PdmFeatureId = pdmOBJ.ID, PdmFeatureLabel = pdmOBJ.GetObjectLabel(), ExaminationsDetails = new List<string>() };
                            foreach (var ln in txt.TextContents)
                            {
                                foreach (var wrd in ln)
                                {

                                    if (ArenaStaticProc.HasProperty(pdmOBJ, wrd.DataSource.Value))
                                    {
                                        var obj = ArenaStaticProc.GetObjectValue(pdmOBJ, wrd.DataSource.Value, false);
                                        if (obj == null || obj.ToString().StartsWith("NaN"))
                                        {
                                            string detStr = wrd.DataSource.Value + " = null";
                                            if (_mes.ExaminationsDetails.IndexOf(detStr) < 0) _mes.ExaminationsDetails.Add(detStr);
                                        }
                                        else if (obj == null || obj.ToString().StartsWith("OTHER"))
                                        {
                                            string detStr = wrd.DataSource.Value + " = "+obj.ToString();
                                            if (_mes.ExaminationsDetails.IndexOf(detStr) < 0) _mes.ExaminationsDetails.Add(detStr);
                                        }
                                    }
                                }
                            }

                            if (protoItem is ChartElement_SigmaCollout)
                            {
                                ChartElement_SigmaCollout calloutTxt = (ChartElement_SigmaCollout)protoItem;

                                if (calloutTxt.CaptionTextLine != null && calloutTxt.CaptionTextLine.Count > 0)
                                {
                                    foreach (var ln in calloutTxt.CaptionTextLine)
                                    {
                                        foreach (var wrd in ln)
                                        {
                                            if (ArenaStaticProc.HasProperty(pdmOBJ, wrd.DataSource.Value))
                                            {
                                                var obj = ArenaStaticProc.GetObjectValue(pdmOBJ, wrd.DataSource.Value, false);
                                                if (obj == null)
                                                {
                                                    _mes.ExaminationsDetails.Add(wrd.DataSource.Value + " = null");
                                                }
                                                else if (obj == null || obj.ToString().StartsWith("OTHER"))
                                                {
                                                    _mes.ExaminationsDetails.Add(wrd.DataSource.Value + " = " + obj.ToString());
                                                }
                                            }
                                        }
                                    }
                                }

                                if (calloutTxt.BottomTextLine != null && calloutTxt.BottomTextLine.Count > 0)
                                {
                                    foreach (var ln in calloutTxt.BottomTextLine)
                                    {
                                        foreach (var wrd in ln)
                                        {
                                            if (ArenaStaticProc.HasProperty(pdmOBJ, wrd.DataSource.Value))
                                            {
                                                var obj = ArenaStaticProc.GetObjectValue(pdmOBJ, wrd.DataSource.Value, false);
                                                if (obj == null)
                                                {
                                                    _mes.ExaminationsDetails.Add(wrd.DataSource.Value + " = null");
                                                }
                                                else if (obj == null || obj.ToString().StartsWith("OTHER"))
                                                {
                                                    _mes.ExaminationsDetails.Add(wrd.DataSource.Value + " = " + obj.ToString());
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            if (_mes.ExaminationsDetails.Count > 0) this.ExaminationResults.Add(_mes);
                        }

                    }

                    #endregion

                }

                
            }


        }

        public void FillExaminationData(List<PDMObject> arspc_featureList, List<PDMObject> navaidsList, List<PDMObject> ADHPList, List<PDMObject> GeoBorderList, List<ProcedureLeg> selectedLegs)
        {
            this.ExaminationData.Add("Airspace", arspc_featureList);
            this.ExaminationData.Add("Navaids", navaidsList);
            this.ExaminationData.Add("AirportHeliport", ADHPList);
            this.ExaminationData.Add("GeoBorder", GeoBorderList);


            List<PDMObject> _legs = new List<PDMObject>();
            List<PDMObject> _dpn = new List<PDMObject>();
            List<PDMObject> _hld = new List<PDMObject>();
            List<PDMObject> _angl = new List<PDMObject>();
            List<PDMObject> _dist = new List<PDMObject>();
            foreach (var item in selectedLegs)
            {
                _legs.Add(item);

                if (item.StartPoint != null && item.StartPoint.PointChoice == PointChoice.DesignatedPoint)
                    _dpn.Add(item.StartPoint);

                if (item.EndPoint != null && item.EndPoint.PointChoice == PointChoice.DesignatedPoint)
                    _dpn.Add(item.EndPoint);

                if (item.HoldingUse != null)
                    _hld.Add(item.HoldingUse);

                if (item.StartPoint != null && item.StartPoint.PointFacilityMakeUp != null && item.StartPoint.PointFacilityMakeUp.AngleIndication != null)
                    _angl.Add(item.StartPoint.PointFacilityMakeUp.AngleIndication);
                if (item.StartPoint != null && item.StartPoint.PointFacilityMakeUp != null && item.StartPoint.PointFacilityMakeUp.DistanceIndication != null)
                    _dist.Add(item.StartPoint.PointFacilityMakeUp.DistanceIndication);

                if (item.EndPoint != null && item.EndPoint.PointFacilityMakeUp != null && item.EndPoint.PointFacilityMakeUp.AngleIndication != null)
                    _angl.Add(item.EndPoint.PointFacilityMakeUp.AngleIndication);
                if (item.EndPoint != null && item.EndPoint.PointFacilityMakeUp != null && item.EndPoint.PointFacilityMakeUp.DistanceIndication != null)
                    _dist.Add(item.EndPoint.PointFacilityMakeUp.DistanceIndication);

            }
            if (_dpn.Count > 0) this.ExaminationData.Add("DesignatedPoint", _dpn);
            if (_hld.Count > 0) this.ExaminationData.Add("HoldingPattern", _hld);
            if (_angl.Count > 0) this.ExaminationData.Add("AngleIndication", _angl);
            if (_dist.Count > 0) this.ExaminationData.Add("DistanceIndication", _dist);
            if (_legs.Count > 0) this.ExaminationData.Add("ProcedureLeg", _legs);

            
        }

        public void FillExaminationData(List<PDMObject> arspc_featureList, List<PDMObject> navaidsList, List<PDMObject> ADHPList, List<PDMObject> GeoBorderList, List<PDMObject> eNRT_featureList, List<PDMObject> hlng_featureList)
        {
            this.ExaminationData.Add("Airspace", arspc_featureList);
            this.ExaminationData.Add("Navaids", navaidsList);
            this.ExaminationData.Add("AirportHeliport", ADHPList);
            this.ExaminationData.Add("GeoBorder", GeoBorderList);


            List<PDMObject> _routes = new List<PDMObject>();
            List<PDMObject> _segPn = new List<PDMObject>();

            foreach (var item in eNRT_featureList)
            {
                Enroute enrt = (Enroute)item;

                if (enrt.Routes != null && enrt.Routes.Count > 0)
                {
                    foreach (RouteSegment _rs in enrt.Routes)
                    {
                        _routes.Add(_rs);

                        if (_rs.StartPoint != null && _rs.StartPoint.PointChoice == PointChoice.DesignatedPoint)
                            _segPn.Add(_rs.StartPoint);

                        if (_rs.EndPoint != null && _rs.EndPoint.PointChoice == PointChoice.DesignatedPoint)
                            _segPn.Add(_rs.EndPoint);

                    }
                }

            }
            if (_segPn.Count > 0) this.ExaminationData.Add("DesignatedPoint", _segPn);
            if (hlng_featureList.Count > 0) this.ExaminationData.Add("HoldingPattern", hlng_featureList);
            if (_routes.Count > 0) this.ExaminationData.Add("Enroute", _routes);

        }
    }
}
