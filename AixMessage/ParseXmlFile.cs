using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Geometries.Operators;

namespace Aran.Aim.AixmMessage
{
    public class ParseXmlFile
    {
        public ParseXmlFile()
        {
            Features = new List<Feature>();
            ErrorInfoList = new List<DeserializedErrorInfo>();
        }

        public List<Feature> Features { get; private set; }

        public List<DeserializedErrorInfo> ErrorInfoList { get; private set; }

        public void Parse(string xmlFileName, bool keepDublicated = false)
        {
            Features.Clear();
            ErrorInfoList.Clear();
            DeserializeLastException.ErrorInfoList.Clear();

            var stream = File.OpenRead(xmlFileName);
            var xmlReader = XmlReader.Create(stream);

            var absFeatRefList = new List<AbstractFeatureRefBase>();

            Other.AbstractFeatureRefTypeReadingHandle.Handle = ((absFeat) => { absFeatRefList.Add(absFeat); });

            var message = new AixmBasicMessage(MessageReceiverType.Panda);
            message.ReadXml(xmlReader);

            ErrorInfoList.AddRange(message.ErrorInfoList);

            stream.Close();
            stream.Dispose();

            Other.AbstractFeatureRefTypeReadingHandle.Handle = null;

            var airspaceDictList = new Dictionary<Guid, Airspace>();

            var featTypeDict = new Dictionary<Guid, FeatureType>();
            foreach (var hasMem in message.HasMember)
            {
                if (hasMem.Count > 0)
                {
                    if (hasMem.Identifier == Guid.Empty)
                    {
                        continue;
                    }

                    var featType = hasMem[0].FeatureType;

                    if (featTypeDict.ContainsKey(hasMem.Identifier))
                    {
						ErrorInfoList.Add (new DeserializedErrorInfo
						{
							Identifier = hasMem.Identifier,
							FeatureType = hasMem[0].FeatureType,
							ErrorMessage = "Identifier is duplicated with " + featTypeDict[hasMem.Identifier],
                            Action = "Feature Ignored",
                            ErrorType=CodeErrorType.IdentifierDuplicated
						});
                        continue;
                    }

                    featTypeDict.Add(hasMem.Identifier, featType);

                    if (keepDublicated)
                        Features.AddRange(hasMem);
                    else
                        Features.Add(hasMem[0]);

                    if (featType == FeatureType.Airspace)
                        airspaceDictList.Add(hasMem.Identifier, hasMem[0] as Airspace);
                }
            }

            foreach (IAbstractFeatureRef afr in absFeatRefList)
            {
                FeatureType featType;
                if (featTypeDict.TryGetValue(afr.Identifier, out featType))
                    afr.FeatureTypeIndex = (int)featType;
            }

            //UnionAirpsaceGeometries(airspaceDictList);

            if (DeserializeLastException.ErrorInfoList.Count > 0)
                ErrorInfoList.AddRange(DeserializeLastException.ErrorInfoList);
        }

        private void UnionAirpsaceGeometries(Dictionary<Guid, Airspace> airspaceDictList)
        {
            var processingAirspaceList = new List<Airspace>(airspaceDictList.Values);
            var step = 0;

            while (processingAirspaceList.Count > 0 && step < 1 )
            {
                step++;
                var tmpList = new List<Airspace>(processingAirspaceList);

                foreach (var airsp in tmpList)
                {
                    if (airsp.GeometryComponent.Count > 1 && airsp.GeometryComponent[0].Operation == CodeAirspaceAggregation.BASE)
                    {
                        MultiPolygon unionGeometry = null;
                        AirspaceVolume baseVolume = airsp.GeometryComponent[0].TheAirspaceVolume;

                        if (baseVolume == null)
                            continue;

                        for (int i = 0; i < airsp.GeometryComponent.Count; i++)
                        {
                            var volume = airsp.GeometryComponent[i].TheAirspaceVolume;

                            if (volume == null ||
                                volume.ContributorAirspace == null ||
                                volume.ContributorAirspace.TheAirspace == null)
                            {
                                continue;
                            }

                            var otherPolygon = GetAirpsaceGeometry(airspaceDictList, volume.ContributorAirspace.TheAirspace.Identifier);

                            if (otherPolygon != null)
                            {
                                processingAirspaceList.Remove(airsp);

                                if (unionGeometry == null)
                                {
                                    unionGeometry = otherPolygon;
                                }
                                else
                                {
                                    if (airsp.GeometryComponent[i].Operation == CodeAirspaceAggregation.UNION)
                                    {
                                        var geom = GeometryOperators.Instance.UnionGeometry(unionGeometry, otherPolygon);

                                        if (geom.Type == GeometryType.MultiPolygon)
                                            unionGeometry = geom as MultiPolygon;

                                    }
                                }
                            }
                        }

                        if (unionGeometry != null)
                        {
                            baseVolume.HorizontalProjection = new Surface();
                            baseVolume.HorizontalProjection.Geo.Assign(unionGeometry);
                        }
                        else
                        {
                            ErrorInfoList.Add(new DeserializedErrorInfo
                            {
                                FeatureType = FeatureType.Airspace,
                                Identifier = airsp.Identifier,
                                PropertyName = "geometryComponent/theAirspaceVolume/contributorAirspace/theAirspace",
                                ErrorMessage = "Base Geometry is null",
                                ErrorType=CodeErrorType.GeometryNull
                            });
                        }
                    }
                    else
                    {
                        processingAirspaceList.Remove(airsp);
                    }
                }
            }
        }

        private MultiPolygon GetAirpsaceGeometry(Dictionary<Guid, Airspace> airspaceDictList, Guid idetifier)
        {
            Airspace airsp;
            if (!airspaceDictList.TryGetValue(idetifier, out airsp))
                return null;

            if (airsp.GeometryComponent.Count == 0 ||
                airsp.GeometryComponent[0].TheAirspaceVolume == null ||
                airsp.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection == null)
            {
                return null;
            }

            return airsp.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo;
        }
    }
}
