using System;
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TOSSM.Util
{
    public partial class GeoDbUtil
    {
        #region Dataset and classes

        public static IFeatureDataset CreateFeatureDataset(IWorkspace workspace, string fdsName, ISpatialReference spatialReference)
        {
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
            return featureWorkspace.CreateFeatureDataset(fdsName, spatialReference);
        }

        public static IObjectClass CreateClass(IFeatureDataset dataset, String className, String classAlias, List<IField> fieldList)
        {
            //add feature id 
            IField idField = new FieldClass();
            IFieldEdit idFieldEdit = (IFieldEdit)idField;
            idFieldEdit.Name_2 = "FeatureId";
            idFieldEdit.Type_2 = esriFieldType.esriFieldTypeGUID;

            fieldList.Add(idField);

            var geometryField = fieldList.FirstOrDefault(t => t.Type == esriFieldType.esriFieldTypeGeometry);
            return geometryField == null ? CreateObjectClass(dataset.Workspace, className, classAlias, fieldList) : 
                CreateFeatureClass(dataset, className, classAlias, fieldList, geometryField.Name);
        }

        public static IObjectClass CreateObjectClass(IWorkspace workspace, String className, String classAlias, List<IField> fieldList)
        {
            IFields fields=new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

            // Add an ObjectID field to the fields collection. This is mandatory for feature classes.
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OBJECTID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);
            
            
            foreach (var item in fieldList)
            {
                fieldsEdit.AddField(item);
            }

            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
            IObjectClassDescription ocDescription = new ObjectClassDescriptionClass();

            // Use IFieldChecker to create a validated fields collection.
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError;
            IFields validatedFields;
            fieldChecker.ValidateWorkspace = workspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);


            // The enumFieldError enumerator can be inspected at this point to determine 
            // which fields were modified during validation.
            ITable table = featureWorkspace.CreateTable(className, validatedFields,
                ocDescription.InstanceCLSID, null, "");


            IClassSchemaEdit edit = table as IClassSchemaEdit;
            edit.AlterAliasName(classAlias);

            return (IObjectClass)table;
        }

        public static IFeatureClass CreateFeatureClass(IFeatureDataset dataset, String featureClassName, String classAlias, List<IField> fieldList, string geometryFieldName)
        {
            // Create a fields collection for the feature class.
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

            // Add an ObjectID field to the fields collection. This is mandatory for feature classes.
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OBJECTID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);

            foreach (var item in fieldList)
            {
                fieldsEdit.AddField(item);
            }

            // Use IFieldChecker to create a validated fields collection.
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError;
            IFields validatedFields;
            fieldChecker.ValidateWorkspace = dataset.Workspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

            // The enumFieldError enumerator can be inspected at this point to determine 
            // which fields were modified during validation.


            // Create the feature class. Note that the CLSID parameter is null—this indicates to use the
            // default CLSID, esriGeodatabase.Feature (acceptable in most cases for feature classes).
            IFeatureClass featureClass = dataset.CreateFeatureClass(featureClassName, validatedFields, null, null,
                esriFeatureType.esriFTSimple, geometryFieldName, "");

            IClassSchemaEdit edit = featureClass as IClassSchemaEdit;
            edit.AlterAliasName(classAlias);
            return featureClass;
        }

        #endregion
    }


}
