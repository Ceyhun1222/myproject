using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.ISO;
using Aran.Metadata.Forms.Views;
using Aran.Metadata.Utils;
using Aran.Temporality.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Extensions;

namespace Aran.Metadata.Forms.Classes
{
    public class MetadataBuilder
    {

        #region Fields

        private readonly List<Feature> _featureList;
        private readonly User _user;
        private readonly List<User> _originators;
        private readonly string _coordinateSystemReferenceName;
        private readonly List<GeoNumericalDataModel> _geoNumericalData;

        #endregion

        #region Properties

        #endregion

        #region Ctors

        public MetadataBuilder(List<Feature> featureList, object user, 
            List<User> originators, string coordinateSystemReferenceName, 
            List<GeoNumericalDataModel> geoNumericalData)
        {
            _featureList = featureList;
            _user = user as User;
            _coordinateSystemReferenceName = coordinateSystemReferenceName;
            _geoNumericalData = geoNumericalData;
            _originators = originators;
        }

        #endregion

        #region Public methods

        public void Build()
        {
            if (_featureList?.Count > 0)
            {
                try
                {
                    CommonPrepare();
                }
                catch (Exception e)
                {
                    LogManager.GetLogger(typeof(MetadataBuilder)).Error(e, e.Message);
                    throw;
                }

                var metadata = new Main(_featureList.Select(x => x.Metadata).ToList(), _originators);
                metadata.ShowDialog();
            }
        }

        #endregion

        #region Private methods

        private void CommonPrepare()
        {
            foreach (var feature in _featureList)
            {
                if(feature == null)
                    LogManager.GetLogger(typeof(MetadataBuilder)).Error("Feature in metadata is null");


                LogManager.GetLogger(typeof(MetadataBuilder)).Debug($"Common prepare started. Feature: " +
                    $"{feature?.Identifier}, {feature?.FeatureType}, {feature?.TimeSlice?.Interpretation}, " +
                    $"{feature?.TimeSlice?.SequenceNumber}.{feature?.TimeSlice?.CorrectionNumber}");

                LogManager.GetLogger(typeof(MetadataBuilder)).Debug($"Set Metadata Identification Info: {_user.Dump()}");
                feature.SetMetadataIdentificationInfo(_user, CiRoleCode.PointOfContact);

                LogManager.GetLogger(typeof(MetadataBuilder)).Debug("Set Metadata Amendments");
                feature.SetMetadataAmendments(feature.TimeSlice.SequenceNumber == 1 && feature.TimeSlice.CorrectionNumber == 0);

                LogManager.GetLogger(typeof(MetadataBuilder)).Debug("Set Metadata Effective Date");
                feature.SetMetadataEffectiveDate();

                LogManager.GetLogger(typeof(MetadataBuilder)).Debug($"Set Numerical Data: {_geoNumericalData.Dump()}");
                feature.SetNumericalData(_geoNumericalData);

                LogManager.GetLogger(typeof(MetadataBuilder)).Debug($"Set Coordinate System Reference: {_coordinateSystemReferenceName.Dump()}");
                feature.SetCoordinateSystemReference(_coordinateSystemReferenceName);
            }
        }

        private void SetIdentificationInfo(MdMetadata metadata)
        {
            var identificationObject = new MdAbstractIdentificationObject();

            if (_user == null)
                return;

            var ciResponsibleParty = new CiResponsibleParty()
            {
                IndividualName = _user.Name + " " + _user.Surname,
                OrganizationName = _user.Organization,
                PositionName = _user.Position,
                ContactInfo = new CiContact()
                {
                    Phone = new CiTelephone(),
                    Address = new CiAddress()
                    {
                        City = _user.City,
                        PostalCode = _user.Zip,
                        Country = _user.Country
                    },
                },
            };

            var dataIdentiifcation = new MdDataIdentification();

            ciResponsibleParty.ContactInfo.Phone.Voice.Add(new BtString() { Value = _user.FaxNumber });
            ciResponsibleParty.ContactInfo.Address.DeliveryPoint.Add(new BtString() { Value = _user.Address });
            ciResponsibleParty.ContactInfo.Address.ElectronicMailAddress.Add(new BtString() { Value = _user.Email });

            identificationObject.Value = dataIdentiifcation;
            identificationObject.Value.PointOfContact.Add(ciResponsibleParty);

            metadata.IdentificationInfo.Add(identificationObject);
        }

        private void SetEffectiveDate(MdMetadata metadata, DateTime effectiveDate)
        {
            var exExtent = new ExExtent();
            exExtent.TemporalElement.Add(new ExTemporalExtent
            {
                Extent = new BtTimePeriod
                {
                    BeginPosition = new BtTimePosition
                    {
                        Value = effectiveDate
                    }
                }
            });

            var dataIdentification = metadata.IdentificationInfo.First().Value as MdDataIdentification;
            dataIdentification.Extent.Add(exExtent);
        }

        private void SetAmendments(MdMetadata metadata, CiDateTypeCode typeCode)
        {
            var citation = new CiCitation();
            citation.Date.Add( new BtDateTime
            {
                Value = DateTime.Now,
                DateType = typeCode
            });

            metadata.IdentificationInfo.First().Value.Citation = citation;
        }

        private void SetNumericalData(MdMetadata metadata)
        {
            if(_geoNumericalData != null)
            {
                foreach (var data in _geoNumericalData)
                {
                    var dataQuality = new DqDataQuality();

                    if (data.Resolution > 0)
                    {
                        var resolutionObject = new DqAbstractElementObject { Value = new DqQuantitativeAttributeAccuracy() };

                        var resolutionResult = new DqQuantitativeResult();
                        resolutionResult.Value.Add(new BtString { Value = Math.Ceiling(data.Resolution).ToString() });

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
                    accuracyResult.Value.Add(new BtString { Value = Math.Ceiling(data.Accuracy).ToString() });

                    accuracyObject.Value.NameOfMeasure.Add(new BtString { Value = "Accuracy" });
                    if(data.LegType != null)
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

        #endregion
    }
}
