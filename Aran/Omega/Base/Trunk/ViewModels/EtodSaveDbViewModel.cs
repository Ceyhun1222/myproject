using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Omega.Models;
using Aran.PANDA.Constants;
using Aran.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aran.Omega.ViewModels
{
    public class EtodSaveDbViewModel:ViewModel
    {
        public EtodSaveDbViewModel()
        {

        }

        public EtodSaveDbViewModel(List<DrawingSurface> itemList)
        {
            if (itemList != null)
            {
                SurfaceList = new List<EtodSaveItem>();
                foreach (var surface in itemList)
                {
                    EtodSaveItem item = new EtodSaveItem(surface);
                    SurfaceList.Add(item);
                }    
            }

            EtodSaveItem area2Item = new EtodSaveItem("Area2");
            area2Item.SurfaceType = EtodSurfaceType.Area2;
            SurfaceList.Add(area2Item);

            SaveCommand = new RelayCommand(new Action<object>(Save));
        }

        private void Save(object obj)
        {
            try
            {


                if (SurfaceList == null) return;

                if (SurfaceList.Count(surface => surface.IsChecked) == 0)
                {
                    MessageBox.Show("You must select at least one Surface for Save!", "Omega", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                GlobalParams.Database.OmegaQPI.ClearAllFeatures();
                var obstacleAreaList = new List<Feature>();


                foreach (var surfaceItem in SurfaceList)
                {
                    if (!surfaceItem.IsChecked)
                        continue;
                    if (surfaceItem.SurfaceType == EtodSurfaceType.Area2)
                    {
                        var area2 = CreateArea2();
                        if (area2 != null)
                            obstacleAreaList.Add(area2);
                    }

                    var surface = surfaceItem.Item;

                    if (surface != null && surface.SurfaceBase.EtodSurfaceType != EtodSurfaceType.Area2)
                    {
                        var obstacleArea = CreateObstacleArea(surface);
                        if (obstacleArea != null)
                            obstacleAreaList.Add(obstacleArea);
                    }
                }
                if (GlobalParams.Database.ObstacleAreaList != null)
                {
                    foreach (ObstacleArea obstacleArea in obstacleAreaList)
                    {
                        ObstacleArea obsArea = null;
                        IEnumerable<ObstacleArea> itemList = null;

                        if (obstacleArea.Reference.Choice == Aran.Aim.ObstacleAreaOriginChoice.AirportHeliport)
                        {
                            itemList = GlobalParams.Database.ObstacleAreaList.Where
                                  (obs => obs.Type == obstacleArea.Type &&
                                  obs.Reference.Choice == Aran.Aim.ObstacleAreaOriginChoice.AirportHeliport
                                  && obs.Reference != null && obs.Reference.OwnerAirport != null && obs.Reference.OwnerAirport.Identifier == GlobalParams.Database.AirportHeliport.Identifier);
                        }
                        else if (obstacleArea.Reference.Choice == Aran.Aim.ObstacleAreaOriginChoice.OrganisationAuthority)
                        {
                            itemList = GlobalParams.Database.ObstacleAreaList.Where
                                  (obs => obs.Type == obstacleArea.Type &&
                                  obs.Reference.Choice == Aran.Aim.ObstacleAreaOriginChoice.OrganisationAuthority
                                  && obs.Reference != null && obs.Reference.OwnerOrganisation != null && obs.Reference.OwnerOrganisation.Identifier == GlobalParams.Database.Organisation.Identifier);
                        }

                        if (itemList.Count() > 1)
                            obsArea = itemList.Aggregate((i1, i2) => i1.TimeSlice.SequenceNumber > i2.TimeSlice.SequenceNumber ? i1 : i2);
                        else
                            obsArea = itemList.FirstOrDefault();

                        if (obsArea != null)
                        {
                            obstacleArea.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA;
                            obstacleArea.Identifier = obsArea.Identifier;
                        }
                    }
                }


                var featViewer = new Aran.Aim.FeatureInfo.ROFeatureViewer();
                System.Windows.Forms.IWin32Window window = new Aran.AranEnvironment.Win32Windows(GlobalParams.HWND);
                featViewer.GettedFeature = GlobalParams.Database.OmegaQPI.GetFeature;
                featViewer.SetOwner(window);
                if (featViewer.ShowFeaturesForm(obstacleAreaList, true, true))
                {
                    if (GlobalParams.Database.OmegaQPI.CommitWithoutViewer(new Aran.Aim.FeatureType[] { Aran.Aim.FeatureType.ObstacleArea }))
                    {
                        MessageBox.Show("Data saved successfully!");
                        Close();
                    }
                }
                else
                    MessageBox.Show("Save Cancelled!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error happened when trying to save DB! " + e.Message, "Omega", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetObstacleAreaType(ObstacleArea obstacleArea,EtodSurfaceType surfaceType)
        {
            switch (surfaceType)
            {
                case EtodSurfaceType.Area1:
                    obstacleArea.Type = CodeObstacleArea.AREA1;
                    break;
                case EtodSurfaceType.Area2A:
                    obstacleArea.Type = CodeObstacleArea.OTHER_AREA2A;
                    break;
                case EtodSurfaceType.Area2B:
                    obstacleArea.Type = CodeObstacleArea.OTHER_AREA2B;
                    break;
                case EtodSurfaceType.Area2C:
                    obstacleArea.Type = CodeObstacleArea.OTHER_AREA2C;
                    break;
                case EtodSurfaceType.Area2D:
                    obstacleArea.Type = CodeObstacleArea.OTHER_AREA2D;
                    break;
                case EtodSurfaceType.Area3:
                    obstacleArea.Type = CodeObstacleArea.AREA3;
                    break;
                case EtodSurfaceType.Area4:
                    obstacleArea.Type = CodeObstacleArea.AREA4;
                    break;
                default:
                    obstacleArea.Type = CodeObstacleArea.AREA2;
                    break;
            }
        }

        private ObstacleArea CreateArea2()
        {
            if (!SurfaceList.Any(surface => surface.Item!=null && surface.Item.SurfaceBase.EtodSurfaceType.ToString().Contains("Area2"))) return null;

            var area2 = GlobalParams.Database.OmegaQPI.CreateFeature<ObstacleArea>();
            area2.Type = CodeObstacleArea.AREA2;
            area2.Reference = new ObstacleAreaOrigin();
            area2.Reference.OwnerAirport = GlobalParams.Database.AirportHeliport.GetFeatureRef();
            Geometry area2GeoPrj = new MultiPolygon();
            area2.SurfaceExtent = new Surface();

            foreach (var surfaceItem in SurfaceList)
            {
                var surface = surfaceItem.Item;

                if (surface == null || !surfaceItem.SurfaceType.ToString().Contains("Area2")) continue;

                foreach (var obstacle in surface.SurfaceBase.GetReport)
                {
                    if (obstacle.Penetrate > 0)
                        area2.Obstacle.Add(obstacle.Obstacle.GetFeatureRefObject());
                }

                if (surface.SurfaceBase is IMultiplePlane)
                {
                    var multipLane = surface.SurfaceBase as IMultiplePlane;
                    foreach (var plane in multipLane.Planes)
                        area2GeoPrj = GlobalParams.GeomOperators.UnionGeometry(
                                new Aran.Geometries.Polygon { ExteriorRing = plane.Geo }, area2GeoPrj);
                }
                else
                {
                    if (surface.SurfaceBase.GeoPrj != null)
                        area2GeoPrj = GlobalParams.GeomOperators.UnionGeometry(surface.SurfaceBase.GeoPrj,
                            area2GeoPrj);
                }

            }

            var area2Geo = GlobalParams.SpatialRefOperation.ToGeo(area2GeoPrj);
            if (area2Geo is MultiPolygon)
            {
                foreach (Aran.Geometries.Polygon poly in area2Geo)
                    area2.SurfaceExtent.Geo.Add(poly);
            }
            else if (area2Geo is Aran.Geometries.Polygon)
                area2.SurfaceExtent.Geo.Add(area2Geo as Aran.Geometries.Polygon);

            return area2;
        }


        private ObstacleArea CreateObstacleArea(DrawingSurface surface)
        {
            
            var obstacleArea = GlobalParams.Database.OmegaQPI.CreateFeature<ObstacleArea>();

            SetObstacleAreaType(obstacleArea, surface.SurfaceBase.EtodSurfaceType);

            obstacleArea.Reference = new ObstacleAreaOrigin();

            if (surface.SurfaceBase.EtodSurfaceType == EtodSurfaceType.Area1)
                obstacleArea.Reference.OwnerOrganisation = GlobalParams.Database.Organisation.GetFeatureRef();
            else
                obstacleArea.Reference.OwnerAirport = GlobalParams.Database.AirportHeliport.GetFeatureRef();

            ObstacleArea feature = obstacleArea;

            foreach (var obstacle in surface.SurfaceBase.GetReport)
            {
                if (obstacle.Penetrate > 0)
                    feature.Obstacle.Add(obstacle.Obstacle.GetFeatureRefObject());
            }

            if (surface.SurfaceBase is IMultiplePlane)
            {
                var multipLane = surface.SurfaceBase as IMultiplePlane;
                feature.SurfaceExtent = new Surface();
                foreach (var plane in multipLane.Planes)
                {
                    var poly = new Aran.Geometries.Polygon();
                    poly.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo(plane.Geo);
                    feature.SurfaceExtent.Geo.Add(poly);
                }
            }
            else
            {
                if (surface.SurfaceBase.GeoPrj != null)
                {
                    var mltGeo = GlobalParams.SpatialRefOperation.ToGeo(surface.SurfaceBase.GeoPrj);
                    feature.SurfaceExtent = new Surface();
                    foreach (Aran.Geometries.Polygon poly in mltGeo)
                        feature.SurfaceExtent.Geo.Add(poly);
                }
            }
            return feature;
        }

        public RelayCommand SaveCommand { get; set; }

        public List<EtodSaveItem> SurfaceList { get; set; }


    }
}
