using Aerodrome.Features;
using Aerodrome.Opc;
using Framework.Attributes;
using Framework.Stasy.SyncProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.Context
{
    public class ApplicationContext : EntityContext
    {
        public ApplicationContext(
            ISyncProvider syncProvider,
            IOpcDataService opcDataService)
            : base(syncProvider)
        {
            OpcDataService = opcDataService;
        }

        public IOpcDataService OpcDataService { get; private set; }

        public bool IsInitialized { get; protected set; }
        
        //public AM_AerodromeReferencePoint AerodromeReferencePoint { get; set; }

        //public CompositeCollection<AM_AerodromeReferencePoint> Airports { get; set; }

        //public CompositeCollection<AM_Apron> Aprons { get; set; }

        //public CompositeCollection<AM_Taxiway> Taxiways { get; set; }

        //public CompositeCollection<AM_ApronElement> ApronElements { get; set; }

        //public CompositeCollection<AM_TaxiwayElement> TaxiwayElements { get; set; }

        public List<TypeRelationInfo> FeatureRelations { get; set; }

        public Dictionary<Type, dynamic> FeatureCollections { get; set; }

        public void Initialize()
        {
            FeatureRelations = new List<TypeRelationInfo>();

            RegisterType<AM_AerodromeReferencePoint>();
                        
            RegisterType<AM_Runway>();
            RegisterType<AM_Taxiway>();
            RegisterType<AM_Apron>();
            RegisterType<AM_RunwayDirection>();
            RegisterType<AM_FinalApproachAndTakeOffArea>();
            RegisterType<AM_RunwayElement>();

            RegisterType<AM_LandAndHoldShortOperationLocation>();
            
            RegisterType<AM_Blastpad>();
            RegisterType<AM_ArrestingSystemLocation>();
            RegisterType<AM_ArrestingGearLocation>();
            RegisterType<AM_PaintedCenterline>();
            RegisterType<AM_RunwayCenterlinePoint>();
            RegisterType<AM_RunwayDisplacedArea>();
            RegisterType<AM_RunwayIntersection>();
            RegisterType<AM_RunwayExitLine>();
            RegisterType<AM_RunwayMarking>();
            RegisterType<AM_RunwayShoulder>();
            RegisterType<AM_RunwayThreshold>();
            RegisterType<AM_Stopway>();

            
            RegisterType<AM_TaxiwayElement>();            
            RegisterType<AM_FrequencyArea>();
            RegisterType<AM_PositionMarking>();
            RegisterType<AM_TaxiwayGuidanceLine>();
            RegisterType<AM_TaxiwayHoldingPosition>();           
            RegisterType<AM_TaxiwayIntersectionMarking>();
            RegisterType<AM_TaxiwayShoulder>();

            RegisterType<AM_VerticalPolygonalStructure>();
            RegisterType<AM_VerticalLineStructure>();
            RegisterType<AM_VerticalPointStructure>();

            RegisterType<AM_ApronElement>();
            RegisterType<AM_ParkingStandLocation>();
            RegisterType<AM_ParkingStandArea>();
            RegisterType<AM_DeicingGroup>();                        
            RegisterType<AM_StandGuidanceLine>();
            RegisterType<AM_DeicingArea>();
            RegisterType<AM_ServiceRoad>();

            
            RegisterType<AM_HelipadThreshold>();
            RegisterType<AM_TouchDownLiftOffArea>();

            RegisterType<AM_BridgeSide>();
            RegisterType<AM_ConstructionArea>();
            RegisterType<AM_ATCBlindSpot>();
            RegisterType<AM_Hotspot>();
            RegisterType<AM_AerodromeSign>();
            RegisterType<AM_SurveyControlPoint>();
            RegisterType<AM_Water>();

            RegisterType<AM_AerodromeSurfaceLighting>();

            RegisterType<AM_AsrnEdge>();
            RegisterType<AM_AsrnNode>();




            FeatureCollections = new Dictionary<Type, dynamic>();

            FeatureCollections.Add(typeof(AM_AerodromeReferencePoint), GetWriteableCollection<AM_AerodromeReferencePoint>());
            
            FeatureCollections.Add(typeof(AM_Runway), GetWriteableCollection<AM_Runway>());
            FeatureCollections.Add(typeof(AM_RunwayElement), GetWriteableCollection<AM_RunwayElement>());
            FeatureCollections.Add(typeof(AM_LandAndHoldShortOperationLocation), GetWriteableCollection<AM_LandAndHoldShortOperationLocation>());
            FeatureCollections.Add(typeof(AM_RunwayDirection), GetWriteableCollection<AM_RunwayDirection>());
            FeatureCollections.Add(typeof(AM_Blastpad), GetWriteableCollection<AM_Blastpad>());
            FeatureCollections.Add(typeof(AM_ArrestingSystemLocation), GetWriteableCollection<AM_ArrestingSystemLocation>());
            FeatureCollections.Add(typeof(AM_ArrestingGearLocation), GetWriteableCollection<AM_ArrestingGearLocation>());
            FeatureCollections.Add(typeof(AM_PaintedCenterline), GetWriteableCollection<AM_PaintedCenterline>());
            FeatureCollections.Add(typeof(AM_RunwayCenterlinePoint), GetWriteableCollection<AM_RunwayCenterlinePoint>());
            FeatureCollections.Add(typeof(AM_RunwayDisplacedArea), GetWriteableCollection<AM_RunwayDisplacedArea>());
            FeatureCollections.Add(typeof(AM_RunwayIntersection), GetWriteableCollection<AM_RunwayIntersection>());
            FeatureCollections.Add(typeof(AM_RunwayExitLine), GetWriteableCollection<AM_RunwayExitLine>());
            FeatureCollections.Add(typeof(AM_RunwayMarking), GetWriteableCollection<AM_RunwayMarking>());
            FeatureCollections.Add(typeof(AM_RunwayShoulder), GetWriteableCollection<AM_RunwayShoulder>());
            FeatureCollections.Add(typeof(AM_RunwayThreshold), GetWriteableCollection<AM_RunwayThreshold>());
            FeatureCollections.Add(typeof(AM_Stopway), GetWriteableCollection<AM_Stopway>());

            FeatureCollections.Add(typeof(AM_Taxiway), GetWriteableCollection<AM_Taxiway>());           
            FeatureCollections.Add(typeof(AM_TaxiwayElement), GetWriteableCollection<AM_TaxiwayElement>());
            FeatureCollections.Add(typeof(AM_BridgeSide), GetWriteableCollection<AM_BridgeSide>());
            FeatureCollections.Add(typeof(AM_FrequencyArea), GetWriteableCollection<AM_FrequencyArea>());
            FeatureCollections.Add(typeof(AM_PositionMarking), GetWriteableCollection<AM_PositionMarking>());
            FeatureCollections.Add(typeof(AM_TaxiwayHoldingPosition), GetWriteableCollection<AM_TaxiwayHoldingPosition>());
            FeatureCollections.Add(typeof(AM_TaxiwayGuidanceLine), GetWriteableCollection<AM_TaxiwayGuidanceLine>());
            FeatureCollections.Add(typeof(AM_TaxiwayIntersectionMarking), GetWriteableCollection<AM_TaxiwayIntersectionMarking>());
            FeatureCollections.Add(typeof(AM_TaxiwayShoulder), GetWriteableCollection<AM_TaxiwayShoulder>());

            FeatureCollections.Add(typeof(AM_Apron), GetWriteableCollection<AM_Apron>());
            FeatureCollections.Add(typeof(AM_ApronElement), GetWriteableCollection<AM_ApronElement>());
            FeatureCollections.Add(typeof(AM_DeicingArea), GetWriteableCollection<AM_DeicingArea>());
            FeatureCollections.Add(typeof(AM_DeicingGroup), GetWriteableCollection<AM_DeicingGroup>());
            FeatureCollections.Add(typeof(AM_ParkingStandLocation), GetWriteableCollection<AM_ParkingStandLocation>());
            FeatureCollections.Add(typeof(AM_ParkingStandArea), GetWriteableCollection<AM_ParkingStandArea>());
            FeatureCollections.Add(typeof(AM_StandGuidanceLine), GetWriteableCollection<AM_StandGuidanceLine>());
            FeatureCollections.Add(typeof(AM_ServiceRoad), GetWriteableCollection<AM_ServiceRoad>());

            FeatureCollections.Add(typeof(AM_FinalApproachAndTakeOffArea), GetWriteableCollection<AM_FinalApproachAndTakeOffArea>());
            FeatureCollections.Add(typeof(AM_HelipadThreshold), GetWriteableCollection<AM_HelipadThreshold>());
            FeatureCollections.Add(typeof(AM_TouchDownLiftOffArea), GetWriteableCollection<AM_TouchDownLiftOffArea>());

            FeatureCollections.Add(typeof(AM_ATCBlindSpot), GetWriteableCollection<AM_ATCBlindSpot>());
            FeatureCollections.Add(typeof(AM_Hotspot), GetWriteableCollection<AM_Hotspot>());

            FeatureCollections.Add(typeof(AM_AerodromeSign), GetWriteableCollection<AM_AerodromeSign>());

            FeatureCollections.Add(typeof(AM_SurveyControlPoint), GetWriteableCollection<AM_SurveyControlPoint>());

            FeatureCollections.Add(typeof(AM_ConstructionArea), GetWriteableCollection<AM_ConstructionArea>());

            FeatureCollections.Add(typeof(AM_Water), GetWriteableCollection<AM_Water>());

            FeatureCollections.Add(typeof(AM_AerodromeSurfaceLighting), GetWriteableCollection<AM_AerodromeSurfaceLighting>());

            FeatureCollections.Add(typeof(AM_AsrnNode), GetWriteableCollection<AM_AsrnNode>());
            FeatureCollections.Add(typeof(AM_AsrnEdge), GetWriteableCollection<AM_AsrnEdge>());

            FeatureCollections.Add(typeof(AM_VerticalPolygonalStructure), GetWriteableCollection<AM_VerticalPolygonalStructure>());
            FeatureCollections.Add(typeof(AM_VerticalLineStructure), GetWriteableCollection<AM_VerticalLineStructure>());
            FeatureCollections.Add(typeof(AM_VerticalPointStructure), GetWriteableCollection<AM_VerticalPointStructure>());


            #region old context
            //Airports = GetWriteableCollection<AM_AerodromeReferencePoint>();
            //Aprons = GetWriteableCollection<AM_Apron>();
            //ApronElements = GetWriteableCollection<AM_ApronElement>();
            //Taxiways = GetWriteableCollection<AM_Taxiway>();
            //TaxiwayElements = GetWriteableCollection<AM_TaxiwayElement>();
            #endregion

            //Здесь на основе FeatureRelations нужно линковать feature.
            //Разобраться в LinkObjects. Может пригодиться

            foreach (var typeRelation in FeatureRelations)
            {
                var featureForLinksApplied = typeRelation.RootFeature;
                foreach (var propertyForRelation in typeRelation.Relations)
                {
                    if (propertyForRelation.RelatedFeatIdList.Count == 0) continue;

                    if (!propertyForRelation.IsCollection)
                    {
         
                        if(propertyForRelation.RelatedPropertyInfo.PropertyType.Name.Equals(typeof(AM_AbstractFeature).Name))
                        {
                            AM_AbstractFeature relFeature = null;
                            var allovableTypesAttr = propertyForRelation.RelatedPropertyInfo.GetCustomAttribute(typeof(AllowableTypesAttribute));
                            if(allovableTypesAttr != null)
                            {
                                var allowableTypes = ((AllowableTypesAttribute)allovableTypesAttr).AllovableTypes;
                                foreach(Type type in allowableTypes)
                                {
                                    
                                    var storageByType = FeatureCollections[type];
                                    foreach (var feat in storageByType)
                                    {
                                        if (((AM_AbstractFeature)feat).featureID.Equals(propertyForRelation.RelatedFeatIdList.First()))
                                        {
                                            relFeature = (AM_AbstractFeature)feat;
                                            break;
                                            //после этого нужно выйти из цикла так как элемент нашли
                                        }
                                    }
                                }
                            }
                            if(relFeature!=null)
                                propertyForRelation.RelatedPropertyInfo.SetValue(featureForLinksApplied, relFeature);
                            continue;
                        }
                        var featStorageByPropertyType = FeatureCollections[propertyForRelation.RelatedPropertyInfo.PropertyType];
                        AM_AbstractFeature relatedFeature = null;
                        foreach (var feat in featStorageByPropertyType)
                        {
                            if (((AM_AbstractFeature)feat).featureID.Equals(propertyForRelation.RelatedFeatIdList.First()))
                            {
                                relatedFeature = (AM_AbstractFeature)feat;
                                break;
                            }
                        }
                        if (relatedFeature != null)
                            propertyForRelation.RelatedPropertyInfo.SetValue(featureForLinksApplied, relatedFeature);
                      
                    }
                    else
                    {
                        //Здесь сначала нужно обработать случай когда List<AbstractFeature>

                        
                        var instance = Activator.CreateInstance(propertyForRelation.RelatedPropertyInfo.PropertyType);
                        List<AM_AbstractFeature> relatedFeatures = new List<AM_AbstractFeature>();
                        var featStorageByPropertyType = FeatureCollections[propertyForRelation.RelatedPropertyInfo.PropertyType.GetGenericArguments().First()];
                        foreach (var relatedFeatId in propertyForRelation.RelatedFeatIdList)
                        {
                                                   
                            AM_AbstractFeature relatedFeature = null;
                            foreach (var item in featStorageByPropertyType)
                            {
                                if (((AM_AbstractFeature)item).featureID.Equals(relatedFeatId))
                                {
                                    relatedFeature = (AM_AbstractFeature)item;
                                    if (relatedFeature is null) continue;
                                    relatedFeatures.Add(relatedFeature);
                                    ((IList)instance).Add(relatedFeature);
                                }

                            }

                        }
                        propertyForRelation.RelatedPropertyInfo.SetValue(featureForLinksApplied, instance);
                    }

                }
            }

            //LinkObjects();

            //Taxiway
            FeatureCollections[typeof(AM_Taxiway)].BeforeAdd += new EventHandler<EntityChangingEventArgs<AM_Taxiway>>((s, e) =>
            {
                if (!e.Entity.Validate())
                    throw new ValidationException(e.Entity.ValidationMessage);
            });
            FeatureCollections[typeof(AM_Taxiway)].BeforeRemove += new EventHandler<EntityChangingEventArgs<AM_Taxiway>>((s, e) =>
            {
                if (((CompositeCollection<AM_TaxiwayElement>)FeatureCollections[typeof(AM_TaxiwayElement)]).Any(it =>it.AssociatedTaxiway == e.Entity))
                    throw new ValidationException(
                        "The Taxiway cannot be deleted because it is used by TaxiwayElements.");
            });
            //Taxiways.BeforeAdd += new EventHandler<EntityChangingEventArgs<AM_Taxiway>>((s, e) =>
            //{
            //    if (!e.Entity.Validate())
            //        throw new ValidationException(e.Entity.ValidationMessage);
            //});
            //Taxiways.BeforeRemove += new EventHandler<EntityChangingEventArgs<AM_Taxiway>>((s, e) =>
            //{
            //    if (TaxiwayElements.Any(it => it.AssociatedTaxiway == e.Entity))
            //        throw new ValidationException(
            //            "The Taxiway cannot be deleted because it is used by TaxiwayElements.");
            //});

        }
    }
}
