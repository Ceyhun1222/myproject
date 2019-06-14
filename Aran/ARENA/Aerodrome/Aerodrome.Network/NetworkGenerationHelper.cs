using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using Framework.Stasy.Context;
using Framework.Stasy;
using Aerodrome.Features;
using System.Reflection;
using System.Collections;
using Framework.Stasy.Core;

namespace Aerodrome.Network
{
    public struct ClPoint
    {
        public string Name;
        public IPoint pt;
        public string FeatureType;
        public string IdNetwork;
    }

    public struct Edge
    {
        public string Name;
        public IPolyline pLine;
        public double? WingSpan;
        public string pcn;
    }
    public class NetworkGenerationHelper
    {
       
        private List<Edge> pEdge;
        private List<Edge> pPCN;       
        private List<ClPoint> _pts;

        private Polyline PL;
        private Polyline[] NewPoly;
        

        public void BubbleSortList(List<ClPoint> intArray)
        {
            for (int i = intArray.Count - 1; i > 0; i--)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    if (intArray[j].pt.X > intArray[j + 1].pt.X)
                    {
                        IClone clone = intArray[j].pt as IClone;
                        var highValue = clone.Clone() as IPoint;
                        var name = intArray[j].Name;
                        var featType = intArray[j].FeatureType;
                        var IdNet = intArray[j].IdNetwork;

                        intArray[j] = intArray[j + 1];
                        intArray[j + 1] = new ClPoint()
                        {
                            pt = highValue,
                            Name = name,
                            FeatureType = featType,
                            IdNetwork = IdNet
                        };
                    }
                }
            }
        }

        public List<ClPoint> ExtractNode(Type featType, string NodeType, string featName, string IdNetField)
        {
           
            var featureCollection = ((IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[featType]);

            int n = featureCollection.Count();

            System.Reflection.PropertyInfo[] propInfos = featType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(prop => prop.FullName == typeof(IGeometry).FullName) != null);

            var NetField = propInfos.First(p => p.Name.Equals(IdNetField));


            PL = new ESRI.ArcGIS.Geometry.Polyline();

            NewPoly = new ESRI.ArcGIS.Geometry.Polyline[n];
            var PtAll = new List<ClPoint>();
            string IdNet;
            
            foreach (var feature in featureCollection)
            {
                PL = geoProp.GetValue(feature) as Polyline;

                if (NetField.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
                {
                    var nilReasonPropInfo = NetField.PropertyType.GetProperty(nameof(AM_Nullable<Type>.NilReason));
                    var valuePropInfo = NetField.PropertyType.GetProperty(nameof(AM_Nullable<Type>.Value));
                    var netFieldValue = NetField.GetValue(feature);

                    if (nilReasonPropInfo.GetValue(netFieldValue) != null)
                        IdNet = null;
                    else
                        IdNet = valuePropInfo.GetValue(netFieldValue) as string;
                }
                else
                {
                    if (NetField.GetValue(feature) is null || (NetField.GetValue(feature) as string).Equals(String.Empty))
                        IdNet = null;
                    else
                        IdNet = NetField.GetValue(feature) as string;
                }
                    

                //TODO: Сначала проверить на NilReason.
                PtAll.Add(new ClPoint() { pt = PL.Point[0], Name = IdNet, FeatureType = featName });// = ;
                PtAll.Add(new ClPoint() { pt = PL.Point[PL.PointCount - 1], Name = IdNet, FeatureType = featName });
            }
            var result = new List<ClPoint>();
            if (PtAll.Count == 0)
                return result;
            BubbleSortList(PtAll);
            
            result.Add(new ClPoint() { pt = PtAll[0].pt, Name = NodeType, FeatureType = featName, IdNetwork = PtAll[0].Name });
            ILine pLine = new ESRI.ArcGIS.Geometry.Line();
            double L;
            int i = 1;
            n = PtAll.Count;
            pLine.FromPoint = PtAll[0].pt;
           
            while (i < n)
            {
                pLine.ToPoint = PtAll[i].pt;
                L = pLine.Length;
            
                if (L > 0.00000001)
                {
                    result.Add(new ClPoint() { pt = PtAll[i].pt, Name = NodeType, FeatureType = featName, IdNetwork = PtAll[i].Name });
                    pLine.FromPoint = PtAll[i].pt;
                }

                i++;
            }
           
            return result;
        }

        public List<ClPoint> ExtractNodePoint(Type featType, string NodeType, string featName, string IdNetField)
        {
            var featureCollection = ((IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[featType]);

            int n = featureCollection.Count();

            System.Reflection.PropertyInfo[] propInfos = featType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(prop => prop.FullName == typeof(IGeometry).FullName) != null);

            var NetField = propInfos.First(p => p.Name.Equals(IdNetField));

           
            var res = new List<ClPoint>();
           
            IPoint Pt = new ESRI.ArcGIS.Geometry.Point();
            string str;
           
            foreach (var feature in featureCollection)
            {
                Pt = geoProp.GetValue(feature) as ESRI.ArcGIS.Geometry.Point;

                if (NetField.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
                {
                    var nilReasonPropInfo = NetField.PropertyType.GetProperty(nameof(AM_Nullable<Type>.NilReason));
                    var valuePropInfo = NetField.PropertyType.GetProperty(nameof(AM_Nullable<Type>.Value));
                    var netFieldValue = NetField.GetValue(feature);
                    if (netFieldValue != null)
                        {
                            if (nilReasonPropInfo.GetValue(netFieldValue) != null)
                                str = null;
                            else
                                str = valuePropInfo.GetValue(netFieldValue) as string;
                        }
                    else
                        str = null;
                }
                else
                {
                    if (NetField.GetValue(feature) is null || (NetField.GetValue(feature) as string).Equals(String.Empty))
                        str = null;
                    else
                        str = NetField.GetValue(feature) as string;
                }


                res.Add(new ClPoint() { pt = Pt, Name = NodeType, FeatureType = featName, IdNetwork = str });
            }
          
            return res;
        }

        public List<ClPoint> ReplaceNodeName(List<ClPoint> List1, List<ClPoint> List2, string NodeType, string FeatType)
        {
            List1.AddRange(List2);
            BubbleSortList(List1);
            ILine pLine = new ESRI.ArcGIS.Geometry.Line();
            double L;
            int i = 1;
            while (i < List1.Count)           
            {
                pLine.FromPoint = List1[i - 1].pt;
                pLine.ToPoint = List1[i].pt;
                L = pLine.Length;


                if (L < 0.00000001)
                {
                    ClPoint tmpPt = List1[i - 1];
                    tmpPt.Name = NodeType;
                    tmpPt.FeatureType = FeatType;
                    tmpPt.IdNetwork = List1[i - 1].IdNetwork;
                    List1[i - 1] = tmpPt;

                    List1.Remove(List1[i]);
                }
                else
                    i++;
            }

            return List1;
        }
        public List<ClPoint> ReplaceNodeNameByArea(List<ClPoint> List1, Type featType, string NodeType, string featName, string IdNetField)
        {
            var featureCollection = ((IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[featType]);

            int n = List1.Count;
            //find Name field

            System.Reflection.PropertyInfo[] propInfos = featType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(prop => prop.FullName == typeof(IGeometry).FullName) != null);

            var NetField = propInfos.First(p => p.Name.Equals(IdNetField));

            var Poly = (IPolygon)new ESRI.ArcGIS.Geometry.Polygon();
           
            IProximityOperator pProxi;
            int i;
            IGeometry pGeo;
            double d;
            string str;
           
            foreach (var feature in featureCollection)
            {
                Poly = geoProp.GetValue(feature) as ESRI.ArcGIS.Geometry.IPolygon;

                if (NetField.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
                {
                    var nilReasonPropInfo = NetField.PropertyType.GetProperty(nameof(AM_Nullable<Type>.NilReason));
                    var valuePropInfo = NetField.PropertyType.GetProperty(nameof(AM_Nullable<Type>.Value));
                    var netFieldValue = NetField.GetValue(feature);

                    if (nilReasonPropInfo.GetValue(netFieldValue) != null)
                        str = null;
                    else
                        str = valuePropInfo.GetValue(netFieldValue) as string;
                }
                else
                {
                    if (NetField.GetValue(feature) is null || (NetField.GetValue(feature) as string).Equals(String.Empty))
                        str = null;
                    else
                        str = NetField.GetValue(feature) as string;
                }


                pProxi = Poly as ESRI.ArcGIS.Geometry.IProximityOperator;

                for (i = 0; i < n; i++)
                {
                    pGeo = List1[i].pt;
                   
                    d = pProxi.ReturnDistance(pGeo);
                    if (d == 0.0)
                    {
                        ClPoint tmpPt = List1[i];
                        tmpPt.Name = NodeType;
                        tmpPt.FeatureType = featName;
                        tmpPt.IdNetwork = str;
                        List1[i] = tmpPt;
                    }

                }
            }
           
            return List1;
        }

        public void GenerateNode()
        {
            var lst1 = ExtractNode(typeof(AM_TaxiwayGuidanceLine), "Taxiway", "Taxiway_Guidance_Line", "idlin"); //Node_where_two_taxiways_meet
            var lst2 = ExtractNode(typeof(AM_StandGuidanceLine), "TaxiwayLink", "Stand_Guidance_Line", "idstd");//????????????Taxiway Link//Node_on_a_taxiway_abeam_to_a_Parking_or_Apron_Entry_Exit_node
            //var lst3 = ExtractNode(RunwayLayer, "RunwayExitLine", "Runway_ExitLine","idlin");//Node_where_a_runway_exit_line_and_runway_intersection_meet
            var lst3 = ExtractNode(typeof(AM_RunwayExitLine), "RunwayExitLine", "Runway_Element", "idrwy");//Node_where_a_runway_exit_line_and_runway_intersection_meet
            ////Node_where_a_runway_and_taxiway_meet
            ////Node_on_taxiway_holding_position
            //Node_joining_a_parking_area_to_a_taxiway
            ////Node_on_a_taxiway_abeam_to_a_Parking_or_Apron_Entry_Exit_node
            ////Node_where_a_stand_is_located
            //Node where a deicing operation maybe performed
            lst1 = ReplaceNodeName(lst1, lst2, "ApronEntryExit", "Apron_Element");//Node_joining_an_apron_area_to_a_taxiway
                                                                                  //            lst1 = ReplaceNodeName(lst1, lst3, "RunwayEntryExit", "Runway_Element");//Node_where_a_runway_and_taxiway_meet
            lst1 = ReplaceNodeName(lst1, lst3, "RunwayEntryExit", "Runway_ExitLine");//Node_where_a_runway_and_taxiway_meet

            lst1 = ReplaceNodeNameByArea(lst1, typeof(AM_ParkingStandArea), "ParkingEntryExit", "Parking_Stand_Area", "idstd");
            lst1 = ReplaceNodeNameByArea(lst1, typeof(AM_DeicingArea), "Deicing", "Deicing_Area", "idbase");


            lst2 = ExtractNodePoint(typeof(AM_PositionMarking), "TaxiwayHoldingPosition", "Taxiway_Holding_Position", "idpm");//Node_on_taxiway_holding_position
            lst1 = ReplaceNodeName(lst1, lst2, "TaxiwayHoldingPosition", "Taxiway_Holding_Position");//Node_on_taxiway_holding_position

            lst2 = ExtractNodePoint(typeof(AM_ParkingStandLocation), "Stand", "Parking_Stand_Location", "idstd");//Node_where_a_stand_is_located
            lst1 = ReplaceNodeName(lst1, lst2, "Stand", "Parking_Stand_Location");//Node_where_a_stand_is_located
                                                                                  //            lst1 = ReplaceNodeName(lst2, lst1, "Parking_Stand_Location", "Stand");//Node_where_a_stand_is_located

            var nodeFeatureCollection = ((CompositeCollection<AM_AsrnNode>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnNode)]);


            var thrFeatureCollection = ((CompositeCollection<AM_RunwayThreshold>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_RunwayThreshold)]);

            System.Reflection.PropertyInfo[] propInfos = typeof(AM_RunwayThreshold).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(prop => prop.FullName == typeof(IGeometry).FullName) != null);

            var thrIdProp = propInfos.First(p => p.Name.Equals(nameof(AM_RunwayThreshold.idthr)));

            List<IPoint> ThrPnt = new List<IPoint>();
            List<string> ThrID = new List<string>();

            foreach (var feature in thrFeatureCollection)
            {
                ThrPnt.Add(geoProp.GetValue(feature) as ESRI.ArcGIS.Geometry.Point);
                ThrID.Add(thrIdProp.GetValue(feature) as string);
            }

            IPoint Pt = new ESRI.ArcGIS.Geometry.Point();
            double d;
            double d1;
            string IDThr = "";
            int k = thrFeatureCollection.Count();
            int index = 1;
            for (int i = 0; i < lst1.Count; i++)
            {
                AM_AsrnNode node = new AM_AsrnNode();
                var nodeTypeVal = Enum.Parse(typeof(Enums.Node_Type), lst1[i].Name);
                node.nodetype = new AM_Nullable<Enums.Node_Type> { Value = (Enums.Node_Type)nodeTypeVal };
                node.geopnt = lst1[i].pt;
                if (lst1[i].IdNetwork != null)
                    node.idnetwrk = new AM_Nullable<string> { Value = lst1[i].IdNetwork, NilReason = null };
                else
                    node.idnetwrk = new AM_Nullable<string> { NilReason = Enums.NilReason.NotEntered };

                node.featureID = Guid.NewGuid().ToString();
                node.idnumber = index.ToString();
                index++;

                d = double.MaxValue;
                // ID THR

                for (int j = 0; j < ThrPnt.Count; j++)
                {
                    Pt = ThrPnt[j];
                    d1 = (Pt.X - lst1[i].pt.X) * (Pt.X - lst1[i].pt.X) + (Pt.Y - lst1[i].pt.Y) * (Pt.Y - lst1[i].pt.Y);
                    if (d1 < d) IDThr = ThrID[j];
                    d = d1;
                }
                //////////////////// End
                node.idthr = IDThr;
                AerodromeDataCash.ProjectEnvironment.Context.PrepareEntity<AM_AsrnNode>(node, false);//

                AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnNode)].Add(node);

            }

            // Save Node to Edge
            //Node          

            List<IPoint> NodePnt = new List<IPoint>();
            List<string> NodeID = new List<string>();
            List<string> NodeType = new List<string>();
            List<string> NodeNet = new List<string>();
            foreach (var node in nodeFeatureCollection)
            {
                IClone clone = node.geopnt as IClone;
                NodePnt.Add(clone.Clone() as ESRI.ArcGIS.Geometry.Point);
                NodeID.Add(node.idnumber as string);
                if (node.nodetype.NilReason is null)
                    NodeType.Add(node.nodetype.Value.ToString() as string);
                else
                    NodeType.Add("");

                if (node.idnetwrk.NilReason is null)
                    NodeNet.Add(node.idnetwrk.Value as string);
                else
                    NodeNet.Add("");
                //TODO: Nullable нужно по другому обработать. Сначала проверить NilReason.
            }

            var edgeFeatureCollection = ((CompositeCollection<AM_AsrnEdge>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnEdge)]);

            //Edge                     
            IPolyline Pl;

            string s1 = "?", s2 = "?", Net1 = "", Net2 = "", Net = "";
            double a, b;

            foreach (var edge in edgeFeatureCollection)
            {
                Pl = edge.geoline as ESRI.ArcGIS.Geometry.IPolyline;
                k = 0;
                for (int j = 0; j < NodePnt.Count; j++)
                {
                    a = (Pl.FromPoint.X - NodePnt[j].X) * (Pl.FromPoint.X - NodePnt[j].X);
                    b = (Pl.FromPoint.Y - NodePnt[j].Y) * (Pl.FromPoint.Y - NodePnt[j].Y);
                    if (Math.Sqrt(a + b) < 0.0000001)
                    {
                        edge.node1ref = NodeID[j];
                        s1 = NodeType[j];
                        Net1 = NodeNet[j];
                        k = k + 1;
                    }

                    a = (Pl.ToPoint.X - NodePnt[j].X) * (Pl.ToPoint.X - NodePnt[j].X);
                    b = (Pl.ToPoint.Y - NodePnt[j].Y) * (Pl.ToPoint.Y - NodePnt[j].Y);
                    if (Math.Sqrt(a + b) < 0.0000001)
                    {
                        edge.node2ref = NodeID[j];
                        s2 = NodeType[j];
                        Net2 = NodeNet[j];
                        k = k + 1;
                    }

                    if (k == 2) break;
                }
                string res = "";
                switch (s1 + s2)
                {
                    // Taxiway
                    case "TaxiwayTaxiway":
                        res = "Taxiway";
                        break;
                    case "TaxiwayTaxiwayHoldingPosition":
                        res = "Taxiway";
                        break;
                    case "TaxiwayHoldingPositionTaxiway":
                        res = "Taxiway";
                        break;
                    case "TaxiwayApronEntryExit":
                        res = "Taxiway";
                        break;
                    case "ApronEntryExitTaxiway":
                        res = "Taxiway";
                        break;
                    //Apron

                    case "TaxiwayLinkTaxiwayLink":
                        res = "Apron";
                        break;
                    case "TaxiwayLinkApronEntryExit":
                        res = "Apron";
                        break;
                    case "ApronEntryExitTaxiwayLink":
                        res = "Apron";
                        break;

                    //Deicing
                    case "TaxiwayLinkDeicing":
                        res = "Deicing";
                        break;
                    case "DeicingTaxiwayLink":
                        res = "Deicing";
                        break;
                    case "DeicingDeicing":
                        res = "Deicing";
                        break;
                    case "TaxiwayDeicing":
                        res = "Deicing";
                        break;
                    case "DeicingTaxiway":
                        res = "Deicing";
                        break;
                    case "DeicingStand":
                        res = "Deicing";
                        break;
                    case "StandDeicing":
                        res = "Deicing";
                        break;
                    case "DeicingParkingEntryExit":
                        res = "Deicing";
                        break;
                    case "ParkingEntryExitDeicing":
                        res = "Deicing";
                        break;

                    //Runway
                    case "RunwayExitLineRunwayExitLine":
                        res = "Runway";
                        break;
                    case "RunwayEntryExitRunwayExitLine":
                        res = "Runway";
                        break;
                    case "RunwayExitLineRunwayEntryExit":
                        res = "Runway";
                        break;

                    //Exit
                    case "RunwayEntryExitTaxiway":
                        res = "Runway_Exit";
                        break;
                    case "TaxiwayRunwayEntryExit":
                        res = "Runway_Exit";
                        break;
                    //Stand
                    case "StandParkingEntryExit":
                        res = "Stand";
                        break;
                    case "ParkingEntryExitStand":
                        res = "Stand";
                        break;
                    case "StandTaxiwayLink":
                        res = "Stand";
                        break;
                    case "TaxiwayLinkStand":
                        res = "Stand";
                        break;
                    case "ParkingEntryExitTaxiwayLink":
                        res = "Parking";
                        break;
                    case "TaxiwayLinkParkingEntryExit":
                        res = "Parking";
                        break;
                    case "ParkingEntryExitParkingEntryExit":
                        res = "Parking";
                        break;
                }

                if (res != String.Empty)
                {
                    var edgeTypeVal = Enum.Parse(typeof(Enums.EdgeType), res);
                    edge.edgetype = new AM_Nullable<Enums.EdgeType> { Value = (Enums.EdgeType)edgeTypeVal, NilReason = null };
                }

                if (Net1 == null) Net1 = "";
                if (Net2 == null) Net2 = "";

                if (Net1 != "")
                {
                    if (Net2 != "")
                    {
                        Net = Net1 + "_" + Net2;
                        if (Net1 == Net2) Net = Net1;
                    }
                    else Net = Net1;
                }
                else Net = Net2;

                if (!Net.Equals(String.Empty))
                    edge.idnetwrk = new AM_Nullable<string> { Value = Net, NilReason = null };

                Net1 = "";
                Net2 = "";

            }

        }

        public void GenerateEdge()
        {
                
            pEdge = new List<Edge>();
            pPCN = new List<Edge>();
            double L, ang, Lmax;
            IGeometry pGeo;
            Lmax = 0;
            string pcn;

            //Taxiway Element GL
            var taxiwayElemFeatureCollection = ((CompositeCollection<AM_TaxiwayElement>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_TaxiwayElement)]);

            foreach(var taxiElem in taxiwayElemFeatureCollection)
            {
                pPCN.Add(new Edge() { Name = taxiElem.idlin as string, pcn = taxiElem.pcn?.Value});
            }

            var taxiGuidanceFeatureCollection = ((CompositeCollection<AM_TaxiwayGuidanceLine>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_TaxiwayGuidanceLine)]);
            foreach(var guidLine in taxiGuidanceFeatureCollection)
            {
                pcn = "";
                for (int i = 0; i < pPCN.Count; i++)
                {

                    if (guidLine.idlin.Value as string == pPCN[i].Name)
                    {
                        pcn = pPCN[i].pcn;
                        break;
                    }

                }
                PL = guidLine.geoline as Polyline;
                L = 0;
                if (guidLine.wingspan != null)
                    L = guidLine.wingspan.Value;
                pEdge.Add(new Edge() { Name = "Taxiway_Guidance_Line" as string, pLine = PL as ESRI.ArcGIS.Geometry.IPolyline, WingSpan = L, pcn = pcn });                
                if (L > Lmax) Lmax = L;
            }
           
        
            pPCN = new List<Edge>();
           
            var standAreaFeatureCollection = ((CompositeCollection<AM_ParkingStandArea>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_ParkingStandArea)]);

            foreach(var standArea in standAreaFeatureCollection)
            {
                pPCN.Add(new Edge() { Name = standArea.idstd, pcn = standArea.pcn?.Value });
            }

            var standGuidanceFeatureCollection = ((CompositeCollection<AM_StandGuidanceLine>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_StandGuidanceLine)]);

            foreach(var standGuidLine in standGuidanceFeatureCollection)
            {
                pcn = "";

                for (int i = 0; i < pPCN.Count; i++)

                {
                    if (standGuidLine.idstd != null)
                    {
                        if (standGuidLine.idstd  == pPCN[i].Name)
                        {
                            pcn = pPCN[i].pcn;
                            break;
                        }
                    }
                }

                PL = standGuidLine.geoline as Polyline;
                if (standGuidLine.wingspan != null)
                {
                    L = Convert.ToDouble(standGuidLine.wingspan.Value);
                    pEdge.Add(new Edge() { Name = "Stand_Guidance_Line" as string, pLine = PL as ESRI.ArcGIS.Geometry.IPolyline, WingSpan = L, pcn = pcn });
                }
                else pEdge.Add(new Edge() { Name = "Stand_Guidance_Line" as string, pLine = PL as ESRI.ArcGIS.Geometry.IPolyline, pcn = pcn });
            }


            var rwyElemFeatureCollection = ((CompositeCollection<AM_RunwayElement>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_RunwayElement)]);

            L = 0;
            pcn = "";

            foreach (var rwyElem in rwyElemFeatureCollection)
            {
                if (rwyElem.length != null)
                {
                    if (Convert.ToDouble(rwyElem.length.Value) > L)
                    {
                        if(rwyElem.pcn.NilReason is null)
                            pcn = rwyElem.pcn.Value as string;
                    }
                    L = Convert.ToDouble(rwyElem.length.Value);
                    //TODO: Всегда конвертировать в метры
                }
               
            }

            var rwyExitLineFeatureCollection = ((CompositeCollection<AM_RunwayExitLine>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_RunwayExitLine)]);

            foreach (var rwyExitLine in rwyExitLineFeatureCollection)
            {
                PL = rwyExitLine.geoline as Polyline;
                pEdge.Add(new Edge() { Name = "Runway_ExitLine" as string, pLine = PL as ESRI.ArcGIS.Geometry.IPolyline, WingSpan = Lmax, pcn = pcn });
            }
            int index = 1;
            List<AM_AsrnEdge> edges = new List<AM_AsrnEdge>();
            for (int i = 0; i < pEdge.Count; i++)
            {
                AM_AsrnEdge feature = new AM_AsrnEdge();
                feature.geoline = pEdge[i].pLine;
                if (pEdge[i].WingSpan != null)
                    feature.wingspan = new DataType.DataType<Enums.UomDistance> {Value= pEdge[i].WingSpan.Value };

                if (pEdge[i].pcn is null || pEdge[i].pcn == String.Empty)
                    feature.pcn = new AM_Nullable<string> { NilReason = Enums.NilReason.NotEntered };
                else
                    feature.pcn = new AM_Nullable<string> { Value = pEdge[i].pcn };
                feature.direc = new AM_Nullable<Enums.AM_Direction> { Value= Enums.AM_Direction.Bidirectional };

                if (pEdge[i].Name != null && pEdge[i].Name != String.Empty)
                    feature.idbase = new AM_Nullable<string> { Value = pEdge[i].Name };
                else
                    feature.idbase = new AM_Nullable<string> { NilReason = Enums.NilReason.NotEntered };
                

                pGeo = pEdge[i].pLine as ESRI.ArcGIS.Geometry.IGeometry;
                pGeo.Project(AerodromeDataCash.ProjectEnvironment.pMap.SpatialReference);                

                IPolyline pLine = pGeo as ESRI.ArcGIS.Geometry.IPolyline;
                L = pLine.Length;

                ISpatialReferenceFactory srFactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference geoSpatialRef = srFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                pGeo.Project(geoSpatialRef);
                IPointCollection PtColl = pLine as ESRI.ArcGIS.Geometry.IPointCollection;
                ILine pLine1 = new ESRI.ArcGIS.Geometry.Line();
                ILine pLine2 = new ESRI.ArcGIS.Geometry.Line();
                pLine1.FromPoint = PtColl.Point[0];
                pLine1.ToPoint = PtColl.Point[1];

                pLine2.FromPoint = PtColl.Point[PtColl.PointCount - 2];
                pLine2.ToPoint = PtColl.Point[PtColl.PointCount - 1];

                ang = Math.Abs(pLine1.Angle - pLine2.Angle) * 180.0 / Math.PI;
                //edited
                if (ang > 180) ang = 360 - ang;
                L = Math.Round(L, 1);
                ang = Math.Round(ang);
                feature.curvatur = new DataType.DataType<Enums.UomBearing> { Value = ang };
                feature.edgelen = new DataType.DataType<Enums.UomDistance> { Value = L };
                feature.edgederv = new AM_Nullable<Enums.Edgederv> { Value = Enums.Edgederv.Derived };
                //feature.edgetype = new AM_Nullable<Enums.EdgeType> { NilReason = Enums.NilReason.NotEntered };
                //feature.idnetwrk = new AM_Nullable<string> { NilReason = Enums.NilReason.NotEntered };
                feature.idnumber = index.ToString();
                feature.featureID = Guid.NewGuid().ToString();
                index++;
                //TODO: ang, L и тд возможно надо сделать nullable
                edges.Add(feature);

                AerodromeDataCash.ProjectEnvironment.Context.PrepareEntity<AM_AsrnEdge>(feature, false);//
                AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnEdge)].Add(feature);
            }
          
        }

        public void CompactDatabase(IWorkspace workspace)
        {
            //The following example show how to use the IDatabaseCompact interface to compact a File or personal geodatabase.
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
            // Stop the edit operation.  
            workspaceEdit.StopEditOperation();

            // Stop the edit session.  
            workspaceEdit.StopEditing(true);

            IDatabaseCompact databaseCompact;

            databaseCompact = (IDatabaseCompact)workspace;
            if (databaseCompact.CanCompact())
            {
                databaseCompact.Compact();
            }

        }

    }
}
