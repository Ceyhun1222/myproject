//using Aerodrome.Context;

using Aerodrome.Features;
//using Aerodrome.Features.Context;
using BusinessCore.Validation;
using BusinessCore.Validation.Exceptions;
using Framework.Stasy.Context;
using Framework.Stasy.SyncProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfEnvelope.Crud;
using WpfEnvelope.Crud.Controls;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MasterData.xaml
    /// </summary>
    public partial class MasterData : Page
    {
        public MasterData(ApplicationContext context)
        {
            
            _context = context;

            InitializeComponent();

            #region CRUD Display Configuration

            crudDisplay.UnhandledRuntimeException
                += new UnhandledExceptionEventHandler(crudDisplay_UnhandledRuntimeException);

            // arp
            var adhp = crudDisplay.CrudManager
                .RegisterType<AM_AerodromeReferencePoint>(() => context.FeatureCollections[typeof(AM_AerodromeReferencePoint)])             
                .DefineEditOperation(
                    (c) => c,
                    (c) => CancelOrCommitEdit(c),
                    (c) => CancelOrCommitEdit(c));

            #region TaxiwayFeatureTypes

            //Taxiway
            var twy = crudDisplay.CrudManager
                .RegisterType<AM_Taxiway>(() => context.FeatureCollections[typeof(AM_Taxiway)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_Taxiway)].Add(m),
                    () => context.CreateInstance(() => new AM_Taxiway()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_Taxiway)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //TaxiwayElement
            var twyElem = crudDisplay.CrudManager
                .RegisterType<AM_TaxiwayElement>(() => context.FeatureCollections[typeof(AM_TaxiwayElement)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayElement)].Add(m),
                    () => context.CreateInstance(() => new AM_TaxiwayElement()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayElement)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //BridgeSide
            var bridgeSide = crudDisplay.CrudManager
                .RegisterType<AM_BridgeSide>(() => context.FeatureCollections[typeof(AM_BridgeSide)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_BridgeSide)].Add(m),
                    () => context.CreateInstance(() => new AM_BridgeSide()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_BridgeSide)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //FrequencyArea
            var freqArea = crudDisplay.CrudManager
                .RegisterType<AM_FrequencyArea>(() => context.FeatureCollections[typeof(AM_FrequencyArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_FrequencyArea)].Add(m),
                    () => context.CreateInstance(() => new AM_FrequencyArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_FrequencyArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //PositionMarking
            var positionMarking = crudDisplay.CrudManager
                .RegisterType<AM_PositionMarking>(() => context.FeatureCollections[typeof(AM_PositionMarking)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_PositionMarking)].Add(m),
                    () => context.CreateInstance(() => new AM_PositionMarking()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_PositionMarking)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //TaxiwayHoldingPosition
            var taxiwayHoldingPosition = crudDisplay.CrudManager
                .RegisterType<AM_TaxiwayHoldingPosition>(() => context.FeatureCollections[typeof(AM_TaxiwayHoldingPosition)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayHoldingPosition)].Add(m),
                    () => context.CreateInstance(() => new AM_TaxiwayHoldingPosition()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayHoldingPosition)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //TaxiwayIntersectionMarking
            var taxiwayIntersectionMarking = crudDisplay.CrudManager
                .RegisterType<AM_TaxiwayIntersectionMarking>(() => context.FeatureCollections[typeof(AM_TaxiwayIntersectionMarking)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayIntersectionMarking)].Add(m),
                    () => context.CreateInstance(() => new AM_TaxiwayIntersectionMarking()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayIntersectionMarking)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //TaxiwayGuidanceLine
            var taxiwayGuidanceLine = crudDisplay.CrudManager
                .RegisterType<AM_TaxiwayGuidanceLine>(() => context.FeatureCollections[typeof(AM_TaxiwayGuidanceLine)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayGuidanceLine)].Add(m),
                    () => context.CreateInstance(() => new AM_TaxiwayGuidanceLine()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_TaxiwayGuidanceLine)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //TaxiwayShoulder
            var taxiwayShoulder = crudDisplay.CrudManager
                .RegisterType<AM_TaxiwayShoulder>(() => context.FeatureCollections[typeof(AM_TaxiwayShoulder)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayShoulder)].Add(m),
                    () => context.CreateInstance(() => new AM_TaxiwayShoulder()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_TaxiwayShoulder)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region RunwayFeatureTypes

            //Runway
            var rwy = crudDisplay.CrudManager
                .RegisterType<AM_Runway>(() => context.FeatureCollections[typeof(AM_Runway)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_Runway)].Add(m),
                    () => context.CreateInstance(() => new AM_Runway()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_Runway)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayElement
            var rwyElem = crudDisplay.CrudManager
                .RegisterType<AM_RunwayElement>(() => context.FeatureCollections[typeof(AM_RunwayElement)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayElement)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayElement()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_RunwayElement)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //LandAndHoldShortOperationLocation
            var lahso = crudDisplay.CrudManager
                .RegisterType<AM_LandAndHoldShortOperationLocation>(() => context.FeatureCollections[typeof(AM_LandAndHoldShortOperationLocation)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_LandAndHoldShortOperationLocation)].Add(m),
                    () => context.CreateInstance(() => new AM_LandAndHoldShortOperationLocation()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_LandAndHoldShortOperationLocation)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayDirection
            var rwyDir = crudDisplay.CrudManager
                .RegisterType<AM_RunwayDirection>(() => context.FeatureCollections[typeof(AM_RunwayDirection)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayDirection)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayDirection()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_RunwayDirection)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //Blastpad
            var blastpad = crudDisplay.CrudManager
                .RegisterType<AM_Blastpad>(() => context.FeatureCollections[typeof(AM_Blastpad)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_Blastpad)].Add(m),
                    () => context.CreateInstance(() => new AM_Blastpad()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_Blastpad)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //ArrestingSystemLocation
            var arrestingSystemLocation = crudDisplay.CrudManager
                .RegisterType<AM_ArrestingSystemLocation>(() => context.FeatureCollections[typeof(AM_ArrestingSystemLocation)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ArrestingSystemLocation)].Add(m),
                    () => context.CreateInstance(() => new AM_ArrestingSystemLocation()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ArrestingSystemLocation)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //ArrestingGearLocation
            var arrestingGearLocation = crudDisplay.CrudManager
                .RegisterType<AM_ArrestingGearLocation>(() => context.FeatureCollections[typeof(AM_ArrestingGearLocation)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ArrestingGearLocation)].Add(m),
                    () => context.CreateInstance(() => new AM_ArrestingGearLocation()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ArrestingGearLocation)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //PaintedCenterline
            var paintedCenterline = crudDisplay.CrudManager
                .RegisterType<AM_PaintedCenterline>(() => context.FeatureCollections[typeof(AM_PaintedCenterline)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_PaintedCenterline)].Add(m),
                    () => context.CreateInstance(() => new AM_PaintedCenterline()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_PaintedCenterline)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayCenterlinePoint
            var runwayCenterlinePoint = crudDisplay.CrudManager
                .RegisterType<AM_RunwayCenterlinePoint>(() => context.FeatureCollections[typeof(AM_RunwayCenterlinePoint)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayCenterlinePoint)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayCenterlinePoint()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayCenterlinePoint)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayDisplacedArea
            var runwayDisplacedArea = crudDisplay.CrudManager
                .RegisterType<AM_RunwayDisplacedArea>(() => context.FeatureCollections[typeof(AM_RunwayDisplacedArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayDisplacedArea)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayDisplacedArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayDisplacedArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayExitLine
            var runwayExitLine = crudDisplay.CrudManager
                .RegisterType<AM_RunwayExitLine>(() => context.FeatureCollections[typeof(AM_RunwayExitLine)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayExitLine)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayExitLine()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_RunwayExitLine)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayIntersection
            var runwayIntersection = crudDisplay.CrudManager
                .RegisterType<AM_RunwayIntersection>(() => context.FeatureCollections[typeof(AM_RunwayIntersection)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayIntersection)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayIntersection()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayIntersection)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayThreshold
            var runwayThreshold = crudDisplay.CrudManager
                .RegisterType<AM_RunwayThreshold>(() => context.FeatureCollections[typeof(AM_RunwayThreshold)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayThreshold)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayThreshold()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayThreshold)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayMarking
            var runwayMarking = crudDisplay.CrudManager
                .RegisterType<AM_RunwayMarking>(() => context.FeatureCollections[typeof(AM_RunwayMarking)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayMarking)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayMarking()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayMarking)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //RunwayShoulder
            var runwayShoulder = crudDisplay.CrudManager
                .RegisterType<AM_RunwayShoulder>(() => context.FeatureCollections[typeof(AM_RunwayShoulder)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayShoulder)].Add(m),
                    () => context.CreateInstance(() => new AM_RunwayShoulder()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_RunwayShoulder)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //Stopway
            var stopway = crudDisplay.CrudManager
                .RegisterType<AM_Stopway>(() => context.FeatureCollections[typeof(AM_Stopway)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_Stopway)].Add(m),
                    () => context.CreateInstance(() => new AM_Stopway()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_Stopway)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region ApronFeatureTypes

            // Apron
            var apron = crudDisplay.CrudManager
                .RegisterType<AM_Apron>(() => context.FeatureCollections[typeof(AM_Apron)])
                .DefineAddOperation(
                    (c) => context.FeatureCollections[typeof(AM_Apron)].Add(c),
                    () => context.CreateInstance(() => new AM_Apron()),
                    (c) => { })
                .DefineRemoveOperation(
                    (c) => context.FeatureCollections[typeof(AM_Apron)].Remove(c))
                .DefineEditOperation(
                    (c) => c,
                    (c) => CancelOrCommitEdit(c),
                    (c) => CancelOrCommitEdit(c));

            // ApronElements
            var apronElem = crudDisplay.CrudManager
                .RegisterType<AM_ApronElement>(() => context.FeatureCollections[typeof(AM_ApronElement)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ApronElement)].Add(m),
                    () => context.CreateInstance(() => new AM_ApronElement()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ApronElement)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));


            //DeicingArea
            var deicingArea = crudDisplay.CrudManager
                .RegisterType<AM_DeicingArea>(() => context.FeatureCollections[typeof(AM_DeicingArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_DeicingArea)].Add(m),
                    () => context.CreateInstance(() => new AM_DeicingArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_DeicingArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //DeicingGroup
            var deicingGroup = crudDisplay.CrudManager
                .RegisterType<AM_DeicingGroup>(() => context.FeatureCollections[typeof(AM_DeicingGroup)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_DeicingGroup)].Add(m),
                    () => context.CreateInstance(() => new AM_DeicingGroup()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) =>context.FeatureCollections[typeof(AM_DeicingGroup)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //ParkingStandLocation
            var parkingStandLocation = crudDisplay.CrudManager
                .RegisterType<AM_ParkingStandLocation>(() => context.FeatureCollections[typeof(AM_ParkingStandLocation)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ParkingStandLocation)].Add(m),
                    () => context.CreateInstance(() => new AM_ParkingStandLocation()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ParkingStandLocation)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //ParkingStandArea
            var parkingStandArea = crudDisplay.CrudManager
                .RegisterType<AM_ParkingStandArea>(() => context.FeatureCollections[typeof(AM_ParkingStandArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ParkingStandArea)].Add(m),
                    () => context.CreateInstance(() => new AM_ParkingStandArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ParkingStandArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            
            //StandGuidanceLine
            var standGuidanceLine = crudDisplay.CrudManager
                .RegisterType<AM_StandGuidanceLine>(() => context.FeatureCollections[typeof(AM_StandGuidanceLine)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_StandGuidanceLine)].Add(m),
                    () => context.CreateInstance(() => new AM_StandGuidanceLine()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_StandGuidanceLine)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //ServiceRoad
            var serviceRoad = crudDisplay.CrudManager
                .RegisterType<AM_ServiceRoad>(() => context.FeatureCollections[typeof(AM_ServiceRoad)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ServiceRoad)].Add(m),
                    () => context.CreateInstance(() => new AM_ServiceRoad()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ServiceRoad)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region HelipadFeatureTypes

            //FinalApproachAndTakeOffArea
            var finalApproachAndTakeOffArea = crudDisplay.CrudManager
                .RegisterType<AM_FinalApproachAndTakeOffArea>(() => context.FeatureCollections[typeof(AM_FinalApproachAndTakeOffArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_FinalApproachAndTakeOffArea)].Add(m),
                    () => context.CreateInstance(() => new AM_FinalApproachAndTakeOffArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_FinalApproachAndTakeOffArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //HelipadThreshold
            var helipadThreshold = crudDisplay.CrudManager
                .RegisterType<AM_HelipadThreshold>(() => context.FeatureCollections[typeof(AM_HelipadThreshold)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_HelipadThreshold)].Add(m),
                    () => context.CreateInstance(() => new AM_HelipadThreshold()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_HelipadThreshold)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //TouchDownLiftOffArea
            var touchDownLiftOffArea = crudDisplay.CrudManager
                .RegisterType<AM_TouchDownLiftOffArea>(() => context.FeatureCollections[typeof(AM_TouchDownLiftOffArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_TouchDownLiftOffArea)].Add(m),
                    () => context.CreateInstance(() => new AM_TouchDownLiftOffArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_TouchDownLiftOffArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region ConstructionAreaFeatureTypes

            //ConstructionArea
            var constructionArea = crudDisplay.CrudManager
                .RegisterType<AM_ConstructionArea>(() => context.FeatureCollections[typeof(AM_ConstructionArea)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ConstructionArea)].Add(m),
                    () => context.CreateInstance(() => new AM_ConstructionArea()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ConstructionArea)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region HotspotFeatureTypes

            //AM_ATCBlindSpot
            var aTCBlindSpot = crudDisplay.CrudManager
                .RegisterType<AM_ATCBlindSpot>(() => context.FeatureCollections[typeof(AM_ATCBlindSpot)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_ATCBlindSpot)].Add(m),
                    () => context.CreateInstance(() => new AM_ATCBlindSpot()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_ATCBlindSpot)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //Hotspot
            var hotspot = crudDisplay.CrudManager
                .RegisterType<AM_Hotspot>(() => context.FeatureCollections[typeof(AM_Hotspot)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_Hotspot)].Add(m),
                    () => context.CreateInstance(() => new AM_Hotspot()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_Hotspot)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region SurveyControlPointFeatureTypes

            //SurveyControlPoint
            var surveyControlPoint = crudDisplay.CrudManager
                .RegisterType<AM_SurveyControlPoint>(() => context.FeatureCollections[typeof(AM_SurveyControlPoint)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_SurveyControlPoint)].Add(m),
                    () => context.CreateInstance(() => new AM_SurveyControlPoint()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_SurveyControlPoint)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region AerodromeSurfaceLightingFeatureTypes

            //AerodromeSurfaceLighting
            var aerodromeSurfaceLighting = crudDisplay.CrudManager
                .RegisterType<AM_AerodromeSurfaceLighting>(() => context.FeatureCollections[typeof(AM_AerodromeSurfaceLighting)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_AerodromeSurfaceLighting)].Add(m),
                    () => context.CreateInstance(() => new AM_AerodromeSurfaceLighting()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_AerodromeSurfaceLighting)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region VerticalStructureFeatureTypes

            //VerticalPolygonalStructure
            var verticalStructurePolygonal = crudDisplay.CrudManager
                .RegisterType<AM_VerticalPolygonalStructure>(() => context.FeatureCollections[typeof(AM_VerticalPolygonalStructure)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_VerticalPolygonalStructure)].Add(m),
                    () => context.CreateInstance(() => new AM_VerticalPolygonalStructure()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_VerticalPolygonalStructure)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //VerticalPointStructure
            var verticalPointStructure = crudDisplay.CrudManager
                .RegisterType<AM_VerticalPointStructure>(() => context.FeatureCollections[typeof(AM_VerticalPointStructure)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_VerticalPointStructure)].Add(m),
                    () => context.CreateInstance(() => new AM_VerticalPointStructure()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_VerticalPointStructure)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            //VerticalLineStructure
            var verticalLineStructure = crudDisplay.CrudManager
                .RegisterType<AM_VerticalLineStructure>(() => context.FeatureCollections[typeof(AM_VerticalLineStructure)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_VerticalLineStructure)].Add(m),
                    () => context.CreateInstance(() => new AM_VerticalLineStructure()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_VerticalLineStructure)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region SignageFeatureTypes

            //AerodromeSign
            var aerodromeSign = crudDisplay.CrudManager
                .RegisterType<AM_AerodromeSign>(() => context.FeatureCollections[typeof(AM_AerodromeSign)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_AerodromeSign)].Add(m),
                    () => context.CreateInstance(() => new AM_AerodromeSign()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_AerodromeSign)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region WaterFeatureTypes

            //Water
            var water = crudDisplay.CrudManager
                .RegisterType<AM_Water>(() => context.FeatureCollections[typeof(AM_Water)])
                .DefineAddOperation(
                    (m) => context.FeatureCollections[typeof(AM_Water)].Add(m),
                    () => context.CreateInstance(() => new AM_Water()),
                    (m) => { })
                .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_Water)].Remove(m))
                .DefineEditOperation(
                    (m) => m,
                    (m) => CancelOrCommitEdit(m),
                    (m) => CancelOrCommitEdit(m));

            #endregion

            #region RoutingFeatureTypes

            var edge = crudDisplay.CrudManager
            .RegisterType<AM_AsrnEdge>(() => context.FeatureCollections[typeof(AM_AsrnEdge)])
            .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_AsrnEdge)].Remove(m));

            var node = crudDisplay.CrudManager
               .RegisterType<AM_AsrnNode>(() => context.FeatureCollections[typeof(AM_AsrnNode)])
            .DefineRemoveOperation(
                    (m) => context.FeatureCollections[typeof(AM_AsrnNode)].Remove(m));
            #endregion

            #region old crud
            //// Apron
            //var adhp = crudDisplay.CrudManager
            //    .RegisterType<AM_AerodromeReferencePoint>(() => context.Airports)

            //    .DefineEditOperation(
            //        (c) => c,
            //        (c) => CancelOrCommitEdit(c),
            //        (c) => CancelOrCommitEdit(c));

            //// Apron
            //var apron = crudDisplay.CrudManager
            //    .RegisterType<AM_Apron>(() => context.Aprons)
            //    .DefineAddOperation(
            //        (c) => context.Aprons.Add(c),
            //        () => context.CreateInstance(() => new AM_Apron()),
            //        (c) => { })
            //    .DefineRemoveOperation(
            //        (c) => Delete(
            //            "Really delete Apron?", () => context.Aprons.Remove(c)))
            //    .DefineEditOperation(
            //        (c) => c,
            //        (c) => CancelOrCommitEdit(c),
            //        (c) => CancelOrCommitEdit(c));

            //// ApronElements
            //var apronElem = crudDisplay.CrudManager
            //    .RegisterType<AM_ApronElement>(() => context.ApronElements)
            //    .DefineAddOperation(
            //        (m) => context.ApronElements.Add(m),
            //        () => context.CreateInstance(() => new AM_ApronElement()),
            //        (m) => { })
            //    .DefineRemoveOperation(
            //        (m) => Delete(
            //            "Really delete ApronElement?", () => context.ApronElements.Remove(m)))
            //    .DefineEditOperation(
            //        (m) => m,
            //        (m) => CancelOrCommitEdit(m),
            //        (m) => CancelOrCommitEdit(m));


            ////Taxiway
            //var twy = crudDisplay.CrudManager
            //    .RegisterType<AM_Taxiway>(() => context.Taxiways)
            //    .DefineAddOperation(
            //        (m) => context.Taxiways.Add(m),
            //        () => context.CreateInstance(() => new AM_Taxiway()),
            //        (m) => { })
            //    .DefineRemoveOperation(
            //        (m) => Delete(
            //            "Really delete Taxiway?", () => context.Taxiways.Remove(m)))
            //    .DefineEditOperation(
            //        (m) => m,
            //        (m) => CancelOrCommitEdit(m),
            //        (m) => CancelOrCommitEdit(m));

            ////TaxiwayElement
            //var twyElem = crudDisplay.CrudManager
            //    .RegisterType<AM_TaxiwayElement>(() => context.TaxiwayElements)
            //    .DefineAddOperation(
            //        (m) => context.TaxiwayElements.Add(m),
            //        () => context.CreateInstance(() => new AM_TaxiwayElement()),
            //        (m) => { })
            //    .DefineRemoveOperation(
            //        (m) => Delete(
            //            "Really delete TaxiwayElement?", () => context.TaxiwayElements.Remove(m)))
            //    .DefineEditOperation(
            //        (m) => m,
            //        (m) => CancelOrCommitEdit(m),
            //        (m) => CancelOrCommitEdit(m));
            #endregion

            #endregion
        }

        private readonly ApplicationContext _context;

        private void CancelOrCommitEdit(IValidatable bo)
        {
            if (!bo.Validate())
                throw new ValidationException(bo.ValidationMessage);
        }

        private void Delete(string question, Action del)
        {
            var res = MessageBox.Show(question, "Question", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
                del();
        }

        private void crudDisplay_UnhandledRuntimeException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is ValidationException)
            {
                var valEx = e.ExceptionObject as ValidationException;
                MessageBox.Show(
                    valEx.Message,
                    "The data is not valid",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else if (e.ExceptionObject is Exception)
            {
                Exception ex = e.ExceptionObject as Exception;

                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(
                    "An unknown error occured.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
      
    }
}
