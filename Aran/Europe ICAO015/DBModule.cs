using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using Aran.AranEnvironment;
using System.Globalization;
using Aran.PANDA.Conventional.Racetrack;
using ChoosePointNS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using Aran.AranEnvironment.Symbols;
using MahApps.Metro.Controls;
using Aran.Aim.Enums;
using Aran.Queries.Common;
using Aran.Queries;
using Aran.Queries.Omega;
using Aran.Geometries.Operators;
using System.Reflection;
using System.Reflection.Emit;
using Aran.Omega.Properties;
using Aran.Omega.View;
using System.Threading;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Converters;
using Aran.Queries.Panda_2;
using Aran.PANDA.Departure;
using Aran.Aim.Objects;

namespace Europe_ICAO015
{
    public class DBModule
    {
        public List<ObstacleArea> ObstacleAreaList = new List<ObstacleArea>();
        public List<DME> DMELists = new List<DME>();
        public List<Localizer> LocList = new List<Localizer>();
        public List<Glidepath> GlidePathList = new List<Glidepath>();
        public static List<Navaid> NavaidLists = new List<Navaid>();
        public List<VOR> VORlistsAll = new List<VOR>();
        public List<string> VORlistsForCvor = new List<string>();
        public List<string> VORlistsForDvor = new List<string>();
        public static List<VerticalStructure> VRTsList = new List<VerticalStructure>();
        public List<NDB> NDBLists = new List<NDB>();
        public List<MarkerBeacon> MarkersList = new List<MarkerBeacon>();
        public List<Runway> RunwayList = new List<Runway>();
        public List<RunwayDirection> RunwayDirList = new List<RunwayDirection>();
        public List<RunwayCentrelinePoint> RunwayCentreLinePoint = new List<RunwayCentrelinePoint>();
        public DBModule()
        {
            DMELists.Clear();
            VORlistsAll.Clear();
            NavaidLists.Clear();
            GlidePathList.Clear();
            LocList.Clear();
            VRTsList.Clear();
            NDBLists.Clear();
            MarkersList.Clear();
            RunwayList.Clear();
            RunwayDirList.Clear();
            RunwayCentreLinePoint.Clear();

            VORlistsForCvor.Clear();
            VORlistsForDvor.Clear();

        }
        public void ICAO_OnLoad()
        {
            //AirspaceList.Clear();
            try
            {
                var slotSelector = true;

                var dbProvider = Aran.Omega.GlobalParams.AranEnvironment.DbProvider as DbProvider;
                if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
                {
                    dynamic methodResult = new System.Dynamic.ExpandoObject();
                    dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                    slotSelector = methodResult.Result;
                }

                if (!slotSelector)
                {
                    MessageBox.Show("Please first select slot!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Aran.Omega.InitOmega.InitCommand()) return;


                OmegaQPI = OmegaQpiFactory.Create();
                OmegaQPI.Open(dbProvider);
                AirportHeliport = GetAirportHeliport();
                //ObstacleAreaList = OmegaQPI.GetObstacleAreaList();
                //Runways = OmegaQPI.GetRunwayList(AirportHeliport.Identifier);

                string airportname = Aran.Omega.GlobalParams.Database.AirportHeliport.Name.ToString();

                //Organisation = GetOrganisation();
                //AirspaceList = OmegaQPI.GetAirspaceList();
                DMELists = OmegaQPI.GetDMEList().Where(erport => erport.Name.ToString() == airportname).Where(narrow => narrow.Type.Value.ToString() == "NARROW").ToList();
                VORlistsAll = OmegaQPI.GetVORList().Where(erport => erport.Name.ToString() == airportname).ToList();
                NavaidLists = OmegaQPI.GetNavaidList().Where(erport => erport.Name == airportname).ToList();
                LocList = OmegaQPI.GetLocalizerList().Where(erport => erport.Name == airportname).ToList();
                GlidePathList = OmegaQPI.GetGlidePathList().Where(erport => erport.Name == airportname).ToList();
                VRTsList = OmegaQPI.GetVerticalStructureList();
                NDBLists = OmegaQPI.GetNDBList().Where(erport => erport.Name == airportname).ToList();
                MarkersList = OmegaQPI.GetMArkersList().Where(erport => erport.Name.Contains(airportname)).ToList();
                RunwayList = OmegaQPI.GetRunwayList(AirportHeliport.Identifier);
                RunwayDirList = GetRunwayDirList(RunwayList);
                RunwayCentreLinePoint = GetRwyCentreLinePointList(RunwayDirList);
                LoadVorList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<VerticalStructure> GetVerticalStructureList(MultiPolygon extend)
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList(extend);
            //VRTsList = verticalStructureList.ToList();
            return verticalStructureList;
        }
        public void LoadVorList()
        {
            try
            {
                for (int r = 0; r < Convert.ToInt32(VORlistsAll.Count); r++)
                {
                    if (VORlistsAll[r].Name.ToString() == Aran.Omega.GlobalParams.Database.AirportHeliport.Name.ToString())
                    {
                        if (VORlistsAll[r].Type.Value.ToString() == "VOR")
                        {
                            VORlistsForCvor.Add(VORlistsAll[r].Designator.ToString());
                        }
                        if (VORlistsAll[r].Type.Value.ToString() == "DVOR")
                        {
                            VORlistsForDvor.Add(VORlistsAll[r].Designator.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string GetAirportDesignator()
        {
            return Aran.Omega.GlobalParams.Database.AirportHeliport.Designator.ToString();
        }
        public List<RunwayCentrelinePoint> GetRunwayCenterLinePoints(Guid rwyDirIdentifier)
        {
            List<RunwayCentrelinePoint> rwyCntrLinePtList = OmegaQPI.GetRunwayCentrelinePointList(rwyDirIdentifier);
            return rwyCntrLinePtList;
        }
        private OrganisationAuthority GetOrganisation()
        {
            return OmegaQPI.GetFeature(FeatureType.OrganisationAuthority, GlobalParams.Settings.OLSQuery.Organization) as OrganisationAuthority;
        }
        public List<Runway> Runways { get; private set; }

        public OrganisationAuthority Organisation { get; private set; }

        public AirportHeliport AirportHeliport { get; private set; }
        public List<RunwayCentrelinePoint> RwyCenterLinePointList { get; private set; }

        public List<Airspace> AirspaceList { get; set; }
        public List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter, double distance)
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList(ptCenter, distance);
            return verticalStructureList;
        }
        public List<VerticalStructure> GetVerticalStructureListForPolygon(Aran.Geometries.MultiPolygon multipolygon)
        {
            List<VerticalStructure> verticalStructureList = OmegaQPI.GetVerticalStructureList(multipolygon);
            return verticalStructureList;
        }
        public List<RunwayDirection> GetRunwayDirList(List<Runway> Rwylist)
        {
            List<RunwayDirection> RwyDirList = new List<RunwayDirection>();
            for (int i = 0; i < Rwylist.Count; i++)
            {
                //RwyDirList = OmegaQPI.GetRunwayDirectionList(Rwylist[i].Identifier);
                RwyDirList.AddRange(OmegaQPI.GetRunwayDirectionList(Rwylist[i].Identifier));
            }
            return RwyDirList;
        }
        public List<RunwayCentrelinePoint> GetRwyCentreLinePointList(List<RunwayDirection> RwyDirList)
        {
            List<RunwayCentrelinePoint> RwyCentreLinePtList = new List<RunwayCentrelinePoint>();
            for (int i = 0; i < RwyDirList.Count; i++)
            {
                RwyCentreLinePtList.AddRange(OmegaQPI.GetRunwayCentrelinePointList(RwyDirList[i].Identifier));
            }
            return RwyCentreLinePtList;
        }
        private AirportHeliport GetAirportHeliport()
        {
            return OmegaQPI.GetAdhp(Aran.Omega.GlobalParams.Settings.OLSQuery.Aeroport);
        }
        public static IOmegaQPI OmegaQPI { get; private set; }

    }
}
