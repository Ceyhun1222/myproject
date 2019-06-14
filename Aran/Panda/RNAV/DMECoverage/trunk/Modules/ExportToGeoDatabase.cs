using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.RNAV.DMECoverage.Modules
{
    class ExportToGeoDatabase
    {
        private readonly List<NavaidType> _dmeList;
        private readonly List<CDMECoverage> _twoDmeCoverageList;
        private readonly List<ThreeDmeCoverage> _threeDmeCoverageList;
        private readonly List<CriticalDME> _criticalDmeList;
        private readonly MultiPolygon _twoDmeFullCoverage;
        private readonly MultiPolygon _threeDmeFullCoverage;
        private string _folderPath;
        private IWorkspace _fileGDB_workspace;
        private ISpatialReference _spGeo;
        private double _altitude;

        public ExportToGeoDatabase(List<NavaidType> dmeList,List<CDMECoverage> twoDmeCoverageList,List<ThreeDmeCoverage> threeDmeCoverageList,
            List<CriticalDME> criticalDmeList,MultiPolygon twoDmeFullCoverage,MultiPolygon threeDmeFullCoverage)
        {
            _dmeList = dmeList;
            _twoDmeCoverageList = twoDmeCoverageList;
            _threeDmeCoverageList = threeDmeCoverageList;
            _criticalDmeList = criticalDmeList;
            _twoDmeFullCoverage = twoDmeFullCoverage;
            _threeDmeFullCoverage = threeDmeFullCoverage;

            var spatRefConverter = new SpatRefConverter();
            _spGeo = spatRefConverter.ToEsriSpatRef(GlobalVars.pSpRefGeo);

            _altitude = _twoDmeCoverageList.First().Altitude;
        }

        public bool Export()
        {
            try
            {
                _folderPath = "";
                var saveFolderDialog = new FolderBrowserDialog();
                DialogResult dialogResult = saveFolderDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                    _folderPath = saveFolderDialog.SelectedPath;
                else
                {
                    _folderPath = null;
                    return false;
                }

                Random rnd1 = new Random();

                IWorkspaceFactory workspaceFactory = new FileGDBWorkspaceFactory();

                var fileName = "DmeDmeCoverage.gdb";
                if (Directory.Exists(_folderPath + "\\DmeDmeCoverage.gdb"))
                    fileName = "DmeDmeCoverage" + rnd1.Next(100).ToString() + ".gdb";

                IWorkspaceName workspaceName = workspaceFactory.Create(_folderPath, fileName, null, 0);
                // Cast for IName
                ESRI.ArcGIS.esriSystem.IName name = (ESRI.ArcGIS.esriSystem.IName)workspaceName;
                //Open a reference to the FileGDB workspace through the name object
                _fileGDB_workspace = (IWorkspace)name.Open();

                

                int twoDmeFullCoverageId =ExportTwoDmeFullCoverage();
                int threeDmeFullCoverageId = ExportThreeDmeFullCoverage();

                ExportDme();
                ExportCoverages(twoDmeFullCoverageId);
                ExportThreeDmePairs(threeDmeFullCoverageId);
                ExportCriticalDmePairs();

                return true;
            }
            catch (Exception ex)
            {
                GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage").Error(ex, ex.Message);
                return false;
            }
        }

        private void ExportDme()
        {
            IFeatureWorkspace featureWorkspace = _fileGDB_workspace as IFeatureWorkspace;
            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
            geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            geomDefEdit.SpatialReference_2 = _spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField nameField = new Field();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Name";
            nameFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(nameFieldEdit);

            IField magVarField = new Field();
            IFieldEdit magVarFieldEdit = (IFieldEdit)magVarField;
            magVarFieldEdit.Name_2 = "MagVar";
            magVarFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(magVarFieldEdit);

            IField antennaElevationField = new Field();
            IFieldEdit antennaElevationFieldEdit = (IFieldEdit)antennaElevationField;
            antennaElevationFieldEdit.Name_2 = "Elevation_M";
            antennaElevationFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(antennaElevationFieldEdit);

            //IField antennaElevationUomField = new Field();
            //IFieldEdit antennaElevationUomFieldEdit = (IFieldEdit)antennaElevationUomField;
            //antennaElevationUomFieldEdit.Name_2 = "ElevationUom";
            //antennaElevationUomFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            //fieldsEdit.AddField(antennaElevationUomFieldEdit);

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("DME", fields, null, null,
                esriFeatureType.esriFTSimple, "Shape", "");


            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_fileGDB_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            for (int i = 0; i < _dmeList.Count; i++)
            {
                var dme = _dmeList[i];
                var feat = featClass.CreateFeature();

                var esriGeo = ConvertToEsriGeom.FromGeometry(dme.pPtGeo, true);

                feat.Value[0] = esriGeo;
                feat.Value[1] = dme.ToString();
                feat.Value[2] = dme.MagVar;
                feat.Value[3] = dme.pPtGeo.Z;

                feat.Store();

                dme.Tag = feat.OID;
                _dmeList[i] = dme;
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }

        private void ExportCoverages(int fullCoverageId)
        {
            IFeatureWorkspace featureWorkspace = _fileGDB_workspace as IFeatureWorkspace;
            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
            geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            geomDefEdit.SpatialReference_2 = _spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField dme1NameField = new Field();
            IFieldEdit dme1NameFieldEdit = (IFieldEdit)dme1NameField;
            dme1NameFieldEdit.Name_2 = "DME1";
            dme1NameFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(dme1NameFieldEdit);

            IField dme2NameField = new Field();
            IFieldEdit dme2NameFieldEdit = (IFieldEdit)dme2NameField;
            dme2NameFieldEdit.Name_2 = "DME2";
            dme2NameFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(dme2NameFieldEdit);

            IField altitudeField = new Field();
            IFieldEdit altitutudeFieldEdit = (IFieldEdit)altitudeField;
            altitutudeFieldEdit.Name_2 = "Altitude_M";
            altitutudeFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(altitutudeFieldEdit);

            IField distanceField = new Field();
            IFieldEdit distanceFieldEdit = (IFieldEdit)distanceField;
            distanceFieldEdit.Name_2 = "Distance_"+GlobalVars.unitConverter.DistanceUnit.ToString();
            distanceFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(distanceFieldEdit);

            IField dme1IdField = new Field();
            IFieldEdit dme1IdFieldEdit = (IFieldEdit)dme1IdField;
            dme1IdFieldEdit.Name_2 = "DME1Id";
            dme1IdFieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(dme1IdFieldEdit);

            IField dme2IdField = new Field();
            IFieldEdit dme2FieldEdit = (IFieldEdit)dme2IdField;
            dme2FieldEdit.Name_2 = "DME2Id";
            dme2FieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(dme2FieldEdit);


            IField fullCoverageIdField = new Field();
            IFieldEdit fullCoverageIdFieldEdit = (IFieldEdit)fullCoverageIdField;
            fullCoverageIdFieldEdit.Name_2 = "FullCoverageId";
            fullCoverageIdFieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(fullCoverageIdFieldEdit);


            IField areaField = new Field();
            IFieldEdit areaFieldEdit = (IFieldEdit)areaField;
            areaFieldEdit.Name_2 = "Area_M";
            areaFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(areaFieldEdit);

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("DMEPairs", fields, null, null,
                esriFeatureType.esriFTSimple, "Shape", "");


            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_fileGDB_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            for (int i = 0; i < _twoDmeCoverageList.Count; i++)
            {
                var coverage = _twoDmeCoverageList[i];
                try
                {
                    var feat = featClass.CreateFeature();
                    var availableZoneGeo = GlobalVars.pspatialReferenceOperation.ToGeo(coverage.AvailableZone);

                    var esriGeo = ConvertToEsriGeom.FromGeometry(availableZoneGeo, true);
                    feat.Value[0] = esriGeo;
                    feat.Value[1] = coverage.DME1.ToString();
                    feat.Value[2] = coverage.DME2.ToString();
                    feat.Value[3] = coverage.Altitude;
                    feat.Value[4] = coverage.Distance;
                    var dme1 = _dmeList.FirstOrDefault(dme => dme.Identifier.Equals(coverage.DME1.Identifier));
                    feat.Value[5] = dme1.Tag;
                    var dme2 = _dmeList.FirstOrDefault(dme => dme.Identifier.Equals(coverage.DME2.Identifier));
                    feat.Value[6] = dme2.Tag;

                    feat.Value[7] = fullCoverageId;
                    feat.Value[8] = coverage.AvailableZone.Area;

                    feat.Store();

                    coverage.Tag = feat.OID;
                    _twoDmeCoverageList[i] = coverage;
                }
                catch (Exception e)
                {
                    GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage").Error(e, e.Message);
                    GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage").Warn(e,coverage.DME1+"-"+coverage.DME2);
                    continue;
                }
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }
        
        private void ExportThreeDmePairs(int fullCoverageId)
        {
            IFeatureWorkspace featureWorkspace = _fileGDB_workspace as IFeatureWorkspace;
            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
            geomDefEdit.HasZ_2 = false;

            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            geomDefEdit.SpatialReference_2 = _spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField dme1NameField = new Field();
            IFieldEdit dme1NameFieldEdit = (IFieldEdit)dme1NameField;
            dme1NameFieldEdit.Name_2 = "DMEPair1";
            dme1NameFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(dme1NameFieldEdit);

            IField dme2NameField = new Field();
            IFieldEdit dme2NameFieldEdit = (IFieldEdit)dme2NameField;
            dme2NameFieldEdit.Name_2 = "DMEPair2";
            dme2NameFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(dme2NameFieldEdit);

            IField dme1IdField = new Field();
            IFieldEdit dme1IdFieldEdit = (IFieldEdit)dme1IdField;
            dme1IdFieldEdit.Name_2 = "DMEPair1Id";
            dme1IdFieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(dme1IdFieldEdit);

            IField dme2IdField = new Field();
            IFieldEdit dme2FieldEdit = (IFieldEdit)dme2IdField;
            dme2FieldEdit.Name_2 = "DMEPair2Id";
            dme2FieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(dme2FieldEdit);

            IField fullCoverageIdField = new Field();
            IFieldEdit fullCoverageIdFieldEdit = (IFieldEdit)fullCoverageIdField;
            fullCoverageIdFieldEdit.Name_2 = "FullCoverageId";
            fullCoverageIdFieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(fullCoverageIdFieldEdit);

            IField areaField = new Field();
            IFieldEdit areaFieldEdit = (IFieldEdit)areaField;
            areaFieldEdit.Name_2 = "Area_M";
            areaFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(areaFieldEdit);

            IField altitudeField = new Field();
            IFieldEdit altitutudeFieldEdit = (IFieldEdit)altitudeField;
            altitutudeFieldEdit.Name_2 = "Altitude_M";
            altitutudeFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(altitutudeFieldEdit);

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("TwoDMEPairsOverlapArea", fields, null, null,
                esriFeatureType.esriFTSimple, "Shape", "");


            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_fileGDB_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            foreach (var coverage in _threeDmeCoverageList)
            {
                try
                {
                    var feat = featClass.CreateFeature();
                    var availableZoneGeo = GlobalVars.pspatialReferenceOperation.ToGeo(coverage.AvailableZone);
                    var area = availableZoneGeo.Area;

                    var esriGeo = ConvertToEsriGeom.FromGeometry(availableZoneGeo, false);
                    feat.Value[0] = esriGeo;
                    feat.Value[1] = coverage.DmeCoverage1.DME1.ToString() + "-" + coverage.DmeCoverage1.DME2.ToString();
                    feat.Value[2] = coverage.DmeCoverage2.DME1.ToString() + "-" + coverage.DmeCoverage2.DME2.ToString();
                    feat.Value[3] = coverage.DmeCoverage1.Tag;
                    feat.Value[4] = coverage.DmeCoverage2.Tag;
                    feat.Value[5] = fullCoverageId;
                    feat.Value[6] = coverage.AvailableZone.Area;
                    feat.Value[7] = _altitude;

                    feat.Store();

                }
                catch (Exception e)
                {
                    GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage").Error(e, e.Message);
                    GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage").Warn(e, coverage.DmeCoverage1.DME1 + "-" + coverage.DmeCoverage1.DME2+","
                        + coverage.DmeCoverage2.DME1 + "-" + coverage.DmeCoverage2.DME2);
                    continue;
                }
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }

        private void ExportCriticalDmePairs()
        {
            IFeatureWorkspace featureWorkspace = _fileGDB_workspace as IFeatureWorkspace;
            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
            geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            geomDefEdit.SpatialReference_2 = _spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField dme1NameField = new Field();
            IFieldEdit dme1NameFieldEdit = (IFieldEdit)dme1NameField;
            dme1NameFieldEdit.Name_2 = "DME";
            dme1NameFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(dme1NameFieldEdit);

            IField dme1IdField = new Field();
            IFieldEdit dme1IdFieldEdit = (IFieldEdit)dme1IdField;
            dme1IdFieldEdit.Name_2 = "DMEId";
            dme1IdFieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(dme1IdFieldEdit);


            IField areaField = new Field();
            IFieldEdit areaFieldEdit = (IFieldEdit)areaField;
            areaFieldEdit.Name_2 = "Area_M";
            areaFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(areaFieldEdit);

            IField altitudeField = new Field();
            IFieldEdit altitutudeFieldEdit = (IFieldEdit)altitudeField;
            altitutudeFieldEdit.Name_2 = "Altitude_M";
            altitutudeFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(altitutudeFieldEdit);


            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("CriticalDME", fields, null, null,
                esriFeatureType.esriFTSimple, "Shape", "");


            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_fileGDB_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            foreach (var critialDme in _criticalDmeList)
            {
                try
                {
                    var feat = featClass.CreateFeature();
                    if (critialDme.CriticalArea == null) continue;
                    var availableZoneGeo = GlobalVars.pspatialReferenceOperation.ToGeo(critialDme.CriticalArea);

                    var esriGeo = ConvertToEsriGeom.FromGeometry(availableZoneGeo, true);
                    feat.Value[0] = esriGeo;
                    var dme1 = _dmeList.FirstOrDefault(dme => dme.Identifier.Equals(critialDme.DMEStation.Identifier));
                    feat.Value[1] = critialDme.DMEStation.ToString();
                    feat.Value[2] = dme1.Tag;
                    feat.Value[3] = critialDme.CriticalArea.Area;
                    feat.Value[4] = _altitude;

                    feat.Store();
                }
                catch (Exception e)
                {
                    GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage").Error(e, e.Message);
                    GlobalVars.gAranEnv?.GetLogger("DmeDmeCoverage")
                        .Warn(e, "Critical Dme:" + critialDme.DMEStation.ToString());
                    continue;
                }
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }

        private int ExportTwoDmeFullCoverage()
        {
            IFeatureWorkspace featureWorkspace = _fileGDB_workspace as IFeatureWorkspace;
            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit) fields;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit) field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit) geomDef;
            geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            geomDefEdit.SpatialReference_2 = _spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField areaField = new Field();
            IFieldEdit areaFieldEdit = (IFieldEdit)areaField;
            areaFieldEdit.Name_2 = "Area_M";
            areaFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(areaFieldEdit);

            IField altitudeField = new Field();
            IFieldEdit altitutudeFieldEdit = (IFieldEdit)altitudeField;
            altitutudeFieldEdit.Name_2 = "Altitude_M";
            altitutudeFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(altitutudeFieldEdit);

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("TwoDMEFullCoverage", fields, null, null,
                esriFeatureType.esriFTSimple, "Shape", "");


            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit) _fileGDB_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            var feat = featClass.CreateFeature();
            var availableZoneGeo = GlobalVars.pspatialReferenceOperation.ToGeo(_twoDmeFullCoverage);

            var esriGeo = ConvertToEsriGeom.FromGeometry(availableZoneGeo, true);
            feat.Value[0] = esriGeo;
            feat.Value[1] = _twoDmeFullCoverage.Area;
            feat.Value[2] = _altitude;

            feat.Store();

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return feat.OID;
        }

        private int ExportThreeDmeFullCoverage()
        {
            IFeatureWorkspace featureWorkspace = _fileGDB_workspace as IFeatureWorkspace;
            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
            geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            geomDefEdit.SpatialReference_2 = _spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField areaField = new Field();
            IFieldEdit areaFieldEdit = (IFieldEdit)areaField;
            areaFieldEdit.Name_2 = "Area_M";
            areaFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(areaFieldEdit);

            IField altitudeField = new Field();
            IFieldEdit altitutudeFieldEdit = (IFieldEdit)altitudeField;
            altitutudeFieldEdit.Name_2 = "Altitude_M";
            altitutudeFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(altitutudeFieldEdit);

            IFeatureClass featClass = null;
            if (featureWorkspace != null)
                featClass = featureWorkspace.CreateFeatureClass("MoreThanTwoDMEFullCoverage", fields, null, null,
                    esriFeatureType.esriFTSimple, "Shape", "");

            if (featClass == null) throw new Exception("Exception occured when saving More than TwoDmeFullCoverage");

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_fileGDB_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();
            var feat = featClass.CreateFeature();
            var availableZoneGeo = GlobalVars.pspatialReferenceOperation.ToGeo(_threeDmeFullCoverage);

            var esriGeo = ConvertToEsriGeom.FromGeometry(availableZoneGeo, true);
            feat.Value[0] = esriGeo;
            feat.Value[1] = _threeDmeFullCoverage.Area;
            feat.Value[2] = _altitude;

            feat.Store();

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return feat.OID;
        }

    }
}
