using System.Runtime.InteropServices;

namespace UMLInfo
{
    public partial class ObjectInfoParser
    {
        void CreateIsoMetadataObjects()
        {
            CreateIsoMetadataEnums();
            CreateBtAbstractObject();
            CreateBtString();
            CreateBtDateTime();
            CreateBtAbstractTimePrimitive();
            CreateBtTimePosition();
            CreateBtTimePeriod();
            CreateCiTelephone();
            CreateCiAddress();
            CreateCiOnlineResource();
            CreateCiContact();
            CreateCiResponsibleParty();
            CreateMdIdentifier();
            CreateCiPresentationFormCodeObject();
            CreateCiSeries();
            CreateCiCitation();
            CreateMdConstraints();
            CreateMdConstraintsObject();
            CreateMdRestrictionCodeObject();
            CreateMdLegalConstraints();
            CreateMdSecurityConstraints();
            CreateMdProgressCodeObject();
            CreateMdAbstractIdentification();
            CreateMdAbstractIdentificationObject();
            CreateExTemporalExtent();
            CreateExVerticalExtent();
            CreateExExtent();
            CreateMdDataIdentification();
            CreateDqAbstractResult();
            CreateDqAbstractResultObject();
            CreateDqQuantitativeResult();
            CreateDqAbstractElement();
            CreateDqAbstractElementObject();
            CreateDqQuantitativeAttributeAccuracy();
            CreateRsIdentifier();
            CreateLiSource();
            CreateLiProcessStep();
            CreateLiLineage();
            CreateDqDataQuality();
            CreateMdMetadata();
        }

        ObjectInfo CreateMdObjectInfo(string name, ObjectInfo baseType = null)
        {
            var item = new ObjectInfo();
            item.IsUsed = true;
            item.Name = name;
            item.Namespace = "Aran.Aim.Metadata.ISO";
            item.SubDir = "Metadata\\ISO";
            item.Type = ObjectInfoType.Object;
            item.Base = baseType;
            item.Documentation = "Information about the metadata";

            return item;
        }

        ObjectInfo CreateBtAbstractObject()
        {
            string name = "BtAbstractObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);
            item.IsAbstract = true;

            item.AddProperty("MetadataId", _stringObjInfo);
            item.AddProperty("Uuid", _guidObjectInfo);
            item.AddProperty("UuidRef", _guidObjectInfo);

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateBtString()
        {
            string name = "BtString";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", _stringObjInfo);

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateBtDateTime()
        {
            string name = "BtDateTime";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("dateTime"));
            item.AddProperty("DateType", GetByName("CiDateTypeCode"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateBtAbstractTimePrimitive()
        {
            string name = "BtAbstractTimePrimitive";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);
            item.IsAbstract = true;

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateBtTimePosition()
        {
            string name = "BtTimePosition";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractTimePrimitive"));

            item.AddProperty("Value", GetByName("dateTime"));
            item.AddProperty("IndeterminatePosition", GetByName("MdTimeIndeterminateValueType"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateBtTimePeriod()
        {
            string name = "BtTimePeriod";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractTimePrimitive"));

            item.AddProperty("BeginPosition", GetByName("BtTimePosition"));
            item.AddProperty("EndPosition", GetByName("BtTimePosition"));
            item.AddProperty("Duration", _stringObjInfo);

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiTelephone()
        {
            string name = "CiTelephone";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddListProperty("Voice", GetByName("BtString"));
            item.AddListProperty("Favsimile", GetByName("BtString"));
            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiAddress()
        {
            string name = "CiAddress";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddListProperty("DeliveryPoint", GetByName("BtString"));
            item.AddProperty("City", GetByName("string"));
            item.AddProperty("AdministrativeArea", GetByName("string"));
            item.AddProperty("PostalCode", GetByName("string"));
            item.AddProperty("Country", GetByName("string"));
            item.AddListProperty("ElectronicMailAddress", GetByName("BtString"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiOnlineResource()
        {
            string name = "CiOnlineResource";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Linkage", GetByName("string"));
            item.AddProperty("Protocol", GetByName("string"));
            item.AddProperty("ApplicationProfile", GetByName("string"));
            item.AddProperty("Name", GetByName("string"));
            item.AddProperty("Description", GetByName("string"));
            item.AddProperty("Function", GetByName("MdOnLineFunctionCode"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiContact()
        {
            string name = "CiContact";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Phone", GetByName("CiTelephone"));
            item.AddProperty("Address", GetByName("CiAddress"));
            item.AddProperty("OnlineResource", GetByName("CiOnlineResource"));
            item.AddProperty("HoursOfService", GetByName("string"));
            item.AddProperty("ContactInstructions", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiResponsibleParty()
        {
            string name = "CiResponsibleParty";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("IndividualName", GetByName("string"));
            item.AddProperty("OrganizationName", GetByName("string"));
            item.AddProperty("PositionName", GetByName("string"));
            item.AddProperty("ContactInfo", GetByName("CiContact"));
            item.AddProperty("RoleCode", GetByName("CiRoleCode"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdIdentifier()
        {
            string name = "MdIdentifier";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Authority", new ObjectInfo
            {
                Name = "CiCitation",
                Base = GetByName("BtAbstractObject"),
                Namespace = item.Namespace,
                Type = ObjectInfoType.Object
            });
            item.AddProperty("Code", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiPresentationFormCodeObject()
        {
            string name = "CiPresentationFormCodeObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("CiPresentationFormCode"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiSeries()
        {
            string name = "CiSeries";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Name", GetByName("string"));
            item.AddProperty("IssueIdentification", GetByName("string"));
            item.AddProperty("Page", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateCiCitation()
        {
            string name = "CiCitation";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Title", GetByName("string"));
            item.AddListProperty("AlternativeTitle", GetByName("BtString"));
            item.AddListProperty("Date", GetByName("BtDateTime"));
            item.AddProperty("Edition", GetByName("string"));
            item.AddProperty("EditionDate", GetByName("dateTime"));
            item.AddListProperty("Identifier", GetByName("MdIdentifier"));
            item.AddListProperty("CitedResponsibleParty", GetByName("CiResponsibleParty"));
            item.AddListProperty("PresentationForm", GetByName("CiPresentationFormCodeObject"));
            item.AddProperty("Series", GetByName("CiSeries"));
            item.AddProperty("OtherCitationDetails", GetByName("string"));
            item.AddProperty("CollectiveTitle", GetByName("string"));
            item.AddProperty("ISBN", GetByName("string"));
            item.AddProperty("ISSN", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdConstraints()
        {
            string name = "MdConstraints";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            item.IsAbstract = true;

            item.AddListProperty("UseLimitation", GetByName("BtString"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdConstraintsObject()
        {
            string name = "MdConstraintsObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("MdConstraints"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdRestrictionCodeObject()
        {
            string name = "MdRestrictionCodeObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("MdRestrictionCode"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdLegalConstraints()
        {
            string name = "MdLegalConstraints";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("MdConstraints"));

            item.AddListProperty("AccessConstraints", GetByName("MdRestrictionCodeObject"));
            item.AddListProperty("UseConstraints", GetByName("MdRestrictionCodeObject"));
            item.AddListProperty("OtherConstraintsField", GetByName("BtString"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdSecurityConstraints()
        {
            string name = "MdSecurityConstraints";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("MdConstraints"));

            item.AddProperty("AccessConstraints", GetByName("MdClassificationCode"));
            item.AddProperty("UserNote", GetByName("string"));
            item.AddProperty("ClassificationSystemField", GetByName("string"));
            item.AddProperty("HandlingDescriptionField", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdProgressCodeObject()
        {
            string name = "MdProgressCodeObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("MdProgressCode"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdAbstractIdentification()
        {
            string name = "MdAbstractIdentification";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            item.IsAbstract = true;

            item.AddProperty("Citation", GetByName("CiCitation"));
            item.AddProperty("Abstract", GetByName("string"));
            item.AddProperty("Purpose", GetByName("string"));
            item.AddListProperty("Credit", GetByName("BtString"));
            item.AddListProperty("Status", GetByName("MdProgressCodeObject"));
            item.AddListProperty("PointOfContact", GetByName("CiResponsibleParty"));
            item.AddListProperty("ResourceConstraints", GetByName("MdConstraintsObject"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdAbstractIdentificationObject()
        {
            string name = "MdAbstractIdentificationObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("MdAbstractIdentification"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateExTemporalExtent()
        {
            string name = "ExTemporalExtent";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Extent", GetByName("BtAbstractTimePrimitive"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateExVerticalExtent()
        {
            string name = "ExVerticalExtent";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("MinimumValue", GetByName("decimal"));
            item.AddProperty("MaximumValue", GetByName("decimal"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateExExtent()
        {
            string name = "ExExtent";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));

            item.AddProperty("Description", GetByName("string"));
            item.AddListProperty("TemporalElement", GetByName("ExTemporalExtent"));
            item.AddListProperty("VerticalElement", GetByName("ExVerticalExtent"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdDataIdentification()
        {
            string name = "MdDataIdentification";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("MdAbstractIdentification"));

            item.AddProperty("Language", GetByName("string"));
            item.AddProperty("EnvironmentDescription", GetByName("string"));
            item.AddListProperty("Extent", GetByName("ExExtent"));
            item.AddProperty("SupplementalInformation", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqAbstractResult()
        {
            string name = "DqAbstractResult";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            item.IsAbstract = true;

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqAbstractResultObject()
        {
            string name = "DqAbstractResultObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("DqAbstractResult"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqQuantitativeResult()
        {
            string name = "DqQuantitativeResult";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("DqAbstractResult"));

            item.AddProperty("ErrorStatistic", GetByName("string"));
            item.AddListProperty("Value", GetByName("BtString"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqAbstractElement()
        {
            string name = "DqAbstractElement";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            item.IsAbstract = true;

            item.AddListProperty("NameOfMeasure", GetByName("BtString"));
            item.AddProperty("MeasureIdentification", GetByName("MdIdentifier"));
            item.AddProperty("MeasureDescription", GetByName("string"));
            item.AddProperty("EvaluationMethodType", GetByName("DqEvaluationMethodTypeCode"));
            item.AddProperty("EvaluationMethodDescription", GetByName("string"));
            item.AddProperty("EvaluationProcedure", GetByName("CiCitation"));
            item.AddListProperty("DateTime", GetByName("BtDateTime"));
            item.AddListProperty("Result", GetByName("DqAbstractResultObject"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqAbstractElementObject()
        {
            string name = "DqAbstractElementObject";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name);

            item.AddProperty("Value", GetByName("DqAbstractElement"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqQuantitativeAttributeAccuracy()
        {
            string name = "DqQuantitativeAttributeAccuracy";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("DqAbstractElement"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateRsIdentifier()
        {
            string name = "RsIdentifier";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("MdIdentifier"));
            
            item.AddProperty("CodeSpace", GetByName("string"));
            item.AddProperty("Version", GetByName("string"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateLiSource()
        {
            string name = "LiSource";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            
            item.AddProperty("Description", GetByName("string"));
            item.AddProperty("ScaleDenominator", GetByName("integer"));
            item.AddProperty("ReferenceSystem", GetByName("RsIdentifier"));
            item.AddProperty("SourceCitation", GetByName("CiCitation"));
            item.AddListProperty("SourceExtent", GetByName("ExExtent"));
            item.AddListProperty("SourceStep", new ObjectInfo
            {
                Name = "LiProcessStep",
                Namespace = item.Namespace,
                Type = ObjectInfoType.Object
            });

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateLiProcessStep()
        {
            string name = "LiProcessStep";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            
            item.AddProperty("Description", GetByName("string"));
            item.AddProperty("Rationale", GetByName("string"));
            item.AddProperty("DateTime", GetByName("dateTime"));
            item.AddListProperty("Processor", GetByName("CiResponsibleParty"));
            item.AddListProperty("Source", GetByName("LiSource"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateLiLineage()
        {
            string name = "LiLineage";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            
            item.AddProperty("Statement", GetByName("string"));
            item.AddListProperty("ProcessStep", GetByName("LiProcessStep"));
            item.AddListProperty("Source", GetByName("LiSource"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateDqDataQuality()
        {
            string name = "DqDataQuality";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            
            item.AddListProperty("Report", GetByName("DqAbstractElementObject"));
            item.AddProperty("Lineage", GetByName("LiLineage"));

            _objInfoList.Add(item);
            return item;
        }

        ObjectInfo CreateMdMetadata()
        {
            string name = "MdMetadata";
            var item = GetByName(name);
            if (item != null)
                return item;

            item = CreateMdObjectInfo(name, GetByName("BtAbstractObject"));
            
            item.AddProperty("FileIdentifier", GetByName("string"));
            item.AddProperty("Language", GetByName("string"));
            item.AddProperty("ParentIdentifier", GetByName("string"));
            item.AddListProperty("Contact", GetByName("CiResponsibleParty"));
            item.AddProperty("DateStamp", GetByName("dateTime"));
            item.AddProperty("MetadataStandardName", GetByName("string"));
            item.AddProperty("MetadataStandardVersion", GetByName("string"));
            item.AddProperty("DataSetUri", GetByName("string"));
            item.AddListProperty("ReferenceSystemInfo", GetByName("RsIdentifier"));
            item.AddListProperty("IdentificationInfo", GetByName("MdAbstractIdentificationObject"));
            item.AddListProperty("DataQualityInfo", GetByName("DqDataQuality"));
            item.AddListProperty("MetadataConstraints", GetByName("MdConstraintsObject"));

            _objInfoList.Add(item);
            return item;
        }
    }
}