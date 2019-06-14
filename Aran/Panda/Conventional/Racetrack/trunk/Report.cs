using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class UserReport
	{
        //public long Id
        //{
        //    get;
        //    set;
        //}
		public string Name
		{
			get;
			set;
		}
		public double Elevation
		{
			get;
			set;
		}
		public double Moc
		{
			get;
			set;
		}
		public double Req_H
		{
			get;
			set;
		}
		public double Penetrate
		{
			get;
			set;
		}
		public bool Validation
		{
			get;
			set;
		}
		public VerticalStructure Obstacle
		{
			get;
			set;
		}
        public ObstactleReportType Location
        {
            get;
            set;
        }

        public string Area
		{
			get;
			set;
		}

		public int AreaNumber
		{
			get;
			set;
		}

		public ObstacleGeomType GeomType
		{
			get;
			set;
		}

		public Geometry GeomPrj
		{
			get;
			set;
		}

		public double HorAccuracy
		{
			get;
			set;
		}

		public double VerAccuracy
		{
			get;
			set;
		}
	}

	public enum ObstacleGeomType
	{
		Point,
		Polygon,
		PolyLine
	}

	public class Report
	{
		//private double _altitude;
		//private double _bufferWidth;
		//private ModulType _selectedModul;
	    //private double _minMoc;

	    private Report ( )
		{
			ObstacleReport = new List<UserReport> ( );
		}

		public Report ( Polygon areaWithSectors, List<Polygon> areaWithBuffers, double altitude, double moc,double minMoc, double bufferWidth, ModulType selectedModulType)
			: this ( )
		{
			Create ( areaWithSectors, areaWithBuffers, altitude, moc, minMoc, bufferWidth, selectedModulType );
		}

		public Dictionary<VerticalStructure, ObstactleReportType> VerticalStructureList
		{
			get
			{
				return ObstacleReport.ToDictionary ( rep => rep.Obstacle,rep=>rep.Location);
			}
		}

		public void Create ( Polygon areaWithSectors, List<Polygon> areaWithBuffers, double altitude, double moc, double minMoc, double bufferWidth, ModulType selectedModulType )
		{
			AreaWithSectors = areaWithSectors;
			//ProtectSect1 = protectSect1;
			//ProtectSect2 = protectSect2;
			//ProtectRecipDir = protectRecipDir;
			//ProtectIntersectRadEntry = protectIntersectRadEntry;
			//ProtectOmniDirectEntry = protectionOmniDirectionalEntry;
			FullAreaWithBuffers = areaWithBuffers[ areaWithBuffers.Count - 1 ];
			//_selectedModul = selectedModulType;
			ObstacleReport.Clear ( );
		    GetObstacleList(moc, minMoc, altitude, bufferWidth, selectedModulType);
		}

		private void GetObstacleList ( double moc, double minMoc, double altitude, double bufferWidth, ModulType selectedModul)
		{
			Omega.Topology.GeometryOperators geomOperators= new Omega.Topology.GeometryOperators ( );
			Box minMaxPoint = TransForm.QueryCoords ( FullAreaWithBuffers );
			var ptMin = new Point ( minMaxPoint.XMin, minMaxPoint.YMin );
			var ptMax = new Point ( minMaxPoint.XMax, minMaxPoint.YMax );
			var extent = new MultiPolygon { CreateExtent ( ptMin.X, ptMin.Y, ptMax.X, ptMax.Y ) };
		    double deltaMoc = moc - minMoc;

            MultiPolygon extentGeo = GlobalParams.SpatialRefOperation.ToGeo ( extent );

			List<VerticalStructure> verticalStructureList = GlobalParams.Database.HoldingQpi.GetVerticalStructureList ( extentGeo[ 0 ] );

			ObstacleReport.Clear ( );
			int j = 0;
			try
			{
				foreach ( var vs in verticalStructureList )
				{
					j++;
					double vsMoc = 0;
                    if(vs.Name == "ATKAN0005")
                    {

                    }
					double maxPenetrate = -1000000;
					var vsAreaNumber = 0;
					bool isIntersect = false;
					double vsElevation = 0;
					var vsGeomType = ObstacleGeomType.Point;
					var vsSurfaceType = ObstactleReportType.BasicArea;
					Geometry vsGeom = null;
					var vsHorAccuracy = 0.0;
					var vsVerAccuracy = 0.0;
					foreach ( var vsPart in vs.Part )
					{
                        Geometry partGeo = null;
						double partElevation = 0;
						var horProjection = vsPart.HorizontalProjection;
						var partGeomType = ObstacleGeomType.Point;
						double partVerAccuracy = 0;
						double partHorAccuracy = 0;
						if ( horProjection == null )
							continue;
						if ( horProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedPoint )
						{
							if ( horProjection.Location?.Geo == null )
								continue;
							else
							{
								partGeo = GlobalParams.SpatialRefOperation.ToPrj ( horProjection.Location.Geo );
								if ( partGeo.IsEmpty )
									continue;
								partElevation = Converters.ConverterToSI.Convert ( horProjection.Location.Elevation, 0 );
								partVerAccuracy = Converters.ConverterToSI.Convert ( horProjection.Location.VerticalAccuracy, 0 );
								partHorAccuracy = Converters.ConverterToSI.Convert ( horProjection.Location.HorizontalAccuracy, 0 );
							}
						}
						else if ( horProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedSurface )
						{
							if ( horProjection.SurfaceExtent?.Geo == null )
								continue;
							else
							{
								partGeo = GlobalParams.SpatialRefOperation.ToPrj ( horProjection.SurfaceExtent.Geo );
								if ( partGeo.IsEmpty )
									continue;
								partElevation = Converters.ConverterToSI.Convert ( horProjection.SurfaceExtent.Elevation, 0 );
								partGeomType = ObstacleGeomType.Polygon;
								partVerAccuracy = Converters.ConverterToSI.Convert ( horProjection.SurfaceExtent.VerticalAccuracy, 0 );
								partHorAccuracy = Converters.ConverterToSI.Convert ( horProjection.SurfaceExtent.HorizontalAccuracy, 0 );
							}
						}
						else if ( horProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedCurve )
						{
							if ( horProjection.LinearExtent?.Geo == null )
								continue;
							else
							{
								partGeo = GlobalParams.SpatialRefOperation.ToPrj ( horProjection.LinearExtent.Geo );
								if ( partGeo.IsEmpty )
									continue;
								partElevation = Converters.ConverterToSI.Convert ( horProjection.LinearExtent.Elevation, 0 );
								partVerAccuracy = Converters.ConverterToSI.Convert ( horProjection.LinearExtent.VerticalAccuracy, 0 );
								partHorAccuracy = Converters.ConverterToSI.Convert ( horProjection.LinearExtent.HorizontalAccuracy, 0 );
								partGeomType = ObstacleGeomType.PolyLine;
							}
						}

						if ( !geomOperators.Disjoint ( AreaWithSectors, partGeo ) )
						{
							var tmpPenetrate = partElevation + moc + partVerAccuracy - altitude;

							if ( maxPenetrate < tmpPenetrate )
							{
								maxPenetrate = tmpPenetrate;
								vsMoc = moc;
								vsSurfaceType = ObstactleReportType.BasicArea;
								vsAreaNumber = 0;
								vsElevation = partElevation;
								vsGeom = partGeo;
								vsHorAccuracy = partHorAccuracy;
								vsVerAccuracy = partVerAccuracy;
								vsGeomType = partGeomType;
							}

							isIntersect = true;
						}
						else
						{
							var distance = geomOperators.GetDistance ( AreaWithSectors, partGeo ) - partHorAccuracy;
							int partAreaNumber;
							double curMoc = 0;
							if ( selectedModul == ModulType.Holding )
							{
								if ( distance > 5 * bufferWidth )
									continue;
								partAreaNumber = ( int ) Math.Floor ( distance / bufferWidth );							    
                                if ( partAreaNumber > 0 )
									curMoc = 0.5 * minMoc - 0.1 * minMoc* ( partAreaNumber - 1 );
								else
									curMoc = minMoc;

							    curMoc += deltaMoc;

								var tmpPenetrate = partElevation + curMoc + partVerAccuracy - altitude;
								if ( maxPenetrate < tmpPenetrate )
								{
									maxPenetrate = tmpPenetrate;
									vsMoc = curMoc;
									vsSurfaceType = ObstactleReportType.SecondaryArea;
									vsAreaNumber = partAreaNumber;
									vsElevation = partElevation;
									vsHorAccuracy = partHorAccuracy;
									vsVerAccuracy = partVerAccuracy;
									vsGeom = partGeo;
									vsGeomType = partGeomType;
								}
							}
							else
							{
								if ( distance > bufferWidth )
									continue;

								partAreaNumber = 1;
								curMoc = ( bufferWidth - distance ) / bufferWidth * moc;

								var tmpPenetrate = partElevation + curMoc + partVerAccuracy - altitude;
								if ( maxPenetrate < tmpPenetrate )
								{
									maxPenetrate = tmpPenetrate;
									vsMoc = curMoc;
									vsSurfaceType = ObstactleReportType.SecondaryArea;
									vsAreaNumber = partAreaNumber;
									vsElevation = partElevation;
									vsHorAccuracy = partHorAccuracy;
									vsVerAccuracy = partVerAccuracy;
									vsGeom = partGeo;
									vsGeomType = partGeomType;
								}
							}


							isIntersect = true;
						}
					}
					if ( isIntersect )
					{
						var vsReport = new UserReport
						{
							Name = vs.Name,
							Elevation = Math.Round(GlobalParams.UnitConverter.HeightToDisplayUnits(vsElevation, eRoundMode.CEIL), 0),
							Moc = Math.Round(GlobalParams.UnitConverter.HeightToDisplayUnits(vsMoc, eRoundMode.CEIL), 0)
						};
						//vsReport.Id = ( int ) vs.Id;
						if ( vsReport.Moc == 296 && GlobalParams.UnitConverter.HeightUnit == "ft" )
							vsReport.Moc = 294;
						vsReport.Req_H = Math.Round ( GlobalParams.UnitConverter.HeightToDisplayUnits ( vsElevation + vsMoc, eRoundMode.CEIL ), 0 );
						vsReport.Penetrate = Math.Round ( GlobalParams.UnitConverter.HeightToDisplayUnits ( vsElevation + vsMoc - altitude, eRoundMode.CEIL ), 0 );
						vsReport.Validation = vsReport.Penetrate <= 0;
						vsReport.Location = vsSurfaceType;
						vsReport.Area = vsSurfaceType == ObstactleReportType.BasicArea ? "Primary " : "Secondary ";
						vsReport.Obstacle = vs;
						vsReport.AreaNumber = vsAreaNumber;
						vsReport.GeomType = vsGeomType;
						vsReport.GeomPrj = vsGeom;
                        vsReport.HorAccuracy = Math.Round(GlobalParams.UnitConverter.HeightToDisplayUnits(vsHorAccuracy, eRoundMode.CEIL), 0);
                        vsReport.VerAccuracy = Math.Round(GlobalParams.UnitConverter.HeightToDisplayUnits(vsVerAccuracy, eRoundMode.CEIL), 0);
                        ObstacleReport.Add ( vsReport );
					}
				}
			}

			catch ( Exception e )
			{
				System.Windows.Forms.MessageBox.Show ( j.ToString ( ) );
			}
		}

		private static Polygon CreateExtent ( double minX, double minY, double maxX, double maxY )
		{
			Polygon result = new Polygon ( );
			Ring ring = new Ring {new Point(minX, minY), new Point(minX, maxY), new Point(maxX, maxY), new Point(maxX, minY)};
			result.ExteriorRing = ring;
			return result;
		}

		private Polygon AreaWithSectors
		{
			get;
			set;
		}

		private Polygon FullAreaWithBuffers
		{
			get;
			set;
		}

		public List<UserReport> ObstacleReport
		{
			get; }

		public int ReportCount => ObstacleReport.Count;
	}
}