using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.ISO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Metadata.Utils
{
    public static class FeatureMetadataExtensions
    {
        private static string GetProperty(dynamic settings, string name)
        {
            try
            {
                if (settings == null)
                    return "";

                if (settings is ExpandoObject)
                    return ((IDictionary<string, object>)settings)[name].ToString();

                return settings.GetType().GetProperty(name).GetValue(settings, null);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void SetMetadataIdentificationInfo(this Feature feature, object user, CiRoleCode userRoleCode)
        {
            try
            {
                if (feature == null)
                    return;

                if (feature.Metadata == null)
                    feature.Metadata = new MdMetadata();

                feature.Metadata.SetMetadataIdentificationInfo(user, userRoleCode);
            }
            catch
            {
                // TODO: logs
            }
        }

        public static string GetMetadataIdentificationCode(this MdMetadata metadata, CiRoleCode userRoleCode)
        {
            return metadata?.IdentificationInfo?.FirstOrDefault()?.Value?.PointOfContact
                ?.Where(x => x.RoleCode == userRoleCode)?.FirstOrDefault()?.IndividualName;
        }
        
        public static string GetMetadataIdentificationCode(this Feature feature, CiRoleCode userRoleCode)
        {
            return feature?.Metadata?.IdentificationInfo?.FirstOrDefault()?.Value?.PointOfContact
                ?.Where(x => x.RoleCode == userRoleCode)?.FirstOrDefault()?.IndividualName;
        }

        public static void SetMetadataIdentificationInfo(this MdMetadata metadata, object user, CiRoleCode userRoleCode)
        {
            try
            {
                if (!metadata.IdentificationInfo.Any())
                    metadata.IdentificationInfo.Add(new MdAbstractIdentificationObject { Value = new MdDataIdentification() });

                var identificationObject = metadata.IdentificationInfo.First();

                if (user == null)
                {
                    identificationObject.Value.PointOfContact.RemoveAll(x => x.RoleCode == userRoleCode);
                    return;
                }

                if (identificationObject.Value.PointOfContact.All(pc => pc.RoleCode != userRoleCode))
                {
                    identificationObject.Value.PointOfContact.Add(new CiResponsibleParty
                    {
                        RoleCode = userRoleCode
                    });
                }

                var ciResponsibleParty = metadata.IdentificationInfo.First().Value.PointOfContact?
                    .Where(pc => pc.RoleCode == userRoleCode).FirstOrDefault();

                if (ciResponsibleParty.ContactInfo == null)
                    ciResponsibleParty.ContactInfo = new CiContact();

                var d = (dynamic)user;

                ciResponsibleParty.IndividualName = GetProperty(d, "DataOriginatorCode");
                ciResponsibleParty.OrganizationName = GetProperty(d, "Organization");
                ciResponsibleParty.PositionName = GetProperty(d, "Position");
                ciResponsibleParty.RoleCode = userRoleCode;

                if (ciResponsibleParty.ContactInfo.Phone == null)
                    ciResponsibleParty.ContactInfo.Phone = new CiTelephone();

                ciResponsibleParty.ContactInfo.Phone.Voice.Clear();
                ciResponsibleParty.ContactInfo.Phone.Voice.Add(new BtString() { Value = GetProperty(d, "FaxNumber") });

                if (ciResponsibleParty.ContactInfo.Address == null)
                    ciResponsibleParty.ContactInfo.Address = new CiAddress();

                ciResponsibleParty.ContactInfo.Address.City = GetProperty(d, "City");
                ciResponsibleParty.ContactInfo.Address.PostalCode = GetProperty(d, "Zip");
                ciResponsibleParty.ContactInfo.Address.Country = GetProperty(d, "Country");

                ciResponsibleParty.ContactInfo.Address.DeliveryPoint.Clear();
                ciResponsibleParty.ContactInfo.Address.DeliveryPoint.Add(new BtString() { Value = GetProperty(d, "Address") });

                ciResponsibleParty.ContactInfo.Address.ElectronicMailAddress.Clear();
                ciResponsibleParty.ContactInfo.Address.ElectronicMailAddress.Add(new BtString() { Value = GetProperty(d, "Email") });
            }
            catch
            {

            }
        }

        public static void SetMetadataEffectiveDate(this Feature feature)
        {
            try
            {
                if (feature == null)
                    return;

                if (feature.Metadata == null)
                    feature.Metadata = new MdMetadata();

                if (feature.Metadata.IdentificationInfo == null)
                    feature.Metadata.IdentificationInfo.Add(new MdAbstractIdentificationObject { Value = new MdDataIdentification() });

                var dataIdentification = feature.Metadata.IdentificationInfo.First().Value as MdDataIdentification;

                if (dataIdentification.Extent.Count == 0)
                    dataIdentification.Extent.Add(new ExExtent());

                if (dataIdentification.Extent.First().TemporalElement.Count == 0)
                    dataIdentification.Extent.First().TemporalElement.Add(new ExTemporalExtent());

                dataIdentification.Extent.First().TemporalElement.First().Extent = new BtTimePeriod
                {
                    BeginPosition = new BtTimePosition
                    {
                        Value = feature?.TimeSlice?.ValidTime?.BeginPosition
                    }
                };
            }
            catch
            {
                // TODO: logs
            }
        }

        public static void SetMetadataAmendments(this Feature feature, bool? creationNewFeature = null)
        {
            try
            {
                if (feature == null)
                    return;

                if (feature.Metadata == null)
                    feature.Metadata = new MdMetadata();

                if (feature.Metadata.IdentificationInfo == null)
                    feature.Metadata.IdentificationInfo.Add(new MdAbstractIdentificationObject { Value = new MdDataIdentification() });

                CiDateTypeCode type = CiDateTypeCode.LastUpdate;
                if (creationNewFeature == null)
                {
                    if (feature.TimeSlice.SequenceNumber == 1 && feature.TimeSlice.CorrectionNumber == 0)
                        type = CiDateTypeCode.Creation;
                    else
                        type = CiDateTypeCode.LastUpdate;
                }
                else
                {
                    if (creationNewFeature == true)
                        type = CiDateTypeCode.Creation;
                    else
                        type = CiDateTypeCode.LastUpdate;
                }

                var citation = new CiCitation();
                citation.Date.Add(new BtDateTime
                {
                    Value = DateTime.Now,
                    DateType = type
                });

                feature.Metadata.IdentificationInfo.First().Value.Citation = citation;
            }
            catch
            {
                // TODO: logs
            }
        }

        public static void SetNumericalData(this Feature feature, List<GeoNumericalDataModel> geoNumericalData)
        {
            if (feature == null)
                return;

            if (feature.Metadata == null)
                feature.Metadata = new MdMetadata();

            var metadata = feature.Metadata;

            if (geoNumericalData != null)
            {
                foreach (var data in geoNumericalData)
                {
                    var dataQuality = new DqDataQuality();

                    if (data.Resolution > 0)
                    {
                        var resolutionObject = new DqAbstractElementObject { Value = new DqQuantitativeAttributeAccuracy() };

                        var resolutionResult = new DqQuantitativeResult();
                        resolutionResult.Value.Add(new BtString { Value = Math.Ceiling(data.Resolution).ToString() + " " + data.ResolutionDimension });

                        resolutionObject.Value.NameOfMeasure.Add(new BtString { Value = "Resolution" });
                        if (data.LegType != null)
                        {
                            resolutionObject.Value.MeasureIdentification = new MdIdentifier
                            {
                                Code = data.LegType + "&" + data.Role
                            };
                            resolutionObject.Value.MeasureDescription = data.GetDescription();
                        }
                        resolutionObject.Value.Result.Add(new DqAbstractResultObject { Value = resolutionResult });

                        dataQuality.Report.Add(resolutionObject);
                    }

                    var accuracyObject = new DqAbstractElementObject { Value = new DqQuantitativeAttributeAccuracy() };

                    var accuracyResult = new DqQuantitativeResult();
                    accuracyResult.Value.Add(new BtString { Value = Math.Ceiling(data.Accuracy).ToString() + " " + data.AccuracyDimension });

                    accuracyObject.Value.NameOfMeasure.Add(new BtString { Value = "Accuracy" });
                    if (data.LegType != null)
                    {
                        accuracyObject.Value.MeasureIdentification = new MdIdentifier
                        {
                            Code = data.LegType + "&" + data.Role
                        };
                        accuracyObject.Value.MeasureDescription = data.GetDescription();
                    }
                    accuracyObject.Value.Result.Add(new DqAbstractResultObject { Value = accuracyResult });

                    dataQuality.Report.Add(accuracyObject);

                    metadata.DataQualityInfo.Add(dataQuality);
                }
            }
        }

        public static void SetCoordinateSystemReference(this Feature feature, string coordinateSystemReferenceName)
        {
            if (feature == null)
                return;

            if (feature.Metadata == null)
                feature.Metadata = new MdMetadata();

            ReferenceSystemUtil.SetCoordinateSystemReference(feature.Metadata, coordinateSystemReferenceName);
        }
    }
}
