using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using Aran.Geometries.Operators;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using esriFieldType = ESRI.ArcGIS.Geodatabase.esriFieldType;

namespace Aran.Omega.Export
{
    class EtodFeatureClassFactory
    {
        private readonly IFeatureWorkspace _featureWorkspace;
        private readonly ILogger _logger = GlobalParams.AranEnvironment.GetLogger("Omega");
        private ISpatialReference _spatialReference;
        private readonly Dictionary<string, esriFieldType> _vsPartFieldDictionary;
        private readonly Dictionary<string, esriFieldType> _vsFieldDictionary;
        private Dictionary<string,IDomain> _domainsDictionary;

        public EtodFeatureClassFactory(IFeatureWorkspace featureWorkspace)
        {
            _featureWorkspace = featureWorkspace;
            _vsPartFieldDictionary = FillFields();
            _vsFieldDictionary = FillVsFields();
            _domainsDictionary = CreateDomains();
            FillFields();
            AddDomainToFeature();
        }

        private Dictionary<string, esriFieldType> FillVsFields()
        {
            var fieldDictionary = new Dictionary<string, esriFieldType>();
            fieldDictionary.Add("OrigId", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("SourceId", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("RevDate", esriFieldType.esriFieldTypeDate);
            fieldDictionary.Add("StFeat", esriFieldType.esriFieldTypeDate);
            fieldDictionary.Add("EndFeat", esriFieldType.esriFieldTypeDate);
            fieldDictionary.Add("StValid", esriFieldType.esriFieldTypeDate);
            fieldDictionary.Add("EndValid", esriFieldType.esriFieldTypeDate);
            fieldDictionary.Add("ObsType", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("Marking", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("Lighting", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("Interp", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("ObsId", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("IdNumber", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("Length", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("Width", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("Radius", esriFieldType.esriFieldTypeDouble);

            fieldDictionary.Add("LightingICAOStandard", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("Grouped", esriFieldType.esriFieldTypeString);

            return fieldDictionary;
        }

        private Dictionary<string,esriFieldType> FillFields()
        {
            var fieldDictionary = new Dictionary<string, esriFieldType>();
            fieldDictionary.Add("VerticalStructure_Id", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("Designator", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("Area", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("HRefSys", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("Type", esriFieldType.esriFieldTypeString);

            //fieldDictionary.Add("ObsAreaId", esriFieldType.esriFieldTypeString);
            //fieldDictionary.Add("ADId", esriFieldType.esriFieldTypeString);
            //fieldDictionary.Add("RwyDirId", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("HRes", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("HAcc", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("HConf", esriFieldType.esriFieldTypeDouble);

            fieldDictionary.Add("Elev", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("Height", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("Unit", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("VRes", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("VRefSys", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("VAcc", esriFieldType.esriFieldTypeDouble);
            fieldDictionary.Add("VConf", esriFieldType.esriFieldTypeDouble);
                
            fieldDictionary.Add("Integrity", esriFieldType.esriFieldTypeString);

            fieldDictionary.Add("Status", esriFieldType.esriFieldTypeString);
            
            fieldDictionary.Add("GeomType", esriFieldType.esriFieldTypeString);
            
            fieldDictionary.Add("HPos", esriFieldType.esriFieldTypeString);
            fieldDictionary.Add("HExt", esriFieldType.esriFieldTypeDouble);
            return fieldDictionary;
        }

        private Dictionary<string, IDomain> CreateDomains()
        {
            var domainsDictionary = new Dictionary<string, IDomain>();
            domainsDictionary.Add("Status",CreateStatusDomain());
            domainsDictionary.Add("Interp",CreateInterpretationDomain());
            domainsDictionary.Add("Lighting",CreateLightingDomain());
            domainsDictionary.Add("ObsType",CreateObstacleTypDomain());
            return domainsDictionary;
        }

        private IDomain CreateObstacleTypDomain()
        {
            ICodedValueDomain result = new CodedValueDomainClass();

            // Value and name pairs.
            result.AddCode("﻿Arch", "Arch");
            result.AddCode("Tethered Balloon", "Tethered Balloon");
            result.AddCode("Bridge", "Bridge");
            result.AddCode("Building", "Building");
            result.AddCode("Catenary", "Catenary");
            result.AddCode("Crane", "Crane");
            result.AddCode("Control Tower", "Control Tower");
            result.AddCode("Dam", "Dam");
            result.AddCode("Dome", "Dome");
            result.AddCode("Elevator", "Elevator");
            result.AddCode("Monument", "Monument");
            result.AddCode("Power Plant", "Power Plant");
            result.AddCode("Pole", "Pole");
            result.AddCode("Rig", "Rig");
            result.AddCode("Refinery", "Refinery");
            result.AddCode("Sign", "Sign");
            result.AddCode("Spire", "Spire");
            result.AddCode("Stack", "Stack");
            result.AddCode("Tank", "Tank");
            result.AddCode("Transmission Line Tower", "Transmission Line Tower");
            result.AddCode("Tower", "Tower");
            result.AddCode("Tramway", "Tramway");
            result.AddCode("Windmill", "Windmill");
            result.AddCode("Tree", "Tree");
            result.AddCode("Antenna", "Antenna");
            result.AddCode("Vegetation", "Vegetation");
            result.AddCode("Natural Highpoint", "Natural Highpoint");
            result.AddCode("Windmill Farms", "Windmill Farms");
            result.AddCode("Transmission Line", "Transmission Line");
            result.AddCode("Wall", "Wall");
            result.AddCode("Cable Railway Fence", "Cable Railway Fence");
            result.AddCode("Fence", "Fence");
            result.AddCode("Grain Elevator", "Grain Elevator");
            result.AddCode("Navaid", "Navaid");
            result.AddCode("Lighthouse", "Lighthouse");
            result.AddCode("Nuclear Reactor", "Nuclear Reactor");
            result.AddCode("Water Tower", "Water Tower");
            result.AddCode("Stadium", "Stadium");
            result.AddCode("Other", "Other");
            var domain = result as IDomain;
            domain.FieldType = esriFieldType.esriFieldTypeString;
            domain.Name = "obstype";
            return domain;
        }

        private IDomain CreateLightingDomain()
        {
            ICodedValueDomain result = new CodedValueDomainClass();

            // Value and name pairs.
            result.AddCode("﻿UnAvailable", "﻿UnAvailable");
            result.AddCode("Available", "Available");

            var domain = (IDomain) result;
            domain.FieldType = esriFieldType.esriFieldTypeString;
            domain.Name = "lighting";

            return domain;
        }

        private IDomain CreateInterpretationDomain()
        {
            ICodedValueDomain result = new CodedValueDomainClass();

            // Value and name pairs.
            result.AddCode("﻿Snapshot", "﻿Snapshot");
            result.AddCode("Baseline", "Baseline");
            result.AddCode("TempDelta", "TempDelta");
            result.AddCode("PermDelta", "PermDelta");
            result.AddCode("Stream", "Stream");

            var domain = (IDomain)result;
            domain.FieldType = esriFieldType.esriFieldTypeString;
            domain.Name = "interp";

            return result as IDomain;
        }

        private IDomain CreateStatusDomain()
        {
            ICodedValueDomain statusDomain = new CodedValueDomainClass();

            // Value and name pairs.
            statusDomain.AddCode("Planned", "Planned");
            statusDomain.AddCode("Under Construction", "Under Construction");
            statusDomain.AddCode("Completed", "Completed");

            var domain = (IDomain)statusDomain;
            domain.FieldType = esriFieldType.esriFieldTypeString;
            domain.Name = "status";

            return domain;
        }

        private void CreateEsriSpatialReference()
        {
            SpatRefConverter spatRefConverter = new SpatRefConverter();
            _spatialReference = spatRefConverter.ToEsriSpatRef(GlobalParams.SpatialRefOperation.SpRefGeo);
        }

        public IFeatureClass CreateObstaclePartFeatClass(string name, ObstacleGeomType geomType)
        {
            var nameOfShapeFile = name;

            var shapeFieldName = "Shape";

            if (_spatialReference==null)
                CreateEsriSpatialReference();

            try
            {
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
                if (geomType == ObstacleGeomType.Polygon)
                    geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                else if (geomType == ObstacleGeomType.PolyLine)
                    geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

                geomDefEdit.SpatialReference_2 = _spatialReference;

                fieldEdit.GeometryDef_2 = geomDef;
                fieldsEdit.AddField(field);

                foreach (var fieldInfo in _vsPartFieldDictionary)
                {
                    field = new Field();
                    fieldEdit = (IFieldEdit)field;
                    fieldEdit.Name_2 =fieldInfo.Key;
                    fieldEdit.AliasName_2 = fieldInfo.Key;
                    fieldEdit.Type_2 =fieldInfo.Value ;
                    fieldsEdit.AddField(field);
                    if (_domainsDictionary.ContainsKey(fieldInfo.Key))
                        fieldEdit.Domain_2 = _domainsDictionary[fieldInfo.Key];
                }

                var clsId = new UIDClass();
                clsId.Value = "esriGeoDatabase.Feature";
                IFeatureClass featureClass = _featureWorkspace.CreateFeatureClass(nameOfShapeFile, fields, clsId, null,
                    esriFeatureType.esriFTSimple, shapeFieldName, null);

                return featureClass;

            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        public ITable CreateVSFeatClass(string name)
        {
            try
            {
                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                foreach (var fieldInfo in _vsFieldDictionary)
                {
                    var field = new Field();
                    var fieldEdit = (IFieldEdit)field;
                    fieldEdit.Name_2 = fieldInfo.Key;
                    fieldEdit.AliasName_2 = fieldInfo.Key;
                    fieldEdit.Type_2 = fieldInfo.Value;
                    fieldsEdit.AddField(field);
                    if (_domainsDictionary.ContainsKey(fieldInfo.Key))
                        fieldEdit.Domain_2 = _domainsDictionary[fieldInfo.Key];
                }

                var clsId = new UIDClass();
                clsId.Value = "esriGeoDatabase.Feature";
                ITable featureClass = _featureWorkspace.CreateTable(name,fields, clsId, null,"");

                return featureClass;

            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        private void AddDomainToFeature()
        {
            var workspaceDomains = (IWorkspaceDomains)_featureWorkspace;

            foreach (var domain in _domainsDictionary)
            {
                workspaceDomains.AddDomain(domain.Value);
            }
        }
    }
}
