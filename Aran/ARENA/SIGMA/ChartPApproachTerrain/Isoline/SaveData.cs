using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Media.Media3D;
using EsriWorkEnvironment;
using Aran.PANDA.Common;
using PDM;

namespace ChartPApproachTerrain
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class SaveData
	{
		const string ShapeFieldName = "Shape";

		public static IFeatureClass CreatePointFeatureClass(string LayerName, IFeatureWorkspace featWorkspace)
		{
			try
			{
				IFieldsEdit pFieldsEdit = (IFieldsEdit)(new Fields());

				// Add the Fields to the class the OID and Shape are compulsory
				//======================================= OID
				IFieldEdit pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "OID";
				pFieldEdit.AliasName_2 = "Object ID";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
				pFieldEdit.Editable_2 = false;
				pFieldEdit.IsNullable_2 = false;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= SHAPE
				IGeometryDefEdit pGeomDef = (IGeometryDefEdit)(new GeometryDef());

				pGeomDef.AvgNumPoints_2 = 1;
				pGeomDef.GeometryType_2 = esriGeometryType.esriGeometryPoint;
				pGeomDef.GridCount_2 = 1;
				pGeomDef.GridSize_2[0] = 1000;
				pGeomDef.HasM_2 = true;
				pGeomDef.HasZ_2 = true;
                pGeomDef.SpatialReference_2 = GlobalParams.Map.SpatialReference;

				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = ShapeFieldName;
				pFieldEdit.AliasName_2 = ShapeFieldName;
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.GeometryDef_2 = pGeomDef;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= ID
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "ID";
				pFieldEdit.AliasName_2 = "ID";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.Length_2 = 48;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= NAME
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "NAME";
				pFieldEdit.AliasName_2 = "NAME";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.Length_2 = 32;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= VALUE
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "VALUE";
				pFieldEdit.AliasName_2 = "VALUE";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.Length_2 = 5;
				pFieldEdit.Precision_2 = 5;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= LEVEL
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "Lvl";
				pFieldEdit.AliasName_2 = "Lvl";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.Length_2 = 5;
				pFieldsEdit.AddField(pFieldEdit);

				//=======================================
				return featWorkspace.CreateFeatureClass(LayerName, pFieldsEdit,
					null, null, esriFeatureType.esriFTSimple, ShapeFieldName, "");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Create Point FeatureClass", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return null;
		}

		public static IFeatureClass CreateLineFeatureClass(string LayerName, IFeatureWorkspace featWorkspace)
		{
			try
			{
				IFieldEdit pFieldEdit;

				// establish a fields collection
				IFieldsEdit pFieldsEdit = (IFieldsEdit)new Fields();

				// create the object id field
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "OID";				//"OBJECTID"
				pFieldEdit.AliasName_2 = "Object ID";	//"OBJECTID"
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
				pFieldEdit.Editable_2 = false;
				pFieldEdit.IsNullable_2 = false;
				pFieldsEdit.AddField(pFieldEdit);

				// create the geometry field
				IGeometryDef pGeomDef = new GeometryDef();
				IGeometryDefEdit pGeomDefEdit = (IGeometryDefEdit)pGeomDef;
				// assign the geometry definiton properties.
				pGeomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
				pGeomDefEdit.GridCount_2 = 1;
				pGeomDefEdit.GridSize_2[0] = 0;
				pGeomDefEdit.AvgNumPoints_2 = 2;
				pGeomDefEdit.HasM_2 = true;
				pGeomDefEdit.HasZ_2 = true;
                pGeomDefEdit.SpatialReference_2 = GlobalParams.Map.SpatialReference;

				// create the SHAPE field
				pFieldEdit = (IFieldEdit)new Field();
				pFieldEdit.Name_2 = ShapeFieldName;
				pFieldEdit.AliasName_2 = ShapeFieldName;
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
				pFieldEdit.GeometryDef_2 = pGeomDef;
				pFieldsEdit.AddField(pFieldEdit);

				// create the ID field
				pFieldEdit = (IFieldEdit)new Field();
				pFieldEdit.Name_2 = "ID";
				pFieldEdit.AliasName_2 = "ID";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;// esriFieldTypeInteger;
				pFieldEdit.IsNullable_2 = false;
				pFieldsEdit.AddField(pFieldEdit);

				// create the NAME field
				pFieldEdit = (IFieldEdit)new Field();
				pFieldEdit.Name_2 = "NAME";
				pFieldEdit.AliasName_2 = "Name";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
				pFieldEdit.Length_2 = 32;
				pFieldEdit.IsNullable_2 = false;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= VALUE
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "VALUE";
				pFieldEdit.AliasName_2 = "VALUE";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.Length_2 = 5;
				pFieldEdit.Precision_2 = 5;
				pFieldsEdit.AddField(pFieldEdit);

				//======================================= LEVEL
				pFieldEdit = (IFieldEdit)(new Field());
				pFieldEdit.Name_2 = "Lvl";
				pFieldEdit.AliasName_2 = "Lvl";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
				pFieldEdit.Editable_2 = true;
				pFieldEdit.IsNullable_2 = false;
				pFieldEdit.Length_2 = 5;
				pFieldsEdit.AddField(pFieldEdit);

				// create the SHAPELength field
				/*
				pFieldEdit = (IFieldEdit)new Field();
				pFieldEdit.Name_2 = "SHAPELength";
				pFieldEdit.AliasName_2 = "SHAPELength";
				pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
				pFieldEdit.IsNullable_2 = false;
				pFieldsEdit.AddField(pFieldEdit);*/
				//Set pFields = pFieldsEdit

				return featWorkspace.CreateFeatureClass(LayerName, pFieldsEdit,
					null, null, esriFeatureType.esriFTSimple, ShapeFieldName, "");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Create line FeatureClass", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return null;
		}

		public static void AddShapeLayer(IFeatureClass pFeatureClass)
		{
			string sName = pFeatureClass.AliasName.ToUpper();
			IFeatureLayer pFeatureLayer = null;

			for (int i = 0; i < GlobalParams.Map.LayerCount; i++)
			{
                if (GlobalParams.Map.Layer[i].Name.ToUpper() == sName)
				{
                    if (GlobalParams.Map.Layer[i] is IFeatureLayer)
					{
                        pFeatureLayer = (IFeatureLayer)GlobalParams.Map.Layer[i];
						break;
					}
				}
			}

			if (pFeatureLayer == null)
			{
				//Create a new FeatureLayer and assign a shapefile to it
			    pFeatureLayer = new FeatureLayer
			    {
			        FeatureClass = pFeatureClass,
			        Name = sName
			    };

			    //Add the FeatureLayer to the focus map
                GlobalParams.Map.AddLayer(pFeatureLayer);
			}

			//GlobalVars.gMap.MoveLayer(pFeatureLayer, GlobalVars.gMap.LayerCount - 1);
            GlobalParams.Map.MoveLayer(pFeatureLayer, 0);
		}

		public static void AddRasterLayer(IRasterDataset pOutputRasterDS)
		{
			IRasterLayer rasterLy = new RasterLayerClass();
			rasterLy.CreateFromDataset(pOutputRasterDS);

            string sName = "RasterArea";
			IRasterLayer pFeatureLayer = null;

            //for (int i = 0; i < GlobalParams.Map.LayerCount; i++)
            //{
            //    if (GlobalParams.Map.Layer[i].Name.ToUpper() == sName.ToUpper())
            //    {
            //        if (GlobalParams.Map.Layer[i] is IRasterLayer)
            //        {
            //            pFeatureLayer = (IRasterLayer)GlobalParams.Map.Layer[i];
            //            break;
            //        }
            //    }
            //}
            
            //if (pFeatureLayer != null)
            //{
				//Create a new FeatureLayer and assign a shapefile to it
				pFeatureLayer = rasterLy;
				//Add the FeatureLayer to the focus map
                GlobalParams.Map.AddLayer(pFeatureLayer);
            //}
            
			//GlobalVars.gMap.MoveLayer(pFeatureLayer, GlobalVars.gMap.LayerCount - 1);
            GlobalParams.Map.MoveLayer(pFeatureLayer, GlobalParams.Map.LayerCount - 1);
		}

		public static IWorkspace OpenResultWorkspace(bool bCreateNew)
		{
			//IDataset pDataset;
			//pFeatWs = null;
			//pDataset = pFeatureClass;
			//pFeatWs = pDataset.Workspace;

			string FileName="";// = GlobalVars.GetMapFileName();

			int L = FileName.Length;
			int pos = FileName.LastIndexOf('\\');
			string Location = "\\";

			if (pos != 0)
			{
				Location = FileName.Remove(pos, L - pos);			//Location = VBA.Left(FileName, pos)
				FileName = FileName.Remove(0, pos);				//FileName = VBA.Right(FileName, L - pos)
				L = FileName.Length;
			}

			pos = FileName.LastIndexOf('.');
			string FileNameForCreate = FileName.Remove(pos, L - pos);		//FileNameForCreate = VBA.Left(FileName, pos - 1)

			FileName = FileNameForCreate + ".mdb";

			IWorkspaceFactory factory = new AccessWorkspaceFactory();
			IWorkspace result = null;

			try
			{
				result = factory.OpenFromFile(Location + FileName, 0);
			}
			catch
			{
			}

			if (result == null && bCreateNew)
			{
				try
				{
					IWorkspaceName workspaceName = factory.Create(Location, FileNameForCreate, null, 0);
					factory = workspaceName.WorkspaceFactory;
					result = factory.OpenFromFile(Location + FileName, 0);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Open result Workspace", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return null;
				}
			}

			return result;
		}

		public static IFeatureClass OpenPointFeatureClass(IFeatureWorkspace featWs, string pointClassName, bool bCreateNew)
		{
			IFeatureClass pointFeatureClass = null;
			try
			{
				pointFeatureClass = featWs.OpenFeatureClass(pointClassName);
			}
			catch
			{
			}

			try
			{
				if (pointFeatureClass == null && bCreateNew)
					pointFeatureClass = CreatePointFeatureClass(pointClassName, featWs);

				if (pointFeatureClass != null) AddShapeLayer(pointFeatureClass);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Open Point Workspace", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return pointFeatureClass;
		}

		public static IFeatureClass OpenLineFeatureClass(IFeatureWorkspace featWs, string lineClassName, bool bCreateNew)
		{
			IFeatureClass lineFeatureClass = null;
			try
			{
				lineFeatureClass = featWs.OpenFeatureClass(lineClassName);
			}
			catch
			{
			}

			try
			{
				if (lineFeatureClass == null && bCreateNew)
					lineFeatureClass = CreateLineFeatureClass(lineClassName, featWs);

				if (lineFeatureClass != null) AddShapeLayer(lineFeatureClass);
			}
			catch
			{
			}

			return lineFeatureClass;
		}

		public static void SavePoints(List<Point3D> sparcePoints, string layerName)
		{
			// Create a ComReleaser for cursor management.
			//using (ComReleaser comReleaser = new ComReleaser())
			{
				IFeatureWorkspace workspace = (IFeatureWorkspace)OpenResultWorkspace(true);
				if (workspace == null)
					return;
				//comReleaser.ManageLifetime(workspace);

				IFeatureClass pointsFeatureClass = OpenPointFeatureClass(workspace, layerName, true);
				if (pointsFeatureClass == null)
					return;
				//comReleaser.ManageLifetime(pointsFeatureClass);

				// Find the positions of the fields used to get and set values.
				int iFieldShape = pointsFeatureClass.FindField("Shape");
				int iFieldID = pointsFeatureClass.FindField("ID");
				int iFieldName = pointsFeatureClass.FindField("Name");
				int iFieldValue = pointsFeatureClass.FindField("Value");
				int iFieldLevel = pointsFeatureClass.FindField("Lvl");

				// Create a feature buffer.
				IFeatureBuffer featureBuffer = pointsFeatureClass.CreateFeatureBuffer();
				//comReleaser.ManageLifetime(featureBuffer);

				// Create an insert cursor.
				IFeatureCursor insertCursor = pointsFeatureClass.Insert(true);
				//comReleaser.ManageLifetime(insertCursor);

				for (int i = 0; i < sparcePoints.Count; i++)
				{
					IPoint pPoint = new Point();
					IZAware pZAware = (IZAware)pPoint;
					IMAware pMAware = (IMAware)pPoint;
					pZAware.ZAware = true;
					pMAware.MAware = true;

					pPoint.X = sparcePoints[i].X;
					pPoint.Y = sparcePoints[i].Y;
					pPoint.Z = sparcePoints[i].Z;
					pPoint.M = i;

					featureBuffer.Shape = pPoint;
					featureBuffer.set_Value(iFieldID, "Pt_" + i.ToString());
					featureBuffer.set_Value(iFieldName, "Pt_" + i.ToString());
					featureBuffer.set_Value(iFieldValue, pPoint.Z);
					featureBuffer.set_Value(iFieldLevel, i);

					insertCursor.InsertFeature(featureBuffer);
				}
				// Flush the buffer to the geodatabase.
				insertCursor.Flush();
			}
		}


        public static void JoinSegments(PolylineBuilder pb, string layerName, double step, RunwayCenterLinePoint thrPnt)
        {

            double resX, resY;
            NativeMethods.PointAlongGeodesic(thrPnt.X.Value, thrPnt.Y.Value, step, 90, out resX, out resY);
            double dx;

            dx = Math.Abs(thrPnt.X.Value - resX);

            Dictionary<string, int> dict;

            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, layerName);

            if (layer == null) throw new Exception("Isoline layer not found!");

            var isolinesFeatureClass = ((IFeatureLayer)layer).FeatureClass;

            // Create an insert cursor.
            IFeatureCursor insertCursor = isolinesFeatureClass.Insert(true);
            ITopologicalOperator topoOper = GlobalParams.Area4Rectangle as ITopologicalOperator;
            if (!topoOper.IsKnownSimple)
                topoOper.Simplify();

            var resultList = new List<IGeometryCollection>();

            IGeometryCollection geoCollection=null;

            List<double> listElev = new List<double>();

            for (int i = 0; i < pb.Get_PolylineCount(); i++)
            {
                dict = new Dictionary<string, int>();
                IGeometry pPolyline = pb.GetPolyline(i);

                var resultGeo = topoOper.Intersect(pPolyline, esriGeometryDimension.esriGeometry1Dimension);

                if (resultGeo.Envelope.IsEmpty)
                    continue;
               
                IGeometryCollection geometryColl = (IGeometryCollection)resultGeo;

                List<IGeometryCollection> listGeomColl = new List<IGeometryCollection>();

                var indexArr = new int[geometryColl.GeometryCount];

                for (int j = 0; j < geometryColl.GeometryCount; j++)
                {
                    indexArr[j] = j;
                    IPointCollection pointColl = (IPointCollection)geometryColl.Geometry[j];

                    IGeometry geo = new PolylineClass();
                    IGeometryCollection geomColl = (IGeometryCollection)geo;
                    geomColl.AddGeometry(geometryColl.Geometry[j]);
                    listGeomColl.Add(geomColl);

                    for (int k = 0; k < pointColl.PointCount; k++)
                    {
                        var key = pointColl.Point[k].X.ToString() + pointColl.Point[k].Y.ToString();
                        if (!dict.ContainsKey(key))
                            dict.Add(key, j);
                        else
                        {
                            int polyIndex;
                            if (dict.TryGetValue(key, out polyIndex))
                            {
                                var index =ReturnIndex(indexArr,polyIndex);
                                if (index == j) continue;

                                var joinGeo = listGeomColl[index];
                                
                                (listGeomColl[j]).AddGeometryCollection(joinGeo);
                                indexArr[index] = j;
                            }
                        }
                    }
                }

               
                for (int k = 0; k < indexArr.Length; k++)
                {
                    if (indexArr[k] == k)
                    {
                        resultList.Add(listGeomColl[k]);
                        listElev.Add(pb.GetPolyline(i).Envelope.ZMin);
                    }
                }
            }
        
            for (int l = 0; l < resultList.Count; l++)
            {
               
                IZAware pZAware = (IZAware)resultList[l];
                IMAware pMAware = (IMAware)resultList[l];
                pZAware.ZAware = true;
                pMAware.MAware = true;

                ((IPolyline)resultList[l]).Smooth(dx);  //0.00001);
                IFeature feature = isolinesFeatureClass.CreateFeature();
                feature.Shape = (IGeometry)resultList[l];
                feature.set_Value(2, listElev[l] );
                feature.set_Value(3, "M");

                int Fid = feature.Fields.FindField("FeatureGUID");
                if (Fid > 0)
                    feature.set_Value(Fid, Guid.NewGuid().ToString());

                feature.Store();
            }

           
        }

        public static int ReturnIndex(int[] indexArr, int arrIndex)
        {
            var tmpIndex = indexArr[arrIndex];
            if (tmpIndex == arrIndex)
                return tmpIndex;

            return ReturnIndex(indexArr, tmpIndex);

        }
		public void MakePermanent(IRaster raster, string format, string sPath, string sFileName)
		{
			// Save temporary raster.
			// Query the output (a Raster object) for IRasterBandCollection.
			IRasterBandCollection rasterBandCollection = (IRasterBandCollection)raster;

			// Get the dataset from the first band.
			IRasterBand rasterBand = rasterBandCollection.Item(0);
			IRasterDataset rasterDataset = rasterBand.RasterDataset;

			// Query the dataset for ITemporaryDataset.
			ITemporaryDataset temporaryDataset = (ITemporaryDataset)rasterDataset;
			IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
			IWorkspace workspace = workspaceFactory.OpenFromFile(sPath, 0);

			temporaryDataset.MakePermanentAs(sFileName, workspace, format);
		}

	}
}
