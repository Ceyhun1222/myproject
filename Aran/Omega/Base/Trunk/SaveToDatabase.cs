using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Omega.Models;
using Aran.PANDA.Constants;
using Aran.Queries;
using static Aran.Omega.GlobalParams;

namespace Aran.Omega
{
    public class SaveDbWrapper
    {
        private readonly EnumName<CategoryNumber> _category;
        private readonly ElevationDatum _elevationDatum;
        private readonly EnumName<RunwayClassificationType> _rwyClassification;
        private readonly RunwayDirection _rwyDir;

        public SaveDbWrapper(RunwayDirection rwyDir, EnumName<RunwayClassificationType> rwyCallClassification,ElevationDatum datum,EnumName<CategoryNumber> category)
        {
            _rwyDir = rwyDir;
            _rwyClassification = rwyCallClassification;
            _elevationDatum = datum;
            _category = category;
        }

        public bool Save(List<DrawingSurface> surfaceList)
        {
            try
            {
                var obstacleAreaList = new List<Feature>();

                Database.OmegaQPI.ClearAllFeatures();
                foreach (var surface in surfaceList)
                {
                    if (surface == null)
                        throw new ArgumentNullException("Surface is null !");

                    var obstacleArea = Database.OmegaQPI.CreateFeature<ObstacleArea>();
                    obstacleArea.Type = CodeObstacleArea.OLS;
                    obstacleArea.Reference = new ObstacleAreaOrigin();

                    SetFeatureRef(surface, obstacleArea);

                    var unicalObstacles = surface.SurfaceBase.GetReport.Where(obs => obs.Penetrate > 0)
                        .GroupBy(rep => rep.Obstacle.Identifier)
                        .Select(g => g.FirstOrDefault()).ToList();

                    foreach (var obstacle in unicalObstacles)
                    {
                        if (obstacle.Penetrate > 0)
                            obstacleArea.Obstacle.Add(obstacle.Obstacle.GetFeatureRefObject());
                    }

                    SetGeometry(surface, obstacleArea);

                    obstacleArea.Annotation.Add(CreateNote());

                    surface.SurfaceBase.ObsArea = obstacleArea;

                    obstacleAreaList.Add(obstacleArea);
                    Database.OmegaQPI.SetFeature(obstacleArea);
                }

                foreach (var feature in obstacleAreaList)
                {
                    var obstacleArea = (ObstacleArea) feature;

                    var obsArea = GetSavedObstacleArea(obstacleArea);

                    if (obsArea != null)
                    {
                        Database.OmegaQPI.ExcludeFeature(obstacleArea.Identifier);

                        obstacleArea.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA;
                        obstacleArea.Id = obsArea.Id;
                        obstacleArea.Identifier = obsArea.Identifier;
                        Database.OmegaQPI.SetFeature(obstacleArea, true);
                    }
                }

                var featViewer = new Aran.Aim.FeatureInfo.ROFeatureViewer();
                System.Windows.Forms.IWin32Window window =
                    new Aran.AranEnvironment.Win32Windows(HWND);
                featViewer.GettedFeature = Database.OmegaQPI.GetFeature;
                featViewer.SetOwner(window);
                if (!featViewer.ShowFeaturesForm(obstacleAreaList, true, true))
                    return false;

                Database.OmegaQPI.CommitWithoutViewer(new Aran.Aim.FeatureType[]
                    {Aran.Aim.FeatureType.ObstacleArea});

                SavedObstacleAreas = obstacleAreaList.Cast<ObstacleArea>().ToList();
                return true;

            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e.Message);
                throw;
            }
        }

        private void SetFeatureRef(DrawingSurface surface,ObstacleArea obstacleArea)
        {
            if (surface.SurfaceType == SurfaceType.InnerHorizontal ||
                surface.SurfaceType == SurfaceType.OuterHorizontal ||
                surface.SurfaceType == SurfaceType.CONICAL || surface.SurfaceType == SurfaceType.Strip)
            {
                obstacleArea.Reference.OwnerAirport =
                    Database.AirportHeliport.GetFeatureRef();
            }
            else
                obstacleArea.Reference.OwnerRunway = _rwyDir.GetFeatureRef();
        }

        private void SetGeometry(DrawingSurface surface, ObstacleArea obstacleArea)
        {
            obstacleArea.SurfaceExtent = new Surface();
            switch (surface.SurfaceType)
            {
                case SurfaceType.Approach:
                    var approach = surface.SurfaceBase as Approach;
                    var section1 = new Aran.Geometries.Polygon();
                    if (approach != null)
                    {
                        approach.Section1.Geo.Close();
                        section1.ExteriorRing =
                            SpatialRefOperation.ToGeo(approach.Section1.Geo);
                        obstacleArea.SurfaceExtent.Geo.Add(section1);
                        if (approach.Section2 != null)
                        {
                            approach.Section2.Geo.Close();
                            var section2 = new Aran.Geometries.Polygon
                            {
                                ExteriorRing =
                                    SpatialRefOperation.ToGeo(approach.Section2.Geo)
                            };

                            approach.Section3.Geo.Close();
                            var section3 = new Aran.Geometries.Polygon
                            {
                                ExteriorRing =
                                    SpatialRefOperation.ToGeo(approach.Section3.Geo)
                            };
                            obstacleArea.SurfaceExtent.Geo.Add(section2);
                            obstacleArea.SurfaceExtent.Geo.Add(section3);
                        }
                    }

                    //As correction

                    obstacleArea.Type = CodeObstacleArea.OTHER_APPROACH;
                    break;
                case SurfaceType.Transitional:
                    var transitional = surface.SurfaceBase as Transitional;
                    if (transitional != null)
                        foreach (var plane in transitional.Planes)
                        {
                            plane.Geo.Close();
                            var poly = new Aran.Geometries.Polygon
                            {
                                ExteriorRing = SpatialRefOperation.ToGeo(plane.Geo)
                            };
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }
                    obstacleArea.Type = CodeObstacleArea.OTHER_TRANSITIONAL;

                    break;
                case SurfaceType.InnerTransitional:
                    var innerTransitional = surface.SurfaceBase as InnerTransitional;
                    if (innerTransitional != null)
                        foreach (var plane in innerTransitional.Planes)
                        {
                            plane.Geo.Close();
                            var poly = new Aran.Geometries.Polygon
                            {
                                ExteriorRing = SpatialRefOperation.ToGeo(plane.Geo)
                            };
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }

                    obstacleArea.Type = CodeObstacleArea.OTHER_INNERTRANSITIONAL;

                    break;
                case SurfaceType.Strip:
                    var strip = surface.SurfaceBase as Strip;
                    if (strip != null)
                        foreach (var plane in strip.Planes)
                        {
                            plane.Geo.Close();
                            var poly = new Aran.Geometries.Polygon
                            {
                                ExteriorRing = SpatialRefOperation.ToGeo(plane.Geo)
                            };
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }
                    obstacleArea.Type = CodeObstacleArea.OTHER_STRIP;
                    break;
                case SurfaceType.Area2A:
                    var area2A = surface.SurfaceBase as Area2A;
                    if (area2A != null)
                        foreach (var plane in area2A.Planes)
                        {
                            plane.Geo.Close();
                            var poly = new Aran.Geometries.Polygon
                            {
                                ExteriorRing = SpatialRefOperation.ToGeo(plane.Geo)
                            };
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }
                    obstacleArea.Type = CodeObstacleArea.OTHER_AREA2A;
                    break;
                default:
                    surface.SurfaceBase.GeoPrj.Close();
                    var mltGeo = SpatialRefOperation.ToGeo(surface.SurfaceBase.GeoPrj);
                    foreach (Aran.Geometries.Polygon poly in mltGeo)
                        obstacleArea.SurfaceExtent.Geo.Add(poly);

                    switch (surface.SurfaceType)
                    {
                        case SurfaceType.CONICAL:
                            obstacleArea.Type = CodeObstacleArea.OTHER_CONICAL;
                            break;
                        case SurfaceType.InnerHorizontal:
                            obstacleArea.Type = CodeObstacleArea.OTHER_INNERHORIZONTAL;
                            break;
                        case SurfaceType.InnerApproach:
                            obstacleArea.Type = CodeObstacleArea.OTHER_INNERAPPROACH;
                            break;
                        case SurfaceType.Approach:
                            obstacleArea.Type = CodeObstacleArea.OTHER_APPROACH;
                            break;
                        case SurfaceType.BalkedLanding:
                            obstacleArea.Type = CodeObstacleArea.OTHER_BALKEDLANDING;
                            break;
                        case SurfaceType.TakeOffClimb:
                            obstacleArea.Type = CodeObstacleArea.OTHER_TAKEOFFCLIMB;
                            break;
                        case SurfaceType.OuterHorizontal:
                            obstacleArea.Type = CodeObstacleArea.OTHER_OUTERHORIZONTAL;
                            break;
                        case SurfaceType.TakeOffFlihtPathArea:
                            obstacleArea.Type = CodeObstacleArea.OTHER_TAKEOFFFLIGHTPATHAREA;
                            break;
                        default:
                            break;
                    }

                    break;
            }

        }

        private Note CreateNote()
        {
            var note = new Note();
            note.Purpose = CodeNotePurpose.REMARK;
            var linguisticNote = new LinguisticNote();
            linguisticNote.Note = new Aim.DataTypes.TextNote();
            var noteText = "Time : " + DateTime.Today + Environment.NewLine + "Classification :" +
                           _rwyClassification.Name + Environment.NewLine;
            if (_category != null)
                noteText += "Category : " + _category.Name + Environment.NewLine +
                            "Elevation Datum :" +
                            _elevationDatum.Height;


            linguisticNote.Note.Value = noteText;

            note.TranslatedNote.Add(linguisticNote);

            return note;
        }

        private ObstacleArea GetSavedObstacleArea(ObstacleArea obstacleArea)
        {
            ObstacleArea ObsArea(IEnumerable<ObstacleArea> itemList)
            {
                var obstacleAreas = itemList as ObstacleArea[] ?? itemList.ToArray();
                if (obstacleAreas.Count() > 1)
                    return obstacleAreas.Aggregate((i1, i2) => i1.TimeSlice.SequenceNumber > i2.TimeSlice.SequenceNumber
                        ? i1
                        : i2);
                return obstacleAreas.FirstOrDefault();
            }

            ObstacleArea obsArea = null;
            switch (obstacleArea.Reference.Choice)
            {
                case Aran.Aim.ObstacleAreaOriginChoice.RunwayDirection:
                {
                    var itemList = Database.ObstacleAreaList.Where(
                        obs => obs.Type == obstacleArea.Type &&
                               obs.Reference.Choice == Aran.Aim.ObstacleAreaOriginChoice.RunwayDirection
                               && obs.Reference.OwnerRunway.Identifier ==
                               _rwyDir.Identifier);

                    obsArea = ObsArea(itemList);
                }
                    break;
                case Aran.Aim.ObstacleAreaOriginChoice.AirportHeliport:
                {
                    var itemList = Database.ObstacleAreaList.Where(
                        obs => obs.Type == obstacleArea.Type &&
                               obs.Reference.Choice ==
                               Aran.Aim.ObstacleAreaOriginChoice.AirportHeliport &&
                               obs.Reference?.OwnerAirport != null &&
                               obs.Reference.OwnerAirport.Identifier ==
                               Database.AirportHeliport.Identifier);

                    obsArea = ObsArea(itemList);
                }
                    break;
                case Aran.Aim.ObstacleAreaOriginChoice.OrganisationAuthority:
                {
                    var itemList = Database.ObstacleAreaList.Where(
                        obs => obs.Type == obstacleArea.Type &&
                               obs.Reference.Choice ==
                               Aran.Aim.ObstacleAreaOriginChoice.OrganisationAuthority &&
                               obs.Reference?.OwnerOrganisation != null &&
                               obs.Reference.OwnerOrganisation.Identifier ==
                               Database.Organisation.Identifier);

                    obsArea = ObsArea(itemList);
                }
                    break;
            }

            return obsArea;
        }

        public List<ObstacleArea> SavedObstacleAreas { get; set; }
    }
}

