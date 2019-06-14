using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.DB
{
    public class LocalGdbGenerator
    {

        public IWorkspace CreateAccessWorkspace(string path, string dbName)
        {
            // Instantiate an Access workspace factory and create a new personal geodatabase.
            // The Create method returns a workspace name object.
            IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspaceName workspaceName = workspaceFactory.Create(path, dbName, null, 0);

            // Cast the workspace name object to the IName interface and open the workspace.
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }

        public void CreateFeatureClassFromType(String dbFeatClassName, Type amFeature, IFeatureWorkspace featureWorkspace, ISpatialReference spatialRef)
        {

            IWorkspaceDomains workspaceDomains = (IWorkspaceDomains)featureWorkspace;

            // Create a fields collection for the feature class.
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

            //Property list
            System.Reflection.PropertyInfo[] propInfos = amFeature.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //propInfos.ToList().AddRange(amFeature.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance ));
            #region OID field

            // Add an object ID field to the fields collection. This is mandatory for feature classes.

            IField oidField = CreateField("OID", esriFieldType.esriFieldTypeOID);

            fieldsEdit.AddField(oidField);

            #endregion

            #region Geometry type

            var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(IGeometry).FullName) != null);
            if (geoProp != null)
            {
                var geoField = CreateGeoField("Shape", (esriGeometryType)Common.PropertyTypeMapping[geoProp.PropertyType.FullName], spatialRef);
        
                fieldsEdit.AddField(geoField);
               
            }
            #endregion

            foreach (var prop in propInfos)
            {
                var propType = prop.PropertyType;
                var propName = prop.Name;

                #region Create Domains

                Type argType = null;
                if (propType.Name == typeof(AM_Nullable<Type>).Name)
                {
                    var genArgs = propType.GenericTypeArguments;
                    argType = genArgs[0];
                }
                if (propType.IsEnum)
                {

                    if (workspaceDomains.DomainByName[propType.Name] is null)
                    {
                        var domain = CreateDomain(propType.Name, propType);
                        workspaceDomains.AddDomain(domain);
                    }
                }

                if (argType != null && argType.IsEnum)
                {
                    if (workspaceDomains.DomainByName[argType.Name] is null)
                    {
                        var domain = CreateDomain(argType.Name, argType);
                        workspaceDomains.AddDomain(domain);
                    }
                }              

                #endregion

                #region Attributes

                string dispAttr = Common.GetAttributeFrom<DisplayAttribute>(amFeature, propName)?.Name;
                var maxLengthAttr = Common.GetAttributeFrom<MaxLengthAttribute>(amFeature, propName)?.Length;
                var willBeInDB= Common.GetAttributeFrom<BrowsableAttribute>(amFeature, propName)?.Browsable;

                #endregion

                if (willBeInDB.HasValue && !willBeInDB.Value)
                    continue;

                #region Enum type

                if (propType.IsEnum)
                {

                    IField enumField = CreateField(propName, esriFieldType.esriFieldTypeString, workspaceDomains.DomainByName[propType.Name], maxLengthAttr);

                    fieldsEdit.AddField(enumField);
                    continue;

                }
                #endregion

                #region DataType

                if (propType.Name == typeof(DataType.DataType<Enum>).Name)
                {
                    var genArgType = typeof(double);

                    //Add simple fields
                    IField genField = CreateField(propName, (esriFieldType)Common.PropertyTypeMapping[genArgType.FullName], length: maxLengthAttr);

                    fieldsEdit.AddField(genField);
                    continue;
                }
                #endregion

                #region AM_Nullable type

                if (propType.Name == typeof(AM_Nullable<Type>).Name)
                {

                    #region Nullable primitive type

                    if (argType.IsPrimitive || argType == typeof(DateTime) || argType == typeof(String))
                    {
                        //Add simple fields
                        IField genField = CreateField(propName, (esriFieldType)Common.PropertyTypeMapping[argType.FullName], length: maxLengthAttr);

                        fieldsEdit.AddField(genField);
                        continue;
                    }
                    #endregion

                    #region Nullable Enum

                    if (argType.IsEnum)
                    {

                        IField enumField = CreateField(propName, esriFieldType.esriFieldTypeString, workspaceDomains.DomainByName[argType.Name], maxLengthAttr);

                        fieldsEdit.AddField(enumField);
                        continue;
                    }
                    #endregion

                }

                #endregion

                #region Simple type

                if (propType.IsPrimitive || propType == typeof(DateTime) || propType == typeof(String))
                {
                    //Add simple fields
                    IField simpleField = CreateField(propName, (esriFieldType)Common.PropertyTypeMapping[propType.FullName], length: maxLengthAttr);

                    fieldsEdit.AddField(simpleField);
                    continue;
                }
                #endregion

            }


            #region Validate fields collection.

            IFieldChecker fieldChecker = new FieldCheckerClass();

            IEnumFieldError enumFieldError = null;

            IFields validatedFields = null;

            fieldChecker.ValidateWorkspace = (IWorkspace)featureWorkspace;

            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

            #endregion

            int shapeField = validatedFields.FindField("Shape");
            if (shapeField <= 0)
            {
                featureWorkspace.CreateTable(dbFeatClassName, validatedFields, null, null, "");
                return;
            }

            IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(dbFeatClassName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

        }

        private static IField CreateField(string name, esriFieldType type, IDomain domain = null, int? length = null)
        {
            IField newField = new FieldClass();

            IFieldEdit newFieldEdit = (IFieldEdit)newField;

            newFieldEdit.Name_2 = name;
            newFieldEdit.Type_2 = type;
            newFieldEdit.Domain_2 = domain;
            if (length != null)
                newFieldEdit.Length_2 = length.Value;

            return newField;
        }

        public static IField CreateGeoField(string name, esriGeometryType type,ISpatialReference spatialRef)
        {
            // Create a geometry definition for the feature class

            IGeometryDef geometryDef = new GeometryDefClass();

            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

            geometryDefEdit.GeometryType_2 = type;

            geometryDefEdit.SpatialReference_2 = spatialRef;


            // Add a geometry field to the fields collection. This is where the geometry definition is applied.

            IField geometryField = new FieldClass();

            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;

            geometryFieldEdit.Name_2 = name;

            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            geometryFieldEdit.GeometryDef_2 = geometryDef;

            return geometryField;
        }

        public static IDomain CreateDomain(string name, Type propType)
        {
            ICodedValueDomain codedValueDomain = new CodedValueDomainClass();
            IDomain domain2 = codedValueDomain as IDomain;

            foreach (var amObj in Enum.GetValues(propType))
            {
                codedValueDomain.AddCode(amObj.ToString(), amObj.ToString());
            }

            foreach (var defValInfo in AerodromeDataCash.ProjectEnvironment.DefaultValues)
            {
                codedValueDomain.AddCode(defValInfo.ValueForString, defValInfo.ValueForString);
            }

            domain2.Name = name;

            domain2.FieldType = esriFieldType.esriFieldTypeString;
            domain2.SplitPolicy = esriSplitPolicyType.esriSPTDuplicate;
            domain2.MergePolicy = esriMergePolicyType.esriMPTDefaultValue;

            return domain2;
        }
    }
}
