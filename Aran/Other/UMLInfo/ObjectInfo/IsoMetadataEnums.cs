using System.Runtime.InteropServices;

namespace UMLInfo
{
    public partial class ObjectInfoParser
    {
        void CreateIsoMetadataEnums()
        {
            CreateMdEnumType("MdOnLineFunctionCode", "Download", "Information", "OfflineAccess", "Order", "Search");

            CreateMdEnumType("CiDateTypeCode", "Creation", "Publication", "Revision", "Adopted", "Deprecated", "Distribution", "Expiry", 
                "InForce", "LastRevision", "LastUpdate", "NextUpdate", "Released", "Superseded", "Unavailable", "ValidityBegins", "ValidityExpires");

            CreateMdEnumType("CiRoleCode", "ResourceProvider", "Custodian", "Owner", "User", "Distributor",
                "Originator", "PointOfContact", "PrincipalInvestigator", "Processor", "Publisher", "Author");

            CreateMdEnumType("CiPresentationFormCode", "DocumentDigital", "DocumentHardcopy", "ImageDigital",
                "ImageHardcopy", "MapDigial", "MapHardcopy", "ModelDigital", "ModelHardcopy", "ProfileDigital", 
                "ProfileHardcopy", "TableDigital", "TableHardcopy", "VideoDigital", "VideoHardcopy");

            CreateMdEnumType("MdTimeIndeterminateValueType", "After", "Before", "Now", "Unknown");

            CreateMdEnumType("DqEvaluationMethodTypeCode", "DirectInternal", "DirectExternal", "Indirect");

            CreateMdEnumType("MdRestrictionCode", "Copyright", "Patent", "PatentPending", "Trademark", "License",
                "IntellectualPropertyRights", "Restricted", "OtherRestrictions");

            CreateMdEnumType("MdClassificationCode", "Unclassified", "Restricted", "Confidential", "Secret", "TopSecret");

            CreateMdEnumType("MdProgressCode", "Completed", "HistoricalArchive", "Obsolete", "OnGoing", "Planned",
                "Required", "UnderDevelopment");
        }

        ObjectInfo CreateMdEnumType(string name, params string[] values)
        {
            ObjectInfo tmp = GetByName(name);
            if (tmp != null)
                return tmp;

            var newObjInfo = new ObjectInfo();
            newObjInfo.IsUsed = true;
            newObjInfo.Name = name;
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Namespace = "Aran.Aim.Enums";

            foreach (var value in values)
            {
                newObjInfo.AddProperty(value, _stringObjInfo);
            }

            _objInfoList.Add(newObjInfo);
            return newObjInfo;
        }
    }
}