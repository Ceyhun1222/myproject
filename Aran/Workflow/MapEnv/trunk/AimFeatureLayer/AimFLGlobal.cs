using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Env2.Layers;
using Aran.Aim;
using Aran.Aim.Utilities;
using Aran.Geometries;
using MapTools;
using AG = Aran.Geometries;
using AF = Aran.Aim.Features;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using EG = ESRI.ArcGIS.Geometry;
using Carto = ESRI.ArcGIS.Carto;
using Display = ESRI.ArcGIS.Display;
using System.IO;
using System.Reflection;

namespace MapEnv
{
	internal static class AimFLGlobal
	{
		private static double [] _xPointCoord = new double [1];
		private static double [] _yPointCoord = new double [1];
		private static double [] _zPointCoord = new double [1];

		
		static AimFLGlobal ( )
		{
			try
			{
				ShapeFileDirectory = Globals.TempDir + @"\SHF\";
				_openedWorkspaceDict = new Dictionary<string, IWorkspace> ( );
			}
			catch ( Exception ex)
			{
				throw new Exception ( "AimFLGlobal", ex );
			}
		}


		public static readonly string ShapeFileDirectory;

		private static Dictionary<string, IWorkspace> _openedWorkspaceDict;

		public static void AddAimSimpleLayer (User selUser)
		{
			var layerBuilderForm = new SimpleLayerBuilderForm (selUser);
			layerBuilderForm.FinishClicked += SimpleLayerBuilderForm_FinishClicked_MultiFeature;
			layerBuilderForm.Show (Globals.MainForm);
		}

		public static bool FillMyFeatureLayer (AimFeatureLayer aimFL)
		{
			aimFL.DbSpatialReference = GeoWGS84_SpatialRef;
			aimFL.SpatialReference = GeoWGS84_SpatialRef;
			aimFL.LayerInfoList.Clear ();

			if (aimFL.AimFeatures.Count () == 0)
				return false;

			GetFeatureClasses (
				aimFL.FeatureType,
				aimFL.AimFeatures,
				aimFL.ShapeInfoList,
				aimFL.LayerInfoList);

			return true;
		}

		public static bool GetFeatureClasses (
			FeatureType featType,
			IEnumerable <AF.Feature> features,
			List<TableShapeInfo> shapeInfos,
			List <FeatLayerInfo> layerInfoList)
		{
			var geoPropInfosList = new List<AimPropInfo []> ();
			var textPropInfosList = new List<AimPropInfo []> ();
			var symbolPropInfosList = new List<AimPropInfo []> ();

			for (int i = 0; i < shapeInfos.Count; i++)
			{
				TableShapeInfo shapeInfo = shapeInfos [i];

				var geoPropInfos = GetInnerProps ((int) featType, shapeInfo.GeoProperty);
				var textPropInfos = GetInnerProps ((int) featType, shapeInfo.TextProperty);
				var symbolPropInfos = GetInnerProps ((int) featType, shapeInfo.CategorySymbol.PropertyName);

				geoPropInfosList.Add (geoPropInfos);
				textPropInfosList.Add (textPropInfos);
				symbolPropInfosList.Add (symbolPropInfos);
			}

			//var shapeFileName = featType + "_" + Guid.NewGuid ().ToString ().Substring (25);
			//var shapeFileName = Guid.NewGuid ();
			var shapeFileName = ((int) featType) + "-" + Guid.NewGuid ();

			for (int i = 0; i < shapeInfos.Count; i++)
			{
				GeometryType geomType = GeometryType.Null;

				var shapeFileRowList = new List<ShapeFileRow> ();

				foreach (var feature in features)
				{
					IAimObject aimObject = (IAimObject) feature;

					var geoPropValueList = AimMetadataUtility.GetInnerPropertyValue (aimObject, geoPropInfosList [i], true);
					var textPropValueList = AimMetadataUtility.GetInnerPropertyValue (aimObject, textPropInfosList [i], true);
					var symbolValuePropValueList = AimMetadataUtility.GetInnerPropertyValue (aimObject, symbolPropInfosList [i], true);

					var label = GetFirstValueAsString (textPropValueList);
					var symbolValue = GetFirstValueAsString (symbolValuePropValueList);
					if (symbolValue != null)
						symbolValue = symbolValue.ToLower ();

					foreach (IAimProperty geoPropValue in geoPropValueList)
					{
						if (geoPropValue.PropertyType == AimPropertyType.AranField)
						{
							var aranGeom = (Aran.Geometries.Geometry) ((IEditAimField) geoPropValue).FieldValue;

							if (geomType == GeometryType.Null)
								geomType = aranGeom.Type;

							double zMin, zMax;
							var shapeFileRow = new ShapeFileRow () {
								Id = feature.Id,
								Identifier = feature.Identifier.ToString (),
								Label = label,
								Geom = aranGeom,
								SymbolValue = symbolValue,
								ZValue = GetZValue (feature, out zMin, out zMax),
								ZMin = zMin,
								ZMax = zMax
							};

							if (!string.IsNullOrEmpty (shapeFileRow.Label) && shapeFileRow.Label.Length > 100)
								shapeFileRow.Label = shapeFileRow.Label.Substring (0, 100);

							if (!string.IsNullOrEmpty (shapeFileRow.SymbolValue) && shapeFileRow.SymbolValue.Length > 100)
								shapeFileRow.SymbolValue = shapeFileRow.SymbolValue.Substring (0, 100);

							shapeFileRowList.Add (shapeFileRow);
						}
					}
				}

				if (shapeFileRowList.Count == 0)
					continue;

				FileInfo shapefileInfo;
				var featClass = CreateShapefile (shapeFileName + "-" + i, geomType, shapeFileRowList, out shapefileInfo);

				var featLayer = new Carto.FeatureLayer ();
				featLayer.FeatureClass = featClass;
				featLayer.Name = (featClass as IDataset).Name;
				featLayer.SpatialReference = GeoWGS84_SpatialRef;
				featLayer.Cached = false;

				var geoFeatLayer = featLayer as Carto.IGeoFeatureLayer;

				#region Fill Symbol

				// Fill if setting up categories symbol 
				var catSymbol = shapeInfos [i].CategorySymbol;
				var defaultSymbol = catSymbol.DefaultSymbol;

				if (defaultSymbol != null)
				{
					if (string.IsNullOrEmpty (catSymbol.PropertyName))
					{
						Carto.ISimpleRenderer render = new Carto.SimpleRenderer ();
						render.Symbol = defaultSymbol;
						geoFeatLayer.Renderer = render as Carto.IFeatureRenderer;
					}
					else
					{
						Carto.IUniqueValueRenderer uRender = new Carto.UniqueValueRenderer ();
						uRender.DefaultSymbol = defaultSymbol;
						uRender.UseDefaultSymbol = true;
						uRender.FieldCount = 1;
						uRender.Field [0] = "symbol_val";

						foreach (var key in catSymbol.Symbols.Keys)
						{
							var subSymbol = catSymbol.Symbols [key];
							uRender.AddValue (key.ToLower (), "Cat: " + key, subSymbol);
						}

						geoFeatLayer.Renderer = uRender as Carto.IFeatureRenderer;
					}
				}

				geoFeatLayer.DisplayAnnotation = true;
				geoFeatLayer.DisplayField = "label";

				#endregion

				layerInfoList.Add (new FeatLayerInfo () {
					Layer = featLayer,
					ShapefileInfo = shapefileInfo
				});
			}

			return true;
		}



		private static void SimpleLayerBuilderForm_FinishClicked_MultiFeature (object sender, EventArgs e)
		{
			var layerBuilderForm = sender as SimpleLayerBuilderForm;

			string layerName;
			Aran.Aim.FeatureType featType;
			Filter filter;
			TableShapeInfo [] shapeInfos;

			if (!layerBuilderForm.GetResult (out layerName, out featType, out filter, out shapeInfos))
				return;

			// Load Features...
			var features = Globals.LoadFeatures (featType, filter);

			var aimFL = new AimFeatureLayer ();
			aimFL.FeatureType = featType;
			aimFL.AimFeatures = features;
			aimFL.AimFilter = filter;
			aimFL.Name = layerName;
			aimFL.ShapeInfoList.AddRange (shapeInfos);
			aimFL.IsLoaded = true;

			FillMyFeatureLayer (aimFL);
			
			var aimLayer = new Toc.AimLayer (aimFL);
			Globals.MainForm.AddAimSimpleShapefileLayer (aimLayer, true);
		}

		private static IFeatureClass CreateShapefile (
			string layerName,
			GeometryType geomType, 
			List<ShapeFileRow> shapeFileRowList,
			out FileInfo shapefileInfo)
		{
			string fileName = CreateShapeFile (layerName, geomType, shapeFileRowList);
			shapefileInfo = new System.IO.FileInfo (fileName);

			IWorkspace ws;
			if (!_openedWorkspaceDict.TryGetValue (shapefileInfo.DirectoryName, out ws))
			{
				IWorkspaceFactory wsFct = new ShapefileWorkspaceFactoryClass ();
				ws = wsFct.OpenFromFile (shapefileInfo.DirectoryName, 0);
				_openedWorkspaceDict.Add (shapefileInfo.DirectoryName, ws);
			}
			
			var featureWS = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) ws;
			return featureWS.OpenFeatureClass (shapefileInfo.Name);
		}


		private static AimPropInfo [] GetInnerProps (int aimTypeIndex, string propertyName)
		{
			if (propertyName == null)
				return new AimPropInfo [0];
			return AimMetadataUtility.GetInnerProps (aimTypeIndex, propertyName);
		}

		private static string GetFirstValueAsString (List<IAimProperty> propValueList)
		{
			if (propValueList.Count > 0 &&
                        propValueList [0] != null &&
                        propValueList [0].PropertyType == AimPropertyType.AranField)
			{
				return ((IEditAimField) propValueList [0]).FieldValue.ToString ();
			}

			return null;
		}

		private static string CreateShapeFile (string layerName, GeometryType geomType, List<ShapeFileRow> dataList)
		{
			ShapeLib.ShapeType shpType = ShapeLib.ShapeType.PointZ;
			if (geomType == Aran.Geometries.GeometryType.MultiPolygon)
				shpType = ShapeLib.ShapeType.PolygonZ;
			else if (geomType == Aran.Geometries.GeometryType.MultiLineString)
				shpType = ShapeLib.ShapeType.PolyLineZ;

			var dir = ShapeFileDirectory;
			var output = dir + layerName;

			if (!Directory.Exists (dir))
				Directory.CreateDirectory (dir);

			// create shapefile
			IntPtr hShp = ShapeLib.SHPCreate (output, shpType);

			if (hShp.Equals (IntPtr.Zero))
				throw new Exception (string.Format ("\nCould not create {0}.shp\nProbable cause: File is in use by EasyStreets\n\nPress any key to exit", output));

			var hDbf = ShapeLib.DBFCreate (output);

			ShapeLib.DBFAddField (hDbf, "id", ShapeLib.DBFFieldType.FTInteger, 32, 0);
			ShapeLib.DBFAddField (hDbf, "identifier", ShapeLib.DBFFieldType.FTString, 40, 0);
			ShapeLib.DBFAddField (hDbf, "label", ShapeLib.DBFFieldType.FTString, 100, 0);
			ShapeLib.DBFAddField (hDbf, "symbol_value", ShapeLib.DBFFieldType.FTString, 100, 0);
			ShapeLib.DBFAddField (hDbf, "z_min", ShapeLib.DBFFieldType.FTDouble, 8, 8);
			ShapeLib.DBFAddField (hDbf, "z_max", ShapeLib.DBFFieldType.FTDouble, 8, 8);

			var n = 0;
			var fieldIndex = 0;

			foreach (var row in dataList)
			{
				var pShp = CreateGeometry (row.Geom, row.ZValue);

				if (pShp != IntPtr.Zero)
				{
					ShapeLib.SHPWriteObject (hShp, -1, pShp);
					ShapeLib.SHPDestroyObject (pShp);

					fieldIndex = 0;
					ShapeLib.DBFWriteIntegerAttribute (hDbf, n, fieldIndex++, (int) row.Id);
					ShapeLib.DBFWriteStringAttribute (hDbf, n, fieldIndex++, row.Identifier);
					ShapeLib.DBFWriteStringAttribute (hDbf, n, fieldIndex++, row.Label);
					ShapeLib.DBFWriteStringAttribute (hDbf, n, fieldIndex++, row.SymbolValue);
					ShapeLib.DBFWriteDoubleAttribute (hDbf, n, fieldIndex++, row.ZMin);
					ShapeLib.DBFWriteDoubleAttribute (hDbf, n, fieldIndex++, row.ZMax);
					n++;
				}
			}
			ShapeLib.SHPClose (hShp);
			ShapeLib.DBFClose (hDbf);

			var prjFile = dir + layerName + ".prj";

			if (File.Exists (prjFile))
				File.Delete (prjFile);
			
			//System.IO.File.Copy (ShapeFilePrjFile, prjFile);
			CreateWGS84ProjectFile (prjFile);

			return output;
		}

		private static void CreateWGS84ProjectFile (string fileName)
		{
			var sw = File.CreateText (fileName);
			sw.WriteLine (Properties.Resources.WGS84PrjFileText);
			sw.Close ();
		}

		private static IntPtr CreateGeometry (Geometry geom, double zValue)
		{
			switch (geom.Type)
			{
				case GeometryType.Point:
					{
						var point = geom as AG.Point;
						_xPointCoord [0] = point.X;
						_yPointCoord [0] = point.Y;
						
						if (!double.IsNaN (zValue))
							_zPointCoord [0] = zValue;
						else
							_zPointCoord [0] = 0;

						return ShapeLib.SHPCreateObject (MapTools.ShapeLib.ShapeType.PointZ, -1, 0, null, null, 1, 
							_xPointCoord, _yPointCoord, _zPointCoord, null);
					}
				case GeometryType.MultiPolygon:
					{
						int nParts = 0;
						int nVertices = 0;
						var multiPolygon = geom as MultiPolygon;

						if (multiPolygon.Count == 0)
							return IntPtr.Zero;

						for (int i = 0; i < multiPolygon.Count; i++)
						{
							var polygon = multiPolygon [i];

							nParts++;
							nVertices += polygon.ExteriorRing.Count;
							nVertices++;

							for (int j = 0; j < polygon.InteriorRingList.Count; j++)
							{
								nParts++;
								nVertices += polygon.InteriorRingList [j].Count;
								nVertices++;
							}
						}

						double [] adfX = new double [nVertices];
						double [] adfY = new double [nVertices];
						double [] adfZ = new double [nVertices];

						double [] tmpXCoord, tmpYCoord, tmpZCoord;
						int [] panPartStart = new int [nParts];
						int indexPanPartStart = 0, currPanPartStart = 0;
						var paPartType = new ShapeLib.PartType [nParts];
						int indexPaPartType = 0;

						foreach (Polygon polygon in multiPolygon)
						{
							paPartType [indexPaPartType++] = ShapeLib.PartType.Ring;

							GetPolygonPoints (polygon.ExteriorRing, out tmpXCoord, out tmpYCoord, out tmpZCoord);

							Array.Copy (tmpXCoord, 0, adfX, currPanPartStart, tmpXCoord.Length);
							Array.Copy (tmpYCoord, 0, adfY, currPanPartStart, tmpYCoord.Length);
							Array.Copy (tmpZCoord, 0, adfZ, currPanPartStart, tmpZCoord.Length);

							panPartStart [indexPanPartStart++] = currPanPartStart;
							currPanPartStart += tmpXCoord.Length;

							for (int j = 0; j < polygon.InteriorRingList.Count; j++)
							{
								paPartType [indexPaPartType++] = ShapeLib.PartType.Ring;

								GetPolygonPoints (polygon.InteriorRingList [j], out tmpXCoord, out tmpYCoord, out tmpZCoord);

								Array.Copy (tmpXCoord, 0, adfX, currPanPartStart, tmpXCoord.Length);
								Array.Copy (tmpYCoord, 0, adfY, currPanPartStart, tmpYCoord.Length);
								Array.Copy (tmpZCoord, 0, adfZ, currPanPartStart, tmpZCoord.Length);

								panPartStart [indexPanPartStart++] = currPanPartStart;
								currPanPartStart += tmpXCoord.Length;
							}

						}

						if (!double.IsNaN (zValue))
						{
							for (int i = 0; i < adfZ.Length; i++)
								adfZ [i] = zValue;
						}

						return ShapeLib.SHPCreateObject (
							MapTools.ShapeLib.ShapeType.PolygonZ, -1, nParts, panPartStart, 
							paPartType, nVertices, adfX, adfY, adfZ, null);
					}
				case GeometryType.MultiLineString:
					{
						int nParts = 0;
						int nVertices = 0;
						var multiLS = geom as MultiLineString;

						if (multiLS.Count == 0)
							return IntPtr.Zero;

						for (int i = 0; i < multiLS.Count; i++)
						{
							nParts++;
							nVertices += multiLS [i].Count;
						}

						double [] adfX = new double [nVertices];
						double [] adfY = new double [nVertices];
						double [] adfZ = new double [nVertices];

						double [] tmpXCoord, tmpYCoord, tmpZCoord;
						int [] panPartStart = new int [nParts];
						int indexPanPartStart = 0, currPanPartStart = 0;
						var paPartType = new ShapeLib.PartType [nParts];
						int indexPaPartType = 0;

						foreach (LineString ls in multiLS)
						{
							paPartType [indexPaPartType++] = ShapeLib.PartType.Ring;

							GetPolylinePoints (ls, out tmpXCoord, out tmpYCoord, out tmpZCoord);

							Array.Copy (tmpXCoord, 0, adfX, currPanPartStart, tmpXCoord.Length);
							Array.Copy (tmpYCoord, 0, adfY, currPanPartStart, tmpYCoord.Length);
							Array.Copy (tmpZCoord, 0, adfZ, currPanPartStart, tmpZCoord.Length);

							panPartStart [indexPanPartStart++] = currPanPartStart;
							currPanPartStart += tmpXCoord.Length;


						}

						if (!double.IsNaN (zValue))
						{
							for (int i = 0; i < adfZ.Length; i++)
								adfZ [i] = zValue;
						}

						return ShapeLib.SHPCreateObject (
							MapTools.ShapeLib.ShapeType.PolyLineZ, -1, nParts, panPartStart,
							paPartType, nVertices, adfX, adfY, adfZ, null);
					}
			}

			return IntPtr.Zero;
		}

		private static void GetPolygonPoints (MultiPoint mp, out double [] xCoord, out double [] yCoord, out double [] zCoord)
		{
			xCoord = new double [mp.Count + 1];
			yCoord = new double [mp.Count + 1];
			zCoord = new double [mp.Count + 1];

			for (int i = 0; i < mp.Count; i++)
			{
				xCoord [i] = mp [i].X;
				yCoord [i] = mp [i].Y;
				zCoord [i] = mp [i].Z;
			}
			
			var n = mp.Count;
			xCoord [n] = mp [0].X;
			yCoord [n] = mp [0].Y;
			zCoord [n] = mp [0].Z;
		}

		private static void GetPolylinePoints (MultiPoint mp, out double [] xCoord, out double [] yCoord, out double [] zCoord)
		{
			xCoord = new double [mp.Count];
			yCoord = new double [mp.Count];
			zCoord = new double [mp.Count];

			for (int i = 0; i < mp.Count; i++)
			{
				xCoord [i] = mp [i].X;
				yCoord [i] = mp [i].Y;
				zCoord [i] = mp [i].Z;
			}
		}

		private static double GetZValue (Aran.Aim.Features.Feature feature, out double zMin, out double zMax)
		{
			zMin = 0;
			zMax = 0;

			if (feature.FeatureType == FeatureType.VerticalStructure)
			{
				var vs = feature as Aran.Aim.Features.VerticalStructure;
				if (vs.Part.Count > 0)
				{
					var hp = vs.Part [0].HorizontalProjection;
					if (hp != null && hp.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint && 
						hp.Location.Elevation != null)
					{
						return hp.Location.Elevation.Value;
					}
				}
			}
			else if (feature.FeatureType == FeatureType.Airspace)
			{
				var airSp = feature as Aran.Aim.Features.Airspace;
				if (airSp.GeometryComponent.Count > 0 && 
					airSp.GeometryComponent [0].TheAirspaceVolume != null)
				{
					var av = airSp.GeometryComponent [0].TheAirspaceVolume;
					if (av.LowerLimit != null && av.LowerLimit.Value > 0)
					{
						zMin = av.LowerLimit.Value;
					}
					if (av.UpperLimit != null && av.UpperLimit.Value > 0)
					{
						zMax = av.UpperLimit.Value;
						return zMax;
					}
				}
			}

			return double.NaN;
		}

		public static EG.ISpatialReference GeoWGS84_SpatialRef
		{
			get
			{
				if (_geoWGS84SpatialRef == null)
				{
					var spatialRefFatcory = new EG.SpatialReferenceEnvironment ();
					EG.IGeographicCoordinateSystem geoCoordSys;
					geoCoordSys = spatialRefFatcory.CreateGeographicCoordinateSystem ((int) EG.esriSRGeoCSType.esriSRGeoCS_WGS1984);
					geoCoordSys.SetFalseOriginAndUnits (-180.0, -180.0, 5000000.0);
					geoCoordSys.SetZFalseOriginAndUnits (0.0, 100000.0);
					geoCoordSys.SetMFalseOriginAndUnits (0.0, 100000.0);

					_geoWGS84SpatialRef = geoCoordSys as EG.ISpatialReference;
				}
				return _geoWGS84SpatialRef;
			}
		}

		private static EG.ISpatialReference _geoWGS84SpatialRef;
	}

	internal class ShapeFileRow
	{
		public long Id { get; set; }

		public string Identifier { get; set; }

		// Length = 100
		public string Label { get; set; }

		public Geometry Geom { get; set; }

		// Length = 100
		public string SymbolValue { get; set; }

		public double ZValue { get; set; }

		public double ZMin { get; set; }
		
		public double ZMax { get; set; }
	}
}
