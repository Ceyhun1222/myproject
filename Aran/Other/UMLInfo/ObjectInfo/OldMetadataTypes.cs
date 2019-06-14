using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLInfo
{
    partial class ObjectInfoParser
    {
        private void CreateMessageMetadata()
        {
            Global.MetadataObjects.Clear();

            for (int i = 0; i < _objInfoList.Count; i++)
            {
                if (_objInfoList[i].Namespace.Contains("Metadata"))
                {
                    _objInfoList.RemoveAt(i);
                    i--;
                }
            }

            string[] sa = new string[]
            {
                "CI_RoleCode",
                "CI_DateTypeCode",
                "MD_ProgressCode",
                "DQ_EvaluationMethodTypeCode"
            };

            foreach (var s in sa)
            {
                var objInfo = _objInfoList.Where(oi => oi.Name == s).FirstOrDefault();
                if (objInfo != null)
                    objInfo.Name = s.Replace("_", "");
            }


            ObjectInfo[] arr = new ObjectInfo[]
            {
                Create_MDMetadata(),
                Create_MessageMetadata(),
                Create_ResponsibleParty(),
                Create_Contact(),
                Create_CIContact(),
                Create_Telephone(),
                //Create_CITelephone (),
                Create_CIAddress(),
                Create_CIResponsibleParty(),
                Create_MDConstraints(),
                Create_MDLegalConstraints(),
                Create_LegalConstraints(),
                Create_IdentificationMessage(),
                Create_MDIdentification(),
                Create_MDSecurityConstraints(),
                Create_SecurityConstraints(),
                Create_FeatureMetadata(),
                Create_IdentificationFeature(),
                Create_Citation(),
                Create_CICitation(),
                Create_CIDate(),
                Create_FeatureTimeSliceMetadata(),
                Create_IdentificationTimesliceFeature(),
                Create_DataQuality(),
                Create_DataQualityElement()
            };

            foreach (var item in arr)
            {
                item.IsUsed = true;
                Global.MetadataObjects.Add(item);

                item.Namespace = "Aran.Aim.Metadata";

                //if (item.Type == ObjectInfoType.Datatype)
                //    item.Namespace = "Aran.Aim.DataTypes";
                //else
                //    item.Namespace = "Aran.Aim.Features";
            }
        }

        private ObjectInfo Create_MDMetadata()
        {
            string name = "MDMetadata";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Information about the metadata";
            _objInfoList.Add(item);

            var dateTimeObjInfo = GetByName("dateTime");
            item.AddProperty("DateStamp", dateTimeObjInfo, true);

            return item;
        }

        private ObjectInfo Create_MessageMetadata()
        {
            string name = "MessageMetadata";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Information about the metadata";
            _objInfoList.Add(item);

            item.Base = Create_MDMetadata();

            var responsibleParty = Create_ResponsibleParty();
            var mdConstraints = Create_MDConstraints();
            var identificationMessage = Create_IdentificationMessage();

            item.AddProperty("Contact", responsibleParty);
            item.AddProperty("MetadataConstraints", mdConstraints, true, true);
            item.AddProperty("MessageIdentificationInfo", identificationMessage);

            return item;
        }

        private ObjectInfo Create_ResponsibleParty()
        {
            string name = "ResponsibleParty";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Datatype;
            item.Documentation =
                "Identification of, and means of communication with, person(s) and organisations associated with the dataset";
            _objInfoList.Add(item);

            item.Base = Create_CIResponsibleParty();

            var ciRoleCode = Create_CIRoleCode();
            var contact = Create_Contact();

            item.AddProperty("SystemName", _stringObjInfo, true, false);
            item.AddProperty("ContactInfo", contact, true, false);
            item.AddProperty("DigitalCertificate", _stringObjInfo, true, false);

            return item;
        }

        private ObjectInfo Create_Contact()
        {
            ObjectInfo tmp = GetByName("Contact");
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.Name = "Contact";
            newObjInfo.Type = ObjectInfoType.Datatype;
            newObjInfo.Documentation =
                "Information required enabling contact with the  responsible person and/or organisation";

            newObjInfo.Base = Create_CIContact();

            ObjectInfo telephone = Create_Telephone();

            newObjInfo.Properties.Add(new PropInfo("Phone", telephone, true, false));

            _objInfoList.Add(newObjInfo);

            return newObjInfo;
        }

        private ObjectInfo Create_CIContact()
        {
            var name = "CIContact";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Datatype;
            newObjInfo.Documentation =
                "Information required enabling contact with the  responsible person and/or organisation";

            ObjectInfo ciAddress = Create_CIAddress();

            newObjInfo.Properties.Add(new PropInfo("Address", ciAddress, true, false));

            _objInfoList.Add(newObjInfo);

            return newObjInfo;
        }

        private ObjectInfo Create_Telephone()
        {
            ObjectInfo tmp = GetByName("Telephone");
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.Name = "Telephone";
            newObjInfo.Type = ObjectInfoType.Datatype;
            newObjInfo.Documentation = "Telephone numbers for contacting the responsible individual or organisation";

            //newObjInfo.Base = Create_CITelephone ();

            ObjectInfo phoneCodeType = Create_PhoneCodeType();

            newObjInfo.AddProperty("CodeType", phoneCodeType, true, false);
            newObjInfo.AddProperty("Number", _stringObjInfo, true, false);
            newObjInfo.AddProperty("OtherDescription", _stringObjInfo, true, false);

            _objInfoList.Add(newObjInfo);

            return newObjInfo;
        }

        //private ObjectInfo Create_CITelephone ()
        //{
        //    var name = "CITelephone";

        //    ObjectInfo tmp = GetByName (_objInfoList, name);
        //    if (tmp != null)
        //        return tmp;

        //    ObjectInfo newObjInfo = new ObjectInfo ();
        //    newObjInfo.Name = name;
        //    newObjInfo.Type = ObjectInfoType.Datatype;
        //    newObjInfo.Documentation = "Telephone numbers for contacting the responsible individual or organisation";

        //    _objInfoList.Add (newObjInfo);

        //    return newObjInfo;
        //}

        private ObjectInfo Create_CIAddress()
        {
            var name = "CIAddress";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Datatype;
            newObjInfo.Documentation = "Location of the responsible individual or organisation";

            newObjInfo.AddProperty("deliveryPoint", _stringObjInfo);
            newObjInfo.AddProperty("city", _stringObjInfo);
            newObjInfo.AddProperty("administrativeArea", _stringObjInfo);
            newObjInfo.AddProperty("postalCode", _stringObjInfo);
            newObjInfo.AddProperty("country", _stringObjInfo);
            newObjInfo.AddProperty("electronicMailAddress", _stringObjInfo);

            _objInfoList.Add(newObjInfo);

            return newObjInfo;
        }

        private ObjectInfo Create_CIResponsibleParty()
        {
            string name = "CIResponsibleParty";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Datatype;
            item.Documentation =
                "Identification of, and means of communication with, person(s) and organisations associated with the dataset";
            _objInfoList.Add(item);

            var ciRoleCode = Create_CIRoleCode();
            var contact = Create_Contact();

            item.AddProperty("IndividualName", _stringObjInfo, true, false);
            item.AddProperty("OrganisationName", _stringObjInfo, true, false);
            item.AddProperty("PositionName", _stringObjInfo, true, false);
            item.AddProperty("Role", ciRoleCode, true, false);

            return item;
        }

        private ObjectInfo Create_MDConstraints()
        {
            string name = "MDConstraints";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Restrictions on the access and use of a dataset or metadata";
            _objInfoList.Add(item);

            item.AddProperty("UseLimitation", _stringObjInfo, true, false);

            return item;
        }

        private ObjectInfo Create_MDLegalConstraints()
        {
            string name = "MDLegalConstraints";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Restrictions and legal prerequisites for accessing and using the dataset.";
            _objInfoList.Add(item);

            item.Base = Create_MDConstraints();

            item.AddProperty("OtherConstraints", _stringObjInfo);

            return item;
        }

        private ObjectInfo Create_LegalConstraints()
        {
            string name = "LegalConstraints";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Restrictions and legal prerequisites for accessing and using the dataset.";
            _objInfoList.Add(item);

            item.Base = Create_MDLegalConstraints();

            var restrictionCode = Create_RestrictionCode();

            item.AddProperty("accessConstraints", restrictionCode);
            item.AddProperty("useConstraints", restrictionCode);
            item.AddProperty("userNote", _stringObjInfo);

            return item;
        }

        private ObjectInfo Create_IdentificationMessage()
        {
            string name = "IdentificationMessage";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "";
            _objInfoList.Add(item);

            item.Base = Create_MDIdentification();

            var responsibleParty = Create_ResponsibleParty();
            var msConstraint = Create_MDConstraints();
            var language = Create_LanguageCodeType();

            item.AddProperty("PointOfContact", responsibleParty);
            item.AddProperty("Language", language);
            item.AddListProperty("MessageConstraintInfo", msConstraint);

            return item;
        }

        private ObjectInfo Create_MDIdentification()
        {
            var name = "MDIdentification";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Object;
            newObjInfo.Documentation = "Basic information about data";
            newObjInfo.IsAbstract = true;

            newObjInfo.AddProperty("Abstract", _stringObjInfo);

            _objInfoList.Add(newObjInfo);

            return newObjInfo;
        }

        private ObjectInfo Create_MDSecurityConstraints()
        {
            string name = "MDSecurityConstraints";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation =
                "Handling restrictions imposed on the dataset because of national security, privacy, or other concerns.";
            _objInfoList.Add(item);

            item.Base = Create_MDConstraints();

            item.AddProperty("ClassificationSystem", _stringObjInfo);
            item.AddProperty("HandlingDescription", _stringObjInfo);

            return item;
        }

        private ObjectInfo Create_SecurityConstraints()
        {
            string name = "SecurityConstraints";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation =
                "Handling restrictions imposed on the dataset because of national security, privacy, or other concerns.";
            _objInfoList.Add(item);

            item.Base = Create_MDSecurityConstraints();

            var classification = Create_ClassificationCode();

            item.AddProperty("Classification", classification);
            item.AddProperty("OtherClassification", _stringObjInfo);

            return item;
        }

        private ObjectInfo Create_FeatureMetadata()
        {
            string name = "FeatureMetadata";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Information about the feature metadata";
            _objInfoList.Add(item);

            item.Base = Create_MDMetadata();

            var responsible = Create_ResponsibleParty();
            var identificationFeature = Create_IdentificationFeature();

            item.AddProperty("Contact", responsible);
            item.AddProperty("FeatureIdentificationInfo", identificationFeature);

            return item;
        }

        private ObjectInfo Create_IdentificationFeature()
        {
            string name = "IdentificationFeature";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Basic information about data";
            _objInfoList.Add(item);

            item.Base = Create_MDIdentification();

            var citation = Create_Citation();
            var responsibleParty = Create_ResponsibleParty();
            var languageCode = Create_LanguageCodeType();

            item.AddProperty("Citation", citation);
            //item.AddProperty ("Abstract", _stringObjInfo);
            item.AddProperty("PointOfContact", responsibleParty);
            item.AddProperty("Language", languageCode);

            return item;
        }

        private ObjectInfo Create_Citation()
        {
            string name = "Citiation";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Datatype;
            item.Documentation = "Standardized resource reference";
            _objInfoList.Add(item);

            item.Base = Create_CICitation();

            item.AddProperty("processCertification", _stringObjInfo);

            return item;
        }

        private ObjectInfo Create_CICitation()
        {
            var name = "CICitation";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Datatype;
            item.Documentation = "Standardized resource reference";

            var CIDate = Create_CIDate();

            item.AddProperty("Title", _stringObjInfo);
            item.AddProperty("Date", CIDate);

            _objInfoList.Add(item);
            return item;
        }

        private ObjectInfo Create_CIDate()
        {
            var name = "CIDate";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Datatype;
            item.Documentation = "";

            var dateTime = GetByName("dateTime");
            var dateType = Create_CIDateTypeCode();

            item.AddProperty("Date", dateTime);
            item.AddProperty("DateType", dateType);

            _objInfoList.Add(item);
            return item;
        }

        private ObjectInfo Create_FeatureTimeSliceMetadata()
        {
            string name = "FeatureTimeSliceMetadata";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Information about the Feature Timeslice metadata";
            _objInfoList.Add(item);

            item.Base = Create_MDMetadata();

            var rp = Create_ResponsibleParty();
            var mc = Create_MeasureClassCode();
            var dec = GetByName("decimal");
            var dc = Create_DataQuality();
            var idenTsFeat = Create_IdentificationTimesliceFeature();

            item.AddProperty("Contact", rp);
            item.AddProperty("MeasureClass", mc);
            item.AddProperty("MeasEquipClass", _stringObjInfo);
            item.AddProperty("DataIntegrity", dec);
            item.AddProperty("HorizontalResolution", dec);
            item.AddProperty("VerticalResolution", dec);
            item.AddListProperty("DataQualityInfo", dc);
            item.AddProperty("FeatureTimesliceIdentificationInfo", idenTsFeat);

            return item;
        }

        private ObjectInfo Create_IdentificationTimesliceFeature()
        {
            string name = "IdentificationTimesliceFeature";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "Basic information about Tilmeslice Feature.";
            _objInfoList.Add(item);

            item.Base = Create_MDIdentification();

            var c = Create_Citation();
            var poc = Create_ResponsibleParty();
            var pg = Create_MDProgressCode();
            var lg = Create_LanguageCodeType();

            item.AddProperty("Citiation", c);
            item.AddProperty("PointOfContact", poc);
            item.AddProperty("DataStatus", pg);
            item.AddProperty("Language", lg);

            return item;
        }

        private ObjectInfo Create_DataQuality()
        {
            string name = "DataQuality";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Object;
            item.Documentation = "";
            _objInfoList.Add(item);

            var dqe = Create_DataQualityElement();

            item.AddProperty("attributes", _stringObjInfo);
            item.AddProperty("report", dqe);

            return item;
        }

        private ObjectInfo Create_DataQualityElement()
        {
            string name = "DataQualityElement";

            var item = GetByName(name);
            if (item != null)
                return item;

            item = new ObjectInfo();
            item.Name = name;
            item.Type = ObjectInfoType.Datatype;
            item.Documentation = "";
            _objInfoList.Add(item);

            var dqe = Create_DataQualityElement();
            var emt = Create_DQEvaluationMethodTypeCode();
            var boolean = GetByName("boolean");

            item.AddProperty("evaluationMethodName", _stringObjInfo);
            item.AddProperty("evaluationMethodType", emt);
            item.AddProperty("pass", boolean);

            return item;
        }

        #region Enum

        private ObjectInfo Create_CIRoleCode()
        {
            ObjectInfo tmp = GetByName("CIRoleCode");
            if (tmp != null)
            {
                tmp.IsUsed = true;
                return tmp;
            }

            return null;

            //ObjectInfo newObjInfo = new ObjectInfo ();
            //newObjInfo.IsUsed = true;
            //newObjInfo.Name = "CI_RoleCode";
            //newObjInfo.Type = ObjectInfoType.Codelist;
            //newObjInfo.Namespace = "Aran.Aim.Enums";

            //PropInfo pi = new PropInfo ("resourceProvider", _stringObjInfo);
            //pi.Documentation = "party that supplies the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("custodian", _stringObjInfo);
            //pi.Documentation = "party that accepts accountability and responsability for the data and ensures appropriate care and maintenance of the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("owner", _stringObjInfo);
            //pi.Documentation = "party that owns the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("user", _stringObjInfo);
            //pi.Documentation = "party who uses the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("distributor", _stringObjInfo);
            //pi.Documentation = "party who distributes the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("originator", _stringObjInfo);
            //pi.Documentation = "party who created the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("pointOfContact", _stringObjInfo);
            //pi.Documentation = "party who can be contacted for acquiring knowledge about or acquisition of the resource";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("principalInvestigator", _stringObjInfo);
            //pi.Documentation = "key party responsible for gathering information and conducting research";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("processor", _stringObjInfo);
            //pi.Documentation = "party wha has processed the data in a manner such that the resource has been modified";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("publisher", _stringObjInfo);
            //pi.Documentation = "";
            //newObjInfo.Properties.Add (pi);

            //pi = new PropInfo ("author", _stringObjInfo);
            //pi.Documentation = "party who authored the resource";
            //newObjInfo.Properties.Add (pi);

            //_objInfoList.Add (newObjInfo);
            //return newObjInfo;
        }

        private ObjectInfo Create_PhoneCodeType()
        {
            ObjectInfo tmp = GetByName("PhoneCodeType");
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.IsUsed = true;
            newObjInfo.Name = "PhoneCodeType";
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Namespace = "Aran.Aim.Enums";

            newObjInfo.AddProperty("Phone", _stringObjInfo);
            newObjInfo.AddProperty("Fax", _stringObjInfo);
            newObjInfo.AddProperty("Mobile", _stringObjInfo);
            newObjInfo.AddProperty("Other", _stringObjInfo);

            _objInfoList.Add(newObjInfo);
            return newObjInfo;
        }

        private ObjectInfo Create_LanguageCodeType()
        {
            var name = "LanguageCodeType";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.IsUsed = true;
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Namespace = "Aran.Aim.Enums";

            newObjInfo.AddProperty("English", _stringObjInfo);
            newObjInfo.AddProperty("Russian", _stringObjInfo);

            _objInfoList.Add(newObjInfo);
            return newObjInfo;
        }

        private ObjectInfo Create_RestrictionCode()
        {
            var name = "RestrictionCode";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.IsUsed = true;
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Namespace = "Aran.Aim.Enums";

            newObjInfo.AddProperty("Copyright", _stringObjInfo);
            newObjInfo.AddProperty("License", _stringObjInfo);
            newObjInfo.AddProperty("IntellectualPropertyRight", _stringObjInfo);
            newObjInfo.AddProperty("Restricted", _stringObjInfo);
            newObjInfo.AddProperty("OtherRestrictions", _stringObjInfo);

            _objInfoList.Add(newObjInfo);
            return newObjInfo;
        }

        private ObjectInfo Create_ClassificationCode()
        {
            var name = "ClassificationCode";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.IsUsed = true;
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Namespace = "Aran.Aim.Enums";

            newObjInfo.AddProperty("Unclassified", _stringObjInfo);
            newObjInfo.AddProperty("Restricted", _stringObjInfo);
            newObjInfo.AddProperty("Confidential", _stringObjInfo);
            newObjInfo.AddProperty("Secret", _stringObjInfo);
            newObjInfo.AddProperty("TopSecret", _stringObjInfo);
            newObjInfo.AddProperty("OtherHandles", _stringObjInfo);

            _objInfoList.Add(newObjInfo);
            return newObjInfo;
        }

        private ObjectInfo Create_CIDateTypeCode()
        {
            var name = "CIDateTypeCode";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
            {
                tmp.IsUsed = true;
                return tmp;
            }

            return null;

            //ObjectInfo newObjInfo = new ObjectInfo ();
            //newObjInfo.IsUsed = true;
            //newObjInfo.Name = name;
            //newObjInfo.Type = ObjectInfoType.Codelist;
            //newObjInfo.Namespace = "Aran.Aim.Enums";

            //newObjInfo.AddProperty ("Creation", _stringObjInfo);
            //newObjInfo.AddProperty ("Publication", _stringObjInfo);
            //newObjInfo.AddProperty ("Revision", _stringObjInfo);

            //_objInfoList.Add (newObjInfo);
            //return newObjInfo;
        }

        private ObjectInfo Create_MeasureClassCode()
        {
            var name = "MeasureClassCode";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            ObjectInfo newObjInfo = new ObjectInfo();
            newObjInfo.IsUsed = true;
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Namespace = "Aran.Aim.Enums";

            newObjInfo.AddProperty("Defined", _stringObjInfo);
            newObjInfo.AddProperty("Calculated", _stringObjInfo);
            newObjInfo.AddProperty("Derived", _stringObjInfo);
            newObjInfo.AddProperty("Surveyed", _stringObjInfo);

            _objInfoList.Add(newObjInfo);
            return newObjInfo;
        }

        private ObjectInfo Create_MDProgressCode()
        {
            var name = "MDProgressCode";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
            {
                tmp.IsUsed = true;
                return tmp;
            }

            return null;

            //ObjectInfo newObjInfo = new ObjectInfo ();
            //newObjInfo.IsUsed = true;
            //newObjInfo.Name = name;
            //newObjInfo.Type = ObjectInfoType.Codelist;
            //newObjInfo.Namespace = "Aran.Aim.Enums";

            //newObjInfo.AddProperty ("Completed", _stringObjInfo);
            //newObjInfo.AddProperty ("HistoricalArchive", _stringObjInfo);
            //newObjInfo.AddProperty ("Obsolete", _stringObjInfo);
            //newObjInfo.AddProperty ("OnGoing", _stringObjInfo);
            //newObjInfo.AddProperty ("Planned", _stringObjInfo);
            //newObjInfo.AddProperty ("Required", _stringObjInfo);
            //newObjInfo.AddProperty ("UnderDevelopment", _stringObjInfo);

            //_objInfoList.Add (newObjInfo);
            //return newObjInfo;
        }

        private ObjectInfo Create_DQEvaluationMethodTypeCode()
        {
            var name = "DQEvaluationMethodTypeCode";

            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
            {
                tmp.IsUsed = true;
                return tmp;
            }

            return tmp;

            //ObjectInfo newObjInfo = new ObjectInfo ();
            //newObjInfo.IsUsed = true;
            //newObjInfo.Name = name;
            //newObjInfo.Type = ObjectInfoType.Codelist;
            //newObjInfo.Namespace = "Aran.Aim.Enums";

            //newObjInfo.AddProperty ("DirectInternal", _stringObjInfo);
            //newObjInfo.AddProperty ("DirectExternal", _stringObjInfo);
            //newObjInfo.AddProperty ("Indirect", _stringObjInfo);

            //_objInfoList.Add (newObjInfo);
            //return newObjInfo;
        }

        #endregion
    }
}
