using AIP.DB;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
using AIP.GUI.Templates;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using XHTML_WPF.Classes;
using AirportHeliport = Aran.Aim.Features.AirportHeliport;
using Navaid = Aran.Aim.Features.Navaid;
using Point = Aran.Geometries.Point;
using Route = Aran.Aim.Features.Route;

namespace AIP.GUI.Classes
{
    /// <summary>
    /// Class to transfer Data from AIXM to AIP DB
    /// ENR section
    /// Other sections are in the FillDB
    /// </summary>
    internal static partial class FillDB
    {

        private static void SectionENR3xFill_bak(AIPSection ent, List<Feature> featureList, DB.eAIP caip, string section)
        {
            try
            {
                int cnt = 0;
                List<Airspace> ft = Globals.dbPro.GetAllFeatuers(FeatureType.Airspace).Cast<Airspace>().Where(n => n.Class.Count > 0).ToList();
                List<AirspaceLayerClass> lst_aslc = ft.Where(n => n.Class.Count > 0).SelectManyNullSafe(n => n.Class).ToList();
                List<RadioCommunicationChannel> rcc = Globals.dbPro.GetAllFeatuers(FeatureType.RadioCommunicationChannel).Cast<RadioCommunicationChannel>().ToList();
                List<ChangeOverPoint> cop = Globals.dbPro.GetAllFeatuers(FeatureType.ChangeOverPoint).Cast<ChangeOverPoint>().ToList();
                List<StandardLevelColumn> slc = Globals.dbPro.GetAllFeatuers(FeatureType.StandardLevelColumn).Cast<StandardLevelColumn>().ToList();

                bool Static = false;
                List<AIP.DB.Route> rts = new List<AIP.DB.Route>();
                List<AIP.DB.Significantpointreference> spl = new List<AIP.DB.Significantpointreference>();
                List<AIP.DB.Routesegment> rss = new List<AIP.DB.Routesegment>();
                AIP.DB.Route rt = new AIP.DB.Route();
                string designator = "";
                string dp_tmp = "";
                List<object> rt_items = new List<object>();
                AIP.DB.Routesegment rs = new AIP.DB.Routesegment();
                List<Aran.Aim.Features.RouteSegment> rslist = new List<Aran.Aim.Features.RouteSegment>();
                List<DesignatedPoint> dplist = new List<DesignatedPoint>();
                List<AirTrafficControlService> atcslist = new List<AirTrafficControlService>();
                DB.Significantpointreference spr = new DB.Significantpointreference();
                List<Note> RTRemarks = new List<Note>();
                List<Note> SPRemarks = new List<Note>();

                List<Aran.Aim.Features.Route> RouteArr = new List<Aran.Aim.Features.Route>();
                foreach (var x in featureList)
                {
                    if (x.FeatureType == FeatureType.Route)
                    {
                        RouteArr.Add((Aran.Aim.Features.Route)x);
                    }
                }
                List<Aran.Aim.Features.RouteSegment> RouteSegmentArr = new List<Aran.Aim.Features.RouteSegment>();
                foreach (var x in featureList)
                {
                    if (x.FeatureType == FeatureType.RouteSegment)
                    {
                        RouteSegmentArr.Add((Aran.Aim.Features.RouteSegment)x);
                    }
                }
                List<Aran.Aim.Features.DesignatedPoint> DPArr = new List<Aran.Aim.Features.DesignatedPoint>();
                foreach (var x in featureList)
                {
                    if (x.FeatureType == FeatureType.DesignatedPoint)
                    {
                        DPArr.Add((Aran.Aim.Features.DesignatedPoint)x);
                    }
                }
                List<Aran.Aim.Features.Navaid> NavArr = new List<Aran.Aim.Features.Navaid>();
                foreach (var x in featureList)
                {
                    if (x.FeatureType == FeatureType.Navaid)
                    {
                        NavArr.Add((Aran.Aim.Features.Navaid)x);
                    }
                }
                List<Aran.Aim.Features.AirTrafficControlService> atcsArr = new List<Aran.Aim.Features.AirTrafficControlService>();
                foreach (var x in featureList)
                {
                    if (x.FeatureType == FeatureType.AirTrafficControlService)
                    {
                        atcsArr.Add((Aran.Aim.Features.AirTrafficControlService)x);
                    }
                }
                List<Aran.Aim.Features.RouteSegment> RS = new List<Aran.Aim.Features.RouteSegment>();
                IEnumerable<Aran.Aim.Features.Route> flst = null;
                // ENR 32 By ICAO annex 11, sec 1-1 (2.3)
                if (section == "ENR31")
                {
                    flst = RouteArr.OfType<Aran.Aim.Features.Route>().Where(
                        n => n.DesignatorPrefix != CodeRouteDesignatorPrefix.U
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.L
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.M
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.N
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.P
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.Q
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.T
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.Y
                    || n.DesignatorSecondLetter != CodeRouteDesignatorLetter.Z
                    || n.DesignatorPrefix != CodeRouteDesignatorPrefix.K
                        ).OrderBy(n => n.DesignatorSecondLetter).ThenBy(n => n.DesignatorNumber);
                    if (flst.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }
                else if (section == "ENR32")
                {
                    flst = RouteArr.OfType<Aran.Aim.Features.Route>().Where(
                        n => n.DesignatorPrefix == CodeRouteDesignatorPrefix.U
                        ).OrderBy(n => n.DesignatorSecondLetter).ThenBy(n => n.DesignatorNumber);
                    if (flst.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }
                // ENR 33 By ICAO annex 11, sec 1-1 (2.2.1)
                else if (section == "ENR33")
                {
                    flst = RouteArr.OfType<Aran.Aim.Features.Route>().Where(
                        n => n.DesignatorSecondLetter == CodeRouteDesignatorLetter.L
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.M
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.N
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.P
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.Q
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.T
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.Y
                    || n.DesignatorSecondLetter == CodeRouteDesignatorLetter.Z
                    ).OrderBy(n => n.DesignatorSecondLetter).ThenBy(n => n.DesignatorNumber);
                    if (flst.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }
                // ENR 34 By ICAO annex 11, sec 1-1 (2.3)
                else if (section == "ENR34")
                {
                    flst = RouteArr.OfType<Aran.Aim.Features.Route>().Where(
                        n => n.DesignatorPrefix == CodeRouteDesignatorPrefix.K
                        ).OrderBy(n => n.DesignatorSecondLetter).ThenBy(n => n.DesignatorNumber);
                    if (flst.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }


                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (Aran.Aim.Features.Route ritem in flst)
                    {
                        rslist.Clear(); RS.Clear(); atcslist.Clear(); RTRemarks.Clear(); SPRemarks.Clear(); dplist.Clear();
                        dp_tmp = "";
                        cnt = 0;

                        rt = new AIP.DB.Route();
                        designator = ritem.Name;
                        rt.Routedesignator = designator;

                        foreach (Aran.Aim.Features.RouteSegment x in RouteSegmentArr)
                        {
                            if (x.RouteFormed.Identifier == ritem.Identifier)
                                RS.Add(x);
                        }
                        rslist = Lib.SortSegments(RS, false);

                        foreach (AirTrafficControlService x in atcsArr)
                        {
                            foreach (RoutePortion y in x.ClientRoute)
                            {
                                if (y.ReferencedRoute.Identifier == ritem.Identifier)
                                    atcslist.Add(x);
                            }
                        }

                        string dp_startname = "";
                        string dp_endname = "";
                        foreach (Aran.Aim.Features.RouteSegment item in rslist)
                        {
                            var dp_start = DPArr.Where(x => x.Identifier == item.Start.PointChoice.FixDesignatedPoint.Identifier).FirstOrDefault();
                            var dp_end = DPArr.Where(x => x.Identifier == item.End.PointChoice.FixDesignatedPoint.Identifier).FirstOrDefault();

                            var nav_start = NavArr.Where(x => x.Identifier == item.Start.PointChoice.FixDesignatedPoint.Identifier).FirstOrDefault();
                            var nav_end = NavArr.Where(x => x.Identifier == item.End.PointChoice.FixDesignatedPoint.Identifier).FirstOrDefault();

                            dp_startname = (dp_start != null) ? dp_start.Designator : (nav_start != null) ? nav_start.Designator : "";
                            dp_endname = (dp_end != null) ? dp_end.Designator : (nav_end != null) ? nav_end.Designator : "";

                            if (item.Annotation.Count() > 0)
                            {
                                RTRemarks.AddRange(item.Annotation);
                            }

                            if (dp_start != null && dp_tmp != dp_startname)
                            {
                                string spatc = "";
                                spr = new DB.Significantpointreference();
                                spr.Ref = "SP-" + dp_startname;
                                if (((Aran.Aim.Features.RouteSegment)item).Start != null)
                                {
                                    Aran.Aim.Features.EnRouteSegmentPoint ersp = ((Aran.Aim.Features.RouteSegment)item).Start;
                                    if (ersp.ReportingATC != null)
                                        spatc = Lib.ConvertAIXMEnum2AIP(ersp.ReportingATC);
                                }
                                spr.SignificantpointATC = spatc;
                                spr.OrderNumber = cnt++;
                                rt_items.Add(spr);
                                dp_tmp = dp_startname;
                                if (dp_start.Annotation.Count() > 0)
                                {
                                    SPRemarks.AddRange(dp_start.Annotation);
                                }
                            }
                            else if (nav_start != null && dp_tmp != dp_startname)
                            {
                                string spatc = "";
                                spr = new DB.Significantpointreference();
                                spr.Ref = "NAV-" + dp_startname;
                                if (((Aran.Aim.Features.RouteSegment)item).Start != null)
                                {
                                    Aran.Aim.Features.EnRouteSegmentPoint ersp = ((Aran.Aim.Features.RouteSegment)item).Start;
                                    if (ersp.ReportingATC != null)
                                        spatc = Lib.ConvertAIXMEnum2AIP(ersp.ReportingATC);
                                }
                                spr.SignificantpointATC = spatc;
                                spr.OrderNumber = cnt++;
                                rt_items.Add(spr);
                                dp_tmp = dp_startname;
                                if (nav_start.Annotation.Count() > 0)
                                {
                                    SPRemarks.AddRange(nav_start.Annotation);
                                }
                            }

                            // ## RouteSegment[requiredNavigationPerformance="%%"]
                            string rnp = ((Aran.Aim.Features.RouteSegment)item).RequiredNavigationPerformance.ToString();
                            rt.RouteRNP = rnp;// TODO: Error: Not inserting

                            // TODO: AIXM???
                            // Routesegmentusage
                            rt.Routesegmentusage = "H24";

                            rs = new DB.Routesegment();
                            // RouteSegment.MagneticTrack 
                            // Routesegmentmagtrack
                            if (((Aran.Aim.Features.RouteSegment)item).MagneticTrack != null)
                            {
                                rs.Routesegmentmagtrack = ((Aran.Aim.Features.RouteSegment)item).MagneticTrack.ToString();
                            }
                            // RouteSegment.ReverseMagneticTrack 
                            // Routesegmentreversemagtrack
                            if (((Aran.Aim.Features.RouteSegment)item).ReverseMagneticTrack != null)
                            {
                                rs.Routesegmentreversemagtrack = ((Aran.Aim.Features.RouteSegment)item).ReverseMagneticTrack.ToString();
                            }
                            // RouteSegment.Length 
                            // Routesegmentlength
                            if (((Aran.Aim.Features.RouteSegment)item).Length != null)
                            {
                                rs.Routesegmentlength = ((Aran.Aim.Features.RouteSegment)item).Length.ToString();
                            }
                            // RouteSegment.UpperLimit    
                            // Routesegmentupper
                            if (((Aran.Aim.Features.RouteSegment)item).UpperLimit != null)
                            {
                                rs.Routesegmentupper = ((Aran.Aim.Features.RouteSegment)item).UpperLimit.ToString();
                            }
                            // RouteSegment.LowerLimit
                            // Routesegmentlower
                            if (((Aran.Aim.Features.RouteSegment)item).LowerLimit != null)
                            {
                                rs.Routesegmentlower = ((Aran.Aim.Features.RouteSegment)item).LowerLimit.ToString();
                            }
                            // RouteSegment.MinimumEnrouteAltitude
                            // Routesegmentminimum
                            if (((Aran.Aim.Features.RouteSegment)item).MinimumEnrouteAltitude != null)
                            {
                                rs.Routesegmentminimum = ((Aran.Aim.Features.RouteSegment)item).MinimumEnrouteAltitude.ToString();
                            }
                            // RouteSegment.WidthLeft + RouteSegment.WidthRight + RouteSegment.MinimumObstacleClearanceAltitude
                            // Routesegmentwidth
                            if (((Aran.Aim.Features.RouteSegment)item).WidthLeft != null && ((Aran.Aim.Features.RouteSegment)item).WidthRight != null && ((Aran.Aim.Features.RouteSegment)item).MinimumObstacleClearanceAltitude != null)
                            {
                                rs.Routesegmentwidth = ((Aran.Aim.Features.RouteSegment)item).WidthLeft.ToString() + ((Aran.Aim.Features.RouteSegment)item).WidthRight.ToString() + ((Aran.Aim.Features.RouteSegment)item).MinimumObstacleClearanceAltitude.ToString();

                            }

                            if (atcslist.Count > 0)
                            {
                                if (((Aran.Aim.Features.AirTrafficControlService)atcslist.FirstOrDefault()).Name != null)
                                {
                                    RadioCommunicationChannel rcc_item = rcc.Where(n => n.Identifier == ((Aran.Aim.Features.AirTrafficControlService)atcslist.FirstOrDefault()).RadioCommunication.FirstOrDefault().Feature.Identifier).FirstOrDefault();
                                    if (rcc_item != null)
                                    {
                                        rs.RoutesegmentATC = ((Aran.Aim.Features.AirTrafficControlService)atcslist.FirstOrDefault()).Name.ToString() + ", " + rcc_item.Logon + " FREQ: " + rcc_item.FrequencyTransmission.ToString();
                                    }
                                    else
                                        rs.RoutesegmentATC = ((Aran.Aim.Features.AirTrafficControlService)atcslist.FirstOrDefault()).Name.ToString();
                                }
                            }
                            if (((Aran.Aim.Features.RouteSegment)item).Availability != null)
                            {
                                List<Aran.Aim.Features.RouteAvailability> ra = ((Aran.Aim.Features.RouteSegment)item).Availability;
                                foreach (RouteAvailability ra_level in ra)
                                {
                                    foreach (StandardLevelColumn slc_item in slc)
                                    {
                                        if (ra_level.Levels.Where(n => n.DiscreteLevelSeries != null && n.DiscreteLevelSeries.Identifier == slc_item.Identifier).Any())
                                        {
                                            rs.Routesegmentusagereference = new DB.Routesegmentusagereference[]
                                            {
                                                new DB.Routesegmentusagereference {
                                                    Ref = "RSU-"+designator+"-1",
                                                Routesegmentusageleveltype = slc_item.Series.ToString(),
                                                Routesegmentusagedirection = Lib.ConvertAIXMEnum2AIP<CodeDirection>(ra_level.Direction)
                                                }
                                            };
                                        }
                                    }

                                    foreach (AirspaceLayer asl in ra_level.Levels)
                                    {
                                        foreach (AirspaceLayerClass xc in lst_aslc)
                                        {
                                            if (xc.AssociatedLevels.Where(n => n.DiscreteLevelSeries != null && asl.DiscreteLevelSeries != null &&
                                                n.DiscreteLevelSeries.Identifier == asl.DiscreteLevelSeries.Identifier).Count() > 0)
                                            {
                                                rs.Routesegmentairspaceclass = "Class " + xc.Classification.ToString();
                                            }
                                        }
                                    }

                                }
                            }
                            rs.OrderNumber = cnt++;
                            rt_items.Add(rs);

                            if (dp_end != null && dp_tmp != dp_endname)
                            {
                                string spatc2 = "";
                                spr = new DB.Significantpointreference();
                                spr.Ref = "SP-" + dp_endname;
                                if (((Aran.Aim.Features.RouteSegment)item).End != null)
                                {
                                    Aran.Aim.Features.EnRouteSegmentPoint ersp = ((Aran.Aim.Features.RouteSegment)item).End;
                                    if (ersp.ReportingATC != null)
                                        spatc2 = Lib.ConvertAIXMEnum2AIP(ersp.ReportingATC);
                                }
                                spr.SignificantpointATC = spatc2;
                                spr.OrderNumber = cnt++;
                                rt_items.Add(spr);
                                dp_tmp = dp_endname;
                                if (dp_end.Annotation.Count() > 0)
                                {
                                    SPRemarks.AddRange(dp_end.Annotation);
                                }
                            }
                            else if (nav_end != null && dp_tmp != dp_endname)
                            {
                                string spatc2 = "";
                                spr = new DB.Significantpointreference();
                                spr.Ref = "NAV-" + dp_endname;
                                if (((Aran.Aim.Features.RouteSegment)item).End != null)
                                {
                                    Aran.Aim.Features.EnRouteSegmentPoint ersp = ((Aran.Aim.Features.RouteSegment)item).End;
                                    if (ersp.ReportingATC != null)
                                        spatc2 = Lib.ConvertAIXMEnum2AIP(ersp.ReportingATC);
                                }
                                spr.SignificantpointATC = spatc2;
                                spr.OrderNumber = cnt++;
                                rt_items.Add(spr);
                                dp_tmp = dp_endname;
                                if (nav_end.Annotation.Count() > 0)
                                {
                                    SPRemarks.AddRange(nav_end.Annotation);
                                }
                            }
                        }
                        // ??????????????????????
                        //rt.Items = rt_items.ToArray();


                        if (RTRemarks.Count > 0)
                        {
                            string[] remarks_array = RTRemarks.Where(n => n.Purpose != null && n.TranslatedNote != null && n.Purpose == Aran.Aim.Enums.CodeNotePurpose.REMARK).SelectManyNullSafe(n => n.TranslatedNote).OfType<LinguisticNote>().Where(t => t.Note.Lang == Aran.Aim.Enums.language.ENG).Select(t => t.Note.Value).ToArray();

                            List<string> lst_rsr = new List<string>();
                            foreach (string remark in remarks_array)
                            {
                                lst_rsr.Add(remark);
                            }

                            rt.Routesegmentremark = lst_rsr.ToArray();
                        }

                        //rts.Add(rt);
                        rt.SubClassType = SubClassType.Route;
                        rt.eAIP = caip;
                        rt.eAIPID = Lib.CurrentAIP.id;
                        rt.AIPSection = ent;
                        //db.Route.Add(rt);
                        rts.Add(rt);
                        foreach (var item in rt_items)
                        {
                            if (item is DB.Significantpointreference)
                            {
                                ((DB.Significantpointreference)item).eAIPID = Lib.CurrentAIP.id;
                                ((DB.Significantpointreference)item).eAIP = caip;
                                ((DB.Significantpointreference)item).SubClassType = SubClassType.Significantpointreference;
                                ((DB.Significantpointreference)item).Parent = rt;
                                //db.Significantpointreference.Add((Dataset.Significantpointreference)item);
                                spl.Add((DB.Significantpointreference)item);
                            }
                            else if (item is DB.Routesegment)
                            {
                                ((DB.Routesegment)item).eAIPID = Lib.CurrentAIP.id;
                                ((DB.Routesegment)item).eAIP = caip;
                                ((DB.Routesegment)item).SubClassType = SubClassType.Routesegment;
                                ((DB.Routesegment)item).Parent = rt;
                                //db.Routesegment.Add((Dataset.Routesegment)item);
                                rss.Add((DB.Routesegment)item);
                            }
                        }
                        rt_items.Clear();
                    }

                    //db.Route.AddRange(rts);
                    //db.Significantpointreference.AddRange(spl);
                    //db.Routesegment.AddRange(rss);
                    //db.Entry(ent).State = EntityState.Modified;
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    //db.SaveChanges();
                    db.Route.AddRange(rts);
                    //db.BulkInsert(rts);
                    db.Significantpointreference.AddRange(spl);
                    //db.BulkInsert(spl);
                    db.Routesegment.AddRange(rss);
                    //db.BulkInsert(rss);

                    db.SaveChanges();
                    transaction.Complete();

                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }
        private static void SectionENR3xFill(AIPSection ent, List<Feature> featureList, DB.eAIP caip, string section)
        {
            try
            {

                List<Route> rtAllList = Globals.GetFeaturesByED(FeatureType.Route)?
                    .Cast<Aran.Aim.Features.Route>()
                    .ToList();
                List<RouteSegment> rsList = Globals.GetFeaturesByED(FeatureType.RouteSegment)?
                    .Cast<Aran.Aim.Features.RouteSegment>()
                    .ToList();
                var rnavGuidList = rsList?.Where(x => x.NavigationType == CodeRouteNavigation.RNAV).Select(x => x.RouteFormed.Identifier).ToList();
                List<AirTrafficControlService> atcsList = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)?
                    .Cast<Aran.Aim.Features.AirTrafficControlService>()
                    .ToList();
                List<RadioCommunicationChannel> rccList = Globals.GetFeaturesByED(FeatureType.RadioCommunicationChannel)?
                    .Cast<Aran.Aim.Features.RadioCommunicationChannel>()
                    .ToList();
                List<AirspaceLayerClass> alcList = Globals.GetFeaturesByED(FeatureType.Airspace)?
                    .Cast<Aran.Aim.Features.Airspace>()
                    .Where(n => n.Class.Count > 0)
                    .SelectManyNullSafe(n => n.Class)
                    .ToList();
                List<StandardLevelColumn> slcAllList = Globals.GetFeaturesByED(FeatureType.StandardLevelColumn)?
                    .Cast<Aran.Aim.Features.StandardLevelColumn>()
                    .ToList();
                List<DesignatedPoint> dpAllList = Globals.GetFeaturesByED(FeatureType.DesignatedPoint)?
                    .Cast<Aran.Aim.Features.DesignatedPoint>()
                    .ToList();
                List<Navaid> navAllList = Globals.GetFeaturesByED(FeatureType.Navaid)?
                    .Cast<Aran.Aim.Features.Navaid>()
                    .ToList();

                List<CodeRouteDesignatorLetter?> InLetter = new List<CodeRouteDesignatorLetter?>()
                {
                    CodeRouteDesignatorLetter.L,
                    CodeRouteDesignatorLetter.M,
                    CodeRouteDesignatorLetter.N,
                    CodeRouteDesignatorLetter.P,
                    CodeRouteDesignatorLetter.Q,
                    CodeRouteDesignatorLetter.T,
                    CodeRouteDesignatorLetter.Y,
                    CodeRouteDesignatorLetter.Z
                };
                List<Route> rtList = new List<Route>();
                // ENR 32 By ICAO annex 11, sec 1-1 (2.3)
                // How to map enr31-34 By Teodor description:
                // If RouteSegment`s type is RNAV - their route are for 3.3
                // If not, and their route have prefix U - 3.2, prefix K - 3.4, no prefix - 3.1



                //
                if (section == "ENR31")
                {
                    //List<CodeRouteDesignatorPrefix?> notInPrefix = new List<CodeRouteDesignatorPrefix?>()
                    //{
                    //    CodeRouteDesignatorPrefix.U,
                    //    CodeRouteDesignatorPrefix.K
                    //};
                    //rtList = rtAllList?.Where(
                    //    n => !notInPrefix.Contains(n.DesignatorPrefix) ||
                    //         !InLetter.Contains(n.DesignatorSecondLetter)
                    //    ).
                    //    OrderBy(n => n.DesignatorSecondLetter)
                    //    .ThenBy(n => n.DesignatorNumber)
                    //    .ToList();
                    rtList = rtAllList?.Where(
                            n => rnavGuidList != null &&
                                 !rnavGuidList.Contains(n.Identifier) &&
                                 n.DesignatorPrefix == null
                        ).
                        OrderBy(n => n.DesignatorSecondLetter).
                        ThenBy(n => n.DesignatorNumber).
                        ToList();

                    if (rtList?.Count == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }
                else if (section == "ENR32")
                {
                    rtList = rtAllList?.Where(
                        n => rnavGuidList != null &&
                        !rnavGuidList.Contains(n.Identifier) &&
                        n.DesignatorPrefix == CodeRouteDesignatorPrefix.U
                        ).
                        OrderBy(n => n.DesignatorSecondLetter).
                        ThenBy(n => n.DesignatorNumber).
                        ToList();
                    //rtList = rtAllList?.Where(
                    //    n => n.DesignatorPrefix == CodeRouteDesignatorPrefix.U
                    //).OrderBy(n => n.DesignatorSecondLetter).ThenBy(n => n.DesignatorNumber).ToList();
                    if (rtList?.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }
                // ENR 33 By ICAO annex 11, sec 1-1 (2.2.1)
                else if (section == "ENR33")
                {
                    rtList = rtAllList?.Where(
                            n => rnavGuidList != null && rnavGuidList.Contains(n.Identifier))
                        .OrderBy(n => n.DesignatorSecondLetter)
                        .ThenBy(n => n.DesignatorNumber)
                        .ToList();

                    //rtList = rtAllList?.Where(
                    //    n => InLetter.Contains(n.DesignatorSecondLetter)
                    //)
                    //.OrderBy(n => n.DesignatorSecondLetter)
                    //.ThenBy(n => n.DesignatorNumber)
                    //.ToList();

                    if (rtList?.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }
                // ENR 34 By ICAO annex 11, sec 1-1 (2.3)
                else if (section == "ENR34")
                {
                    //rtList = rtAllList?.Where(
                    //    n => n.DesignatorPrefix == CodeRouteDesignatorPrefix.K
                    //    ).OrderBy(n => n.DesignatorSecondLetter).ThenBy(n => n.DesignatorNumber).ToList();

                    rtList = rtAllList?.Where(
                            n => rnavGuidList != null &&
                                 !rnavGuidList.Contains(n.Identifier) &&
                                 n.DesignatorPrefix == CodeRouteDesignatorPrefix.K
                        ).
                        OrderBy(n => n.DesignatorSecondLetter).
                        ThenBy(n => n.DesignatorNumber).
                        ToList();

                    if (rtList?.Count() == 0)
                    {
                        ((DB.AIPSection)ent).NIL = DB.NILReason.Notavailable;
                    }
                }

                List<AIP.DB.Route> rts = new List<AIP.DB.Route>();
                List<AIP.DB.Significantpointreference> spl = new List<AIP.DB.Significantpointreference>();
                List<AIP.DB.Routesegment> rss = new List<AIP.DB.Routesegment>();
                List<AIP.DB.Routesegmentusagereference> rsur = new List<AIP.DB.Routesegmentusagereference>();

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (rtList != null)
                    {
                        foreach (Aran.Aim.Features.Route ritem in rtList)
                        {
                            List<RouteSegment> RouteRsList =
                                Lib.SortSegments(
                                    rsList.Where(x => x.RouteFormed.Identifier == ritem.Identifier).ToList(), false);
                            var dpGuidList = RouteRsList
                                .Select(x => x.Start.PointChoice.FixDesignatedPoint.Identifier)
                                .ToList();
                            List<DesignatedPoint> dpList = dpAllList.Where(x => dpGuidList != null && dpGuidList.Contains(x.Identifier))
                                .ToList();
                            var navGuidList = RouteRsList
                                .Select(x => x.Start.PointChoice.NavaidSystem.Identifier)
                                .ToList();
                            List<Navaid> navList = navAllList.Where(x => navGuidList != null && navGuidList.Contains(x.Identifier))
                                .ToList();
                            //List<AirTrafficControlService> RouteAtcsList = atcsList.Where(x=>x.re)

                            //rslist.Clear(); RS.Clear(); atcslist.Clear(); RTRemarks.Clear(); SPRemarks.Clear(); dplist.Clear();
                            //dp_tmp = "";
                            //cnt = 0;
                            List<object> rt_items = new List<object>();
                            string dp_tmp = "";
                            int cnt = 0;
                            var rt = new AIP.DB.Route();
                            rt.Routedesignator = ritem.Name;
                            rt.Routeremark = Lib.AIXM_GetNotes(ritem.Annotation);
                            string dp_startname = "";
                            string dp_endname = "";
                            string designator = ritem.Name;
                            foreach (Aran.Aim.Features.RouteSegment item in RouteRsList)
                            {
                                Routesegment rs = new DB.Routesegment();
                                rt.RouteRNP = item.RequiredNavigationPerformance?.ToString();
                                rt.Routesegmentremark = new string[] { Lib.AIXM_GetNotes(item.Annotation) };
                                rt.Routesegmentusage =
                                    Lib.GetHoursOfOperations(item.Availability.SelectManyNullSafe(x => x.TimeInterval)
                                        .ToList());
                                rs.Routesegmentmagtrack = item.MagneticTrack?.ToString();
                                rs.Routesegmentreversemagtrack = item.ReverseMagneticTrack.ToString();
                                rs.Routesegmentlength = item.Length?.StringValue;
                                rs.Routesegmentupper = item.UpperLimit?.ToValString();
                                rs.Routesegmentlower = item.LowerLimit?.ToValString();
                                rs.Routesegmentminimum = item.MinimumEnrouteAltitude?.StringValue;

                                List<Guid?> g2List = item.Availability?.SelectManyNullSafe(x =>
                                                            x.Levels?
                                                            .Select(ix => ix.DiscreteLevelSeries?.Identifier))
                                                        .ToList();
                                if (g2List != null)
                                {
                                    rs.Routesegmentairspaceclass = alcList?
                                        .FirstOrDefault(x => x.AssociatedLevels
                                            .Any(ix =>
                                                g2List.Contains(ix.DiscreteLevelSeries?.Identifier)))?
                                        .Classification
                                        .ToString();

                                    var temp = slcAllList.Where(x => g2List.Contains(x.Identifier)).ToList();
                                    foreach (StandardLevelColumn slc in temp)
                                    {
                                        Routesegmentusagereference rsurItem = new Routesegmentusagereference();
                                        rsurItem.Ref = $@"RSA-{designator}-1";
                                        rsurItem.Routesegmentusageleveltype = slc.Series.ToString();
                                        rsurItem.Routesegmentusagedirection =
                                            Lib.ConvertAIXMEnum2AIP<CodeDirection>(item.Availability?.FirstOrDefault().Direction);
                                        rsurItem.eAIP = caip;
                                        rsurItem.Routesegment = rs;

                                        rsur.Add(rsurItem);
                                    }
                                }

                                var tst = alcList?
                                    .FirstOrDefault(x => x.AssociatedLevels
                                        .Any(ix =>
                                            g2List.Contains(ix.DiscreteLevelSeries?.Identifier)))?
                                    .Classification
                                    .ToString();

                                rs.Routesegmentwidth = item.WidthLeft?.ToString() + " " + item.WidthRight?.ToString() + " " + item.MinimumObstacleClearanceAltitude?.StringValue;

                                AirTrafficControlService atcsItem = atcsList
                                                .FirstOrDefault(x =>
                                                    x.ClientRoute.Select(ox => ox.ReferencedRoute.Identifier)
                                                .Contains(ritem.Identifier));
                                if (atcsItem != null)
                                {
                                    RadioCommunicationChannel rcc_item = rccList.FirstOrDefault(n => atcsItem.RadioCommunication.Select(x => x.Feature.Identifier).Contains(n.Identifier));
                                    rs.RoutesegmentATC = atcsItem.Name;
                                    if (rcc_item != null)
                                    {
                                        rs.RoutesegmentATC +=
                                            ", " + rcc_item.Logon + " FREQ: " +
                                            rcc_item.FrequencyTransmission.ToString();
                                    }
                                }

                                //rs.RoutesegmentATC = item.Start?.ReportingATC?.ToString(); // incorrect mapping

                                var dp_start = dpAllList
                                    .FirstOrDefault(x => x.Identifier == item.Start.PointChoice.FixDesignatedPoint.Identifier);
                                var dp_end = dpAllList
                                    .FirstOrDefault(x => x.Identifier == item.End.PointChoice.FixDesignatedPoint.Identifier);

                                var nav_start = navAllList
                                    .FirstOrDefault(x => x.Identifier == item.Start.PointChoice.FixDesignatedPoint.Identifier);
                                var nav_end = navAllList
                                    .FirstOrDefault(x => x.Identifier == item.End.PointChoice.FixDesignatedPoint.Identifier);

                                dp_startname = dp_start != null ? dp_start.Designator : (nav_start != null) ? nav_start.Designator : "";
                                dp_endname = (dp_end != null) ? dp_end.Designator : (nav_end != null) ? nav_end.Designator : "";

                                if (dp_tmp != dp_startname)
                                {
                                    Significantpointreference spr = new DB.Significantpointreference();
                                    spr.Ref = (dp_start != null ? "SP-" : "NAV-") + dp_startname;
                                    spr.SignificantpointATC = Lib.ConvertAIXMEnum2AIP(item.Start?.ReportingATC);
                                    spr.OrderNumber = cnt++;
                                    rt_items.Add(spr);
                                    dp_tmp = dp_startname;
                                }

                                rs.OrderNumber = cnt++;
                                rt_items.Add(rs);

                                if (dp_tmp != dp_endname)
                                {
                                    Significantpointreference spr = new DB.Significantpointreference();
                                    spr.Ref = (dp_end != null ? "SP-" : "NAV-") + dp_endname;
                                    spr.SignificantpointATC = Lib.ConvertAIXMEnum2AIP(item.Start?.ReportingATC);
                                    spr.OrderNumber = cnt++;
                                    rt_items.Add(spr);
                                    dp_tmp = dp_endname;
                                }
                            }

                            rt.SubClassType = SubClassType.Route;
                            rt.eAIP = caip;
                            rt.eAIPID = Lib.CurrentAIP.id;
                            rt.AIPSection = ent;

                            rts.Add(rt);
                            foreach (var item in rt_items)
                            {
                                if (item is DB.Significantpointreference)
                                {
                                    ((DB.Significantpointreference)item).eAIPID = Lib.CurrentAIP.id;
                                    ((DB.Significantpointreference)item).eAIP = caip;
                                    ((DB.Significantpointreference)item).SubClassType =
                                        SubClassType.Significantpointreference;
                                    ((DB.Significantpointreference)item).Parent = rt;
                                    spl.Add((DB.Significantpointreference)item);
                                }
                                else if (item is DB.Routesegment)
                                {
                                    ((DB.Routesegment)item).eAIPID = Lib.CurrentAIP.id;
                                    ((DB.Routesegment)item).eAIP = caip;
                                    ((DB.Routesegment)item).SubClassType = SubClassType.Routesegment;
                                    ((DB.Routesegment)item).Parent = rt;
                                    rss.Add((DB.Routesegment)item);
                                }
                            }
                            rt_items.Clear();
                        }
                        db.Route.AddRange(rts);
                        db.Significantpointreference.AddRange(spl);
                        db.Routesegment.AddRange(rss);
                        db.Routesegmentusagereference.AddRange(rsur);
                    }
                    else
                    {
                        ent.NIL = NILReason.Notavailable;
                    }
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_ENR21(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<CodeAirspace?> CodeAirspaceList = new List<CodeAirspace?>()
                {
                    CodeAirspace.FIR, CodeAirspace.UIR, CodeAirspace.TMA, CodeAirspace.TMA_P, // By MapDoc
                    CodeAirspace.SECTOR, CodeAirspace.CTA, CodeAirspace.RAS // By Teodor in the Skype
                };

                var oaGuids = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority).Cast<OrganisationAuthority>().Where(n => n.Type == CodeOrganisation.ATS).Select(n => n.Identifier)
                    .ToList();
                var afaGuids = Globals.GetFeaturesByED(FeatureType.AuthorityForAirspace).Cast<AuthorityForAirspace>().Where(n => oaGuids.Contains(n.ResponsibleOrganisation.Identifier) && n.Type == CodeAuthority.DLGT).Select(n => n.AssignedAirspace.Identifier)
                    .ToList();
                IEnumerable<IGrouping<CodeAirspace?, Airspace>> ftgroup = Globals.GetFeaturesByED(FeatureType.Airspace).Cast<Airspace>().Where(n => afaGuids.Contains(n.Identifier) &&
                    CodeAirspaceList.Contains(n.Type)).OrderBy(x => x.Type).ToList().GroupBy(x => x.Type);
                List<Unit> un = Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().ToList();
                List<AirTrafficControlService> at = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)
                    .Cast<AirTrafficControlService>().ToList();
                List<RadioCommunicationChannel> rc = Globals.GetFeaturesByED(FeatureType.RadioCommunicationChannel)
                    .Cast<RadioCommunicationChannel>().ToList();

                //throw new Exception();
                AIP.DB.Subsection ss;
                RulesProcedures rp = null;
                List<ForENR21> dataList = new List<ForENR21>();
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var ft in ftgroup)
                    {
                        ForENR21 data = new ForENR21();
                        data.Type = Lib.GetEnumText(ft.Key).ToUpperInvariant();
                        data.Row = new List<ForENR21.ForENR21Row>();
                        foreach (Airspace air in ft)
                        {
                            ForENR21.ForENR21Row dataRow = new ForENR21.ForENR21Row();
                            var lst = air.Class.Select(n => n.Classification).Distinct().ToList();
                            string Class = string.Join(", ", lst);
                            string Name = air.Name;
                            //string Type = air.Type.ToString();
                            string Limit = air.GeometryComponent.FirstOrDefault()?.TheAirspaceVolume?.ToUpperLower();
                            List<string> strlst = new List<string>();
                            Guid id = air.Identifier;
                            AirTrafficControlService airTraffic = at.FirstOrDefault(n =>
                                n.ClientAirspace.FirstOrDefault(m => m.Feature?.Identifier == air.Identifier) != null);
                            Guid? unitId = airTraffic?.ServiceProvider?.Identifier;
                            string unitName = (unitId != null)
                                ? un.FirstOrDefault(m => m.Identifier == unitId)?.Designator
                                : "";
                            string callSign = airTraffic?.CallSign?.FirstOrDefault()?.CallSign;
                            string language = airTraffic?.CallSign?.FirstOrDefault()?.Language;
                            string freq = "";
                            if (airTraffic != null)
                            {
                                var rc0 = airTraffic?.RadioCommunication.Select(x => x.Feature?.Identifier)
                                    .ToList();
                                List<RadioCommunicationChannel> rcf = rc.Where(n =>
                                    airTraffic.RadioCommunication.Select(x => x.Feature?.Identifier).ToList()
                                        .Contains(n.Identifier)).ToList();
                                freq = string.Join("<br/>",
                                    rcf.Select(n => n.FrequencyTransmission?.StringValue).ToList());
                            }
                            //string conditions = air.Annotation.Where(n=>n.Purpose == CodeNotePurpose.)

                            var notePoints = air.GetHorizontalProjectionAnnotation();

                            string Points = notePoints ??
                                air.GeometryComponent?.SelectManyNullSafe(x => x?.TheAirspaceVolume?
                                .HorizontalProjection?
                                .Geo?
                                .ToMultiPoint()).ToList().ToPointString();
                            string notes = Lib.AIXM_GetNotes(air.Annotation);

                            dataRow.Name = Name;
                            dataRow.Points = Points;
                            dataRow.Limit = Limit;
                            dataRow.Class = Class;
                            dataRow.UnitName = unitName;
                            dataRow.CallSign = callSign;
                            dataRow.Language = language?.ToUpperInvariant();
                            dataRow.Freq = freq;
                            dataRow.Notes = notes;

                            data.Row.Add(dataRow);
                        }
                        dataList.Add(data);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText(ent.SectionName.ToString());
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_ENR31(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                SectionENR3xFill(ent, featureList, caip, "ENR31"); // ENR 3.1-3.5 are very similar and all difference made in the common method
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_ENR32(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                SectionENR3xFill(ent, featureList, caip, "ENR32"); // ENR 3.1-3.5 are very similar and all difference made in the common method
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_ENR33(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                SectionENR3xFill(ent, featureList, caip, "ENR33"); // ENR 3.1-3.5 are very similar and all difference made in the common method
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_ENR34(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                SectionENR3xFill(ent, featureList, caip, "ENR34"); // ENR 3.1-3.5 are very similar and all difference made in the common method
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }


        public static void Fill_ENR36(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<HoldingPattern> hp = Globals.GetFeaturesByED(FeatureType.HoldingPattern).Cast<HoldingPattern>().Where(n => n.Type == CodeHoldingUsage.ENR).ToList();
                List<DesignatedPoint> dp = Globals.GetFeaturesByED(FeatureType.DesignatedPoint).Cast<DesignatedPoint>().OrderBy(x => x.Name).ToList();
                List<AirTrafficControlService> atcs = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService).Cast<AirTrafficControlService>().ToList();
                List<RadioCommunicationChannel> radio = Globals.GetFeaturesByED(FeatureType.RadioCommunicationChannel).Cast<RadioCommunicationChannel>().ToList();
                List<Aran.Aim.Features.Navaid> nav = Globals.GetFeaturesByED(FeatureType.Navaid).Cast<Aran.Aim.Features.Navaid>().OrderBy(x => x.Name).ToList();
                var VOR = Globals.GetFeaturesByED(FeatureType.VOR).Cast<VOR>().ToList();
                var DME = Globals.GetFeaturesByED(FeatureType.DME).Cast<DME>().ToList();
                List<ForENR36> dataList = new List<ForENR36>();

                using (var transaction = new TransactionScope())
                {
                    foreach (DesignatedPoint dpitem in dp)
                    {
                        ForENR36 ForTemplate = new ForENR36();
                        List<HoldingPattern> hpitem = hp.Where(n => n.HoldingPoint?.PointChoice?.FixDesignatedPoint != null && n.HoldingPoint?.PointChoice?.FixDesignatedPoint?.Identifier == dpitem.Identifier).OrderBy(n => n.LowerLimit.StringValue).ToList();
                        if (hpitem.Any())
                        {
                            ForTemplate.Name = dpitem.Designator.ToDataTag(dpitem, "Designator");
                            ForTemplate.Coordinates = dpitem?.Location?.Geo?.ToPointString().ToDataTag(dpitem, "Geo");
                            foreach (HoldingPattern item in hpitem)
                            {
                                ForTemplate.SpeedLimit.Add(item.SpeedLimit?.StringValue.ToDataTag(item, "SpeedLimit"));
                                ForTemplate.UpperLowerLimits.Add((item.LowerLimit?.ToValString(item.LowerLimitReference) + " - " + item.UpperLimit?.ToValString(item.UpperLimitReference)).ToDataTag(item, "LowerLimit"));
                                ForTemplate.Duration.Add(item.OutboundLegSpan?.EndTime?.Duration?.StringValue.ToLowerInvariant().ToDataTag(item, "Duration"));
                                ForTemplate.InboundCourse = item.InboundCourse.ToDegree().ToDataTag(item, "InboundCourse");
                                ForTemplate.Direction = item.TurnDirection?.ToString().UpperFirst().ToDataTag(item, "TurnDirection");

                                var control = atcs?.Where(x => x.ClientHolding.Select(n => n.Feature.Identifier).ToList().Contains(item.Identifier)).FirstOrDefault();
                                if (control != null)
                                {
                                    ForTemplate.ControlUnit.Add(control.Name.ToDataTag(control, "Name"));
                                    List<Guid> radioX = control.RadioCommunication.Select(n => n.Feature.Identifier).ToList();
                                    List<string> freq = radio.Where(x => radioX.Contains(x.Identifier)).Select(x => x.FrequencyTransmission.ToValString().ToDataTag(x, "FrequencyTransmission")).ToList();
                                    ForTemplate.Frequency.AddRange(freq);
                                }
                            }


                            dataList.Add(ForTemplate);
                        }
                    }
                    foreach (Aran.Aim.Features.Navaid dpitem in nav)
                    {
                        List<HoldingPattern> hpitem = hp.Where(n => n.HoldingPoint?.PointChoice?.FixDesignatedPoint != null && n.HoldingPoint?.PointChoice?.FixDesignatedPoint?.Identifier == dpitem.Identifier).OrderBy(n => n.LowerLimit.StringValue).ToList();
                        var type = dpitem.Type.ToString().Replace("_", "/");
                        if (hpitem.Any())
                        {
                            ForENR36 ForTemplate = new ForENR36();
                            ForTemplate.Name = dpitem.Name.ToDataTag(dpitem, "Name");
                            if (dpitem.Type == CodeNavaidService.VOR_DME)
                            {
                                var eqs = dpitem.NavaidEquipment.Select(n => n.TheNavaidEquipment?.Identifier).ToList();
                                IEnumerable<VOR> vors = VOR.Where(n => eqs.Contains(n.Identifier));
                                IEnumerable<DME> dmes = DME.Where(n => eqs.Contains(n.Identifier));
                                List<string> types = new List<string>();
                                foreach (VOR item in vors)
                                {
                                    types.Add((item.Type != null) ? item.Type.ToString() : item.NavaidEquipmentType.ToString());
                                }
                                foreach (DME item in dmes)
                                {
                                    types.Add((item.Type != null) ? item.Type.ToString() : item.NavaidEquipmentType.ToString());
                                }
                                type = string.Join("/", types);
                            }
                            ForTemplate.Ident = (dpitem.Designator + " " + type).ToDataTag(dpitem, "Designator&Type");
                            ForTemplate.Coordinates = dpitem?.Location?.Geo?.ToPointString().ToDataTag(dpitem, "Geo");
                            foreach (HoldingPattern item in hpitem)
                            {
                                ForTemplate.SpeedLimit.Add(item.SpeedLimit?.StringValue.ToDataTag(item, "SpeedLimit"));
                                ForTemplate.UpperLowerLimits.Add((item.LowerLimit?.ToValString(item.LowerLimitReference) + " - " + item.UpperLimit?.ToValString(item.UpperLimitReference)).ToDataTag(item, "LowerLimit"));
                                ForTemplate.Duration.Add(item.OutboundLegSpan?.EndTime?.Duration?.StringValue.ToLowerInvariant().ToDataTag(item, "Duration"));
                                ForTemplate.InboundCourse = item.InboundCourse.ToDegree().ToDataTag(item, "InboundCourse");
                                ForTemplate.Direction = item.TurnDirection?.ToString().UpperFirst().ToDataTag(item, "TurnDirection");

                                var control = atcs?.Where(x => x.ClientHolding.Select(n => n.Feature.Identifier).ToList().Contains(item.Identifier)).FirstOrDefault();
                                if (control != null)
                                {
                                    ForTemplate.ControlUnit.Add(control.Name.ToDataTag(control, "Name"));
                                    List<Guid> radioX = control.RadioCommunication.Select(n => n.Feature.Identifier).ToList();
                                    List<string> freq = radio.Where(x => radioX.Contains(x.Identifier)).Select(x => x.FrequencyTransmission.ToValString().ToDataTag(x, "FrequencyTransmission")).ToList();
                                    ForTemplate.Frequency.AddRange(freq);
                                }
                            }

                            dataList.Add(ForTemplate);
                        }
                    }

                    if (dataList.Count > 0)
                    {
                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }

                        AIP.DB.Subsection ss = new DB.Subsection
                        {
                            AIPSection = ent,
                            Title = Lib.GetText(ent.SectionName.ToString()),
                            eAIP = caip,
                            SubClassType = SubClassType.Subsection,
                            OrderNumber = 1,
                            Content = output
                        };
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }



        //public static void Fill_ENR36(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        //{
        //    try
        //    {
        //        List<HoldingPattern> hp = Globals.GetFeaturesByED(FeatureType.HoldingPattern).Cast<HoldingPattern>().Where(n => n.Type == CodeHoldingUsage.ENR).ToList();
        //        List<DesignatedPoint> dp = Globals.GetFeaturesByED(FeatureType.DesignatedPoint).Cast<DesignatedPoint>().ToList();
        //        List<Aran.Aim.Features.Navaid> nav = Globals.GetFeaturesByED(FeatureType.Navaid).Cast<Aran.Aim.Features.Navaid>().ToList();
        //        List<DB.Sec36Table> DataList = new List<DB.Sec36Table>();

        //        using (var transaction = new TransactionScope())
        //        {
        //            foreach (DesignatedPoint dpitem in dp)
        //            {
        //                DB.Sec36Table l1 = new DB.Sec36Table();

        //                List<DB.Sec36Table2> l2_List = new List<Sec36Table2>();
        //                List<HoldingPattern> hpitem = hp.Where(n => n.HoldingPoint?.PointChoice?.FixDesignatedPoint != null && n.HoldingPoint?.PointChoice?.FixDesignatedPoint?.Identifier == dpitem.Identifier).ToList();
        //                if (hpitem.Any())
        //                {
        //                    l1.rowspan = hpitem.Count();
        //                    l1.Name = dpitem.Designator;
        //                    l1.X = Lib.LonToDDMMSS(dpitem?.Location?.Geo?.X.ToString(), Lib.coordtype.DDMMSS_2, 0);
        //                    l1.Y = Lib.LatToDDMMSS(dpitem.Location?.Geo?.Y.ToString(), Lib.coordtype.DDMMSS_2, 0);
        //                    foreach (HoldingPattern item in hpitem)
        //                    {
        //                        DB.Sec36Table2 Data2 = new DB.Sec36Table2();
        //                        Data2.inboundCourse = item.InboundCourse?.ToDegree();
        //                        Data2.turnDirection = item.TurnDirection?.ToString();
        //                        Data2.speedLimit = item.SpeedLimit?.StringValue;
        //                        Data2.upperLimit = item.UpperLimit?.StringValue;
        //                        Data2.lowerLimit = item.LowerLimit?.StringValue;
        //                        Data2.duration = (item.OutboundLegSpan?.EndTime?.Duration != null) ? item.OutboundLegSpan?.EndTime?.Duration?.StringValue : "";

        //                        Data2.eAIP = caip;
        //                        Data2.Sec36Table = l1;
        //                        l2_List.Add(Data2);
        //                    }

        //                    l1.Sec36Table2 = l2_List;
        //                    l1.SubClassType = SubClassType.Sec36Table;
        //                    DataList.Add(l1);
        //                    l1.eAIP = caip;
        //                    l1.AIPSection = ent;
        //                    db.Sec36Table.Add(l1);
        //                    db.Sec36Table2.AddRange(l2_List);
        //                }
        //            }
        //            foreach (Aran.Aim.Features.Navaid dpitem in nav)
        //            {
        //                DB.Sec36Table Data = new DB.Sec36Table();
        //                List<DB.Sec36Table2> DataList2 = new List<DB.Sec36Table2>();
        //                List<HoldingPattern> hpitem = hp.Where(n => n.HoldingPoint?.PointChoice?.FixDesignatedPoint != null && n.HoldingPoint?.PointChoice?.FixDesignatedPoint?.Identifier == dpitem.Identifier).ToList();
        //                if (hpitem.Any())
        //                {
        //                    Data.rowspan = hpitem.Count();
        //                    Data.Name = dpitem.Name;
        //                    Data.Ident = dpitem.Designator + " " + dpitem.Type.ToString();
        //                    Data.X = Lib.LonToDDMMSS(dpitem.Location?.Geo?.X.ToString(), Lib.coordtype.DDMMSS_2, 0);
        //                    Data.Y = Lib.LatToDDMMSS(dpitem.Location?.Geo?.Y.ToString(), Lib.coordtype.DDMMSS_2, 0);
        //                    foreach (HoldingPattern item in hpitem)
        //                    {
        //                        DB.Sec36Table2 Data2 = new DB.Sec36Table2();
        //                        Data2.inboundCourse = item.InboundCourse?.ToDegree();
        //                        Data2.turnDirection = item.TurnDirection?.ToString();
        //                        Data2.speedLimit = item.SpeedLimit?.StringValue;
        //                        Data2.upperLimit = item.UpperLimit?.StringValue;
        //                        Data2.lowerLimit = item.LowerLimit?.StringValue;
        //                        Data2.duration = (item.OutboundLegSpan != null && item.OutboundLegSpan.EndTime != null && item.OutboundLegSpan.EndTime.Duration != null) ? item.OutboundLegSpan?.EndTime?.Duration?.StringValue : "";
        //                        Data2.eAIP = caip;
        //                        Data2.Sec36Table = Data;
        //                        DataList2.Add(Data2);
        //                    }
        //                    Data.Sec36Table2 = DataList2;
        //                    Data.SubClassType = SubClassType.Sec36Table;
        //                    DataList.Add(Data);

        //                    Data.eAIP = caip;
        //                    Data.AIPSection = ent;
        //                    db.Sec36Table.Add(Data);
        //                    db.Sec36Table2.AddRange(DataList2);
        //                }
        //            }


        //            //db.Entry(ent).State = EntityState.Modified;
        //            ent.SectionStatus = SectionStatusEnum.Filled;
        //            //db.ENR44.Attach(ent);
        //            //db.SaveChanges();
        //            //db.BulkInsert(dpl);
        //            SendOutput("Saving data...", Percent: 90);
        //            //db.BulkSaveChanges();
        //            db.SaveChanges();
        //            //db.BulkMerge(ent);
        //            transaction.Complete();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return;
        //    }
        //}

        public static void Fill_ENR41(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    featureList.Clear();
                    var Nav = Globals.GetFeaturesByED(FeatureType.Navaid).OfType<Aran.Aim.Features.Navaid>();

                    var VOR = Globals.GetFeaturesByED(FeatureType.VOR);
                    var DME = Globals.GetFeaturesByED(FeatureType.DME);
                    var MB = Globals.GetFeaturesByED(FeatureType.MarkerBeacon);
                    var NDB = Globals.GetFeaturesByED(FeatureType.NDB);
                    var TACAN = Globals.GetFeaturesByED(FeatureType.TACAN);
                    var ROUTE = Globals.GetFeaturesByED(FeatureType.Route).OfType<Aran.Aim.Features.Route>();
                    var ROUTEDME = Globals.GetFeaturesByED(FeatureType.RouteDME).OfType<RouteDME>();
                    //var REFROUTE = Globals.GetFeaturesByED(FeatureType.Route).OfType<Aran.Aim.Features.Refe>();

                    List<Feature> ff = new List<Feature>();
                    //var x = Nav.Where(n=>n.NavaidEquipment.All(p => p.TheNavaidEquipment.Identifier))
                    foreach (Aran.Aim.Features.Navaid item in Nav)
                    {
                        featureList.Add(item);
                        foreach (var item2 in item.NavaidEquipment.Where(n => n.TheNavaidEquipment != null))
                        {
                            ff = VOR.Where(n => n.Identifier == item2.TheNavaidEquipment?.Identifier).ToList();
                            if (ff.Any())
                            {
                                featureList.AddRange(ff);
                            }
                            ff = DME.Where(n => n.Identifier == item2.TheNavaidEquipment?.Identifier).ToList();
                            if (ff.Any())
                            {
                                featureList.AddRange(ff);

                                foreach (DME xdme in ff)
                                {
                                    var ret = ROUTEDME?.Where(n => n.ReferencedDME?.Identifier == xdme.Identifier)?.OfType<RouteDME>()?.FirstOrDefault();
                                    if (ret != null)
                                    {
                                        var res = ROUTE.Where(n => n.Identifier == ret.ApplicableRoutePortion?.ReferencedRoute?.Identifier);
                                        if (res.Any())
                                        {
                                            featureList.AddRange(res);
                                        }
                                    }
                                }
                            }

                            ff = MB.Where(n => n.Identifier == item2.TheNavaidEquipment?.Identifier).ToList();
                            if (ff.Any())
                            {
                                featureList.AddRange(ff);
                            }
                            ff = NDB.Where(n => n.Identifier == item2.TheNavaidEquipment?.Identifier).ToList();
                            if (ff.Any())
                            {
                                featureList.AddRange(ff);
                            }
                            ff = TACAN.Where(n => n.Identifier == item2.TheNavaidEquipment?.Identifier).ToList();
                            if (ff.Any())
                            {
                                featureList.AddRange(ff);
                            }
                        }
                    }

                    string navname = "";

                    var flst = featureList.OfType<Aran.Aim.Features.Navaid>().Where(n =>
                    n.Purpose == CodeNavaidPurpose.ALL ||
                    n.Purpose == CodeNavaidPurpose.ENROUTE
                    ).OrderBy(n => n.Name);



                    List<Aran.Aim.Features.Navaid> NavArr = new List<Aran.Aim.Features.Navaid>();
                    foreach (var x in flst)
                    {
                        if (x.FeatureType == FeatureType.Navaid)
                        {
                            NavArr.Add((Aran.Aim.Features.Navaid)x);
                        }
                    }
                    List<Aran.Aim.Features.VOR> VORArr = new List<Aran.Aim.Features.VOR>();
                    foreach (var x in featureList)
                    {
                        if (x.FeatureType == FeatureType.VOR)
                        {
                            VORArr.Add((Aran.Aim.Features.VOR)x);
                        }
                    }
                    List<Aran.Aim.Features.DME> DMEArr = new List<Aran.Aim.Features.DME>();
                    foreach (var x in featureList)
                    {
                        if (x.FeatureType == FeatureType.DME)
                        {
                            DMEArr.Add((Aran.Aim.Features.DME)x);
                        }
                    }

                    List<AIP.DB.Navaid> navaidArr = new List<AIP.DB.Navaid>();
                    foreach (Aran.Aim.Features.Navaid feature in NavArr)
                    {
                        AIP.DB.Navaid navaid = new AIP.DB.Navaid();
                        navaid.Navaidhours = " ";
                        navaid.eAIP = caip;
                        //navaid.eAIPID = caip.id;
                        navaid.SubClassType = SubClassType.Navaid;
                        navaid.AIPSection = ent;
                        //navaid.ENR41id = ent.id;
                        navname = ((Aran.Aim.Features.Navaid)feature).Name;
                        var eqs = feature.NavaidEquipment.Select(n => n.TheNavaidEquipment?.Identifier).ToList();

                        IEnumerable<VOR> vors = VORArr.Where(n => eqs.Contains(n.Identifier));
                        IEnumerable<DME> dmes = DMEArr.Where(n => eqs.Contains(n.Identifier));
                        List<string> types = new List<string>();
                        List<string> freqs = new List<string>();


                        // Latitude - Point.Y
                        // Longitude - Point.X
                        foreach (VOR item in vors)
                        {
                            types.Add((item.Type != null) ? item.Type.ToString() : item.NavaidEquipmentType.ToString());
                            freqs.Add(item.Frequency.ToValString());

                            if (((Aran.Aim.Features.VOR)item).Location != null)
                            {
                                navaid.Latitude = Lib.LatToDDMMSS(((Aran.Aim.Features.VOR)item).Location?.Geo?.Y.ToString(), Lib.coordtype.DDMMSS_2, 0);
                                navaid.Longitude = Lib.LonToDDMMSS(((Aran.Aim.Features.VOR)item).Location?.Geo?.X.ToString(), Lib.coordtype.DDMMSS_2, 0);
                            }

                            navaid.Navaidname = item.Name;
                            navaid.Navaidmagneticvariation = item.MagneticVariation.ToString() + "°";
                            navaid.Navaidident = item.Designator.ToString();
                            navaid.Navaidhours = item.Availability?
                                                     .SelectManyNullSafe(x => x.TimeInterval)
                                                     .ToList()
                                                     .ToHoursOfOperations();
                        }
                        foreach (DME item in dmes)
                        {
                            types.Add((item.Type != null) ? item.Type.ToString() : item.NavaidEquipmentType.ToString());
                            // _116Y => (CH 116Y)
                            freqs.Add(item.Channel.ToString().Replace("_", "(CH ") + ")");
                            if (item.Location != null && item.Location.Elevation != null)
                            {
                                string ft = (item.Location.Elevation.Uom == UomDistanceVertical.M) ? Math.Round(Math.Round((item.Location.Elevation.Value / 0.3048) / 100, 0) * 100) + " FT" : item.Location?.Elevation?.StringValue;
                                navaid.Navaidelevation = ft;
                            }
                            navaid.Navaidhours = item.Availability
                                .SelectManyNullSafe(x => x.TimeInterval)
                                .ToList()
                                .ToHoursOfOperations();
                        }

                        string[] remarks_array = feature.Annotation.Where(n => n.Purpose != null && n.TranslatedNote != null && n.Purpose == Aran.Aim.Enums.CodeNotePurpose.REMARK).SelectManyNullSafe(n => n.TranslatedNote)?.OfType<LinguisticNote>()?.Where(t => t.Note.Lang == Aran.Aim.Enums.language.ENG).Select(t => t.Note.Value).ToArray();

                        if (types.Count > 0) navaid.Navaidtype = string.Join("/", types);
                        if (freqs.Count > 0) navaid.Navaidfrequency = string.Join(" ", freqs);
                        navaid.Navaidremarks = remarks_array;
                        navaid.NavaidListed = YesNo.Yes;
                        navaidArr.Add(navaid);
                    }
                    //ent.Children = navaidArr.ToArray();
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    // Is new or modified?
                    //db.Entry(ent).State = EntityState.Modified;
                    db.Navaid.AddRange(navaidArr.ToArray());

                    SendOutput("Saving data...", Percent: 90);
                    //db.BulkSaveChanges();
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }


        public static void Fill_ENR42(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                CodeSpecialNavigationStation? lastType = null;
                bool NewCat = false;
                List<Dictionary<string, string>> obj = new List<Dictionary<string, string>>();
                List<ForENR42> dataList = new List<ForENR42>();
                ForENR42 ForTemplate = null;
                Dictionary<string, string> subdata = null;

                List<SpecialNavigationStation> ft = Globals.GetFeaturesByED(FeatureType.SpecialNavigationStation)?.Cast<SpecialNavigationStation>()?.OrderBy(n => n.Type.ToString())?.ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    foreach (SpecialNavigationStation sns in ft)
                    {
                        subdata = new Dictionary<string, string>();
                        if (sns.Type != lastType)
                        {
                            if (!(ForTemplate == null || ForTemplate.Equals(default(ForENR42))))
                            {
                                dataList.Add(ForTemplate);
                            }

                            lastType = sns.Type;
                            NewCat = true;
                            ForTemplate = new ForENR42();
                            ForTemplate.Row = new List<Dictionary<string, string>>();
                            //ForTemplate.Category = lastType.ToString();

                        }
                        else
                        {
                            NewCat = false;
                        }

                        if (sns?.SystemChain?.Identifier != null)
                        {
                            SpecialNavigationSystem snt = Globals.GetFeaturesByED(FeatureType.SpecialNavigationSystem).Cast<SpecialNavigationSystem>().FirstOrDefault(n => n.Identifier == sns?.SystemChain?.Identifier);
                            if (ForTemplate != null) ForTemplate.name = snt?.Type.ToString();
                        }

                        subdata["snsName"] = sns.Name;
                        subdata["type"] = lastType.ToString();
                        subdata["Frequency"] = sns.Frequency?.StringValue;
                        subdata["Availability"] = "";//sns.Availability; //ToDo: common function
                        subdata["Coordinates"] = sns.Position?.Geo?.ToPointString();// Lib.LonToDDMMSS(sns.Position?.Geo.X.ToString(), Lib.coordtype.DDMMSS_2, 2) + " " + Lib.LatToDDMMSS(sns.Position?.Geo.Y.ToString(), Lib.coordtype.DDMMSS_2, 2);
                        subdata["notes"] = Lib.AIXM_GetNotes(sns.Annotation);

                        ForTemplate?.Row?.Add(subdata);
                        //output += Lib.TemplateReplace(ent.SectionName.ToString(), data);
                    }
                    if (ForTemplate != null && !ForTemplate.Equals(default(ForENR42)))
                    {
                        dataList.Add(ForTemplate);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText(ent.SectionName.ToString());
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_ENR44(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    List<DB.Designatedpoint> dpl = new List<DB.Designatedpoint>();
                    List<DesignatedPoint> flst = Globals.GetFeaturesByED(FeatureType.DesignatedPoint)?
                        .Cast<Aran.Aim.Features.DesignatedPoint>()
                            .OrderBy(n => n.Designator)
                            .ThenBy(n => n.Name)
                        .ToList();

                    var flstGuids = flst?.Select(x => x.Identifier).ToList();
                    List<Guid> air = Globals.GetFeaturesByED(FeatureType.Airspace)?
                        .Cast<Aran.Aim.Features.Airspace>()
                        .Where(x => x.Designator?.Contains("EVRR") == true)
                        .Select(x => x.Identifier)
                        .ToList();
                    List<SignificantPointInAirspace> spa = Globals.GetFeaturesByED(FeatureType.SignificantPointInAirspace)?
                        .Cast<Aran.Aim.Features.SignificantPointInAirspace>()
                        .Where(x => flstGuids?.Contains(x.Location.FixDesignatedPoint.Identifier) == true &&
                                   !air.IsNull() && air?.Contains(x.ContainingAirspace.Identifier) == true)
                        .ToList();
                    List<DepartureLeg> dl = Globals.GetFeaturesByED(FeatureType.DepartureLeg)?
                        .Cast<Aran.Aim.Features.DepartureLeg>()
                        .ToList();
                    List<ArrivalLeg> al = Globals.GetFeaturesByED(FeatureType.ArrivalLeg)?
                        .Cast<Aran.Aim.Features.ArrivalLeg>()
                        .ToList();
                    //var flst = featureList.OfType<Aran.Aim.Features.DesignatedPoint>().OrderBy(n => n.Designator).ThenBy(n => n.Name);

                    string spname = "";
                    foreach (Feature feature in flst)
                    {
                        if (feature.ToString() == "Aran.Aim.Features.DesignatedPoint")
                        {
                            DB.Designatedpoint dp = new DB.Designatedpoint();
                            dp.SubClassType = SubClassType.Designatedpoint;
                            spname = ((Aran.Aim.Features.DesignatedPoint)feature).Designator;
                            if (spname != null && ((Aran.Aim.Features.DesignatedPoint)feature).Type == CodeDesignatedPoint.ICAO)
                            {
                                //dp.id = "SP-" + spname;
                                dp.Designatedpointident = spname;
                                if (((Aran.Aim.Features.DesignatedPoint)feature).Location != null)
                                {
                                    dp.Latitude = Lib.LatToDDMMSS(((Aran.Aim.Features.DesignatedPoint)feature).Location.Geo.Y.ToString(), Lib.coordtype.DDMMSS_2, 0);
                                    dp.Longitude = Lib.LonToDDMMSS(((Aran.Aim.Features.DesignatedPoint)feature).Location.Geo.X.ToString(), Lib.coordtype.DDMMSS_2, 0);
                                }
                                else
                                {
                                    dp.Latitude = dp.Longitude = "";
                                }
                                dp.eAIPID = Lib.CurrentAIP.id;
                                dp.DesignatedpointListed = YesNo.Yes;
                                dp.AIPSection = ent;
                                var spas = spa
                                    .Where(x => x.Location.FixDesignatedPoint.Identifier == feature.Identifier)
                                    .ToList();
                                List<string> sidStars = new List<string>();
                                foreach (var sid in spas)
                                {
                                    if (sid.RelativeLocation == CodeAirspacePointPosition.IN) sidStars.Add("I");
                                    else if (sid.RelativeLocation == CodeAirspacePointPosition.BORDER)
                                    {
                                        var str = "";
                                        if (sid.Type == CodeAirspacePointRole.ENTRY) str = "E";
                                        else if (sid.Type == CodeAirspacePointRole.EXIT) str = "X";
                                        else if (sid.Type == CodeAirspacePointRole.ENTRY_EXIT) str = "E X";
                                        sidStars.Add(str);
                                    }
                                }
                                if (al?.Any(x => x.StartPoint?.PointChoice?.FixDesignatedPoint?.Identifier == feature.Identifier ||
                                                 x.EndPoint?.PointChoice?.FixDesignatedPoint?.Identifier == feature.Identifier) == true) sidStars.Add("A");
                                if (dl?.Any(x => x.StartPoint?.PointChoice?.FixDesignatedPoint?.Identifier == feature.Identifier ||
                                             x.EndPoint?.PointChoice?.FixDesignatedPoint?.Identifier == feature.Identifier) == true) sidStars.Add("D");

                                dp.SIDSTAR = string.Join(" ", sidStars);
                                dpl.Add(dp);
                            }

                        }
                    }
                    db.Designatedpoint.AddRange(dpl);
                    //db.Entry(ent).State = EntityState.Modified;
                    //ent.Children = dpl.ToArray();
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    //db.ENR44.Attach(ent);
                    //db.SaveChanges();
                    //db.BulkInsert(dpl);
                    SendOutput("Saving data...", Percent: 90);
                    //db.BulkSaveChanges();
                    db.SaveChanges();
                    //db.BulkMerge(ent);
                    transaction.Complete();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }
        public static void Fill_ENR45(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                Dictionary<string, string> subdata = null;

                List<AeronauticalGroundLight> ft = Globals.GetFeaturesByED(FeatureType.AeronauticalGroundLight)?.Cast<AeronauticalGroundLight>().ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    foreach (AeronauticalGroundLight agl in ft)
                    {
                        subdata = new Dictionary<string, string>();
                        string Intens = "";
                        string Avail = "";
                        if (agl.StructureBeacon?.Identifier != null)
                        {
                            VerticalStructure vs = Globals.GetFeaturesByED(FeatureType.VerticalStructure)?.Cast<VerticalStructure>().FirstOrDefault(n => n.Identifier == agl.StructureBeacon?.Identifier);
                            if (vs?.SupportedGroundLight?.FirstOrDefault()?.Feature?.Identifier != null)
                            {
                                RunwayProtectAreaLightSystem vs2 = Globals.GetFeaturesByED(FeatureType.RunwayProtectAreaLightSystem)?.Cast<RunwayProtectAreaLightSystem>().FirstOrDefault(n => n.Identifier == vs?.SupportedGroundLight?.FirstOrDefault()?.Feature?.Identifier);
                                Intens = vs2?.IntensityLevel.ToString();
                                Avail = Lib.GetAvailabilities(new List<PropertiesWithSchedule>(vs2?.Availability));
                            }

                        }


                        subdata["Name"] = agl.Name;
                        if (agl.Location?.Geo != null)
                        {
                            subdata["Name"] += " (" + agl.Location?.Geo.ToPointString() + ")";
                        }
                        subdata["Intensity"] = Intens;
                        subdata["Availability"] = Avail;

                        subdata["Type"] = agl.Type.ToString();
                        subdata["Color"] = agl.Colour.ToString();
                        subdata["Flashing"] = (agl.Flashing == true) ? "Yes" : "No";

                        subdata["Notes"] = Lib.AIXM_GetNotes(agl.Annotation);

                        dataList.Add(subdata);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText(ent.SectionName.ToString());
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_ENR51(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<Dictionary<string, string>> ObjList = new List<Dictionary<string, string>>();
                List<ForENR51> dataList = new List<ForENR51>();
                ForENR51 ForTemplate = null;
                Dictionary<string, string> subdata = null;


                var ftgroup = Globals.GetFeaturesByED(FeatureType.Airspace)?.Cast<Airspace>().Where(n => n.Type == CodeAirspace.D || n.Type == CodeAirspace.P || n.Type == CodeAirspace.R)
                    .OrderBy(x => x.Designator)
                    .ThenBy(x => x.Name)
                    .ToList()
                    .GroupBy(n => n.Type);

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    foreach (var ft in ftgroup)
                    {
                        ObjList = new List<Dictionary<string, string>>();
                        ForTemplate = new ForENR51();
                        ForTemplate.type = Lib.GetEnumText(ft.Key);
                        foreach (Airspace ais in ft)
                        {
                            subdata = new Dictionary<string, string>();

                            subdata["Identification"] = ais.Designator;
                            subdata["Name"] = ais.Name;
                            subdata["Coordinates"] = subdata["limit"] = "";
                            if (ais.GeometryComponent.Count > 0)
                            {
                                subdata["Coordinates"] = ais.GetHorizontalProjectionAnnotation();

                                if (subdata["Coordinates"] == null)
                                {
                                    foreach (AirspaceGeometryComponent agc in ais.GeometryComponent)
                                    {
                                        foreach (Aran.Geometries.Point pnt in agc.TheAirspaceVolume.HorizontalProjection
                                            .Geo.ToMultiPoint())
                                        {
                                            subdata["Coordinates"] +=
                                                Lib.LonToDDMMSS(pnt.X.ToString(), Lib.coordtype.DDMMSS_2, 0) + " " +
                                                Lib.LatToDDMMSS(pnt.Y.ToString(), Lib.coordtype.DDMMSS_2, 0) + " - ";
                                        }
                                    }
                                }
                                foreach (AirspaceGeometryComponent agc in ais.GeometryComponent)
                                {
                                    subdata["limit"] += agc.TheAirspaceVolume.ToUpperLower();
                                }


                            }

                            subdata["notes"] = Lib.AIXM_GetNotes(ais.Annotation);
                            ObjList.Add(subdata);
                        }
                        ForTemplate.Row = ObjList;
                        dataList.Add(ForTemplate);
                    }
                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText(ent.SectionName.ToString());
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_ENR52(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<CodeAirspace?> reqAirspaces = new List<CodeAirspace?>()
                {
                    CodeAirspace.ADIZ,
                    CodeAirspace.TSA,
                    CodeAirspace.TRA
                };
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);

                List<Airspace> airList = Globals.GetFeaturesByED(FeatureType.Airspace)
                    .Cast<Airspace>()
                    .Where(x => reqAirspaces.Contains(x.Type) && x.ControlType == CodeMilitaryOperations.MIL)
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    List<ForENR52> dataList = new List<ForENR52>();
                    foreach (Airspace air in airList.OrderBy(x => x.Designator))
                    {
                        ForENR52 data = new ForENR52();
                        data.Name = air.Name;

                        var notePoints = air.GetHorizontalProjectionAnnotation();
                        data.LateralLimits = notePoints ?? air.GeometryComponent?
                            //.Select(x => x.TheAirspaceVolume?.HorizontalProjection?.Geo.Centroid)
                            .SelectManyNullSafe(x => x.TheAirspaceVolume?.HorizontalProjection?.Geo.ToMultiPoint())
                            //.SelectManyNullSafe(x => x.TheAirspaceVolume?.HorizontalProjection?.Geo.ToExteriorRingPointList())
                            .ToList()
                            .ToPointString();
                        data.UpperLowerLimits = air.GeometryComponent?
                            .Select(x => x?.TheAirspaceVolume?.ToUpperLower())
                            .ToList()
                            .ToSeparatedValues();
                        data.SystemMeansOfActivation = Lib.AIXM_GetNotesByPurpose(air.Annotation,
                            CodeNotePurpose.OTHER_AIRSPACE_ADIZ_ACTIVATION_PROCS);
                        data.Remarks = ""; //Problem in the mapping - can`t duplicate
                        data.TimeOfActivity =
                            Lib.GetHoursOfOperations(air.Activation.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        data.RiskOfInterception = Lib.AIXM_GetNotesByPurpose(air.Annotation,
                            CodeNotePurpose.OTHER_ADIZ_INTERCEPTION_RISK);

                        dataList.Add(data);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText(ent.SectionName.ToString());
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }


                    //5.2.2 OTHER_MILITARY_EXERCISE_AND_TRAINING_AREAS_AND_ADIZ
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_MILITARY_EXERCISE_AND_TRAINING_AREAS_AND_ADIZ, 2, ref ss);

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_ENR53(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);

                List<CodeAirspace?> reqAirspaces = new List<CodeAirspace?>()
                {
                    CodeAirspace.D_OTHER // changed from D by Didzis request in the Skype
                };

                List<Airspace> airList = Globals.GetFeaturesByED(FeatureType.Airspace)
                    .Cast<Airspace>()
                    .Where(x => reqAirspaces.Contains(x.Type))
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    List<ForENR53_1> dataList = new List<ForENR53_1>();
                    foreach (Airspace air in airList.OrderBy(x => x.Designator))
                    {
                        ForENR53_1 data = new ForENR53_1();
                        data.Name = air.Name;
                        data.VerticalLimits = air.GeometryComponent
                            .Select(x => x.TheAirspaceVolume?.ToUpperLower())
                            .ToList()
                            .ToSeparatedValues();
                        data.LateralLimits = air.GeometryComponent
                            .SelectManyNullSafe(x => x.TheAirspaceVolume?.HorizontalProjection?.Geo.ToMultiPoint())
                            .ToList()
                            .ToPointString();
                        data.AdvisoryMeasures = Lib.AIXM_GetNotesByPurpose(air.Annotation,
                            CodeNotePurpose.OTHER_AIRSPACE_ADVISORY_MEASURES);

                        List<AuthorityForAirspace> afaList = Globals.GetFeaturesByED(FeatureType.AuthorityForAirspace)
                                .Cast<AuthorityForAirspace>()
                                .Where(x => x.AssignedAirspace?.Identifier == air.Identifier)
                            .ToList();
                        var oaGuid = afaList.Select(x => x.ResponsibleOrganisation?.Identifier).ToList();
                        if (oaGuid.Count > 0)
                        {
                            List<OrganisationAuthority> oaList = Globals
                                .GetFeaturesByED(FeatureType.OrganisationAuthority)
                                .Cast<OrganisationAuthority>()
                                .Where(x => oaGuid.Contains(x.Identifier))
                                .ToList();
                            data.AuthorityResponsible = oaList?.Select(x => x.Name).ToList().ToSeparatedValues();
                        }

                        data.Remarks = Lib.AIXM_GetNotes(air.Annotation); // possible problem
                        data.Remarks += Lib.GetHoursOfOperations(air.Activation.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        dataList.Add(data);
                    }

                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        Title = Lib.HeaderWithNumbering("ENR531"),
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 1
                    };
                    if (dataList.Count > 0)
                    {
                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                    }
                    else
                    {
                        ss.Content = Lib.XhtmlNil();
                    }
                    db.Subsection.Add(ss);

                    // 5.3.2 OTHER_AIP_AERO_CHART_REVISION_AMENDMENT
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_OTHER_POTENTIAL_HAZARDS, 2, ref ss);

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_ENR54(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                ForENR54 dataList = new ForENR54();
                Dictionary<string, string> subdata = null;

                List<VerticalStructure> ft = Globals.GetFeaturesByED(FeatureType.VerticalStructure)?
                    .Cast<VerticalStructure>()
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.Type)
                    .ThenBy(x => x.Identifier)
                    .ToList();
                List<Note> airNote = Globals.GetFeaturesByED(FeatureType.Airspace)?.Cast<Airspace>().Where(x => x.Type == CodeAirspace.FIR || x.Type == CodeAirspace.FIR_P && x.Annotation.Any(c => c.Purpose == CodeNotePurpose.OTHER_OBS_AREA1_ELIST_AVAILABILITY))?.FirstOrDefault()?.Annotation;

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    foreach (VerticalStructure vs in ft)
                    {
                        subdata = new Dictionary<string, string>();
                        subdata["Name"] = vs.Name;
                        subdata["Type"] = vs.Type.ToString();
                        subdata["Coordinates"] = "";
                        subdata["Elevation"] = "";
                        subdata["LightingColor"] = "";
                        subdata["LightingType"] = "";
                        int cnt = 0;
                        foreach (VerticalStructurePart vsp in vs.Part)
                        {
                            if (vsp.HorizontalProjection != null && vsp.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                            {
                                if (cnt > 0)
                                {
                                    subdata["Coordinates"] += "<br />";
                                    subdata["Elevation"] += "<br />";
                                }
                                if (vsp.HorizontalProjection?.Location?.Geo != null)
                                    subdata["Coordinates"] += vsp.HorizontalProjection?.Location?.Geo?.ToPointString();
                                if (vsp.HorizontalProjection != null)
                                    subdata["Elevation"] += vsp.HorizontalProjection?.Location?.Elevation?.StringValue;
                                if (vsp.Lighting != null && vsp.Lighting.Count > 0)
                                {
                                    subdata["LightingColor"] = vsp.Lighting?.FirstOrDefault()?.Colour?.ToString();
                                    subdata["LightingType"] = vsp.Lighting?.FirstOrDefault()?.Type?.ToString();
                                }
                                cnt++;
                            }
                        }

                        if (airNote != null)


                            dataList.Row.Add(subdata);
                    }
                    dataList.Notes = Lib.AIXM_GetNotes(airNote);
                    if (dataList != null)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText(ent.SectionName.ToString());
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        #region ENR 5.5 Classic

        // Original ENR 5.5 EC Doc mapping
        //public static void Fill_ENR55(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        //{
        //    try
        //    {

        //        Dictionary<CodeAirspaceActivity?, List<Airspace>> airGrouped = new Dictionary<CodeAirspaceActivity?, List<Airspace>>();

        //        List<RPHelper> RPHelperList =
        //            RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
        //        List<RulesProcedures> ft = GetRP(RPHelperList);
        //        // Original mapping
        //        var airActAll = Globals
        //            .GetFeaturesByED(FeatureType.Airspace)
        //            .Cast<Airspace>()
        //            .Where(x => x.Activation.Any())
        //            //.Where(x => x.Type == CodeAirspace.TRA && x.ControlType != CodeMilitaryOperations.MIL) 
        //            .OrderBy(x => x.Name)
        //            .ToList();

        //        foreach (Airspace air in airActAll)
        //        {
        //            foreach (var act in air.Activation)
        //            {
        //                if (act.Activity != null)
        //                {
        //                    if (airGrouped.ContainsKey(act.Activity))
        //                        airGrouped[act.Activity].Add(air);
        //                    else
        //                        airGrouped.Add(act.Activity, new List<Airspace>() { air });
        //                }
        //            }
        //        }

        //        var oaList = Globals
        //            .GetFeaturesByED(FeatureType.OrganisationAuthority)
        //            .Cast<OrganisationAuthority>()
        //            .ToList();

        //        var afaList = Globals
        //            .GetFeaturesByED(FeatureType.AuthorityForAirspace)
        //            .Cast<AuthorityForAirspace>()
        //            .ToList();

        //        List<ForENR55> dataList = new List<ForENR55>();
        //        List<Dictionary<string, string>> ObjList = new List<Dictionary<string, string>>();

        //        AIP.DB.Subsection ss;
        //        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            ss = new DB.Subsection();
        //            if (airGrouped.Any())
        //            {
        //                foreach (CodeAirspaceActivity? caa in airGrouped.Keys)
        //                {
        //                    List<Airspace> airList = airGrouped[caa];
        //                    ForENR55 enr55 = new ForENR55();
        //                    enr55.Activity = caa.ToString();
        //                    foreach (Airspace air in airList.OrderBy(x => x.Designator))
        //                    {
        //                        Dictionary<string, string> dic = new Dictionary<string, string>();
        //                        dic["Name"] = air.Name;
        //                        dic["VerticalLimits"] = air.Class.SelectManyNullSafe(x => x.AssociatedLevels.Select(ix => ix.UpperLimit?.ToValString(ix.UpperLimitReference) + " / " + ix.LowerLimit?.ToValString(ix.LowerLimitReference)))
        //                            .ToList()
        //                            .ToSeparatedValues();

        //                        var oaGuid = afaList.Where(x => x.AssignedAirspace?.Identifier == air.Identifier).Select(x => x.ResponsibleOrganisation?.Identifier).ToList();
        //                        dic["OperatorPhone"] = "";
        //                        if (oaGuid.Count > 0)
        //                        {
        //                            List<OrganisationAuthority> oaList2 = oaList
        //                                .Where(x => oaGuid.Contains(x.Identifier))
        //                                .ToList();
        //                            dic["OperatorPhone"] = oaList2.Select(x =>
        //                                x.Name + " " +
        //                                x.Contact.SelectManyNullSafe(ox => ox.PhoneFax)
        //                                    .Select(ix => ix.Voice)
        //                                    .ToList()
        //                                    .ToSeparatedValues())
        //                                .ToList()
        //                                .ToSeparatedValues();
        //                        }

        //                        dic["Remarks"] = Lib.AIXM_GetNotes(air.Annotation); // possible problem
        //                        //dic["Remarks"] +=
        //                        //    Lib.GetHoursOfOperations(air.Activation.SelectManyNullSafe(x => x.TimeInterval).ToList());
        //                        ObjList.Add(dic);

        //                    }
        //                    enr55.Row = ObjList;
        //                    dataList.Add(enr55);
        //                }
        //                if (dataList.Count > 0)
        //                {
        //                    ss = new DB.Subsection();
        //                    ss.AIPSection = ent;
        //                    ss.Title = Lib.GetText(ent.SectionName.ToString());
        //                    ss.eAIP = caip;
        //                    ss.SubClassType = SubClassType.Subsection;
        //                    ss.OrderNumber = 1;

        //                    string output = Razor.Run(dataList);
        //                    if (output == null)
        //                    {
        //                        SendOutput("Error in generating template ", Percent: 80);
        //                        return;
        //                    }
        //                    ss.Content = output;
        //                    db.Subsection.Add(ss);
        //                }

        //                // 5.5 OTHER_AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES
        //                InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES, 2, ref ss);

        //                ent.SectionStatus = SectionStatusEnum.Filled;
        //                SendOutput("Saving data...", Percent: 90);
        //                db.SaveChanges();
        //                transaction.Complete();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return;
        //    }
        //}
        #endregion

        // Mapping by Didzis Dobelis LGS, original mapping above
        public static void Fill_ENR55(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                Dictionary<CodeAirspace?, List<Airspace>> airGrouped = new Dictionary<CodeAirspace?, List<Airspace>>();

                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);
                var airActAll = Globals
                    .GetFeaturesByED(FeatureType.Airspace)
                    .Cast<Airspace>()
                    .Where(x => x.Type == CodeAirspace.TRA && x.ControlType != CodeMilitaryOperations.MIL)
                    .OrderBy(x => x.Name)
                    .ToList();

                foreach (Airspace air in airActAll)
                {
                    if (airGrouped.ContainsKey(air.Type))
                        airGrouped[air.Type].Add(air);
                    else
                        airGrouped.Add(air.Type, new List<Airspace>() { air });
                }

                var oaList = Globals
                    .GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .Cast<OrganisationAuthority>()
                    .ToList();

                var afaList = Globals
                    .GetFeaturesByED(FeatureType.AuthorityForAirspace)
                    .Cast<AuthorityForAirspace>()
                    .ToList();

                List<ForENR55> dataList = new List<ForENR55>();
                List<Dictionary<string, string>> ObjList = new List<Dictionary<string, string>>();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    if (airGrouped.Any())
                    {
                        foreach (CodeAirspace? caa in airGrouped.Keys)
                        {
                            List<Airspace> airList = airGrouped[caa];
                            ForENR55 enr55 = new ForENR55();
                            enr55.Activity = Lib.GetEnumText(caa);
                            foreach (Airspace air in airList.OrderBy(x => x.Name, new NumericComparer()))
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic["Name"] = air.Name;
                                dic["VerticalLimits"] = air.GeometryComponent?.FirstOrDefault()?.TheAirspaceVolume?.ToUpperLower();
                                var oa = air.Activation?.FirstOrDefault(x => x.User?.Count > 0)?
                                    .User?.FirstOrDefault()?.Feature.Identifier;
                                dic["OperatorPhone"] = "";
                                if (oa != null)
                                {
                                    List<OrganisationAuthority> oaList2 = oaList
                                        .Where(x => x.Identifier == oa)
                                        .ToList();
                                    dic["OperatorPhone"] = oaList2.Select(x =>
                                            x.Name + "<br/>" +
                                            x.Contact.SelectManyNullSafe(ox => ox.PhoneFax)
                                                .Select(ix => ix.Voice)
                                                .ToList()
                                                .ToSeparatedValues())
                                        .ToList()
                                        .ToSeparatedValues();

                                }
                                dic["Remarks"] = Lib.AIXM_GetNotes(air.Annotation); // possible problem
                                                                                    //dic["Remarks"] +=
                                                                                    //    Lib.GetHoursOfOperations(air.Activation.SelectManyNullSafe(x => x.TimeInterval).ToList());
                                ObjList.Add(dic);

                            }
                            enr55.Row = ObjList;
                            dataList.Add(enr55);
                        }
                        if (dataList.Count > 0)
                        {
                            ss = new DB.Subsection();
                            ss.AIPSection = ent;
                            ss.Title = Lib.GetText(ent.SectionName.ToString());
                            ss.eAIP = caip;
                            ss.SubClassType = SubClassType.Subsection;
                            ss.OrderNumber = 1;

                            string output = Razor.Run(dataList);
                            if (output == null)
                            {
                                SendOutput("Error in generating template ", Percent: 80);
                                return;
                            }
                            ss.Content = output;
                            db.Subsection.Add(ss);
                        }

                        // 5.5 OTHER_AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES
                        InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES, 2, ref ss);

                        ent.SectionStatus = SectionStatusEnum.Filled;
                        SendOutput("Saving data...", Percent: 90);
                        db.SaveChanges();
                        transaction.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }
        public static void Fill_ENR6(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                Fill_FileSection(ent, featureList, caip);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

    }

}